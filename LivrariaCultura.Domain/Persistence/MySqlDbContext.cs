using System.Data.Common;
using Dapper;
using LivrariaCultura.Domain.Interfaces;
using MySql.Data.MySqlClient;

namespace LivrariaCultura.Domain.Persistence;

public class MySqlDbContext(string? connectionString) : IDatabase
{
    private MySqlConnection? _conn;

    private DbConnection GetConnection() => _conn ??= new MySqlConnection(connectionString);
    
    public async Task<T?> QueryOneAsync<T>(string sql, object? parameters = null)
    {
        await using var conn = GetConnection();
        return await conn.QueryFirstOrDefaultAsync<T>(sql, parameters);
    }

    public async Task<IEnumerable<T>> QueryManyAsync<T>(string sql, object? parameters = null)
    {
        await using var conn = GetConnection();
        return await conn.QueryAsync<T>(sql, parameters);
    }

    public async Task<bool> ExecuteAsync(string sql, object? parameters = null)
    {
        await using var conn = GetConnection();
        return await conn.ExecuteAsync(sql, parameters) > 0;
    }

    public async Task<U> ExecuteScalarAsync<U>(string sql, object? parameters = null) where U : struct
    {
        await using var conn = GetConnection();
        return await conn.ExecuteScalarAsync<U>(sql, parameters);
    }
}