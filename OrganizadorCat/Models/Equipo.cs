using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrganizadorCat.Contracts.ViewModels;
using OrganizadorCat.Helpers;

namespace OrganizadorCat.Models
{
   

    public class Equipo : IValidatorModel
    {
        [BsonId]

        public ObjectId Id { get; set; }
        public string Nombre { get; set; }
        [BsonIgnoreIfNull]
        [BsonIgnoreIfDefault]
        public List<Usuario> Integrantes { get; set; }

        public bool Validar()
        {          
            if (!Utilitarios.IsValidField(Nombre, ""))
            {
                return false;
            }
            return true;
        }
    }
}
