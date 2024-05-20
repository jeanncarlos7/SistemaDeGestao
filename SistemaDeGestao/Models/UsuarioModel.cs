namespace SistemaDeGestao.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }

        public ICollection<AvaliacaoModel>? Avaliacoes { get; set; }
    }
}
