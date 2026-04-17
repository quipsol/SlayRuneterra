using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;
using SlayRuneterra.Content.Powers;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Relics.Kayle;


[Pool(typeof(EventRelicPool))]
public class SwordOfJustice : SlayRuneterraRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;


    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<SwordOfJusticeStrengthPower>(4)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Type is not CardType.Attack)
            return;
        await PowerCmd.Apply<SwordOfJusticeStrengthPower>(Owner.Creature, DynamicVars[nameof(SwordOfJusticeStrengthPower)].BaseValue, Owner.Creature, null);
    }
}