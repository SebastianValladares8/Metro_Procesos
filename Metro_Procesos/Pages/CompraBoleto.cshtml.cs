using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Metro_Procesos.Data;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Metro_Procesos.Pages
{
    public class CompraBoletoModel : PageModel
    {
        private readonly AppDbContext _context;
        public CompraBoletoModel(AppDbContext context) { _context = context; }

        [BindProperty]
        public string Metodo { get; set; } = string.Empty;

        public async Task<IActionResult> OnPostAsync()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null) return RedirectToPage("/Index");

            // Buscamos al usuario en la base de datos
            var usuario = await _context.usuario.FirstOrDefaultAsync(u => u.Id == usuarioId);

            if (usuario != null && usuario.Saldo >= 0.45m)
            {
                // Restar saldo
                usuario.Saldo -= 0.45m;
                string ticketCode = "MQ-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
                await _context.SaveChangesAsync();

                // Generar PDF
                var pdfData = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A6);
                        page.Margin(1, Unit.Centimetre);
                        page.Header().Text("METRO DE QUITO").FontSize(20).SemiBold().FontColor(Colors.Red.Medium);
                        page.Content().Column(c => {
                            c.Item().Text($"Pasajero: {usuario.Nombre}");
                            c.Item().Text($"Método: {Metodo}");
                            c.Item().PaddingTop(10).AlignCenter().Text($"CÓDIGO: {ticketCode}").FontSize(16).Bold();
                        });
                    });
                }).GeneratePdf();

                // Retornar archivo (Esto detiene el bucle y descarga el PDF)
                return File(pdfData, "application/pdf", $"Boleto_{ticketCode}.pdf");
            }
            return Page();
        }
    }
}