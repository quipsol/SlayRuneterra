using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes.HoverTips;

namespace SlayRuneterra.Patches;


[HarmonyPatch(typeof(NHoverTipSet), "Init")]
public static class NHoverTipSet_Init_Patch
{
    static bool Prefix(NHoverTipSet __instance, Control owner, ref IEnumerable<IHoverTip> hoverTips)
    {
        foreach (var hoverTip in hoverTips)
        {
            //MainFile.Logger.Info(hoverTip.Id);
            if (hoverTip.Id == "AFFLICTION.BLIND")
            {
                hoverTips = new List<IHoverTip>() {hoverTip};
                return true;
            }
        }
        return true;
    }
}