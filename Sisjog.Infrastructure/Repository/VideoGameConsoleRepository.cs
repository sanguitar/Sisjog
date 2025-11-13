using Microsoft.EntityFrameworkCore;
using Sisjog.Application.Interfaces;
using Sisjog.Domain.Entities;
using Sisjog.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sisjog.Infrastructure.Repository
{
    // Em Sisjog.Infrastructure.Repositories
    public class VideoGameConsoleRepository : IVideoGameConsoleRepository
    {
        private readonly SisjogDbContext _context;

        public VideoGameConsoleRepository(SisjogDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<VideoGameConsole>> GetAllAsync()
            => await _context.Consoles.AsNoTracking().ToListAsync();

        public async Task<VideoGameConsole?> GetByIdAsync(int id)
            => await _context.Consoles.FindAsync(id);

        public async Task AddAsync(VideoGameConsole console)
        {
            _context.Consoles.Add(console);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(VideoGameConsole console)
        {
            _context.Consoles.Update(console);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(VideoGameConsole console)
        {
            _context.Consoles.Remove(console);
            await _context.SaveChangesAsync();
        }
    }

}
