using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Metro_Procesos.Data;
using Metro_Procesos.Models;

namespace Metro_Procesos.Pages
{
    public class RegistroModel : PageModel
    {
        private readonly AppDbContext _context;

        // Constructor para conectar con la base de datos
        public RegistroModel(AppDbContext context)
        {
            _context = context;
        }

        // Esta propiedad se conecta con el 'asp-for' de tu HTML
        [BindProperty]
        public Usuario Usuario { get; set; } = new Usuario();

        public void OnGet()
        {
            // Se ejecuta al cargar la página por primera vez
        }

        public IActionResult OnPost()
        {
            // 1. Verificamos que el formulario esté bien lleno
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // 2. Agregamos el usuario (que ya trae el Nombre, Correo, Cedula y Contrasena)
                _context.usuario.Add(Usuario);

                // 3. Guardamos los cambios en SQL Server
                _context.SaveChanges();

                // 4. Si todo sale bien, lo mandamos al Login para que entre
                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                // Si hay un error (ej. correo duplicado), lo mostramos en pantalla
                ModelState.AddModelError(string.Empty, "Hubo un error al registrar: " + ex.Message);
                return Page();
            }
        }
    }
}