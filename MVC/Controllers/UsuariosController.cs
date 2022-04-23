using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Servicos;

namespace MVC.Controllers
{
    public class UsuariosController : Controller
    {
        // GET: Usuarios
        public async Task<IActionResult> Index()
        {


            return View(await BuscaUsuario.BuscarTodosUsuarios());

        }

        #region cadastrar usuario
        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NomeCompleto, NomeUsuario, Senha")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {

                var result = await BuscaUsuario.BuscarUsuarioPeloNome(usuario.NomeUsuario);

                if (result == null)
                {
                    BuscaUsuario.CadastrarUsuario(usuario);
                }
                else
                {
                    return Conflict("Usuario ja cadastrada");
                }

                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }
        #endregion


        #region editar usuario
        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await BuscaUsuario.BuscarUsuarioPeloId(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,NomeUsuario, NomeCompleto, Senha")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await BuscaUsuario.BuscarUsuarioPeloNome(usuario.NomeUsuario);

                    if (result == null)
                    {
                        BuscaUsuario.UpdateUsuario(id, usuario);
                    }
                    else
                    {
                        return Conflict("Usuario ja cadastrada");
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;

                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }
        #endregion


        #region deletar usuario
        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await BuscaUsuario.BuscarUsuarioPeloId(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var usuario = await BuscaUsuario.BuscarUsuarioPeloId(id);
            BuscaUsuario.RemoverUsuario(id);
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}