using ApiCidade.Servicos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Collections.Generic;

namespace ApiCidade.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CidadesController : ControllerBase
    {
        private readonly CidadeServicos _cidadeServicos;

        public CidadesController(CidadeServicos cidadeServicos)
        {
            _cidadeServicos = cidadeServicos;
        }

        [HttpGet]
        public ActionResult<List<Cidade>> Get() =>
            _cidadeServicos.Get();

        [HttpGet("{id:length(24)}", Name = "GetCidade")]
        public ActionResult<Cidade> Get(string id)
        {
            var cidade = _cidadeServicos.Get(id);

            if (cidade == null)
            {
                return NotFound();
            }

            return cidade;
        }

        [HttpGet("Nome")]
        public ActionResult<Cidade> GetNome(string nome)
        {
            var cidade = _cidadeServicos.GetNome(nome);

            if (cidade == null)
            {
                return NotFound();
            }

            return cidade;
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult PutCidade(string id, Cidade upCidade)
        {
            var cidade = _cidadeServicos.Get(id);

            if (cidade == null)
            {
                return NotFound();
            }

            _cidadeServicos.Update(id, upCidade);

            return NoContent();

        }

        [HttpPost]
        public ActionResult<Cidade> PostCidade(Cidade cidade)
        {
            var checar = _cidadeServicos.ChecarCidade(cidade.Nome);

            if (checar == null)
            {
                _cidadeServicos.Create(cidade);
            }
            else
            {
                return Conflict("Cidade Já Cadastrada");
            }

            return CreatedAtRoute("GetCidade", new { id = cidade.Id }, cidade);
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult DeleteCidade(string id)
        {
            var cidade = _cidadeServicos.Get(id);

            if (cidade == null)
            {
                return NotFound();
            }

            _cidadeServicos.Remove(cidade.Id);
            return NoContent();
        }
    }
}
