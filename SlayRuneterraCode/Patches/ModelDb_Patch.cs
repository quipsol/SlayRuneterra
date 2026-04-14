using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using SlayRuneterra.Content.Acts;

namespace SlayRuneterra.Patches;

[HarmonyPatch(typeof(ModelDb), nameof(ModelDb.Acts), MethodType.Getter)]
public static class ModelDb_Acts_Patch
{
    static void Postfix(ref IEnumerable<ActModel> __result)
    {
        if (!__result.Any(a => a is Demacia))
        {
            __result = __result.Concat([ModelDb.Act<Demacia>()]);
        }
    }
}