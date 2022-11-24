using API_TRABALHO_BD1.Entity;
using Microsoft.AspNetCore.Mvc;

namespace API_TRABALHO_BD1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriaController : ControllerBase
    {

        [HttpGet]
        public Categoria GetCategoriaPorCodigo(int codigo)
        {
        }

        [HttpGet]
        public IEnumerable<Categoria> GetTodasCategorias()
        {
        }
    }
}