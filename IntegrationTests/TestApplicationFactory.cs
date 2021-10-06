using System;
using DriveMeShop;
using DriveMeShop.Repository;
using DriveMeShop.Repository.implementation;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests
{
    public class TestApplicationFactory: WebApplicationFactory<Startup>
    {
        private InMemoryDB inMemoryDB;
        public TestApplicationFactory()
        {
            inMemoryDB = new InMemoryDB();
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureTestServices(services =>
            {
                var carCollection = inMemoryDB.GetCarCollection();
                var carRepository = new CarRepository(carCollection);

                services.AddTransient<ICarRepository>(_ => carRepository);
            });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if(inMemoryDB != null)
            {
                inMemoryDB.Dispose();
            }
        }


    }
}
