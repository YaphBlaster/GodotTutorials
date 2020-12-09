using Godot;
using System;

public class World : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private AnimationPlayer animationPlayer = null;
    private Sprite rocketShipSprite = null;

    [Export]
    private Texture rocketLaunchTexture = null;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Set animationPlayer variable to the AnimationPlayer node
        animationPlayer = (AnimationPlayer)GetNode("AnimationPlayer");

        // Set rocketShipSprite variable to the RocketShipSprite node
        rocketShipSprite = (Sprite)GetNode("RocketShipSprite");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    public void _on_LaunchButton_pressed()
    {
        animationPlayer.Play("Launch");
        rocketShipSprite.Texture = rocketLaunchTexture;
    }
}
