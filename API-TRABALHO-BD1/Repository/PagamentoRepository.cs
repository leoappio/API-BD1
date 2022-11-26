using API_TRABALHO_BD1.Entity;
using API_TRABALHO_BD1.Repository.Interfaces;
using Microsoft.Data.SqlClient;

namespace API_TRABALHO_BD1.Repository
{
    public class PagamentoRepository : IPagamentoRepository
    {
        private readonly IConfiguration configuration;
        private readonly SqlConnection sqlConnection;

        public PagamentoRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            sqlConnection = new SqlConnection(configuration.GetConnectionString("dbAzure"));
        }

        public Pagamento InserirTipoPagamento(string nome)
        {
            var query = $@"INSERT INTO TIPO_PAGAMENTO VALUES('{nome}');
                           SELECT SCOPE_IDENTITY()";

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                var id = command.ExecuteScalar();
                sqlConnection.Close();

                return new Pagamento
                {
                    CodTipoPagamento = (decimal)id,
                    Nome = nome
                };
            }
        }

        public Pagamento GetPagamentoPorCodigo(decimal codigo)
        {
            var query = $"SELECT * FROM TIPO_PAGAMENTO WHERE COD_TIPO_PAGAMENTO = {codigo}";
            Pagamento pagamento = null;
            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    pagamento = new Pagamento
                    {
                        CodTipoPagamento = codigo,
                        Nome = (string)reader["nome"]
                    };
                }
            }
            sqlConnection.Close();
            return pagamento;
        }
        public IEnumerable<Pagamento> GetTodosOsTiposPagamento()
        {
            var query = $"SELECT * FROM TIPO_PAGAMENTO";

            var listaPagamento = new List<Pagamento>();

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    listaPagamento.Add(new Pagamento
                    {
                        CodTipoPagamento = (int)reader["cod_tipo_pagamento"],
                        Nome = (string)reader["nome"]
                    });
                }

                sqlConnection.Close();
            }
            return listaPagamento;
        }

        public Pagamento EditarPagamento(decimal codigo, string nome)
        {
            var query = $@"UPDATE TIPO_PAGAMENTO 
                           SET NOME = '{nome}'
                           WHERE COD_TIPO_PAGAMENTO = {codigo}";

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                command.ExecuteNonQuery();
                sqlConnection.Close();

                return new Pagamento
                {
                    CodTipoPagamento = codigo,
                    Nome = nome
                };
            }
        }

        public void ApagarTipoPagamento(decimal codigo)
        {
            var query = $"DELETE FROM TIPO_PAGAMENTO WHERE COD_TIPO_PAGAMENTO = {codigo}";

            using (var command = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                command.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}
