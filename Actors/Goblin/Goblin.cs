using Godot;

/// <summary>
/// Clase que representa a un Goblin en el juego
/// </summary>
public partial class Goblin : CharacterBody2D
{
    /// <summary>
    /// Nombre de la animacion 'Hit' de <c>Goblin</c>
    /// </summary>
    public const string HIT_ANIMATION_NAME = "Hit";

    /// <summary>
    /// Nombre de la animacion 'Walk' de <c>Goblin</c>
    /// </summary>
    public const string WALK_ANIMATION_NAME = "Walk";

    /// <summary>
    /// Nombre de la animacion 'Idle' de <c>Goblin</c>
    /// </summary>
    public const string IDLE_ANIMATION_NAME = "Idle";

    [Signal]
    public delegate void DropLootEventHandler(SlotData[] loot, Vector2 position);

    /// <summary>
    /// Velocidad de movimiento de <c>Goblin</c>
    /// </summary>
    /// <value>Por defecto: 100</value>
    [Export]
    public float MoveSpeed { get; set; } = 100;

    /// <summary>
    /// Direccion de movimiento de <c>Goblin</c>
    /// </summary>
    /// <value>Por defecto: (0,0)</value>
    [Export]
    public Vector2 MoveDirection { get; set; } = Vector2.Zero;

    /// <summary>
    /// Vida de <c>Goblin</c>
    /// </summary>
    /// <value>Por defecto: 100</value>
    [Export]
    public double Life { get; set; } = 100;

    /// <summary>
    /// Tiempo que tarda en mandar una nueva direccion
    /// </summary>
    [Export]
    public double ChangeDirectionTime { get; private set; } = 5;

    public GoblinLootTable LootTable { get; set; } = new();

    /// <summary>
    /// Arbol de animaciones de <c>Goblin</c>
    /// </summary>
    public AnimationTree AnimationTree { get; private set; }

    /// <summary>
    /// Imagen que representa a <c>Goblin</c>
    /// </summary>
    public Sprite2D Sprite { get; private set; }

    /// <summary>
    /// Caja de colisiones para detectar danyo de otra entidad
    /// </summary>
    public HurtBox HurtBox { get; private set; }

    /// <summary>
    /// Maquina de estados de las animaciones de <c>Goblin</c>
    /// </summary>
    public AnimationNodeStateMachinePlayback AnimationStateMachineTree { get; private set; }

    public WardArea WardArea { get; private set; }

    /// <summary>
    /// Estado que al que se va cambiar en la maquina de estados
    /// </summary>
    /// <value>Intancia de <c>GoblinIdleState</c></value>
    public GoblinState NextState { get; set; } = GoblinIdleState.Instance();

    /// <summary>
    /// Comprueba que se quiere cambiar el estado a <c>GoblinIdleState</c>
    /// </summary>
    /// <value><c>True</c> si <c>Goblin</c> ha llegado a <c>_finishPosition</c>, <c>false</c> si no ha llegado</value>
    public bool WantToIdle
    {
        get => this.GlobalPosition.DistanceSquaredTo(this._finishPosition) < 1f;
    }

    /// <summary>
    /// Comprueba que se quiere cambiar el estado a <c>GoblinWalkState</c>
    /// </summary>
    /// <value><c>True</c> si <c>Goblin</c> no ha llegado a <c>_finishPosition</c>, <c>false</c> si  ha llegado</value>
    public bool WantToWalk
    {
        get =>
            this.WardArea.IsPlayerInside
            || this.GlobalPosition.DistanceSquaredTo(this._finishPosition) >= 1f;
    }

    /// <summary>
    /// Valor que de si la animacion 'Hit' ha acabado
    /// </summary>
    /// <value><c>True</c> si la animacion acabo, sino <c>false</c></value>
    public bool IsHitAnimationDone { get; private set; }

    /// <summary>
    /// Maquina de estados de <c>Goblin</c>
    /// </summary>
    private DefaultStateMachine<Goblin, GoblinState> _defaultStateMachine;

    /// <summary>
    /// Posicion a la que se movera el Goblin
    /// </summary>
    private Vector2 _finishPosition;

    /// <summary>
    /// Genera numeros aleatorios
    /// </summary>
    private RandomNumberGenerator _randomNumberGenerator = new();

    /// <summary>
    /// Contador del tiempo para volver a mandar una direccion
    /// </summary>
    private double _changeDirectionTimeCount = 0;

