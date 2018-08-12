using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using mongodb_mvc.Models;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace mongodb_mvc
{
    public class connect
    {
        public static IMongoQuery query;
        public static MongoDatabase mongodb;
       
       public connect()
        {
            MongoClientSettings settings = new MongoClientSettings();
            settings.Server = new MongoServerAddress("localhost", 27017);
            var client = new MongoClient(settings);
            var server = client.GetServer();
            mongodb = server.GetDatabase("test");
        }
       
    }
}