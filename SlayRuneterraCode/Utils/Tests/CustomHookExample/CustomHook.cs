using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace SlayRuneterra.Utils.Tests.CustomHookExample;


// A way to iterate custom hooks (code by lamali). Example methods would need to be called where necessary

public class CustomHook
{
    // iterate over models
    private static async Task Dispatch<T>(ICombatState combatState, PlayerChoiceContext ctx, Func<T, Task> action) where T : class
    {
        foreach (var model in combatState.IterateHookListeners().OfType<T>())
        {
            var abstractModel = (AbstractModel)(object)model;
            ctx.PushModel(abstractModel);
            await action(model);
            ctx.PopModel(abstractModel);
        }
    }
    
    // have values changed by hooks
    private static TResult Aggregate<T, TResult>(ICombatState combatState, TResult seed, Func<T, TResult, TResult> action) where T : class
    {
        return combatState.IterateHookListeners().OfType<T>().Aggregate(seed, (current, model) => action(model, current));
    }
    
    private static TResult Aggregate<T, TResult>(IRunState runState, TResult seed, Func<T, TResult, TResult> action) where T : class
    {
        return runState.IterateHookListeners(null).OfType<T>().Aggregate(seed, (current, model) => action(model, current));
    }
    

    // have conditions decided by hooks
    private static bool Any<T>(ICombatState combatState, Func<T, bool> predicate) where T : class
    {
        return combatState.IterateHookListeners().OfType<T>().Any(predicate);
    }

// Examples
    public static int ModifyCollectorDoomDamage(ICombatState cs, Creature creature, int baseAmount)
    {
        return Aggregate<IModifyCollectorDoomDamage, int>(cs, baseAmount, (m, current) => m.ModifyCollectorDoomDamage(creature, current));
    }

    public static bool PreventDoomRemoval(ICombatState cs, Creature creature)
    {
        return Any<IPreventDoomRemoval>(cs, m => m.PreventDoomRemoval(creature));
    }

    public static Task OnConsume(ICombatState cs, PlayerChoiceContext ctx, Player player, int exampleParam)
    {
        return Dispatch<IOnConsume>(cs, ctx, m => m.OnConsume(ctx, player, exampleParam));
    }

}


/*

// YourModel.cs (card, relics, etc.)
public YourModel : YourAbstractModel, IOnConsume
{
    public async Task IOnConsume(PlayerChoiceContext ctx, Player player, other things you need)
    {
        // custom actions that shoul happen for this model when things are consumed        
        // i.e. gain block or smth like that
    }
}

// and call it when it consumes
await YourModdedHook.OnConsume(CombatState!, ctx, Owner, other things you need);

*/