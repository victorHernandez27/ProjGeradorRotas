using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Servicos;

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
            var retornoCidade = await BuscaCidade.BuscarTodasCidades();
            List<Cidade> cidade = new List<Cidade>();
            cidade.AddRange(retornoCidade);
            var ordenarCidade = from c in cidade orderby c.Nome select new { c.Nome, c.Id };
            ViewBag.Cidade = ordenarCidade;
           
            var retornoPessoa = await BuscaPessoa.BuscarTodasPessoas();
            List<Pessoa> pessoa = new List<Pessoa>();

            for (int i = 0; i < retornoPessoa.Count; i++)
            {
                if (retornoPessoa[i].Disponivel == true)
                    pessoa.Add(retornoPessoa[i]);
            }
            
            List<Pessoa> ordenarPessoa = pessoa.OrderBy(p => p.Nome).ToList();
            ViewBag.Pessoa = ordenarPessoa;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Codigo,Cidade")] Equipe equipe)
        {
            List<Pessoa> listaPessoa = new List<Pessoa>();

            var cidade = Request.Form["Cidade"].ToString(); 
            var buscaCidade = await BuscaCidade.BuscarCidadePeloId(cidade);
            var pessoa = Request.Form["VerificaPessoaEquipe"].ToList();

            if (ModelState.IsValid)
            {
                var result = await BuscaEquipe.BuscarEquipePeloCodigo(equipe.Codigo); 

                if (result == null)
                {
                    if (pessoa.Count != 0 && buscaCidade != null)
                    {
                        for (int i = 0; i < pessoa.Count; i++) 
                        {
                            listaPessoa.Add(await BuscaPessoa.BuscarPessoaPeloId(pessoa[i]));
                            listaPessoa[i].Disponivel = false;
                            BuscaPessoa.UpdatePessoa(pessoa[i], listaPessoa[i]);
                        }

                        equipe.Pessoa = listaPessoa;
                        equipe.Cidade = buscaCidade;

                        BuscaEquipe.CadastrarEquipe(equipe);
                    }
                    else
                    {
                        return Conflict("ERRO >> Para cadastrar uma equipe, deve-se selecionar uma cidade e pelo menos uma pessoa.");
                    }
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

            for (int i = 0; i < retornoPessoa.Count; i++)
            {
                if (retornoPessoa[i].Disponivel == true) 
                    pessoa.Add(retornoPessoa[i]);
                else
                {
                    for (int j = 0; j < equipe.Pessoa.Count; j++) 
                    {
                        if (equipe.Pessoa[j].Id == retornoPessoa[i].Id) 
                        {
                            pessoa.Add(retornoPessoa[i]);
                            break;
                        }
                    }
                }
            }

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
                var buscaEquipe = await BuscaEquipe.BuscarEquipePeloId(id);
                var buscaCidade = await BuscaCidade.BuscarCidadePeloId(Request.Form["Cidade"].ToString());
                var pessoaUp = Request.Form["AtualizarPessoa"].ToList();

                for (int i = 0; i < buscaEquipe.Pessoa.Count; i++) 
                {
                    buscaEquipe.Pessoa[i].Disponivel = true;
                    BuscaPessoa.UpdatePessoa(buscaEquipe.Pessoa[i].Id, buscaEquipe.Pessoa[i]);
                }

                if (pessoaUp.Count > 0)
                {
                    for (int i = 0; i < pessoaUp.Count; i++) 
                    {
                        listaPessoa.Add(await BuscaPessoa.BuscarPessoaPeloId(pessoaUp[i]));
                        listaPessoa[i].Disponivel = false;
                        BuscaPessoa.UpdatePessoa(pessoaUp[i], listaPessoa[i]);
                    }
                    
                    equipe.Codigo = buscaEquipe.Codigo;
                    equipe.Pessoa = listaPessoa;
                    equipe.Cidade = buscaCidade;
                    BuscaEquipe.UpdateEquipe(id, equipe);
                }
                else
                    return Conflict("Volte e selecione pelo menos 1 pessoa");

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
            for (int i = 0; i < equipe.Pessoa.Count; i++)
            {
                equipe.Pessoa[i].Disponivel = true;
                BuscaPessoa.UpdatePessoa(equipe.Pessoa[i].Id, equipe.Pessoa[i]);
            }
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