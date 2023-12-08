using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models
{
    [Table("Banners")]
    public class Banner
    {
        [Key]
        public int BannerId { get; set; }
        
        [Required(ErrorMessage = "A imagem é obrigatória")]
        public string Imagem { get; set; }

        public string? Titulo { get; set; }
        public string? Subtitulo { get; set; }
        public string? Link { get; set; }
        public bool Exibir { get; set; }
        }
}