using Godot;
using System;

public class GameOverScreen : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    private Label HighscoreLabel = null;

    // Loading a GDScript file
    GDScript SaveAndLoadScript = (GDScript)GD.Load("res://SpaceShooter/Scripts/SaveAndLoad.gd");
    Node SaveAndLoadNode = null;



    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Creating a new Node object of the SaveAndLoad GDScript
        SaveAndLoadNode = (Node)SaveAndLoadScript.New();

        HighscoreLabel = (Label)GetNode("HighscoreLabel");

        SetHighscoreLabel();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("ui_cancel"))
        {
            GetTree().ChangeScene("res://SpaceShooter/Scenes/StartMenu.tscn");
        }
    }

    public void SetHighscoreLabel()
    {
        // Calling a GDScript variable with C#
        // Cast to dictionary type
        Godot.Collections.Dictionary saveData = (Godot.Collections.Dictionary)SaveAndLoadNode.Call("load_data_from_file");
        HighscoreLabel.Text = "Highscore : " + saveData["highscore"];
    }
}
