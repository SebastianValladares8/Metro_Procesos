namespace Metro_Procesos.Models
{
    public class Recarga
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public decimal Monto { get; set; }
        public string MetodoPago { get; set; } = "";
        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}
