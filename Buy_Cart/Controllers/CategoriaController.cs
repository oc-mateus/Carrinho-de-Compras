using CarrinhoCompras.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CarrinhoCompras.Data;

namespace CarrinhoCompras.Controllers
{
    [Authorize]
    public class CategoriaController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CategoriaController(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Log()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var categorias = _context.Categorias.Where(c => c.UsuarioId == userId).ToList();
            return View(categorias);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Adicionar(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                TempData["Erro"] = "O nome da categoria não pode estar vazio.";
                return RedirectToAction("Index");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var categoria = new Categoria
            {
                Nome = nome,
                UsuarioId = userId
            };

            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Excluir(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null || categoria.UsuarioId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Unauthorized();
            }

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
