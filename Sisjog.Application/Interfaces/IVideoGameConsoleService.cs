using Sisjog.Application.DTOs;


namespace Sisjog.Application.Interfaces
{
    public interface IVideoGameConsoleService
    {
        Task<IEnumerable<ConsoleDto>> GetAllAsync();
        Task<ConsoleDto?> GetByIdAsync(int id);
        Task<ConsoleDto> CreateAsync(ConsoleDto dto);
        Task UpdateAsync(ConsoleDto dto);
        Task DeleteAsync(int id);
    }
}
