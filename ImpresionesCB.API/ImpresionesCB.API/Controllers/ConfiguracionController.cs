using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ImpresionesCB.API.Data;
using ImpresionesCB.API.Models;

namespace ImpresionesCB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfiguracionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ConfiguracionController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ConfiguracionEtiqueta>> GetConfiguracion()
        {
            var config = await _context.Configuracion.FirstOrDefaultAsync();
            return config == null ? NotFound() : config;
        }

        [HttpPut]
        public async Task<IActionResult> PutConfiguracion(ConfiguracionEtiqueta config)
        {
            _context.Entry(config).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
