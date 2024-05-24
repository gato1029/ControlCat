using BTCat.Generico;
using MongoDB.Bson;
using MongoDB.Driver;
using OrganizadorCat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizadorCat.Repositorios
{
     class ProyectoRepository
    {
        private readonly IMongoCollection<Proyecto> _proyectoCollection;

        public ProyectoRepository(MongoDBContext dbContext)
        {
            _proyectoCollection = dbContext.Proyectos;
        }

        public bool InsertProyecto(Proyecto proyecto)
        {
            MessageBoxCat msg = new MessageBoxCat();
            try
            {
                _proyectoCollection.InsertOne(proyecto);
                msg.Mensaje("Proyecto insertado correctamente.");
                return true;
            }
            catch (MongoWriteException ex)
            {
                if (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
                {
                    msg.Mensaje("Error: El Proyecto electrónico ya existe en la base de datos.");
                }
                else
                {
                    msg.Mensaje($"Error al insertar Proyecto: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                msg.Mensaje($"Error al insertar Proyecto: {ex.Message}");
            }
            return false;
        }

        public List<Proyecto> GetAllProyectos()
        {
            return _proyectoCollection.Find(new BsonDocument()).ToList();
        }
        public List<Proyecto> GetProyectosByEquipo(Equipo equipo)
        {
            var builder = Builders<Proyecto>.Filter;
            var filter = builder.Eq(f => f.Equipo.Id,equipo.Id);
            return _proyectoCollection.Find(filter).ToList();
        }
        public Proyecto GetProyectoById(string id)
        {
            var objectId = new ObjectId(id);
            return _proyectoCollection.Find<Proyecto>(u => u.Id == objectId).FirstOrDefault();
        }
        public List<Proyecto> GetProyectosPorPagina(int paginaNumero, int elementosPorPagina)
        {
            return _proyectoCollection.Find(new BsonDocument())
                                      .Skip((paginaNumero - 1) * elementosPorPagina)
                                      .Limit(elementosPorPagina)
                                      .ToList();
        }
        public bool UpdateProyecto(string id, Proyecto proyecto)
        {
            MessageBoxCat msg = new MessageBoxCat();
            try
            {
                proyecto.Equipo.Integrantes = null;
                var objectId = new ObjectId(id);
                _proyectoCollection.ReplaceOne(u => u.Id == objectId, proyecto);
                msg.Mensaje("Proyecto actualizado correctamente.");
                return true;
            }
            catch (MongoWriteException ex)
            {

                msg.Mensaje($"Error al actualizar proyecto: {ex.Message}");
            }
            catch (Exception ex)
            {
                msg.Mensaje($"Error al actualizar proyecto: {ex.Message}");
            }
            return false;

        }

        public void DeleteProyecto(string id)
        {
            var objectId = new ObjectId(id);
            _proyectoCollection.DeleteOne(u => u.Id == objectId);
            Console.WriteLine("Proyecto eliminado correctamente.");
        }
        public bool DeleteProyecto(Proyecto proyecto)
        {
            MessageBoxCat msg = new MessageBoxCat();
            try
            {
                DeleteResult result = _proyectoCollection.DeleteOne(u => u.Id == proyecto.Id);
                msg.Mensaje("Proyecto eliminado correctamente.");
                return true;
            }
            catch (MongoWriteException ex)
            {

                msg.Mensaje($"Error al eliminar proyecto: {ex.Message}");

            }
            catch (Exception ex)
            {
                msg.Mensaje($"Error al eliminar proyecto: {ex.Message}");
            }
            return false;
        }
    }
}
