using API_TRABALHO_BD1.Entity;
using API_TRABALHO_BD1.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API_TRABALHO_BD1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteRepository clienteRepository;

        public ClienteController(IClienteRepository clienteRepository)
        {
            this.clienteRepository = clienteRepository;
        }


        [HttpGet]
        [Route("GetClientePorCodigo")]
        public ActionResult<Cliente> GetClientePorCodigo(decimal codigo)
        {
            if (codigo > 0)
            {
                var cliente = clienteRepository.GetClientePorCodigo(codigo);
                if (cliente != null)
                {
                    return Ok(cliente);
                }
                return NotFound("Cliente não encontrado");
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("GetTodosClientes")]
        public ActionResult<IEnumerable<Cliente>> GetTodosClientes()
        {
            return Ok(clienteRepository.GetTodosOsClientes());
        }

        [HttpPost]
        [Route("CadastrarCliente")]
        public ActionResult<Cliente> CadastrarCliente(decimal idade, string cpf, string nome)
        {
            if (idade > 0 && !cpf.IsNullOrEmpty() && !nome.IsNullOrEmpty())
            {
                return Ok(clienteRepository.InserirCliente(idade, cpf, nome));
            }

            return BadRequest("Valores inválidos para cadastro");
        }

        [HttpPost]
        [Route("EditarCliente")]
        public ActionResult<Cliente> EditarCliente(decimal codigo, decimal idade, string cpf, string nome)
        {
            if (codigo > 0 && idade > 0 && !cpf.IsNullOrEmpty() && !nome.IsNullOrEmpty())
            {
                var cliente = clienteRepository.GetClientePorCodigo(codigo);

                if (cliente != null)
                {
                    return Ok(clienteRepository.EditarCliente(codigo, idade, cpf, nome));
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
        [Route("ApagarCliente")]
        public ActionResult ApagarCliente(decimal codigo)
        {
            if (codigo > 0)
            {
                var cliente = clienteRepository.GetClientePorCodigo(codigo);

                if (cliente != null)
                {
                    clienteRepository.ApagarCliente(codigo);
                    return Ok("Cliente apagado com sucesso!");
                }
                else
                {
                    return NotFound("Não foi encontrado cliente com esse codigo");
                }
            }
            else
            {
                return BadRequest("Código do cliente inválido");
            }
        }
    }
}
