using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Relics.Kayle;

[Pool(typeof(EventRelicPool))]
public class DivineHelmet : SlayRuneterraRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new ("DamageCap", 5)];

    public override Decimal ModifyHpLostAfterOsty(Creature target, Decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        return CombatManager.Instance.IsInProgress && target == this.Owner.Creature ? Math.Min(DynamicVars["DamageCap"].BaseValue, amount) : amount;
    }

    public override Task AfterModifyingHpLostAfterOsty()
    {
        Flash();
        return Task.CompletedTask;
    }

    // This caps the damage the enemies do, even before block
    // public override Decimal ModifyDamageCap(Creature? target, ValueProp props, Creature? dealer, CardModel? cardSource)
    // {
    //     return target != Owner.Creature ? Decimal.MaxValue : DynamicVars["DamageCap"].BaseValue;
    // }

    public override Task AfterModifyingDamageAmount(CardModel? cardSource)
    {
        this.Flash();
        return Task.CompletedTask;
    }
}