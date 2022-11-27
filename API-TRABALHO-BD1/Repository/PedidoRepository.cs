using API_TRABALHO_BD1.Entity;
using API_TRABALHO_BD1.Repository.Interfaces;
using Microsoft.Data.SqlClient;

namespace API_TRABALHO_BD1.Repository
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly IConfiguration configuration;
        private readonly IClienteRepository clienteRepository;
        private readonly IEnderecoRepository enderecoRepository;
        private readonly IDistribuidoraRepository distribuidoraRepository;
        private readonly IPagamentoRepository pagamentoRepository;
        private readonly IProdutoPedidoRepository produtoPedidoRepository;
        private readonly SqlConnection sqlConnection;

        public PedidoRepository(IConfiguration configuration,
            IClienteRepository clienteRepository,
            IEnderecoRepository enderecoRepository,
            IDistribuidoraRepository distribuidoraRepository,
            IPagamentoRepository pagamentoRepository,
            IProdutoPedidoRepository produtoPedidoRepository)
        {
            this.configuration = configuration;
            sqlConnection = new SqlConnection(configuration.GetConnectionString("dbAzure"));
            this.clienteRepository = clienteRepository;
            this.enderecoRepository = enderecoRepository;
            this.pagamentoRepository = pagamentoRepository;
            this.distribuidoraRepository = distribuidoraRepository;
            this.produtoPedidoRepository = produtoPedidoRepository;
        }

        public Pedido InserirPedido(float valorTotal, string status,
            int codEndereco, int codCliente, int codDistribuidora, int codTipoPagamento, DateTime dataPedido)
        {
            var query = $@"INSERT INTO PEDIDO VALUES({valorTotal},'{status}', {codEndereco}, {codCliente},
                           {codDistribuidora}, {codTipoPagamento}, '{dataPedido.ToString("MM-dd-yyyy")}');
                           SELECT SCOPE_IDENTITY()";

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                var id = command.ExecuteScalar();
                sqlConnection.Close();

                return new Pedido
                {
                    CodPedido = (decimal)id,
                    ValorTotal = valorTotal,
                    Status = status,
                    EnderecoEntrega = enderecoRepository.GetEnderecoPorCodigo(codEndereco),
                    Cliente = clienteRepository.GetClientePorCodigo(codCliente),
                    Distribuidora = distribuidoraRepository.GetDistribuidoraPorCodigo(codDistribuidora),
                    TipoPagamento = pagamentoRepository.GetPagamentoPorCodigo(codTipoPagamento),
                    DataPedido = dataPedido
                };
            }
        }

        public Pedido GetPedidoPorCodigo(decimal codigo)
        {
            var query = $"SELECT * FROM PEDIDO WHERE COD_PEDIDO = {codigo}";
            Pedido pedido = null;
            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    pedido = new Pedido
                    {
                        CodPedido = codigo,
                        ValorTotal = Convert.ToSingle(reader["valor_total"]),
                        Status = (string)reader["status"],
                        EnderecoEntrega = enderecoRepository.GetEnderecoPorCodigo((int)reader["cod_endereco_entrega"]),
                        Cliente = clienteRepository.GetClientePorCodigo((int)reader["cod_cliente"]),
                        Distribuidora = distribuidoraRepository.GetDistribuidoraPorCodigo((int)reader["cod_distribuidora"]),
                        TipoPagamento = pagamentoRepository.GetPagamentoPorCodigo((int)reader["cod_tipo_pagamento"]),
                        DataPedido = Convert.ToDateTime(reader["data_pedido"]),
                        Produtos = produtoPedidoRepository.GetProdutosDeUmPedido((int)codigo)
                    };
                }
            }
            sqlConnection.Close();
            return pedido;
        }
        public List<Pedido> GetTodosOsPedidosDeUmCliente(decimal codigoCliente)
        {
            var query = $"SELECT * FROM PEDIDO WHERE COD_CLIENTE = {codigoCliente}";

            var listaPedidos = new List<Pedido>();

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    listaPedidos.Add(new Pedido
                    {
                        CodPedido = (int)reader["cod_pedido"],
                        ValorTotal = Convert.ToSingle(reader["valor_total"]),
                        Status = (string)reader["status"],
                        EnderecoEntrega = enderecoRepository.GetEnderecoPorCodigo((int)reader["cod_endereco_entrega"]),
                        Cliente = clienteRepository.GetClientePorCodigo((int)reader["cod_cliente"]),
                        Distribuidora = distribuidoraRepository.GetDistribuidoraPorCodigo((int)reader["cod_distribuidora"]),
                        TipoPagamento = pagamentoRepository.GetPagamentoPorCodigo((int)reader["cod_tipo_pagamento"]),
                        DataPedido = Convert.ToDateTime(reader["data_pedido"]),
                        Produtos = produtoPedidoRepository.GetProdutosDeUmPedido((int)reader["cod_pedido"])
                    });
                }

                sqlConnection.Close();
            }
            return listaPedidos;
        }

        public Pedido EditarPedido(decimal codigo, string status,
            int codEndereco, int codCliente, int codDistribuidora, int codTipoPagamento, DateTime dataPedido)
        {
            var query = $@"UPDATE PEDIDO 
                           SET STATUS = {status},
                           COD_ENDERECO_ENTREGA= {codEndereco},
                           COD_CLIENTE = {codCliente},
                           COD_DISTRIBUIDORA = {codDistribuidora},
                           COD_TIPO_PAGAMENTO = {codTipoPagamento}
                           WHERE COD_PEDIDO = {codigo}";

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                command.ExecuteNonQuery();
                sqlConnection.Close();

                return GetPedidoPorCodigo(codigo);
            }
        }

        public Pedido EditarStatus(decimal codigo, string status)
        {
            var query = $@"UPDATE PEDIDO 
                           SET STATUS = '{status}'
                           WHERE COD_PEDIDO = {codigo}";

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                command.ExecuteNonQuery();
                sqlConnection.Close();

                return GetPedidoPorCodigo(codigo);
            }
        }

        public void ApagarPedido(decimal codigo)
        {
            var query = $"DELETE FROM PEDIDO WHERE COD_PEDIDO = {codigo}";

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                command.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        public List<RelatorioTipoDePagamento> GetRelatorioTipoDePagamento()
        {
            var query = $@"SELECT TP.NOME, SUM(P.VALOR_TOTAL) AS TOTAL, COUNT(P.cod_pedido) AS TOTAL_PEDIDOS
                           FROM PEDIDO P
                           INNER JOIN TIPO_PAGAMENTO TP ON (P.COD_TIPO_PAGAMENTO = TP.COD_TIPO_PAGAMENTO)
                           GROUP BY TP.NOME 
                           ORDER BY TOTAL DESC";

            var listaRelatorio = new List<RelatorioTipoDePagamento>();

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    listaRelatorio.Add(new RelatorioTipoDePagamento
                    {
                        TotalDePedidos = (int)reader["total_pedidos"],
                        ValorTotal = Convert.ToSingle(reader["total"]),
                        TipoPagamento = (string)reader["nome"]
                    });
                }

                sqlConnection.Close();
            }
            return listaRelatorio;
        }

        public List<RelatorioValorMedioPorCategoria> GetRelatorioValorMedioPorCategoria()
        {
            var query = $@"SELECT CAT.DESCRICAO, AVG(P.VALOR_TOTAL) AS TOTAL
                           FROM PEDIDO P
                           INNER JOIN PRODUTOS_PEDIDO PP ON (P.COD_PEDIDO = PP.COD_PEDIDO)
                           INNER JOIN  PRODUTO PROD ON (PP.COD_PRODUTO  = PROD.COD_PRODUTO)
                           INNER JOIN CATEGORIA CAT ON (PROD.COD_CATEGORIA  = CAT.COD_CATEGORIA)
                           GROUP BY CAT.DESCRICAO 
                           ORDER BY TOTAL DESC";

            var listaRelatorio = new List<RelatorioValorMedioPorCategoria>();

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    listaRelatorio.Add(new RelatorioValorMedioPorCategoria
                    {
                        ValorMédio = Convert.ToSingle(reader["total"]),
                        Categoria = (string)reader["descricao"]
                    });
                }

                sqlConnection.Close();
            }
            return listaRelatorio;
        }

        public List<RelatorioVendasMensal> GetRelatorioVendasMensal()
        {
            var query = $@"SELECT CONCAT(DATEPART(mm, P.DATA_PEDIDO),'/', DATEPART(yyyy, P.DATA_PEDIDO)) AS DATA, SUM(P.VALOR_TOTAL) AS TOTAL, SUM(PP.quantidade) AS PRODUTOS_VENDIDOS
                        FROM PEDIDO P
                        INNER JOIN PRODUTOS_PEDIDO PP ON (PP.cod_pedido = P.cod_pedido)
                        GROUP BY CONCAT(DATEPART(mm, P.DATA_PEDIDO),'/', DATEPART(yyyy, P.DATA_PEDIDO))
                        ORDER BY TOTAL DESC";

            var listaRelatorio = new List<RelatorioVendasMensal>();

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    listaRelatorio.Add(new RelatorioVendasMensal
                    {
                        ValorTotal = Convert.ToSingle(reader["total"]),
                        Data = (string)reader["data"],
                        ProdutosVendidos = int.Parse(reader["produtos_vendidos"].ToString())
                    });
                }

                sqlConnection.Close();
            }
            return listaRelatorio;
        }
    }
}
