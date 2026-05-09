using MegaCrit.Sts2.Core.Models;

namespace SlayRuneterra.Utils.Extensions;

public static class CardModelExtensions
{
    public static List<CardModel> Ordered(this IReadOnlyList<CardModel> cards)
    {
        return cards.OrderBy(card => card.Rarity).ThenBy(card => card.Id).ToList();
    }   
}