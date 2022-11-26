using API_TRABALHO_BD1.Entity;

namespace API_TRABALHO_BD1.Repository.Interfaces
{
    public interface ILojaRepository
    {
        Loja InserirLoja(string endereco, string CNPJ, int numEstrelas);
        Loja GetLojaPorCodigo(decimal codigo);
        IEnumerable<Loja> GetTodasAsLojas();
        Loja EditarLoja(decimal codigo, string endereco, string CNPJ, int numEstrelas);
        void ApagarLoja(decimal codigo);
    }
}
