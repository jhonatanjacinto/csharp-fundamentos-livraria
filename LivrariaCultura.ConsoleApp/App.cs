using LivrariaCultura.ConsoleApp.Enums;
using LivrariaCultura.ConsoleApp.Screens;
using LivrariaCultura.Domain.Exceptions;
using LivrariaCultura.Domain.Interfaces;
using LivrariaCultura.Domain.Models;
using LivrariaCultura.Domain.Repositories;
using Microsoft.Win32.SafeHandles;

namespace LivrariaCultura.ConsoleApp;

public class App(CategoriasScreenManager categoriasScreenManager)
{
    private Screen CurrentScreen = Screen.Home;

    public async Task RunAsync()
    {
        while (CurrentScreen != Screen.Exit)
        {
            switch (CurrentScreen)
            {
                case Screen.Home:
                    HomeScreen();
                    break;
                case Screen.Categorias:
                    await CategoriasScreen();
                    break;
                case Screen.Produtos:
                    Console.Clear();
                    Console.Write("Tela de produtos não implementada ainda...");
                    Console.ReadKey();
                    CurrentScreen = Screen.Home;
                    break;
                default:
                    break;
            }
        }
        
        Console.Clear();
        Console.WriteLine("Programa finalizado com sucesso...");
    }

    private void HomeScreen()
    {
        Console.Clear();
        Console.WriteLine("Bem-vindo(a) ao Sistema da Livraria Cultura!");
        Console.WriteLine("Selecione uma das opções abaixo:");
        Console.WriteLine("1 - Gerenciar Categorias");
        Console.WriteLine("2 - Gerenciar Produtos");
        Console.WriteLine("3 - Sair");
        Console.Write("Informe a opção desejada: ");

        if (int.TryParse(Console.ReadLine(), out var opcao))
        {
            CurrentScreen = opcao switch
            {
                1 => Screen.Categorias,
                2 => Screen.Produtos,
                3 => Screen.Exit,
                _ => Screen.Home
            };
        }

        if (CurrentScreen == Screen.Home)
        {
            ExibirMensagemErro("Opcão selecionada é inválida! Tente novamente...");
        }
    }

    private async Task CategoriasScreen()
    {
        Console.Clear();
        Console.WriteLine("Livraria Cultura - Gerenciamento de Categorias");
        Console.WriteLine("Selecione uma das opções abaixo:");
        Console.WriteLine("1 - Cadastrar Categoria");
        Console.WriteLine("2 - Listar Categorias");
        Console.WriteLine("3 - Atualizar Categoria");
        Console.WriteLine("4 - Excluir Categoria");
        Console.WriteLine("5 - Voltar ao menu principal");
        Console.Write("Informe a opção desejada: ");
        var mensagemSucesso = string.Empty;

        try
        {
            if (int.TryParse(Console.ReadLine(), out var opcao))
            {
                switch (opcao)
                {
                    case 1:
                        mensagemSucesso = await categoriasScreenManager.CadastrarCategoriaScreen();
                        break;
                    case 2:
                        await categoriasScreenManager.ListarCategoriasScreen();
                        break;
                    case 3:
                        mensagemSucesso = await categoriasScreenManager.AtualizarCategoriaScreen();
                        break;
                    case 4:
                        mensagemSucesso = await categoriasScreenManager.ExcluirCategoriaScreen();
                        break;
                    case 5:
                        CurrentScreen = Screen.Home;
                        break;
                    default:
                        throw new CategoriaException("Opcão selecionada é inválida! Tente novamente...");
                        break;
                }
            }
            
            if (!string.IsNullOrWhiteSpace(mensagemSucesso))
            {
                ExibirMensagemSucesso(mensagemSucesso);
            }
        }
        catch (CategoriaException ex)
        {
            ExibirMensagemErro(ex.Message);
        }
        catch (Exception ex)
        {
            ExibirMensagemErro("Não foi possível realizar a operação desejada no gerencimento de categorias. Tente novamente!");
            // Log ex.Message
        }
    }

    private static void ExibirMensagemErro(string mensagem)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(mensagem);
        Console.ResetColor();
        Console.ReadKey();
    }

    private static void ExibirMensagemSucesso(string mensagem)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(mensagem);
        Console.ResetColor();
        Console.ReadKey();
    }
}