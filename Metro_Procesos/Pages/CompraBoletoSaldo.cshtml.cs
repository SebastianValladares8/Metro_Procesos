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

        public string QrCodeImage { get; set; }
        public string UsuarioNombre { get; set; }
        public decimal SaldoRestante { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // 1. Validar Sesión
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null) return RedirectToPage("/Index");

            // 2. Buscar Usuario y procesar cobro
            var usuario = await _context.usuario.FirstOrDefaultAsync(u => u.Id == usuarioId);

            if (usuario != null)
            {
                if (usuario.Saldo >= 0.45m)
                {
                    // RESTAR SALDO Y GUARDAR
                    usuario.Saldo -= 0.45m;
                    await _context.SaveChangesAsync();

                    UsuarioNombre = usuario.Nombre;
                    SaldoRestante = usuario.Saldo;

                    // 3. GENERAR QR
                    string datosQR = $"METRO|SALDO|USER:{usuario.Nombre}|FECHA:{DateTime.Now:yyyyMMddHHmm}";
                    using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                    using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(datosQR, QRCodeGenerator.ECCLevel.Q))
                    using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
                    {
                        byte[] qrBytes = qrCode.GetGraphic(20);
                        QrCodeImage = $"data:image/png;base64,{Convert.ToBase64String(qrBytes)}";
                    }

                    return Page();
                }
                else
                {
                    // Si no tiene saldo, lo enviamos a recargar
                    TempData["Mensaje"] = "Saldo insuficiente para comprar el boleto.";
                    return RedirectToPage("/RecargaSaldo");
                }
            }
            return RedirectToPage("/Index");
        }
    }
}