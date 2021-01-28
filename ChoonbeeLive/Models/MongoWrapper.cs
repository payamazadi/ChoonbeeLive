using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using MongoDB.Shared;
using MongoDB.Driver.Linq;

namespace ChoonbeeLive.Models
{
    public static class MongoWrapper
    {
        private static MongoClient client = new MongoClient(System.Configuration.ConfigurationManager.ConnectionStrings["league_pimaEntitiesMongo"].ToString());
        private static MongoServer server = client.GetServer();
        public static MongoDatabase db = server.GetDatabase("choonbeelive");

    }
}