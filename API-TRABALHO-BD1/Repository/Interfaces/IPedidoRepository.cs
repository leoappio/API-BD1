using API_TRABALHO_BD1.Entity;

namespace API_TRABALHO_BD1.Repository.Interfaces
{
    public interface IPedidoRepository
    {
        public Pedido GetPedidoPorCodigo(decimal codigoPedido);
        public List<Pedido> GetTodosOsPedidosDeUmCliente(decimal codigoCliente);
        public Pedido InserirPedido(float valorTotal, string status,
            int codEndereco, int codCliente, int codDistribuidora, int codTipoPagamento);
        public Pedido EditarPedido(decimal codigo, float valorTotal, string status,
            int codEndereco, int codCliente, int codDistribuidora, int codTipoPagamento);
        public Pedido EditarStatus(decimal codigo, string status);

    }
}
