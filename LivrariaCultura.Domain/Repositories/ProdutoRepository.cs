using Dapper;
using LivrariaCultura.Domain.Interfaces;
using LivrariaCultura.Domain.Models;
using LivrariaCultura.Domain.Utility;

namespace LivrariaCultura.Domain.Repositories;

public class ProdutoRepository<T>(IDatabase db) : IRepository<T> where T : Produto, new()
{
    public async Task<T?> GetAsync(uint id)
    {
        var sql = ProdutoSqlBuilder.BuildSelectFor<T>(true);
        await using var conn = db.GetConnection();
        
        var t = new T();
        if (t is Livro)
        {
            var result = await conn.QueryAsync<T, Categoria, T?>(sql, (produto, categoria) =>
            {
                (produto as Livro)!.Categoria = categoria;
                return produto;
            }, new { Id = id }, splitOn: "Id");
            
            return result.FirstOrDefault();
        }
        
        return await conn.QueryFirstOrDefaultAsync<T>(sql, new { Id = id });
    }

    public async Task<IEnumerable<T>> GetListAsync(uint page = 1, uint maxItems = 10)
    {
        var offset = (page - 1) * maxItems;
        var sql = ProdutoSqlBuilder.BuildSelectFor<T>();
        await using var conn = db.GetConnection();

        var t = new T();
        if (t is Livro)
        {
            return await conn.QueryAsync<T, Categoria, T>(sql, (produto, categoria) =>
            {
                (produto as Livro)!.Categoria = categoria;
                return produto;
            }, new { Offset = offset, MaxItems = maxItems }, splitOn: "Id");
        }
        
        return await conn.QueryAsync<T>(sql, new { Offset = offset, MaxItems = maxItems });
    }

    public async Task<bool> InsertAsync(T entity)
    {
        var sql = ProdutoSqlBuilder.BuildInsertFor<T>();
        Dictionary<string,object>? replacingParameters = new();

        if (entity is Livro { Categoria: not null } livro)
        {
            replacingParameters.Add("Categoria", livro.Categoria.Id);
        }
        
        return await db.ExecuteAsync(sql, entity, replacingParameters);
    }

    public async Task<bool> UpdateAsync(T entity)
    {
        var sql = ProdutoSqlBuilder.BuildUpdateFor<T>();
        Dictionary<string,object>? replacingParameters = new();

        if (entity is Livro { Categoria: not null } livro)
        {
            replacingParameters.Add("Categoria", livro.Categoria.Id);
        }
        
        return await db.ExecuteAsync(sql, entity, replacingParameters);
    }

    public async Task<bool> DeleteAsync(uint id)
    {
        var sql = ProdutoSqlBuilder.BuildDeleteFor<T>();
        return await db.ExecuteAsync(sql, new { Id = id });
    }
}