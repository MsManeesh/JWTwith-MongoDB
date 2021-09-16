using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
namespace ReminderService.Models
{
    public class ReminderContext
    {
        //declare variables to connect to MongoDB database
        MongoClient mongoClient;
        static IMongoDatabase database;
        public ReminderContext(IConfiguration configuration)
        {
            //Initialize MongoClient and Database using connection string and database name from configuration
            mongoClient = new MongoClient(configuration.GetSection("MongoDB:ConnectionString").Value);
            database = mongoClient.GetDatabase(configuration.GetSection("MongoDB:ReminderDatabase").Value);
        }
        //Define a MongoCollection to represent the Reminders collection of MongoDB based on Reminder type
        public IMongoCollection<Reminder> Reminders => database.GetCollection<Reminder>("Reminder");
    }
}
