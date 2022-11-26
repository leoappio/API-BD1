using API_TRABALHO_BD1.Entity;
using API_TRABALHO_BD1.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API_TRABALHO_BD1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoPagamentoController : ControllerBase
    {
        private readonly IPagamentoRepository pagamentoRepository;

        public TipoPagamentoController(IPagamentoRepository pagamentoRepository)
        {
            this.pagamentoRepository = pagamentoRepository;
        }


        [HttpGet]
        [Route("GetPagamentoPorCodigo")]
        public ActionResult<Pagamento> GetPagamentoPorCodigo(decimal codigo)
        {
            if (codigo > 0)
            {
                var loja = pagamentoRepository.GetPagamentoPorCodigo(codigo);
                if (loja != null)
                {
                    return Ok(loja);
                }
                return NotFound("Tipo Pagamento não encontrado");
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("GetTodosOsTiposPagamento")]
        public ActionResult<IEnumerable<Pagamento>> GetTodosOsTiposPagamento()
        {
            return Ok(pagamentoRepository.GetTodosOsTiposPagamento());
        }

        [HttpPost]
        [Route("CadastrarPagamento")]
        public ActionResult<Pagamento> CadastrarPagamento(string nome)
        {
            if (!nome.IsNullOrEmpty())
            {
                return Ok(pagamentoRepository.InserirTipoPagamento(nome));
            }

            return BadRequest("Valores inválidos para cadastro");
        }

        [HttpPost]
        [Route("EditarTipoPagamento")]
        public ActionResult<Pagamento> EditarTipoPagamento(decimal codigo, string nome)
        {
            if (codigo > 0 && !nome.IsNullOrEmpty())
            {
                var pagamento = pagamentoRepository.GetPagamentoPorCodigo(codigo);

                if (pagamento != null)
                {
                    return Ok(pagamentoRepository.EditarPagamento(codigo, nome));
                }
                else
                {
                    return NotFound("Não foi encontrado um tipo de pagamento com esse codigo");
                }
            }
            else
            {
                return BadRequest("Dados Inválidos para edição");
            }
        }

        [HttpDelete]
        [Route("ApagarTipoPagamento")]
        public ActionResult ApagarTipoPagamento(decimal codigo)
        {
            if (codigo > 0)
            {
                var loja = pagamentoRepository.GetPagamentoPorCodigo(codigo);

                if (loja != null)
                {
                    pagamentoRepository.ApagarTipoPagamento(codigo);
                    return Ok("Tipo de pagamento apagado com sucesso!");
                }
                else
                {
                    return NotFound("Não foi encontrado tipo de pagamento com esse codigo");
                }
            }
            else
            {
                return BadRequest("Código de tipo de pagamento inválido");
            }
        }
    }
}
