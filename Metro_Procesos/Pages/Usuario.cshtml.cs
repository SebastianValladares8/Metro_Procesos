using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Metro_Procesos.Data;
using Metro_Procesos.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Metro_Procesos.Pages
{
    public class UsuarioModel : PageModel
    {
        private readonly AppDbContext _context;

        public UsuarioModel(AppDbContext context)
        {
            _context = context;
        }

        // Propiedad para enviar los datos al HTML
        public Usuario? DatosUsuario { get; set; }

        public IActionResult OnGet()
        {
            // 1. Recuperamos el ID de la sesión
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            // 2. Si no hay ID, lo mandamos al Login por seguridad
            if (usuarioId == null)
            {
                return RedirectToPage("/Index");
            }

            // 3. Buscamos tus datos reales en SQL
            DatosUsuario = _context.usuario.FirstOrDefault(u => u.Id == usuarioId);

            if (DatosUsuario == null)
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }
    }
}