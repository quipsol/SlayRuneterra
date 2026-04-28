using BaseLib.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;

namespace SlayRuneterra.Utils.Content;


// Rename if I keep this solution
public static class ModifyMaxUpgradeLevelHandler
{
    private static readonly SpireField<CardModel, int> CustomMaxUpgradeLevel = new(() => 0);
    /// <summary>
    /// Can not be lower than 0
    /// </summary>
    /// <param name="card"></param>
    /// <param name="addAmount"></param>
    public static void ModifyCustomMaxUpgradeLevel(CardModel card, int addAmount) =>
                CustomMaxUpgradeLevel.Set(card, Math.Max(0, CustomMaxUpgradeLevel.Get(card) + addAmount));
    public static int GetCustomMaxUpgradeLevel(CardModel card) => CustomMaxUpgradeLevel.Get(card);
    
    
    
    [HarmonyPatch(typeof(CardModel), nameof(CardModel.MaxUpgradeLevel), MethodType.Getter)]
    public static class MaxUpgradeLevelPatch
    {
        static void Postfix(CardModel __instance, ref int __result)
        {
            // This check prevents crashes but EVERY upgrade now shows '<name>+1' instead of '<name>+'
            // I would need a proper reference from the copy to the original to fix this
            __result = (__instance.RunState is null || __instance.Pile is null) ? Really.BigNumber : __result + GetCustomMaxUpgradeLevel(__instance);
        }
        
    }
}

