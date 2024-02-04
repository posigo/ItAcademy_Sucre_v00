using FluentValidation;
using Hangfire;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Sucre_MVC.Filters;
using Sucre_MVC.FluentValidation;
using Sucre_Core.LoggerExternal;
using Sucre_DataAccess.Data;
using Sucre_DataAccess.Repository;
using Sucre_DataAccess.Repository.IRepository;
using Sucre_DataAccess.Services;
using Sucre_Mappers;
using Sucre_Services;
using Sucre_Services.Interfaces;
using Sucre_Utility;
using System.Security.Claims;
using Sucre_DataAccess.CQS.Commands;

namespace Sucre
{
    public class Program
    {       
        public static void Main(string[] args)
        {
            LoggerExternal.LoggerEx.Information($"*->------Program Main start {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} ------");
            try
            {
                
                var builder = WebApplication.CreateBuilder(args);

                //var SuperPassword = WM.GenerateMD5Hash(
                //    builder.Configuration["AppSettings:SuperUser"].ToString(),
                //    builder.Configuration["AppSettings:PasswordSalt"].ToString());

                //Serilog.ILogger logger = new LoggerConfiguration()
                //    .MinimumLevel.Debug()
                //    .Enrich.FromLogContext()
                //    .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug)
                //    .WriteTo.File("Log-.log", rollingInterval: RollingInterval.Day,restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
                //    .CreateLogger();
                builder.Logging.ClearProviders();
                //builder.Logging.AddSerilog(logger);
                
                //Link External Logger from Sucre_Core
                builder.Logging.AddSerilog(LoggerExternal.LoggerEx);
                //builder.Logging.AddSerilog();
                                
                LoggerExternal.LoggerEx.Information("*->------Application Run------");

                var provider = builder.Services.BuildServiceProvider();
                var configuration = provider.GetRequiredService<IConfiguration>();
                builder.Services.AddSingleton<IConfiguration>(configuration);

                // Add services to the container.
                builder.Services.AddControllersWithViews(options =>
                {
                    //options.Filters.Add(new ResourceFilterGlobal(0));
                });

                builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LoginPath = "/AppUser/AppUserLogin";
                        options.AccessDeniedPath = "/AppUser/AccessDeniedPath";
                        options.LogoutPath = "/AppUser/AppUserLogout";
                    });
                builder.Services.AddAuthorization(options =>
                {
                    options.AddPolicy(WC.SupervisorPolicy, policy =>
                    {
                        policy.RequireAssertion(claim => claim.User.HasClaim(ClaimTypes.Role, WC.SupervisorRole) &&
                            claim.User.HasClaim("GroupId", "999"));                        
                    });
                    options.AddPolicy(WC.ManagerPolicy, policy =>
                    {
                        policy.RequireAssertion(claim => claim.User.HasClaim(ClaimTypes.Role, WC.AdminRole) &&
                            claim.User.HasClaim("GroupId", "999"));
                    });
                    options.AddPolicy(WC.UserPolicy, policy =>
                    {
                        policy.RequireAssertion(claim => claim.User.HasClaim(ClaimTypes.Role, WC.UserRole));
                    });
                });

                string connectionString = builder.Configuration.GetConnectionString("DefaultConnection").ToString();
                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(connectionString));
                WP.DatabaseName = connectionString.
                    Split(';').ToList().FirstOrDefault(item => item.Contains("Database")).
                    Split('=').Last().ToString();
                var dbn = WP.DatabaseName;

                //add services validationn
                builder.Services.AddValidatorsFromAssemblyContaining<UserLoginValidator>();

                //подключаемые сервисы из DAL
                builder.Services.AddScoped<IDbSucreAppRole, DbSucreAppRole>();
                builder.Services.AddScoped<IDbSucreAppUser, DbSucreAppUser>();
                builder.Services.AddScoped<IDbSucreAsPaz, DbSucreAsPaz>();
                builder.Services.AddScoped<IDbSucreCanal, DbSucreCanal>();
                builder.Services.AddScoped<IDbSucreCex, DbSucreCex>();
                builder.Services.AddScoped<IDbSucreEnergy, DbSucreEnergy>();
                builder.Services.AddScoped<IDbSucreGroupUser, DbSucreGroupUser>();
                builder.Services.AddScoped<IDbSucreParameterType, DbSucreParameterType>();
                builder.Services.AddScoped<IDbSucrePoint, DbSucrePoint>();
                builder.Services.AddScoped<ISucreUnitOfWork, SucreUnitOfWork>();

