using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using CarrinhoCompras.Models;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Metadata;
using System.Xml.Linq;
using Image = iTextSharp.text.Image;

namespace CarrinhoComprasPDF.Controllers
{
    public class PdfController : Controller
    {
        [HttpGet]
        public IActionResult DownloadPdf(string tema)
        {
            var sessionData = HttpContext.Session.GetString("ListaCompras");
            if (string.IsNullOrEmpty(sessionData)) return NotFound("Nenhuma lista disponível.");

            var listas = JsonSerializer.Deserialize<Dictionary<string, List<ItemCompra>>>(sessionData);
            if (!listas.ContainsKey(tema.ToLower()) || !listas[tema.ToLower()].Any())
                return NotFound("Tema não encontrado ou vazio.");

            var itens = listas[tema.ToLower()];

            using (var ms = new MemoryStream())
            {
                var doc = new iTextSharp.text.Document();
                var writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();

                doc.Add(new Paragraph($"Lista de {tema}") { Alignment = Element.ALIGN_CENTER });
                doc.Add(new Paragraph(" "));

                foreach (var item in itens)
                {
                    doc.Add(new Paragraph($"Produto: {item.Nome}"));
                    doc.Add(new Paragraph($"Quantidade: {item.Quantidade}"));
                    doc.Add(new Paragraph($"Preço: R$ {item.Preco:F2}"));
                    doc.Add(new Paragraph($"Status: {(item.Comprado ? "Comprado" : "Pendente")}"));

                    if (!string.IsNullOrEmpty(item.ImagemUrl))
                    {
                        try
                        {
                            var img = Image.GetInstance(item.ImagemUrl);
                            img.ScaleToFit(100f, 100f);
                            doc.Add(img);
                        }
                        catch { /* Ignora imagens inválidas */ }
                    }

                    doc.Add(new Paragraph("---------------------------"));
                }

                doc.Close();
                writer.Close();

                return File(ms.ToArray(), "application/pdf", $"Lista_{tema}.pdf");
            }
        }
    }
}
