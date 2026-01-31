using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Metro_Procesos.Pages
{
    public class MetodosPagoCompraModel : PageModel
    {
        [BindProperty]
        public string? Metodo { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // Si no selecciona nada, se queda en la misma página
            if (string.IsNullOrWhiteSpace(Metodo))
            {
                ModelState.AddModelError(string.Empty, "Seleccione un método de pago.");
                return Page();
            }

            // Aquí decides a dónde ir después:
            // Ejemplo: volver a Usuario o ir a una página de confirmación
            return RedirectToPage("/Usuario");
        }
    }
}
