using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Metro_Procesos.Pages
{
    public class ConfirmarCuentaCiudadModel : PageModel
    {
        [BindProperty]
        public decimal Monto { get; set; } = 0.45m; // Tarifa estándar

        public void OnGet() { }

        public IActionResult OnPost()
        {
            // Simulación de validación de saldo
            // Aquí llamarías a tu base de datos en un proyecto real

            TempData["Mensaje"] = $"¡Pago exitoso! Se han descontado ${Monto} de su Cuenta Ciudad. Saldo actualizado.";

            return RedirectToPage("/Index");
        }
    }
}
