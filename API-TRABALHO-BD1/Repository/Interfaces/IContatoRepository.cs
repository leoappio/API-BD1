using API_TRABALHO_BD1.Entity;

namespace API_TRABALHO_BD1.Repository.Interfaces
{
    public interface IContatoRepository
    {
        Contato InserirContato(decimal cod_cliente, string numeroCelular, string email);
        Contato GetContatoPorCodigo(decimal codigo);
        IEnumerable<Contato> GetTodosOsContatosDeUmCliente(decimal codigo_cliente);
        Contato EditarContato(decimal codigo, decimal cod_cliente, string numeroCelular, string email);
        void ApagarContato(decimal codigo);
    }
}
