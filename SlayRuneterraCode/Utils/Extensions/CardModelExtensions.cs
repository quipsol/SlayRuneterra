using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace SlayRuneterra.Extensions;

public static class CardModelExtensions
{
    public static List<CardModel> Ordered(this IReadOnlyList<CardModel> cards)
    {
        return cards.OrderBy(card => card.Rarity).ThenBy(card => card.Id).ToList();
    }   
}

// return (from c in pileType.GetPile(player).Cards orderby c.Rarity, c.Id select c).ToList();