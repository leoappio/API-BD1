using API_TRABALHO_BD1.Entity;

namespace API_TRABALHO_BD1.Repository.Interfaces
{
    public interface IPagamentoRepository
    {
        Pagamento InserirTipoPagamento(string nome);
        Pagamento GetPagamentoPorCodigo(decimal codigo);
        IEnumerable<Pagamento> GetTodosOsTiposPagamento();
        Pagamento EditarPagamento(decimal codigo, string nome);
        void ApagarTipoPagamento(decimal codigo);
    }
}
