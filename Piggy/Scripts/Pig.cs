using Godot;
using System;

public class Pig : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    private int SPEED = 100;
    private Boolean isMoving = false;
    private AnimationPlayer animationPlayer = null;
    private Sprite sprite = null;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Set animationPlayer variable to the AnimationPlayer node
        animationPlayer = (AnimationPlayer)GetNode("AnimationPlayer");

        sprite = (Sprite)GetNode("Sprite");
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        isMoving = false;
        float deltaSpeed = SPEED * delta;
        if (Input.IsActionPressed("ui_right"))
        {
            Move(deltaSpeed);
            sprite.FlipH = false;
        }
        if (Input.IsActionPressed("ui_left"))
        {
            Move(-deltaSpeed);
            sprite.FlipH = true;
        }
        if (Input.IsActionPressed("ui_up"))
        {
            Move(ySpeed: -deltaSpeed);
        }
        if (Input.IsActionPressed("ui_down"))
        {
            Move(ySpeed: deltaSpeed);
        }

        animationPlayer.Play(isMoving ? "Run" : "Idle");

        // used for multiple overlapping Areas
        // Can check how many objects are being collided with (Array.length)
        // Godot.Collections.Array areas = GetOverlappingAreas();

        // foreach (Area2D area in areas)
        // {
        //     area.QueueFree();
        // }
    }

    public void Move(float xSpeed = 0, float ySpeed = 0)
    {
        Vector2 positionTemp = Position;
        positionTemp.x += xSpeed;
        positionTemp.y += ySpeed;
        Position = positionTemp;
        isMoving = true;
    }

    public void _on_Pig_area_entered(Area2D area)
    {
        // Delete overlapped Area Node
        area.QueueFree();
        Scale *= 1.1f;
    }
}
