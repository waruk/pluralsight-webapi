# Building Your First API with ASP.NET Core
Kevin Dockx, Pluralsight  

## Overview

.NET Core versions can be installed side by side.  
.NET Core runs on both .NET Framework and .NET Core  

**Program -> Main()** is the starting point of the application. **Startup** class is the entry point for a web application.

**ConfigureServices()** is used to add services to the container, and to configure those services.

Hosting environment can be configured for a .NET ASP Core application:
- Development
- Staging
- Production  
- Custom

Middleware can be added to a .net Core application. Middleware are components that fit in the request/response pipeline.

## Creating the API and returning resources

ASP.NET Core MVC is used to create both http services and web applications.  

The nuget package Microsoft.AspNetCore.All contains the MVC framework package (Microsoft.AspNetCore.Mvc) and other useful packages (entity framework, etc.).

**RunTime store** is a sort of GAC.

## HTTP Verbs

GET: retrieves resource  
POST: creates resource  
DELETE: removes resource

## HTTP Status Codes

100: Informational (not used by APIs)  
200: Success  
- 200 GET success
- 201 Resource created
- 204 Success, does not return anything (eg. DELETE)  

300: Redirection (APIs don’t usually use these)  
400: Client Error
- 400 Bad request
- 401 Unauthorized
- 403 User does not have access to the resource
- 404 Not found
- 409 Conflict (eg. Conflict between two simultaneous updates)

500: Server Error
- 500 Internal Server Error

## Routing

For APIs, **attribute based** routing is preffered to convention based routing (convention based is used when returning views).

## Formatters and Content Negociation

Client send media type accepted via the Accept header of the request:
- application/json
- application/xml
- etc.

## IoC & DI

Without IoC you have tight coupling:
- Class implementation has to change when a dependency changes
- Difficult to test (can't replace the dependency with a mockup)
- Class manages the lifetime of the dependency

**Inversion of Control:**  
Delegates the function of selecting a concrete implementation type for a class's dependencies
to an external component.

**Dependency Injection**
Is a specialization of the IoC pattern. DI pattern uses an object (the container)
to initialize objects and provide the required dependencies to the object.
- Class has a reference to an interface implemented by the dependency
- Concrete implementation is injected via constructor

Dependency Injection is built into asp.net core. **ConfigureServices** is used 
to register services with the built-in container.

## Logging API

Logging is built into asp.net core. It works with a variety of built-in and 
third-party logging providers.

Built-in:
- Console
- Debug
- EventSource
- EventLog
- TraceSource

## Custom services

Services can be added in **Startup** class with the following lifetime:
- AddTransient: created each time they are requested
- AddScoped: created once per request
- AddSingleton: created the first time they are requested and last for the 
application lifetime?

## Configuration files
Create an IConfigurationRoot property in Startup class. It should be static.
Inject an IConfiguration object in Startup contructor.  
It is possible to have different configurations for different 
environments (eg. appSettings.Production.json).

## EF Core
Code-first / Database-first  
Has providers for:
 - Sql Server
 - Sql Lite
 - PostgreSQL
 - Sql Server CE
 - MySql
 - IBM DB2
 - In-memory provider for testing

**Migrations:**
Add-Migration \<name>  
Table _EFMigrationsHistory_ keeps track of migrations applied to the db.

Sensitive data, such as connection strings, can be kept in **environment variabiles**.
If defined via Visual Studio (project properties), these are kept in **launchSettings.json**,
which can be used in development. For production server, define a system 
environment variabile on that machine.

**Seed data**

There are several ways this can be accomplished in EF Core:
- Model seed data
- Manual migration customization
- Custom initialization logic
