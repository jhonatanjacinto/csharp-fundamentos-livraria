using System.Text;
using LivrariaCultura.Domain.Exceptions;

namespace LivrariaCultura.Domain.Utility;

public static class ProdutoSqlBuilder
{
    private const string ProdutosTable = "tb_produtos";
    private const string LivrosTable = "tb_livros";
    private const string LivrosFisicosTable = "tb_livros_fisicos";
    private const string EbooksTable = "tb_ebooks";
    private const string PapelariaTable = "tb_papelaria";
    private const string BebidasTable = "tb_bebidas";
    
    public static string BuildInsertFor<T>() where T : class, new()
    {
        var sqlBuilder = new StringBuilder();
        var type = typeof(T);
        
        sqlBuilder.Append($"""
                          START TRANSACTION
                          INSERT INTO {ProdutosTable}(Nome, Descricao, Preco, QuantidadeEstoque, IsEmPromocao, TipoProduto)
                          VALUES (@Nome, @Descricao, @Preco, @QuantidadeEstoque, @IsEmPromocao, 'Livro Físico');
                          
                          SET @lastId = LAST_INSERT_ID();
                          """);
        
        var sql = type.Name switch
        {
            "LivroFisico" => $"""
                              INSERT INTO {LivrosTable}(ProdutoId, Autor, Editora, ISBN, QuantidadePaginas, CategoriaId)
                              VALUES(@lastId, @Autor, @Editora, @ISBN, @QuantidadePaginas, @Categoria);
                              INSERT INTO {LivrosFisicosTable}(ProdutoId, IsUsado)
                              VALUES(@lastId, @IsUsado);
                              """,
            "Ebook" => $"""
                        INSERT INTO {LivrosTable}(ProdutoId, Autor, Editora, ISBN, QuantidadePaginas, CategoriaId)
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
        sqlBuilder.Append("COMMIT;");

        return sqlBuilder.ToString();
    }
    
    public static string BuildSelectFor<T>(bool single = false) where T : class, new()
    {
        var sqlBuilder = new StringBuilder();
        var type = typeof(T);
        
        sqlBuilder.Append($"SELECT * FROM {ProdutosTable} p");
        
        var sql = type.Name switch
        {
            "LivroFisico" => $"""
                              JOIN {LivrosTable} l
                              ON p.Id = l.ProdutoId
                              JOIN {LivrosFisicosTable} lf
                              ON p.Id = lf.ProdutoId
                              """,
            "Ebook" => $"""
                        JOIN {LivrosTable} l
                        ON p.Id = l.ProdutoId
                        JOIN {EbooksTable} e
                        ON p.Id = e.ProdutoId
                        """,
            "Papelaria" => $"""
                           JOIN {PapelariaTable} pa
                           ON p.Id = pa.ProdutoId
                           """,
            "Bebida" => $"""
                        JOIN {BebidasTable} b
                        ON p.Id = b.ProdutoId
                        """,
            _ => throw new ProdutoException("Tipo de produto inválido para seleção.")
        };
        
        sqlBuilder.Append(sql);
        sqlBuilder.Append("ORDER BY p.Id ASC LIMIT @Offset, @MaxItems;");

        if (single)
        {
            sqlBuilder.Append("WHERE p.Id = @Id;");
        }

        return sqlBuilder.ToString();
    }
}