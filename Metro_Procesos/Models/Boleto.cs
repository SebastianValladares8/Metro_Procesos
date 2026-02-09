namespace Metro_Procesos.Models
{
    public class Boleto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public decimal Costo { get; set; } = 0.45m; // Tarifa del Metro
        public DateTime FechaViaje { get; set; } = DateTime.Now;
        public string CodigoQr { get; set; } = "";
    }
}
