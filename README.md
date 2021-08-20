# Online Shop specializing in computer components
## Technologies
- ASP.NET Core 3.1
- Entity Framework Core 3.1 Code First
- RESTful API, Swagger
- HTML/CSS/Javscript/Ajax/jQuery
## Install Tools
- .NET Core SDK 3.1
- Git client
- Visual Studio 2019
- SQL Server 2019
## Install Packages
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.EntityFrameworkCore.Design
- FluentValidation.AspNetCore
- Microsoft.AspNetCore.Authentication.JwtBearer
- And mores....
## Youtube tutorial
- Video list: https://www.youtube.com/playlist?list=PLRhlTlpDUWsyN_FiVQrDWMtHix_E2A_UD
## Install tutorial
## How to configure and run
- Clone code from Github: git clone https://github.com/thisthuanyo/TechShopSolution.git
- Open solution TechShopSolution.sln in Visual Studio 2019
- Set startup project is TechShopSolution.Data
- Change connection string in Appsetting.json in TechShopSolution.Data project
- Open Tools --> Nuget Package Manager --> Package Manager Console in Visual Studio
- Run Update-database and Enter.
- After migrate database successful, set Startup Project is TechShopSolution.BackendApi
- Change database connection in appsettings.Development.json in TechShopSolution.BackendApi project.
- You need to change 3 projects to self-host profile.
- Set multiple run project: Right click to Solution and choose Properties and set Multiple Project, choose Start for 3 Projects: BackendApi, WebApp and AdminApp.
- Choose profile to run or press F5
## Template Used 
- Admin template: https://startbootstrap.com/template/sb-admin
- Portal template: https://www.free-css.com/free-css-templates/page194/bootstrap-shop
## My Productions 
- Admin App: https://admin.techshopvn.xyz
- Portal App: https://techshopvn.xyz
- API: https://api.techshopvn.xyz/swagger