using API_TRABALHO_BD1.Entity;
using API_TRABALHO_BD1.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API_TRABALHO_BD1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContatoController : ControllerBase
    {
        private readonly IContatoRepository contatoRepository;
        private readonly IClienteRepository clienteRepository;

        public ContatoController(IContatoRepository contatoRepository, IClienteRepository clienteRepository)
        {
            this.contatoRepository = contatoRepository;
            this.clienteRepository = clienteRepository;
        }


        [HttpGet]
        [Route("GetContatoPorCodigo")]
        public ActionResult<Contato> GetContatoPorCodigo(decimal codigo)
        {
            if (codigo > 0)
            {
                var contato = contatoRepository.GetContatoPorCodigo(codigo);
                if (contato != null)
                {
                    return Ok(contato);
                }
                return NotFound("Contato não encontrado");
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("GetTodosOsContatosDeUmCliente")]
        public ActionResult<IEnumerable<Contato>> GetTodosGetTodosOsContatosDeUmClienteClientes(decimal codigo)
        {
            var cliente = clienteRepository.GetClientePorCodigo(codigo);
            if(cliente != null)
            {
                return Ok(contatoRepository.GetTodosOsContatosDeUmCliente(codigo));
            }
            else
            {
                return NotFound("Cliente não encontrado");
            }

        }

        [HttpPost]
        [Route("CadastrarContato")]
        public ActionResult<Contato> CadastrarContato(decimal cod_cliente, string numeroCelular, string email)
        {
            var cliente = clienteRepository.GetClientePorCodigo(cod_cliente);

            if (cliente != null)
            {
                if (!numeroCelular.IsNullOrEmpty() && !email.IsNullOrEmpty())
                {
                    return Ok(contatoRepository.InserirContato(cod_cliente, numeroCelular, email));
                }
                else
                {

                    return BadRequest("Valores inválidos para cadastro");
                }
            }
            else
            {
                return NotFound("Cliente não encontrado");
            }

        }

        [HttpPost]
        [Route("EditarContato")]
        public ActionResult<Contato> EditarContato(decimal codigo, decimal cod_cliente, string numeroCelular, string email)
        {
            if (codigo > 0 && cod_cliente > 0 && !numeroCelular.IsNullOrEmpty() && !email.IsNullOrEmpty())
            {
                var cliente = clienteRepository.GetClientePorCodigo(codigo);

                if (cliente != null)
                {
                    var contato = contatoRepository.GetContatoPorCodigo(codigo);

                    if (contato != null)
                    {
                        return Ok(contatoRepository.EditarContato(codigo, cod_cliente, numeroCelular, email));
                    }
                    else
                    {
                        return NotFound("Não foi encontrado contato com esse codigo");
                    }
                }
                else
                {
                    return NotFound("Não foi encontrado cliente com esse codigo");
                }
            }
            else
            {
                return BadRequest("Dados Inválidos para edição");
            }
        }

        [HttpDelete]
        [Route("ApagarContato")]
        public ActionResult ApagarContato(decimal codigo)
        {
            if (codigo > 0)
            {
                var contato = contatoRepository.GetContatoPorCodigo(codigo);

                if (contato != null)
                {
                    contatoRepository.ApagarContato(codigo);

                    return Ok("Contato apagado com sucesso!");
                }
                else
                {
                    return NotFound("Não foi encontrado contato com esse codigo");
                }
            }
            else
            {
                return BadRequest("Código do contato inválido");
            }
        }
    }
}
