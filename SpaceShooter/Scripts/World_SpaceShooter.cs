using Godot;
using System;

public class World_SpaceShooter : Node
{
	private int score = 0;

	private Label ScoreLabel = null;
	GDScript SaveAndLoadScript = (GDScript)GD.Load("res://SpaceShooter/Scripts/SaveAndLoad.gd");
	Node SaveAndLoadNode = null;

	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	private void SetScore(int value)
	{
		score = value;
		UpdateScoreLabel();
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SaveAndLoadNode = (Node)SaveAndLoadScript.New(); // This is a Godot.Object
		ScoreLabel = (Label)GetNode("ScoreLabel");
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      
	//  }

	public void _on_Enemy_Increase_Score()
	{
		SetScore(score + 10);
	}

	async public void _on_Ship_PlayerDeath()
	{
		// The player has died so update the saveData
		UpdateSaveData();
		// Create a timer for 1 second and await for that timer to be done
		await ToSignal(GetTree().CreateTimer(1), "timeout");

		GetTree().ChangeScene("res://SpaceShooter/Scenes/GameOverScreen.tscn");
	}

	public void UpdateScoreLabel()
	{
		ScoreLabel.Text = $"Score = {score}";
	}

	public void UpdateSaveData()
	{
		Godot.Collections.Dictionary saveData = (Godot.Collections.Dictionary)SaveAndLoadNode.Call("load_data_from_file");
		int highscore = Int32.Parse(saveData["highscore"].ToString());

		if (score > highscore)
		{
			saveData["highscore"] = score;
			SaveAndLoadNode.Call("save_data_to_file", saveData);
		}
	}
}
