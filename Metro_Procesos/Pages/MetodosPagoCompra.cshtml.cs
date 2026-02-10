using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Metro_Procesos.Pages
{
    public class MetodosPagoCompraModel : PageModel
    {
        // Esta propiedad se enlaza con el asp-for="Metodo" de tu HTML
        [BindProperty]
        public string Metodo { get; set; }

        public void OnGet()
        {
            // Opcional: dejamos Transferencia marcada por defecto
            Metodo = "Transferencia";
        }

        public IActionResult OnPost()
        {
            // Si el usuario no seleccionó nada (aunque pusiste 'required'), volvemos a cargar
            if (string.IsNullOrEmpty(Metodo))
            {
                return Page();
            }

            // REDIRECCIÓN PURA:
            // Aquí NO procesamos dinero, solo saltamos de página
            if (Metodo == "Transferencia")
            {
                return RedirectToPage("/ConfirmarTransferencia");
            }
            else if (Metodo == "Tarjeta")
            {
                return RedirectToPage("/PagoTarjeta");
            }

            return Page();
        }
    }
}