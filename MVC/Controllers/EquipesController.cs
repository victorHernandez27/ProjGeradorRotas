using Microsoft.AspNetCore.Mvc;
using Models;
using Servicos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Controllers
{
    public class EquipesController : Controller
    {
        // GET: Equipes
        public async Task<IActionResult> Index()
        {
            return View(await BuscaEquipe.BuscarTodasEquipes());
        }




        #region Criar nova equipe
        // GET: Equipes/Create
        public async Task<IActionResult> Create()
        {
            //buscando todas as cidades e ordenando
            var retornoCidade = await BuscaCidade.BuscarTodasCidades();
            List<Cidade> cidade = new List<Cidade>();
            cidade.AddRange(retornoCidade);
            var ordenarCidade = from c in cidade orderby c.Nome select new { c.Nome, c.Id };
            ViewBag.Cidade = ordenarCidade;

            //buscando todas as pessoas e ordenando
            var retornoPessoa = await BuscaPessoa.BuscarTodasPessoas();
            List<Pessoa> pessoa = new List<Pessoa>();
            pessoa.AddRange(retornoPessoa);
            List<Pessoa> ordenarPessoa = pessoa.OrderBy(p => p.Nome).ToList();
            ViewBag.Pessoa = ordenarPessoa;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Codigo,Cidade")] Equipe equipe)
        {

            List<Pessoa> listaPessoa = new List<Pessoa>();

            var cidade = Request.Form["Cidade"].ToString(); //recupero o ID da cidade que foi selecionada no dropdown e salvo na variavel
            var buscaCidade = await BuscaCidade.BuscarCidadePeloId(cidade); //busco a cidade selecionada na API de cidade e retorno todas as informações
            var pessoa = Request.Form["VerificaPessoaEquipe"].ToList();//recupero os id das pessoas que foram selecionadas nas checkbox

            foreach (var item in pessoa)
            {
                listaPessoa.Add(await BuscaPessoa.BuscarPessoaPeloId(item));
            }

            if (ModelState.IsValid)
            {

                var result = await BuscaEquipe.BuscarEquipePeloCodigo(equipe.Codigo);

                if (result == null)
                {
                    equipe.Pessoa = listaPessoa;
                    equipe.Cidade = buscaCidade;
                    BuscaEquipe.CadastrarEquipe(equipe);
                }
                else
                {
                    return Conflict("Codigo da equipe ja cadastrada");
                }

                return RedirectToAction(nameof(Index));
            }
            return View(equipe);
        }
        #endregion


        #region Editar equipe
        // GET: Equipes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var equipe = await BuscaEquipe.BuscarEquipePeloId(id);

            var result = await BuscaCidade.BuscarTodasCidades();
            List<Cidade> cidade = new List<Cidade>();
            cidade.AddRange(result);
            var ordenarCidade = from c in cidade orderby c.Nome select new { c.Nome, c.Id };
            ViewBag.Cidade = ordenarCidade;

            var retornoPessoa = await BuscaPessoa.BuscarTodasPessoas();
            List<Pessoa> pessoa = new List<Pessoa>();
            pessoa.AddRange(retornoPessoa);
            List<Pessoa> ordenarPessoa = pessoa.OrderBy(p => p.Nome).ToList();
            ViewBag.Pessoa = ordenarPessoa;


            if (equipe == null)
            {
                return NotFound();
            }
            return View(equipe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Codigo,Cidade")] Equipe equipe)
        {
            if (id != equipe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                List<Pessoa> listaPessoa = new List<Pessoa>();

                var result = await BuscaEquipe.BuscarEquipePeloCodigo(equipe.Codigo);
                var cidadeUp = Request.Form["Cidade"].ToString();
                var buscaCidade = await BuscaCidade.BuscarCidadePeloId(cidadeUp);
                var pessoaUp = Request.Form["AtualizarPessoa"].ToList();

                if (result == null) //verifico se o codigo da equipe ja está cadastrado
                {
                    if (pessoaUp.Count > 0)
                    {
                        for (int i = 0; i < pessoaUp.Count; i++)
                        {
                            var buscarPessoa = await BuscaPessoa.BuscarPessoaPeloId(pessoaUp[i]);
                            listaPessoa.Add(buscarPessoa);
                        }
                    }

                    equipe.Pessoa = listaPessoa;
                    equipe.Cidade = buscaCidade;
                    BuscaEquipe.UpdateEquipe(id, equipe);
                }
                else
                {
                    return Conflict("Codigo da equipe ja cadastrada");
                }

                return RedirectToAction(nameof(Index));
            }
            return View(equipe);
        }
        #endregion


        #region Deletar equipe
        // GET: Equipes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipe = await BuscaEquipe.BuscarEquipePeloId(id);

            if (equipe == null)
            {
                return NotFound();
            }

            return View(equipe);
        }

        // POST: Equipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var equipe = await BuscaEquipe.BuscarEquipePeloId(id);
            BuscaEquipe.RemoverEquipe(id);
            return RedirectToAction(nameof(Index));
        }
        #endregion



        #region Detalhes da equipe

        public async Task<IActionResult> Details(string id)
        {
            var equipe = await BuscaEquipe.BuscarEquipePeloId(id);

            if (equipe == null)
            {
                return NotFound();
            }

            return View(equipe);
        }



        #endregion
    }
}
