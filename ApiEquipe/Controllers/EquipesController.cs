using ApiEquipe.Servicos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiEquipe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipesController : ControllerBase
    {
        private readonly EquipeServicos _equipeServicos;

        public EquipesController(EquipeServicos equipeServicos)
        {
            _equipeServicos = equipeServicos;
        }

        [HttpGet]
        public ActionResult<List<Equipe>> Get() =>
            _equipeServicos.Get();

        [HttpGet("{id:length(24)}", Name = "GetEquipe")]
        public ActionResult<Equipe> Get(string id)
        {
            var equipe = _equipeServicos.Get(id);

            if (equipe == null)
            {
                return NotFound();
            }

            return equipe;
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult PutEquipe(string codigo, Equipe upEquipe)
        {
            var equipe = _equipeServicos.Get(codigo);

            if (equipe == null)
            {
                return NotFound();
            }

            _equipeServicos.Update(codigo.ToUpper(), upEquipe);

            return NoContent();


        }

        [HttpPost]
        public async Task<ActionResult<Equipe>> PostEquipe(Equipe equipe)
        {
            var checar = _equipeServicos.ChecarEquipe(equipe.Codigo);

            if (checar == null)
            {
                equipe = await _equipeServicos.CreateAsync(equipe);
            }
            else
            {
                return Conflict("Equipe Já Cadastrada");
            }

            return CreatedAtRoute("GetEquipe", new { id = equipe.Id }, equipe);
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult DeleteEquipe(string id)
        {
            var equipe = _equipeServicos.Get(id);

            if (equipe == null)
            {
                return NotFound();
            }

            _equipeServicos.Remove(equipe.Id);
            return NoContent();
        }
    }
}
