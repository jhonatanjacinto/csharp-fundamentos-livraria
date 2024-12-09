namespace LivrariaCultura.ConsoleApp.Screens;

public class ProdutosScreenManager
{
    public async Task<string> CadastrarProdutoScreen()
    {
        Console.Clear();
        Console.WriteLine("Livraria Cultura - Cadastrar Produto");
        ExibirTiposProdutos();
        Console.Write("Informe o tipo de produto: ");
        return "";
    }
    
    public async Task<string> AtualizarProdutoScreen()
    {
        return "";
    }
    
    public async Task<string> ExcluirProdutoScreen()
    {
        return "";
    }
    
    public async Task ListarProdutosScreen()
    {
        Console.WriteLine("Listando produtos...");
    }

    private static void ExibirTiposProdutos()
    {
        Console.WriteLine("Tipos de produtos disponíveis:");
        Console.WriteLine("1 - Livro Físico");
        Console.WriteLine("2 - E-book");
        Console.WriteLine("3 - Papelaria");
        Console.WriteLine("4 - Bebida");
    }
}