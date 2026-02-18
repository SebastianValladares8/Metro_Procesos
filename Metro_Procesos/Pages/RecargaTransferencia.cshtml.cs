using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Metro_Procesos.Data;
using Microsoft.AspNetCore.Http;

namespace Metro_Procesos.Pages
{
    public class RecargaTransferenciaModel : PageModel
    {
        private readonly AppDbContext _context;

        public RecargaTransferenciaModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public decimal MontoRecarga { get; set; }

        [BindProperty]
        public string Referencia { get; set; } = string.Empty;

        public void OnGet()
        {
            // Verificación de sesión
            if (HttpContext.Session.GetInt32("UsuarioId") == null)
            {
                Response.Redirect("/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // 1. Obtener el ID del usuario logueado
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null) return RedirectToPage("/Index");

            // 2. Validar que la referencia no sea nula
            if (string.IsNullOrWhiteSpace(Referencia))
            {
                ModelState.AddModelError("Referencia", "La referencia es obligatoria.");
                return Page();
            }

            // 3. Buscar el usuario en la base de datos
            var usuario = await _context.usuario.FirstOrDefaultAsync(u => u.Id == usuarioId);

            if (usuario != null)
            {
                // 4. Actualizar el saldo sumando la recarga
                usuario.Saldo += MontoRecarga;

                // 5. Guardar cambios en SQL
                await _context.SaveChangesAsync();

                // 6. Notificar éxito y redirigir al perfil
                TempData["Mensaje"] = $"Transferencia registrada. Se han acreditado {MontoRecarga:C2} a su cuenta.";
                return RedirectToPage("/Usuario");
            }

            return Page();
        }
    }
}