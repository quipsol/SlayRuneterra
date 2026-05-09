using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Timeline;

namespace SlayRuneterra.Utils.Epochs;

[HarmonyPatch(typeof(NTimelineScreen), nameof(NTimelineScreen.AddEpochSlots))]
class NTimelineScreen_Patch
{
    static void Prefix(NTimelineScreen __instance, ref List<EpochSlotData> slotsToAdd, bool isAnimated)
    {
        MainFile.Logger.Info("NTimelineScreen AddEpochSlots Prefix");
        //slotsToAdd.Add(new EpochSlotData("SLAYRUNETERRA-UPRISING_EPOCH", EpochSlotState.NotObtained));
        foreach (var slot in slotsToAdd)
        {
            MainFile.Logger.Info(slot.Model.Id);
        }
    }
}
