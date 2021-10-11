using System;
using DriveMeShop.Entity;
using Mongo2Go;
using MongoDB.Driver;

namespace IntegrationTests
{
    public class InMemoryDB: IDisposable
    {
        private readonly MongoDbRunner mongoDbRunner;
        private MongoClient client;
        private IMongoDatabase database;

        private const string DATABASE_NAME = "test_db";
        private const string CAR_COLLECTION_NAME = "cars";

        public InMemoryDB()
        {
            mongoDbRunner = MongoDbRunner.Start(singleNodeReplSet: false);

            client = new MongoClient(mongoDbRunner.ConnectionString);
            database = client.GetDatabase(DATABASE_NAME);
            database.CreateCollection(CAR_COLLECTION_NAME);

            var collection = GetCarCollection();
            collection.InsertManyAsync(DataSet.GetData()).Wait();

        }

        public IMongoCollection<Car> GetCarCollection()
        {
            return database.GetCollection<Car>(CAR_COLLECTION_NAME);

        }

        public void Dispose()
        {
            mongoDbRunner.Dispose();
        }
    }
}
