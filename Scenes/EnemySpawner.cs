using Godot;
using System;
using System.Diagnostics;

public partial class EnemySpawner : Node2D
{
	[Export] public PackedScene EnemyScene;
	[Export] public float SpawnRateSeconds = 2f;
	private float _timeSinceLastSpawn;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Debug.Assert(EnemyScene != null, "EnemyScene is not assigned in spawner");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_timeSinceLastSpawn += (float)delta;

		if (_timeSinceLastSpawn >= SpawnRateSeconds)
		{
			SpawnEnemy();
			_timeSinceLastSpawn = 0f;
		}
	}

	private void SpawnEnemy()
	{
		if (EnemyScene == null)
		{
			GD.PrintErr("EnemyScene is not assigned in spawner");
			return;
		}

		Node2D instance = EnemyScene.Instantiate<Node2D>();
		Enemy enemy = instance.GetNode<Enemy>("MovePathFollow/Enemy");
		Vector2 spawnPosition = new Vector2((float)GD.RandRange(-1d,1d) * GetViewportRect().Size.X, -50f);
		instance.Position = Position + spawnPosition;
		GetParent().AddChild(instance);
		GD.Print("Enemy Spawned");
	}
}
