using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Metro_Procesos.Pages
{
    public class ProcesarPaypalModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "El monto es obligatorio")]
        [Range(1.00, 100.00, ErrorMessage = "El monto debe estar entre $1 y $100")]
        public decimal Monto { get; set; }

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Lógica de simulación:
            // 1. Aquí llamarías a la API de PayPal.
            // 2. Como es exitoso, actualizamos el mensaje.
            TempData["Mensaje"] = $"¡Pago de ${Monto} exitoso! Su saldo ha sido acreditado instantáneamente a través de PayPal.";

            return RedirectToPage("/Index");
        }
    }
}