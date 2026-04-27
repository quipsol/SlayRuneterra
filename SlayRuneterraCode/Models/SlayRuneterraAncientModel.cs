using BaseLib.Abstracts;
using BaseLib.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;

namespace SlayRuneterra.Models;


public abstract class SlayRuneterraAncientModel : CustomAncientModel
{
    // use CustomModel CustomScenePath ??
    public virtual string? CustomBackgroundScenePath => null;
}

[HarmonyPatch(typeof(EventModel), "get_BackgroundScenePath")]
class DemaciaAncientModelBackgroundScenePath
{
    [HarmonyPrefix]
    static bool UseAltTexture(EventModel __instance, ref string? __result)
    {
        if (__instance is not SlayRuneterraAncientModel model) return true;
        __result = model.CustomBackgroundScenePath;
        return false;
    }
}