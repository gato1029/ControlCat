using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using OrganizadorCat.Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.Windows.Shared;
using OrganizadorCat.Helpers;

namespace OrganizadorCat.Models
{
    public class Proyecto : IValidatorModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Codigo { get; set; }
        public string Nombre { get; set; }
        
        [BsonIgnoreIfNull]
        [BsonIgnoreIfDefault]
        public int HorasEstimadas { get; set; }
        [BsonIgnoreIfNull]
        [BsonIgnoreIfDefault]
        public int HorasReales { get; set; }
        [BsonIgnoreIfNull]
        [BsonIgnoreIfDefault]
        public DateTime FechaInicio { get; set; }
        [BsonIgnoreIfNull]
        [BsonIgnoreIfDefault]
        public DateTime FechaFin { get; set; }

        [BsonIgnoreIfNull]
        [BsonIgnoreIfDefault]
        public Equipo Equipo { get; set; }
        [BsonIgnoreIfNull]
        [BsonIgnoreIfDefault]
        public Persona ClienteAsignado { get; set; }
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
