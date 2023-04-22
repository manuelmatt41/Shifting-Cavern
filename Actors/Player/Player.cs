using Godot;

public partial class Player : CharacterBody2D
{
    /// <summary>
    /// Velocidad de movimiento del jugador
    /// </summary>
    [Export]
    public float MoveSpeed { get; set; } = 100f;

    /// <summary>
    /// Vida del jugador
    /// </summary>
    [Export]
    public double Life { get; set; } = 100;

    /// <summary>
    /// Velocidad del movimiento Dash
    /// </summary>
    [Export]
    public float DashSpeed { get; set; } = 2f;

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
    private Sprite2D sprite;

    /// <summary>
    /// Maquina de estados para manejar los cambios entre ellos del personaje
    /// </summary>
    public AnimationNodeStateMachinePlayback AnimationStateMachineTree { get; set; }

    /// <summary>
    /// <c>Timer</c>  para esperar un tiempo determinado para volver ejecutar el movimiento Dash
    /// </summary>
    private Timer dashCooldownTimer;

    public DefaultStateMachine<Player, IState<Player>> DefaultStateMachine { get; set; }

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

    /// <summary>
    /// Funcion integrada de Godot que se ejecuta al crear el nodo en la escena, se usa para iniciar las variables de nodos subyacentes de <c>Player</c>
    /// </summary>
    public override void _Ready()
    {
        this.animationTree = this.GetNode<AnimationTree>("AnimationTree");
        this.sprite = this.GetNode<Sprite2D>("Sprite2D");
        this.Camera = this.GetNode<Camera2D>("Camera2D");
        this.dashCooldownTimer = this.GetNode<Timer>("DashCooldownTimer");

        this.AnimationStateMachineTree = this.animationTree
            .Get("parameters/playback")
            .As<AnimationNodeStateMachinePlayback>();

        this.DefaultStateMachine = new DefaultStateMachine<Player, IState<Player>>(this, IdleState.Instance());
    }

    /// <summary>
    /// Funcion integrada de Godot que se ejecuta 1 / 60 frames para trabajar con mas facilmente con las fisicas del juego, se encarga del movimiento del personaje y comprobar si quiere un nuevo estado
    /// </summary>
    /// <param name="delta">Valor del tiempo entre frames</param>
    public override void _PhysicsProcess(double delta)
    {
        this.SelectNewDirection();
        this.UpdateAnimationParameters();

        // Comprueba que esta en el  Walk o Dash
        if (this.DefaultStateMachine.CurrentState == WalkState.Instance() || this.DefaultStateMachine.CurrentState == DashState.Instance())
        {
            this.Velocity =
                moveDirection.Normalized()
                * (MoveSpeed * (this.DefaultStateMachine.CurrentState == DashState.Instance() ? DashSpeed : 1)); //TODO Cambiar los algoritmos de cada State a UpdateState antes de comprobar si se puede cambiar
            this.MoveAndSlide();
        }

        this.DefaultStateMachine.Update();
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
        this.sprite.FlipH = this.moveDirection.X == 1; // TODO Cuando se tenga los sprites adecuados no deberia hacer falta

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
        GD.Print(this.Life);
    }
}
