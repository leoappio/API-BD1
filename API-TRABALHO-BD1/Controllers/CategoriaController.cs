using API_TRABALHO_BD1.Entity;
using API_TRABALHO_BD1.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API_TRABALHO_BD1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaRepository categoriaRepository;

        public CategoriaController(ICategoriaRepository categoriaRepository)
        {
            this.categoriaRepository = categoriaRepository;
        }

        
        [HttpGet]
        [Route("GetCategoriaPorCodigo")]
        public ActionResult<Categoria> GetCategoriaPorCodigo(decimal codigo)
        {
            if(codigo > 0)
            {
                var categoria = categoriaRepository.GetCategoriaPorCodigo(codigo);
                if (categoria != null)
                {
                    return Ok(categoria);
                }
                return NotFound("Categoria n�o encontrada");
            }

            return BadRequest();
        }
        
        [HttpGet]
        [Route("GetTodasCategorias")]
        public ActionResult<IEnumerable<Categoria>> GetTodasCategorias()
        {
            return Ok(categoriaRepository.GetTodasAsCategorias());
        }

        [HttpPost]
        [Route("CadastrarCategoria")]
        public ActionResult<Categoria> CadastrarCategoria(string descricao)
        {
            if (!descricao.IsNullOrEmpty())
            {
                return Ok(categoriaRepository.InserirCategoria(descricao));
            }

            return BadRequest("O campo descri��o deve conter algum valor");
        }

        [HttpPost]
        [Route("EditarCategoria")]
        public ActionResult<Categoria> EditarCategoria(decimal codigo, string descricao)
        {
            if(codigo > 0 && !descricao.IsNullOrEmpty())
            {
                var categoria = categoriaRepository.GetCategoriaPorCodigo(codigo);

                if(categoria != null)
                {
                    return Ok(categoriaRepository.EditarCategoria(codigo, descricao));
                }
                else
                {
                    return NotFound("N�o foi encontrada categoria com esse codigo");
                }
            }
            else
            {
                return BadRequest("Dados Inv�lidos para edi��o");
            }
        }

        [HttpDelete]
        [Route("ApagarCategoria")]
        public ActionResult ApagarCategoria(decimal codigo)
        {
            if (codigo > 0)
            {
                var categoria = categoriaRepository.GetCategoriaPorCodigo(codigo);

                if (categoria != null)
                {
                    categoriaRepository.ApagarCategoria(codigo);
                    return Ok("Categoria apagada com sucesso!");
                }
                else
                {
                    return NotFound("N�o foi encontrada categoria com esse codigo");
                }
            }
            else
            {
                return BadRequest("C�digo da categoria inv�lido");
            }
        }
    }
}