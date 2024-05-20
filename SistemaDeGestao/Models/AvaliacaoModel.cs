using SistemaDeGestao.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SistemaDeGestao.Models
{
    public class AvaliacaoModel
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Nome")]
        public string Nome { get; set; }

        [Column("Descricao")]
        public string Descricao { get; set; }

        [Column("Status")]
        public StatusTarefa Status { get; set; }

        [Column("UsuarioId")]
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public UsuarioModel Usuario { get; set; }

        [Column("Email")]
        public string Email { get; set; }
    }
}
