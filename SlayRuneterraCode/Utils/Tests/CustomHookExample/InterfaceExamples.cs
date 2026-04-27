using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace SlayRuneterra.Utils.Tests.CustomHookExample;

public interface IOnConsume
{
    Task OnConsume(PlayerChoiceContext ctx, Player player, int exampleParam);
}

public interface IModifyCollectorDoomDamage
{
    int ModifyCollectorDoomDamage(Creature creature, int current);
}

public interface IPreventDoomRemoval
{
    bool PreventDoomRemoval(Creature creature);
}