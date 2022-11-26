using API_TRABALHO_BD1.Entity;
using API_TRABALHO_BD1.Repository.Interfaces;
using Microsoft.Data.SqlClient;

namespace API_TRABALHO_BD1.Repository
{
    public class DistribuidoraRepository : IDistribuidoraRepository
    {
        private readonly IConfiguration configuration;
        private readonly SqlConnection sqlConnection;

        public DistribuidoraRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            sqlConnection = new SqlConnection(configuration.GetConnectionString("dbAzure"));
        }

        public Distribuidora InserirDistribuidora(string endereco, int numEstrelas, int entregasRealizadas)
        {
            var query = $@"INSERT INTO DISTRIBUIDORA VALUES('{endereco}',{numEstrelas}, {entregasRealizadas});
                           SELECT SCOPE_IDENTITY()";

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                var id = command.ExecuteScalar();
                sqlConnection.Close();

                return new Distribuidora
                {
                    CodDistribuidora = (decimal)id,
                    Endereco = endereco,
                    NumeroEstrelas = numEstrelas,
                    EntregasRealizadas = entregasRealizadas
                };
            }
        }

        public Distribuidora GetDistribuidoraPorCodigo(decimal codigo)
        {
            var query = $"SELECT * FROM DISTRIBUIDORA WHERE COD_DIST = {codigo}";
            Distribuidora distribuidora = null;
            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    distribuidora = new Distribuidora
                    {
                        CodDistribuidora = codigo,
                        Endereco = (string)reader["end_dist"],
                        NumeroEstrelas = (int)reader["num_estrelas"],
                        EntregasRealizadas = (int)reader["entregas_realizadas"]
                    };
                }
            }
            sqlConnection.Close();
            return distribuidora;
        }
        public IEnumerable<Distribuidora> GetTodasAsDistribuidoras()
        {
            var query = $"SELECT * FROM DISTRIBUIDORA";

            var listaDistribuidoras = new List<Distribuidora>();

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    listaDistribuidoras.Add(new Distribuidora
                    {
                        CodDistribuidora = (int)reader["cod_dist"],
                        Endereco = (string)reader["end_dist"],
                        NumeroEstrelas = (int)reader["num_estrelas"],
                        EntregasRealizadas = (int)reader["entregas_realizadas"]
                    });
                }

                sqlConnection.Close();
            }
            return listaDistribuidoras;
        }

        public Distribuidora EditarDistribuidora(decimal codigo, string endereco, int numEstrelas, int entregasRealizadas)
        {
            var query = $@"UPDATE DISTRIBUIDORA 
                           SET END_DIST = '{endereco}',
                           NUM_ESTRELAS = {numEstrelas},
                           ENTREGAS_REALIZADAS = {entregasRealizadas}
                           WHERE COD_DIST = {codigo}";

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                command.ExecuteNonQuery();
                sqlConnection.Close();

                return new Distribuidora
                {
                    CodDistribuidora = codigo,
                    Endereco = endereco,
                    NumeroEstrelas = numEstrelas,
                    EntregasRealizadas = entregasRealizadas
                };
            }
        }

        public Distribuidora AdicionarUmaEntrega(decimal codigo, int qntdEntregasAnterior)
        {
            var query = $@"UPDATE DISTRIBUIDORA 
                           SET ENTREGAS_REALIZADAS = {qntdEntregasAnterior+1}
                           WHERE COD_DIST = {codigo}";

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                command.ExecuteNonQuery();
                sqlConnection.Close();
            }

            return GetDistribuidoraPorCodigo(codigo);
        }

        public void ApagarDistribuidora(decimal codigo)
        {
            var query = $"DELETE FROM DISTRIBUIDORA WHERE COD_DIST = {codigo}";

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                command.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}
