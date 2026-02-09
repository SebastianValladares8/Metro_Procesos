using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Metro_Procesos.Data;
using Metro_Procesos.Models;
using System.Linq;
using Microsoft.AspNetCore.Http; // Necesario para usar sesiones

namespace Metro_Procesos.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context) { _context = context; }

        [BindProperty]
        public string Correo { get; set; } = "";
        [BindProperty]
        public string Contrasena { get; set; } = "";

        public IActionResult OnPost()
        {
            // Busca al usuario en la base de datos usando el nombre 'usuario' que definimos en AppDbContext
            var user = _context.usuario.FirstOrDefault(u => u.Correo == Correo && u.Contrasena == Contrasena);

            if (user != null)
            {
                // ¡MEMORIA DEL SISTEMA!
                // Guardamos el ID y el Correo en la sesión para usarlos en la Recarga y el Perfil
                HttpContext.Session.SetInt32("UsuarioId", user.Id);
                HttpContext.Session.SetString("UsuarioCorreo", user.Correo);

                // Si existe, lo mandamos a la página de Usuario (su perfil)
                return RedirectToPage("./Usuario");
            }

            // Si falla, mostramos el error
            ModelState.AddModelError(string.Empty, "Credenciales incorrectas. Verifica tu correo y contraseña.");
            return Page();
        }
    }
}