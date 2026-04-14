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
    
    public virtual string? CustomBackgroundScenePath => null; // tscn
    public virtual string? CustomMapTopBgPath => null; // png
    public virtual string? CustomMapMidBgPath => null; // png
    public virtual string? CustomMapBotBgPath => null; // png
    public virtual string? CustomRestSiteBackgroundPath => null; // tscn

    public virtual Dictionary<string, List<string?>> LayerPaths => new();
    
    
    // This is a method instead of a property to allow the use of the rng reference in overrides
    // TODO: Remove the rng reference? This is purely cosmetic, prefer to use Rng.Chaotic!?
    public virtual string? GetBackgroundAssetScenePath(Rng rng) { return CustomBackgroundScenePath; }
    public virtual List<string> GetBackgroundAssetBgLayerPaths(Rng rng) 
    {
        List<string> list = new List<string>();
        list.Add(rng.NextItem(LayerPaths.GetValueOrDefault("background") ?? [""]) ?? "");
        foreach (var layerPath in LayerPaths)
        {
            if (layerPath.Key is "background" or "foreground")
                continue;
            list.Add(rng.NextItem(layerPath.Value) ?? "");
        }
        return list;
    }
    public virtual string? GetBackgroundAssetFgLayerPath(Rng rng) { return rng.NextItem(LayerPaths.GetValueOrDefault("foreground") ?? [""]) ?? ""; }
    
    
    
}


[HarmonyPatch(typeof(ActModel), nameof(ActModel.BackgroundScenePath), MethodType.Getter)]
class CustomActBackgroundScenePath
{
    [HarmonyPrefix]
    static bool UseAltTexture(ActModel __instance, ref string? __result)
    {
        if (__instance is not CustomActModel customAct) return true;
        if (customAct.CustomBackgroundScenePath == null) return true;
        __result = customAct.CustomBackgroundScenePath;
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
        if (customAct.CustomMapBotBgPath == null) return true;
        __result = customAct.CustomMapBotBgPath;
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
        if (customAct.CustomMapMidBgPath == null) return true;
        __result = customAct.CustomMapMidBgPath;
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
        if (customAct.CustomMapTopBgPath == null) return true;
        __result = customAct.CustomMapTopBgPath;
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
        if (customAct.CustomRestSiteBackgroundPath == null) return true;
        __result = customAct.CustomRestSiteBackgroundPath;
        return false;
    }
}


[HarmonyPatch(typeof(ActModel), nameof(ActModel.GenerateBackgroundAssets))]
public class CustomActGenerateBackgroundAssets
{
    [HarmonyPrefix]
    public static bool Prefix(ActModel __instance, Rng rng, ref BackgroundAssets __result)
    {
        if (__instance is not CustomActModel customActModel)
            return true;
        __result = CreateBackgroundAssets(customActModel, rng);
        return false;
    }
    
    private static BackgroundAssets CreateBackgroundAssets(CustomActModel customActModel, Rng rng)
    {
        var instance = (BackgroundAssets)FormatterServices.GetUninitializedObject(typeof(BackgroundAssets));
        
        var bgSceneField = AccessTools.Field(instance.GetType(), "<BackgroundScenePath>k__BackingField");
        var bgLayersField = AccessTools.Field(instance.GetType(), "<BgLayers>k__BackingField");
        var fgLayerField = AccessTools.Field(instance.GetType(), "<FgLayer>k__BackingField");
        
        bgSceneField.SetValue(instance, customActModel.GetBackgroundAssetScenePath(rng));
        bgLayersField.SetValue(instance, customActModel.GetBackgroundAssetBgLayerPaths(rng));
        fgLayerField.SetValue(instance, customActModel.GetBackgroundAssetFgLayerPath(rng));
        
        return instance;
    }
}

