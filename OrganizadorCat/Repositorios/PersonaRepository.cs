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
    class PersonaRepository
    {
        private readonly IMongoCollection<Persona> _personaCollection;

        public PersonaRepository(MongoDBContext dbContext)
        {
            _personaCollection = dbContext.Personas;
        }

        public bool InsertPersona(Persona persona)
        {
            MessageBoxCat msg = new MessageBoxCat();
            try
            {              
                _personaCollection.InsertOne(persona);
                msg.Mensaje("Persona insertado correctamente.");
                return true;
            }           
            catch (Exception ex)
            {
                msg.Mensaje($"Error al insertar Persona: {ex.Message}");
            }
            return false;
        }

        public List<Persona> GetAllPersonas()
        {
            return _personaCollection.Find(new BsonDocument()).ToList();
        }

        public Persona GetPersonaById(string id)
        {
            var objectId = new ObjectId(id);
            return _personaCollection.Find<Persona>(u => u.Id == objectId).FirstOrDefault();
        }
        public List<Persona> GetPersonasPorPagina(int paginaNumero, int elementosPorPagina)
        {
            return _personaCollection.Find(new BsonDocument())
                                      .Skip((paginaNumero - 1) * elementosPorPagina)
                                      .Limit(elementosPorPagina)
                                      .ToList();
        }
    
        public bool UpdatePersona(string id, Persona persona)
        {
            MessageBoxCat msg = new MessageBoxCat();
            try
            {
            
                var objectId = new ObjectId(id);
                _personaCollection.ReplaceOne(u => u.Id == objectId, persona);
                msg.Mensaje("Persona actualizado correctamente.");
                return true;
            }
            catch (MongoWriteException ex)
            {

                msg.Mensaje($"Error al actualizar persona: {ex.Message}");
            }
            catch (Exception ex)
            {
                msg.Mensaje($"Error al actualizar persona: {ex.Message}");
            }
            return false;

        }

        public void DeletePersona(string id)
        {
            var objectId = new ObjectId(id);
            _personaCollection.DeleteOne(u => u.Id == objectId);
            Console.WriteLine("Persona eliminado correctamente.");
        }
        public bool DeletePersona(Persona persona)
        {
            MessageBoxCat msg = new MessageBoxCat();
            try
            {
                DeleteResult result = _personaCollection.DeleteOne(u => u.Id == persona.Id);
                msg.Mensaje("Persona eliminado correctamente.");
                return true;
            }
            catch (MongoWriteException ex)
            {

                msg.Mensaje($"Error al eliminar persona: {ex.Message}");

            }
            catch (Exception ex)
            {
                msg.Mensaje($"Error al eliminar persona: {ex.Message}");
            }
            return false;
        }
    }
}
