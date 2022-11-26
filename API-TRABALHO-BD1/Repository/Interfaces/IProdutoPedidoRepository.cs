using API_TRABALHO_BD1.Entity;

namespace API_TRABALHO_BD1.Repository.Interfaces
{
    public interface IProdutoPedidoRepository
    {
        public List<ProdutoPedido> GetProdutosDeUmPedido(int cod_pedido);
        public void InserirProdutoEmUmPedido(int codProduto, int codPedido, int quantidade, float valorTotal);
        public void RemoverProdutoDeUmPedido(int codProduto, int codPedido);
        public bool ProdutoEstaEmUmPedido(int codProduto);
    }
}
