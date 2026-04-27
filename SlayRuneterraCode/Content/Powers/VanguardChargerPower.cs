using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using SlayRuneterra.Content.Monsters;
using SlayRuneterra.Extensions;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Powers;


public class VanguardChargerPower : SlayRuneterraPowerModel
{
    private const string VANGUARD = "vanguard"; 
    private const string JARVAN = "jarvan"; 
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StrengthPower>(1), new BoolVar("HasJarvanAlly", false),
    new IfUpgradedVar("HasJarvanAllyyy", 0)];

    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        if (Owner.CombatState!.Enemies.Any(creature => creature.Monster is JarvanTheFourth))
            (DynamicVars["HasJarvanAlly"] as BoolVar)!.BoolVal = true;
        //     (DynamicVars["HasJarvanAlly"] as IfUpgradedVar)!.upgradeDisplay = UpgradeDisplay.Upgraded;
        // else
        //     (DynamicVars["HasJarvanAlly"] as IfUpgradedVar)!.upgradeDisplay = UpgradeDisplay.Normal;
        
        return Task.CompletedTask;
    }

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if ( !props.IsPoweredAttack() || 
             (!target.ModelId.Entry.Contains(VANGUARD, StringComparison.InvariantCultureIgnoreCase) &&
            !target.ModelId.Entry.Contains(JARVAN, StringComparison.InvariantCultureIgnoreCase))
            || result.UnblockedDamage <= 0 || target == this.Owner)
            return;
        await PowerCmd.Apply<StrengthPower>(choiceContext, this.Owner, this.Amount, this.Owner, null);
    }

    public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
    {
        if (wasRemovalPrevented)
            return;
        if (!creature.ModelId.Entry.Contains(VANGUARD, StringComparison.InvariantCultureIgnoreCase) &&
            !creature.ModelId.Entry.Contains(JARVAN, StringComparison.InvariantCultureIgnoreCase))
            return;
        await PowerCmd.Apply<StrengthPower>(choiceContext, this.Owner, this.Amount, this.Owner, null);
    }
}