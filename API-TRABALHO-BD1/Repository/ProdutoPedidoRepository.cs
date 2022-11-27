using API_TRABALHO_BD1.Entity;
using API_TRABALHO_BD1.Repository.Interfaces;
using Microsoft.AspNetCore.Routing.Constraints;
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

        public float GetValorTotalPedido(decimal codigo)
        {
            var query = $"SELECT VALOR_TOTAL FROM PEDIDO WHERE COD_PEDIDO = {codigo}";

            var valorTotal = (float)0.0;

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    valorTotal =  Convert.ToSingle(reader["valor_total"]);
                }

                sqlConnection.Close();
            }
            return valorTotal;
        }

        public float GetValorTotalDeUmProdutoVendido(decimal codigoProduto, decimal codigoPedido)
        {
            var query = $"SELECT VALOR_TOTAL FROM PRODUTOS_PEDIDO WHERE COD_PEDIDO = {codigoPedido} AND COD_PRODUTO = {codigoProduto}";

            var valorTotal = (float)0.0;

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    valorTotal = Convert.ToSingle(reader["valor_total"]);
                }

                sqlConnection.Close();
            }
            return valorTotal;
        }

        public void InserirProdutoEmUmPedido(int codProduto, int codPedido, int quantidade, float valorTotal)
        {
            var valorTotalAntigo = GetValorTotalPedido(codPedido);
            var produto = produtoRepository.GetProdutoPorCodigo(codProduto);
            var novoTotal = valorTotalAntigo + (valorTotal);

            var query = $@"INSERT INTO PRODUTOS_PEDIDO VALUES({codProduto}, {codPedido}, {quantidade}, {valorTotal});
                           UPDATE PEDIDO SET VALOR_TOTAL = {novoTotal} WHERE COD_PEDIDO = {codPedido}";

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
                        Produto = produtoRepository.GetProdutoPorCodigo(decimal.Parse(reader["cod_produto"].ToString())),
                        CodPedido = cod_pedido,
                        Quantidade = int.Parse(reader["quantidade"].ToString()),
                        ValorTotal = Convert.ToSingle(reader["valor_total"])
                    });
                }

                sqlConnection.Close();
            }
            return listaProdutos;
        }

        public void RemoverProdutoDeUmPedido(int codProduto, int codPedido)
        {
            var valorTotalAntigo = GetValorTotalPedido(codPedido);
            var valorVendaProduto = GetValorTotalDeUmProdutoVendido(codProduto, codPedido);
            var novoTotal = valorTotalAntigo - valorVendaProduto;

            var query = $@"DELETE FROM PRODUTOS_PEDIDO WHERE COD_PRODUTO = {codProduto} AND COD_PEDIDO = {codPedido};
                           UPDATE PEDIDO SET VALOR_TOTAL = {novoTotal} WHERE COD_PEDIDO = {codPedido}";

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                command.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}
