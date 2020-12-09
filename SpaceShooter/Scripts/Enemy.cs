using Godot;
using System;

public class Enemy : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    private int SPEED = 20;
    [Export]
    private int ARMOR = 3;

    private PackedScene ExplosionEffect = (PackedScene)ResourceLoader.Load("res://SpaceShooter/Scenes/ExplosionEffect.tscn");

    private Vector2 positionTemp = Vector2.Zero;


    [Signal]
    public delegate void IncreaseScore();


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Node main = GetTree().CurrentScene;

        if (main.GetType().ToString().Equals("World_SpaceShooter"))
        {
            //Creating a custom connection to the IncreaseScore signal
            //This will connect to the World Node which is referenced in the variable main
            //This will connect the ready function to the _on_Enemy_Increase_Score receiver function that will be created in the World_SpaceShooter Class
            Connect("IncreaseScore", main, "_on_Enemy_Increase_Score");
        }


    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        positionTemp = Position;
        float deltaSpeed = SPEED * delta;

        positionTemp.x -= deltaSpeed;

        Position = positionTemp;
    }

    public void _on_Enemy_body_entered(Node body)
    {
        Laser laser = (Laser)body;
        if (laser != null)
        {
            laser.CreateHitEffect();
        }
        body.QueueFree();
        ARMOR -= 1;
        if (ARMOR <= 0)
        {
            //Calling a custom signal
            EmitSignal(nameof(IncreaseScore));

            //Create explosion effect
            CreateExplosion();

            //Delete Enemy
            QueueFree();
        }
    }

    public void _on_VisibilityNotifier2D_screen_exited()
    {
        QueueFree();
    }

    //This function will run when a node is going to exit the tree (be deleted)
    //Since this is tying into the Godot System, the override keyword must be used
    // public override void _ExitTree()
    // {
    //     base._ExitTree();
    //     Node main = GetTree().CurrentScene;
    //     Sprite explosionEffect = (Sprite)ExplosionEffect.Instance();
    //     main.AddChild(explosionEffect);
    //     explosionEffect.GlobalPosition = GlobalPosition;
    // }

    public void AddToScore()
    {

    }

    public void CreateExplosion()
    {
        Node main = GetTree().CurrentScene;
        Sprite explosionEffect = (Sprite)ExplosionEffect.Instance();
        main.AddChild(explosionEffect);
        explosionEffect.GlobalPosition = GlobalPosition;
    }


}
