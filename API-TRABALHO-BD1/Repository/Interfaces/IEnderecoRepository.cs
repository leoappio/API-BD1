using API_TRABALHO_BD1.Entity;

namespace API_TRABALHO_BD1.Repository.Interfaces
{
    public interface IEnderecoRepository
    {
        Endereco InserirEndereco(decimal codCliente, string logradouro, int numero, string bairro, string cidade, string CEP);
        Endereco GetEnderecoPorCodigo(decimal codigo);
        IEnumerable<Endereco> GetTodosOsEnderecoDeUmCliente(decimal codigo_cliente);
        Endereco EditarEndereco(decimal codigo, decimal codCliente, string logradouro, int numero, string bairro, string cidade, string CEP);
        void ApagarEndereco(decimal codigo);
    }
}
