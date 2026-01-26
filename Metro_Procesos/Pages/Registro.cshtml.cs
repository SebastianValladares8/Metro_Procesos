using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Metro_Procesos.Data;
using Metro_Procesos.Models;

namespace Metro_Procesos.Pages
{
    public class RegistroModel : PageModel
    {
        private readonly AppDbContext _context;

        public RegistroModel(AppDbContext context)
        {
            _context = context;
        }

        // Esto permite que el form llene Usuario automáticamente
        [BindProperty]
        public Usuario Usuario { get; set; } = new Usuario();

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // Validación mínima (puedes mejorar luego)
            if (string.IsNullOrWhiteSpace(Usuario.Correo) ||
                string.IsNullOrWhiteSpace(Usuario.Cedula) ||
                string.IsNullOrWhiteSpace(Usuario.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Completa todos los campos.");
                return Page();
            }

            // Guardar en BD
            _context.Usuarios.Add(Usuario);
            _context.SaveChanges();

            return RedirectToPage("/Index");
        }
    }
}
