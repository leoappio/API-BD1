using API_TRABALHO_BD1.Entity;
using API_TRABALHO_BD1.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API_TRABALHO_BD1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistribuidoraController : ControllerBase
    {
        private readonly IDistribuidoraRepository distribuidoraRepository;

        public DistribuidoraController(IDistribuidoraRepository distribuidoraRepository)
        {
            this.distribuidoraRepository = distribuidoraRepository;
        }


        [HttpGet]
        [Route("GetDistribuidoraPorCodigo")]
        public ActionResult<Distribuidora> GetDistribuidoraPorCodigo(decimal codigo)
        {
            if (codigo > 0)
            {
                var distribuidora = distribuidoraRepository.GetDistribuidoraPorCodigo(codigo);
                if (distribuidora != null)
                {
                    return Ok(distribuidora);
                }
                return NotFound("Distribuidora não encontrada");
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("GetTodasAsDistribuidoras")]
        public ActionResult<IEnumerable<Distribuidora>> GetTodasAsDistribuidoras()
        {
            return Ok(distribuidoraRepository.GetTodasAsDistribuidoras());
        }

        [HttpPost]
        [Route("CadastrarDistribuidora")]
        public ActionResult<Distribuidora> CadastrarDistribuidora(string endereco, int numEstrelas, int entregasRealizadas)
        {
            if (numEstrelas > 0 && entregasRealizadas >= 0 && !endereco.IsNullOrEmpty())
            {
                return Ok(distribuidoraRepository.InserirDistribuidora(endereco, numEstrelas, entregasRealizadas));
            }

            return BadRequest("Valores inválidos para cadastro");
        }

        [HttpPost]
        [Route("EditarDistribuidora")]
        public ActionResult<Distribuidora> EditarDistribuidora(decimal codigo, string endereco, int numEstrelas, int entregasRealizadas)
        {
            if (codigo > 0 && numEstrelas > 0 && !endereco.IsNullOrEmpty())
            {
                var distribuidora = distribuidoraRepository.GetDistribuidoraPorCodigo(codigo);

                if (distribuidora != null)
                {
                    return Ok(distribuidoraRepository.EditarDistribuidora(codigo, endereco, numEstrelas, entregasRealizadas));
                }
                else
                {
                    return NotFound("Não foi encontrada distribuidora com esse codigo");
                }
            }
            else
            {
                return BadRequest("Dados Inválidos para edição");
            }
        }

        [HttpPost]
        [Route("AdicionarUmaEntrega")]
        public ActionResult<Distribuidora> AdicionarUmaEntrega(decimal codigo)
        {
            if (codigo > 0)
            {
                var distribuidora = distribuidoraRepository.GetDistribuidoraPorCodigo(codigo);

                if (distribuidora != null)
                {
                    return Ok(distribuidoraRepository.AdicionarUmaEntrega(codigo, distribuidora.EntregasRealizadas));
                }
                else
                {
                    return NotFound("Não foi encontrada distribuidora com esse codigo");
                }
            }
            else
            {
                return BadRequest("Dados Inválidos");
            }
        }

        [HttpDelete]
        [Route("ApagarDistribuidora")]
        public ActionResult ApagarDistribuidora(decimal codigo)
        {
            if (codigo > 0)
            {
                var distribuidora = distribuidoraRepository.GetDistribuidoraPorCodigo(codigo);

                if (distribuidora != null)
                {
                    distribuidoraRepository.ApagarDistribuidora(codigo);
                    return Ok("Distribuidora apagado com sucesso!");
                }
                else
                {
                    return NotFound("Não foi encontrada distribuidora com esse codigo");
                }
            }
            else
            {
                return BadRequest("Código da distribuidora inválido");
            }
        }
    }
}
