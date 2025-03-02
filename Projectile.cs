using Godot;
using System.Diagnostics;

public partial class Projectile : Node2D
{
	[Export] public int Speed = 300;

	public override void _Ready()
	{
		Area2D projectileArea = GetNode<Area2D>("ProjectileArea");
		Debug.Assert(projectileArea != null, "ProjectileArea node not found");
		
		projectileArea.AreaEntered += OnProjectileAreaEntered;
		
		void OnProjectileAreaEntered(Area2D area)
		{
			if (area is Enemy enemy)
			{
				QueueFree();
				enemy.QueueFree();
				GD.Print("Projectile hit enemy");
			}
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = -Transform.Y * Speed;

		Position += velocity * (float)delta;

		if (Position.Y < -1000) //Destroy if off-screen (could check if visible on-screen instead
		{
			QueueFree();
		}
	}
}
