using Sisjog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sisjog.Application.Interfaces
{
    public interface IVideoGameConsoleRepository
    {
        Task<IEnumerable<VideoGameConsole>> GetAllAsync();
        Task<VideoGameConsole?> GetByIdAsync(int id);
        Task AddAsync(VideoGameConsole console);
        Task UpdateAsync(VideoGameConsole console);
        Task DeleteAsync(VideoGameConsole console);
    }
}
