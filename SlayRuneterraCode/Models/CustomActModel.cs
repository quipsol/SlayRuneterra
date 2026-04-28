using BaseLib.Abstracts;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using SlayRuneterra.Nodes;

namespace SlayRuneterra.Models;

public abstract class CustomActModel : ActModel, ICustomModel
{
    //public abstract string DummyThicc { get; }
    
    #region Insert Base ActModel defaults
    
    public override Color MapTraveledColor => new Color("27221C");
    public override Color MapUntraveledColor => new Color("6E7750");
    public override Color MapBgColor => new Color("9B9562");

    public override string[] BgMusicOptions => ["event:/music/act3_a1_v1", "event:/music/act3_a2_v1"];
    public override string[] MusicBankPaths => ["res://banks/desktop/act3_a1.bank", "res://banks/desktop/act3_a2.bank"];
    public override string AmbientSfx => "event:/sfx/ambience/act3_ambience";

    protected override int BaseNumberOfRooms => 15;
    
    public override string ChestSpineResourcePath => "res://animations/backgrounds/treasure_room/chest_room_act_3_skel_data.tres";
    public override string ChestSpineSkinNameNormal => "act3";
    public override string ChestSpineSkinNameStroke => "act3_stroke";
    public override string ChestOpenSfx => "event:/sfx/ui/treasure/treasure_act3";
    
    #endregion Insert Base ActModel defaults
    
    protected abstract string CustomBackgroundScenePath { get; }
    protected abstract string CustomMapTopBgPath { get; }
    protected abstract string CustomMapMidBgPath { get; }
    protected abstract string CustomMapBotBgPath { get; }
    protected abstract string CustomRestSiteBackgroundPath { get; }
    
    /// <summary>
    /// Override this if you want to replace the visuals (not the room itself) of the chest in Treasure Rooms.<br></br>
    /// The scenes root node MUST have a script attached that derives from <see cref="NCustomTreasureRoomChest"/> <br></br>
    /// If you intend to replace the entire room, leave this be! <br></br>
    /// (The default chest open animation duration is 1 second)
    /// </summary>
    public virtual string? CustomChestScene => null;
    
    public virtual BackgroundAssets CustomGenerateBackgroundAssets(ActModel parentAct, Rng rng)
    {
        return  new BackgroundAssets("glory", Rng.Chaotic);
    }
    
    
    #region Patches
    
    [HarmonyPatch(typeof(ActModel), nameof(ActModel.BackgroundScenePath), MethodType.Getter)]
    private class CustomActBackgroundScenePath
    {
        [HarmonyPrefix]
        private static bool UseAltTexture(ActModel __instance, ref string? __result)
        {
            if (__instance is not CustomActModel customAct) return true;
            __result = customAct.CustomBackgroundScenePath;
            return false;
        }
    }
    
    [HarmonyPatch(typeof(ActModel), nameof(ActModel.MapTopBgPath), MethodType.Getter)]
    private class CustomActMapTopBgPath
    {
        [HarmonyPrefix]
        private static bool UseAltTexture(ActModel __instance, ref string? __result)
        {
            if (__instance is not CustomActModel customAct) return true;
            __result = customAct.CustomMapTopBgPath;
            return false;
        }
    }
    
    [HarmonyPatch(typeof(ActModel), nameof(ActModel.MapMidBgPath), MethodType.Getter)]
    private class CustomActMapMidBgPath
    {
        [HarmonyPrefix]
        private static bool UseAltTexture(ActModel __instance, ref string? __result)
        {
            if (__instance is not CustomActModel customAct) return true;
            __result = customAct.CustomMapMidBgPath;
            return false;
        }
    }

    [HarmonyPatch(typeof(ActModel), nameof(ActModel.MapBotBgPath), MethodType.Getter)]
    private class CustomActMapBotBgPath
    {
        [HarmonyPrefix]
        private static bool UseAltTexture(ActModel __instance, ref string? __result)
        {
            if (__instance is not CustomActModel customAct) return true;
            __result = customAct.CustomMapBotBgPath;
            return false;
        }
    }
    
    [HarmonyPatch(typeof(ActModel), nameof(ActModel.RestSiteBackgroundPath), MethodType.Getter)]
    private class CustomActRestSiteBackgroundPath
    {
        [HarmonyPrefix]
        private static bool UseAltTexture(ActModel __instance, ref string? __result)
        {
            if (__instance is not CustomActModel customAct) return true;
            __result = customAct.CustomRestSiteBackgroundPath;
            return false;
        }
    }
    
    #endregion Patches
    
}







