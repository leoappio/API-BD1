using API_TRABALHO_BD1.Entity;
using API_TRABALHO_BD1.Repository.Interfaces;
using Microsoft.Data.SqlClient;

namespace API_TRABALHO_BD1.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly IConfiguration configuration;
        private readonly SqlConnection sqlConnection;

        public ClienteRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            sqlConnection = new SqlConnection(configuration.GetConnectionString("dbAzure"));
        }

        public Cliente InserirCliente(decimal idade, string cpf, string nome)
        {
            var query = $@"INSERT INTO CLIENTE VALUES({idade},'{cpf}', '{nome}');
                           SELECT SCOPE_IDENTITY()";

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                var id = command.ExecuteScalar();
                sqlConnection.Close();

                return new Cliente
                {
                    CodCliente = (decimal)id,
                    Idade = idade,
                    CPF = cpf,
                    Nome = nome
                };
            }
        }

        public Cliente GetClientePorCodigo(decimal codigo)
        {
            var query = $"SELECT * FROM CLIENTE WHERE COD_CLIENTE = {codigo}";
            Cliente cliente = null;
            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    cliente = new Cliente
                    {
                        CodCliente = (int)reader["cod_cliente"],
                        Idade = (int)reader["idade"],
                        CPF = (string)reader["cpf"],
                        Nome = (string)reader["nome"]
                    };
                }
            }
            sqlConnection.Close();
            return cliente;
        }
        public IEnumerable<Cliente> GetTodosOsClientes()
        {
            var query = $"SELECT * FROM CLIENTE";

            var listaClientes = new List<Cliente>();

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    listaClientes.Add(new Cliente
                    {
                        CodCliente = (int)reader["cod_cliente"],
                        Idade = (int)reader["idade"],
                        CPF = (string)reader["cpf"],
                        Nome = (string)reader["nome"]
                    });
                }

                sqlConnection.Close();
            }
            return listaClientes;
        }

        public Cliente EditarCliente(decimal codigo, decimal idade, string cpf, string nome)
        {
            var query = $@"UPDATE CLIENTE 
                           SET IDADE = {idade},
                           CPF = '{cpf}',
                           NOME = '{nome}'
                           WHERE COD_CLIENTE = {codigo}";

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                command.ExecuteNonQuery();
                sqlConnection.Close();

                return new Cliente
                {
                    CodCliente = (decimal)id,
                    Idade = idade,
                    CPF = cpf,
                    Nome = nome
                };
            }
        }

        public void ApagarCliente(decimal codigo)
        {
            var query = $"DELETE FROM CLIENTE WHERE COD_CLIENTE = {codigo}";

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                command.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}
