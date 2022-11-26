using API_TRABALHO_BD1.Entity;

namespace API_TRABALHO_BD1.Repository.Interfaces
{
    public interface IProdutoRepository
    {
        Produto InserirProduto(string nome, int codCategoria, int codLoja, float preco);
        Produto GetProdutoPorCodigo(decimal codigo);
        IEnumerable<Produto> GetTodosOsProdutos();
        Produto EditarProduto(decimal codigo, string nome, int codCategoria, int codLoja, float preco);
        void ApagarProduto(decimal codigo);
    }
}
