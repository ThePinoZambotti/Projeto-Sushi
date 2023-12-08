using System.Diagnostics;
using App.Context;
using Microsoft.AspNetCore.Mvc;
using Projeto_Sushi.Models;
using X.PagedList;
using App.Models;
using Microsoft.EntityFrameworkCore;



namespace Projeto_Sushi.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View(_context.Banners.ToList());
    }

    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult Sobrenos()
    {
        return View();
    }
    public IActionResult Cardapio(string botao, string? txtFiltro, string? selOrdenacao, int pagina = 1)
    {
        int pageSize = 8;
        IQueryable<Produto> lista = _context.Produtos.Include(p => p.Categorias);

        if (botao == "Relatorio")
        {
            pageSize = lista.Count();
        }

        if (txtFiltro != null && txtFiltro != "")
        {
            ViewData["txtFiltro"] = txtFiltro;
            lista = lista.Where(item =>
            item.Nome.ToLower().Contains(txtFiltro.ToLower())
            ||
            item.Descricao.ToLower().Contains(txtFiltro.ToLower())
            ||
            item.Categorias.CategoriaNome.ToLower().Contains(txtFiltro.ToLower()));
        }
        if (selOrdenacao == "Nome" || selOrdenacao == null)
        {
            lista = lista.OrderBy(item => item.Nome.ToLower());
        }
        else if (selOrdenacao == "Nome_Desc")
        {
            lista = lista.OrderByDescending(item => item.Nome.ToLower());
        }


        ProdutoCategoriaViewModel vm = new ProdutoCategoriaViewModel
        {
            ListaProduto = lista.ToPagedList(pagina, pageSize),
            ListaCategoria = _context.Categorias.OrderBy(item => item.CategoriaNome).ToList()
        };
        return View(vm);
        //return View(lista.ToPagedList(pagina, pageSize));

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
