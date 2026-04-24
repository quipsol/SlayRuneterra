using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using SlayRuneterra.Models;

namespace SlayRuneterra.Patches;

[HarmonyPatch(typeof(ActModel), nameof(ActModel.GenerateBackgroundAssets))]
public class CustomActGenerateBackgroundAssetsTwo
{
    [HarmonyPrefix]
    public static bool Prefix(ActModel __instance, Rng rng, ref BackgroundAssets __result)
    {
        MainFile.Logger.Warn("Generate BackgroundAssets Prefix");
        if (__instance is not CustomActModel customAct) return true;
        __result = customAct.CustomGenerateBackgroundAssets(__instance, rng);
        return false;
    }
}
