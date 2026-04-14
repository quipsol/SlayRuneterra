using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using SlayRuneterra.Models;

namespace SlayRuneterra.Utils;

public static class CustomHooks
{
    public static void BeforeCardDrawnSync(CombatState combatState, CardModel card, PileType targetPileType, bool fromHandDraw)
    {
        //MainFile.Logger.Warn("=======  BeforeCardDrawnSync  ========");
        // This seems to have had an issue where if one of the awaits on BeforeCardDrawn takes too long the game crashes
        // Task.Run(async () =>
        // {
        //     await CustomHooks.BeforeCardDrawn(combatState, card,targetPileType, fromHandDraw);
        // });
        // return;
        //
        // CustomHooks.BeforeCardDrawn(combatState, card, targetPileType, fromHandDraw)
        //             .GetAwaiter()
        //             .GetResult();
    }
    
    // public static async Task BeforeCardDrawn(CombatState combatState, CardModel card, PileType targetPileType, bool fromHandDraw)
    // {
    //     foreach (AbstractModel model in combatState.IterateHookListeners())
    //     {
    //         if(model is SlayRuneterraPowerModel slayRuneterraPowerModel)
    //         {
    //             await slayRuneterraPowerModel.BeforeCardDrawnEarly(card, targetPileType, fromHandDraw);
    //             model.InvokeExecutionFinished();
    //         }
    //         
    //     }
    //     foreach (AbstractModel model in combatState.IterateHookListeners())
    //     {
    //         if(model is SlayRuneterraPowerModel slayRuneterraPowerModel)
    //         {
    //             await slayRuneterraPowerModel.BeforeCardDrawn(card, targetPileType, fromHandDraw);
    //             model.InvokeExecutionFinished();
    //         }
    //     }
    // }
    
    public static void BeforeCardDrawn(CombatState combatState, CardModel card, PileType targetPileType, bool fromHandDraw)
    {
        foreach (AbstractModel model in combatState.IterateHookListeners())
        {
            if(model is SlayRuneterraPowerModel slayRuneterraPowerModel)
            {
                slayRuneterraPowerModel.BeforeCardDrawnEarly(card, targetPileType, fromHandDraw);
                model.InvokeExecutionFinished();
            }
            
        }
        foreach (AbstractModel model in combatState.IterateHookListeners())
        {
            if(model is SlayRuneterraPowerModel slayRuneterraPowerModel)
            {
                slayRuneterraPowerModel.BeforeCardDrawn(card, targetPileType, fromHandDraw);
                model.InvokeExecutionFinished();
            }
        }
    }
    
}