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
        [BsonIgnoreIfNull]
        [BsonIgnoreIfDefault]
        public string Codigo { get; set; }
        [BsonIgnoreIfNull]
        [BsonIgnoreIfDefault]
        public string Nombre { get; set; }
        
        [BsonIgnoreIfNull]
        [BsonIgnoreIfDefault]

        public string Estado { get; set; }

        [BsonIgnoreIfNull]
        [BsonIgnoreIfDefault]

        public string Comentario { get; set; }

        [BsonIgnoreIfNull]
        [BsonIgnoreIfDefault]
        public int HorasEstimadas { get; set; }
        [BsonIgnoreIfNull]
        [BsonIgnoreIfDefault]
        public int HorasReales { get; set; }
        [BsonIgnoreIfNull]
        [BsonIgnoreIfDefault]

        public string Area { get; set; }

        [BsonIgnoreIfNull]
        [BsonIgnoreIfDefault]
        public DateTime FechaRecepcion { get; set; }

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

        public Proyecto()
        { }
        public Proyecto(ObjectId objectId)
        {
            Id = objectId;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otherUsuario = (Proyecto)obj;
            return Id == otherUsuario.Id;
        }
        public bool Validar()
        {
            if (!Utilitarios.IsValidField(Nombre, ""))
            {
                return false;
            }            
            return true;
        }
        public override string ToString()
        {
            return Codigo+"-"+Nombre;
        }
    }
}
