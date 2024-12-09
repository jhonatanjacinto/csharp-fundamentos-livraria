namespace LivrariaCultura.Domain.Interfaces;

public interface IRepository<T>
{
    // 1. Retornar apenas 1 dado da Entidade que o repository representa
    Task<T?> GetAsync(uint id);
    
    // 2. Retornar uma lista paginada das Entidades representadas pelo repository
    Task<IEnumerable<T>> GetListAsync(uint page = 1, uint maxItems = 10);

    // 3. Operação de Inserção de dados da Entidade
    Task<bool> InsertAsync(T entity);

    // 4. Operação de Atualização de dados da Entidade
    Task<bool> UpdateAsync(T entity);

    // 5. Operação de Exclusão de dados da Entidade
    Task<bool> DeleteAsync(uint id);
}