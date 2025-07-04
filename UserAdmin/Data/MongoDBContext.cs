using MongoDB.Driver;
using UserAdmin.MongoEntities;

namespace UserAdmin.Data;
public class MongoDBContext
{
    private readonly IMongoDatabase _database;

    public MongoDBContext(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<Comment> Comments => _database.GetCollection<Comment>("comments");
}