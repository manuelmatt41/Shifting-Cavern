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

    private bool _isActivate = false;

    public override void _Ready()
    {
        this.CollisionShape2D = this.GetNode<CollisionShape2D>(nameof(this.CollisionShape2D));
    }

    public override void _Process(double delta)
    {
        if (this._isActivate)
        {
            this._timeToSpawnCount += delta;
        }

        if (this._timeToSpawnCount >= this.TimeToSpawn)
        {
            var collisionAreaSize = this.CollisionShape2D.Shape.GetRect().Size;

            var minX = this.CollisionShape2D.GlobalPosition.X - collisionAreaSize.X * 0.5f;
            var minY = this.CollisionShape2D.GlobalPosition.Y - collisionAreaSize.Y * 0.5f;
            var maxX = this.CollisionShape2D.GlobalPosition.X + collisionAreaSize.X * 0.5f;
            var maxY = this.CollisionShape2D.GlobalPosition.Y + collisionAreaSize.Y * 0.5f;

            var minSpawnArea = new Vector2(minX, minY);
            var maxSpawnArea = new Vector2(maxX, maxY);

            this.SpawnEnemy(
                this._randomNumberGenerator.RandiRange(1, this.MaxEnemies),
                minSpawnArea,
                maxSpawnArea
            );
            this._timeToSpawnCount = 0;
        }
    }

    public void Start() => this._isActivate = true;

    public void Stop() => this._isActivate = false;

    public void SpawnEnemy(int cuantity, Vector2 minSpawnSize, Vector2 maxSpawnSize)
    {
        CharacterBody2D[] enemies = new CharacterBody2D[cuantity];

        for (int i = 0; i < cuantity; i++)
        {
            var enemy = GD.Load<PackedScene>(this.EnemyScene).Instantiate<CharacterBody2D>();
            var position = new Vector2(
                this._randomNumberGenerator.RandiRange((int)minSpawnSize.X, (int)maxSpawnSize.X),
                this._randomNumberGenerator.RandiRange((int)minSpawnSize.Y, (int)maxSpawnSize.Y)
            );

            enemy.Position = position;
            enemies[i] = enemy;
        }

        this.EmitSignal(SignalName.CreateEnemies, enemies);
    }
}
