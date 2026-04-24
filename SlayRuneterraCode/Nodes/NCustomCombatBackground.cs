using Godot;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace SlayRuneterra.Nodes;

[GlobalClass]
public partial class NCustomCombatBackground : NCombatBackground
{
	public override void _Ready()
	{
		MainFile.Logger.Warn("Ready() from NCustomCombatBackground");
		base._Ready();
	}
}
