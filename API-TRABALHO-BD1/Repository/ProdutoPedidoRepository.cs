using API_TRABALHO_BD1.Entity;
using API_TRABALHO_BD1.Repository.Interfaces;
using Microsoft.Data.SqlClient;

namespace API_TRABALHO_BD1.Repository
{
    public class ProdutoPedidoRepository : IProdutoPedidoRepository
    {
        private readonly IConfiguration configuration;
        private readonly IProdutoRepository produtoRepository;
        private readonly SqlConnection sqlConnection;

        public ProdutoPedidoRepository(IConfiguration configuration,
            IProdutoRepository produtoRepository)
        {
            this.configuration = configuration;
            sqlConnection = new SqlConnection(configuration.GetConnectionString("dbAzure"));
            this.produtoRepository = produtoRepository;
        }

        public void InserirProdutoEmUmPedido(int codProduto, int codPedido, int quantidade, float valorTotal)
        {
            var query = $@"INSERT INTO PRODUTO_PEDIDO VALUES({codProduto}, {codPedido}, {quantidade}, {valorTotal})";

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                command.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        public List<ProdutoPedido> GetProdutosDeUmPedido(int cod_pedido)
        {
            var query = $"SELECT * FROM PRODUTOS_PEDIDO WHERE COD_PEDIDO = {cod_pedido}";

            var listaProdutos = new List<ProdutoPedido>();

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    listaProdutos.Add(new ProdutoPedido
                    {
                        Produto = produtoRepository.GetProdutoPorCodigo((int)reader["cod_produto"]),
                        CodPedido = cod_pedido,
                        Quantidade = (int)reader["quantidade"],
                        ValorTotal = (float)reader["valor_total"]
                    });
                }

                sqlConnection.Close();
            }
            return listaProdutos;
        }

        public void RemoverProdutoDeUmPedido(int codProduto, int codPedido)
        {
            var query = $"DELETE FROM PRODUTOS_PEDIDO WHERE COD_PRODUTO = {codProduto} AND COD_PEDIDO = {codPedido}";

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                command.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        public bool ProdutoEstaEmUmPedido(int codProduto)
        {
            var query = $"SELECT * FROM PRODUTOS_PEDIDO WHERE COD_PRODUTO = {codProduto}";

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                var reader = command.ExecuteReader();

                return reader.Read();
            }
        }
    }
}
