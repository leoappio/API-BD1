using API_TRABALHO_BD1.Entity;
using API_TRABALHO_BD1.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API_TRABALHO_BD1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnderecoController : ControllerBase
    {
        private readonly IEnderecoRepository enderecoRepository;
        private readonly IClienteRepository clienteRepository;

        public EnderecoController(IEnderecoRepository enderecoRepository, IClienteRepository clienteRepository)
        {
            this.enderecoRepository = enderecoRepository;
            this.clienteRepository = clienteRepository;
        }


        [HttpGet]
        [Route("GetEnderecoPorCodigo")]
        public ActionResult<Endereco> GetEnderecoPorCodigo(decimal codigo)
        {
            if (codigo > 0)
            {
                var endereco = enderecoRepository.GetEnderecoPorCodigo(codigo);
                if (endereco != null)
                {
                    return Ok(endereco);
                }
                return NotFound("Endereco não encontrado");
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("GetTodosOsEnderecoDeUmCliente")]
        public ActionResult<IEnumerable<Endereco>> GetTodosOsEnderecoDeUmCliente(decimal codigo)
        {
            var cliente = clienteRepository.GetClientePorCodigo(codigo);
            if (cliente != null)
            {
                return Ok(enderecoRepository.GetTodosOsEnderecoDeUmCliente(codigo));
            }
            else
            {
                return NotFound("Cliente não encontrado");
            }

        }

        [HttpPost]
        [Route("CadastrarEndereco")]
        public ActionResult<Endereco> CadastrarEndereco(decimal codCliente, string logradouro, int numero, string bairro, string cidade, string CEP)
        {
            var cliente = clienteRepository.GetClientePorCodigo(codCliente);

            if (cliente != null)
            {
                if (!logradouro.IsNullOrEmpty() && !bairro.IsNullOrEmpty() && !cidade.IsNullOrEmpty() && !CEP.IsNullOrEmpty())
                {
                    return Ok(enderecoRepository.InserirEndereco(codCliente, logradouro, numero, bairro, cidade, CEP));
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
        [Route("EditarEndereco")]
        public ActionResult<Endereco> EditarEndereco(decimal codigo, decimal codCliente, string logradouro, int numero, string bairro, string cidade, string CEP)
        {
            if (codigo > 0 && codCliente > 0 && !logradouro.IsNullOrEmpty() && !bairro.IsNullOrEmpty() && !cidade.IsNullOrEmpty() && !CEP.IsNullOrEmpty())
            {
                var cliente = clienteRepository.GetClientePorCodigo(codigo);

                if (cliente != null)
                {
                    var endereco = enderecoRepository.GetEnderecoPorCodigo(codigo);

                    if (endereco != null)
                    {
                        return Ok(enderecoRepository.EditarEndereco(codigo, codCliente,logradouro, numero, bairro, cidade, CEP));
                    }
                    else
                    {
                        return NotFound("Não foi encontrado endereco com esse codigo");
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
        [Route("ApagarEndereco")]
        public ActionResult ApagarEndereco(decimal codigo)
        {
            if (codigo > 0)
            {
                var endereco = enderecoRepository.GetEnderecoPorCodigo(codigo);

                if (endereco != null)
                {
                    enderecoRepository.ApagarEndereco(codigo);

                    return Ok("Endereco apagado com sucesso!");
                }
                else
                {
                    return NotFound("Não foi encontrado endereco com esse codigo");
                }
            }
            else
            {
                return BadRequest("Código do endereco inválido");
            }
        }
    }
}
