using Sisjog.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sisjog.Domain.Entities
{
    public class Jogo
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Genero { get; set; } = string.Empty;
        public int Ano { get; set; }
        public TipoMidia Midia { get; set; }
        public EstadoConservacao Estado { get; set; }

        public bool Emprestado { get; set; }
        public string? NomePessoaEmprestimo { get; set; }

        public int ConsoleId { get; set; }
        public VideoGameConsole Console { get; set; } = null!;
    }
}
