using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Servicos;
using System.Threading.Tasks;

namespace MVC.Controllers
{
    public class PessoasController : Controller
    {
        // GET: Pessoas
        public async Task<IActionResult> Index()
        {


            return View(await BuscaPessoa.BuscarTodasPessoas());

        }

        // GET: Pessoas/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Nome")] Pessoa pessoa)
        {
            if (ModelState.IsValid)
            {
                BuscaPessoa.CadastrarPessoa(pessoa);
                return RedirectToAction(nameof(Index));
            }
            return View(pessoa);
        }

        // GET: Pessoas/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pessoa = await BuscaPessoa.BuscarPessoaPeloId(id);
            if (pessoa == null)
            {
                return NotFound();
            }
            return View(pessoa);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Nome")] Pessoa pessoa)
        {
            if (id != pessoa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var retornoPessoa = await BuscaPessoa.BuscarPessoaPeloId(id);
                    BuscaPessoa.UpdatePessoa(id, pessoa);

                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;

                }
                return RedirectToAction(nameof(Index));
            }
            return View(pessoa);
        }

        // GET: Pessoas/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pessoa = await BuscaPessoa.BuscarPessoaPeloId(id);

            if (pessoa == null)
            {
                return NotFound();
            }

            return View(pessoa);
        }

        // POST: Pessoas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var pessoa = await BuscaPessoa.BuscarPessoaPeloId(id);
            BuscaPessoa.RemoverPessoa(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
