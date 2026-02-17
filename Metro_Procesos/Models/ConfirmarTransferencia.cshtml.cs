using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Metro_Procesos.Data;
using QRCoder; // Librería externa para generar el QR
using System.ComponentModel.DataAnnotations;

namespace Metro_Procesos.Pages
{
    public class ConfirmarTransferenciaModel : PageModel
    {
        private readonly AppDbContext _context;

        // Constructor para inyectar el contexto de la base de datos
        public ConfirmarTransferenciaModel(AppDbContext context)
        {
            _context = context;
        }

        // Propiedades vinculadas al formulario (BindProperty permite recibir datos del POST)
        [BindProperty]
        public decimal Monto { get; set; } = 0.45m;

        [BindProperty]
        public string Referencia { get; set; }

        // Propiedades para pasar datos a la vista
        public string QrCodeImage { get; set; }
        public string UsuarioNombre { get; set; }

        public void OnGet()
        {
            // Se ejecuta al cargar la página por primera vez
        }

        // Método que procesa el formulario cuando el usuario hace clic en el botón
        public async Task<IActionResult> OnPostAsync()
        {
            // 1. SEGURIDAD: Obtener el ID del usuario que está logueado mediante la Sesión
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null) return RedirectToPage("/Login");

            // 2. BASE DE DATOS: Buscar los datos completos del usuario
            var usuario = await _context.usuario.FirstOrDefaultAsync(u => u.Id == usuarioId);

            if (usuario != null)
            {
                // 3. VALIDACIÓN: Verificar que el campo de referencia no esté vacío
                if (string.IsNullOrWhiteSpace(Referencia))
                {
                    ModelState.AddModelError("Referencia", "El número de comprobante es obligatorio.");
                    return Page();
                }

                // Asignamos el nombre para el ticket
                UsuarioNombre = usuario.Nombre;

                // 4. LÓGICA DEL QR: Creamos una cadena de texto única con la info del viaje
                // Incluimos Referencia bancaria, Nombre y Fecha para evitar fraudes
                string datosQR = $"METRO|REF:{Referencia}|USER:{usuario.Nombre}|FECHA:{DateTime.Now:yyyyMMddHHmm}";

                // 5. GENERACIÓN DE IMAGEN: Usamos QRCoder para convertir el texto en una imagen PNG
                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(datosQR, QRCodeGenerator.ECCLevel.Q))
                using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
                {
                    byte[] qrBytes = qrCode.GetGraphic(20);
                    // Convertimos los bytes de la imagen a una cadena Base64 para que el navegador la entienda
                    QrCodeImage = $"data:image/png;base64,{Convert.ToBase64String(qrBytes)}";
                }

                // Recargamos la misma página pero ahora el if inicial mostrará el boleto
                return Page();
            }

            // Si algo falla con el usuario, regresamos al perfil
            return RedirectToPage("/Usuario");
        }
    }
}