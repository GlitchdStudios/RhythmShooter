using Godot;
using System.Diagnostics;

public partial class BeatVisualCue : CanvasLayer
{
	[Export] public float BeatInterval = 0.666666667f;
	[Export] public Color PulseColor = Colors.White;
	[Export] public Color BaseColor = new Color(0.1f, 0.1f, 0.1f);
	public bool IsOnBeat;
	private ColorRect _beatCueSprite;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_beatCueSprite = GetNode<ColorRect>("BeatCueSprite");
		Debug.Assert(_beatCueSprite != null, "BeatCueSprite (ColorRect) node not found! Make sure it's named correctly and is a child of the node with this script (or adjust GetNode path).");
		_beatCueSprite.Color = BaseColor;
		
		Timer timer = new Timer();
		timer.WaitTime = BeatInterval;
		timer.Autostart = true;
		timer.Timeout += TriggerBeatCue;
		AddChild(timer);
	}
	
	private async void TriggerBeatCue()
	{
		IsOnBeat = true;
		Debug.Assert(_beatCueSprite != null, "BeatCueSprite (ColorRect) node not found! Make sure it's named correctly and is a child of the node with this script (or adjust GetNode path).");
		_beatCueSprite.Color = PulseColor;
		await ToSignal(GetTree().CreateTimer(0.05f), "timeout");
		_beatCueSprite.Color = BaseColor;
		await ToSignal(GetTree().CreateTimer(0.15f), "timeout");
		IsOnBeat = false;
	}
}
