namespace API_TRABALHO_BD1.Entity
{
    public class Endereco
    {
        public decimal CodEndereco { get; set; }
        public Cliente Cliente { get; set; }
        public string Logradouro { get; set; }
        public int Numero { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string CEP { get; set; }
    }
}
