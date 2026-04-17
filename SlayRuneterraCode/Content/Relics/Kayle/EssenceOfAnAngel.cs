using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Models.Relics;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Relics.Kayle;

[Pool(typeof(EventRelicPool))]
public class EssenceOfAnAngel : SlayRuneterraRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    protected override IEnumerable<IHoverTip> ExtraHoverTips  => HoverTipFactory.FromCardWithCardHoverTips<Apotheosis>();

    public override bool HasUponPickupEffect => true;

    public override async Task AfterObtained()
    {
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(Owner.RunState.CreateCard<Apotheosis>(Owner), PileType.Deck), 2f);
    }
}