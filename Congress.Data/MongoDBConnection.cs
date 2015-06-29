using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using Congress.Data.Properties;
using Congress.Data.Models;

namespace Congress.Data
{
    public class MongoDBConnection
    {
        public static MongoDatabase Database;

        public MongoDBConnection()
        {
            var client = new MongoClient(Settings.Default.MongoDBConnectionString);
            var server = client.GetServer();
            Database = server.GetDatabase(Settings.Default.MongoDBDatabaseName);
        }

        public MongoCollection<Post> Posts
        {
            get
            {
                return Database.GetCollection<Post>("posts");
            }
        }

        public MongoCollection<Vote> Votes
        {
            get
            {
                return Database.GetCollection<Vote>("votes");
            }
        }

        public MongoCollection<Comment> Comments
        {
            get
            {
                return Database.GetCollection<Comment>("comments");
            }
        }
    }
}