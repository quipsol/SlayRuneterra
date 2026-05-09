using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using SlayRuneterra.Content.Monsters;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Powers;


public class WaspEnragePower : SlayRuneterraPowerModel
{
    public override PowerType Type => PowerType.None;
    public override PowerStackType StackType => PowerStackType.Single;
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StrengthPower>(3), new DynamicVar("WNHealth", 100)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];

    private Creature? _waspNest;
    
    public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        _waspNest = CombatState.Enemies.FirstOrDefault(c => c.Monster is WaspNest);
        if (_waspNest == null)
        {
            await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(),Owner, DynamicVars.Strength.BaseValue, Owner, null);
            await PowerCmd.Remove(this);
        }
        else
        {
            DynamicVars["WNHealth"].BaseValue = _waspNest.MaxHp * 0.3m;
            if (_waspNest.CurrentHp > DynamicVars["WNHealth"].BaseValue) return;
            await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(),Owner, DynamicVars.Strength.BaseValue, Owner, null);
            await PowerCmd.Remove(this);
        }
    }

    public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
    {
        if (creature != _waspNest || wasRemovalPrevented)
            return;

        await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), Owner, DynamicVars.Strength.BaseValue, Owner, null);
        await PowerCmd.Remove(this);
    }

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target != _waspNest) return;
        if (target.CurrentHp > DynamicVars["WNHealth"].BaseValue) return;
        await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(),Owner, DynamicVars.Strength.BaseValue, Owner, null);
        await PowerCmd.Remove(this);
    }
}