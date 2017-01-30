using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using LinqKit;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SiteWatchman.Common.Application.Queries;
using SiteWatchman.Common.Domain;
using SiteWatchman.Common.Persistance;

namespace SiteWatchman.Persistance
{
    public sealed class DocumentDbDatabase : IDatabase
    {
        private readonly string _collectionId;
        private readonly Uri _collectionUri;
        private readonly string _databaseId;
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        private DocumentClient _documentClient;

        public DocumentDbDatabase(string serviceEndpoint, string authKey, string databaseId, string collectionId)
        {
            _databaseId = databaseId;
            _collectionId = collectionId;
            if (null == serviceEndpoint)
            {
                throw new ConfigurationErrorsException($"{nameof(serviceEndpoint)} cannot be null");
            }

            if (string.IsNullOrWhiteSpace(authKey))
            {
                throw new ConfigurationErrorsException($"{nameof(authKey)} cannot be null or whitespace");
            }

            if (string.IsNullOrWhiteSpace(databaseId))
            {
                throw new ConfigurationErrorsException($"{nameof(databaseId)} cannot be null or whitespace");
            }

            if (string.IsNullOrWhiteSpace(collectionId))
            {
                throw new ConfigurationErrorsException($"{nameof(collectionId)} cannot be null or whitespace");
            }

            _collectionUri = UriFactory.CreateDocumentCollectionUri(databaseId, collectionId);
            Initialize(new Uri(serviceEndpoint), authKey);
            _jsonSerializerSettings = new JsonSerializerSettings { ContractResolver = new InternalPropertyContractResolver() };
        }

        async Task IDatabase.DeleteAsync<T>(string id)
        {
            // build the document's self link for the fastest query
            var selflink = UriFactory.CreateDocumentUri(_databaseId, _collectionId, id);
            await _documentClient.DeleteDocumentAsync(selflink).ConfigureAwait(false);
        }

        public T Get<T>(Expression<Func<T, bool>> query) where T : Entity
        {
            // we want to inject the entity type into the predicate so we ensure to only return records of the correct type
            query = InjectEntityTypeFilter(query);

            // build the query using the generic typed predicate
            var updatedQuery = _documentClient.CreateDocumentQuery<T>(_collectionUri)
                .Where(query)
                .ToString();

            // if any of the predicate conditions is by Id, we need to change the case to "id" so documentdb will work
            if (updatedQuery.Contains("root[\\\"Id\\\"]"))
            {
                updatedQuery = updatedQuery.Replace("root[\\\"Id\\\"]", "root[\\\"id\\\"]");
            }

            // convert the query to a dynamic so that we can easily pull just the query text
            dynamic jsonQuery = JsonConvert.DeserializeObject<dynamic>(updatedQuery);

            // execute the query without using generics so that we can just get a Document back
            var jObject = _documentClient.CreateDocumentQuery<JObject>(_collectionUri, (string)jsonQuery.query)
                .AsEnumerable()
                .FirstOrDefault();

            return null == jObject ? default(T) : JsonConvert.DeserializeObject<T>(jObject.ToString(), _jsonSerializerSettings);
        }

        async Task<T> IDatabase.GetByIdAsync<T>(string id)
        {
            // build the document's self link for the fastest query
            var selflink = UriFactory.CreateDocumentUri(_databaseId, _collectionId, id);

            var document = await _documentClient.ReadDocumentAsync(selflink).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T>(document.Resource.ToString(), _jsonSerializerSettings);
        }

        public EntityListQueryResult<T> List<T>(Expression<Func<T, bool>> where, string sortBy, int page, int pageSize) where T : Entity
        {
            // we want to inject the entity type into the predicate so we ensure to only return records of the correct type
            where = InjectEntityTypeFilter(where);

            // create a query for T that includes the sorting and filtering
            var updatedQuery = _documentClient.CreateDocumentQuery<T>(_collectionUri)
                .Where(where);

            // split the sort string
            var listSort = sortBy.Split(',');

            // loop through the sorting options and create a sort expression string from them
            var firstIteration = true;
            foreach (var sortOption in listSort)
            {
                // NOTE: DocumentDb only supports sorting by one property currently
                // TODO: Once DocumentDb supports sorting by multiple properties we can remove this restriction
                if (!firstIteration) break;

                string propertyName;
                string methodName;

                // if the sort option starts with "-" we order descending, otherwise ascending
                if (sortOption.StartsWith("-", StringComparison.Ordinal))
                {
                    propertyName = sortOption.Remove(0, 1).ToPascalCase();
                    methodName = "OrderByDescending";
                }
                else
                {
                    propertyName = sortOption.ToPascalCase();
                    methodName = "OrderBy";
                }

                firstIteration = false;

                var type = typeof(T);
                var property = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic);
                var parameter = Expression.Parameter(type, type.Name);
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                var typeArguments = new[] { type, property.PropertyType };
                var resultExp = Expression.Call(typeof(Queryable), methodName, typeArguments, updatedQuery.Expression, Expression.Quote(orderByExp));

                updatedQuery = updatedQuery.Provider.CreateQuery<T>(resultExp);
            }

            // if any of the predicate conditions is by Id, we need to change the case to "id" so documentdb will work
            var stringQuery = updatedQuery.ToString();
            if (stringQuery.Contains("root[\\\"Id\\\"]"))
            {
                stringQuery = stringQuery.Replace("root[\\\"Id\\\"]", "root[\\\"id\\\"]");
            }

