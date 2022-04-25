using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Servicos;

namespace MVC.Controllers
{
    public class PessoasController : Controller
    {
        // GET: Pessoas
        public async Task<IActionResult> Index()
        {


            return View(await BuscaPessoa.BuscarTodasPessoas());

        }
        #region cadastrar pessoa
        // GET: Pessoas/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] Pessoa pessoa)
        {
            if (ModelState.IsValid)
            {
                var result = await BuscaPessoa.BuscarPessoaPeloNome(pessoa.Nome);

                if (result == null)
                {
                    BuscaPessoa.CadastrarPessoa(pessoa);
                }
                else
                {
                    return Conflict("Pessoa ja cadastrada");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pessoa);
        }
        #endregion


        #region editar pessoa
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

                var result = await BuscaPessoa.BuscarPessoaPeloNome(pessoa.Nome);
                if (result == null)
                {
                    BuscaPessoa.UpdatePessoa(id, pessoa);
                }
                else
                {
                    return Conflict("Pessoa ja está cadastrada, tente outro nome.");
                }

                return RedirectToAction(nameof(Index));
            }
            return View(pessoa);
        }
        #endregion

        #region deletar pessoa
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
        #endregion
    }
}