using Congress.Data;
using Congress.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Congress.App.Helpers
{
    public class Seeder
    {
        private static MongoDBConnection dbInstance;

        public Seeder(MongoDBConnection db)
        {
            dbInstance = db;
        }

        public void Posts(int amount = 10)
        {
            for (int i = 0; i < amount; i++)
            {
                PostShepherd.Create(dbInstance, new SetterPost(Helper.GenerateRandomString(20), Helper.GenerateRandomString(20), Helper.GenerateRandomString(10)));
            }
        }
    }
}