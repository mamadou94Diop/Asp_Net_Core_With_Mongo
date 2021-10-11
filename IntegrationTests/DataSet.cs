using System;
using System.Collections.Generic;
using DriveMeShop.Entity;

namespace IntegrationTests
{
    public class DataSet
    {
        public static List<Car> GetData()
        {
            return new List<Car>
            {
                new Car
                {
                    Id = "6168c06d89af83d580f6e01e",
                    Make = "BMW",
                    Model = "X6",
                    TransmissionMode ="AUTOMATIC",
                    ReleasedYear = 2016,
                    LastRevisionYear = 2018,
                    Mileage = 60000 
                }
            };
        }
    }
}
