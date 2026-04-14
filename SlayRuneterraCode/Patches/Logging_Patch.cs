using BaseLib.Abstracts;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;

namespace SlayRuneterra.Patches;


//TODO: Combat room loading crashed without this????
[HarmonyPatch(typeof(ActModel), "GenerateBackgroundAssets")]
public class ActModel_GenerateBackgroundAssets
{
    [HarmonyPrefix]
    public static bool Prefix(ActModel __instance)
    {
        MainFile.Logger.Debug("ActModel GenerateBackgroundAssets Prefix ???");
        return true;
    }
}

//TODO: Combat room loading crashes without this patch????
// like, even removing the log statement makes it crash?
// I need help
[HarmonyPatch(typeof(EncounterModel), "GetBackgroundAssets")]
public class EncounterModel_GetBackgroundAssets
{
    [HarmonyPrefix]
    public static void Prefix(EncounterModel __instance)
    {
        MainFile.Logger.Debug("EncounterModel GetBackgroundAssets Prefix ???");
    }
}