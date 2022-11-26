using API_TRABALHO_BD1.Entity;
using API_TRABALHO_BD1.Repository.Interfaces;
using Microsoft.Data.SqlClient;

namespace API_TRABALHO_BD1.Repository
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly IConfiguration configuration;
        private readonly SqlConnection sqlConnection;

        public CategoriaRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            sqlConnection = new SqlConnection(configuration.GetConnectionString("dbAzure"));
        }

        public Categoria InserirCategoria(string descricao)
        {
            var query = $"INSERT INTO CATEGORIA VALUES('{descricao}'); SELECT SCOPE_IDENTITY()";

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                var id = command.ExecuteScalar();
                sqlConnection.Close();

                return new Categoria
                {
                    CodCategoria = (decimal)id,
                    Descricao = descricao
                };
            }
        }

        public Categoria GetCategoriaPorCodigo(decimal codigo)
        {
            var query = $"SELECT * FROM CATEGORIA WHERE COD_CATEGORIA = {codigo}";
            Categoria categoria = null;
            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    categoria = new Categoria
                    {
                        CodCategoria = (int)reader["cod_categoria"],
                        Descricao = (string)reader["descricao"]
                    };
                }
            }
            sqlConnection.Close();
            return categoria;
        }
        public IEnumerable<Categoria> GetTodasAsCategorias()
        {
            var query = $"SELECT * FROM CATEGORIA";

            var listaCategorias = new List<Categoria>();

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    listaCategorias.Add(new Categoria
                    {
                        CodCategoria = (int)reader["cod_categoria"],
                        Descricao = (string)reader["descricao"]
                    });
                }

                sqlConnection.Close();
            }
            return listaCategorias;
        }

        public Categoria EditarCategoria(decimal codigo, string descricao)
        {
            var query = $@"UPDATE CATEGORIA SET DESCRICAO = '{descricao}'
                           WHERE COD_CATEGORIA = {codigo}";

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                command.ExecuteNonQuery();
                sqlConnection.Close();

                return new Categoria
                {
                    CodCategoria = codigo,
                    Descricao = descricao
                };
            }
        }

        public void ApagarCategoria(decimal codigo)
        {
            var query = $"DELETE FROM CATEGORIA WHERE COD_CATEGORIA = {codigo}";

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                command.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}
