using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ImpresionesCB.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImpresionController : ControllerBase
    {

        [HttpPost("imprimir")]
        public IActionResult ImprimirEtiquetas([FromBody] List<string> listaCodigos)
        {
            if (listaCodigos == null || listaCodigos.Count == 0)
            {
                return BadRequest("La lista de códigos está vacía.");
            }

            foreach (var codigo in listaCodigos)
            {
                Console.WriteLine($"Preparando impresión para el código: {codigo}");
            }

            return Ok(new { mensaje = "¡Conexión exitosa! La API recibió " + listaCodigos.Count + " códigos." });
        }

        [HttpPost("generar-pdf")]
        public IActionResult GenerarPdf([FromBody] List<string> listaCodigos) 
        {
            if (listaCodigos == null || listaCodigos.Count == 0)
            {
                return BadRequest("La lista de códigos está vacía.");
            }

            QuestPDF.Settings.License = LicenseType.Community;

            try
            {
                string nombreArchivo = "Etiquetas_IEA_Prueba.pdf";
                string rutaEscritorio = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), nombreArchivo);

                Document.Create(container =>
                {
                    foreach (var codigo in listaCodigos)
                    {
                        container.Page(page =>
                        {
                            page.Size(new PageSize(216, 108));
                            page.Margin(5);
                            page.DefaultTextStyle(x => x.FontSize(8).FontFamily(Fonts.Verdana));

                            page.Content().Column(col =>
                            {
                                col.Item().Text("I. E. A.").FontSize(14).SemiBold().AlignCenter();
                                col.Item().Text("Dirección de Finanzas y Administración").FontSize(6).AlignCenter();


                                col.Item().PaddingVertical(2).Height(35).AlignCenter().Background(Colors.Grey.Lighten4)
                                    .Text("||||||||||||||||||||||||||||||||||||||||||").FontSize(10);

                                col.Item().Text(codigo).FontSize(10).SemiBold().AlignCenter();
                                col.Item().Text("Control Patrimonial").FontSize(8).AlignCenter();
                            });
                        });
                    }
                }).GeneratePdf(rutaEscritorio);

                return Ok(new { mensaje = "¡Éxito! El PDF se guardó en tu escritorio." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno: " + ex.Message });
            }
        }

    }
} 