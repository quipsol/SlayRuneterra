using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using SlayRuneterra.Content.Afflictions;

namespace SlayRuneterra.Patches;


[HarmonyPatch(typeof(CardModel), nameof(CardModel.Title), MethodType.Getter)]
public static class CardModel_Title_Patch
{
    static bool Prefix(CardModel __instance, ref string __result)
    {
        if (__instance.Affliction is Blind)
        {
            __result = "???";
            return false;
        }
        return true;
    }
}


[HarmonyPatch(typeof(CardModel))]
public static class CardModel_GetDescriptionForPile_Patch
{
    
    static MethodBase TargetMethod()
    {
        var previewType = AccessTools.TypeByName("MegaCrit.Sts2.Core.Models.CardModel+DescriptionPreviewType");

        return AccessTools.Method(typeof(CardModel), "GetDescriptionForPile",
                    new Type[]
                    {
                                typeof(PileType),
                                previewType,
                                typeof(Creature)
                    });
    }
    
    static bool Prefix(CardModel __instance, ref string __result)
    {
        if (__instance.Affliction is Blind)
        {
            __result = "???";
            return false;
        }
        return true;
    }
}
