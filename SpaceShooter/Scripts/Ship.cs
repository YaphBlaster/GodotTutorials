using Godot;
using System;

public class Ship : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    private int SPEED = 100;
    private Vector2 positionTemp = Vector2.Zero;

    private PackedScene Laser = (PackedScene)ResourceLoader.Load("res://SpaceShooter/Scenes/Laser.tscn");

    private PackedScene ExplosionEffect = (PackedScene)ResourceLoader.Load("res://SpaceShooter/Scenes/ExplosionEffect.tscn");


    [Signal]
    public delegate void PlayerDeath();


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        positionTemp = Position;
        float deltaSpeed = SPEED * delta;

        if (Input.IsActionPressed("ui_up"))
        {
            positionTemp.y -= deltaSpeed;
        }

        if (Input.IsActionPressed("ui_down"))
        {
            positionTemp.y += deltaSpeed;
        }

        if (Input.IsActionJustPressed("ui_accept"))
        {
            FireLaser();
        }

        Position = positionTemp;
    }

    public void FireLaser()
    {
        //Bullet instance
        RigidBody2D laser = (RigidBody2D)Laser.Instance();

        //Root Node
        Node main = GetTree().CurrentScene;
        main.AddChild(laser);

        //Set bullet's global position to the ship's global position
        laser.GlobalPosition = GlobalPosition;

    }

    public void _on_Ship_area_entered(Area2D area)
    {
        area.QueueFree();
        QueueFree();
    }

    //This function will run when a node is going to exit the tree (be deleted)
    //Since this is tying into the Godot System, the override keyword must be used
    public override void _ExitTree()
    {
        base._ExitTree();
        Node main = GetTree().CurrentScene;
        Sprite explosionEffect = (Sprite)ExplosionEffect.Instance();
        //Calls the method on the object during idle time
        main.CallDeferred("add_child", explosionEffect);
        explosionEffect.GlobalPosition = GlobalPosition;
        EmitSignal("PlayerDeath");
    }
}
