using Hangfire;
using Sucre_Services;

namespace Sucre_WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("AFINN-ru.json");

            // Add services to the container.

            //SwaggerGen
            //builder.Services.AddSwaggerGen();            
            builder.Services.ConfigureSwaggerGen();

            //����������� ��������
            builder.Services.RegisterServices(builder.Configuration);
            //����������� �������� article
            //builder.Services.RegisterServicesWebApi(builder.Configuration);
            
            //���������������� JWT �����������
            builder.Services.ConfigureJwt(builder.Configuration);
            //���� ��������� � article
            //builder.Services.AddScoped<EnService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
                        
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            
            //use hangfire
            app.UseHangfireDashboard();

            app.MapControllers();

            app.Run();
        }
    }
}
