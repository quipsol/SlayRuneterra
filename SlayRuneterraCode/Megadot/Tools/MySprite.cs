using Godot;

namespace SlayRuneterra.Megadot.Tools;


[Tool]
public partial class MySprite : Sprite2D
{
	public override void _Process(double delta)
	{
		Rotation += Mathf.Pi * (float)delta;
	}
}
