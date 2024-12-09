namespace LivrariaCultura.Domain.Interfaces;

public interface IDatabase
{
    // método para retornar apenas 1 registro
    Task<T?> QueryOneAsync<T>(string sql, object? parameters = null);
    
    // método para retornar uma lista de registros
    Task<IEnumerable<T>> QueryManyAsync<T>(string sql, object? parameters = null);
    
    // método para executar operações que não sejam queries (Insert, Update e Delete)
    Task<bool> ExecuteAsync(string sql, object? parameters = null);
    
    // método para retornar dados escalares (scalar) (SELECT COUNT(*))
    // só aceita tipos de valor em U
    Task<U> ExecuteScalarAsync<U>(string sql, object? parameters = null) where U : struct; 
}