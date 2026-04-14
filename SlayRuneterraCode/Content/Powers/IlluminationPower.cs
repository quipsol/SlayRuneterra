using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using SlayRuneterra.Content.Monsters;
using SlayRuneterra.Extensions;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Powers;


public class IlluminationPower : SlayRuneterraPowerModel
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    private bool _active = false;
    private decimal _latestAddedAmount = 0;
    
    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (dealer?.Monster is not Lux || !props.IsPoweredAttack())
                    return;
        if(result.UnblockedDamage > 0) // || !_active)
            await CreatureCmd.Damage(choiceContext, target, new DamageVar(this.Amount - _latestAddedAmount, ValueProp.Unpowered), dealer);
        await PowerCmd.Remove(this);
    }

    

    public override Task BeforePowerAmountChanged(PowerModel power, decimal amount, Creature target, Creature? applier, CardModel? cardSource)
    {
        // if (target != this.Owner || power is not IlluminationPower)
        //     return Task.CompletedTask;
        // _latestAddedAmount = amount;
        // _active = false;
        return Task.CompletedTask;
    }

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        // if (side == this.Owner.Side)
        // {
        //     if(_active)
        //         await PowerCmd.Apply(this);
        //     else
        //         _active = true;
        // }
    }
}