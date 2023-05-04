using Godot;
using System;

public partial class EnemySpawner : Area2D
{
    [Signal]
    public delegate void CreateEnemiesEventHandler(CharacterBody2D[] enemies);

    [Export]
    public int TimeToSpawn { get; set; }

    [Export]
    public string EnemyScene { get; set; }

    [Export]
    public int MaxEnemies { get; set; }

    public CollisionShape2D CollisionShape2D { get; set; }

    private double _timeToSpawnCount = 0;

    private RandomNumberGenerator _randomNumberGenerator = new();

    public override void _Ready()
    {
        this.CollisionShape2D = this.GetNode<CollisionShape2D>(nameof(this.CollisionShape2D));
    }

    public override void _Process(double delta)
    {
        this._timeToSpawnCount += delta;

        if (this._timeToSpawnCount >= this.TimeToSpawn)
        {
            var collisionArea = this.CollisionShape2D.Shape.GetRect().Size;
            var spawnArea = new Vector2(this.CollisionShape2D.GlobalPosition.X + collisionArea.X * 0.5f, this.CollisionShape2D.GlobalPosition.Y + collisionArea.Y * 0.5f);

            this.SpawnEnemy(
                this._randomNumberGenerator.RandiRange(1, this.MaxEnemies),
                spawnArea
            );
            this._timeToSpawnCount = 0;
        }
    }

    public void SpawnEnemy(int cuantity, Vector2 mapSize)
    {
        CharacterBody2D[] enemies = new CharacterBody2D[cuantity];

        for (int i = 0; i < cuantity; i++)
        {
            var enemy = GD.Load<PackedScene>(this.EnemyScene).Instantiate<CharacterBody2D>();
            var position = new Vector2I(
                this._randomNumberGenerator.RandiRange(0, (int)mapSize.X),
                this._randomNumberGenerator.RandiRange(0, (int)mapSize.Y)
            );

            enemy.GlobalPosition = position;
            enemies[i] = enemy;
        }

        this.EmitSignal(SignalName.CreateEnemies, enemies);
    }
}
