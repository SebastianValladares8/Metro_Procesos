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

            // 2. Redirección directa: Como el monto se pedirá en la siguiente página,
            // ya no enviamos parámetros en el RedirectToPage.
            if (Metodo == "Transferencia")
            {
                return RedirectToPage("/ConfirmarTransferencia");
            }

            // Enviamos a la página de PayPal
            return RedirectToPage("/ProcesarPaypal");
        }
    }
}
