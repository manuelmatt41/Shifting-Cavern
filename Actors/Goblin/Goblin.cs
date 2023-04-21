using Godot;

public partial class Goblin : CharacterBody2D
{
    /// <summary>
    /// Velocidad de movimiento del Goblin
    /// </summary>
    [Export]
    public float MoveSpeed { get; set; } = 50;
    /// <summary>
    /// Estado actual del Goblin
    /// </summary>
    [Export]
    public GoblinState CurrentState { get; set; } = GoblinState.WALK;
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
    private Player player;
    /// <summary>
    /// Arbol que lleva las animacion del Goblin
    /// </summary>
    private AnimationTree animationTree;
    /// <summary>
    /// Imagen que representa al Goblin y sus animaciones
    /// </summary>
    private Sprite2D sprite;
    /// <summary>
    /// Maquina de estados para manejar los cambios entre ellos del Goblin
    /// </summary>
    private AnimationNodeStateMachinePlayback stateMachine;
    /// <summary>
    /// Funcion integrada de Godot que se ejecuta al crear el nodo en la escena, se usa para iniciar las variables de nodos subyacentes de <c>Goblin</c>
    /// </summary>
    public override void _Ready()
    {
        this.animationTree = this.GetNode<AnimationTree>("AnimationTree");
        this.sprite = this.GetNode<Sprite2D>("Sprite2D");
        this.player = this.GetTree().GetFirstNodeInGroup("Player") as Player;

        this.stateMachine = this.animationTree
            .Get("parameters/playback")
            .As<AnimationNodeStateMachinePlayback>();

        this.PickNewState(this.CurrentState);
    }
    /// <summary>
    /// Funcion integrada de Godot que se ejecuta 1 / 60 frames para trabajar con mas facilmente con las fisicas del juego, se encarga del movimiento del Goblin y comprobar si quiere un nuevo estado
    /// </summary>
    /// <param name="delta">Valor del tiempo entre frames</param>
    public override void _PhysicsProcess(double delta)
    {
        //Coge la posicion en la escena del personaje y se posicion en direccion a el
        this.MoveDirection = this.GlobalPosition.DirectionTo(this.player.GlobalPosition);

        if (CurrentState == GoblinState.WALK)
        {
            this.Velocity = this.MoveSpeed * this.MoveDirection;

            this.UpdateAnimationParameters();

            this.MoveAndSlide();
        }
    }

    private void PickNewState(GoblinState nextState) //TODO Rehacer estados del goblin
    {
        switch (nextState)
        {
            case GoblinState.WALK:
                this.stateMachine.Travel("Walk");
                this.CurrentState = GoblinState.WALK;
                break;
        }
    }
    /// <summary>
    /// Cambia la direccion de la animacion
    /// </summary>
    private void UpdateAnimationParameters()
    {
        this.sprite.FlipH = this.MoveDirection.X == 1;

        animationTree.Set("parameters/Walk/blend_position", this.MoveDirection);
    }

}

/// <summary>
/// Estados que puede estar el Goblin
/// </summary>
public enum GoblinState
{
    WALK
}
