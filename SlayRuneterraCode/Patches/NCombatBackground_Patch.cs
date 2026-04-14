using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Rooms;

namespace SlayRuneterra.Patches;


//[HarmonyPatch(typeof(NCombatBackground), nameof(NCombatBackground.Create))]
public class LogFoundBackgroundLayer_Patch
{
    public static bool Prefix(BackgroundAssets bg, ref NCombatBackground __result)
    {
        MainFile.Logger.Info($"Prefix SetLayers");
        MainFile.Logger.Info($" Foreground: {bg.FgLayer?.ToString() ?? "No layer"}");
        MainFile.Logger.Info($" Background layers: {bg.BgLayers.Count.ToString()}");
        foreach (var bgLayer in bg.BgLayers)
        {
            MainFile.Logger.Info(bgLayer.ToString());
        }
        return true;
    }
}
