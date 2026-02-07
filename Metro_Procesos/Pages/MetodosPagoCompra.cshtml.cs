using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Metro_Procesos.Pages
{
    public class MetodosPagoCompraModel : PageModel
    {
        [BindProperty]
        public string Metodo { get; set; }

        public void OnGet()
        {
            // Opción por defecto
            Metodo = "CuentaCiudad";
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(Metodo))
            {
                return Page();
            }

            // Aquí es donde "llamamos" a las otras clases/páginas
            return Metodo switch
            {
                "CuentaCiudad" => RedirectToPage("/ConfirmarCuentaCiudad"),
                "Transferencia" => RedirectToPage("/ConfirmarTransferencia"),
                "Paypal" => RedirectToPage("/ProcesarPaypal"),
                _ => Page(),
            };
        }
    }
}
