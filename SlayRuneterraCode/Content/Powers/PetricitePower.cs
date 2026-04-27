using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Powers;

public class PetricitePower : SlayRuneterraPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override decimal ModifyPowerAmountGiven(PowerModel power, Creature giver, decimal amount, Creature? target, CardModel? cardSource)
    {
        return amount - (target == Owner && power.Type == PowerType.Debuff ? Math.Min(Amount, amount) : 0m);
    }

    public override async Task AfterModifyingPowerAmountGiven(PowerModel power)
    {
        await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), this.Owner, Amount, this.Owner, null);
    }


}