using App.Models;
using X.PagedList;
namespace Projeto_Sushi.Models;
public class ProdutoCategoriaViewModel
{
    public IPagedList<Produto> ListaProduto { get; set; }
    public IEnumerable<Categoria> ListaCategoria { get; set; }
}