using API_TRABALHO_BD1.Entity;
using API_TRABALHO_BD1.Repository.Interfaces;
using Microsoft.Data.SqlClient;

namespace API_TRABALHO_BD1.Repository
{
    public class EnderecoRepository : IEnderecoRepository
    {
        private readonly IConfiguration configuration;
        private readonly IClienteRepository clienteRepository;
        private readonly SqlConnection sqlConnection;

        public EnderecoRepository(IConfiguration configuration, IClienteRepository clienteRepository)
        {
            this.configuration = configuration;
            sqlConnection = new SqlConnection(configuration.GetConnectionString("dbAzure"));
            this.clienteRepository = clienteRepository;
        }

        public Endereco InserirEndereco(decimal codCliente, string logradouro, int numero, string bairro, string cidade, string CEP)
        {
            var query = $@"INSERT INTO ENDERECO VALUES({codCliente},'{logradouro}', {numero}, '{bairro}', '{cidade}', '{CEP}');
                           SELECT SCOPE_IDENTITY()";

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                var id = command.ExecuteScalar();
                sqlConnection.Close();

                return new Endereco
                {
                    CodEndereco = (decimal)id,
                    Cliente = clienteRepository.GetClientePorCodigo(codCliente),
                    Logradouro = logradouro,
                    Numero = numero,
                    Bairro = bairro,
                    Cidade = cidade,
                    CEP = CEP
                };
            }
        }

        public Endereco GetEnderecoPorCodigo(decimal codigo)
        {
            var query = $"SELECT * FROM ENDERECO WHERE COD_ENDERECO = {codigo}";
            Endereco endereco = null;
            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    endereco = new Endereco
                    {
                        CodEndereco = codigo,
                        Cliente = clienteRepository.GetClientePorCodigo((int)reader["cod_cliente"]),
                        Logradouro = (string)reader["logradouro"],
                        Numero = (int)reader["num_imovel"],
                        Bairro = (string)reader["bairro"],
                        Cidade = (string)reader["cidade"],
                        CEP = (string)reader["CEP"],
                    };
                }
            }
            sqlConnection.Close();
            return endereco;
        }
        public IEnumerable<Endereco> GetTodosOsEnderecoDeUmCliente(decimal codigo_cliente)
        {
            var query = $"SELECT * FROM ENDERECO WHERE COD_CLIENTE = {codigo_cliente}";

            var listaEndereco = new List<Endereco>();

            var cliente = clienteRepository.GetClientePorCodigo(codigo_cliente);

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    listaEndereco.Add(new Endereco
                    {
                        CodEndereco = (int)reader["cod_endereco"],
                        Cliente = cliente,
                        Logradouro = (string)reader["logradouro"],
                        Numero = (int)reader["num_imovel"],
                        Bairro = (string)reader["bairro"],
                        Cidade = (string)reader["cidade"],
                        CEP = (string)reader["CEP"],
                    });
                }

                sqlConnection.Close();
            }
            return listaEndereco;
        }

        public Endereco EditarEndereco(decimal codigo, decimal codCliente, string logradouro, int numero, string bairro, string cidade, string CEP)
        {
            var query = $@"UPDATE ENDERECO 
                           SET COD_CLIENTE = {codCliente},
                           LOGRADOURO = '{logradouro}',
                           NUM_IMOVEL = '{numero}',
                           BAIRRO = '{bairro}',
                           CIDADE = '{cidade}',
                           CEP = '{CEP}'
                           WHERE COD_ENDERECO = {codigo}";

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                command.ExecuteNonQuery();
                sqlConnection.Close();

                return new Endereco
                {
                    CodEndereco = codigo,
                    Cliente = clienteRepository.GetClientePorCodigo(codCliente),
                    Logradouro = logradouro,
                    Numero = numero,
                    Bairro = bairro,
                    Cidade = cidade,
                    CEP = CEP
                };
            }
        }

        public void ApagarEndereco(decimal codigo)
        {
            var query = $"DELETE FROM ENDERECO WHERE COD_ENDERECO = {codigo}";

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                command.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}
