using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace SlayRuneterra.Utils.Extensions;

public static class PileTypeExtensions
{
    public static List<CardModel> GetCardsAsOrderedList(this PileType pileType, Player player)
    {
        return (from c in pileType.GetPile(player).Cards orderby c.Rarity, c.Id select c).ToList();
    }
}