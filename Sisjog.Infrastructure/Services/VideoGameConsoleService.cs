using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sisjog.Application.DTOs;
using Sisjog.Application.Interfaces;
using Sisjog.Domain.Entities;
using Sisjog.Infrastructure.Persistence;
using Sisjog.Domain.Entities;
using Sisjog.Application.DTOs;
using Sisjog.Application.Interfaces;
using Sisjog.Infrastructure.Persistence;
using Sisjog.Infrastructure.Persistence;



namespace Sisjog.Infrastructure.Services;

public class VideoGameConsoleService : IVideoGameConsoleService
{
    private readonly SisjogDbContext _context;
    private readonly IMapper _mapper;

    public VideoGameConsoleService(SisjogDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ConsoleDto>> GetAllAsync()
    {
        var consoles = await _context.Consoles.AsNoTracking().ToListAsync();
        return _mapper.Map<IEnumerable<ConsoleDto>>(consoles);
    }

    public async Task<ConsoleDto?> GetByIdAsync(int id)
    {
        var console = await _context.Consoles.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        return console == null ? null : _mapper.Map<ConsoleDto>(console);
    }

    public async Task<ConsoleDto> CreateAsync(ConsoleDto dto)
    {
        var console = _mapper.Map<VideoGameConsole>(dto);
        _context.Consoles.Add(console);
        await _context.SaveChangesAsync();
        return _mapper.Map<ConsoleDto>(console);
    }

    public async Task UpdateAsync(ConsoleDto dto)
    {
        var console = await _context.Consoles.FindAsync(dto.Id);
        if (console == null) return;

        _mapper.Map(dto, console);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var console = await _context.Consoles.FindAsync(id);
        if (console == null) return;

        _context.Consoles.Remove(console);
        await _context.SaveChangesAsync();
    }
}
