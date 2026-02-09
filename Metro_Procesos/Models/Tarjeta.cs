namespace Metro_Procesos.Models
{
    public class Tarjeta
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string NombreTitular { get; set; } = "";
        public string NumeroTarjeta { get; set; } = "";
        public string MesExpiracion { get; set; } = "";
        public string AnioExpiracion { get; set; } = "";
    }
}
