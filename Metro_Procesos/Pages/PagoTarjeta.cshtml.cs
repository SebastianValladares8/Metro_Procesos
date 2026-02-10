using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Metro_Procesos.Data;
using Metro_Procesos.Models;
using Microsoft.AspNetCore.Http; // Para usar Session

namespace Metro_Procesos.Pages
{
    public class PagoTarjetaModel : PageModel
    {
        private readonly AppDbContext _context;

        public PagoTarjetaModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public decimal Monto { get; set; }

        public void OnGet() { }

        public IActionResult OnPost()
        {
            // VALIDACIÓN: Si el monto no es válido, no hace nada
            if (!ModelState.IsValid || Monto <= 0)
            {
                return Page();
            }

            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null) return RedirectToPage("/Index");

            var user = _context.usuario.Find(usuarioId);

            if (user != null)
            {
                // 1. Actualizar saldo del usuario (S mayúscula)
                user.Saldo += Monto;

                // 2. Crear registro de recarga
                var nuevaRecarga = new Recarga
                {
                    UsuarioId = user.Id,
                    Monto = Monto,
                    MetodoPago = "Tarjeta de Crédito/Débito",
                    Fecha = DateTime.Now
                };

                // Asegúrate que en AppDbContext el DbSet se llame 'recarga'
                _context.recarga.Add(nuevaRecarga);

                try
                {
                    // 3. Guardar cambios en SQL Server
                    _context.SaveChanges();

                    TempData["Mensaje"] = $"¡Recarga exitosa de ${Monto} con tarjeta!";
                    return RedirectToPage("/Usuario");
                }
                catch (Exception ex)
                {
                    // Si vuelve a salir "Invalid column name", el error está en Recarga.cs
                    ModelState.AddModelError(string.Empty, "Error al guardar en la base de datos.");
                    return Page();
                }
            }

            return Page();
        }
    }
}