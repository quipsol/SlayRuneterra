using HarmonyLib;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes;

namespace SlayRuneterra.Patches;

[HarmonyPatch(typeof(NGame), nameof(NGame.IsReleaseGame))]
public class EnableAutoSlay_Patch
{
    [HarmonyPrefix]
    private static bool Prefix(ref bool __result)
    {
        __result = false;
        return false;
    }
}