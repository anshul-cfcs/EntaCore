using Application.Interfaces;
using Application.Interfaces.Database;
using Application.Services;
using Infrastructure.Data;
using Infrastructure.Services;
using Infrastructure.Services.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.Filters;
using System.Text;


namespace EntaCoreAPI.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddProjectDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            AddFrameworkServices(services, configuration);
            AddCoreServices(services);
            AddDatabase(services, configuration);

            return services;
        }
        private static void AddFrameworkServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme(\"bearer {token}\")",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                    };
                });

            services.AddAuthorization();
            services.AddHttpContextAccessor();
            services.AddMemoryCache();
        }

        private static void AddCoreServices(IServiceCollection services)
        {
            services.AddScoped<IStoredProcedureExecutor, StoredProcedureExecutor>();
            services.AddScoped<ISuperAdminCompanyService, SuperAdminCompanyService>();
            services.AddScoped<ISuperAdminCompanyDbService, SuperAdminCompanyDbService>();
            services.AddScoped<IDapperConnectionFactory, DapperConnectionFactory>();
        }
        private static void AddDatabase(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CoreDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("Default")));
        }
    }
}
