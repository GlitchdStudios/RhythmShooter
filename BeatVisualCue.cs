using Godot;
using System;
using System.Diagnostics;

public partial class BeatVisualCue : CanvasLayer
{
	[Export] public float BeatInterval = 0.666666667f;
	[Export] public Color PulseColor = Colors.White;
	[Export] public Color BaseColor = new Color(0.1f, 0.1f, 0.1f);
	public bool IsOnBeat;
	private float _timeSinceLastBeat;
	private ColorRect _beatCueSprite;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_beatCueSprite = GetNode<ColorRect>("BeatCueSprite");
		Debug.Assert(_beatCueSprite != null, "BeatCueSprite (ColorRect) node not found! Make sure it's named correctly and is a child of the node with this script (or adjust GetNode path).");
		_beatCueSprite.Color = BaseColor;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_timeSinceLastBeat += (float)delta;

		if (_timeSinceLastBeat >= BeatInterval)
		{
			_timeSinceLastBeat = 0f;
			TriggerBeatCue();
		}
	}

	private async void TriggerBeatCue()
	{
		IsOnBeat = true;
		Debug.Assert(_beatCueSprite != null, "BeatCueSprite (ColorRect) node not found! Make sure it's named correctly and is a child of the node with this script (or adjust GetNode path).");
		_beatCueSprite.Color = PulseColor;
		await ToSignal(GetTree().CreateTimer(0.05f), "timeout");
		_beatCueSprite.Color = BaseColor;
		await ToSignal(GetTree().CreateTimer(0.2f), "timeout");
		IsOnBeat = false;
	}
}
