using LivrariaCultura.Domain.Enums;

namespace LivrariaCultura.Domain.Models;

public class Papelaria : Produto
{
    public string Marca { get; set; } = string.Empty;
    public TipoPapelaria TipoPapelaria { get; set; } = TipoPapelaria.Lapis;
}