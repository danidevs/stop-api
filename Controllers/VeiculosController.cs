using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StopApi.Models;
using StopApi.DbContexto;
using Microsoft.EntityFrameworkCore;
using StopApi.Services;


namespace StopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculosController : ControllerBase
    {
        private readonly EstacionamentoContext _context;
        private readonly CalculoEstacionamento _calculoEstacionamento;

        public VeiculosController(EstacionamentoContext context, CalculoEstacionamento calculoEstacionamento)
        {
            _context = context;
            _calculoEstacionamento = calculoEstacionamento;
        }

       [HttpGet]
        public async Task<IActionResult> GetVeiculos()
        {
            var veiculosAtivos = await _context.Veiculos.Where(v => v.Ativo).ToListAsync();
            return Ok(veiculosAtivos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Veiculo>> GetVeiculo(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);

            if (veiculo == null)
            {
                return NotFound();
            }

            return veiculo;
        }

       [HttpGet("{id}/calcular-horas")]
        public async Task<IActionResult> CalcularHorasEstacionadas(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null || !veiculo.Ativo)
            {
                return NotFound();
            }

            var horasEstacionadas = (DateTime.Now - veiculo.HoraEntrada).TotalHours;
            return Ok(new { Id = veiculo.Id, HorasEstacionadas = horasEstacionadas });
        }

       [HttpPost]
        public async Task<ActionResult<Veiculo>> PostVeiculo(Veiculo veiculo)
        {
            try
            {
                veiculo.HoraEntrada = DateTime.Now; 
                _context.Veiculos.Add(veiculo);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetVeiculo), new { id = veiculo.Id }, veiculo);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao adicionar veículo: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutVeiculo(int id, Veiculo veiculo)
        {
            if (id != veiculo.Id)
            {
                return BadRequest();
            }

            _context.Entry(veiculo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VeiculoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarVeiculo(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null)
            {
                return NotFound();
            }

            veiculo.Ativo = false;
            _context.Veiculos.Update(veiculo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool VeiculoExists(int id)
        {
            return _context.Veiculos.Any(e => e.Id == id);
        }
    }
}
