using API_TRABALHO_BD1.Entity;

namespace API_TRABALHO_BD1.Repository.Interfaces
{
    public interface IDistribuidoraRepository
    {
        Distribuidora InserirDistribuidora(string endereco, int numEstrelas, int entregasRealizadas);
        Distribuidora GetDistribuidoraPorCodigo(decimal codigo);
        IEnumerable<Distribuidora> GetTodasAsDistribuidoras();
        Distribuidora EditarDistribuidora(decimal codigo, string endereco, int numEstrelas, int entregasRealizadas);
        Distribuidora AdicionarUmaEntrega(decimal codigo, int qntdEntregasAnterior);
        void ApagarDistribuidora(decimal codigo);
    }
}
