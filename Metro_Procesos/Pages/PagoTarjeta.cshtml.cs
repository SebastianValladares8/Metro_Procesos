using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Metro_Procesos.Data;
using Microsoft.EntityFrameworkCore;
using QRCoder;

namespace Metro_Procesos.Pages
{
    public class PagoTarjetaModel : PageModel
    {
        private readonly AppDbContext _context;

        public PagoTarjetaModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Titular { get; set; } = string.Empty;

        [BindProperty]
        public string NumeroTarjeta { get; set; } = string.Empty;

        [BindProperty]
        public string Expiracion { get; set; } = string.Empty;

        [BindProperty]
        public string CVV { get; set; } = string.Empty;

        public string QrCodeImage { get; set; } = string.Empty;
        public string UsuarioNombre { get; set; } = string.Empty;
        public string TarjetaEnmascarada { get; set; } = string.Empty;

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null) return RedirectToPage("/Index");

            var usuario = await _context.usuario.FirstOrDefaultAsync(u => u.Id == usuarioId);

            if (usuario != null)
            {
                // Validación de tarjeta
                if (string.IsNullOrEmpty(NumeroTarjeta) || NumeroTarjeta.Length < 16)
                {
                    ModelState.AddModelError("NumeroTarjeta", "Número de tarjeta inválido.");
                    return Page();
                }

                // 1. Preparar datos para la factura
                UsuarioNombre = usuario.Nombre;
                TarjetaEnmascarada = "**** **** **** " + NumeroTarjeta.Substring(NumeroTarjeta.Length - 4);

                // 2. Generar el QR
                string contenidoQR = $"METRO|TARJETA|{usuario.Nombre}|{DateTime.Now:yyyyMMddHHmm}";

                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(contenidoQR, QRCodeGenerator.ECCLevel.Q))
                using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
                {
                    byte[] qrBytes = qrCode.GetGraphic(20);
                    QrCodeImage = $"data:image/png;base64,{Convert.ToBase64String(qrBytes)}";
                }

                return Page();
            }

            TempData["Mensaje"] = "Error al procesar el pago.";
            return RedirectToPage("/Usuario");
        }
    }
}