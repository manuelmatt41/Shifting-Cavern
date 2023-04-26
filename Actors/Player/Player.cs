using Godot;

public partial class Player : CharacterBody2D
{
    private const string AnimationAttackName = "Attack";
    private const string AnimationDashName = "Dash";

    /// <summary>
    /// Velocidad de movimiento del jugador
    /// </summary>
    [Export]
    public float MoveSpeed { get; set; } = 150f;

    /// <summary>
    /// Vida del jugador
    /// </summary>
    [Export]
    public double Life { get; set; } = 100;

    /// <summary>
    /// Velocidad del movimiento Dash
    /// </summary>
    [Export]
    public float DashSpeed { get; set; } = 3f;

    /// <summary>
    /// Camara principal del juego
    /// </summary>
    public Camera2D Camera { get; set; }

    /// <summary>
    /// Direccion del movimiento del jugador
    /// </summary>
    public Vector2 MoveDirection { get; set; } = new(0, 1);

    /// <summary>
    /// Imagen que representa al personaje con sus animaciones
    /// </summary>
    public Sprite2D Sprite { get; set; }

    /// <summary>
    /// Maquina de estados para manejar los cambios entre ellos del personaje
    /// </summary>
    public AnimationNodeStateMachinePlayback AnimationStateMachineTree { get; set; }

    public HitBox HitBox { get; set; }

    public CollisionShape2D CollisionShape2D { get; set; }

    /// <summary>
    /// <c>Timer</c>  para esperar un tiempo determinado para volver ejecutar el movimiento Dash
    /// </summary>
    private Timer dashCooldownTimer;

    public DefaultStateMachine<Player, PlayerState> DefaultStateMachine { get; set; }

    public PlayerState NextState { get; set; }

    /// <summary>
    /// Valor que si esta <c>true</c> indica que la animacion de Ataque a terminado y si no es <c>false</c>
    /// </summary>
    public bool IsAttackAnimationDone = false;

    /// <summary>
    /// Valor que si esta <c>true</c> indica que la animacion de Dash a terminado y si no es <c>false</c>
    /// </summary>
    public bool IsDashAnimationDone = false;

    /// <summary>
    /// Comprueba que el personaje quiere cambiar a <c>PlayerState.Walk</c>
    /// </summary>
    public bool WantToWalk
    {
        get => this.MoveDirection != Vector2.Zero;
    }

    /// <summary>
    /// Comprueba que el personaje quiere cambiar a <c>PlayerState.Idle</c>
    /// </summary>
    public bool WantToIdle
    {
        get => this.MoveDirection == Vector2.Zero;
    }

    /// <summary>
    /// Comprueba que el personaje quiere cambiar a <c>PlayerState.Attack</c>
    /// </summary>
    public bool WantToAttack
    {
        get => Input.IsActionJustPressed("Attack");
    }

    /// <summary>
    /// Comprueba que el personaje quiere cambiar a <c>PlayerState.Dash</c>
    /// </summary>
    public bool WantToDash
    {
        get => Input.GetActionStrength("Dash") == 1 && this.dashCooldownTimer.IsStopped();
    }

    public Vector2 MapSize { get; set; }

    /// <summary>
    /// Funcion integrada de Godot que se ejecuta al crear el nodo en la escena, se usa para iniciar las variables de nodos subyacentes de <c>Player</c>
    /// </summary>
    public override void _Ready()
    {
        this.Sprite = this.GetNode<Sprite2D>("Sprite2D");
        this.Camera = this.GetNode<Camera2D>("Camera2D");
        this.dashCooldownTimer = this.GetNode<Timer>("DashCooldownTimer");
        this.HitBox = this.GetNode<HitBox>("HitBox");
        this.CollisionShape2D = this.GetNode<CollisionShape2D>("CollisionShape2D");

        var animationTree = this.GetNode<AnimationTree>("AnimationTree");
        this.AnimationStateMachineTree = animationTree
            .Get("parameters/playback")
            .As<AnimationNodeStateMachinePlayback>();
        this.DefaultStateMachine = new DefaultStateMachine<Player, PlayerState>(
            this,
            PlayerIdleState.Instance()
        );

        var terrainGenerator = (this.GetTree().GetFirstNodeInGroup("Map") as TerrainGenerator); //TODO Mejorar este codigo
        if (terrainGenerator != null)
        {
            this.Camera.LimitLeft = 0;
            this.Camera.LimitTop = 0;
            this.Camera.LimitRight =
                terrainGenerator.Width * terrainGenerator.TileMap.CellQuadrantSize;
            this.Camera.LimitBottom =
                terrainGenerator.Height * terrainGenerator.TileMap.CellQuadrantSize;
        }
    }

