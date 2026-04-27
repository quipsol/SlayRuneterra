using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using SlayRuneterra.Models;

namespace SlayRuneterra.Patches;

// help
[HarmonyPatch(typeof(ActModel), nameof(ActModel.GenerateBackgroundAssets))]
 public class ActModel_GenerateBackgroundAssets
 {
     [HarmonyPrefix]
     public static void Prefix(ActModel __instance)
     {
         MainFile.Logger.Warn("ActModel GenerateBackgroundAssets Prefix ???");
     }
 }

// I need help
[HarmonyPatch(typeof(EncounterModel), "GetBackgroundAssets")]
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