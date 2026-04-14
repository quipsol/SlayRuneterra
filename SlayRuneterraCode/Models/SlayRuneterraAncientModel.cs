using BaseLib.Abstracts;
using BaseLib.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;

namespace SlayRuneterra.Models;


public abstract class SlayRuneterraAncientModel : CustomAncientModel
{
    public virtual string? CustomBackgroundScenePath => null;
}

[HarmonyPatch(typeof(EventModel), "get_BackgroundScenePath")]
class DemaciaAncientModelBackgroundScenePath
{
    [HarmonyPrefix]
    static bool UseAltTexture(EventModel __instance, ref string? __result)
    {
        MainFile.Logger.Info("============ Background Scene Path ============");
        if (__instance is not SlayRuneterraAncientModel model) return true;
        MainFile.Logger.Info("============ Background Scene Path ============");
        __result = model.CustomBackgroundScenePath;
        return false;
    }
}