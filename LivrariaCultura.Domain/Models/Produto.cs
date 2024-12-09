namespace LivrariaCultura.Domain.Models;

public abstract class Produto
{
    public uint Id { get; set; } = 0;
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal Preco { get; set; } = 0;
    public uint QuantidadeEstoque { get; set; } = 0;
    public bool IsEmPromocao { get; set; } = false;

    public virtual decimal TaxaDesconto => 0.05m;
    public virtual decimal TaxaImposto => 0.07m;

    public virtual decimal CalcularImposto() => Preco * TaxaImposto;

    public virtual decimal AplicarDesconto() => (!IsEmPromocao) ? Preco : (Preco - Preco * TaxaDesconto);
}