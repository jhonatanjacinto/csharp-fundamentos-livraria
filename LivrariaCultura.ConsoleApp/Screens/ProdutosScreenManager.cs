using ConsoleTables;
using LivrariaCultura.Domain.Enums;
using LivrariaCultura.Domain.Exceptions;
using LivrariaCultura.Domain.Models;
using LivrariaCultura.Domain.Repositories;

namespace LivrariaCultura.ConsoleApp.Screens;

public class ProdutosScreenManager(
    CategoriaRepository categoriaRepository,
    ProdutoRepository<LivroFisico> livroFisicoRepository,
    ProdutoRepository<Ebook> ebookRepository,
    ProdutoRepository<Papelaria> papelariaRepository,
    ProdutoRepository<Bebida> bebidaRepository)
{
    public async Task<string> CadastrarProdutoScreen()
    {
        Console.Clear();
        Console.WriteLine("Livraria Cultura - Cadastrar Produto");
        ExibirTiposProdutos();
        Console.Write("Informe o tipo de produto: ");

        if (int.TryParse(Console.ReadLine(), out var opcao))
        {
            switch (opcao)
            {
                case 1:
                    var livroFisico = new LivroFisico();
                    PreencherDadosProduto(livroFisico);
                    await PreencherDadosLivro(livroFisico);
                    PreencherDadosLivroFisico(livroFisico);
                    await livroFisicoRepository.InsertAsync(livroFisico);
                    break;
                case 2:
                    var ebook = new Ebook();
                    PreencherDadosProduto(ebook);
                    await PreencherDadosLivro(ebook);
                    PreencherDadosEbook(ebook);
                    await ebookRepository.InsertAsync(ebook);
                    break;
                case 3:
                    var papelaria = new Papelaria();
                    PreencherDadosProduto(papelaria);
                    PreencherDadosPapelaria(papelaria);
                    await papelariaRepository.InsertAsync(papelaria);
                    break;
                case 4:
                    var bebida = new Bebida();
                    PreencherDadosProduto(bebida);
                    PreencherDadosBebida(bebida);
                    await bebidaRepository.InsertAsync(bebida);
                    break;
            }
        }

        return "Produto cadastrado com sucesso!";
    }

    public async Task<string> AtualizarProdutoScreen()
    {
        Console.Clear();
        Console.WriteLine("Livraria Cultura - Atualizar Produto");
        Console.WriteLine("Informe o Id do produto que deseja atualizar: ");
        if (!uint.TryParse(Console.ReadLine(), out var idProduto))
        {
            throw new ProdutoException("Id do produto é inválido!");
        }
        
        ExibirTiposProdutos();
        Console.Write("Informe o tipo de produto: ");
        if (int.TryParse(Console.ReadLine(), out var opcao))
        {
            switch (opcao)
            {
                case 1:
                    var livroFisico = await livroFisicoRepository.GetAsync(idProduto);
                    if (livroFisico == null)
                    {
                        throw new ProdutoException("Livro físico não encontrado!");
                    }
                    PreencherDadosProduto(livroFisico);
                    await PreencherDadosLivro(livroFisico);
                    PreencherDadosLivroFisico(livroFisico);
                    await livroFisicoRepository.UpdateAsync(livroFisico);
                    break;
                case 2:
                    var ebook = await ebookRepository.GetAsync(idProduto);
                    if (ebook == null)
                    {
                        throw new ProdutoException("E-book não encontrado!");
                    }
                    PreencherDadosProduto(ebook);
                    await PreencherDadosLivro(ebook);
                    PreencherDadosEbook(ebook);
                    await ebookRepository.UpdateAsync(ebook);
                    break;
                case 3:
                    var papelaria = await papelariaRepository.GetAsync(idProduto);
                    if (papelaria == null)
                    {
                        throw new ProdutoException("Papelaria não encontrada!");
                    }
                    PreencherDadosProduto(papelaria);
                    PreencherDadosPapelaria(papelaria);
                    await papelariaRepository.UpdateAsync(papelaria);
                    break;
                case 4:
                    var bebida = await bebidaRepository.GetAsync(idProduto);
                    if (bebida == null)
                    {
                        throw new ProdutoException("Bebida não encontrada!");
                    }
                    PreencherDadosProduto(bebida);
                    PreencherDadosBebida(bebida);
                    await bebidaRepository.UpdateAsync(bebida);
                    break;
            }
        }
        return "Produto atualizado com sucesso!";
    }
    
    public async Task<string> ExcluirProdutoScreen()
    {
        Console.Clear();
        Console.WriteLine("Livraria Cultura - Excluir Produto");
        Console.WriteLine("Informe o Id do produto que deseja excluir: ");
        if (!uint.TryParse(Console.ReadLine(), out var idProduto))
        {
            throw new ProdutoException("Id do produto é inválido!");
        }
        ExibirTiposProdutos();
        Console.Write("Informe o tipo de produto: ");
        if (int.TryParse(Console.ReadLine(), out var opcao))
        {
            switch (opcao)
            {
                case 1:
                    await livroFisicoRepository.DeleteAsync(idProduto);
                    break;
                case 2:
                    await ebookRepository.DeleteAsync(idProduto);
                    break;
                case 3:
                    await papelariaRepository.DeleteAsync(idProduto);
                    break;
                case 4:
                    await bebidaRepository.DeleteAsync(idProduto);
                    break;
            }
        }
        return "Produto excluído com sucesso!";
    }
    
    public async Task<string> ListarProdutosScreen()
    {
        Console.Clear();
        Console.WriteLine("Livraria Cultura - Listar Produtos");
        ExibirTiposProdutos();
        Console.Write("Informe o tipo de produto para listar: ");
        if (int.TryParse(Console.ReadLine(), out var opcao))
        {
            switch (opcao)
            {
                case 1:
                    var livrosFisicos = await livroFisicoRepository.GetListAsync();
                    var livrosFisicosTable = new ConsoleTable("Id", "Nome", "Descrição", "Preço", "Quantidade em Estoque", "Autor", "Editora", "ISBN", "Número de Páginas", "Categoria", "Usado");
                    foreach (var livroFisico in livrosFisicos)
                    {
                        livrosFisicosTable.AddRow(
                            livroFisico.Id,
                            livroFisico.Nome, 
                            livroFisico.Descricao, 
                            livroFisico.Preco, 
                            livroFisico.QuantidadeEstoque, 
                            livroFisico.Autor, 
                            livroFisico.Editora, 
                            livroFisico.Isbn, 
                            livroFisico.QuantidadePaginas, 
                            livroFisico.Categoria?.Nome, 
                            livroFisico.IsUsado ? "Sim" : "Não");
                    }
                    livrosFisicosTable.Write();
                    break;
                case 2:
                    var ebooks = await ebookRepository.GetListAsync();
                    var ebooksTable = new ConsoleTable("Id", "Nome", "Descrição", "Preço", "Quantidade em Estoque", "Autor", "Editora", "ISBN", "Número de Páginas", "Categoria", "Formato");
                    foreach (var ebook in ebooks)
                    {
                        ebooksTable.AddRow(
                            ebook.Id, 
                            ebook.Nome, 
                            ebook.Descricao, 
                            ebook.Preco, 
                            ebook.QuantidadeEstoque, 
                            ebook.Autor, 
                            ebook.Editora, 
                            ebook.Isbn, 
                            ebook.QuantidadePaginas, 
                            ebook.Categoria?.Nome, 
                            ebook.Formato);
                    }
                    ebooksTable.Write();
                    break;
                case 3:
                    var papelarias = await papelariaRepository.GetListAsync();
                    var papelariasTable = new ConsoleTable("Id", "Nome", "Descrição", "Preço", "Quantidade em Estoque", "Marca", "Tipo");
                    foreach (var papelaria in papelarias)
                    {
                        papelariasTable.AddRow(
                            papelaria.Id, 
                            papelaria.Nome, 
                            papelaria.Descricao, 
                            papelaria.Preco, 
                            papelaria.QuantidadeEstoque, 
                            papelaria.Marca, 
                            papelaria.TipoPapelaria);
                    }
                    papelariasTable.Write();
                    break;
                case 4:
                    var bebidas = await bebidaRepository.GetListAsync();
                    var bebidasTable = new ConsoleTable("Id", "Nome", "Descrição", "Preço", "Quantidade em Estoque", "Tipo");
                    foreach (var bebida in bebidas)
                    {
                        bebidasTable.AddRow(
                            bebida.Id, 
                            bebida.Nome, 
                            bebida.Descricao, 
                            bebida.Preco, 
                            bebida.QuantidadeEstoque, 
                            bebida.Tipo);
                    }
                    bebidasTable.Write();
                    break;
            }
        }

        Console.WriteLine("Pressione qualquer tecla para continuar...");
        Console.ReadKey();
        return "";
    }

    private static void ExibirTiposProdutos()
    {
        Console.WriteLine("Tipos de produtos disponíveis:");
        Console.WriteLine("1 - Livro Físico");
        Console.WriteLine("2 - E-book");
        Console.WriteLine("3 - Papelaria");
        Console.WriteLine("4 - Bebida");
    }

    private static void ExibirTiposPapelarias()
    {
        Console.WriteLine("Tipos de produtos disponíveis:");
        Console.WriteLine("1 - Caderno");
        Console.WriteLine("2 - Caneta");
        Console.WriteLine("3 - Lápis");
        Console.WriteLine("4 - Borracha");
        Console.WriteLine("5 - Apontador");
        Console.WriteLine("6 - Régua");
        Console.WriteLine("7 - Estojo");
    }

    private static void PreencherDadosProduto(Produto produto)
    {
        Console.Write("Informe o nome do produto: ");
        var n = Console.ReadLine();
        var nome = string.IsNullOrEmpty(n) ? produto.Nome : n;
        
        if (string.IsNullOrWhiteSpace(nome))
        {
            throw new ProdutoException("Nome do produto é obrigatório!");
        }

        produto.Nome = nome;
        
        Console.Write("Informe a descricao do produto: ");
        var d = Console.ReadLine();
        produto.Descricao = string.IsNullOrEmpty(d) ? produto.Descricao : d;
        
        Console.Write("Informe o preço do produto: ");
        decimal preco = 0;
        if (!decimal.TryParse(Console.ReadLine(), out preco) || preco < 0)
        {
            throw new ProdutoException("Preço do produto é inválido!");
        }
        
        produto.Preco = preco;
        
        Console.Write("Informe a quantidade em estoque do produto: ");
        uint quantidadeEstoque = 0;
        if (!uint.TryParse(Console.ReadLine(), out quantidadeEstoque))
        {
            throw new ProdutoException("Quantidade em estoque do produto é inválida!");
        }
        
        produto.QuantidadeEstoque = quantidadeEstoque;
        
        Console.Write("O produto está em promoção? S/N: ");
        var promocao = Console.ReadLine()?.ToUpper() ?? string.Empty;
        produto.IsEmPromocao = promocao == "S";
    }
    
    private async Task PreencherDadosLivro(Livro produto)
    {
        Console.Write("Informe o autor do livro: ");
        var a = Console.ReadLine();
        var autor = string.IsNullOrEmpty(a) ? produto.Autor : a;

        if (string.IsNullOrWhiteSpace(autor))
        {
            throw new ProdutoException("Autor do livro é obrigatório!");
        }
        
        produto.Autor = autor;
        
        Console.Write("Informe a editora do livro: ");
        var e = Console.ReadLine();
        var editora = string.IsNullOrEmpty(e) ? produto.Editora : e;

        if (string.IsNullOrWhiteSpace(editora))
        {
            throw new ProdutoException("Editora do livro é obrigatório!");
        }
        
        produto.Editora = editora;
        
        Console.Write("Informe o ISBN do livro: ");
        var i = Console.ReadLine();
        var isbn = string.IsNullOrEmpty(i) ? produto.Isbn : i;

        if (string.IsNullOrWhiteSpace(isbn))
        {
            throw new ProdutoException("ISBN do livro é obrigatório!");
        }
        
        produto.Isbn = isbn;
        
        Console.Write("Informe o número de páginas do livro: ");
        uint numeroPaginas = 0;
        if (!uint.TryParse(Console.ReadLine(), out numeroPaginas))
        {
            throw new ProdutoException("Número de páginas do livro é inválido!");
        }
        
        produto.QuantidadePaginas = numeroPaginas;
        
        Console.WriteLine("Categorias disponíveis:");
        var categoriasTable = new ConsoleTable("Id", "Categoria");
        var categorias = await categoriaRepository.GetListAsync();
        foreach (var categoria in categorias)
        {
            categoriasTable.AddRow(categoria.Id, categoria.Nome);
        }
        categoriasTable.Write();
        
        Console.Write("Informe o nome da categoria do livro: ");
        var nomeCategoria = Console.ReadLine() ?? string.Empty;
        var categoriaEncontrada = categorias.FirstOrDefault(c => c.Nome.Contains(nomeCategoria, StringComparison.OrdinalIgnoreCase));
        if (categoriaEncontrada == null)
        {
            throw new ProdutoException("Categoria do livro é inválida!");
        }

        produto.Categoria = categoriaEncontrada;
        Console.WriteLine($"Categoria {categoriaEncontrada.Nome} selecionada e associada ao livro!");
    }
    
    private static void PreencherDadosLivroFisico(LivroFisico produto)
    {
        Console.Write("Este livro é Usado? S/N: ");
        var usado = Console.ReadLine()?.ToUpper() ?? string.Empty;
        produto.IsUsado = usado == "S";
    }
    
    private static void PreencherDadosEbook(Ebook produto)
    {
        Console.WriteLine("Formatos Disponíveis:");
        Console.WriteLine("1 - Mobi");
        Console.WriteLine("2 - Pdf");
        Console.WriteLine("3 - Epub");
        Console.WriteLine("4 - Kpf");
        Console.Write("Informe o formato do E-book: ");

        if (!Enum.TryParse<FormatoEbook>(Console.ReadLine(), out var formatoEbook))
        {
            throw new ProdutoException("Formato do E-book inválido!");
        }
        
        produto.Formato = formatoEbook;
    }
    
    private static void PreencherDadosPapelaria(Papelaria produto)
    {
        Console.Write("Informe a marca do produto: ");
        var m = Console.ReadLine();
        produto.Marca = string.IsNullOrEmpty(m) ? produto.Marca : m;
        ExibirTiposPapelarias();
        
        Console.Write("Informe o tipo de papelaria: ");
        if (!Enum.TryParse<TipoPapelaria>(Console.ReadLine(), out var tipoPapelaria))
        {
            throw new ProdutoException("Tipo de papelaria inválido!");
        }

        produto.TipoPapelaria = tipoPapelaria;
    }

    private static void PreencherDadosBebida(Bebida bebida)
    {
        Console.Write("Informe o tipo de bebida (1 - Quente, 2 - Frio): ");
        if (!Enum.TryParse<TipoBebida>(Console.ReadLine(), out var tipoBebida))
        {
            throw new ProdutoException("Tipo de bebida inválido!");
        }

        bebida.Tipo = tipoBebida;
    }
}