# Introduction

This is the DotNet core template to use when create new Api project

## The Technolgies and libraries used:

1. Asp.net 6
2. Entityframework 6
3. Pomelo EntityFrameworkCore MySql
4. Asp.net identity
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
15. Amazon S3

## Included features

1. Registeration and authentication process
2. Authorization and permissions mechanizm
3. Sending emails
4. Sending firebase notifications
5. Phone number validation
6. Database audit trail
7. Multi language support
8. File type & size validators
9. Upload to local server or amazon S3
10. Logging

# Basic Usage

To use this template, open your cli window and run the following command

```
>> dotnet new --install Ibtkiar.DotNetCore.TheBoilerPlateTemplate::2.0.0
```

Then navigate to your project folder directory and run the following command

```
>> dotnet new dotnetcorethebpp
```

To start working with the template we need to setup some basic configuration:

1. you need to add the database connection string as a user secret or as environment variable, the default name used is **_DbConnection_**.
2. you need to add the basic database migrations, the default database provider configured is **_MySql_**, you can change it to any database provider as SqlServer or Postgres
   but make sure to change it before adding migrations.
   - To change the database provider install the required nuget package and in the Persistence project in infrastructure layer go to the following **_ServiceCollectionExtensions -> AddPersistenceServices_** to configure the database provider.

```
services.AddDbContext<ApplicationDbContext>(options =>
              options.UseMySql(configuration[KeyValueConstants.DbConnection],
              ServerVersion.AutoDetect(configuration[KeyValueConstants.DbConnection])).UseLazyLoadingProxies());
```

3. you need add the secret key to hach and encrypt the JWT tokens, this can be done by add the following in user secrets or as an environment variable
   **_System:JWTSettings:SecretHashKey_**

# Template Structure

This template is following the clean architecture model, it is divided in to the following 3 main layers

- Core layer
- Infrastructure layer
- Presentation layer

## Core layer

This layer contains the core application domain entities and business logic, it is divided into 3 projects:

**1. Helpers project:** contains all the base interfaces and classes that we need accros the application, for example: Generic Respository,
Unit of work, application exception, constants and so on.It is divided into subfolders as following:

- **Interfaces:** all basic interfaces
- **Classes:** all interfaces implementation
- **Constants:** as resource names, error types and any constant used across the application
- **Exceptions:** custom application exceptions
- **Extensions:** extension methods
- **Models:** basic models as settings
- **Resources:** the language resource files

**2. Domain project:** contains all the domain entities and domain business logic and validations

**3. Application project:** contains all the use cases features and interfaces to interact with other services as database, send emails, send SMS and so on. It is divided into subfolders as following:

- **Common:** contains any comman logic shared across application
- **DTOs:** contains all data transfer object classes used in methods response
- **Interfaces:** all contracts to use/integrate with other internal or external services implemented in the infrastructure
- **Features:** the main appplication use cases, it is using the Mediator pattern so that each feature is a request (command/query) with its handler
- **Specifications:** contians all the queries logic we use to get data from the database, they are grouped by features
- **Authorization:** contains all the logic for building dynamic policy based authorization system

## Infrastructure layer

This layer contains any application interface implementation or integration with any external services. it is divided into 2 projects:

**1. Persistence project:** conatins all the main configuration needed to communicate with the database. The ORM used to query the database is Entityframework core.It is
divided into subfolders as following:

- **AuditTrail:** contains all the logic needed to add audit trail for every operation done on the database tables
- **Common:** contains all the implementation of the common logic across the persistence as Generic repository and unit of work
- **Context:** contains the DbContext to interact with the database and extension methods to extend its functionality
- **EntitiesConfiguration:** contains database entities configurations
- **Identity:** contains identity services to manage users, roles and claims data
- **Migrations:** contains database migration files

**2. Utilities project:** contains the application interfaces implementation to use different services, the available services are:

- **Application configurations:** contains all sources of application configurations
- **Localization:** contains methods to get translation and the application culture data
- **Send emails:** contains methods to send emails
- **Send firebase notifications:** contains methods to send firebase notifications to individual devices and sending firebase tobics
- **Phone validator:** contains methods to validate phone numbers to countires and getting the national and international phone formates
- **File validator:** contains methods to check for file types and file size
- **Local server upload:** contains implementation to upload files to application local server
- **Amazon upload:** contains implementation to upload files to amazon S3 storage

## Presentation layer

This layer contains the Api project, it contains the controllers end points, extensions, middlewares and services.

# How things are working

- All application configurations is defined in <span style="background-color: #ffd555; padding: 3px">**appsettings.json**</span>, it is your job to define configuration for each environment and also store sensetive configuration data in secure place like user secrets on local machine or environment variables on server.
  <br>
- Every domain entity inherit from the base class <span style="background-color: #ffd555; padding: 3px">**Entity&lt;T&gt;**</span> where T is the type of the identity property, this class contains all the shared properties between domain entities.
  <br>
- You can create value objects simply by inheriting from <span style="background-color: #ffd555; padding: 3px">**ValueObject**</span> class.
  <br>
- You can mark some classes as aggregate roots by make them implement an empty interface called <span style="background-color: #ffd555; padding: 3px">**IAggregateRoot**</span>.
  <br>
