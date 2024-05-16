using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using OrganizadorCat.Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizadorCat.Models
{
    public class Persona : IValidatorModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Nombre { get; set; }

        public bool Validar()
        {
            return true;
        }
    }
}
