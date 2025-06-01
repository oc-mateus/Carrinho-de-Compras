using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CarrinhoCompras.Data;
using CarrinhoCompras.Models;
using Buy_Cart.Models;

namespace CarrinhoCompras.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var totalCategorias = await _context.Categorias.CountAsync();
            var totalItens = await _context.Itens.CountAsync();
            var itensComprados = await _context.Itens.Where(i => i.Comprado).CountAsync();
            var itensPendentes = totalItens - itensComprados;

            var resumo = new ResumoViewModel
            {
                TotalCategorias = totalCategorias,
                TotalItens = totalItens,
                ItensComprados = itensComprados,
                ItensPendentes = itensPendentes
            };

            return View(resumo);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
