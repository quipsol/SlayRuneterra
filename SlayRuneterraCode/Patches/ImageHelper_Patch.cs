using HarmonyLib;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;

namespace SlayRuneterra.Patches;


[HarmonyPatch(typeof(ImageHelper), nameof(ImageHelper.GetRoomIconPath))]
public static class ImageHelper_GetRoomIconPath_Patch
{
    static bool Prefix(MapPointType mapPointType, RoomType roomType, ModelId? modelId, ref string __result)
    {
        if (modelId?.Entry.ToLowerInvariant() == "slayruneterra-galio_boss")
        {
            __result = "res://SlayRuneterra/images/placeholder/100_100/yellow.png";
            return false; 
        }
        if (modelId?.Entry.ToLowerInvariant() == "slayruneterra-jarvan_the_fourth_boss")
        {
            __result = "res://SlayRuneterra/images/placeholder/100_100/green.png";
            return false; 
        }
        if (modelId?.Entry.ToLowerInvariant() == "slayruneterra-garen_boss")
        {
            __result = "res://SlayRuneterra/images/placeholder/100_100/blue.png";
            return false; 
        }
        if (modelId?.Entry.ToLowerInvariant() == "slayruneterra-lux_boss")
        {
            __result = "res://SlayRuneterra/images/placeholder/100_100/purple.png";
            return false; 
        }
        

        return true;
    }
}

[HarmonyPatch(typeof(ImageHelper), nameof(ImageHelper.GetRoomIconOutlinePath))]
public static class ImageHelper_GetRoomIconOutlinePath_Patch
{
    static bool Prefix(MapPointType mapPointType, RoomType roomType, ModelId? modelId, ref string __result)
    {
        if (modelId?.Entry.ToLowerInvariant() == "slayruneterra-galio_boss")
        {
            __result = "res://SlayRuneterra/images/placeholder/100_100/black.png";
            return false; 
        }
        if (modelId?.Entry.ToLowerInvariant() == "slayruneterra-jarvan_the_fourth_boss")
        {
            __result = "res://SlayRuneterra/images/placeholder/100_100/black.png";
            return false; 
        }
        if (modelId?.Entry.ToLowerInvariant() == "slayruneterra-garen_boss")
        {
            __result = "res://SlayRuneterra/images/placeholder/100_100/black.png";
            return false; 
        }
        if (modelId?.Entry.ToLowerInvariant() == "slayruneterra-lux_boss")
        {
            __result = "res://SlayRuneterra/images/placeholder/100_100/black.png";
            return false; 
        }

        return true;
    }
}