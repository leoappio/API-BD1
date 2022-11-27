using API_TRABALHO_BD1.Entity;
using API_TRABALHO_BD1.Repository;
using API_TRABALHO_BD1.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace API_TRABALHO_BD1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoRepository pedidoRepository;
        private readonly IProdutoRepository produtoRepository;
        private readonly IClienteRepository clienteRepository;
        private readonly IEnderecoRepository enderecoRepository;
        private readonly IDistribuidoraRepository distribuidoraRepository;
        private readonly IPagamentoRepository pagamentoRepository;
        private readonly IProdutoPedidoRepository produtoPedidoRepository;

        public PedidoController(IPedidoRepository pedidoRepository,
            IProdutoRepository produtoRepository,
            IClienteRepository clienteRepository,
            IEnderecoRepository enderecoRepository,
            IDistribuidoraRepository distribuidoraRepository,
            IPagamentoRepository pagamentoRepository,
            IProdutoPedidoRepository produtoPedidoRepository)
        {
            this.pedidoRepository = pedidoRepository;
            this.clienteRepository = clienteRepository;
            this.enderecoRepository = enderecoRepository;
            this.pagamentoRepository = pagamentoRepository;
            this.distribuidoraRepository = distribuidoraRepository;
            this.produtoPedidoRepository = produtoPedidoRepository;
            this.produtoRepository = produtoRepository;
        }

        [HttpGet]
        [Route("GetRelatorioTipoDePagamento")]
        public ActionResult<List<RelatorioTipoDePagamento>> GetRelatorioTipoDePagamento()
        {
            return Ok(pedidoRepository.GetRelatorioTipoDePagamento());
        }

        [HttpGet]
        [Route("GetRelatorioValorMedioPorCategoria")]
        public ActionResult<List<RelatorioValorMedioPorCategoria>> GetRelatorioValorMedioPorCategoria()
        {
            return Ok(pedidoRepository.GetRelatorioValorMedioPorCategoria());
        }

        [HttpGet]
        [Route("GetRelatorioVendasMensal")]
        public ActionResult<List<RelatorioVendasMensal>> GetRelatorioVendasMensal()
        {
            return Ok(pedidoRepository.GetRelatorioVendasMensal());
        }

        [HttpGet]
        [Route("GetPedidoPorCodigo")]
        public ActionResult<Pedido> GetPedidoPorCodigo(decimal codigo)
        {
            if (codigo > 0)
            {
                var produto = pedidoRepository.GetPedidoPorCodigo(codigo);
                if (produto != null)
                {
                    return Ok(produto);
                }
                return NotFound("Pedido não encontrado");
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("GetTodosOsPedidosDeUmCliente")]
        public ActionResult<List<Pedido>> GetTodosOsPedidosDeUmCliente(decimal codigo)
        {
            if (codigo > 0)
            {
                var cliente = clienteRepository.GetClientePorCodigo(codigo);
                if (cliente != null)
                {
                    return Ok(pedidoRepository.GetTodosOsPedidosDeUmCliente(codigo));
                }
                return NotFound("Cliente não encontrado");
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("CadastrarPedido")]
        public ActionResult<Pedido> CadastrarPedido(string status,
            int codEndereco, int codCliente, int codDistribuidora, int codTipoPagamento, DateTime dataPedido)
        {

            var endereco = enderecoRepository.GetEnderecoPorCodigo(codEndereco);
            if (endereco == null) return NotFound("Endereço não encontrado");
            if(endereco.Cliente.CodCliente != codCliente) return BadRequest("O endereço de entrega não pertence a este cliente");

            var cliente = clienteRepository.GetClientePorCodigo(codCliente);
            if (cliente == null) return NotFound("Cliente não encontrado");

            var distribuidora = distribuidoraRepository.GetDistribuidoraPorCodigo(codDistribuidora);
            if (distribuidora == null) return NotFound("Distribuidora não encontrada");

            var tipoPagamento = pagamentoRepository.GetPagamentoPorCodigo(codTipoPagamento);
            if (tipoPagamento == null) return NotFound("Tipo de pagamento não encontrado");

            return pedidoRepository.InserirPedido((float)0.0, status, codEndereco, codCliente,
                codDistribuidora, codTipoPagamento, dataPedido);
        }

        [HttpPost]
        [Route("AdicionarProdutoNoPedido")]
        public ActionResult<Pedido> AdicionarProdutoNoPedido(decimal codPedido, decimal codProduto, int quantidade)
        {

            var pedido = pedidoRepository.GetPedidoPorCodigo(codPedido);
            if (pedido == null) return NotFound("Pedido não encontrado");

            var produto = produtoRepository.GetProdutoPorCodigo(codProduto);
            if (produto == null) return NotFound("Produto não encontrado");

            var valorTotal = produto.Preco * quantidade;

            produtoPedidoRepository.InserirProdutoEmUmPedido((int)codProduto, (int)codPedido, quantidade, valorTotal);

            return pedidoRepository.GetPedidoPorCodigo(codPedido);
        }

        [HttpPost]
        [Route("RemoverProdutoDeUmPedido")]
        public ActionResult<Pedido> RemoverProdutoDeUmPedido(decimal codPedido, decimal codProduto)
        {

            var pedido = pedidoRepository.GetPedidoPorCodigo(codPedido);
            if (pedido == null) return NotFound("Pedido não encontrado");

            var produto = produtoRepository.GetProdutoPorCodigo(codProduto);
            if (produto == null) return NotFound("Produto não encontrado");

            produtoPedidoRepository.RemoverProdutoDeUmPedido((int)codProduto, (int)codPedido);

            return pedidoRepository.GetPedidoPorCodigo(codPedido);
        }

        [HttpPost]
        [Route("EditarPedido")]
        public ActionResult<Pedido> EditarPedido(decimal codigo, string status,
            int codEndereco, int codCliente, int codDistribuidora, int codTipoPagamento, DateTime dataPedido)
        {

            var pedido = pedidoRepository.GetPedidoPorCodigo(codigo);
            if (pedido == null) return NotFound("Pedido não encontrado");
            var endereco = enderecoRepository.GetEnderecoPorCodigo(codEndereco);
            if (endereco == null) return NotFound("Endereço não encontrado");
            if (endereco.Cliente.CodCliente != codCliente) return BadRequest("O endereço de entrega não pertence a este cliente");

            var cliente = clienteRepository.GetClientePorCodigo(codCliente);
            if (cliente == null) return NotFound("Cliente não encontrado");

            var distribuidora = distribuidoraRepository.GetDistribuidoraPorCodigo(codDistribuidora);
            if (distribuidora == null) return NotFound("Distribuidora não encontrada");

            var tipoPagamento = pagamentoRepository.GetPagamentoPorCodigo(codTipoPagamento);
            if (tipoPagamento == null) return NotFound("Tipo de pagamento não encontrado");

            return pedidoRepository.EditarPedido(codigo, status, codEndereco, codCliente,
                codDistribuidora, codTipoPagamento, dataPedido);
        }

        [HttpPost]
        [Route("EditarStatus")]
        public ActionResult<Pedido> EditarStatus(decimal codigo, string status)
        {
            if (codigo > 0 && !status.IsNullOrEmpty())
            {
                return Ok(pedidoRepository.EditarStatus(codigo, status));
            }

            return BadRequest();
        }

        [HttpDelete]
        [Route("ApagarPedido")]
        public ActionResult ApagarPedido(decimal codigo)
        {
            if (codigo > 0)
            {
                var pedido = pedidoRepository.GetPedidoPorCodigo(codigo);

                if (pedido != null)
                {
                    pedidoRepository.ApagarPedido(codigo);

                    return Ok("Pedido apagado com sucesso!");
                }
                else
                {
                    return NotFound("Não foi encontrado pedido com esse codigo");
                }
            }
            else
            {
                return BadRequest("Código do pedido inválido");
            }
        }
    }
}
