using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.CardRewardAlternatives;
using MegaCrit.Sts2.Core.Entities.Rewards;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Timeline;
using SlayRuneterra.Utils.Epochs;

namespace SlayRuneterra.Models;

public abstract class CustomEpochModel : EpochModel
{
    public abstract string CustomBigPortraitPath { get; } // png
    public abstract string CustomPackedPortraitPath { get; } // tres
    
    
    [HarmonyPatch(typeof(EpochModel), nameof(EpochModel.BigPortraitPath), MethodType.Getter)]
    class CustomEpochBigPortraitPath
    {
        [HarmonyPrefix]
        static bool UseAltTexture(EpochModel __instance, ref string? __result)
        {
            if (__instance is not CustomEpochModel customEpoch) return true;
            __result = customEpoch.CustomBigPortraitPath;
            return false;
        }
    }
    
    [HarmonyPatch(typeof(EpochModel), nameof(EpochModel.PackedPortraitPath), MethodType.Getter)]
    class CustomEpochPackedPortraitPath
    {
        [HarmonyPrefix]
        static bool UseAltTexture(EpochModel __instance, ref string? __result)
        {
            if (__instance is not CustomEpochModel customEpoch) return true;
            __result = customEpoch.CustomPackedPortraitPath;
            return false;
        }
    }
}