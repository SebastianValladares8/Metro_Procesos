using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Metro_Procesos.Data;
using QRCoder;

namespace Metro_Procesos.Pages
{
    public class CompraBoletoSaldoModel : PageModel
    {
        private readonly AppDbContext _context;
        public CompraBoletoSaldoModel(AppDbContext context) { _context = context; }

        // Propiedades requeridas por la vista .cshtml
        public string QrCodeImage { get; set; }
        public string UsuarioNombre { get; set; }
        public decimal SaldoRestante { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null) return RedirectToPage("/Login");

            var usuario = await _context.usuario.FirstOrDefaultAsync(u => u.Id == usuarioId);

            // Verificamos si tiene saldo suficiente ($0.45)
            if (usuario != null && usuario.Saldo >= 0.45m)
            {
                // 1. Proceso de cobro
                usuario.Saldo -= 0.45m;
                await _context.SaveChangesAsync();

                // 2. Preparar datos para la factura
                UsuarioNombre = usuario.Nombre;
                SaldoRestante = usuario.Saldo;

                // 3. Generar el QR
                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                using (QRCodeData qrCodeData = qrGenerator.CreateQrCode($"METRO|{usuario.Nombre}|{DateTime.Now}", QRCodeGenerator.ECCLevel.Q))
                using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
                {
                    byte[] qrBytes = qrCode.GetGraphic(20);
                    QrCodeImage = $"data:image/png;base64,{Convert.ToBase64String(qrBytes)}";
                }
                return Page();
            }

            return RedirectToPage("/Usuario");
        }
    }
}