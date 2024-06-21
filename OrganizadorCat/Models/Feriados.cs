using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrganizadorCat.Contracts.ViewModels;

namespace OrganizadorCat.Models
{
    public class Feriados : IValidatorModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonIgnoreIfNull]
        [BsonIgnoreIfDefault]

        public string Pais { get; set; }
        [BsonIgnoreIfNull]
        [BsonIgnoreIfDefault]
        public List<DateTime> Fechas { get; set; }
        public bool Validar()
        {
            throw new NotImplementedException();
        }
    }


    public class FechaFeriado
    {
        public FechaFeriado()
        {
        }

        public FechaFeriado(int mes, int dia)
        {
            Dia = dia;
            Mes = mes;
        }
        public int Mes { get; set; }
        public int Dia { get; set; }
      
    }
}
