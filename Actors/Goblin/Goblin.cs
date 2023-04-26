using Godot;

public partial class Goblin : CharacterBody2D
{
    /// <summary>
    /// Velocidad de movimiento del Goblin
    /// </summary>
    [Export]
    public float MoveSpeed { get; set; } = 100;

    /// <summary>
    /// Direccion de movimiento del Goblin
    /// </summary>
    [Export]
    public Vector2 MoveDirection { get; set; } = Vector2.Zero;

    public Vector2 FinishPosition { get; set; }

    /// <summary>
    /// Vida del Goblin
    /// </summary>
    [Export]
    public double Life { get; set; } = 100;

    /// <summary>
    /// Arbol que lleva las animacion del Goblin
    /// </summary>
    public AnimationTree AnimationTree { get; set; }

    /// <summary>
    /// Imagen que representa al Goblin y sus animaciones
    /// </summary>
    public Sprite2D Sprite { get; set; }

    public HurtBox HurtBox { get; set; }

    /// <summary>
    /// Maquina de estados para manejar los cambios entre ellos del Goblin
    /// </summary>
    public AnimationNodeStateMachinePlayback AnimationStateMachineTree { get; set; }

    public DefaultStateMachine<Goblin, GoblinState> DefaultStateMachine { get; set; }

    public GoblinState NextState { get; set; }
    public bool WantToIdle
    {
        get => this.GlobalPosition.DistanceSquaredTo(this.FinishPosition) < 1f;
    }
    public bool WantToWalk
    {
        get => this.GlobalPosition.DistanceSquaredTo(this.FinishPosition) >= 1f;
    }

    public bool IsHitAnimationDone { get; private set; }

    /// <summary>
    /// Funcion integrada de Godot que se ejecuta al crear el nodo en la escena, se usa para iniciar las variables de nodos subyacentes de <c>Goblin</c>
    /// </summary>
    public override void _Ready()
    {
        this.AnimationTree = this.GetNode<AnimationTree>("AnimationTree");
        this.Sprite = this.GetNode<Sprite2D>("Sprite2D");
        this.FinishPosition = this.GlobalPosition;
        this.AnimationStateMachineTree = this.AnimationTree
            .Get("parameters/playback")
            .As<AnimationNodeStateMachinePlayback>();
        this.HurtBox = this.GetNode<HurtBox>("HurtBox");

        this.DefaultStateMachine = new DefaultStateMachine<Goblin, GoblinState>(
            this,
            GoblinIdleState.Instance()
        );
    }

    /// <summary>
    /// Funcion integrada de Godot que se ejecuta 1 / 60 frames para trabajar con mas facilmente con las fisicas del juego, se encarga del movimiento del Goblin y comprobar si quiere un nuevo estado
    /// </summary>
    /// <param name="delta">Valor del tiempo entre frames</param>
    public override void _PhysicsProcess(double delta)
    {
        if (this.DefaultStateMachine.IsInState(GoblinWalkState.Instance()))
        {
            this.Sprite.FlipH = this.MoveDirection.X < 0;

            this.Velocity = this.MoveSpeed * this.MoveDirection;

            this.MoveAndSlide();
        }

        this.DefaultStateMachine.Update();

        if (this.NextState != null && !this.DefaultStateMachine.IsInState(this.NextState))
        {
            this.DefaultStateMachine.ChangeState(this.NextState);
        }
    }

    /// <summary>
    /// Cambia la direccion de la animacion
    /// </summary>
    //private void UpdateAnimationParameters()
    //{
    //    //this.Sprite.FlipH = this.MoveDirection.X == 1;

    //}

    private void OnWardAreaChangeDirection(Vector2 newDirection)
    {
        this.MoveDirection = this.GlobalPosition.DirectionTo(newDirection);
        this.FinishPosition = newDirection;
    }

    /// <summary>
    /// Evento que se ejecuta al comenzar una animacion
    /// </summary>
    /// <param name="animName">Nombre de la animacion</param>
    private void OnAniamtionPlayerStarted(StringName animName)
    {
        switch (animName)
        {
            case "Hit":
                this.IsHitAnimationDone = false;
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
            case "Hit":
                this.IsHitAnimationDone = true;
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
        this.NextState = GoblinHitState.Instance();
        if (this.Life <= 0)
        {
            SoundManager.Instance.PlayGoblinDeadSound();
            this.QueueFree();
        }
    }
}
