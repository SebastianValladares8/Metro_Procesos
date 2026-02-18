using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Metro_Procesos.Pages
{
    public class RecargaSaldoModel : PageModel
    {
        // Esta propiedad guarda el método elegido (Transferencia o Paypal)
        [BindProperty]
        public string Metodo { get; set; }

        public void OnGet() { }

        // Eliminamos el parámetro decimal montoRecarga porque ya no viene del formulario
        public IActionResult OnPost()
        {
            // 1. Validación: Si no se seleccionó un método de pago, recargamos la página
            if (string.IsNullOrEmpty(Metodo)) return Page();

            // 2. Redirección a las nuevas páginas de RECARGA específicas
            if (Metodo == "Transferencia")
            {
                // Redirige a RecargaTransferencia.cshtml
                return RedirectToPage("/RecargaTransferencia");
            }
            else if (Metodo == "Tarjeta")
            {
                // Redirige a RecargaTarjeta.cshtml
                return RedirectToPage("/RecargaTarjeta");
            }

            return Page();
        }
    }
}
