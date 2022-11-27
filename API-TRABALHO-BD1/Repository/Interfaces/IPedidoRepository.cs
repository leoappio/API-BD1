using API_TRABALHO_BD1.Entity;

namespace API_TRABALHO_BD1.Repository.Interfaces
{
    public interface IPedidoRepository
    {
        public Pedido GetPedidoPorCodigo(decimal codigoPedido);
        public List<Pedido> GetTodosOsPedidosDeUmCliente(decimal codigoCliente);
        public Pedido InserirPedido(float valorTotal, string status,
            int codEndereco, int codCliente, int codDistribuidora, int codTipoPagamento, DateTime dataPedido);
        public Pedido EditarPedido(decimal codigo, string status,
            int codEndereco, int codCliente, int codDistribuidora, int codTipoPagamento, DateTime dataPedido);
        public Pedido EditarStatus(decimal codigo, string status);
        public void ApagarPedido(decimal codigo);
        public List<RelatorioTipoDePagamento> GetRelatorioTipoDePagamento();
        public List<RelatorioValorMedioPorCategoria> GetRelatorioValorMedioPorCategoria();
        public List<RelatorioVendasMensal> GetRelatorioVendasMensal();

    }
}
