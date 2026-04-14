using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;
using SlayRuneterra.Models;
using SlayRuneterra.Utils.CustomVars;

namespace SlayRuneterra.Content.Relics.Kayle;

[Pool(typeof(EventRelicPool))]
public class Judgement : SlayRuneterraRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VigorPower>()];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
                new CalculationBaseVar(0M),
                new CalculationExtraVar(1M),
                new CalculatedRelicVar("CalculatedVigor").WithMultiplier(relic => relic.Owner.Relics.Count)
    ];

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side != Owner.Creature.Side || combatState.RoundNumber != 1) return;
        await PowerCmd.Apply<VigorPower>(Owner.Creature, ((CalculatedRelicVar) DynamicVars["CalculatedVigor"]).Calculate(), Owner.Creature, null);
    }
    
}