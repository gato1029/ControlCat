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
    public class Persona : IValidatorModel, IComparable
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonIgnoreIfNull]
        [BsonIgnoreIfDefault]
        public string Nombre { get; set; }
        [BsonIgnoreIfNull]
        [BsonIgnoreIfDefault]
        public string Cliente { get; set; }
        [BsonIgnoreIfNull]
        [BsonIgnoreIfDefault]
        public string Correo { get; set; }

        public bool Validar()
        {
            return true;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otherUsuario = (Persona)obj;
            return Id == otherUsuario.Id;
        }
        public override string ToString()
        {
            return Nombre;
        }

        public int CompareTo(object? obj)
        {
            if (obj == null) return 1;
            Persona otherTemperature = obj as Persona;
            if (otherTemperature != null)
                return this.Nombre.CompareTo(otherTemperature.Nombre);
            else
                throw new ArgumentException("Object is not a person");
        }
    }
}
