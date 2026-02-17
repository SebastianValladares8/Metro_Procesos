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

        // --- Propiedades Vinculadas al Formulario ---
        [BindProperty]
        public string Titular { get; set; } = string.Empty;

        [BindProperty]
        public string NumeroTarjeta { get; set; } = string.Empty;

        [BindProperty]
        public string Expiracion { get; set; } = string.Empty;

        [BindProperty]
        public string CVV { get; set; } = string.Empty;

        // --- Propiedades para la Vista ---
        public string QrCodeImage { get; set; } = string.Empty;
        public string UsuarioNombre { get; set; } = string.Empty;
        public string TarjetaEnmascarada { get; set; } = string.Empty;

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            // 1. Obtener usuario de la sesión
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null) return RedirectToPage("/Index");

            var usuario = await _context.usuario.FirstOrDefaultAsync(u => u.Id == usuarioId);

            if (usuario != null)
            {
                // SIMULACIÓN DE PASARELA DE PAGO: 
                // En un sistema real, aquí enviaríamos los datos a Visa/Mastercard.
                // Aquí solo validamos que el número tenga 16 dígitos.
                if (NumeroTarjeta.Length < 16)
                {
                    ModelState.AddModelError("NumeroTarjeta", "Número de tarjeta inválido.");
                    return Page();
                }

                // 2. Seguridad: Enmascarar la tarjeta para el ticket (Ej: **** 1234)
                TarjetaEnmascarada = "**** **** **** " + NumeroTarjeta.Substring(NumeroTarjeta.Length - 4);

                // 3. Asignar datos para el ticket
                UsuarioNombre = usuario.Nombre;

                // 4. Generar el QR con info de la transacción
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