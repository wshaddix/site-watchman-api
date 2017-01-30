# Getting Started for Developers

### Software Installation Requirements  
- [Git](https://git-scm.com/)  
- [Visual Studio Professional](https://www.visualstudio.com/downloads/)  
- [DocumentDB Emulator](https://docs.microsoft.com/en-us/azure/documentdb/documentdb-nosql-local-emulator)  
- [DocumentDB Studio](https://github.com/mingaliu/DocumentDBStudio)  
- [Postman](https://www.getpostman.com/)  

### 1. Clone the project  
`https://github.com/wshaddix/site-watchman-api.git`

### 2. Start DocumentDB Local Emulator  
`Start => DocumentDb Emulator`

### 3. Create the SiteWatchman database  
From the [DocumentDB Studio](https://github.com/mingaliu/DocumentDBStudio) create a new database named `SiteWatchman` with the following properties  
- **Database Id:** SiteWatchman 
- **Collection Id:** default  
- **Throughput:** 10,000  
- **Indexing Policy:** `{"Kind": "Range", "dataType": "String", "precision": -1}`  

### 4. Open the solution in Visual Studio  
`Start => Visual Studio 2017`

### 5. Ensure that the Api project's Web.config is setup to point to the local DocumentDB Emulator  
`<add key="DocumentDb.Uri" value="https://localhost:8081" />`

### 6. Create AppSecrets.config  
If you don't already have it create a file `\src\Api\AppSecrets.config`

### 7. Ensure that AppSecrets.config is setup to authenticate to the local DocumentDB Emulator and contains user credentials and api keys to initialize the platform  
```
<configuration>
	<appSettings>
		<add key="DocumentDb.AuthKey" value="C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==" />
                <add key="Postman.ApiKey" value="<insert your api key here>" />
		<add key="PlatformAdmin.Username" value="<insert your username here>" />
		<add key="PlatformAdmin.Password" value="<insert your password here>" />
	</appSettings>
</configuration>
```

### 8. Restore NuGet Packages in Visual Studio  
`Right click the solution => Restore NuGet Packages`

### 9. Run the SynapseMX Platform Code  
`CTRL+F5`

### 10. Open Postman  
`Start => Postman`

### 11. Import the SynapseMX Api Postman Collection  
`https://www.getpostman.com/collections/`

### 12. Import the SiteWatchman.Workstation Environment  
This is a postman environment file. This file contains all of the urls and header values needed for the api requests.

### 13. Open the Postman Runner & execute the SiteWatchman Api collection against the `SiteWatchman.Worksation` environment  
Ensure that all tests pass. If they don't then create a github issue for help