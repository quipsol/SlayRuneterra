using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Timeline;
using SlayRuneterra.Content.Epochs;

namespace SlayRuneterra.Utils.Epochs;

[HarmonyPatch(typeof(EpochModel), "EpochIds", MethodType.Getter)]
public class EpochModels_Patch
{ 
    
    [HarmonyPostfix] 
    private static void Postfix(EpochModel __instance, ref IEnumerable<string> __result)
    {
        MainFile.Logger.Info("EpochModels_Patch EpochIds Postfix");
        __result = [.. __result, .. CustomEpochList.CustomEpochs.Select(x => x.Id)];
        foreach (var epoch in __result)
        {
            MainFile.Logger.Info($"epoch: {epoch}");
        }
    }
}