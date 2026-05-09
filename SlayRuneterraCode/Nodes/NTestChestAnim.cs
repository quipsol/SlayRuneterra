using BaseLib.BaseLibScenes.Acts;
using Godot;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace SlayRuneterra.Nodes;

[GlobalClass]
public partial class NTestChestAnim : NCustomTreasureRoomChest
{
    private AnimationPlayer? AnimationPlayer { get; set; }
    
    public override void _Ready()
    {
        AnimationPlayer = GetNode<AnimationPlayer>("%ChestAnimationPlayer");
    }
    
    protected override void OnChestButtonReleased(NButton _)
    {
        TaskHelper.RunSafely(OpenChest());
    }
    
    protected override void OnMouseEntered()
    {
        TaskHelper.RunSafely(ChestHighlighted());
    }

    protected override void OnMouseExited()
    {
        TaskHelper.RunSafely(ChestClosed());
    }
    
    
    private async Task OpenChest()
    {
        AnimationPlayer!.Play("animation_test_one");
    }
    
    private async Task ChestClosed()
    {
        AnimationPlayer!.Play("chest_closed");
    }
    
    private async Task ChestHighlighted()
    {
        AnimationPlayer!.Play("chest_highlighted");
    }
    
   

    
 
}