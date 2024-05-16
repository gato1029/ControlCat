using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using OrganizadorCat.Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.Windows.Shared;

namespace OrganizadorCat.Models
{
    public class Asignacion : IValidatorModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public Proyecto Proyecto { get; set; } 
        public Usuario Usuario { get; set; }
        
        public int Horas { get; set; }
        public Date FechaInicio { get; set; }
        public Date FechaFin { get; set; }

        public bool Validar()
        {
            throw new NotImplementedException();
        }
    }
}
