using LivrariaCultura.Domain.Interfaces;
using LivrariaCultura.Domain.Models;
using LivrariaCultura.Domain.Persistence;

namespace LivrariaCultura.Domain.Repositories;

public class CategoriaRepository(IDatabase db) : IRepository<Categoria>
{
    public async Task<Categoria?> GetAsync(uint id)
    {
        var sql = "SELECT * FROM tb_categorias WHERE Id = @Id";
        return await db.QueryOneAsync<Categoria>(sql, new { Id = id });
    }

    public async Task<IEnumerable<Categoria>> GetListAsync(uint page = 1, uint maxItems = 10)
    {
        var offset = (page - 1) * maxItems;
        var sql = "SELECT * FROM tb_categorias LIMIT @MaxItems OFFSET @Offset";
        return await db.QueryManyAsync<Categoria>(sql, new { MaxItems = maxItems, Offset = offset });
    }

    public async Task<bool> InsertAsync(Categoria entity)
    {
        var sql = "INSERT INTO tb_categorias(Nome) VALUES(@Nome)";
        return await db.ExecuteAsync(sql, entity);
    }

    public async Task<bool> UpdateAsync(Categoria entity)
    {
        var sql = "UPDATE tb_categorias SET Nome = @Nome WHERE Id = @Id";
        return await db.ExecuteAsync(sql, entity);
    }

    public async Task<bool> DeleteAsync(uint id)
    {
        var sql = "DELETE FROM tb_categorias WHERE Id = @Id";
        return await db.ExecuteAsync(sql, new { Id = id });
    }
}