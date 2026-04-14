using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;
using SlayRuneterra.Extensions;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Powers;



public class DefinitelyNotFlutterPower : SlayRuneterraPowerModel
{
    private const string DAMAGE_DECREASE = "DamageDecrease";

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override bool ShouldScaleInMultiplayer => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar(DAMAGE_DECREASE, 50M)];

    public override Decimal ModifyDamageMultiplicative(Creature? target, Decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        return target != this.Owner || !props.IsPoweredAttack() ? 1M : this.DynamicVars[DAMAGE_DECREASE].BaseValue / 100M;
    }

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        DefinitelyNotFlutterPower power = this;
        if (target != power.Owner || result.UnblockedDamage == 0 || !props.IsPoweredAttack())
            return;
        await PowerCmd.Decrement((PowerModel) power);
        if (power.Amount > 0)
            return;
        await CreatureCmd.TriggerAnim(power.Owner, "StunTrigger", 0.6f);
        string nextState = power.Owner.Monster!.MoveStateMachine!.StateLog.Last<MonsterState>().GetNextState(power.Owner, power.Owner.Monster.RunRng.MonsterAi);
        await CreatureCmd.Stun(power.Owner, new Func<IReadOnlyList<Creature>, Task>(power.StunnedMove), nextState);
        SfxCmd.StopLoop("event:/sfx/enemy/enemy_attacks/thieving_hopper/thieving_hopper_hover_loop");
        power.Flash();
        await Cmd.Wait(0.25f);
    }

    private Task StunnedMove(IReadOnlyList<Creature> targets) => Task.CompletedTask;
}