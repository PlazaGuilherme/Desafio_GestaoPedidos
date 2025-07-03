using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.IO;

namespace Infrastructure
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext()
        {
            var basePath = Directory.GetCurrentDirectory();
            var configPath = Path.Combine(basePath, "appsettings.json");

            if (!File.Exists(configPath))
                throw new FileNotFoundException("Arquivo appsettings.json não encontrado no projeto de infrastructure.");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration["ReadDbSettings:DbConn"];
            var dbName = configuration["ReadDbSettings:DbName"];

            if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(dbName))
                throw new InvalidOperationException("Configuração do MongoDB não encontrada no appsettings.json.");

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(dbName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }
    }
}