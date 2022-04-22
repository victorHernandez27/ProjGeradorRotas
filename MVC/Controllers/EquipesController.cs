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

        // GET: Equipes/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Codigo,Cidade")] Equipe equipe)
        {

            List<Pessoa> listaPessoa = new List<Pessoa>();

            var cidade = Request.Form["Cidade"].FirstOrDefault(); 
            var buscaCidade = await BuscaCidade.BuscarCidadePeloNome(cidade); 
            var pessoa = Request.Form["VerificaPessoaEquipe"].ToList();

            foreach (var item in pessoa)
            {
                listaPessoa.Add(await BuscaPessoa.BuscarPessoaPeloId(item));
            }

            if (ModelState.IsValid)
            {
                equipe.Pessoa = listaPessoa;
                equipe.Cidade = buscaCidade;
                BuscaEquipe.CadastrarEquipe(equipe);
                return RedirectToAction(nameof(Index));
            }
            return View(equipe);
        }

        // GET: Equipes/Edit/5
        public async Task<IActionResult> Edit(string id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Codigo,Pessoa,Cidade")] Equipe equipe)
        {
            if (id != equipe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var pessoaAdd = Request.Form["adicionarPessoa"].ToList();

                if (pessoaAdd.Count > 0)
                {
                    for (int i = 0; i < pessoaAdd.Count; i++)
                    {
                        var buscarPessoa = await BuscaPessoa.BuscarPessoaPeloNome(pessoaAdd[i]);
                        equipe.Pessoa.Add(buscarPessoa);
                    }
                }
                BuscaEquipe.UpdateEquipe(id, equipe);
                return RedirectToAction(nameof(Index));
            }
            return View(equipe);
        }

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

    }
}
