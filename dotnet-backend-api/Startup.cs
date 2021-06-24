using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_backend_api.Services.MarvelService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using AutoMapper;
using dotnet_backend_api.Data;
using Microsoft.EntityFrameworkCore;

namespace dotnet_backend_api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           
            services.AddDbContext<DataContext>( options =>
            options.UseSqlServer(GetDbConnectionString())
           );
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "dotnet_backend_api", Version = "v1" });
            });
            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<IMarvelService, MarvelService>();
        }

        private String GetDbConnectionString()
        {
            String hostNameKey = Configuration.GetSection("ConnectionStrings").GetSection("HOSTNAME_KEY").Value;
            String hostNameValue = Configuration.GetSection("ConnectionStrings").GetSection("HOSTNAME_VALUE").Value;
            String port = Configuration.GetSection("ConnectionStrings").GetSection("PORT").Value;
            String databaseKey = Configuration.GetSection("ConnectionStrings").GetSection("DatabaseKey").Value;
            String databaseValue = Configuration.GetSection("ConnectionStrings").GetSection("DatabaseName").Value;
            String userIDKey = Configuration.GetSection("ConnectionStrings").GetSection("UserIDKey").Value;
            String userIdValue = Configuration.GetSection("ConnectionStrings").GetSection("UserIdValue").Value;
            String passwordKey = Configuration.GetSection("ConnectionStrings").GetSection("PasswordKey").Value;
            String passwordValue = Configuration.GetSection("ConnectionStrings").GetSection("PasswordValue").Value;
            String trustedConnectionKey = Configuration.GetSection("ConnectionStrings").GetSection("TrustedConnectionKey").Value;
            String trustedConnectionValue = Configuration.GetSection("ConnectionStrings").GetSection("TrustedConnectionValue").Value;

            //"DefaultConnection": "Server=localhost,1433; Database=dotnet-marvel; User Id=sa ; Password=reallyStrongPwd123; Trusted_Connection=false;",

            // String connnection = hostNameKey + "=" + hostNameValue + "," + port + ";" + databaseKey + "=" + databaseValue + ";" + userIDKey + "=" + userIdValue + ";" + passwordKey + "=" + passwordValue + ";" + trustedConnectionKey + "=" + trustedConnectionValue + ";";
            String connnection = Configuration.GetConnectionString("DefaultConnection");
            Console.WriteLine(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
            Console.WriteLine(connnection);
            return connnection;
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "dotnet_backend_api v1"));
            } else
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "dotnet_backend_api v1"));
            }

        app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
