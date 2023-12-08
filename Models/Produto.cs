using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace App.Models
{
    //DataAnnotation de como será criado o nome da tabela no BD
    [Table("Produtos")]
    public class Produto
    {
        [Key] //Falando para o BD que este atributo será uma chave
        public int ProdutoId { get; set; }
        [Required(ErrorMessage = "Nome é obrigatório")]
        [MaxLength(200, ErrorMessage = "Descrição pode exceder {1} caracteres")]
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }
        public decimal? Preco { get; set; }
        public bool Ativo { get; set; }
        public int CategoriaId { get; set; }
        public virtual Categoria Categorias { get; set; }
    }
}