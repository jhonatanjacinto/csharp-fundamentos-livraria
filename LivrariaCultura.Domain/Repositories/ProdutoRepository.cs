using LivrariaCultura.Domain.Interfaces;
using LivrariaCultura.Domain.Models;
using LivrariaCultura.Domain.Utility;

namespace LivrariaCultura.Domain.Repositories;

public class ProdutoRepository<T>(IDatabase db) : IRepository<T> where T : Produto, new()
{
    public async Task<T?> GetAsync(uint id)
    {
        var sql = ProdutoSqlBuilder.BuildSelectFor<T>(true);
        return await db.QueryOneAsync<T>(sql, new { Id = id });
    }

    public async Task<IEnumerable<T>> GetListAsync(uint page = 1, uint maxItems = 10)
    {
        var offset = (page - 1) * maxItems;
        var sql = ProdutoSqlBuilder.BuildSelectFor<T>();
        return await db.QueryManyAsync<T>(sql, new { Offset = offset, MaxItems = maxItems });
    }

    public async Task<bool> InsertAsync(T entity)
    {
        var sql = ProdutoSqlBuilder.BuildInsertFor<T>();
        return await db.ExecuteAsync(sql, entity);
    }

    public async Task<bool> UpdateAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(uint id)
    {
        throw new NotImplementedException();
    }
}