## Oracle.API
A .NET6 Web API that talks to an Oracle database with EF Core.

### Nuget packages used
```
<PackageReference Include="Mediator.SourceGenerator" Version="2.1.7" />
<PackageReference Include="Microsoft.CodeAnalysis" Version="4.7.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="7.0.11" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="7.0.11" />
<PackageReference Include="Microsoft.Net.Compilers.Toolset" Version="4.7.0" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />
<PackageReference Include="Mediator.Abstractions" Version="2.1.7" />
<PackageReference Include="Riok.Mapperly" Version="3.2.0" />
<PackageReference Include="Oracle.EntityFrameworkCore" Version="7.21.11" />
```

### Project Layout
The projects in this solution are structured in a hexagonal ports and adaptors style enabled by `Mediator.SourceGenerator`

### Docker
There is a `docker-compose.yml` YAML file included that references the `Dockerfile` in the `Oracle.API` project.
The API has a dependency on an Oracle database and the `gvenzl/oracle-xe` Oracle Database Express Edition Container / Docker image
is included as the `oracle-db` service in the `docker-compose.yml`. `oracle-db` is configured to create a new Oracle database called
`APIDB` on first run with test user credentials.

Docker and docker-compose run and debug were tested in Jetbrains Rider 2023.2.2

### EF Core
The `Oracle.Database` project contains migrations and the application is setup to apply migrations at runtime.

### API
The goal of this project is to demonstrate .NET EF Core interaction with an Oracle database. The two included controllers are REST-like
but are not strictly REST. The general concept modelled in the API is an Employee/Department relationship with endpoints to support listing
departments, employees, department and employee search, department and employee insert and department and employee removal. 

### Interacting with the API
The API is Swashbuckle Swagger enabled and there is also a postman collection included in the `/test` directory.