using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace NewsService.Models
{
    public class NewsContext
    {
        //declare variables to connect to MongoDB database
        MongoClient mongoClient;
        static IMongoDatabase database;

        public NewsContext(IConfiguration configuration)
        {
            mongoClient = new MongoClient(configuration.GetSection("MongoDB:ConnectionString").Value);
            database = mongoClient.GetDatabase(configuration.GetSection("MongoDB:NewsDatabase").Value);
            //Initialize MongoClient and Database using connection string and database name from configuration
        }
        //Define a MongoCollection to represent the News collection of MongoDB based on UserNews type
        public IMongoCollection<UserNews> News => database.GetCollection<UserNews>("News");

    }
}
