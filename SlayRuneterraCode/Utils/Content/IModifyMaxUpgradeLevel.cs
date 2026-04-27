using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using SlayRuneterra.Models;
using SlayRuneterra.Utils.Tests.CustomHookExample;

namespace SlayRuneterra.Utils.Content;

public interface IModifyMaxUpgradeLevel
{
    int ModifyMaxUpgradeLevel(int amount) => amount; // Default implementation
    int ModifyMaxUpgradeLevelHook(CardModel card, int amount) => amount; // Default implementation
}

public static class ModifyMaxUpgradeLevelHandler
{
    private static readonly Dictionary<CardModel, List<IModifyMaxUpgradeLevel>> CardModels = new();

    public static void ModifyMaxUpgradeLevelOfCard(this IModifyMaxUpgradeLevel modificationSource, CardModel card)
    {
        MainFile.Logger.Warn("Added Card Upgrade Level Modification");
        if(CardModels.TryGetValue(card, out var value))
            value.Add(modificationSource);
        else
            CardModels.Add(card, [modificationSource]);
    }
    
    [HarmonyPatch(typeof(CardModel), nameof(CardModel.MaxUpgradeLevel), MethodType.Getter)]
    public static class MaxUpgradeLevelPatch
    {
        static void Postfix(CardModel __instance, ref int __result)
        {
            if (__instance.RunState is null)
            {
                MainFile.Logger.Info($"Runstate of Model {__instance} is null");
                return;
            }
            __result = CustomHook.ModifyCardMaxUpgradeLevel(__instance.RunState, __instance, __result);

            return;
            
            
            // The game creates a copy of a card to use for the displayed upgraded card.
            // However, I couldn't find a reference from this new card to the original.
            // What is the case though is that this new card does not have a Pile object.
            // If I knew how to get the original card from this copy, I could make it so the copy Aggregates the modifiers from the original
            if (__instance.Pile is null)
            {
                __result = 99;
                return;
            }
            
            MainFile.Logger.Info($"MaxUpgradeLevel of {__instance} before modification  {__result}");
            foreach(var card in CardModels)
                MainFile.Logger.Info($"Card {card.Key} inside the Dictionary");
            if (CardModels.TryGetValue(__instance, out var cardModel))
            {
                __result = Math.Max(0, cardModel.Aggregate(__result, (current, modifyMaxUpgradeLevel) => modifyMaxUpgradeLevel.ModifyMaxUpgradeLevel(current)));
            }
            MainFile.Logger.Info($"MaxUpgradeLevel of {__instance} has been modified to {__result}");
        }
        
    }
}

