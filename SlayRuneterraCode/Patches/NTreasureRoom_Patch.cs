using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Runs;
using SlayRuneterra.Models;
using SlayRuneterra.Nodes;

namespace SlayRuneterra.Patches;

[HarmonyPatch(typeof(NTreasureRoom), nameof(NTreasureRoom._Ready))]
public static class NTreasureRoom_Patch
{
    private static readonly AccessTools.FieldRef<NTreasureRoom, IRunState?> RunStateRef =
                AccessTools.FieldRefAccess<NTreasureRoom, IRunState?>("_runState");
    private static readonly AccessTools.FieldRef<NTreasureRoom, Node2D?> ChestNodeRef =
                AccessTools.FieldRefAccess<NTreasureRoom, Node2D?>("_chestNode");
    private static readonly AccessTools.FieldRef<NTreasureRoom, NButton?> ChestButtonRef =
                AccessTools.FieldRefAccess<NTreasureRoom, NButton?>("_chestButton");

    [HarmonyPostfix]
    public static void Postfix(NTreasureRoom __instance)
    {
        // validation
        IRunState? runState = RunStateRef(__instance);
        if (runState?.Act is not CustomActModel customActModel) return;
        if (customActModel.CustomChestScene is null) return;
        Node2D? chestNode = ChestNodeRef(__instance);
        NButton? chestButton = ChestButtonRef(__instance);
        if (chestNode is null || chestButton is null)
        {
            MainFile.Logger.Error("References not found");
            return;
        }
        
        // The actual stuff that is being done
        chestNode.Visible = false;
        Node parent = chestNode.GetParent();
        NCustomTreasureRoomChest? customTreasureRoom = NCustomTreasureRoomChest.Create(__instance, runState, chestButton, customActModel.CustomChestScene);
        if (customTreasureRoom is null)
        {
            MainFile.Logger.Error($"NCustomTreasureRoom is null. Tried to instantiate Node from path: {customActModel.CustomChestScene}");
            return;
        }
        parent.AddChildSafely(customTreasureRoom);
    }
}