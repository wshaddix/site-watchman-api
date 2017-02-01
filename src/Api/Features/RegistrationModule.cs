using Nancy;

namespace SiteWatchman.Api.Features
{
    public class RegistrationModule : NancyModule
    {
        public RegistrationModule() : base("/registrations")
        {
            Get["/"] = parameters => HandleGet(parameters);
            Get["/{id}"] = parameters => HandleList(parameters);
            Post["/"] = parameters => HandlePost(parameters);
            Patch["/{id}"] = parameters => HandlePatch(parameters);
            Delete["/{id}"] = parameters => HandleDelete(parameters);
        }

        private object HandleDelete(DynamicDictionary parameters)
        {
            throw new System.NotImplementedException();
        }

        private object HandleGet(DynamicDictionary parameters)
        {
            throw new System.NotImplementedException();
        }

        private object HandleList(DynamicDictionary parameters)
        {
            throw new System.NotImplementedException();
        }

        private object HandlePatch(DynamicDictionary parameters)
        {
            throw new System.NotImplementedException();
        }

        private object HandlePost(DynamicDictionary parameters)
        {
            throw new System.NotImplementedException();
        }
    }
}