using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizadorCat.Models
{
    public class General
    {
        [BsonId]
        public ObjectId Id { get; set; }
        
        public string Aplicacion { get; set; }
        public string Version { get; set; }
    }
}
