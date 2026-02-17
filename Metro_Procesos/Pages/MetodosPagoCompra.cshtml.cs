using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Metro_Procesos.Pages
{
    public class MetodosPagoCompraModel : PageModel
    {
        [BindProperty]
        public string Metodo { get; set; } = string.Empty;

        public IActionResult OnPost()
        {
            // Si elige transferencia, lo mandamos a tu página de transferencia
            if (Metodo == "Transferencia")
            {
                return RedirectToPage("/ConfirmarTransferencia");
            }
            // Si elige tarjeta, lo mandamos a tu página de tarjeta
            else if (Metodo == "Tarjeta")
            {
                return RedirectToPage("/PagoTarjeta");
            }

            return Page();
        }
    }
}