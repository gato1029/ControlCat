
using MongoDB.Driver;
using OrganizadorCat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTCat.Generico
{
    class MongoDBContext
    {
        private static MongoDBContext _instance;
        private readonly IMongoDatabase _database;

        private MongoDBContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public static MongoDBContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    // Configura aquí la cadena de conexión y el nombre de la base de datos
                    string connectionString = "mongodb://localhost:27017";
                    string databaseName = "BTCat";
                    _instance = new MongoDBContext(connectionString, databaseName);
                }
                return _instance;
            }
        }

        public IMongoCollection<Usuario> Usuarios
        {
            get { return _database.GetCollection<Usuario>("Usuario"); }
        }
        public IMongoCollection<Equipo> Equipos
        {
            get { return _database.GetCollection<Equipo>("Equipo"); }
        }
        public IMongoCollection<Proyecto> Proyectos
        {
            get { return _database.GetCollection<Proyecto>("Proyecto"); }
        }
        public IMongoCollection<Asignacion> Asignaciones
        {
            get { return _database.GetCollection<Asignacion>("Asignacion"); }
        }

    }
}
