using ApiEquipe.Util;
using Models;
using MongoDB.Driver;
using Servicos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiEquipe.Servicos
{
    public class EquipeServicos
    {
        private readonly IMongoCollection<Equipe> _equipe;

        public EquipeServicos(IEquipeDatabase settings)
        {
            var equipe = new MongoClient(settings.ConnectionString);
            var database = equipe.GetDatabase(settings.DatabaseName);
            _equipe = database.GetCollection<Equipe>(settings.EquipeCollectionName);
        }

        public List<Equipe> Get() =>
            _equipe.Find(equipe => true).ToList();

        public Equipe Get(string codigo) =>
            _equipe.Find(equipe => equipe.Codigo.ToUpper() == codigo.ToUpper()).FirstOrDefault();

        public Equipe ChecarEquipe(string codigo) =>
            _equipe.Find(equipe => equipe.Codigo.ToUpper() == codigo.ToUpper()).FirstOrDefault();

        public async Task<Equipe> CreateAsync(Equipe equipe)
        {
            var retornoPessoa = new List<Pessoa>();

            foreach (var item in equipe.Pessoa)
            {
                Pessoa pessoa = await BuscaPessoa.GetPessoa(item.Nome);
                retornoPessoa.Add(pessoa);
            }


            var retornoCidade = await BuscaCidade.GetCidade(equipe.Cidade.Nome);

            equipe.Codigo = equipe.Codigo.ToUpper();
            equipe.Pessoa = retornoPessoa;
            equipe.Cidade = retornoCidade;

            _equipe.InsertOne(equipe);
            return equipe;
        }

        public void Update(string codigo, Equipe upEquipe) =>
            _equipe.ReplaceOne(equipe => equipe.Codigo.ToUpper() == codigo.ToUpper(), upEquipe);

        public void Remove(string id) =>
            _equipe.DeleteOne(equipe => equipe.Id == id);

    }
}
