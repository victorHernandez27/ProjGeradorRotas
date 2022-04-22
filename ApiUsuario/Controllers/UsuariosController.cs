using ApiUsuario.Servicos;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Collections.Generic;

namespace ApiUsuario.Controllers
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioServicos _usuarioServicos;

        public UsuariosController(UsuarioServicos usuarioServicos)
        {
            _usuarioServicos = usuarioServicos;
        }

        [HttpGet]
        public ActionResult<List<Usuario>> Get() =>
            _usuarioServicos.Get();


        [HttpGet("{id:length(24)}", Name = "GetUsuario")]
        public ActionResult<Usuario> Get(string id)
        {
            var usuario = _usuarioServicos.GetId(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        [HttpGet("Nome")]
        public ActionResult<Usuario> GetNome(string nome)
        {
            var usuario = _usuarioServicos.GetNome(nome);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult PutUsuario(string id, Usuario upUsuario)
        {
            var usuario = _usuarioServicos.GetId(id);

            if (usuario == null)
            {
                return NotFound();
            }

            _usuarioServicos.Update(id, upUsuario);

            return NoContent();


        }

        [HttpPost]
        public ActionResult<Usuario> PostUsuario(Usuario usuario)
        {
            var checar = _usuarioServicos.ChecarUsuario(usuario.NomeUsuario);

            if (checar == null)
            {
                _usuarioServicos.Create(usuario);
            }
            else
            {
                return Conflict("Usuario Já Cadastrada");
            }

            return CreatedAtRoute("GetUsuario", new { id = usuario.Id }, usuario);
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult DeleteUsuario(string id)
        {
            var usuario = _usuarioServicos.GetId(id);

            if (usuario == null)
            {
                return NotFound();
            }

            _usuarioServicos.Remove(usuario.Id);
            return NoContent();
        }
    }
}
