using BaseLib.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using SlayRuneterra.Content.Enchantments;

namespace SlayRuneterra.Utils.Content;

/*

REFLECTIONS event resets card back to upgrade level 0... AND it reapplied enchantment so it now can be upgraded 5 times
However, reloading the run clears the SpirePatch field and properly re enchants to the max upgrade of 3. But this can force crash runs.
Severity: Medium - Can brick a run if someone upgrades over the intended MaxUpgadeLevel but neeeds specific events and investments to happen.
priority: Very Low - Enchantment not intended to ever appear outside of a SlayRuneterrra run which has 100% modded events.


 */


// Rename if I keep this solution
public static class ModifyMaxUpgradeLevelHandler
{
    private static readonly SpireField<CardModel, int> CustomMaxUpgradeLevel = new(() => 0);
    public static void ModifyCustomMaxUpgradeLevel(CardModel card, int addAmount) =>
                CustomMaxUpgradeLevel.Set(card, Math.Max(0, CustomMaxUpgradeLevel.Get(card) + addAmount));
    public static int GetCustomMaxUpgradeLevel(CardModel card) => CustomMaxUpgradeLevel.Get(card);
    
    [HarmonyPatch(typeof(CardModel), nameof(CardModel.MaxUpgradeLevel), MethodType.Getter)]
    public static class MaxUpgradeLevelPatch
    {
        private static void Postfix(CardModel __instance, ref int __result)
        {
            // This check prevents crashes but EVERY upgrade now shows '<name>+1' instead of '<name>+'
            // I would need a proper reference from the copy to the original to fix this

            __result = (__instance.RunState is null || __instance.Pile is null) ? Really.BigNumber : Math.Max(__instance.CurrentUpgradeLevel, __result + GetCustomMaxUpgradeLevel(__instance));
        }
        
    }
    
    [HarmonyPatch(typeof(NMainMenu), nameof(NMainMenu._Ready))]
    public static class MainMenuSpireFieldCheckLogic
    {
        [HarmonyPrefix]
        public static void Prefix(NMainMenu __instance)
        {
            MainFile.Logger.Warn($"SpireFiled: {CustomMaxUpgradeLevel.ToString()}");
        }
    }
}

