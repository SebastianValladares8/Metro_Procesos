namespace Metro_Procesos.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Cedula { get; set; } = "";     
        public string Correo { get; set; } = "";
        public string Contrasena { get; set; } = ""; 
        public decimal Saldo { get; set; } = 0.00m; 
    }
}