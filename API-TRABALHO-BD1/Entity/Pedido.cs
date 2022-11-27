namespace API_TRABALHO_BD1.Entity
{
    public class Pedido
    {
        public decimal CodPedido { get; set; }
        public float ValorTotal { get; set; }
        public string Status { get; set; }
        public Endereco EnderecoEntrega { get; set; }
        public Cliente Cliente { get; set; }
        public Distribuidora Distribuidora { get; set; }
        public Pagamento TipoPagamento { get; set; }
        public DateTime DataPedido { get; set; }
        public List<ProdutoPedido> Produtos { get; set; }

    }
}
