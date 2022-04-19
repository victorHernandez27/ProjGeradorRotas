using ApiPessoa.Servicos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Collections.Generic;

namespace ApiPessoa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoasController : ControllerBase
    {
        private readonly PessoaServicos _pessoaServicos;

        public PessoasController(PessoaServicos pessoaServicos)
        {
            _pessoaServicos = pessoaServicos;
        }

        [HttpGet]
        public ActionResult<List<Pessoa>> Get() =>
            _pessoaServicos.Get();

        [HttpGet("{id:length(24)}", Name = "GetPessoa")]
        public ActionResult<Pessoa> Get(string id)
        {
            var pessoa = _pessoaServicos.Get(id);

            if (pessoa == null)
            {
                return NotFound();
            }

            return pessoa;
        }

        [HttpGet("Nome")]
        public ActionResult<Pessoa> GetNome(string nome)
        {
            var pessoa = _pessoaServicos.GetNome(nome);

            if (pessoa == null)
            {
                return NotFound();
            }

            return pessoa;
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult PutPessoa(string id, Pessoa upPessoa)
        {
            var pessoa = _pessoaServicos.Get(id);

            if (pessoa == null)
            {
                return NotFound();
            }

            _pessoaServicos.Update(id, upPessoa);

            return NoContent();


        }

        [HttpPost]
        public ActionResult<Pessoa> PostPessoa(Pessoa pessoa)
        {
            var checar = _pessoaServicos.ChecarPessoa(pessoa.Nome);

            if (checar == null)
            {
                _pessoaServicos.Create(pessoa);
            }
            else
            {
                return Conflict("Pessoa Já Cadastrada");
            }

            return CreatedAtRoute("GetPessoa", new { id = pessoa.Id }, pessoa);
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult DeletePessoa(string id)
        {
            var pessoa = _pessoaServicos.Get(id);

            if (pessoa == null)
            {
                return NotFound();
            }

            _pessoaServicos.Remove(pessoa.Id);
            return NoContent();
        }
    }
}
