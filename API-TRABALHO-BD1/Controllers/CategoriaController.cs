using API_TRABALHO_BD1.Entity;
using API_TRABALHO_BD1.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API_TRABALHO_BD1.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
                return NotFound("Categoria não encontrada");
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

            return BadRequest("O campo descrição deve conter algum valor");
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
                    return NotFound("Não foi encontrada categoria com esse codigo");
                }
            }
            else
            {
                return BadRequest("Dados Inválidos para edição");
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
                    return NotFound("Não foi encontrada categoria com esse codigo");
                }
            }
            else
            {
                return BadRequest("Código da categoria inválido");
            }
        }
    }
}