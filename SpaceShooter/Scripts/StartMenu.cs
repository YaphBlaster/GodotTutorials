using Godot;
using System;

public class StartMenu : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";\

    private AudioStreamPlayer Music = null;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Access singleton from Project Settings > Auto Load
        Music = (AudioStreamPlayer)GetNode("/root/Music");
        Music.Play();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("ui_accept"))
        {
            GetTree().ChangeScene("res://SpaceShooter/Scenes/World_SpaceShooter.tscn");

        }

        if (Input.IsActionJustPressed("ui_accept"))
        {
            // Exit the game
            GetTree().Quit();
        }
    }

    public void _on_StartButton_pressed()
    {
    }
}
