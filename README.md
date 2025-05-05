Complete implementation of a .NET Core(9) Web API with JWT authentication using In Memory Database. 
 - This example includes:
  - Entities for defining domain models
  - Models/DTOs for request and response objects
  - DbContext for configuring the database context
  - Controller with endpoints for user registration, login, fetching user profile (authorization required), and refreshing tokens
  - Middleware for validating JWT tokens
  - Interfaces and Services to handle authentication and business logic with dependency injection
  - appsettings.json configuration for JWT settings
  - Program.cs setup for middleware and services

Packages needs to include like below:
- Microsoft.AspNetCore.Authentication.JwtBearer
- Microsoft.EntityFrameworkCore
- Swashbuckle.AspNetCore
- System.IdentityModel.Tokens.Jwt
