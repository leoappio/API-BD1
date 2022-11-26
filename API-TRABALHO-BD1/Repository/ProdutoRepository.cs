using API_TRABALHO_BD1.Entity;
using API_TRABALHO_BD1.Repository.Interfaces;
using Microsoft.Data.SqlClient;

namespace API_TRABALHO_BD1.Repository
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly IConfiguration configuration;
        private readonly ICategoriaRepository categoriaRepository;
        private readonly ILojaRepository lojaRepository;
        private readonly SqlConnection sqlConnection;

        public ProdutoRepository(IConfiguration configuration,
            ICategoriaRepository categoriaRepository,
            ILojaRepository lojaRepository)
        {
            this.configuration = configuration;
            sqlConnection = new SqlConnection(configuration.GetConnectionString("dbAzure"));
            this.categoriaRepository = categoriaRepository;
            this.lojaRepository = lojaRepository;
        }

        public Produto InserirProduto(string nome, int codCategoria, int codLoja, float preco)
        {
            var query = $@"INSERT INTO PRODUTO VALUES('{nome}',{codCategoria}, {codLoja}, {preco});
                           SELECT SCOPE_IDENTITY()";

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                var id = command.ExecuteScalar();
                sqlConnection.Close();

                return new Produto
                {
                    CodProduto = (decimal)id,
                    Nome = nome,
                    Preco = preco,
                    Categoria = categoriaRepository.GetCategoriaPorCodigo(codCategoria),
                    Loja = lojaRepository.GetLojaPorCodigo(codLoja)
                };
            }
        }

        public Produto GetProdutoPorCodigo(decimal codigo)
        {
            var query = $"SELECT * FROM PRODUTO WHERE COD_PRODUTO = {codigo}";
            Produto produto = null;
            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    produto = new Produto
                    {
                        CodProduto = codigo,
                        Nome = (string)reader["nome"],
                        Preco = Convert.ToSingle(reader["preco"]),
                        Categoria = categoriaRepository.GetCategoriaPorCodigo((int)reader["cod_categoria"]),
                        Loja = lojaRepository.GetLojaPorCodigo((int)reader["cod_loja"])
                    };
                }
            }
            sqlConnection.Close();
            return produto;
        }
        public IEnumerable<Produto> GetTodosOsProdutos()
        {
            var query = $"SELECT * FROM PRODUTO";

            var listaProduto = new List<Produto>();

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    listaProduto.Add(new Produto
                    {
                        CodProduto = (int)reader["cod_produto"],
                        Nome = (string)reader["nome"],
                        Preco = Convert.ToSingle(reader["preco"]),
                        Categoria = categoriaRepository.GetCategoriaPorCodigo((int)reader["cod_categoria"]),
                        Loja = lojaRepository.GetLojaPorCodigo((int)reader["cod_loja"])
                    });
                }

                sqlConnection.Close();
            }
            return listaProduto;
        }

        public Produto EditarProduto(decimal codigo, string nome, int codCategoria, int codLoja, float preco)
        {
            var query = $@"UPDATE PRODUTO 
                           SET NOME = '{nome}',
                           COD_CATEGORIA = {codCategoria},
                           COD_LOJA = {codLoja},
                           PRECO = {preco}
                           WHERE COD_PRODUTO = {codigo}";

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                command.ExecuteNonQuery();
                sqlConnection.Close();

                return new Produto
                {
                    CodProduto = codigo,
                    Nome = nome,
                    Preco = preco,
                    Categoria = categoriaRepository.GetCategoriaPorCodigo(codCategoria),
                    Loja = lojaRepository.GetLojaPorCodigo(codLoja)
                };
            }
        }

        public void ApagarProduto(decimal codigo)
        {
            var query = $"DELETE FROM PRODUTO WHERE COD_PRODUTO = {codigo}";

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                command.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}
