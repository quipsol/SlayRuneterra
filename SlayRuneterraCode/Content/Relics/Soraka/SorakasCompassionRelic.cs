using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.RelicPools;
using SlayRuneterra.Content.Cards.Ancient;
using SlayRuneterra.Content.Enchantments;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Relics.Soraka;

[Pool(typeof(EventRelicPool))]
public class SorakasCompassionRelic() : SlayRuneterraRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    public override bool HasUponPickupEffect => true;
    protected override IEnumerable<IHoverTip> ExtraHoverTips  => HoverTipFactory.FromCardWithCardHoverTips<SorakasCompassion>().Concat(HoverTipFactory.FromEnchantment<Potential>());

    public override async Task AfterObtained()
    {
        var res = await CardPileCmd.Add(Owner.RunState.CreateCard<SorakasCompassion>(Owner), PileType.Deck);
        CardCmd.Enchant<Potential>(res.cardAdded, 1);
        CardCmd.PreviewCardPileAdd(res, 2f);
    }
    
}