- Any method in application features considered an operation and returns an operation result, these operation results is divided into 4 types:

  1.  <span style="background-color: #ffd555; padding: 3px">**OperationResult.Success()**</span>: used to return a success operation result.
  2.  <span style="background-color: #ffd555; padding: 3px">**OperationResult.Success(message)**</span>: used to return a success operation result with string message/description.
  3.  <span style="background-color: #ffd555; padding: 3px">**OperationResult.Success(data)**</span>: used to return a success operation result with data.
  4.  <span style="background-color: #ffd555; padding: 3px">**OperationResult.Fail(httpStatusCode, error_1, error_2, ...)**</span>: used to return a fail operation result, you can specify which http status
      code to return and a comma sperated array of <span style="background-color: #ffd555; padding: 3px">**OperationError**</span>, each operation error has its **_Type_** specify where the error
      is caused, **_Error_** which descripe the error and **_ErrorPlaceholders_** array to replace any values in the Error attribute.
      <br>

- These operation results can be handeled automatically to transform to a proper Api response by simply decorate an action method or
  a controller class by <span style="background-color: #ffd555; padding: 3px">**[ServiceFilter(typeof(ApiResultFilterAttribute))]**</span>, this is done by default in the **BaseApiController**. Or you can handle this operation result mannual in case you make MVC action methods with views.
  <br>
- We have three types of results we can get from Api responses:

1. <span style="background-color: #ffd555; padding: 3px">**Result&lt;T&gt;**</span> for single record

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

2. <span style="background-color: #ffd555; padding: 3px">**PaginatedResult&lt;T&gt;**</span> for lists contain paging data

```
{
  "meta": {
      "hasPrevious": false,
      "hasNext": false,
      "totalPages": 1,
      "total": 1,
      "pageIndex": 1
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

3. <span style="background-color: #ffd555; padding: 3px">**Error**</span> for returing list of errors with error type and error message

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

- For errors handling we use a middleware <span style="background-color: #ffd555; padding: 3px">**ErrorHandlingMiddleware**</span> to convert it to the error response format.
  <br/>
- We have 4 Http status codes used in the system by default:
  1. **_200_** for successfull response
  2. **_401_** for unauthorized response
  3. **_422_** for validation and attribute errors response
  4. **_400_** for invalid or missing request headers
  5. **_404_** for not found data
     <br>
- You can extend and add more response codes to use in the application by editing the <span style="background-color: #ffd555; padding: 3px">**ErrorStatusCodes**</span> in the Helpers project in Constant folder.
  <br>
- To request any Api end point you must provide <span style="background-color: #ffd555; padding: 3px">**x-api-key**</span> header in the request headers. All api keys for different clients are configured in the **_appsettings.json_** file.
  <br>
- The application is configured with 2 languages Arabic and English, the default language is English, to change it add the <span style="background-color: #ffd555; padding: 3px">**Accept-Language**</span> header when requesting the Api end points, ex: ar-SA or ar for Arabic.
  <br>
- The Api controllers are inherit from the <span style="background-color: #ffd555; padding: 3px">**BaseApiController**</span> which contains shared dependencies and attributes between controllers.
  <br>
- The application contains by default all account registeration process end points in the <span style="background-color: #ffd555; padding: 3px">**UsersController**</span>.
  <br>
- To use the authorization system:

  1. you will use the two enum files included in the authorization folder in application layer, the first file is <span style="background-color: #ffd555; padding: 3px">**DefaultRoles.cs**</span> to define any default roles your business need, by default there is a single role which is **Super_Admin** which has all privileges in the system.
  2. The second file is <span style="background-color: #ffd555; padding: 3px">**Permissions.cs**</span> this enum is used to add all your custom permissions of your application.
  3. You can add custom roles according to your app needs through the **[Role]** entity of the .net identity.
  4. The permissions you add can be granted to roles or to individual users, in both cases we use claims to add permissions to roles **[by using RoleClaims entity]** or by adding permissions to users **[by using UserClaims entity]**, you just need to add claim with type <span style="background-color: #ffd555; padding: 3px">**action_permission**</span> and its value will be the id of the permission.
  5. Finally to add permission to an end point you just need to add attribute <span style="background-color: #ffd555; padding: 3px">**HasPermission**</span> to the action method, this attribute can take single permission or multiple permissions with definition of how to join this permissions either by **_AND_** [must have all permissions] or by **_OR_** [any permission the user have can access the end point].
     <br>

  ```
  [HasPermission(Permissions.ViewUsers)]
  [HttpGet(baseRoute)]
  public async Task<IActionResult> GetUsersList([FromQuery] string name,
            [FromQuery] Dictionary<string, string> sort,
            [FromQuery]int page_number, [FromQuery]int page_size)
  ```

  ```
  [HasPermission(new[] { Permissions.ViewUsers, Permissions.EditUsers }, PermissionCompareOperator.And)]
  [HttpGet(baseRoute)]
  public async Task<IActionResult> GetUsersList([FromQuery] string name,
            [FromQuery] Dictionary<string, string> sort,
            [FromQuery]int page_number, [FromQuery]int page_size)
  ```

  ```
  [HasPermission(new[] { Permissions.ViewUsers, Permissions.EditUsers }, PermissionCompareOperator.Or)]
  [HttpGet(baseRoute)]
  public async Task<IActionResult> GetUsersList([FromQuery] string name,
            [FromQuery] Dictionary<string, string> sort,
            [FromQuery]int page_number, [FromQuery]int page_size)
  ```

- By default the logs is saved in simple text files in the application server and there is a **_LogsController_** with single end point to see all log files ordered by date and each file is a link to open the log file. You can extend this implementation to use the database or even better use external application server as ELK which is done very simply as we already use serilog in the template.
