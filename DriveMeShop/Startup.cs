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
using MongoDB.Bson;
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

            var databaseConnection = Configuration.GetSection("DatabaseConnection");
            var databaseName = Configuration.GetSection("DatabaseName");
            var collectionName = Configuration.GetSection("CarCollectionName");

            var mongoClient = new MongoClient(databaseConnection.Value);
            var database = mongoClient.GetDatabase(databaseName.Value);
            var collection = database.GetCollection<Car>(collectionName.Value);

            var carRepository = new CarRepository(collection);

            services.AddTransient<ICarRepository>(_ => carRepository);
            services.AddTransient<IValidator<CarModel>>(_ => new CarModelValidator());
            


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
