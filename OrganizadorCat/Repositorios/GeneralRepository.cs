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
     class GeneralRepository
    {
        private readonly IMongoCollection<General> _generalsCollection;

        public GeneralRepository(MongoDBContext dbContext)
        {
            _generalsCollection = dbContext.General;

            // Crear índice único en el campo Correo
            //var correoIndexModel = new CreateIndexModel<General>(Builders<General>.IndexKeys.Ascending(u => u.Correo), new CreateIndexOptions { Unique = true });
            //_generalsCollection.Indexes.CreateOne(correoIndexModel);
        }

        public bool InsertGeneral(General general)
        {
            MessageBoxCat msg = new MessageBoxCat();
            try
            {
               
                _generalsCollection.InsertOne(general);

                msg.Mensaje("General insertado correctamente.");
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
                    msg.Mensaje($"Error al insertar general: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                msg.Mensaje($"Error al insertar general: {ex.Message}");
            }
            return false;
        }

        public List<General> GetAllGenerals()
        {
            return _generalsCollection.Find(new BsonDocument()).ToList();
        }

        public General GetGeneralById(string id)
        {
            var objectId = new ObjectId(id);
            var general = _generalsCollection.Find<General>(u => u.Id == objectId).FirstOrDefault();
            return general;
        }

        public General GetGeneralByAplicacion(string aplicacion)
        {

            var general = _generalsCollection.Find<General>(u => u.Aplicacion == aplicacion).FirstOrDefault();
            return general;
        }
        public List<General> GetGeneralsPorPagina(int paginaNumero, int elementosPorPagina)
        {
            return _generalsCollection.Find(new BsonDocument())
                                      .Skip((paginaNumero - 1) * elementosPorPagina)
                                      .Limit(elementosPorPagina)
                                      .ToList();
        }


        public bool UpdateGeneral(string id, General general, bool message = true)
        {
            if (message)
            {
                MessageBoxCat msg = new MessageBoxCat();
                try
                {                   
                    var objectId = new ObjectId(id);
                    _generalsCollection.ReplaceOne(u => u.Id == objectId, general);
                    msg.Mensaje("General actualizado correctamente.");
                    return true;
                }
                catch (MongoWriteException ex)
                {

                    msg.Mensaje($"Error al actualizar general: {ex.Message}");
                }
                catch (Exception ex)
                {
                    msg.Mensaje($"Error al actualizar general: {ex.Message}");
                }
                return false;
            }
            else
            {
                
                var objectId = new ObjectId(id);
                _generalsCollection.ReplaceOne(u => u.Id == objectId, general);
                return true;
            }
        }

        public void DeleteGeneral(string id)
        {
            var objectId = new ObjectId(id);
            _generalsCollection.DeleteOne(u => u.Id == objectId);
            Console.WriteLine("General eliminado correctamente.");
        }
        public bool DeleteGeneral(General general)
        {
            MessageBoxCat msg = new MessageBoxCat();
            try
            {
                DeleteResult result = _generalsCollection.DeleteOne(u => u.Id == general.Id);
                msg.Mensaje("General eliminado correctamente.");
                return true;
            }
            catch (MongoWriteException ex)
            {

                msg.Mensaje($"Error al eliminar general: {ex.Message}");

            }
            catch (Exception ex)
            {
                msg.Mensaje($"Error al eliminar general: {ex.Message}");
            }
            return false;
        }
    }
}
