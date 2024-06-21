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
    class FeriadosRepository
    {
        private readonly IMongoCollection<Feriados> _feriadosCollection;

        public FeriadosRepository(MongoDBContext dbContext)
        {
            _feriadosCollection = dbContext.Feriados;

            // Crear índice único en el campo Correo
            //var correoIndexModel = new CreateIndexModel<Feriados>(Builders<Feriados>.IndexKeys.Ascending(u => u.Correo), new CreateIndexOptions { Unique = true });
            //_feriadosCollection.Indexes.CreateOne(correoIndexModel);
        }

        public bool InsertFeriados(Feriados feriado)
        {
            MessageBoxCat msg = new MessageBoxCat();
            try
            {
              
                _feriadosCollection.InsertOne(feriado);

                msg.Mensaje("Feriados insertado correctamente.");
                return true;
            }
            catch (MongoWriteException ex)
            {
                if (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
                {
                    msg.Mensaje("Error: El correo electrónico ya existe en la base de datos.");
                }
                else
                {
                    msg.Mensaje($"Error al insertar feriado: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                msg.Mensaje($"Error al insertar feriado: {ex.Message}");
            }
            return false;
        }

        public List<Feriados> GetAllFeriadoss()
        {
            return _feriadosCollection.Find(new BsonDocument()).ToList();
        }

        public Feriados GetFeriadosById(string id)
        {
            var objectId = new ObjectId(id);
            var feriado = _feriadosCollection.Find<Feriados>(u => u.Id == objectId).FirstOrDefault();
            return feriado;
        }
        public Feriados GetFeriadosByPais(string pais)
        {         
            var feriado = _feriadosCollection.Find<Feriados>(u => u.Pais == pais).FirstOrDefault();
            return feriado;
        }

        public List<Feriados> GetFeriadossPorPagina(int paginaNumero, int elementosPorPagina)
        {
            return _feriadosCollection.Find(new BsonDocument())
                                      .Skip((paginaNumero - 1) * elementosPorPagina)
                                      .Limit(elementosPorPagina)
                                      .ToList();
        }
        public bool UpdateFeriados(string id, Feriados feriado)
        {
            MessageBoxCat msg = new MessageBoxCat();
            try
            {
               
                var objectId = new ObjectId(id);
                _feriadosCollection.ReplaceOne(u => u.Id == objectId, feriado);
                msg.Mensaje("Feriados actualizado correctamente.");
                return true;
            }
            catch (MongoWriteException ex)
            {

                msg.Mensaje($"Error al actualizar feriado: {ex.Message}");
            }
            catch (Exception ex)
            {
                msg.Mensaje($"Error al actualizar feriado: {ex.Message}");
            }
            return false;

        }

        public void DeleteFeriados(string id)
        {
            var objectId = new ObjectId(id);
            _feriadosCollection.DeleteOne(u => u.Id == objectId);
            Console.WriteLine("Feriados eliminado correctamente.");
        }
        public bool DeleteFeriados(Feriados feriado)
        {
            MessageBoxCat msg = new MessageBoxCat();
            try
            {
                DeleteResult result = _feriadosCollection.DeleteOne(u => u.Id == feriado.Id);
                msg.Mensaje("Feriados eliminado correctamente.");
                return true;
            }
            catch (MongoWriteException ex)
            {

                msg.Mensaje($"Error al eliminar feriado: {ex.Message}");

            }
            catch (Exception ex)
            {
                msg.Mensaje($"Error al eliminar feriado: {ex.Message}");
            }
            return false;
        }
    }
}
