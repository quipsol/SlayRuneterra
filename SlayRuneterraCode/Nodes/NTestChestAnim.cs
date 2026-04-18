using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.Vfx.Ui;
using MegaCrit.Sts2.Core.Runs;

namespace SlayRuneterra.Nodes;

[GlobalClass]
public partial class NTestChestAnim : Control
{
    private const string SCENE_PATH = "res://SlayRuneterra/animations/backgrounds/treasure_room/test_anim.tscn";

    private IRunState _runState;
    private AnimationPlayer _animationPlayer;

    public static NTestChestAnim? Create(IRunState runState)
    {
        NTestChestAnim nTestChestAnim = PreloadManager.Cache.GetScene(SCENE_PATH).Instantiate<NTestChestAnim>(PackedScene.GenEditState.Disabled);
        nTestChestAnim._runState = runState;
        return nTestChestAnim;
    }
    
    public override void _Ready()
    {
        MainFile.Logger.Warn("Entered _Ready of NTestChestAnim");
        _animationPlayer = GetNode<AnimationPlayer>("%ChestAnimationPlayer");
        //SpeedScale = 0.5f;
        //Play("animation_test_one");
    }

    public override void _EnterTree()
    {
        
    }

    public override void _ExitTree()
    {
        
    }

    public void OpenChest()
    {
        //Play("animation_test_one");
    }
    
 
}