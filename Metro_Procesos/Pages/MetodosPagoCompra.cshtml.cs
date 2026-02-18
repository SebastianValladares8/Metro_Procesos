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
            if (string.IsNullOrEmpty(Metodo)) return Page();

            // AÑADE ESTA LÍNEA AQUÍ:
            if (Metodo == "Saldo") return RedirectToPage("/CompraBoletoSaldo");

            if (Metodo == "Transferencia") return RedirectToPage("/ConfirmarTransferencia");
            if (Metodo == "Tarjeta") return RedirectToPage("/PagoTarjeta");

            return Page();
        }
    }
}