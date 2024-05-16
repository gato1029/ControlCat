using BTCat.Generico;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;
using OrganizadorCat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OrganizadorCat.Repositorios
{
     class EquipoRepository
    {
        private readonly IMongoCollection<Equipo> _equipoCollection;

        public EquipoRepository(MongoDBContext dbContext)
        {
            _equipoCollection = dbContext.Equipos;
        }

        public bool InsertEquipo(Equipo equipo)
        {
            MessageBoxCat msg = new MessageBoxCat();
            try
            {
                foreach (var usuario in equipo.Integrantes)
                {                   
                    usuario.Password = null;
                    usuario.Privilegios = null;               
                }
                _equipoCollection.InsertOne(equipo);

                msg.Mensaje("Equipo insertado correctamente.");
                return true;
            }
            catch (MongoWriteException ex)
            {
                if (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
                {
                    msg.Mensaje("Error: El Equipo electrónico ya existe en la base de datos.");
                }
                else
                {
                    msg.Mensaje($"Error al insertar Equipo: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                msg.Mensaje($"Error al insertar Equipo: {ex.Message}");
            }
            return false;
        }

        public List<Equipo> GetAllEquipos()
        {
            return _equipoCollection.Find(new BsonDocument()).ToList();
        }

        public Equipo GetEquipoById(string id)
        {
            var objectId = new ObjectId(id);
            return _equipoCollection.Find<Equipo>(u => u.Id == objectId).FirstOrDefault();
        }
        public List<Equipo> GetEquiposPorPagina(int paginaNumero, int elementosPorPagina)
        {
            return _equipoCollection.Find(new BsonDocument())
                                      .Skip((paginaNumero - 1) * elementosPorPagina)
                                      .Limit(elementosPorPagina)
                                      .ToList();
        }
        public List<Equipo> GetEquiposPorUsuario(Usuario usuario)
        {   
            var builder = Builders<Equipo>.Filter;
            var filter = builder.AnyEq(f => f.Integrantes.Select(u=>u.Id), usuario.Id);
            return _equipoCollection.Find(filter).ToList();
        }
        public bool UpdateEquipo(string id, Equipo equipo)
        {
            MessageBoxCat msg = new MessageBoxCat();
            try
            {
                foreach (var usuario in equipo.Integrantes)
                {
                    usuario.Password = null;
                    usuario.Privilegios = null;
                }
                var objectId = new ObjectId(id);
                _equipoCollection.ReplaceOne(u => u.Id == objectId, equipo);
                msg.Mensaje("Equipo actualizado correctamente.");
                return true;
            }
            catch (MongoWriteException ex)
            {

                msg.Mensaje($"Error al actualizar equipo: {ex.Message}");
            }
            catch (Exception ex)
            {
                msg.Mensaje($"Error al actualizar equipo: {ex.Message}");
            }
            return false;

        }

        public void DeleteEquipo(string id)
        {
            var objectId = new ObjectId(id);
            _equipoCollection.DeleteOne(u => u.Id == objectId);
            Console.WriteLine("Equipo eliminado correctamente.");
        }
        public bool DeleteEquipo(Equipo equipo)
        {
            MessageBoxCat msg = new MessageBoxCat();
            try
            {
                DeleteResult result = _equipoCollection.DeleteOne(u => u.Id == equipo.Id);
                msg.Mensaje("Equipo eliminado correctamente.");
                return true;
            }
            catch (MongoWriteException ex)
            {

                msg.Mensaje($"Error al eliminar equipo: {ex.Message}");

            }
            catch (Exception ex)
            {
                msg.Mensaje($"Error al eliminar equipo: {ex.Message}");
            }
            return false;
        }
    }

    
}
