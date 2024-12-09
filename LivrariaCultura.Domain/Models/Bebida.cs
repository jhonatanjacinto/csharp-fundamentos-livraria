using LivrariaCultura.Domain.Enums;

namespace LivrariaCultura.Domain.Models;

public class Bebida : Produto
{
    public TipoBebida Tipo { get; set; } = TipoBebida.Quente;
    public override decimal TaxaDesconto => 0.15m;
    public override decimal TaxaImposto => 0.08m;
}