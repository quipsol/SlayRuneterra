using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace SlayRuneterra.Nodes;

public partial class NCustomTreasureRoomChest  : Control
{
    
    public static NCustomTreasureRoomChest? Create(NTreasureRoom nTreasureRoom, IRunState runState, NButton chestButton, string scenePath)
    {
        NCustomTreasureRoomChest nTestChestAnim = PreloadManager.Cache.GetScene(scenePath).Instantiate<NCustomTreasureRoomChest>(PackedScene.GenEditState.Disabled);
        nTestChestAnim.RunState = runState;
        nTestChestAnim.TreasureRoomNode = nTreasureRoom;
        chestButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(nTestChestAnim.OnChestButtonReleased));
        chestButton.Connect(Control.SignalName.MouseEntered, Callable.From(nTestChestAnim.OnMouseEntered));
        chestButton.Connect(Control.SignalName.MouseExited, Callable.From(nTestChestAnim.OnMouseExited));
        return nTestChestAnim;
    }
    
    protected IRunState? RunState { get; private set; }
    protected NTreasureRoom? TreasureRoomNode { get; private set; }
    
    protected virtual void OnChestButtonReleased(NButton nButton) { }
    protected virtual void OnMouseEntered() { }
    protected virtual void OnMouseExited() { }
}