using HarmonyLib;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;

namespace SlayRuneterra.Patches;


[HarmonyPatch(typeof(SceneHelper), nameof(SceneHelper.GetScenePath))]
public static class SceneHelper_GetScenePath_Patch
{
    static bool Prefix(string innerPath, ref string __result)
    {
        //MainFile.Logger.Warn("innerPath: " + innerPath);
        if (innerPath == "encounters/slayruneterra-jarvan_the_fourth_boss")
        {
            __result = "res://SlayRuneterra/scenes/encounters/jarvan_the_fourth_boss.tscn";
            return false; 
        }
        if (innerPath == "encounters/slayruneterra-lux_boss")
        {
            __result = "res://SlayRuneterra/scenes/encounters/lux_boss.tscn";
            return false; 
        }
        if (innerPath == "encounters/slayruneterra-quinn_elite")
        {
            __result = "res://SlayRuneterra/scenes/encounters/quinn_elite.tscn";
            return false; 
        }

        return true;
    }
}