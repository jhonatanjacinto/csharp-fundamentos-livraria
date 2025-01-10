using System.Text;
using LivrariaCultura.Domain.Exceptions;
using LivrariaCultura.Domain.Models;

namespace LivrariaCultura.Domain.Utility;

public static class ProdutoSqlBuilder
{
    private const string ProdutosTable = "tb_produtos";
    private const string LivrosTable = "tb_livros";
    private const string LivrosFisicosTable = "tb_livros_fisicos";
    private const string EbooksTable = "tb_ebooks";
    private const string PapelariaTable = "tb_papelaria";
    private const string BebidasTable = "tb_bebidas";
    private const string CategoriasTable = "tb_categorias";
    
    public static string BuildInsertFor<T>() where T : Produto, new()
    {
        var sqlBuilder = new StringBuilder();
        var type = typeof(T);
        var typeProduto = GetTypeProdutoDb(type.Name);
        
        sqlBuilder.Append($"""
                          INSERT INTO {ProdutosTable}(Nome, Descricao, Preco, QuantidadeEstoque, IsEmPromocao, TipoProduto)
                          VALUES (@Nome, @Descricao, @Preco, @QuantidadeEstoque, @IsEmPromocao, '{typeProduto}');
                          SET @lastId = LAST_INSERT_ID();
                          """);
        
        var sql = type.Name switch
        {
            "LivroFisico" => $"""
                              INSERT INTO {LivrosTable}(ProdutoId, Autor, Editora, ISBN, QuantidadePaginas, Categoria)
                              VALUES(@lastId, @Autor, @Editora, @ISBN, @QuantidadePaginas, @Categoria);
                              INSERT INTO {LivrosFisicosTable}(ProdutoId, IsUsado)
                              VALUES(@lastId, @IsUsado);
                              """,
            "Ebook" => $"""
                        INSERT INTO {LivrosTable}(ProdutoId, Autor, Editora, ISBN, QuantidadePaginas, Categoria)
                        VALUES(@lastId, @Autor, @Editora, @ISBN, @QuantidadePaginas, @Categoria);
                        INSERT INTO {EbooksTable}(ProdutoId, Formato)
                        VALUES(@lastId, @Formato);
                        """,
            "Papelaria" => $"""
                           INSERT INTO {PapelariaTable}(ProdutoId, Marca, TipoPapelaria)
                           VALUES(@lastId, @Marca, @Tipo);
                           """,
            "Bebida" => $"""
                        INSERT INTO {BebidasTable}(ProdutoId, TipoBebida)
                        VALUES(@lastId, @Tipo);
                        """,
            _ => throw new ProdutoException("Tipo de produto inválido para inserção.")
            
        };
        
        sqlBuilder.Append(sql);

        return sqlBuilder.ToString();
    }

    private static string GetTypeProdutoDb(string typeName)
    {
        return typeName switch
        {
            "LivroFisico" => "Livro Físico",
            "Ebook" => "E-book",
            _ => typeName
        };
    }

    public static string BuildSelectFor<T>(bool single = false) where T : Produto, new()
    {
        var sqlBuilder = new StringBuilder();
        var type = typeof(T);
        var typeProduto = GetTypeProdutoDb(type.Name);
        
        sqlBuilder.Append($"SELECT p.* ");
        
        var sql = type.Name switch
        {
            "LivroFisico" => $"""
                              , l.Autor, l.Editora, l.Isbn, l.QuantidadePaginas, lf.IsUsado, c.Id, c.Nome FROM {ProdutosTable} p
                              JOIN {LivrosTable} l
                              ON p.Id = l.ProdutoId
                              JOIN {CategoriasTable} c
                              ON l.Categoria = c.Id
                              JOIN {LivrosFisicosTable} lf
                              ON p.Id = lf.ProdutoId
                              """,
            "Ebook" => $"""
                        , l.Autor, l.Editora, l.Isbn, l.QuantidadePaginas, e.Formato, c.Id, c.Nome FROM {ProdutosTable} p
                        JOIN {LivrosTable} l
                        ON p.Id = l.ProdutoId
                        JOIN {CategoriasTable} c
                        ON l.Categoria = c.Id
                        JOIN {EbooksTable} e
                        ON p.Id = e.ProdutoId
                        """,
            "Papelaria" => $"""
                           , pa.* FROM {ProdutosTable} p
                           JOIN {PapelariaTable} pa
                           ON p.Id = pa.ProdutoId
                           """,
            "Bebida" => $"""
                        , b.TipoBebida As Tipo FROM {ProdutosTable} p
                        JOIN {BebidasTable} b
                        ON p.Id = b.ProdutoId
                        """,
            _ => throw new ProdutoException("Tipo de produto inválido para seleção.")
        };
        
        sqlBuilder.Append(sql);
        sqlBuilder.Append($" WHERE p.TipoProduto = '{typeProduto}'");
        sqlBuilder.Append(single ? " AND p.Id = @Id;" : " ORDER BY p.Id ASC LIMIT @Offset, @MaxItems;");

        return sqlBuilder.ToString();
    }
    
    public static string BuildUpdateFor<T>() where T : Produto, new()
    {
        var sqlBuilder = new StringBuilder();
        var type = typeof(T);
        var typeProduto = GetTypeProdutoDb(type.Name);
        
        sqlBuilder.Append($"""
                          UPDATE {ProdutosTable}
                          SET Nome = @Nome, Descricao = @Descricao, Preco = @Preco, QuantidadeEstoque = @QuantidadeEstoque, IsEmPromocao = @IsEmPromocao
                          WHERE Id = @Id;
                          """);
        
        var sql = type.Name switch
        {
            "LivroFisico" => $"""
                              UPDATE {LivrosTable}
                              SET Autor = @Autor, Editora = @Editora, ISBN = @ISBN, QuantidadePaginas = @QuantidadePaginas, Categoria = @Categoria
                              WHERE ProdutoId = @Id;
                              UPDATE {LivrosFisicosTable}
                              SET IsUsado = @IsUsado
                              WHERE ProdutoId = @Id;
                              """,
            "Ebook" => $"""
                        UPDATE {LivrosTable}
                        SET Autor = @Autor, Editora = @Editora, ISBN = @ISBN, QuantidadePaginas = @QuantidadePaginas, Categoria = @Categoria
                        WHERE ProdutoId = @Id;
                        UPDATE {EbooksTable}
                        SET Formato = @Formato
                        WHERE ProdutoId = @Id;
                        """,
            "Papelaria" => $"""
                           UPDATE {PapelariaTable}
                           SET Marca = @Marca, TipoPapelaria = @TipoPapelaria
                           WHERE ProdutoId = @Id;
                           """,
            "Bebida" => $"""
                        UPDATE {BebidasTable}
                        SET TipoBebida = @Tipo
                        WHERE ProdutoId = @Id;
                        """,
            _ => throw new ProdutoException("Tipo de produto inválido para atualização.")
        };
        
        sqlBuilder.Append(sql);

        return sqlBuilder.ToString();
    }
    
    public static string BuildDeleteFor<T>() where T : Produto, new()
    {
        var type = typeof(T);
        var typeProduto = GetTypeProdutoDb(type.Name);
        var sql = $"DELETE FROM {ProdutosTable} WHERE Id = @Id AND TipoProduto = '{typeProduto}';";
        return sql;
    }
}