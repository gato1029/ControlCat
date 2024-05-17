using BTCat.Generico;

using MongoDB.Bson;
using MongoDB.Driver;
using OrganizadorCat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace OrganizadorCat.Repositorios
{
    class UsuarioRepository
    {
        private readonly IMongoCollection<Usuario> _usuariosCollection;

        public UsuarioRepository(MongoDBContext dbContext)
        {
            _usuariosCollection = dbContext.Usuarios;

            // Crear índice único en el campo Correo
            var correoIndexModel = new CreateIndexModel<Usuario>(Builders<Usuario>.IndexKeys.Ascending(u => u.Correo), new CreateIndexOptions { Unique = true });
            _usuariosCollection.Indexes.CreateOne(correoIndexModel);
        }

        public bool InsertUsuario(Usuario usuario)
        {
            MessageBoxCat msg = new MessageBoxCat();
            try
            {
                usuario.Equipos = null;
                _usuariosCollection.InsertOne(usuario);
               
                msg.Mensaje("Usuario insertado correctamente.");
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
                    msg.Mensaje($"Error al insertar usuario: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                msg.Mensaje($"Error al insertar usuario: {ex.Message}");
            }
            return false;
        }

        public List<Usuario> GetAllUsuarios()
        {
            return _usuariosCollection.Find(new BsonDocument()).ToList();
        }

        public Usuario GetUsuarioById(string id)
        {
            var objectId = new ObjectId(id);
            var usuario = _usuariosCollection.Find<Usuario>(u => u.Id == objectId).FirstOrDefault();        
            return usuario;
        }
        public List<Usuario> GetUsuariosPorPagina(int paginaNumero, int elementosPorPagina)
        {
            return _usuariosCollection.Find(new BsonDocument())
                                      .Skip((paginaNumero - 1) * elementosPorPagina)
                                      .Limit(elementosPorPagina)
                                      .ToList();
        }
        public bool UpdateUsuario(string id, Usuario usuario)
        {
            MessageBoxCat msg = new MessageBoxCat();
            try
            {
                usuario.Equipos = null;
                var objectId = new ObjectId(id);
                _usuariosCollection.ReplaceOne(u => u.Id == objectId, usuario);
                msg.Mensaje("Usuario actualizado correctamente.");
                return true;
            }
            catch (MongoWriteException ex)
            {

                msg.Mensaje($"Error al actualizar usuario: {ex.Message}");
            }
            catch (Exception ex)
            {
                msg.Mensaje($"Error al actualizar usuario: {ex.Message}");
            }
            return false;

        }

        public void DeleteUsuario(string id)
        {
            var objectId = new ObjectId(id);
            _usuariosCollection.DeleteOne(u => u.Id == objectId);
            Console.WriteLine("Usuario eliminado correctamente.");
        }
        public bool DeleteUsuario(Usuario usuario)
        {
            MessageBoxCat msg = new MessageBoxCat();
            try
            {
                DeleteResult result = _usuariosCollection.DeleteOne(u => u.Id == usuario.Id);
                msg.Mensaje("Usuario eliminado correctamente.");
                return true;
            }
            catch (MongoWriteException ex)
            {

                msg.Mensaje($"Error al eliminar usuario: {ex.Message}");
               
            }
            catch (Exception ex)
            {
                msg.Mensaje($"Error al eliminar usuario: {ex.Message}");
            }
            return false;               
        }
    }
}
