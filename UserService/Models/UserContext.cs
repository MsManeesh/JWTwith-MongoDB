using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
namespace UserService.Models
{
    public class UserContext
    {
        //declare variables to connect to MongoDB database
        static IMongoDatabase database;
        MongoClient mongoClient;
        public UserContext(IConfiguration configuration)
        {
            //Initialize MongoClient and Database using connection string and database name from configuration
            mongoClient = new MongoClient(configuration.GetSection("MongoDB:ConnectionString").Value);
            database = mongoClient.GetDatabase(configuration.GetSection("MongoDB:UserDatabase").Value);
        }
        //Define a MongoCollection to represent the Users collection of MongoDB based on UserProfile type
        public IMongoCollection<UserProfile> Users => database.GetCollection<UserProfile>("User");
    }
}
