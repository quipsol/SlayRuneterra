using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using SlayRuneterra.Utils.CustomVars;

namespace SlayRuneterra.Patches;


[HarmonyPatch(typeof(RelicModel), nameof(RelicModel.DynamicDescription), MethodType.Getter)]
public class RelicModel_DynamicDescription_Patch
{
    private static readonly AccessTools.FieldRef<RelicModel, Player?> OwnerRef =
                AccessTools.FieldRefAccess<RelicModel, Player?>("_owner");
    
    public static void Postfix(RelicModel __instance, ref LocString __result)
    {
        LocString resVar = __result;

        Player? owner = OwnerRef(__instance);
        bool hasOwner = owner is not null;
        resVar.Add("HasOwner", hasOwner);
        resVar.Add("IsEvent", false);
        resVar.Add("InRun", hasOwner && owner!.RunState is not null); // Should theoretically always be equal to "HasOwner"
        resVar.Add("IsMultiplayer", hasOwner && owner!.RunState.Players.Count > 1);
        __result = resVar;

        
        // This works, but I would prefer the logic to update CalculatedVars to not be in this patch
        if (hasOwner)
        {
            foreach (DynamicVar item in __instance.DynamicVars.Values.ToList())
            {
                if (item is CalculatedRelicVar calculatedRelicVar)
                {
                    calculatedRelicVar.UpdatePreviewVar(false);
                }
            }     
        }
       
    }
}

// The same as above, but for a different method
[HarmonyPatch(typeof(RelicModel), nameof(RelicModel.DynamicEventDescription), MethodType.Getter)]
public class RelicModel_DynamicEventDescription_Patch
{
    private static readonly AccessTools.FieldRef<RelicModel, Player?> OwnerRef =
                AccessTools.FieldRefAccess<RelicModel, Player?>("_owner");
    
    public static void Postfix(RelicModel __instance, ref LocString __result)
    {
        LocString resVar = __result;

        Player? owner = OwnerRef(__instance);
        bool hasOwner = owner is not null;
        resVar.Add("HasOwner", hasOwner);
        resVar.Add("IsEvent", true);
        resVar.Add("InRun", hasOwner && owner!.RunState is not null);
        resVar.Add("IsMultiplayer", hasOwner && owner!.RunState.Players.Count > 1);
        __result = resVar;
        
        if (hasOwner)
        {
            foreach (DynamicVar item in __instance.DynamicVars.Values.ToList())
            {
                if (item is CalculatedRelicVar calculatedRelicVar)
                {
                    calculatedRelicVar.UpdatePreviewVar(false);
                }
            }     
        }
       
    }
}