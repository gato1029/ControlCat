﻿using BTCat.Generico;
using MongoDB.Bson;
using MongoDB.Driver;
using OrganizadorCat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;

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
                proyecto.Equipo.Integrantes = null;
                proyecto.Equipo.PersonasCliente = null;
                proyecto.Equipo.IntegrantesPorcentajes = null;

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
        public List<Proyecto> GetProyectosByEquipo(Equipo equipo, DateTime fechaRecepcion)
        {
            var builder = Builders<Proyecto>.Filter;
            var filter = builder.Eq(f => f.Equipo.Id,equipo.Id)  & builder.Gte(f => f.FechaRecepcion, fechaRecepcion);
            return _proyectoCollection.Find(filter).ToList();
        }
        public List<Proyecto> GetProyectosByEquipoCerrado(Equipo equipo, DateTime fechaRecepcion,bool cerrado)
        {
            var builder = Builders<Proyecto>.Filter;
            var filter = builder.Eq(f => f.Equipo.Id, equipo.Id) & builder.Gte(f => f.FechaRecepcion, fechaRecepcion) & builder.Eq(f => f.Cerrado, cerrado);
            return _proyectoCollection.Find(filter).ToList();
        }
        public List<Proyecto> GetProyectosByEquipoAbiertos(Equipo equipo, bool cerrado)
        {
            var builder = Builders<Proyecto>.Filter;
            var filter = builder.Eq(f => f.Equipo.Id, equipo.Id)  & builder.Eq(f => f.Cerrado, cerrado);
            return _proyectoCollection.Find(filter).ToList();
        }
        public List<Proyecto> GetProyectosByEquipoExcluyendoEstado(Equipo equipo, string estadoExcluido)
        {
            var builder = Builders<Proyecto>.Filter;
            var filter = builder.Eq(f => f.Equipo.Id, equipo.Id) & builder.Ne(f => f.Estado, estadoExcluido);
            return _proyectoCollection.Find(filter).ToList();
        }
        public Proyecto GetProyectoById(string id)
        {
            var objectId = new ObjectId(id);
            return _proyectoCollection.Find<Proyecto>(u => u.Id == objectId).FirstOrDefault();
        }
        public Proyecto GetProyectoByCodigo(string codigo)
        {          
            return _proyectoCollection.Find<Proyecto>(u => u.Codigo == codigo).FirstOrDefault();
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
                proyecto.Equipo.PersonasCliente = null;
                proyecto.Equipo.IntegrantesPorcentajes = null;
                
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

        public bool UpdateProyectoCampo<TField>(ObjectId id, Expression<Func<Proyecto, TField>> campo, TField valor)
        {
            try
            {
                var filter = Builders<Proyecto>.Filter.Eq(p => p.Id, id);
                var update = Builders<Proyecto>.Update.Set(campo, valor);
                var result = _proyectoCollection.UpdateOne(filter, update);
                return true;
            }
            catch (Exception ex)
            {
                MessageBoxCat msg = new MessageBoxCat();
                msg.Mensaje($"Error al actualizar proyecto: {ex.Message}");
            }
            return false;
        }

        public TField ObtenerCampo<TField>(ObjectId id, Expression<Func<Proyecto, TField>> campo)
        {
            try
            {
                var filter = Builders<Proyecto>.Filter.Eq(p => p.Id, id);
                var projection = Builders<Proyecto>.Projection.Expression(campo);
                return _proyectoCollection.Find(filter).Project(projection).FirstOrDefault();
            }
            catch (Exception ex)
            {
                MessageBoxCat msg = new MessageBoxCat();
                msg.Mensaje($"Error al actualizar proyecto: {ex.Message}");
                return default;
            }
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
