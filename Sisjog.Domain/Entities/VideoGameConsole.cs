using Sisjog.Domain.Enums;

namespace Sisjog.Domain.Entities
{
    public class VideoGameConsole
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public int AnoLancamento { get; set; }
        public EstadoConservacao Estado { get; set; }

        public ICollection<Jogo> Jogos { get; set; } = new List<Jogo>();
    }
}
