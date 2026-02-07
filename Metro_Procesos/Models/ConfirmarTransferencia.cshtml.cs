using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Metro_Procesos.Pages
{
    public class ConfirmarTransferenciaModel : PageModel
    {
        // Validaciones simples para que no estén vacíos
        [BindProperty]
        [Required(ErrorMessage = "El monto es obligatorio")]
        public decimal Monto { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "La referencia es obligatoria")]
        public string Referencia { get; set; }

        public void OnGet() { }

        public IActionResult OnPost()
        {
            // Verificamos si las validaciones del modelo se cumplen
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Aquí simulamos que guardamos la información
            // Usamos TempData para pasar un mensaje a la siguiente página
            TempData["Mensaje"] = $"Transferencia de ${Monto} (Ref: {Referencia}) recibida. En proceso de validación.";

            // Redirigimos al inicio o a una página de éxito
            return RedirectToPage("/Index");
        }
    }
}