    /// <summary>
    /// Funcion integrada de Godot que se ejecuta 1 / 60 frames para trabajar con mas facilmente con las fisicas del juego, se encarga del movimiento del personaje y comprobar si quiere un nuevo estado
    /// </summary>
    /// <param name="delta">Valor del tiempo entre frames</param>
    public override void _PhysicsProcess(double delta)
    {
        if (!this.DefaultStateMachine.IsInState(PlayerDashState.Instance()))
        {
            this.SelectNewDirection();
        }

        // Comprueba que esta en el  Walk o Dash
        if (
            this.DefaultStateMachine.CurrentState == PlayerWalkState.Instance()
            || this.DefaultStateMachine.CurrentState == PlayerDashState.Instance()
        )
        {
            this.Sprite.FlipH = this.MoveDirection.X == 1;
            //La velocidad se calcula con la direccion de movimiento normalizada por la velocidad aplicando un fuerza a mayores si se ha ejecutado el dash
            this.Velocity =
                MoveDirection.Normalized()
                * this.MoveSpeed
                * (this.DefaultStateMachine.IsInState(PlayerDashState.Instance()) ? DashSpeed : 1);
            this.MoveAndSlide();
        }

        this.DefaultStateMachine.Update();

        if (this.NextState != null && !this.DefaultStateMachine.IsInState(this.NextState))
        {
            this.DefaultStateMachine.ChangeState(this.NextState);
        }
    }

    /// <summary>
    /// Selecciona una nueva direccion de movimiento dependiendo de las teclas que se esten pulsando
    /// </summary>
    private void SelectNewDirection()
    {
        var movX = Input.GetActionStrength("Right") - Input.GetActionStrength("Left");
        var movY = Input.GetActionStrength("Down") - Input.GetActionStrength("Up");

        this.MoveDirection = new Vector2(movX, movY);
    }

    /// <summary>
    /// Cambia la direccion de la animacion
    /// </summary>
    private void UpdateAnimationParameters() //Revisar si me compensa hacer animaciones para todas las direcciones
    {
        //this.animationTree.Set("parameters/Idle/blend_position", this.MoveDirection);
    }

    /// <summary>
    /// Evento que se ejecuta al comenzar una animacion
    /// </summary>
    /// <param name="animName">Nombre de la animacion</param>
    private void OnAniamtionPlayerStarted(StringName animName)
    {
        switch (animName)
        {
            case AnimationAttackName:
                this.IsAttackAnimationDone = false;
                break;
            case AnimationDashName:
                this.IsDashAnimationDone = false;
                break;
        }
    }

    /// <summary>
    /// Evento que se ejecuta al terminar una animacion, no funciona para animaciones LOOP
    /// </summary>
    /// <param name="animName"></param>
    private void OnAnimationPlayerFinished(StringName animName)
    {
        switch (animName)
        {
            case AnimationAttackName:
                this.IsAttackAnimationDone = true;
                break;
            case AnimationDashName:
                this.IsDashAnimationDone = true;
                this.dashCooldownTimer.Start();
                break;
        }
    }

    /// <summary>
    /// Evento que ejecuta al recibir danyo
    /// </summary>
    /// <param name="damage">Danyo recibido</param>
    private void OnHurtBoxHurt(double damage)
    {
        this.Life -= damage;
    }
}