    /// <summary>
    /// Funcion integrada de Godot que se ejecuta al crear el nodo en la escena, se usa para iniciar las variables de <c>Goblin</c>
    /// </summary>
    public override void _Ready()
    {
        this.AnimationTree = this.GetNode<AnimationTree>("AnimationTree");

        this.Sprite = this.GetNode<Sprite2D>("Sprite2D");

        this.WardArea = this.GetNode<WardArea>("WardArea");

        this._finishPosition = this.GlobalPosition;

        this.AnimationStateMachineTree = this.AnimationTree
            .Get("parameters/playback")
            .As<AnimationNodeStateMachinePlayback>();

        this.HurtBox = this.GetNode<HurtBox>("HurtBox");

        this._defaultStateMachine = new DefaultStateMachine<Goblin, GoblinState>(
            this,
            this.NextState
        );
    }

    /// <summary>
    /// Funcion integrada de Godot que se ejecuta 1 / 60 frames para trabajar con mas facilmente con las fisicas del juego, comprobueba si quiere un nuevo estado y lo cambia si <c>NextState</c> es diferente al actual en la <c>DefaultStateMAchine</c>
    /// </summary>
    /// <param name="delta">Valor del tiempo entre frames de fisicas</param>
    public override void _PhysicsProcess(double delta)
    {
        if (
            this._defaultStateMachine.IsInState(GoblinIdleState.Instance())
            && !this.WardArea.IsPlayerInside
        )
        {
            this._changeDirectionTimeCount += delta;

            if (this._changeDirectionTimeCount >= this.ChangeDirectionTime)
            {
                this.PickNewDirection();
                this._changeDirectionTimeCount = 0;
            }
        }

        this._defaultStateMachine.Update();

        if (!this._defaultStateMachine.IsInState(this.NextState))
        {
            this._defaultStateMachine.ChangeState(this.NextState);
        }
    }

    /// <summary>
    /// Realzia el movimiento de <c>Goblin</c>
    /// </summary>
    public void DoWalk()
    {
        this.Sprite.FlipH = this.MoveDirection.X < 0;

        this.Velocity = this.MoveSpeed * this.MoveDirection;

        this.MoveAndSlide();
    }

    private void PickNewDirection()
    {
        var halfWidth = (this.WardArea.AreaSize.X / 2);
        var halfHeight = (this.WardArea.AreaSize.Y / 2);

        var minX = (int)(this.WardArea.GlobalPosition.X - halfWidth);
        var minY = (int)(this.WardArea.GlobalPosition.Y - halfHeight);
        var maxX = (int)(this.WardArea.GlobalPosition.X + halfWidth);
        var maxY = (int)(this.WardArea.GlobalPosition.Y + halfHeight);

        var newDirection = new Vector2(
            this._randomNumberGenerator.RandiRange(minX, maxX),
            this._randomNumberGenerator.RandiRange(minY, maxY)
        );

        this.MoveDirection = this.GlobalPosition.DirectionTo(newDirection);
        this._finishPosition = newDirection;
    }

    /// <summary>
    /// Evento que se ejecuta al detectar una posicion aleatoria dentro de <c>WardArea</c> o la entrada de un <c>Player</c> dentro del area, y se cambia <c>MoveDirection</c> y <c>_finishPosition</c> a la nueva posicion
    /// </summary>
    /// <param name="playerDirection">Direccion obtenida del evento</param>
    private void OnDetectPlayer(Vector2 playerDirection)
    {
        this.MoveDirection = this.GlobalPosition.DirectionTo(playerDirection);
        this._finishPosition = playerDirection;
    }

    /// <summary>
    /// Evento que se ejecuta al comenzar una animacion
    /// </summary>
    /// <param name="animName">Nombre de la animacion</param>
    private void OnAniamtionPlayerStarted(StringName animName)
    {
        switch (animName)
        {
            case HIT_ANIMATION_NAME:
                this.IsHitAnimationDone = false;
                break;
        }
    }

    /// <summary>
    /// Evento que se ejecuta al terminar una animacion, no funciona para animaciones LOOP
    /// </summary>
    /// <param name="animName">Nombre de la animacion</param>
    private void OnAnimationPlayerFinished(StringName animName)
    {
        switch (animName)
        {
            case HIT_ANIMATION_NAME:
                this.IsHitAnimationDone = true;
                break;
        }
    }

    /// <summary>
    /// Evento que ejecuta al recibir danyo, reduciendo la vida de <c>Goblin</c> y cambiando a <c>GoblinHitState</c>, luego comprueba si no le queda vida a <c>Goblin</c> y emite el sonido <c>PlayGoblinDeadSound</c> y lo elimina soltando los items de su inventario de forma aleatoria.
    /// </summary>
    /// <param name="damage">Danyo recibido</param>
    private void OnHurtBoxHurt(double damage)
    {
        this.Life -= damage;
        this.NextState = GoblinHitState.Instance();

        if (this.Life <= 0)
        {
            SoundManager.Instance.PlayGoblinDeadSound();
            this.EmitSignal(
                SignalName.DropLoot,
                this.LootTable.DropRandomSlotDatas(),
                this.GlobalPosition
            );
            this.QueueFree();
        }
    }
}
