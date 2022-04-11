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
16. Excel Mapper
17. Core Html To Image
18. Dink To Pdf

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
11. Export & read from excel file
12. Export HTML to PDF
13. Export HTML to image

# Basic Usage

To use this template, open your cli window and run the following command

```
>> dotnet new --install Ibtkiar.DotNetCore.TheBoilerPlateTemplate::2.1.0
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

- All application configurations is defined in **appsettings.json**, it is your job to define configuration for each environment and also store sensetive configuration data in secure place like user secrets on local machine or environment variables on server.
  <br>
- Every domain entity inherit from the base class **Entity&lt;T&gt;** where T is the type of the identity property, this class contains all the shared properties between domain entities.
  <br>
- You can create value objects simply by inheriting from **ValueObject** class.
  <br>
- You can mark some classes as aggregate roots by make them implement an empty interface called **IAggregateRoot**.
  <br>
- Any method in application features considered an operation and returns an operation result, these operation results is divided into 4 types:

  1.  **OperationResult.Success()**: used to return a success operation result.
  2.  **OperationResult.Success(message)**: used to return a success operation result with string message/description.
  3.  **OperationResult.Success(data)**: used to return a success operation result with data.
  4.  **OperationResult.Fail(httpStatusCode, error_1, error_2, ...)**: used to return a fail operation result, you can specify which http status
      code to return and a comma sperated array of **OperationError**, each operation error has its **_Type_** specify where the error
      is caused, **_Error_** which descripe the error and **_ErrorPlaceholders_** array to replace any values in the Error attribute.
      <br>

- These operation results can be handeled automatically to transform to a proper Api response by simply decorate an action method or
  a controller class by **[ServiceFilter(typeof(ApiResultFilterAttribute))]**, this is done by default in the **BaseApiController**. Or you can handle this operation result mannual in case you make MVC action methods with views.
  <br>
- We have three types of results we can get from Api responses:

1. **Result&lt;T&gt;** for single record

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

2. **PaginatedResult&lt;T&gt;** for lists contain paging data

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

3. **Error** for returing list of errors with error type and error message

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

- For errors handling we use a middleware **ErrorHandlingMiddleware** to convert it to the error response format.
  <br/>
- We have 4 Http status codes used in the system by default:
  1. **_200_** for successfull response
  2. **_401_** for unauthorized response
  3. **_422_** for validation and attribute errors response
  4. **_400_** for invalid or missing request headers
  5. **_404_** for not found data
     <br>
- You can extend and add more response codes to use in the application by editing the **ErrorStatusCodes** in the Helpers project in Constant folder.
  <br>
- To request any Api end point you must provide **x-api-key** header in the request headers. All api keys for different clients are configured in the **_appsettings.json_** file.
  <br>
- The application is configured with 2 languages Arabic and English, the default language is English, to change it add the **Accept-Language** header when requesting the Api end points, ex: ar-SA or ar for Arabic.
  <br>
- The Api controllers are inherit from the **BaseApiController** which contains shared dependencies and attributes between controllers.
  <br>
- The application contains by default all account registeration process end points in the **UsersController**.
  <br>
- To use the authorization system:

  1. you will use the two enum files included in the authorization folder in application layer, the first file is **DefaultRoles.cs** to define any default roles your business need, by default there is a single role which is **Super_Admin** which has all privileges in the system.
  2. The second file is **Permissions.cs** this enum is used to add all your custom permissions of your application.
  3. You can add custom roles according to your app needs through the **[Role]** entity of the .net identity.
  4. The permissions you add can be granted to roles or to individual users, in both cases we use claims to add permissions to roles **[by using RoleClaims entity]** or by adding permissions to users **[by using UserClaims entity]**, you just need to add claim with type **action_permission** and its value will be the id of the permission.
  5. Finally to add permission to an end point you just need to add attribute **HasPermission** to the action method, this attribute can take single permission or multiple permissions with definition of how to join this permissions either by **_AND_** [must have all permissions] or by **_OR_** [any permission the user have can access the end point].
     <br>

  ```
  [HasPermission(Permissions.ViewUsers)]
  [HttpGet(baseRoute)]
  public async Task<IActionResult> GetUsersList([FromQuery] string name,
            [FromQuery] Dictionary<string, string> sort,
            [FromQuery]int page_number, [FromQuery]int page_size)
  ```

  ```
  [HasPermission(PermissionCompareOperator.And, Permissions.ViewUsers, Permissions.EditUsers)]
  [HttpGet(baseRoute)]
  public async Task<IActionResult> GetUsersList([FromQuery] string name,
            [FromQuery] Dictionary<string, string> sort,
            [FromQuery]int page_number, [FromQuery]int page_size)
  ```

  ```
  [HasPermission(PermissionCompareOperator.Or, Permissions.ViewUsers, Permissions.EditUsers)]
  [HttpGet(baseRoute)]
  public async Task<IActionResult> GetUsersList([FromQuery] string name,
            [FromQuery] Dictionary<string, string> sort,
            [FromQuery]int page_number, [FromQuery]int page_size)
  ```

- By default the logs is saved in simple text files in the application server and there is a **_LogsController_** with single end point to see all log files ordered by date and each file is a link to open the log file. You can extend this implementation to use the database or even better use external application server as ELK which is done very simply as we already use serilog in the template.

- To use the PDF export you need to be aware of the following:
   1. **For Windows deployment:** you need to download [libwkhtmltox.dll] file from the following url https://github.com/rdvojmoc/DinkToPdf/tree/master/v0.12.4/64%20bit and put this file in the root directory of the WebApi application and right-click on the dll file in Solution Explorer and choose properties.  For the Build Action we are going to choose Content and for the Copy to Output Directory, we are going to choose Copy always.
   2. **For Linux deployment:** you need to install the WebKit engine files on the Linux machine, if you use docker you just need to add these two lines to be able to export pdf from Linux environment
   ```
   FROM base AS final
   RUN apt-get update && apt-get install -y libgdiplus wget libfontenc1 x11-common xfonts-encodings fontconfig xfonts-75dpi xfonts-base xfonts-utils lsb-base 

   WORKDIR /app
   RUN wget https://github.com/wkhtmltopdf/packaging/releases/download/0.12.6-1/wkhtmltox_0.12.6-1.buster_amd64.deb && dpkg -i wkhtmltox_0.12.6-1.buster_amd64.deb
   ```
- You will find two sample Apis for exporting PDF and image files, they return byte[] which can be returned directly to the caller to download the files or to use them to save the files on local or cloud storage.
- You will find also in the WebApi project a docker file to use it when deploy to Linux server.
