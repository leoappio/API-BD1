using API_TRABALHO_BD1.Entity;
using API_TRABALHO_BD1.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API_TRABALHO_BD1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoRepository produtoRepository;
        private readonly ICategoriaRepository categoriaRepository;
        private readonly ILojaRepository lojaRepository;

        public ProdutoController(IProdutoRepository produtoRepository,
            ICategoriaRepository categoriaRepository,
            ILojaRepository lojaRepository)
        {
            this.produtoRepository = produtoRepository;
            this.categoriaRepository = categoriaRepository;
            this.lojaRepository = lojaRepository;
        }

        [HttpGet]
        [Route("GetProdutoPorCodigo")]
        public ActionResult<Produto> GetProdutoPorCodigo(decimal codigo)
        {
            if (codigo > 0)
            {
                var produto = produtoRepository.GetProdutoPorCodigo(codigo);
                if (produto != null)
                {
                    return Ok(produto);
                }
                return NotFound("Produto não encontrado");
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("GetTodosOsProdutos")]
        public ActionResult<IEnumerable<Produto>> GetTodosOsProdutos()
        {
            return Ok(produtoRepository.GetTodosOsProdutos());
        }

        [HttpPost]
        [Route("CadastrarProduto")]
        public ActionResult<Produto> CadastrarProduto(string nome, int codCategoria, int codLoja, float preco)
        {
            var categoria = categoriaRepository.GetCategoriaPorCodigo(codCategoria);
            if(categoria == null) return NotFound("Categoria não encontrada");

            var loja = lojaRepository.GetLojaPorCodigo(codLoja);
            if (loja == null) return NotFound("Loja não encontrada");

            return produtoRepository.InserirProduto(nome, codCategoria, codLoja, preco);
        }

        [HttpPost]
        [Route("EditarProduto")]
        public ActionResult<Produto> EditarProduto(decimal codigo, string nome, int codCategoria, int codLoja, float preco)
        {
            var categoria = categoriaRepository.GetCategoriaPorCodigo(codCategoria);
            if (categoria == null) return NotFound("Categoria não encontrada");

            var loja = lojaRepository.GetLojaPorCodigo(codLoja);
            if (loja == null) return NotFound("Loja não encontrada");

            return produtoRepository.EditarProduto(codigo, nome, codCategoria, codLoja, preco);
        }

        [HttpDelete]
        [Route("ApagarProduto")]
        public ActionResult ApagarProduto(decimal codigo)
        {
            if (codigo > 0)
            {
                var produto = produtoRepository.GetProdutoPorCodigo(codigo);

                if (produto != null)
                {
                    produtoRepository.ApagarProduto(codigo);

                    return Ok("Produto apagado com sucesso!");
                }
                else
                {
                    return NotFound("Não foi encontrado produto com esse codigo");
                }
            }
            else
            {
                return BadRequest("Código do produto inválido");
            }
        }
    }
}
