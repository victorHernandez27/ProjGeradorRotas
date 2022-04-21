using ApiUsuario.Util;
using Models;
using MongoDB.Driver;
using System.Collections.Generic;

namespace ApiUsuario.Servicos
{
    public class UsuarioServicos
    {
        private readonly IMongoCollection<Usuario> _usuario;

        public UsuarioServicos(IUsuarioDatabase settings)
        {
            var usuario = new MongoClient(settings.ConnectionString);
            var database = usuario.GetDatabase(settings.DatabaseName);
            _usuario = database.GetCollection<Usuario>(settings.UsuarioCollectionName);
        }

        public List<Usuario> Get() =>
            _usuario.Find(usuario => true).ToList();

        public Usuario Get(string id) =>
            _usuario.Find(usuario => usuario.Id == id).FirstOrDefault();

        public Usuario GetNome(string nome) =>
          _usuario.Find(usuario => usuario.NomeUsuario.ToLower() == nome.ToLower()).FirstOrDefault();

        public Usuario ChecarUsuario(string nome) =>
            _usuario.Find(usuario => usuario.NomeUsuario.ToLower() == nome.ToLower()).FirstOrDefault();

        public Usuario Create(Usuario usuario)
        {
            _usuario.InsertOne(usuario);
            return usuario;
        }

        public void Update(string id, Usuario upUsuario) =>
            _usuario.ReplaceOne(usuario => usuario.Id == id, upUsuario);

        public void Remove(string id) =>
            _usuario.DeleteOne(usuario => usuario.Id == id);


    }
}
