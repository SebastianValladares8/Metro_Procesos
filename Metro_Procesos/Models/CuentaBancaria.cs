namespace Metro_Procesos.Models
{
    public class CuentaBancaria
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; } // Relación con el usuario
        public string Banco { get; set; } = "";
        public string NumeroCuenta { get; set; } = "";
        public string TipoCuenta { get; set; } = ""; //
    }
}
