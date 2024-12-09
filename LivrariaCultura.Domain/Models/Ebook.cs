using LivrariaCultura.Domain.Enums;

namespace LivrariaCultura.Domain.Models;

public class Ebook : Livro
{
    public FormatoEbook Formato { get; set; } = FormatoEbook.Mobi;

    public override decimal AplicarDesconto()
    {
        if (Formato == FormatoEbook.Pdf)
        {
            return Preco - Preco * .20m;
        }

        return base.AplicarDesconto();
    }
}