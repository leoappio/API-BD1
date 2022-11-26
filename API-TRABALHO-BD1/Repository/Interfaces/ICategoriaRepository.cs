using API_TRABALHO_BD1.Entity;

namespace API_TRABALHO_BD1.Repository.Interfaces
{
    public interface ICategoriaRepository
    {
        Categoria InserirCategoria(string descricao);
        Categoria GetCategoriaPorCodigo(decimal codigo);
        IEnumerable<Categoria> GetTodasAsCategorias();
        Categoria EditarCategoria(decimal codigo, string descricao);
        void ApagarCategoria(decimal codigo);
    }
}