            // we need to return TotalCount and TotalPages for all queries
            int totalCount;
            List<T> dataList;

            // if page is > 1 then we need to perform two queries. The first to get just the Id's of the documents that match the query and the second
            // to pull the actual documents that match the page/pageSize parameters
            if (page > 0)
            {
                var options = new FeedOptions
                {
                    MaxItemCount = int.MaxValue
                };

                // we want to change the query to return only the value of the id property, not the property itself (so we get an IEnumerable<string>
                // of ids back instead of an IEnumerable<dynamic> that has an id property and a value)
                var idStringQuery = stringQuery.Replace("SELECT * FROM", "SELECT VALUE root.id FROM");

                // convert the query to a dynamic so that we can easily pull just the query text
                dynamic idQuery = JsonConvert.DeserializeObject<dynamic>(idStringQuery);

                // get a list of ids that match the query
                var ids =
                    _documentClient.CreateDocumentQuery<string>(_collectionUri, (string)idQuery.query, options)
                        .ToList();

                // capture the count of how many documents match the WHERE clause
                totalCount = ids.Count;

                // if we are asking for more documents than exist, make sure we don't skip them all
                var skip = (page - 1) * pageSize;

                // update the original query to just pull all data where the ids are in the id list WE HAVE TO MAINTAIN THE ORIGINAL SORT BY THAT THE
                // idQuery USES!!!
                var startIndex = idStringQuery.IndexOf("ORDER BY", StringComparison.Ordinal);
                var orderBy = idStringQuery.Substring(startIndex, idStringQuery.Length - startIndex - 3);

                stringQuery =
                    $"SELECT * FROM root WHERE root.id IN ('{string.Join("','", ids.Skip(skip).Take(pageSize))}')" + " " + orderBy.Replace("\\\"", "'");

                var jObjects = _documentClient.CreateDocumentQuery<JObject>(_collectionUri, stringQuery).ToList();

                dataList = jObjects.Select(jObject => JsonConvert.DeserializeObject<T>(jObject.ToString(), _jsonSerializerSettings)).ToList();
            }
            else
            {
                // convert the query to a dynamic so that we can easily pull just the query text
                dynamic jsonQuery = JsonConvert.DeserializeObject<dynamic>(stringQuery);
                stringQuery = jsonQuery.query;

                var jObjects = _documentClient.CreateDocumentQuery<JObject>(_collectionUri, stringQuery).ToList();

                dataList = jObjects.Select(jObject => JsonConvert.DeserializeObject<T>(jObject.ToString(), _jsonSerializerSettings)).ToList();

                // capture the count of how many documents match the WHERE clause
                totalCount = dataList.Count;
            }

            // calculate the number of total pages
            var actualPages = totalCount / (double) pageSize;
            var totalPages = (int)Math.Ceiling(actualPages);

            return new EntityListQueryResult<T>
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                DataList = dataList
            };
        }

        public async Task SaveAsync(object data)
        {
            // we're saving internal classes (business entities) so we need to include the internally scoped properties when we convert the object to a JObject
            var serializer = new JsonSerializer { ContractResolver = new InternalPropertyContractResolver() };

            // we need to change the property named "Id" to "id" so that it works with Azure Document Db
            var jObject = JObject.FromObject(data, serializer);
            jObject["Id"].Rename("id");

            await _documentClient.UpsertDocumentAsync(_collectionUri, jObject).ConfigureAwait(false);
        }

        private static Expression<Func<T, bool>> InjectEntityTypeFilter<T>(Expression<Func<T, bool>> predicate) where T : Entity
        {
            // we want to get the type of T so that we can filter the documents returned to be just those types
            var entityType = typeof(T).Name;

            // we want to "inject" the condition that the entity type matches typeof(T) into the predicate
            Expression<Func<T, bool>> newPredicate = t => predicate.Invoke(t) && (t.EntityType == entityType);
            return newPredicate.Expand();
        }

        private async Task<T> GetAsync<T>(string id)
        {
            // build the document's self link for the fastest query
            var selflink = UriFactory.CreateDocumentUri(_databaseId, _collectionId, id);

            var document = await _documentClient.ReadDocumentAsync(selflink).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T>(document.Resource.ToString(), _jsonSerializerSettings);
        }

        private void Initialize(Uri serviceEndpoint, string authKey)
        {
#if DEBUG
            _documentClient = new DocumentClient(serviceEndpoint, authKey,
                new ConnectionPolicy { EnableEndpointDiscovery = false });
#else
            _documentClient = new DocumentClient(serviceEndpoint, authKey);
#endif
        }

        private sealed class InternalPropertyContractResolver : DefaultContractResolver
        {
            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            {
                var jsonProperties = new List<JsonProperty>();
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                foreach (var property in properties)
                {
                    var jsonProperty = CreateProperty(property, memberSerialization);
                    jsonProperty.Writable = HasSetMethod(property);
                    jsonProperty.Readable = true;
                    jsonProperties.Add(jsonProperty);
                }

                return jsonProperties;
            }

            private static bool HasSetMethod(PropertyInfo propertyInfo)
            {
                return propertyInfo.GetSetMethod(true) != null;
            }
        }
    }
}