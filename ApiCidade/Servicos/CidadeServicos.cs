using ApiCidade.Util;
using Models;
using MongoDB.Driver;
using System.Collections.Generic;

namespace ApiCidade.Servicos
{
    public class CidadeServicos
    {
        private readonly IMongoCollection<Cidade> _cidade;

        public CidadeServicos(ICidadeDatabase settings)
        {
            var cidade = new MongoClient(settings.ConnectionString);
            var database = cidade.GetDatabase(settings.DatabaseName);
            _cidade = database.GetCollection<Cidade>(settings.CidadeCollectionName);
        }

        public List<Cidade> Get() =>
            _cidade.Find(cidade => true).ToList();

        public Cidade Get(string id) =>
            _cidade.Find(cidade => cidade.Id == id).FirstOrDefault();

        public Cidade GetNome(string nome) =>
           _cidade.Find(cidade => cidade.Nome.ToLower() == nome.ToLower()).FirstOrDefault();

        public Cidade ChecarCidade(string nome) =>
            _cidade.Find(cidade => cidade.Nome.ToLower() == nome.ToLower()).FirstOrDefault();

        public Cidade Create(Cidade cidade)
        {
            _cidade.InsertOne(cidade);
            return cidade;
        }

        public void Update(string id, Cidade upCidade)
        {
            upCidade.Id = id;
            _cidade.ReplaceOne(cidade => cidade.Id == id, upCidade);
        }

        public void Remove(string id) =>
            _cidade.DeleteOne(cidade => cidade.Id == id);
    }
}
