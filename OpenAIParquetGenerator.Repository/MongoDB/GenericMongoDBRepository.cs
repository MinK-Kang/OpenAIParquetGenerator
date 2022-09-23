using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using OpenAIParquetGenerator.Domain;

namespace OpenAIParquetGenerator.Repository.MongoDB
{
    public class GenericMongoDBRepository<T> : IRepository<T>
    {
        protected readonly IMongoDatabase MetadataDatabase;
        protected readonly IMongoCollection<T> CurrentCollection;

        public GenericMongoDBRepository(
            string connectionString,
            string dbName,
            string collectionName)
        {
            var settings = MongoClientSettings.FromConnectionString(connectionString);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            BsonClassMap.RegisterClassMap<T>();

            var client = new MongoClient(settings);
            MetadataDatabase = client.GetDatabase(dbName);
            CurrentCollection = MetadataDatabase.GetCollection<T>(collectionName);
        }

        public bool Delete()
        {
            throw new NotImplementedException();
        }

        public bool Get()
        {
            throw new NotImplementedException();
        }

        public bool Insert(T objToSave)
        {
            throw new NotImplementedException();
        }

        public bool Update(T objToSave)
        {
            throw new NotImplementedException();
        }
    }
}