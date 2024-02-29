# BandLabAssesment

## Getting the app running
The application comprises of the following: <br/>
  3 HTTP Trigger functions <br />
  1 Cosmos Trigger function <br />

To get the app up and running the following needs to be configured/provisioned:
### Infrastructure

1. Azure Blob Storage account
2. Azure Blob Storage container
3. Cosmos Database: A cosmos db database with two containers: <br/>
   `posts` container with `creatorId` as the partitionKey <br/>
   `comments` container with `creatorId` as the partitionKey, and ttl enabled without a default duration. 

### Environment Variables
The following configuration values need to be setup either in your local.settings.json file or as environment variables <br />
`FUNCTIONS_REQUEST_BODY_SIZE_LIMIT` : This needs to be set to above 100Mb to override the default request size limit on Azure function http triggers. e.g 110100480 <br />
`BlobStorage__ConnectionString`: Connection string to the azure storage account either provisoned in Azure or running on your local environment via the storage emulator <br />
`BlobStorage__Container`: Name of the container created within the storage account <br />
`CosmosDb__DatabaseName`: Cosmos database name <br />
`CosmosDb__ConnectionString`: Connection string to the cosmos database.

## Running the functional tests in BandlabAssesments.Tests
The following are required for the functional tests 
1. Storage account Connection String for tests
2. A separate database for functional tests with TTL enabled on both `posts` and `comments` containers with whatever default is desired ( for cleanup of documents).
3. The necessary configurations are in the test.settings.json file and should be replaced with the appropriate values or can be setup as environment variables in the executing environment

The functions are grouped within one 1 function app for the sake of this assesment and can be deployed to Azure this way, however the `UpdatePostWithComments` function which is reacts to updates/deletes to documents within the <br/>
comments database can be deployed as a separate function app in a production environment. <br /> <br /><br/>
**THIS APPLICATION REQUIRES A PROPER IDENTITY MECHANISM (AUTHENTICATION / AUTHORIZATION) TO BE PRODUCTION READT (e.g JWT Authentication)**

