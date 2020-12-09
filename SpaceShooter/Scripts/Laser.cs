using Godot;
using System;

public class Laser : RigidBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    private AudioStreamPlayer LaserSound = null;
    private PackedScene HitEffect = (PackedScene)ResourceLoader.Load("res://SpaceShooter/Scenes/HitEffect.tscn");

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        LaserSound = (AudioStreamPlayer)GetNode("LaserSound");
        LaserSound.Play();
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    public void _on_VisibilityNotifier2D_screen_exited()
    {
        QueueFree();
    }

    public void CreateHitEffect()
    {
        Node main = GetTree().CurrentScene;
        Node2D hitEffect = (Node2D)HitEffect.Instance();
        main.AddChild(hitEffect);
        hitEffect.GlobalPosition = GlobalPosition;
    }
}
