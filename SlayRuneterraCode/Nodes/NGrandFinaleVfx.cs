using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;

namespace SlayRuneterra.Nodes;

[GlobalClass]
public partial class NGrandFinaleVfx : Node2D
{
    private static readonly StringName _color = new StringName("color");
    public static readonly string scenePath = "res://SlayRuneterra/scenes/vfx/special/vfx_grand_finale.tscn";
    
    [Export(PropertyHint.None, "")]
    private Array<GpuParticles2D> _particles = new ();
    [Export(PropertyHint.None, "")]
    private Array<GpuParticles2D> _modulateParticles = new ();
    
    private CancellationTokenSource? _cts;
    
    
    public static NGrandFinaleVfx? Create(Creature creature, Color tint, bool goingRight)
    {
        MainFile.Logger.Info("NGrandFinaleVfx Create");
        if (TestMode.IsOn)
        {
            return null;
        }
        NCreature? nCreature = NCombatRoom.Instance?.GetCreatureNode(creature);
        if (nCreature != null)
        {
            return Create(nCreature.VfxSpawnPosition, tint, goingRight);
        }
        return null;
    }
    
    public static NGrandFinaleVfx? Create(Vector2 targetCenter, Color tint, bool goingRight)
    {
        MainFile.Logger.Info("NGrandFinaleVfx Create");
        if (TestMode.IsOn)
            return null;
        NGrandFinaleVfx nGrandFinaleVfx = PreloadManager.Cache.GetScene(NGrandFinaleVfx.scenePath).Instantiate<NGrandFinaleVfx>();
        nGrandFinaleVfx.GlobalPosition = targetCenter;
        nGrandFinaleVfx.ApplyTint(tint);
        nGrandFinaleVfx.Scale = new Vector2(goingRight ? 1f : -1f, 1f);
        return nGrandFinaleVfx;
    }
    
    public override void _Ready() => TaskHelper.RunSafely(this.PlaySequence());
    public override void _ExitTree()
    {
        this._cts?.Cancel();
        this._cts?.Dispose();
    }
    
    private async Task PlaySequence()
    {
        MainFile.Logger.Info("NGrandFinaleVfx Play Sequence");
        this._cts = new CancellationTokenSource();
        for (int index = 0; index < this._particles.Count; ++index)
            this._particles[index].Restart();
        await Cmd.Wait(2f, this._cts.Token);
        MainFile.Logger.Info("NGrandFinaleVfx Exit Play Sequence");
        this.QueueFreeSafely();
    }
    
    public void ApplyTint(Color tint)
    {
        for (int index = 0; index < this._modulateParticles.Count; ++index)
        {
            this._modulateParticles[index].ProcessMaterial = (Material) this._modulateParticles[index].ProcessMaterial.Duplicate();
            this._modulateParticles[index].ProcessMaterial.Set(NGrandFinaleVfx._color, (Variant) tint);
        }
    }
    
    
    
}