using Microsoft.AspNetCore.Mvc;
using CarrinhoCompras.Models;
using System.Text.Json;


namespace CarrinhoCompras.Controllers
{
    public class HomeController : Controller
    {
            private const string SessionKey = "ListaCompras";

        

        public IActionResult Index()
            {
                var listas = GetListasFromSession();
                return View(listas);
            }

        public IActionResult Contatos()
        {
            return View();
        }

        [HttpPost]
            public IActionResult Adicionar(string nome, int quantidade, decimal preco, string categoria, string imagemUrl)
            {
                var listas = GetListasFromSession();
                if (!listas.ContainsKey(categoria))
                    listas[categoria] = new List<ItemCompra>();

                listas[categoria].Add(new ItemCompra
                {
                    Nome = nome,
                    Quantidade = quantidade,
                    Preco = preco,
                    Categoria = categoria,
                    ImagemUrl = imagemUrl
                });

                SaveListasToSession(listas);
                return RedirectToAction("Index");
            }

            [HttpPost]
            public IActionResult Marcar(Guid id)
            {
                var listas = GetListasFromSession();
                foreach (var categoria in listas.Keys)
                {
                    var item = listas[categoria].FirstOrDefault(i => i.Id == id);
                    if (item != null)
                    {
                        item.Comprado = !item.Comprado;
                        break;
                    }
                }
                SaveListasToSession(listas);
                return RedirectToAction("Index");
            }

            [HttpPost]
            public IActionResult Excluir(Guid id)
            {
                var listas = GetListasFromSession();
                foreach (var categoria in listas.Keys.ToList())
                {
                    listas[categoria] = listas[categoria].Where(i => i.Id != id).ToList();
                }
                SaveListasToSession(listas);
                return RedirectToAction("Index");
            }

            [HttpPost]
            public IActionResult Editar(Guid id, string nome, int quantidade, decimal preco, string imagemUrl)
            {
                var listas = GetListasFromSession();
                foreach (var categoria in listas.Keys)
                {
                    var item = listas[categoria].FirstOrDefault(i => i.Id == id);
                    if (item != null)
                    {
                        item.Nome = nome;
                        item.Quantidade = quantidade;
                        item.Preco = preco;
                        item.ImagemUrl = imagemUrl;
                        break;
                    }
                }
                SaveListasToSession(listas);
                return RedirectToAction("Index");
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
