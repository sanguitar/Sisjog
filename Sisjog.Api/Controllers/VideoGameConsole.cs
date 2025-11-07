using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sisjog.Application.DTOs;
using Sisjog.Infrastructure.Persistence;

namespace Sisjog.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsolesController : ControllerBase
    {
        private readonly SisjogDbContext _context;
        private readonly IMapper _mapper;

        public ConsolesController(SisjogDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConsoleDto>>> GetConsoles()
        {
            var consoles = await _context.Consoles.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<ConsoleDto>>(consoles));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ConsoleDto>> GetConsole(int id)
        {
            var console = await _context.Consoles.FindAsync(id);
            if (console == null) return NotFound();

            return Ok(_mapper.Map<ConsoleDto>(console));
        }

        [HttpPost]
        public async Task<ActionResult<ConsoleDto>> CreateConsole(ConsoleDto dto)
        {
            var entity = _mapper.Map<Domain.Entities.VideoGameConsole>(dto);
            _context.Consoles.Add(entity);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetConsole), new { id = entity.Id }, _mapper.Map<ConsoleDto>(entity));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConsole(int id, ConsoleDto dto)
        {
            if (id != dto.Id) return BadRequest("ID não confere");

            var entity = await _context.Consoles.FindAsync(id);
            if (entity == null) return NotFound();

            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConsole(int id)
        {
            var entity = await _context.Consoles.FindAsync(id);
            if (entity == null) return NotFound();

            _context.Consoles.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
