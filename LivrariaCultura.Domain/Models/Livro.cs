namespace LivrariaCultura.Domain.Models;

public abstract class Livro : Produto
{
    public string Autor { get; set; } = string.Empty;
    public string Editora { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public uint QuantidadePaginas { get; set; } = 0;
    public Categoria? Categoria { get; set; } = null;
    public override decimal CalcularImposto() => 0;
}