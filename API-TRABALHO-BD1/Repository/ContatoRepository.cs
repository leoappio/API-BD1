using API_TRABALHO_BD1.Entity;
using API_TRABALHO_BD1.Repository.Interfaces;
using Microsoft.Data.SqlClient;

namespace API_TRABALHO_BD1.Repository
{
    public class ContatoRepository : IContatoRepository
    {
        private readonly IConfiguration configuration;
        private readonly IClienteRepository clienteRepository;
        private readonly SqlConnection sqlConnection;

        public ContatoRepository(IConfiguration configuration, IClienteRepository clienteRepository)
        {
            this.configuration = configuration;
            sqlConnection = new SqlConnection(configuration.GetConnectionString("dbAzure"));
            this.clienteRepository = clienteRepository;
        }

        public Contato InserirContato(decimal cod_cliente, string numeroCelular, string email)
        {
            var query = $@"INSERT INTO CONTATO VALUES({cod_cliente},'{numeroCelular}', '{email}');
                           SELECT SCOPE_IDENTITY()";

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                var id = command.ExecuteScalar();
                sqlConnection.Close();

                return new Contato
                {
                    CodContato = (decimal)id,
                    Cliente = clienteRepository.GetClientePorCodigo(cod_cliente),
                    NumeroCelular = numeroCelular,
                    Email = email
                };
            }
        }

        public Contato GetContatoPorCodigo(decimal codigo)
        {
            var query = $"SELECT * FROM CONTATO WHERE COD_CONTATO = {codigo}";
            Contato contato = null;
            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    contato = new Contato
                    {
                        CodContato = codigo,
                        Cliente = clienteRepository.GetClientePorCodigo((int)reader["cod_cliente"]),
                        NumeroCelular = (string)reader["num_celular"],
                        Email = (string)reader["email"]
                    };
                }
            }
            sqlConnection.Close();
            return contato;
        }
        public IEnumerable<Contato> GetTodosOsContatosDeUmCliente(decimal codigo_cliente)
        {
            var query = $"SELECT * FROM CONTATO WHERE COD_CLIENTE = {codigo_cliente}";

            var listaContato = new List<Contato>();

            var cliente = clienteRepository.GetClientePorCodigo(codigo_cliente);

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    listaContato.Add(new Contato
                    {
                        CodContato = (int)reader["cod_contato"],
                        Cliente = cliente,
                        NumeroCelular = (string)reader["num_celular"],
                        Email = (string)reader["email"]
                    });
                }

                sqlConnection.Close();
            }
            return listaContato;
        }

        public Contato EditarContato(decimal codigo, decimal cod_cliente, string numeroCelular, string email)
        {
            var query = $@"UPDATE CONTATO 
                           SET COD_CLIENTE = {cod_cliente},
                           NUM_CELULAR = '{numeroCelular}',
                           EMAIL = '{email}'
                           WHERE COD_CONTATO = {codigo}";

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                command.ExecuteNonQuery();
                sqlConnection.Close();

                return new Contato
                {
                    CodContato = codigo,
                    Cliente = clienteRepository.GetClientePorCodigo(cod_cliente),
                    NumeroCelular = numeroCelular,
                    Email = email
                };
            }
        }

        public void ApagarContato(decimal codigo)
        {
            var query = $"DELETE FROM CONTATO WHERE COD_CONTATO = {codigo}";

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                command.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}
