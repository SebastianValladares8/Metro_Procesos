using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Metro_Procesos.Data; // Asegúrate que este sea el namespace de tu AppDbContext
using Microsoft.AspNetCore.Http;

namespace Metro_Procesos.Pages
{
    public class RecargaTarjetaModel : PageModel
    {
        private readonly AppDbContext _context;

        public RecargaTarjetaModel(AppDbContext context)
        {
            _context = context;
        }

        // Estas 5 propiedades eliminan los errores CS1061 del HTML
        [BindProperty]
        public decimal MontoRecarga { get; set; }

        [BindProperty]
        public string Titular { get; set; } = string.Empty;

        [BindProperty]
        public string NumeroTarjeta { get; set; } = string.Empty;

        [BindProperty]
        public string Expiracion { get; set; } = string.Empty;

        [BindProperty]
        public string CVV { get; set; } = string.Empty;

        public void OnGet()
        {
            // Verifica que el usuario esté logueado al cargar la página
            if (HttpContext.Session.GetInt32("UsuarioId") == null)
            {
                Response.Redirect("/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // 1. Recuperamos el ID del usuario de la sesión
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null) return RedirectToPage("/Index");

            // 2. Buscamos al usuario en la base de datos SQL
            var usuario = await _context.usuario.FirstOrDefaultAsync(u => u.Id == usuarioId);

            if (usuario != null)
            {
                // 3. ACTUALIZACIÓN DE SALDO: Sumamos el monto ingresado
                usuario.Saldo += MontoRecarga;

                // 4. Guardamos los cambios en SQL Server
                await _context.SaveChangesAsync();

                // 5. Mensaje de confirmación para la página de perfil
                TempData["Mensaje"] = $"Recarga exitosa. Se han añadido {MontoRecarga:C2} a su cuenta.";

                return RedirectToPage("/Usuario");
            }

            return Page();
        }
    }
}