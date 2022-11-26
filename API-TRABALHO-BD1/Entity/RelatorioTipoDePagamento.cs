namespace API_TRABALHO_BD1.Entity
{
    public class RelatorioTipoDePagamento
    {
        public Pagamento TipoPagamento { get; set; }
        public int TotalDePedidos { get; set; }
        public float ValorTotal { get; set; }
    }
}
