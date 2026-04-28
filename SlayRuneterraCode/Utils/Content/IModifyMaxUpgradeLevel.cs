using BaseLib.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using Microsoft.VisualBasic.FileIO;
using SlayRuneterra.Models;
using SlayRuneterra.Utils.Tests.CustomHookExample;

namespace SlayRuneterra.Utils.Content;

public interface IModifyMaxUpgradeLevel
{
    int ModifyMaxUpgradeLevelHook(CardModel card, int amount) => amount; // Default implementation
}

public static class ModifyMaxUpgradeLevelHandler
{
    private static readonly SpireField<CardModel, int> CustomMaxUpgradeLevel = new(() => 0);

    public static void ModifyCustomMaxUpgradeLevel(CardModel card, int addAmount)
    {
        int oldAmount = CustomMaxUpgradeLevel.Get(card);
        CustomMaxUpgradeLevel.Set(card, Math.Max(0, oldAmount + addAmount));
    }

    public static int GetCustomMaxUpgradeLevel(CardModel card)
    {
        return  CustomMaxUpgradeLevel.Get(card);
    }
    
    
    [HarmonyPatch(typeof(CardModel), nameof(CardModel.MaxUpgradeLevel), MethodType.Getter)]
    public static class MaxUpgradeLevelPatch
    {
        static void Postfix(CardModel __instance, ref int __result)
        {
            if (__instance.RunState is null || __instance.Pile is null)
            {
                __result = Really.BigNumber;
                //MainFile.Logger.Info("runState or Pile is null");
                return;
            }
            
            __result += CustomMaxUpgradeLevel.Get(__instance);

            return;
            // The game creates a copy of a card to use for the displayed upgraded card.
            // However, I couldn't find a path of reference from this new card to the original.
            // What is the case though is that this new card does not have a Pile object.
            // If I knew how to get the original card from this copy,
            // I could make it so the copy Aggregates the modifications from the original.
            // Instead, I give it big upgradeability value and assume it will be discarded eventually anyway
            // This creates a small visual bug when upgrading cards that I'm not going to fix now.
            //
            // The runstate is null when loading a run in the main menu
            //MainFile.Logger.Info($"Getting MaxUpgradeLevel for card {__instance}");
            if (__instance.RunState is null || __instance.Pile is null)
            {
                __result = Really.BigNumber;
                //MainFile.Logger.Info("runState or Pile is null");
                return;
            }
            if(__instance.CombatState is not null)
                __result = CustomHook.ModifyCardMaxUpgradeLevel(__instance.CombatState, __instance, __result);
            else
                __result = CustomHook.ModifyCardMaxUpgradeLevel(__instance.RunState, __instance, __result);
            //MainFile.Logger.Info($"Max upgrade level changed to {__result}");
        }
        
    }
}

