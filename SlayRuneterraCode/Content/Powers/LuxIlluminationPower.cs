using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Powers;

public class LuxIlluminationPower : SlayRuneterraPowerModel
{
    public override PowerType Type => PowerType.None;
    public override PowerStackType StackType => PowerStackType.Counter;

    
    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props, Creature target, CardModel? cardSource)
    {
        // if (dealer != this.Owner || dealer == target || target.IsDead)
        //     return;
        // await PowerCmd.Apply<IlluminationPower>(target, this.Amount, dealer, null);
    }
}