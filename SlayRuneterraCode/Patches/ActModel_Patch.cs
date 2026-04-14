using System.Reflection;
using System.Runtime.Serialization;
using SlayRuneterra.Content.Acts;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Unlocks;

namespace SlayRuneterra.Patches;


[HarmonyPatch(typeof(ActModel), nameof(ActModel.GetRandomList))]
public class LegacyActsPatch
{
    public static void Postfix(ref IEnumerable<ActModel> __result, Rng rng, UnlockState unlockState, bool isMultiplayer)
    {
        if (!SlayRuneterraConfig.IsEnabled) return;
        
        var list = __result.ToList();
        
        list[0] = ModelDb.Act<Demacia>();
        list[1] = ModelDb.Act<Freljord>();

        __result = list;
    }
}