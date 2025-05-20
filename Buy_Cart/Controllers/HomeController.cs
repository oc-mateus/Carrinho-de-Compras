using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using CarrinhoCompras.Models;


namespace CarrinhoComprasMVC.Controllers
{
    public class HomeController : Controller
    {
        private const string SessionKey = "ListaCompras";

        public IActionResult Index()
        {
            var listas = GetListasFromSession();
            ViewBag.TotalPendente = listas.Values.SelectMany(x => x)
                .Where(i => !i.Comprado)
                .Sum(i => i.Preco * i.Quantidade);

            return View(listas);
        }

        [HttpPost]
        public IActionResult Adicionar(string nome, int quantidade, decimal preco, string categoria)
        {
            var listas = GetListasFromSession();

            if (!listas.ContainsKey(categoria))
                listas[categoria] = new List<ItemCompra>();

            listas[categoria].Add(new ItemCompra
            {
                Nome = nome,
                Quantidade = quantidade,
                Preco = preco,
                Categoria = categoria
            });

            SaveListasToSession(listas);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Marcar(Guid id)
        {
            var listas = GetListasFromSession();

            foreach (var lista in listas.Values)
            {
                var item = lista.FirstOrDefault(i => i.Id == id);
                if (item != null)
                {
                    item.Comprado = !item.Comprado;
                    break;
                }
            }

            SaveListasToSession(listas);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Exportar()
        {
            var listas = GetListasFromSession();
            var json = JsonSerializer.Serialize(listas);
            var bytes = System.Text.Encoding.UTF8.GetBytes(json);
            return File(bytes, "application/json", "lista_compras.json");
        }

        private Dictionary<string, List<ItemCompra>> GetListasFromSession()
        {
            var json = HttpContext.Session.GetString(SessionKey);
            if (string.IsNullOrEmpty(json))
                return new Dictionary<string, List<ItemCompra>>();
            return JsonSerializer.Deserialize<Dictionary<string, List<ItemCompra>>>(json);
        }

        private void SaveListasToSession(Dictionary<string, List<ItemCompra>> listas)
        {
            var json = JsonSerializer.Serialize(listas);
            HttpContext.Session.SetString(SessionKey, json);
        }
    }
}
