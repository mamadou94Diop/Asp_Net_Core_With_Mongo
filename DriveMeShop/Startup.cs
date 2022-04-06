using System;
using AutoMapper;
using DriveMeShop.Entity;
using DriveMeShop.Mapper;
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
            services.AddApiVersioning();
            services.AddVersionedApiExplorer(config =>
              {
                  config.SubstituteApiVersionInUrl = true;
                  config.GroupNameFormat = "'v'VVV";

              }
            );
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


            services.AddTransient<IValidator<Model.V1.UnidentifiedCarModel>>(_ => new CarModelValidator<Model.V1.UnidentifiedCarModel>());
            services.AddTransient<IValidator<Model.V1.IdentifiedCarModel>>(_ => new CarModelValidator<Model.V1.IdentifiedCarModel>());
            services.AddTransient<IValidator<Model.V2.UnidentifiedCarModel>>(_ => new CarModelValidator<Model.V2.UnidentifiedCarModel>());
            services.AddTransient<IValidator<Model.V2.IdentifiedCarModel>>(_ => new CarModelValidator<Model.V2.IdentifiedCarModel>());



            var mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile(new CarProfile());
            });

            var mapper = mapperConfiguration.CreateMapper();

            services.AddSingleton(mapper);
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
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "DriveMeShop API");
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
