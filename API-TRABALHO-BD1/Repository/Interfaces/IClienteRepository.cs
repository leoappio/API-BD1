using API_TRABALHO_BD1.Entity;

namespace API_TRABALHO_BD1.Repository.Interfaces
{
    public interface IClienteRepository
    {
        Cliente InserirCliente(decimal idade, string cpf, string nome);
        Cliente GetClientePorCodigo(decimal codigo);
        IEnumerable<Cliente> GetTodosOsClientes();
        Cliente EditarCliente(decimal codigo, decimal idade, string cpf, string nome);
        void ApagarCliente(decimal codigo);
    }
}
