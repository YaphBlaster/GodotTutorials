using Godot;
using System;

public class EnemySpawner : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private PackedScene Enemy = (PackedScene)ResourceLoader.Load("res://SpaceShooter/Scenes/Enemy.tscn");
    private Node2D spawnPoints = null;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        spawnPoints = (Node2D)GetNode("SpawnPoints");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    public Vector2 GetSpawnPosition()
    {
        Random rand = new Random();
        Godot.Collections.Array points = spawnPoints.GetChildren();
        Node2D point = (Node2D)points[rand.Next(points.Count)];

        return point.GlobalPosition;
    }

    public void SpawnEnemy()
    {
        Vector2 spawnPosition = GetSpawnPosition();
        Area2D enemy = (Area2D)Enemy.Instance();
        Node main = GetTree().CurrentScene;
        main.AddChild(enemy);
        enemy.GlobalPosition = spawnPosition;

    }

    public void _on_Timer_timeout()
    {
        SpawnEnemy();
    }
}
