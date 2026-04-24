using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using SlayRuneterra.Models;

namespace SlayRuneterra.Patches;



// moving this patch out of CustomActModel and into this file makes it run?
//[HarmonyPatch(typeof(ActModel), nameof(ActModel.GenerateBackgroundAssets))]
// public class CustomActGenerateBackgroundAssets
// {
//     [HarmonyPrefix]
//     public static bool Prefix(ActModel __instance, Rng rng, ref BackgroundAssets __result)
//     {
//         //MainFile.Logger.Warn("GenerateBackgroundAssets - Prefix");
//         if (__instance is not CustomActModel customActModel)
//             return true;
//         var backgroundAsset = new BackgroundAssets("hive", Rng.Chaotic);
//         __result = OverwriteFieldsWithActualValues(customActModel, rng, backgroundAsset);
//         return false;
//     }
//     
//     private static BackgroundAssets OverwriteFieldsWithActualValues(CustomActModel customActModel, Rng rng, BackgroundAssets backgroundAssets)
//     {
//      
//         var bgSceneField = AccessTools.Field(backgroundAssets.GetType(), "<BackgroundScenePath>k__BackingField");
//         var bgLayersField = AccessTools.Field(backgroundAssets.GetType(), "<BgLayers>k__BackingField");
//         var fgLayerField = AccessTools.Field(backgroundAssets.GetType(), "<FgLayer>k__BackingField");
//
//         (string? ScenePath, List<string> BackgroundLayerPaths, string? ForegroundLayerPath) customBackgroundAssets = customActModel.GetBackgroundAssetPaths(rng);
//         
//         //TODO: Option to build scene dynamically. It will read the Lists length and create the node with all the Layers it needs
//         // This can't be done in here though but should be done in where ever the scene path is used
//         
//         bgSceneField.SetValue(backgroundAssets, customBackgroundAssets.ScenePath);
//         bgLayersField.SetValue(backgroundAssets, customBackgroundAssets.BackgroundLayerPaths);
//         fgLayerField.SetValue(backgroundAssets, customBackgroundAssets.ForegroundLayerPath);
//         
//         return backgroundAssets;
//     }
// }




// help
//[HarmonyPatch(typeof(ActModel), nameof(ActModel.GenerateBackgroundAssets))]
public class ActModel_GenerateBackgroundAssets
{
    [HarmonyPrefix]
    public static void Prefix(ActModel __instance)
    {
        MainFile.Logger.Warn("ActModel GenerateBackgroundAssets Prefix ???");
    }
}

// I need help
//[HarmonyPatch(typeof(EncounterModel), "GetBackgroundAssets")]
public class EncounterModel_GetBackgroundAssets
{
    [HarmonyPrefix]
    public static void Prefix(EncounterModel __instance)
    {
        MainFile.Logger.Warn("EncounterModel GetBackgroundAssets Prefix ???");
    }
}

[HarmonyPatch(typeof(BackgroundAssets))]
[HarmonyPatch(MethodType.Constructor)]
[HarmonyPatch(new Type[] { typeof(string), typeof(Rng) })]
public class BackgroundAssets_Constructor
{
    
    [HarmonyPrefix]
    public static bool Prefix(BackgroundAssets __instance, ref string title, Rng rng)
    {
        MainFile.Logger.Warn("BackgroundAssets Constructor Prefix");
        MainFile.Logger.Info(title);
        if (title.Contains("slayruneterra"))
        {
            MainFile.Logger.Warn("Tried loading SlayRuneterra background assets!");
            //title = "hive";
        }
        return true;
    }
}

[HarmonyPatch(typeof(EncounterModel), "GetBackgroundAssets")]
static class GetCustomBackgroundAssets {
    [HarmonyPrefix]
    static void Custom(EncounterModel __instance, ActModel parentAct, Rng rng) {
        MainFile.Logger.Warn("EncounterModel GetBackgroundAssets Prefix");
        MainFile.Logger.Warn($"Type: {__instance.GetType().FullName}");
        var prop = AccessTools.Property(__instance.GetType(), "HasCustomBackground");

        var before = prop?.GetValue(__instance);
        MainFile.Logger.Warn($"Before: {before}");

    }
}