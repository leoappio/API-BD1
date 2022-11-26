using API_TRABALHO_BD1.Entity;
using API_TRABALHO_BD1.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API_TRABALHO_BD1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LojaController : ControllerBase
    {
        private readonly ILojaRepository lojaRepository;

        public LojaController(ILojaRepository lojaRepository)
        {
            this.lojaRepository = lojaRepository;
        }


        [HttpGet]
        [Route("GetLojaPorCodigo")]
        public ActionResult<Loja> GetLojaPorCodigo(decimal codigo)
        {
            if (codigo > 0)
            {
                var loja = lojaRepository.GetLojaPorCodigo(codigo);
                if (loja != null)
                {
                    return Ok(loja);
                }
                return NotFound("Loja não encontrada");
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("GetTodasAsLojas")]
        public ActionResult<IEnumerable<Loja>> GetTodasAsLojas()
        {
            return Ok(lojaRepository.GetTodasAsLojas());
        }

        [HttpPost]
        [Route("CadastrarLoja")]
        public ActionResult<Loja> CadastrarLoja(string endereco, string CNPJ, int numEstrelas)
        {
            if (numEstrelas >= 0 && !CNPJ.IsNullOrEmpty() && !endereco.IsNullOrEmpty())
            {
                return Ok(lojaRepository.InserirLoja(endereco, CNPJ, numEstrelas));
            }

            return BadRequest("Valores inválidos para cadastro");
        }

        [HttpPost]
        [Route("EditarLoja")]
        public ActionResult<Loja> EditarLoja(decimal codigo, string endereco, string CNPJ, int numEstrelas)
        {
            if (codigo > 0 && numEstrelas >= 0 && !endereco.IsNullOrEmpty())
            {
                var loja = lojaRepository.GetLojaPorCodigo(codigo);

                if (loja != null)
                {
                    return Ok(lojaRepository.EditarLoja(codigo, endereco, CNPJ, numEstrelas));
                }
                else
                {
                    return NotFound("Não foi encontrada loja com esse codigo");
                }
            }
            else
            {
                return BadRequest("Dados Inválidos para edição");
            }
        }

        [HttpDelete]
        [Route("ApagarLoja")]
        public ActionResult ApagarLoja(decimal codigo)
        {
            if (codigo > 0)
            {
                var loja = lojaRepository.GetLojaPorCodigo(codigo);

                if (loja != null)
                {
                    lojaRepository.ApagarLoja(codigo);
                    return Ok("Loja apagado com sucesso!");
                }
                else
                {
                    return NotFound("Não foi encontrada loja com esse codigo");
                }
            }
            else
            {
                return BadRequest("Código da loja inválido");
            }
        }
    }
}
