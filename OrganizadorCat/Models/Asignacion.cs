using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using OrganizadorCat.Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.Windows.Shared;
using System.Windows.Media;
using System.Collections.ObjectModel;

namespace OrganizadorCat.Models
{
    public class Asignacion : IValidatorModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonIgnoreIfNull]
        [BsonIgnoreIfDefault]
        public Proyecto Proyecto { get; set; }
        public Equipo Equipo { get; set; }
        public Usuario Usuario { get; set; }
        
        public int Horas { get; set; }
        public bool DiaCompleto { get; set; }
        public bool Vacaciones { get; set; }
        public string Comentario { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public string ColorBarra { get; set; }

        [BsonIgnore]
        public Brush BackgroundColor { get; set; }
        [BsonIgnore]
        public Brush ForegroundColor { get; set; }

        [BsonIgnore]
        public  ObservableCollection<object> CalendarioScheduler { get; set; }

        [BsonIgnore]
        public string CalendarioNombre { get; set; }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (Asignacion)obj;
            return Id == other.Id;
        }
        public bool Validar()
        {
            throw new NotImplementedException();
        }
    }
}
