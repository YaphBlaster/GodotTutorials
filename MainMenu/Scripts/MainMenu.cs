using Godot;
using System;

public class MainMenu : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    private PackedScene rocketLaunchScene;

    [Export]
    private PackedScene piggyScene;

    [Export]
    private PackedScene spaceShooterScene;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    public void _on_RocketLaunchButton_pressed()
    {
        changeSceneCustom(rocketLaunchScene);
    }

    public void _on_Piggy_pressed()
    {
        changeSceneCustom(piggyScene);
    }

    public void _on_SpaceShooter_pressed()
    {
        changeSceneCustom(spaceShooterScene);
    }

    public void changeSceneCustom(PackedScene scene)
    {
        GetTree().ChangeScene(scene.ResourcePath);
    }
}
