using Godot;

public partial class Goblin : CharacterBody2D
{
    /// <summary>
    /// Velocidad de movimiento del Goblin
    /// </summary>
    [Export]
    public float MoveSpeed { get; set; } = 50;

    /// <summary>
    /// Direccion de movimiento del Goblin
    /// </summary>
    [Export]
    public Vector2 MoveDirection { get; set; } = Vector2.Zero;

    /// <summary>
    /// Vida del Goblin
    /// </summary>
    [Export]
    public int Life { get; set; } = 100;

    /// <summary>
    /// Jugador que esta en la escena
    /// </summary>
    public Player Player { get; set; }

    /// <summary>
    /// Arbol que lleva las animacion del Goblin
    /// </summary>
    public AnimationTree AnimationTree { get; set; }

    /// <summary>
    /// Imagen que representa al Goblin y sus animaciones
    /// </summary>
    public Sprite2D Sprite { get; set; }

    /// <summary>
    /// Maquina de estados para manejar los cambios entre ellos del Goblin
    /// </summary>
    public AnimationNodeStateMachinePlayback AnimationStateMachineTree { get; set; }

    public DefaultStateMachine<Goblin, GoblinState> DefaultStateMachine { get; set; }

    /// <summary>
    /// Funcion integrada de Godot que se ejecuta al crear el nodo en la escena, se usa para iniciar las variables de nodos subyacentes de <c>Goblin</c>
    /// </summary>
    public override void _Ready()
    {
        this.AnimationTree = this.GetNode<AnimationTree>("AnimationTree");
        this.Sprite = this.GetNode<Sprite2D>("Sprite2D");
        this.Player = this.GetTree().GetFirstNodeInGroup("Player") as Player;

        this.AnimationStateMachineTree = this.AnimationTree
            .Get("parameters/playback")
            .As<AnimationNodeStateMachinePlayback>();

        this.DefaultStateMachine = new DefaultStateMachine<Goblin, GoblinState>(this, GoblinWalkState.Instance());
    }

    /// <summary>
    /// Funcion integrada de Godot que se ejecuta 1 / 60 frames para trabajar con mas facilmente con las fisicas del juego, se encarga del movimiento del Goblin y comprobar si quiere un nuevo estado
    /// </summary>
    /// <param name="delta">Valor del tiempo entre frames</param>
    public override void _PhysicsProcess(double delta)
    {
        //Coge la posicion en la escena del personaje y se posicion en direccion a el
        //this.MoveDirection = this.GlobalPosition.DirectionTo(this.Player.GlobalPosition);

        if (this.DefaultStateMachine.IsInState(GoblinWalkState.Instance()))
        {
            this.Velocity = this.MoveSpeed * this.MoveDirection;

            this.UpdateAnimationParameters();

            this.MoveAndSlide();
        }
    }

    /// <summary>
    /// Cambia la direccion de la animacion
    /// </summary>
    private void UpdateAnimationParameters()
    {
        this.Sprite.FlipH = this.MoveDirection.X == 1;

        AnimationTree.Set("parameters/Walk/blend_position", this.MoveDirection);
    }

    private void OnWardAreaChangeDirection(Vector2 newDirection)
    {
        this.MoveDirection = this.GlobalPosition.DirectionTo(newDirection);
    }

    private void OnWardAreaDetectPlayer(Vector2 playerDirection)
    {
        this.MoveDirection = this.GlobalPosition.DirectionTo(playerDirection);

    }
}

