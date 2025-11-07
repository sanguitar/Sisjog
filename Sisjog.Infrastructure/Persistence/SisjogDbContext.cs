using Microsoft.EntityFrameworkCore;
using Sisjog.Domain.Entities;

namespace Sisjog.Infrastructure.Persistence;

public class SisjogDbContext : DbContext
{
    public SisjogDbContext(DbContextOptions<SisjogDbContext> options)
        : base(options) { }

    public DbSet<VideoGameConsole> Consoles => Set<VideoGameConsole>();

    public DbSet<Jogo> Jogos => Set<Jogo>();
}
