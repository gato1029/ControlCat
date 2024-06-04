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
    internal class AsignacionRepository
    {
        private readonly IMongoCollection<Asignacion> _asignacionCollection;

        public AsignacionRepository(MongoDBContext dbContext)
        {
            _asignacionCollection = dbContext.Asignaciones;
        }

        public bool InsertAsignacion(Asignacion asignacion)
        {
            MessageBoxCat msg = new MessageBoxCat();
            try
            {
                Proyecto proyecto = null;
                if (!asignacion.Vacaciones)
                {
                    proyecto = new Proyecto(asignacion.Proyecto.Id);
                    proyecto.Codigo = asignacion.Proyecto.Codigo;
                    proyecto.Nombre = asignacion.Proyecto.Nombre;
                }
                                
                Equipo equipo = new Equipo(asignacion.Equipo.Id);
                equipo.Nombre = asignacion.Equipo.Nombre;                

                Usuario usuario = new Usuario(asignacion.Usuario.Id);
                usuario.Nombre = asignacion.Usuario.Nombre;                                
                asignacion.Usuario = usuario;
                asignacion.Equipo  = equipo;
                asignacion.Proyecto = proyecto;
                asignacion.DiaCompleto = true;
                _asignacionCollection.InsertOne(asignacion);

                msg.Mensaje("Asignacion insertado correctamente.");
                return true;
            }
            catch (MongoWriteException ex)
            {
                if (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
                {
                    msg.Mensaje("Error: El Asignacion electrónico ya existe en la base de datos.");
                }
                else
                {
                    msg.Mensaje($"Error al insertar Asignacion: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                msg.Mensaje($"Error al insertar Asignacion: {ex.Message}");
            }
            return false;
        }

        public List<Asignacion> GetAllAsignaciones()
        {
            return _asignacionCollection.Find(new BsonDocument()).ToList();
        }

        public Asignacion GetAsignacionById(string id)
        {
            var objectId = new ObjectId(id);
            return _asignacionCollection.Find<Asignacion>(u => u.Id == objectId).FirstOrDefault();
        }
        public List<Asignacion> GetAsignacionsPorPagina(int paginaNumero, int elementosPorPagina)
        {
            return _asignacionCollection.Find(new BsonDocument())
                                      .Skip((paginaNumero - 1) * elementosPorPagina)
                                      .Limit(elementosPorPagina)
                                      .ToList();
        }
        public List<Asignacion> GetAsignacionesPorUsuario(Usuario usuario)
        {
            var builder = Builders<Asignacion>.Filter;
            var filter = builder.Eq(f => f.Usuario.Id, usuario.Id);
            return _asignacionCollection.Find(filter).ToList();
        }
        public List<Asignacion> GetAsignacionesPorProyecto(Proyecto proyecto)
        {
            var builder = Builders<Asignacion>.Filter;
            var filter = builder.Eq(f => f.Proyecto.Id, proyecto.Id);
            return _asignacionCollection.Find(filter).ToList();
        }
        public List<Asignacion> GetAsignacionesPorEquipo(Equipo equipo)
        {
            var builder = Builders<Asignacion>.Filter;
            var filter = builder.Eq(f => f.Equipo.Id, equipo.Id);
            return _asignacionCollection.Find(filter).ToList();
        }
        public bool UpdateAsignacion(string id, Asignacion asignacion)
        {
            MessageBoxCat msg = new MessageBoxCat();
            try
            {
                Proyecto proyecto = null;
                if (!asignacion.Vacaciones)
                {
                    proyecto = new Proyecto(asignacion.Proyecto.Id);
                    proyecto.Codigo = asignacion.Proyecto.Codigo;
                    proyecto.Nombre = asignacion.Proyecto.Nombre;
                }

                Equipo equipo = new Equipo(asignacion.Equipo.Id);
                equipo.Nombre = asignacion.Equipo.Nombre;

                Usuario usuario = new Usuario(asignacion.Usuario.Id);
                usuario.Nombre = asignacion.Usuario.Nombre;
                asignacion.Usuario = usuario;
                asignacion.Equipo = equipo;
                asignacion.Proyecto = proyecto;
                asignacion.DiaCompleto = true;

                var objectId = new ObjectId(id);
                _asignacionCollection.ReplaceOne(u => u.Id == objectId, asignacion);
                msg.Mensaje("Asignacion actualizado correctamente.");
                return true;
            }
            catch (MongoWriteException ex)
            {

                msg.Mensaje($"Error al actualizar asignacion: {ex.Message}");
            }
            catch (Exception ex)
            {
                msg.Mensaje($"Error al actualizar asignacion: {ex.Message}");
            }
            return false;

        }

        public void DeleteAsignacion(string id)
        {
            var objectId = new ObjectId(id);
            _asignacionCollection.DeleteOne(u => u.Id == objectId);
            Console.WriteLine("Asignacion eliminado correctamente.");
        }
        public bool DeleteAsignacion(Asignacion asignacion)
        {
            MessageBoxCat msg = new MessageBoxCat();
            try
            {
                DeleteResult result = _asignacionCollection.DeleteOne(u => u.Id == asignacion.Id);
                msg.Mensaje("Asignacion eliminado correctamente.");
                return true;
            }
            catch (MongoWriteException ex)
            {

                msg.Mensaje($"Error al eliminar asignacion: {ex.Message}");

            }
            catch (Exception ex)
            {
                msg.Mensaje($"Error al eliminar asignacion: {ex.Message}");
            }
            return false;
        }
    }
}
