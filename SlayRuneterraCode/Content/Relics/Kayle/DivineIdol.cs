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
public class DivineIdol : SlayRuneterraRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    protected override IEnumerable<IHoverTip> ExtraHoverTips  => 
                
                IsCanonical ? HoverTipFactory.FromCardWithCardHoverTips<DivineIntervention>().Concat(HoverTipFactory.FromCardWithCardHoverTips<GreaterDivineIntervention>())
                            : Owner is not null  &&  Owner.RunState.Players.Count > 1 ?
                HoverTipFactory.FromCardWithCardHoverTips<GreaterDivineIntervention>() :
                HoverTipFactory.FromCardWithCardHoverTips<DivineIntervention>();

    public override bool HasUponPickupEffect => true;

    public override async Task AfterObtained()
    {
        if(Owner.RunState.Players.Count > 1)
            CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(Owner.RunState.CreateCard<GreaterDivineIntervention>(Owner), PileType.Deck), 2f);
        else
            CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(Owner.RunState.CreateCard<DivineIntervention>(Owner), PileType.Deck), 2f);
    }
}