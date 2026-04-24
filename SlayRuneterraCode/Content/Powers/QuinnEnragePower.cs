using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using SlayRuneterra.Content.Monsters;
using SlayRuneterra.Models;

namespace SlayRuneterra.Content.Powers;

public class QuinnEnragePower : SlayRuneterraPowerModel
{
    public override PowerType Type => PowerType.None;
    public override PowerStackType StackType => PowerStackType.Single;
    public override bool IsInstanced => true;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<RitualPower>(), HoverTipFactory.FromPower<StrengthPower>()];
    
    public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
    {
        if (creature.Monster is not Valor || wasRemovalPrevented)
            return;

        var p = await PowerCmd.Apply<RitualPower>(choiceContext,this.Owner, 2, this.Owner, null);
        await p!.AfterTurnEnd(new ThrowingPlayerChoiceContext(), CombatSide.Enemy); // Give me a proper "deactivate this thing" MegaCrit, so I don't have to do this shit
        await PowerCmd.Remove(this);
    }
}