using Microsoft.AspNetCore.Mvc;
using App.Context;
using App.Models;

namespace App.Admin.Controllers
{
    [Area("Admin")]
    public class BannerController : Controller
    {
        private readonly AppDbContext _BancoDados;
        private readonly string _caminhoPasta;
        public BannerController(AppDbContext BancoDados, IWebHostEnvironment pastaSite)
        {
            _BancoDados = BancoDados;
            _caminhoPasta = pastaSite.WebRootPath;
        }

        [HttpPost]
        public IActionResult CriarBanner(Banner bannerNovo, IFormFile foto)
        {
            string img = SalvarFoto(foto);
            bannerNovo.Imagem = img;
            _BancoDados.Add(bannerNovo);
            _BancoDados.SaveChanges();
            return RedirectToAction(nameof(ListaBanner));

            return View(bannerNovo);
        }

        public string SalvarFoto(IFormFile imagemSelecionada)
        {
            var nome = Guid.NewGuid().ToString() + imagemSelecionada.FileName;
            var caminhoPastaFotos = _caminhoPasta + "\\fotos";
            if (!Directory.Exists(caminhoPastaFotos))
            {
                Directory.CreateDirectory(caminhoPastaFotos);
            }
            using (var stream = System.IO.File.Create(caminhoPastaFotos + "\\" + nome))
            {
                imagemSelecionada.CopyTo(stream);
            }
            return nome;
        }

        public IActionResult ListaBanner()
        {
            return View(_BancoDados.Banners.ToList());
        }

        public IActionResult CriarBanner()
        {
            return View();
        }

        public IActionResult EditarBanner(int id)
        {
            var banner = _BancoDados.Banners.Find(id);
            return View(banner);
        }

        [HttpPost]
        public IActionResult EditarBanner(int id, Banner banner)
        {
            if (ModelState.IsValid)
            {
                _BancoDados.Update(banner);
                _BancoDados.SaveChanges();
                return RedirectToAction(nameof(ListaBanner));
            }
            return View(banner);
        }

        public IActionResult DeletarBanner(int id)
        {
            var banner = _BancoDados.Banners.Find(id);
            System.IO.File.Delete(_caminhoPasta + "\\fotos\\" + banner.Imagem);
            _BancoDados.Banners.Remove(banner);
            _BancoDados.SaveChanges();
            return RedirectToAction(nameof(ListaBanner));
        }
    }


}