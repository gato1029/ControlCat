using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OrganizadorCat.Contracts.ViewModels;
using OrganizadorCat.Helpers;
using Syncfusion.Windows.PropertyGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OrganizadorCat.Models
{


    public class Usuario: IValidatorModel
    {
      

        [BsonId]
        
        public ObjectId Id { get; set; }
        [BsonIgnoreIfNull]
        [BsonIgnoreIfDefault]
        public string Nombre { get; set; }
        [BsonIgnoreIfNull]
        [BsonIgnoreIfDefault]
        public string Correo { get; set; }

        [BsonIgnoreIfNull]
        [BsonIgnoreIfDefault]
        public string Password { get; set; }
        [BsonIgnoreIfNull]
        [BsonIgnoreIfDefault]
        public List<string> Privilegios { get; set; }

        [BsonIgnoreIfNull]
        [BsonIgnoreIfDefault]
        public List<Equipo> Equipos { get; set; }
        
        [BsonIgnore]
        public Brush BackgroundColor { get; set; }
        [BsonIgnore]
        public Brush ForegroundColor { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otherUsuario = (Usuario)obj;
            return Id == otherUsuario.Id;               
        }
        public Usuario()
        {          
        }
        public Usuario(ObjectId id)
        {
            Id = id;
        }
        public bool Validar()
        {
            if (!Utilitarios.IsValidField(Correo, @"^[A-Za-z0-9._%-]+@[A-Za-z0-9]+.[A-Za-z]{2,3}$"))
            {
                return false;
            }
            if (!Utilitarios.IsValidField(Nombre, ""))
            {
                return false;
            }            
            return true;
        }
        public override string ToString()
        {
            return Nombre;
        }
    }
}
