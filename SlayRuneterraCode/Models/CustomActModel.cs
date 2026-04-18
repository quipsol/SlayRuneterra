using System.Reflection;
using System.Runtime.Serialization;
using BaseLib.Abstracts;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using SlayRuneterra.Utils;
using SlayRuneterra.Utils.HookInterface;

namespace SlayRuneterra.Models;

public abstract class CustomActModel : ActModel, ICustomModel, IHookReceiver
{
    public override bool ShouldReceiveCombatHooks => true;

    public virtual string CustomBackgroundScenePath => $"res://{MainFile.ModId}/scenes/backgrounds/{Id.Entry.ToLowerInvariant()}/{Id.Entry.ToLowerInvariant()}_background.tscn";
    public virtual string CustomMapTopBgPath => $"res://{MainFile.ModId}/images/packed/map/map_bgs/{Id.Entry.ToLowerInvariant()}/map_bottom_{Id.Entry.ToLowerInvariant()}.png";
    public virtual string CustomMapMidBgPath => $"res://{MainFile.ModId}/images/packed/map/map_bgs/{Id.Entry.ToLowerInvariant()}/map_middle_{Id.Entry.ToLowerInvariant()}.png";
    public virtual string CustomMapBotBgPath => $"res://{MainFile.ModId}/images/packed/map/map_bgs/{Id.Entry.ToLowerInvariant()}/map_top_{Id.Entry.ToLowerInvariant()}.png";
    public virtual string CustomRestSiteBackgroundPath => $"res://{MainFile.ModId}/scenes/rest_site/{Id.Entry.ToLowerInvariant()}_rest_site.tscn";

    // ChestSpineResourcePath
    public virtual string? ChestAtlasPath => null;
    
    public virtual List<string> ForegroundLayerPaths => [];
    public virtual List<List<string>> BackgroundLayerPaths => [];
    
    // game only ever has one scene file for an act so this method is currently useless
    // might change behaviour to allow for multiple scenes or even create a scene dynamically
    public virtual string GetBackgroundAssetScenePath(Rng rng) => CustomBackgroundScenePath;
    public virtual string? GetBackgroundAssetFgLayerPath(Rng rng) => rng.NextItem(ForegroundLayerPaths);
    public virtual List<string> GetBackgroundAssetBgLayerPaths(Rng rng) 
    {
        List<string> list = [];
        foreach (var layerPaths in BackgroundLayerPaths)
        {
            list.Add(rng.NextItem(layerPaths) ?? "");
        }
        return list;
    }

    /// <returns>Tuple of (ScenePath, BackgroundLayerPaths, ForegroundLayerPath)</returns>
    public virtual (string, List<string>, string?) GetBackgroundAssetPaths(Rng rng)
    {
        return (GetBackgroundAssetScenePath(rng), GetBackgroundAssetBgLayerPaths(rng), GetBackgroundAssetFgLayerPath(rng));
    }
    
    
}


[HarmonyPatch(typeof(ActModel), nameof(ActModel.BackgroundScenePath), MethodType.Getter)]
class CustomActBackgroundScenePath
{
    [HarmonyPrefix]
    static bool UseAltTexture(ActModel __instance, ref string? __result)
    {
        if (__instance is not CustomActModel customAct) return true;
        __result = customAct.CustomBackgroundScenePath;
        return false;
    }
}
[HarmonyPatch(typeof(ActModel), nameof(ActModel.MapTopBgPath), MethodType.Getter)]
class CustomActMapTopBgPath
{
    [HarmonyPrefix]
    static bool UseAltTexture(ActModel __instance, ref string? __result)
    {
        if (__instance is not CustomActModel customAct) return true;
        __result = customAct.CustomMapTopBgPath;
        return false;
    }
}

[HarmonyPatch(typeof(ActModel), nameof(ActModel.MapMidBgPath), MethodType.Getter)]
class CustomActMapMidBgPath
{
    [HarmonyPrefix]
    static bool UseAltTexture(ActModel __instance, ref string? __result)
    {
        if (__instance is not CustomActModel customAct) return true;
        __result = customAct.CustomMapMidBgPath;
        return false;
    }
}

[HarmonyPatch(typeof(ActModel), nameof(ActModel.MapBotBgPath), MethodType.Getter)]
class CustomActMapBotBgPath
{
    [HarmonyPrefix]
    static bool UseAltTexture(ActModel __instance, ref string? __result)
    {
        if (__instance is not CustomActModel customAct) return true;
        __result = customAct.CustomMapBotBgPath;
        return false;
    }
}

[HarmonyPatch(typeof(ActModel), nameof(ActModel.RestSiteBackgroundPath), MethodType.Getter)]
class CustomActRestSiteBackgroundPath
{
    [HarmonyPrefix]
    static bool UseAltTexture(ActModel __instance, ref string? __result)
    {
        if (__instance is not CustomActModel customAct) return true;
        __result = customAct.CustomRestSiteBackgroundPath;
        return false;
    }
}