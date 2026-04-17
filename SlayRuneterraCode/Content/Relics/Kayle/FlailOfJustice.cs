using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.ValueProps;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Relics.Kayle;

[Pool(typeof(EventRelicPool))]
public class FlailOfJustice : SlayRuneterraRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<WeakPower>(2), new PowerVar<VulnerablePower>(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<WeakPower>(), HoverTipFactory.FromPower<VulnerablePower>()];

    public override async Task AfterBlockBroken(Creature creature)
    {
        if (!creature.IsMonster || creature.IsDead)
            return;
        await PowerCmd.Apply<WeakPower>(creature, DynamicVars[nameof(WeakPower)].BaseValue, Owner.Creature, null);
        await PowerCmd.Apply<VulnerablePower>(creature, DynamicVars[nameof(VulnerablePower)].BaseValue, Owner.Creature, null);
    }
}