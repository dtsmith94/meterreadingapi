using System;
using MeterReadingsApi.Data.Context;
using MeterReadingsApi.Data.Repositories;
using MeterReadingsApi.Interface.Data.Repositories;
using MeterReadingsApi.Interface.Service.Services;
using MeterReadingsApi.Service.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace MeterReadingsApi.Web
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MeterReadingsApi.Web", Version = "v1" });
            });

            // register the MeterReadingsContext and point at a SQLite db for the purpose of this exercise
            services.AddDbContext<MeterReadingsContext>(options =>
            {
                var dbPath = $"{Environment.CurrentDirectory}{System.IO.Path.DirectorySeparatorChar}MeterReadingsApi.db";
                options.UseSqlite($"Data Source={dbPath}");
            });

            services.AddScoped<ICustomerAccountRepository, CustomerAccountRepository>();
            services.AddScoped<IMeterReadingRepository, MeterReadingRepository>();
            services.AddScoped<ICustomerAccountService, CustomerAccountService>();
            services.AddScoped<IMeterReadingService, MeterReadingService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MeterReadingsApi.Web v1"));
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
