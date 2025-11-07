namespace Sisjog.Application.DTOs
{
    public class ConsoleDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public int Ano { get; set; }
        public string Midia { get; set; } = string.Empty; // CD ou Cartucho
        public string EstadoConservacao { get; set; } = string.Empty; // Novo, Usado, etc.
        public bool Emprestado { get; set; }
        public string? EmprestadoPara { get; set; }
        public string? ImagemUrl { get; set; }
    }
}
