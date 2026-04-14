using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using SlayRuneterra.Content.Monsters;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Powers;


public class SummonGalioPower : SlayRuneterraPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target.Monster is not Lux || target.CurrentHp > this.Amount)
            return;
        (target.Monster as Lux)!.SetStateToBeaconOfHope();
        await PowerCmd.Remove(this);
    }
}