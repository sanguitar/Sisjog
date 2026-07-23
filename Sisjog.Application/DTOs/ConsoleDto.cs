using System.Text.Json.Serialization;
using Sisjog.Domain.Enums;

namespace Sisjog.Application.DTOs
{
    public class ConsoleDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public int AnoLancamento { get; set; }
        public string Midia { get; set; } = string.Empty; // CD ou Cartucho

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EstadoConsole Estado { get; set; }

        public bool Emprestado { get; set; }
        public string? EmprestadoPara { get; set; }
        public string? ImagemUrl { get; set; }
    }
}
