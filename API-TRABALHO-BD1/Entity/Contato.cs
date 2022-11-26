namespace API_TRABALHO_BD1.Entity
{
    public class Contato
    {
        public decimal CodContato { get; set; }
        public Cliente Cliente { get; set; }
        public string NumeroCelular { get; set; }
        public string Email { get; set; }
    }
}
