using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.RelicPools;
using SlayRuneterra.Content.Cards.Ancient;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Relics.Kayle;


[Pool(typeof(EventRelicPool))]
public class DivineFeather : SlayRuneterraRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    protected override IEnumerable<IHoverTip> ExtraHoverTips  => HoverTipFactory.FromCardWithCardHoverTips<CelestialBlessing>();

    public override bool HasUponPickupEffect => true;

    public override async Task AfterObtained()
    {
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(Owner.RunState.CreateCard<CelestialBlessing>(Owner), PileType.Deck), 2f);
    }
}