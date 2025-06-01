using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using CarrinhoCompras.Data;
using CarrinhoCompras.Models;

namespace CarrinhoCompras.Controllers
{
    [Authorize]
    public class ListaController : Controller
    {
        private readonly AppDbContext _context;

        public ListaController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var categorias = await _context.Categorias
                .Include(c => c.Itens)
                .Where(c => c.UsuarioId == userId)
                .ToListAsync();

            var listas = categorias.ToDictionary(c => c.Nome, c => c.Itens.ToList());
            return View(listas);
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar(string nome, int quantidade, decimal preco, string categoria, string imagemUrl)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var categoriaExistente = await _context.Categorias
                .FirstOrDefaultAsync(c => c.Nome == categoria && c.UsuarioId == userId);

            if (categoriaExistente == null)
            {
                categoriaExistente = new Categoria
                {
                    Nome = categoria,
                    UsuarioId = userId,
                    Itens = new List<ItemCompra>()
                };
                _context.Categorias.Add(categoriaExistente);
            }

            categoriaExistente.Itens.Add(new ItemCompra
            {
                Nome = nome,
                Quantidade = quantidade,
                Preco = preco,
                Imagem = imagemUrl,
                Comprado = false
            });

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Marcar(Guid id)
        {
            var item = await _context.Itens.FindAsync(id);
            if (item != null)
            {
                item.Comprado = !item.Comprado;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Excluir(Guid id)
        {
            var item = await _context.Itens.FindAsync(id);
            if (item != null)
            {
                _context.Itens.Remove(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Guid id, string nome, int quantidade, decimal preco, string imagemUrl)
        {
            var item = await _context.Itens.FindAsync(id);
            if (item != null)
            {
                item.Nome = nome;
                item.Quantidade = quantidade;
                item.Preco = preco;
                item.Imagem = imagemUrl;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
