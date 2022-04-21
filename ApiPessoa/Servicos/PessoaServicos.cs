using ApiPessoa.Util;
using Models;
using MongoDB.Driver;
using System.Collections.Generic;

namespace ApiPessoa.Servicos
{
    public class PessoaServicos
    {
        private readonly IMongoCollection<Pessoa> _pessoa;

        public PessoaServicos(IPessoaDatabase settings)
        {
            var pessoa = new MongoClient(settings.ConnectionString);
            var database = pessoa.GetDatabase(settings.DatabaseName);
            _pessoa = database.GetCollection<Pessoa>(settings.PessoaCollectionName);
        }

        public List<Pessoa> Get() =>
            _pessoa.Find(pessoa => true).ToList();

        public Pessoa Get(string id) =>
            _pessoa.Find(pessoa => pessoa.Id == id).FirstOrDefault();

        public Pessoa GetNome(string nome) =>
          _pessoa.Find(pessoa => pessoa.Nome.ToLower() == nome.ToLower()).FirstOrDefault();

        public Pessoa ChecarPessoa(string nome) =>
            _pessoa.Find(pessoa => pessoa.Nome.ToLower() == nome.ToLower()).FirstOrDefault();

        public Pessoa Create(Pessoa pessoa)
        {
            _pessoa.InsertOne(pessoa);
            return pessoa;
        }

        public void Update(string id, Pessoa upPessoa)
        {
            upPessoa.Id = id;
            _pessoa.ReplaceOne(pessoa => pessoa.Id == id, upPessoa);

        }

        public void Remove(string id) =>
            _pessoa.DeleteOne(pessoa => pessoa.Id == id);


    }
}
