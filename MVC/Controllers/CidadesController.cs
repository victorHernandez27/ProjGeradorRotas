using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Servicos;
using System.Threading.Tasks;

namespace MVC.Controllers
{
    public class CidadesController : Controller
    {
        // GET: Cidades
        public async Task<IActionResult> Index()
        {
            return View(await BuscaCidade.BuscarTodasCidades());
        }

        // GET: Cidades/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Nome")] Cidade cidade)
        {
            if (ModelState.IsValid)
            {
                BuscaCidade.CadastrarCidade(cidade);
                return RedirectToAction(nameof(Index));
            }
            return View(cidade);
        }

        // GET: Cidades/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cidade = await BuscaCidade.BuscarCidadePeloId(id);
            if (cidade == null)
            {
                return NotFound();
            }
            return View(cidade);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Nome")] Cidade cidade)
        {
            if (id != cidade.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var retornoCidade = await BuscaCidade.BuscarCidadePeloId(id);
                    BuscaCidade.UpdateCidade(id, cidade);

                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;

                }
                return RedirectToAction(nameof(Index));
            }
            return View(cidade);
        }

        // GET: Cidades/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cidade = await BuscaCidade.BuscarCidadePeloId(id);

            if (cidade == null)
            {
                return NotFound();
            }

            return View(cidade);
        }

        // POST: Cidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var cidade = await BuscaCidade.BuscarCidadePeloId(id);
            BuscaCidade.RemoverCidade(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
