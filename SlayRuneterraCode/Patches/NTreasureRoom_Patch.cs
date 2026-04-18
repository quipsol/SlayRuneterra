using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
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
    private static readonly AccessTools.FieldRef<NTreasureRoom, MegaSprite?> ChestAnimControllerRef =
                AccessTools.FieldRefAccess<NTreasureRoom, MegaSprite?>("_chestAnimController");
    private static readonly AccessTools.FieldRef<NTreasureRoom, Node2D?> ChestNodeRef =
                AccessTools.FieldRefAccess<NTreasureRoom, Node2D?>("_chestNode");

    [HarmonyPostfix]
    public static void Postfix(NTreasureRoom __instance)
    {
        MainFile.Logger.Warn("NTreasureRoom Postfix");
        IRunState? runState = RunStateRef(__instance);
        MegaSprite? chestAnimController = ChestAnimControllerRef(__instance);
        Node2D? chestNode = ChestNodeRef(__instance);
        if (runState is null || chestAnimController is null || chestNode is null)
        {
            MainFile.Logger.Error("RunState not found");
            return;
        }

        if (runState.Act is not CustomActModel customActModel)
            return;
        MainFile.Logger.Warn("We are custom Act");
        if (customActModel.ChestAtlasPath is null)
            return;

        chestNode.Visible = false;
        Node parent = chestNode.GetParent();
        
        MainFile.Logger.Info(parent.Name);
       
        NTestChestAnim? nTestChestAnim = NTestChestAnim.Create(runState);
        if (nTestChestAnim is null)
        {
            MainFile.Logger.Error("NTestChestAnim is null");
            return;
        }
        
        MainFile.Logger.Info("Attempting to add child safely");
        parent.AddChildSafely(nTestChestAnim);
        
        foreach (var n in nTestChestAnim.GetChildren())
        {
            MainFile.Logger.Info(n.Name);
        }
        
        var chestAnimationPlayer = nTestChestAnim.GetNode<Node>("%ChestAnimationPlayer");
        MainFile.Logger.Info(chestAnimationPlayer.Name);
        
        
        
    }
}