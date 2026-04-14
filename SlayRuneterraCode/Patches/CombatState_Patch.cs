using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace SlayRuneterra.Patches;

// Not necessary yet

//[HarmonyPatch(typeof(CombatState), nameof(CombatState.IterateHookListeners))]
public static class CombatState_IterateHookListeners_Patch
{
    static void Postfix(CombatState __instance, ref IEnumerable<AbstractModel> __result)
    {
        //MainFile.Logger.Warn("Iterate Hook Listener --- Postfix");
       __result = AppendItem(__result, __instance);
    }
    
    static IEnumerable<AbstractModel> AppendItem(IEnumerable<AbstractModel> original, CombatState instance)
    {
        foreach (var item in original)
            yield return item;
        
        yield return instance.RunState.Act;
    }
}


