using System.Data.Common;
using Dapper;
using LivrariaCultura.Domain.Interfaces;
using MySql.Data.MySqlClient;

namespace LivrariaCultura.Domain.Persistence;

public class MySqlDbContext(string? connectionString) : IDatabase
{
    private MySqlConnection? _conn;

    public DbConnection GetConnection() => _conn ??= new MySqlConnection(connectionString);
    
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

    public async Task<bool> ExecuteAsync(string sql, object? parameters = null, Dictionary<string, object>? replacingParameters = null)
    {
        await using var conn = GetConnection();
        if (replacingParameters is not null && replacingParameters.Count > 0 && parameters is not null)
        {
            var sqlParams = new DynamicParameters();
            var parametersAsDictionary = parameters.GetType()
                                                    .GetProperties()
                                                    .Where(p => p.CanRead)
                                                    .ToDictionary(
                                                        prop => prop.Name, 
                                                        prop => prop.GetValue(parameters));
            if (parametersAsDictionary is null) 
                throw new ArgumentException("Parametros da operação sql devem ser informados!");
            
            foreach (var (key, value) in parametersAsDictionary)
            {
                sqlParams.Add(key, replacingParameters.GetValueOrDefault(key, value));
            }

            return await conn.ExecuteAsync(sql, sqlParams) > 0;
        }
        return await conn.ExecuteAsync(sql, parameters) > 0;
    }

    public async Task<U> ExecuteScalarAsync<U>(string sql, object? parameters = null) where U : struct
    {
        await using var conn = GetConnection();
        return await conn.ExecuteScalarAsync<U>(sql, parameters);
    }
}