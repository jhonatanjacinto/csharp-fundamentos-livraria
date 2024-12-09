using ConsoleTables;
using LivrariaCultura.Domain.Exceptions;
using LivrariaCultura.Domain.Models;
using LivrariaCultura.Domain.Repositories;

namespace LivrariaCultura.ConsoleApp.Screens;

public class CategoriasScreenManager(CategoriaRepository categoriaRepository)
{
    public async Task<string> CadastrarCategoriaScreen()
    {
        Console.Clear();
        Console.WriteLine("Livraria Cultura - Cadastrar Categoria");
        Console.Write("Informe o nome da categoria: ");

        var nomeCategoria = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(nomeCategoria))
        {
            throw new CategoriaException("Nome da Categoria é inválido!");
        }

        Categoria categoria = new(0, nomeCategoria);
        await categoriaRepository.InsertAsync(categoria);
        
        return "Categoria cadastrada com sucesso!";
    }

    public async Task<string> AtualizarCategoriaScreen()
    {
        Console.Clear();
        Console.WriteLine("Livraria Cultura - Atualizar Categoria");
        Console.Write("Informe o Id da categoria: ");

        if (!uint.TryParse(Console.ReadLine(), out var idCategoria))
        {
            throw new CategoriaException("Id da Categoria é inválido!");
        }

        var categoria = await categoriaRepository.GetAsync(idCategoria);
        if (categoria == null)
        {
            throw new CategoriaException("Categoria não encontrada!");
        }
        
        Console.WriteLine($"Você irá atualizar a categoria abaixo: {categoria.Nome}");
        Console.Write("Informe o novo nome da categoria: ");
        var novoNomeCategoria = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(novoNomeCategoria))
        {
            throw new CategoriaException("Nome da Categoria é inválido!");
        }

        Categoria categoriaAtualizada = new(idCategoria, novoNomeCategoria);
        await categoriaRepository.UpdateAsync(categoriaAtualizada);

        return "Categoria atualizada com sucesso!";
    }

    public async Task<string> ExcluirCategoriaScreen()
    {
        Console.Clear();
        Console.WriteLine("Livraria Cultura - Excluir Categoria");
        Console.Write("Informe o Id da categoria: ");

        if (!uint.TryParse(Console.ReadLine(), out var idCategoria))
        {
            throw new CategoriaException("Id da Categoria é inválido!");
        }

        var categoria = await categoriaRepository.GetAsync(idCategoria);
        if (categoria == null)
        {
            throw new CategoriaException("Categoria não encontrada!");
        }
        
        Console.WriteLine($"Você irá excluir a categoria abaixo: {categoria.Nome}");
        Console.WriteLine("Deseja realmente excluir a categoria? (S/N)");
        var confirmacao = Console.ReadLine()?.ToUpper() ?? string.Empty;
        
        if (confirmacao != "S")
        {
            return "Operação cancelada!";
        }

        await categoriaRepository.DeleteAsync(idCategoria);

        return "Categoria excluída com sucesso!";
    }

    public async Task ListarCategoriasScreen()
    {
        Console.Clear();
        Console.WriteLine("Livraria Cultura - Listar Categorias");
        var enumerable = await categoriaRepository.GetListAsync();
        var categorias = enumerable.ToList();
        var categoriasTable = new ConsoleTable("Id", "Categoria");
        if (categorias.Count > 0)
        {
            foreach (var categoria in categorias)
            {
                categoriasTable.AddRow(categoria.Id, categoria.Nome);
            }
            
            categoriasTable.Write();
            Console.WriteLine();
            Console.WriteLine("Pressione qualquer tecla para continuar...");
        }
        else
        {
            Console.WriteLine("Nenhuma categoria cadastrada!");
        }

        Console.ReadKey();
    }
}