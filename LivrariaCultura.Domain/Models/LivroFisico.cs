namespace LivrariaCultura.Domain.Models;

public class LivroFisico : Livro
{
    public bool IsUsado { get; set; } = false;
}