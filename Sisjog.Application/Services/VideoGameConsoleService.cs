using AutoMapper;
using Sisjog.Application.DTOs;
using Sisjog.Application.Interfaces;
using Sisjog.Domain.Entities;



namespace Sisjog.Application.Services
{
    public class VideoGameConsoleService : IVideoGameConsoleService
    {
        private readonly IVideoGameConsoleRepository _repository;
        private readonly IMapper _mapper;

        public VideoGameConsoleService(IVideoGameConsoleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ConsoleDto>> GetAllAsync()
        {
            var consoles = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ConsoleDto>>(consoles);
        }

        public async Task<ConsoleDto?> GetByIdAsync(int id)
        {
            var console = await _repository.GetByIdAsync(id);
            return console == null ? null : _mapper.Map<ConsoleDto>(console);
        }

        public async Task<ConsoleDto> CreateAsync(ConsoleDto dto)
        {
            var entity = _mapper.Map<VideoGameConsole>(dto);
            await _repository.AddAsync(entity);
            return _mapper.Map<ConsoleDto>(entity);
        }

        public async Task UpdateAsync(ConsoleDto dto)
        {
            var entity = _mapper.Map<VideoGameConsole>(dto);
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity != null)
                await _repository.DeleteAsync(entity);
        }
    }

}
