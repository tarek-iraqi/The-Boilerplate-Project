# Introduction
This is the DotNet core template to use when create new Api project

## The Technolgies and libraries used:
1. Asp.net core 5
2. Entityframework core 5
3. Pomelo EntityFrameworkCore MySql
4. Asp.net core identity
5. Mediatr
6. Fluent validation
7. Fluent email
8. Automapper
9. Serilog
10. Firebase admin
11. Lib Phonenumber
12. Xunit
13. Moq
14. Shoudly

## Included features
1. Registeration and authentication process
2. Sending emails
3. Sending firebase notifications
4. Phone number validation
5. Database audit trail
6. Multi language support

# Basic Usage
To use this template, open your cli window and run the following command
  ```
  >> dotnet new --install Ibtkiar.DotNetCore.TheBoilerPlateTemplate::1.0.1
  ```
  Then navigate to your project folder directory and run the following command
  ```
  >> dotnet new dotnetcorethebpp
  ```
  To start working with the template we need to setup some basic configuration:
  1. you need to add the database connection string as a user secret or as environment variable, the default name used is ***DbConnection***.
  2. you need to add the basic database migrations, the default database provider configured is ***MySql***, you can change it to any database provider as SqlServer or Postgres
  but make sure to change it before adding migrations.
     * To change the database provider install the required nuget package and in the Persistence project in infrastructure layer go to the following ***ServiceCollectionExtensions -> AddPersistenceServices*** to configure the database provider.
  ```
  services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(configuration[KeyValueConstants.DbConnection],
                ServerVersion.AutoDetect(configuration[KeyValueConstants.DbConnection]), mySqlOptions =>
                {
                    mySqlOptions.CharSetBehavior(CharSetBehavior.AppendToAllColumns)
                    .CharSet(CharSet.Utf8Mb4);
                }).UseLazyLoadingProxies());
  ```
  3. you need add the secret key to hach and encrypt the JWT tokens, this can be done by add the following in user secrets or as an environment variable 
  ***System:JWTSettings:SecretHashKey***
  # Template Structure
  This template is following the clean architecture model, it is divided in to the following 3 main layers
  * Core layer
  * Infrastructure layer
  * Presentation layer
  ## Core layer
  This layer contains the core application domain entities and business logic, it is divided into 3 projects:
  
  **1. Helpers project:** contains all the base interfaces and classes that we need accros the application, for example: Generic Respository,
  Unit of work, application exception, constants and so on.It is divided into subfolders as following:
  * **Interfaces:** all basic interfaces
  * **Classes:** all interfaces implementation
  * **Constants:** as resource names, error types and any constant used across the application
  * **Exceptions:** custom application exceptions
  * **Extensions:** extension methods
  * **Models:** basic models as settings
  * **Resources:** the language resource files
  
  **2. Domain project:** contains all the domain entities and domain business logic and validations
  
  **3. Application project:** contains all the use cases features and interfaces to interact with other services as database, send emails, send SMS and so on.It is divided into 
  subfolders as following:
  * **Common:** contains any comman logic shared across application
  * **DTOs:** contains all data transfer object classes used in methods response
  * **Interfaces:** all contracts to use/integrate with other internal or external services implemented in the infrastructure
  * **Features:** the main appplication use cases, it is using the Mediator pattern so that each feature is a request (command/query) with its handler
  * **Specifications:** contians all the queries logic we use to get data from the database, they are grouped by features
  
  ## Infrastructure layer
  This layer contains any application interface implementation or integration with any external services. it is divided into 2 projects:
  
  **1. Persistence project:** conatins all the main configuration needed to communicate with the database. The ORM used to query the database is Entityframework core.It is
  divided into subfolders as following:
  * **AuditTrail:** contains all the logic needed to add audit trail for every operation done on the database tables
  * **Common:** contains all the implementation of the common logic across the persistence as Generic repository and unit of work
  * **Context:** contains the DbContext to interact with the database and extension methods to extend its functionality
  * **EntitiesConfiguration:** contains database entities configurations
  * **Identity:** contains identity services to manage users, roles and claims data
  * **Migrations:** contains database migration files
  
  **2. Utilities project:** contains the application interfaces implementation to use different services, the available services are:
  * **Application configurations:** contains all sources of application configurations
  * **Localization:** contains methods to get translation and the application culture data
  * **Send emails:** contains methods to send emails
  * **Send firebase notifications:** contains methods to send firebase notifications to individual devices and sending firebase tobics
  * **Phone validator:** contains methods to validate phone numbers to countires and getting the national and international phone formates
  
  ## Presentation layer
  This layer contains the Api project, it contains the controllers end points, extensions, middlewares and services.
  
  # How things are working
  * Every domain entity inherit from the base class ***AuditableEntity&lt;T&gt;*** where T is the type of the identity property, this class contains all the shared properties
  between domain entities.

  * We have three types of responses we can get from request handlers:
  1. ***Result&lt;T&gt;*** for single record
  ```
  {
    "data": {
        "user": {
            "id": "08d8c6ea-51f4-4a2c-8061-2e65642111d8",
            "name": "Tarek Iraqi",
            "email": "tarek.iraqi@gmail.com",
            "mobile": "+201003776200"
        },
  }
  ```
  2. ***PaginatedResult&lt;T&gt;*** for lists contain paging data
  ```
  {
    "meta": {
        "hasPrevious": false,
        "hasNext": false,
        "totalPages": 1,
        "total": 1
    },
    "data": [
        {
            "id": "08d8c6ea-51f4-4a2c-8061-2e65642111d8",
            "name": "Tarek Iraqi",
            "email": "tarek.iraqi@gmail.com",
            "mobile_number": "+201003776200"
        }
    ]
  }
  ```
  3. ***Error*** for returing list of errors with error type and error message
  ```
  {
    "errors": [
        {
            "type": "username",
            "error": "'Username' must not be empty."
        },
        {
            "type": "username",
            "error": "'Username' is not a valid email address."
        }
    ]
  }
  ```
* For errors and validations handling we throw application custom exception with list of all errors with messages and the proper http status code to return, this exception is 
intercepted by a middleware ***ErrorHandlingMiddleware*** to convert it to the error response format and set the http status code for the request response.

* We have 4 Http status codes used in the system by default:
1. ***200*** for successfull response
2. ***401*** for unauthorized response
3. ***402*** for validation and attribute errors response
4. ***400*** for invalid or missing request headers

* You can extend and add more response codes to use in the application by editing the ***ErrorStatusCodes*** in the Helpers project in Constant folder.

* To request any Api end point you must provide a ***x-api-key*** header in the request headers. All api keys for different clients are configured in the ***appsettings.json***
file.

* The application is configured with 2 languages Arabic and English, the default language is English, to change it add the ***Accept-Language*** header when requesting
the Api end points, ex: ar-SA for Arabic.

* The Api controllers are inherit from the ***BaseApiController*** which contains shared dependencies and attributes between controllers.

* The application contains by default all account registeration process end points in the ***UsersController***.
