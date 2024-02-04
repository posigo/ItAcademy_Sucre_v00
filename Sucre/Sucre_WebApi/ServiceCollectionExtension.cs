using Microsoft.EntityFrameworkCore;
using Sucre_DataAccess.Data;
using Sucre_DataAccess.Repository.IRepository;
using Sucre_DataAccess.Repository;
using Sucre_Services.Interfaces;
using Sucre_Services;
using Sucre_Mappers;
using Sucre_DataAccess.CQS.Commands;
using Hangfire;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace Sucre_WebApi
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection RegisterServices
            (this IServiceCollection services, IConfiguration configuration)
        {

            string connectionString = configuration.GetConnectionString("DefaultConnection").ToString();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            //подключаемые сервисы из DAL            
            services.AddScoped<IDbSucreAppRole, DbSucreAppRole>();
            services.AddScoped<IDbSucreAppUser, DbSucreAppUser>();
            services.AddScoped<IDbSucreAsPaz, DbSucreAsPaz>();
            services.AddScoped<IDbSucreCanal, DbSucreCanal>();
            services.AddScoped<IDbSucreCex, DbSucreCex>();
            services.AddScoped<IDbSucreEnergy, DbSucreEnergy>();
            services.AddScoped<IDbSucreGroupUser, DbSucreGroupUser>();
            services.AddScoped<IDbSucreParameterType, DbSucreParameterType>();
            services.AddScoped<IDbSucrePoint, DbSucrePoint>();
            services.AddScoped<ISucreUnitOfWork, SucreUnitOfWork>();

            //подключаемые сервисы из BLL
            services.AddScoped<IAsPazService, AsPazService>();
            services.AddScoped<ICanalService, CanalService>();
            services.AddScoped<ICexService, CexService>();            
            services.AddScoped<IEnergyService, EnergyService>();
            services.AddScoped<IMeasurementService, MeasurementService>();
            services.AddScoped<IParameterTypeService, ParameterTypeService>();  
            services.AddScoped<IPointService, PointService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ITokenService,TokenService>();
            services.AddScoped<IUserService, UserService>();

            //Mappers
            services.AddScoped<AppRoleMapper>();
            services.AddScoped<AppUserMapper>();
            services.AddScoped<AsPazMapper>();
            services.AddScoped<CanalMapper>();
            services.AddScoped<CexMapper>();
            services.AddScoped<EnergyMapper>();
            services.AddScoped<ParameterTypeMapper>();
            services.AddScoped<PointMapper>();
            services.AddScoped<TokenMapper>();
            services.AddScoped<ValueMapper>();

            // Add Hangfire services.
            services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));
            // Add the processing server as IHostedService
            services.AddHangfireServer();

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(AddEnergyCommand).Assembly);
                //cfg.RegisterServicesFromAssembly(typeof(GetCexPointsByIdQuery).Assembly);
            });


            return services;
        }

        public static void ConfigureJwt(this IServiceCollection services,
            IConfiguration configuration)
        {
            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];
            var secretKey = configuration.GetSection("Jwt:Secret").Value;
            var signindKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = issuer,
                        ValidateAudience = true,
                        ValidAudience = audience,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signindKey
                    };
                    //options.Events = new JwtBearerEvents()
                    //{
                        
                    //}
                });
        }

        public static void ConfigureSwaggerGen(this IServiceCollection services)
        {
            services.AddSwaggerGen(config =>
            {
                config.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    //Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    //BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme"
                });
                config.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                        },
                        new string[] {}
                    }
                });
            });

        }

    }
}
