using Microsoft.AspNetCore.Mvc;
using App.Context;
using App.Models;
namespace App.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;
        public LoginController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(Usuario usuario)
        {
            // Buscando no banco de dados o se há um usuário igual aos dados recebidos no form
            Usuario usuarioAutenticado = _context.Usuarios.FirstOrDefault(u => u.Login == usuario.Login && u.Senha == usuario.Senha);
            if (usuarioAutenticado != null)

            {
                // Autenticação bem-sucedida, faça o redirecionamento para a Admin
                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }
            else
            {
                // Autenticação falhou, exiba uma mensagem de erro
                ViewBag.ErrorMessage = "Nome de usuário ou senha incorretos.";
                return View();
            }
        }
    }
}