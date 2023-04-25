using Godot;

public partial class Player : CharacterBody2D
{
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
    private Camera2D Camera;

    /// <summary>
    /// Direccion del movimiento del jugador
    /// </summary>
    private Vector2 moveDirection = new(0, 1);

    /// <summary>
    /// Arbol que lleva las animaciones del personaje
    /// </summary>
    private AnimationTree animationTree;

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
        get => this.moveDirection != Vector2.Zero;
    }

    /// <summary>
    /// Comprueba que el personaje quiere cambiar a <c>PlayerState.Idle</c>
    /// </summary>
    public bool WantToIdle
    {
        get => this.moveDirection == Vector2.Zero;
    }

    /// <summary>
    /// Comprueba que el personaje quiere cambiar a <c>PlayerState.Attack</c>
    /// </summary>
    public bool WantToAttack
    {
        get => Input.GetActionStrength("Attack") == 1;
    }

    /// <summary>
    /// Comprueba que el personaje quiere cambiar a <c>PlayerState.Dash</c>
    /// </summary>
    public bool WantToDash
    {
        get => Input.GetActionStrength("Dash") == 1 && this.dashCooldownTimer.IsStopped();
    }

    public bool AttackDirection { get; set; } = false;

    public double ViewportHalfWidth
    {
        get => GetViewportRect().Size.X * 0.5f;
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

        this.animationTree = this.GetNode<AnimationTree>("AnimationTree");
        this.AnimationStateMachineTree = this.animationTree
            .Get("parameters/playback")
            .As<AnimationNodeStateMachinePlayback>();
        this.DefaultStateMachine = new DefaultStateMachine<Player, PlayerState>(
            this,
            PlayerIdleState.Instance()
        );

        var terrainGenerator = (this.GetTree().GetFirstNodeInGroup("Map") as TerrainGenerator);
        this.Camera.LimitRight = terrainGenerator.Width * terrainGenerator.TileMap.CellQuadrantSize;
        this.Camera.LimitBottom = terrainGenerator.Height * terrainGenerator.TileMap.CellQuadrantSize;
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
            this.Sprite.FlipH = this.moveDirection.X == 1;
            this.Velocity =
                moveDirection.Normalized()
                * (
                    MoveSpeed
                    * (
                        this.DefaultStateMachine.CurrentState == PlayerDashState.Instance()
                            ? DashSpeed
                            : 1
                    )
                ); //TODO Cambiar los algoritmos de cada State a UpdateState antes de comprobar si se puede cambiar
            this.MoveAndSlide();
        }

        this.DefaultStateMachine.Update();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton eventMouse)
        {
            if (eventMouse.ButtonIndex == MouseButton.Left)
            {
                GD.Print(eventMouse.Position.DirectionTo(this.GlobalPosition).X);
                this.AttackDirection = eventMouse.Position.DirectionTo(this.GlobalPosition).X >= -1f; //TODO Revisar estos valores
            }
        }
    }

    /// <summary>
    /// Selecciona una nueva direccion de movimiento dependiendo de las teclas que se esten pulsando
    /// </summary>
    private void SelectNewDirection()
    {
        var movX = Input.GetActionStrength("Right") - Input.GetActionStrength("Left");
        var movY = Input.GetActionStrength("Down") - Input.GetActionStrength("Up");

        this.moveDirection = new Vector2(movX, movY);
    }

    /// <summary>
    /// Cambia la direccion de la animacion
    /// </summary>
    private void UpdateAnimationParameters()
    {
        // TODO Cuando se tenga los sprites adecuados no deberia hacer falta

        this.animationTree.Set("parameters/Idle/blend_position", this.moveDirection);
    }

    /// <summary>
    /// Evento que se ejecuta al comenzar una animacion
    /// </summary>
    /// <param name="animName">Nombre de la animacion</param>
    private void OnAniamtionPlayerStarted(StringName animName)
    {
        switch (animName)
        {
            case "Attack":
                this.IsAttackAnimationDone = false;
                break;
            case "Dash":
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
            case "Attack":
                this.IsAttackAnimationDone = true;
                break;
            case "Dash":
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
