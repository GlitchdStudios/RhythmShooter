using Godot;
using System.Diagnostics;

public partial class Player : CharacterBody2D
{
	[Export] public float Speed = 200.0f;
	[Export] public float FireRate = 0.2f;
	[Export] public Node2D Muzzle;
	private float _timeSinceLastShot;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity; // Get current velocity (important for CharacterBody2D)
		Vector2 direction = Input.GetVector("MoveLeft", "MoveRight", "MoveForward", "MoveBackward"); // Get input direction

		if (direction != Vector2.Zero)
		{
			velocity = direction.Normalized() * Speed; // Set velocity based on direction and speed
		}
		else
		{
			velocity = Vector2.Zero; // Stop moving if no input
		}

		Velocity = velocity; // Set the CharacterBody2D's velocity
		MoveAndSlide(); // Call MoveAndSlide (no velocity argument needed for CharacterBody2D in 4.x)

		_timeSinceLastShot += (float)delta;

		if (_timeSinceLastShot >= FireRate)
		{
			Fire();
			_timeSinceLastShot = 0f;
		}
		
		FireRhythmBlast();
	}

	private void Fire()
	{
		PackedScene projectileScene = ResourceLoader.Load<PackedScene>("res://Scenes/Projectile.tscn");
		Debug.Assert(projectileScene != null, "Could not load Projectile scene! Make sure Projectile.tscn exists in the 'Scenes' folder.");
		Projectile projectileInstance = projectileScene.Instantiate<Projectile>();
		projectileInstance.Position = Muzzle.GlobalPosition;
		projectileInstance.Rotation = Muzzle.Rotation;
		GetParent().AddChild(projectileInstance);
	}
	
	private void FireRhythmBlast()
	{
		BeatVisualCue beatVisualCue = GetNode<BeatVisualCue>("/root/Main/UILayer");
		Debug.Assert(beatVisualCue != null, $"Couldn't find {nameof(BeatVisualCue)}");

		if (beatVisualCue.IsOnBeat && Input.IsActionJustPressed("RhythmBlast"))
		{
			PackedScene projectileScene = ResourceLoader.Load<PackedScene>("res://Scenes/Projectile.tscn");
			Debug.Assert(projectileScene != null, "Could not load Projectile scene! Make sure Projectile.tscn exists in the 'Scenes' folder.");

			int numberOfProjectiles = 5;
			float spreadAngleDegrees = 30f;
			float angleStepDegrees = spreadAngleDegrees / (numberOfProjectiles - 1);

			for (int i = 0; i < numberOfProjectiles; i++)
			{
				Projectile projectileInstance = projectileScene.Instantiate<Projectile>();
				projectileInstance.Position = Muzzle.GlobalPosition;
				float currentAngleRadians = Mathf.DegToRad(-spreadAngleDegrees / 2f + i * angleStepDegrees);
				projectileInstance.Rotation = Muzzle.Rotation + currentAngleRadians;
				projectileInstance.Scale = Vector2.One * 3;
				GetParent().AddChild(projectileInstance);
			}
		}
	}
}
