using API_TRABALHO_BD1.Entity;
using API_TRABALHO_BD1.Repository.Interfaces;
using Microsoft.Data.SqlClient;

namespace API_TRABALHO_BD1.Repository
{
    public class LojaRepository : ILojaRepository
    {
        private readonly IConfiguration configuration;
        private readonly SqlConnection sqlConnection;

        public LojaRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            sqlConnection = new SqlConnection(configuration.GetConnectionString("dbAzure"));
        }

        public Loja InserirLoja(string endereco, string CNPJ, int numEstrelas)
        {
            var query = $@"INSERT INTO LOJA VALUES('{endereco}','{CNPJ}', {numEstrelas});
                           SELECT SCOPE_IDENTITY()";

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                var id = command.ExecuteScalar();
                sqlConnection.Close();

                return new Loja
                {
                    CodLoja = (decimal)id,
                    Endereco = endereco,
                    CNPJ = CNPJ,
                    NumEstrelas = numEstrelas
                };
            }
        }

        public Loja GetLojaPorCodigo(decimal codigo)
        {
            var query = $"SELECT * FROM LOJA WHERE COD_LOJA = {codigo}";
            Loja loja = null;
            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    loja = new Loja
                    {
                        CodLoja = codigo,
                        Endereco = (string)reader["endereco"],
                        NumEstrelas = (int)reader["num_estrelas"],
                        CNPJ = (string)reader["cnpj"]
                    };
                }
            }
            sqlConnection.Close();
            return loja;
        }
        public IEnumerable<Loja> GetTodasAsLojas()
        {
            var query = $"SELECT * FROM LOJAS";

            var listaLojas = new List<Loja>();

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    listaLojas.Add(new Loja
                    {
                        CodLoja = (int)reader["cod_loja"],
                        Endereco = (string)reader["endereco"],
                        NumEstrelas = (int)reader["num_estrelas"],
                        CNPJ = (string)reader["cnpj"]
                    });
                }

                sqlConnection.Close();
            }
            return listaLojas;
        }

        public Loja EditarLoja(decimal codigo, string endereco, string CNPJ, int numEstrelas)
        {
            var query = $@"UPDATE LOJA 
                           SET ENDERECO = '{endereco}',
                           CNPJ = '{CNPJ}',
                           NUM_ESTRELAS = {numEstrelas},
                           WHERE COD_LOJA = {codigo}";

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                command.ExecuteNonQuery();
                sqlConnection.Close();

                return new Loja
                {
                    CodLoja = codigo,
                    Endereco = endereco,
                    CNPJ = CNPJ,
                    NumEstrelas = numEstrelas
                };
            }
        }

        public void ApagarLoja(decimal codigo)
        {
            var query = $"DELETE FROM LOJA WHERE COD_LOJA = {codigo}";

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                command.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}
