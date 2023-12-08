using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Context;
using App.Models;
using X.PagedList;
using System.Xml;
using System.Text;

namespace Projeto_Sushi.Controllers
{
    [Area("Admin")]
    public class ProdutoController : Controller
    {
        private readonly AppDbContext _context;

        public ProdutoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Produto
        public IActionResult Index(string botao, string? txtFiltro, string? selOrdenacao, int pagina = 1)
        {
            int pageSize = 5;
            IQueryable<Produto> lista = _context.Produtos;

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
                item.Descricao.ToLower().Contains(txtFiltro.ToLower()));
            }
            if (selOrdenacao == "Nome" || selOrdenacao == null)
            {
                lista = lista.OrderBy(item => item.Nome.ToLower());
            }
            else if (selOrdenacao == "Nome_Desc")
            {
                lista = lista.OrderByDescending(item => item.Nome.ToLower());
            }

            if (botao == "XML")
            {
                return ExportarXML(lista.ToList());
            }
            else if (botao == "Json")
            {
                return ExportarJson(lista.ToList());
            }


            return View(lista.ToPagedList(pagina, pageSize));

        }

        private IEnumerable<Produto> GetProdutosFiltrados(string txtFiltro)
        {
            return _context.Produtos.Where(p => p.Nome.Contains(txtFiltro) || p.Descricao.Contains(txtFiltro));
        }

        private IActionResult ExportarXML(List<Produto> lista)
        {
            var stream = new StringWriter();
            var xml = new XmlTextWriter(stream);
            xml.Formatting = System.Xml.Formatting.Indented;
            xml.WriteStartDocument();
            xml.WriteStartElement("Dados");
            xml.WriteStartElement("Produtos");
            foreach (var item in lista)
            {
                xml.WriteStartElement("Produto");
                xml.WriteElementString("Id", item.ProdutoId.ToString());
                xml.WriteElementString("Nome", item.Nome);
                xml.WriteElementString("Preço", item.Preco.ToString());
                xml.WriteEndElement(); // </Usuario>
            }
            xml.WriteEndElement(); // </Usuarios>
            xml.WriteEndElement(); // </Dados>
            return File(Encoding.UTF8.GetBytes(stream.ToString()), "application/xml", "dados_usuarios.xml");
        }

        private IActionResult ExportarJson(List<Produto> lista)
        {
            var json = new StringBuilder();
            json.AppendLine("{");
            json.AppendLine(" \"Produtos\": [");
            int total = 0;
            foreach (var item in lista)
            {
                json.AppendLine(" {");
                json.AppendLine($" \"Id\": {item.ProdutoId},");
                json.AppendLine($" \"Nome\": \"{item.Nome}\",");
                json.AppendLine($" \"Preço\": \"{item.Preco}\"");
                json.AppendLine(" }");
                total++;
                if (total < lista.Count())
                {
                    json.AppendLine(" ,");
                }
            }
            json.AppendLine(" ]");
            json.AppendLine("}");
            return File(Encoding.UTF8.GetBytes(json.ToString()), "application/json", "dados_usuarios.json");
        }


        // GET: Produto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Produtos == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos
                .Include(p => p.Categorias)
                .FirstOrDefaultAsync(m => m.ProdutoId == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // GET: Produto/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaId");
            return View();
        }

        // POST: Produto/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProdutoId,Nome,Descricao,Imagem, Preco, Ativo,CategoriaId")] Produto produto)
        {
            if (true)
            {
                _context.Add(produto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaNome", produto.CategoriaId);
            return View(produto);
        }

        // GET: Produto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Produtos == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaId", produto.CategoriaId);
            return View(produto);
        }

        // POST: Produto/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProdutoId,Nome,Descricao,Imagem, Preco, Ativo,CategoriaId")] Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                return NotFound();
            }

            if (true)
            {
                try
                {
                    _context.Update(produto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutoExists(produto.ProdutoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaId", produto.CategoriaId);
            return View(produto);
        }

        // GET: Produto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Produtos == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos
                .Include(p => p.Categorias)
                .FirstOrDefaultAsync(m => m.ProdutoId == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // POST: Produto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Produtos == null)
            {
                return Problem("Entity set 'AppDbContext.Produtos'  is null.");
            }
            var produto = await _context.Produtos.FindAsync(id);
            if (produto != null)
            {
                _context.Produtos.Remove(produto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProdutoExists(int id)
        {
            return (_context.Produtos?.Any(e => e.ProdutoId == id)).GetValueOrDefault();
        }

        
    }


}
