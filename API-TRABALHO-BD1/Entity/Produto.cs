namespace API_TRABALHO_BD1.Entity
{
    public class Produto
    {
        public decimal CodProduto { get; set; }
        public string Nome { get; set; }
        public float Preco { get; set; }
        public Categoria Categoria { get; set; }
        public Loja Loja { get; set; }
    }
}
