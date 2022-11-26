namespace API_TRABALHO_BD1.Entity
{
    public class ProdutoPedido
    {
        public Produto Produto { get; set; }
        public decimal CodPedido { get; set; }
        public int Quantidade { get; set; }
        public float ValorTotal { get; set; }
    }
}
