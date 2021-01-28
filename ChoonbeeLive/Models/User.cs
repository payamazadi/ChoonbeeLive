using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;

namespace ChoonbeeLive.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public List<Participant> Participants { get; set; }

        public User(string username)
        {
            Id = username;
        }


    }
}