using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CC_Proj1_Alireza.Domains
{
    public class MovieEntity
    {
        public string Id { get; set; }
        public int Year { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }


    }
}