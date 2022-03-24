using System;
using System.Reflection;
using DriveMeShop.Entity;
using DriveMeShop.Model;
using DriveMeShop.Repository;
using DriveMeShop.Repository.implementation;
using DriveMeShop.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace DriveMeShop
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
            services.AddMvc().AddFluentValidation();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Description = "Documentation for DriveMeShop API",
                    Title = "DriveMeShop API",
                    Version = "V1.0",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact { 
                    
                         Name = "Mamadou Diop",
                         Email = "mamadou-abdoulaye.diop@hotmail.com"
                    }
                });
                var rootPath = AppContext.BaseDirectory;
                var xmlPath = "DriveMeShop.xml";
                options.IncludeXmlComments(rootPath + xmlPath, true);

            });

            var databaseConnection = Configuration.GetSection("DatabaseConnection");
            var databaseName = Configuration.GetSection("DatabaseName");
            var collectionName = Configuration.GetSection("CarCollectionName");

            var mongoClient = new MongoClient(databaseConnection.Value);
            var database = mongoClient.GetDatabase(databaseName.Value);
            var collection = database.GetCollection<Car>(collectionName.Value);

            var carRepository = new CarRepository(collection);

            services.AddTransient<ICarRepository>(_ => carRepository);

            services.AddTransient<IValidator<UnidentifiedCarModel>>(_ => new CarModelValidator<UnidentifiedCarModel>());
            services.AddTransient<IValidator<IdentifiedCarModel>>(_ => new CarModelValidator<IdentifiedCarModel>());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("v1/swagger.json", "DriveMeShop API");
                });
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