                //подключаемые сервисы из BLL
                builder.Services.AddScoped<IAsPazService, AsPazService>();
                builder.Services.AddScoped<ICanalService, CanalService>();
                builder.Services.AddScoped<ICexService, CexService>();
                builder.Services.AddScoped<IDeviceTagService, DeviceTagService>();
                builder.Services.AddScoped<IEnergyService, EnergyService>();
                builder.Services.AddScoped<IMeasurementService, MeasurementService>();
                builder.Services.AddScoped<IParameterTypeService, ParameterTypeService>();
                builder.Services.AddScoped<IPointService, PointService>();
                builder.Services.AddScoped<IRoleService, RoleService>();
                builder.Services.AddScoped<IUserService, UserService>();                
                
                //Intitial BD and begin values
                builder.Services.AddScoped<InitApplicattionDbContext>();

                //builder.Services.AddScoped<HttpContext>();

                //builder.Services.AddScoped<IConfigurationSection>();

                //for filters подключение сервисов для фильтров
                //builder.Services.AddScoped<ResourceFilterAsyncLog>();
                builder.Services.AddScoped<ResourceFilterAsyncLog>();

                //Mappers
                builder.Services.AddScoped<AppRoleMapper>();
                builder.Services.AddScoped<AppUserMapper>();
                builder.Services.AddScoped<AsPazMapper>();
                builder.Services.AddScoped<CanalMapper>();
                builder.Services.AddScoped<CexMapper>();
                builder.Services.AddScoped<DeviceMapper>();
                builder.Services.AddScoped<EnergyMapper>();
                builder.Services.AddScoped<ParameterTypeMapper>();                
                builder.Services.AddScoped<PointMapper>();
                builder.Services.AddScoped<ValueMapper>();
                
                //Add Mediator service
                builder.Services.AddMediatR(cfg =>
                {
                    cfg.RegisterServicesFromAssembly(typeof(AddEnergyCommand).Assembly);
                    //cfg.RegisterServicesFromAssembly(typeof(GetCexPointsByIdQuery).Assembly);
                });

                // Add Hangfire services.
                builder.Services.AddHangfire(config => config
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));

                // Add the processing server as IHostedService
                builder.Services.AddHangfireServer();


                //logger.Information($"*->Services register successfully ({builder.ToString()})");
                LoggerExternal.LoggerEx.Information($"*->Services register successfully ({builder.ToString()})");

                var app = builder.Build();

                //logger.Information($"*->App create succesfully ({app.ToString()})");
                LoggerExternal.LoggerEx.Information($"*->App create succesfully ({app.ToString()})");

                // Configure the HTTP request pipeline.
                if (!app.Environment.IsDevelopment())
                {
                    app.UseExceptionHandler("/Home/Error");
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                }

                app.UseHttpsRedirection();
                app.UseStaticFiles();

                app.UseRouting();

                app.UseAuthentication();        
                app.UseAuthorization();

                //use hangfire
                app.UseHangfireDashboard();

                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                //logger.Information($"*->App run ({app.Environment.EnvironmentName})");
                LoggerExternal.LoggerEx.Information($"*->App run ({app.Environment.EnvironmentName})");
                app.Run();

                //delete
                var t = TestGlobalFilterResourceValue.GlobalResourceIn;
                //logger.Information(t);

            }
            catch (Exception ex) 
            {
                //Log.Fatal(ex, "*->!!! FATAL ERROR. Programs-Main...fucked up");
                LoggerExternal.LoggerEx.Fatal(ex, "*->!!! FATAL ERROR. Programs-Main...fucked up");
            }
            finally 
            {               
                Log.CloseAndFlush();
                var t = TestGlobalFilterResourceValue.GlobalResourceIn;
                Console.WriteLine(t);
            }

            
        }
    }
}