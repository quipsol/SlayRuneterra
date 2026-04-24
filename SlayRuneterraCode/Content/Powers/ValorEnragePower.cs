using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using SlayRuneterra.Models;
using SlayRuneterra.Content.Monsters;

namespace SlayRuneterra.Content.Powers;


public class ValorEnragePower : SlayRuneterraPowerModel
{
    public override PowerType Type => PowerType.None;
    public override PowerStackType StackType => PowerStackType.Single;
    public override bool IsInstanced => true;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];

    public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
    {
        if (creature.Monster is not Quinn || wasRemovalPrevented)
            return;

        await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(),this.Owner, 3, this.Owner, null);
        await PowerCmd.Remove(this);
    }
}