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

        public Equipe GetId(string id) =>
           _equipe.Find(equipe => equipe.Id == id).FirstOrDefault();

        public Models.Equipe ChecarEquipe(string codigo) =>
            _equipe.Find(equipe => equipe.Codigo.ToUpper() == codigo.ToUpper()).FirstOrDefault();

        public async Task<Equipe> CreateAsync(Equipe equipe)
        {


            var retornoPessoa = new List<Pessoa>();

            foreach (var item in equipe.Pessoa)
            {
                Pessoa pessoa = await BuscaPessoa.BuscarPessoaPeloNome(item.Nome);
                retornoPessoa.Add(pessoa);
            }


            var retornoCidade = await BuscaCidade.BuscarCidadePeloNome(equipe.Cidade.Nome);

            equipe.Codigo = equipe.Codigo.ToUpper();
            equipe.Pessoa = retornoPessoa;
            equipe.Cidade = retornoCidade;

            _equipe.InsertOne(equipe);
            return equipe;
        }

        public async Task<Equipe> Update(string id, Equipe upEquipe)
        {

            if (upEquipe.Pessoa != null)
            {
                var retornoPessoa = new List<Pessoa>();

                foreach (var item in upEquipe.Pessoa)
                {
                    Pessoa pessoa = await BuscaPessoa.BuscarPessoaPeloNome(item.Nome);
                    retornoPessoa.Add(pessoa);
                }
                upEquipe.Pessoa = retornoPessoa;
            }

            if (upEquipe.Cidade.Nome != null)
            {
                var retornoCidade = await BuscaCidade.BuscarCidadePeloNome(upEquipe.Cidade.Nome);
                upEquipe.Cidade = retornoCidade;
            }

            if (upEquipe.Codigo == null)
            {
                var codigo = GetId(id);
                upEquipe.Codigo = codigo.Codigo.ToUpper();
            }
            if (upEquipe.Codigo != null)
            {
                upEquipe.Codigo = upEquipe.Codigo.ToUpper();
            }


            upEquipe.Id = id;

            _equipe.ReplaceOne(equipe => equipe.Id == id, upEquipe);

            return upEquipe;
        }

        public void Remove(string id) =>
            _equipe.DeleteOne(equipe => equipe.Id == id);

    }
}
