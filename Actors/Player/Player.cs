using System.Collections.Generic;
using Godot;

/// <summary>
/// Clase que representa al jugador en el juego
/// </summary>
public partial class Player : CharacterBody2D
{
    /// <summary>
    /// Nombre de la animacion 'Attack' de <c>Player</c>
    /// </summary>
    public const string ATTACK_ANIMATION_NAME = "Attack";

    /// <summary>
    /// Nombre de la animacion 'Dash' de <c>Player</c>
    /// </summary>
    public const string DASH_ANIMATION_NAME = "Dash";

    /// <summary>
    /// Nombre de la animacion 'Walk' de <c>Player</c>
    /// </summary>
    public const string WALK_ANIMATION_NAME = "Walk";

    /// <summary>
    /// Nombre de la animacion 'Idle' de <c>Player</c>
    /// </summary>
    public const string IDLE_ANIMATION_NAME = "Idle";

    [Export]
    public PlayerResource PlayerResource { get; set; }

    ///// <summary>
    ///// Velocidad de movimiento de <c>Player</c> en px/s
    ///// </summary>
    ///// <value>Por defecto: 150</value>
    //[Export]
    //public float MoveSpeed { get; set; } = 150f;

    ///// <summary>
    ///// Vida de <c>Player</c>
    ///// </summary>
    ///// <value>Por defecto: 100</value>
    //[Export]
    //public double Life { get; set; } = 100;

    ///// <summary>
    ///// Fuerza aplicado al movimiento al hacer un Dash
    ///// </summary>
    ///// <value>Por defecto: 3</value>
    //[Export]
    //public float DashSpeed { get; set; } = 3f;

    //[Export]
    //public InventoryData InventoryData { get; set; }

    //[Export]
    //public EquipmentInventoryData EquipmentInventoryData { get; set; }

    /// <summary>
    /// Camara principal del juego que sigue a <c>Player</c>
    /// </summary>
    public Camera2D Camera { get; set; }

    /// <summary>
    /// Direccion del movimiento de <c>Player</c>
    /// </summary>
    /// <value>Por defecto: (0, 0) </value>
    public Vector2 MoveDirection { get; set; } = new(0, 0);

    /// <summary>
    /// Imagen que representa a <c>Player</c>
    /// </summary>
    public Sprite2D Sprite { get; private set; }

    /// <summary>
    /// Arbol de animaciones de <c>Player</c>
    /// </summary>
    public AnimationTree AnimationTree { get; set; }

    /// <summary>
    /// Maquina de estados de las animaciones de <c>Player</c>
    /// </summary>
    public AnimationNodeStateMachinePlayback AnimationStateMachineTree { get; private set; }

    /// <summary>
    /// Caja de colisiones para hacer danyo a otra entidad
    /// </summary>
    public HitBox HitBox { get; private set; }

    /// <summary>
    /// <c>Timer</c> para esperar un tiempo determinado para volver ejecutar el movimiento Dash
    /// </summary>
    public Timer DashCooldownTimer { get; private set; }

    /// <summary>
    /// Estado que al que se va cambiar en la maquina de estados
    /// </summary>
    /// <value>Intancia de <c>PlayerIdleState</c></value>
    public PlayerState NextState { get; set; } = PlayerIdleState.Instance();

    /// <summary>
    /// Valor que de si la animacion 'Attack' ha acabado
    /// </summary>
    /// <value><c>True</c> si la animacion acabo, sino <c>false</c></value>
    public bool IsAttackAnimationDone = false;

    /// <summary>
    /// Valor que de si la animacion 'Dash' ha acabado
    /// </summary>
    /// <value><c>True</c> si la animacion acabo, sino <c>false</c></value>
    public bool IsDashAnimationDone = false;

    /// <summary>
    /// Comprueba que se quiere cambiar el estado a <c>PlayerWalkState</c>
    /// </summary>
    /// <value><c>True</c> si <c>MoveDirection</c> es distinto de 0 sino <c>false</c></value>
    public bool WantToWalk
    {
        get =>
            Input.IsActionPressed("Right")
            || Input.IsActionPressed("Left")
            || Input.IsActionPressed("Up")
            || Input.IsActionPressed("Down");
    }

    /// <summary>
    /// Comprueba que se quiere cambiar el estado a <c>PlayerIdleState</c>
    /// </summary>
    /// <value><c>True</c> si <c>MoveDirection</c> es igual a 0 sino <c>false</c></value>
    public bool WantToIdle
    {
        get => this.MoveDirection == Vector2.Zero;
    }

    /// <summary>
    /// Comprueba que se quiere cambiar el estado a <c>PlayerAttackState</c>
    /// </summary>
    /// <value><c>True</c> si ha pulsado el click izquierdo del raton sino <c>false</c></value>
    public bool WantToAttack
    {
        get => Input.IsActionJustPressed("Attack") && !this.IsInventoryVisible;
    }

    /// <summary>
    /// Comprueba que se quiere cambiar el estado a <c>PlayerDashState</c>
    /// </summary>
    /// <value><c>True</c> si ha pulsado el espacio y que se haya parado <c>DashCooldownTimer</c> sino <c>false</c></value>
    public bool WantToDash
    {
        get => Input.GetActionStrength("Dash") == 1 && this.DashCooldownTimer.IsStopped();
    }

    public bool IsInventoryVisible { get; set; } //TODO Rehacer codigo

    /// <summary>
    /// Maquina de estados de <c>Player</c>
    /// </summary>
    private DefaultStateMachine<Player, PlayerState> _defaultStateMachine;

    /// <summary>
    /// Funcion integrada de Godot que se ejecuta al crear el nodo en la escena, se usa para iniciar las variables de nodos subyacentes de <c>Player</c>
    /// </summary>
    public override void _Ready()
    {
        this.Sprite = this.GetNode<Sprite2D>("Sprite2D");

        this.Camera = this.GetNode<Camera2D>("Camera2D");

        this.DashCooldownTimer = this.GetNode<Timer>("DashCooldownTimer");

        this.HitBox = this.GetNode<HitBox>("HitBox");

        this.AnimationTree = this.GetNode<AnimationTree>("AnimationTree");

        this.AnimationStateMachineTree = this.AnimationTree
            .Get("parameters/playback")
            .As<AnimationNodeStateMachinePlayback>();

        this._defaultStateMachine = new(this, this.NextState);

    }

    public void Initali()
    {
        if (this.PlayerResource.EquipmentInventory.SlotDatas[0] != null)
        {
            this.HitBox.Damage = (
                this.PlayerResource.EquipmentInventory.SlotDatas[0].ItemData as WeaponItemData
            ).Damage;
        }
        else
        {
            this.HitBox.Damage = 100; ///TODO SOlo para pruebas
        }
    }

    /// <summary>
    /// Funcion integrada de Godot que se ejecuta 1 / 60 frames para trabajar con mas facilmente con las fisicas del juego, comprobar si quiere un nuevo estado y cambiarlo si <c>NextState</c> es diferente al actual de la <c>_defaultStateMachine</c>
    /// </summary>
    /// <param name="delta">Valor del tiempo entre frames</param>
    public override void _PhysicsProcess(double delta)
    {
        this._defaultStateMachine.Update();

        if (!this._defaultStateMachine.IsInState(this.NextState))
        {
            this._defaultStateMachine.ChangeState(this.NextState);
        }
    }

    /// <summary>
    /// Realiza el movimiento de <c>Player</c> y se llama a una pool de sonidos de caminar <c>PlayRandomPlayerWalkSound</c>
    /// </summary>
    public void DoWalk()
    {
        //SoundManager.Instance.PlayRandomPlayerWalkSound();

        this.SelectNewDirection();

        this.Sprite.FlipH = this.MoveDirection.X == 1;

        //La velocidad se calcula con la direccion de movimiento normalizada por la velocidad aplicando un fuerza a mayores si se ha ejecutado el dash
        this.Velocity = this.MoveDirection.Normalized() * this.PlayerResource.MoveSpeed;
        this.MoveAndSlide();
    }

    /// <summary>
    /// Realiza el movimiento Dash de <c>Player</c>
    /// </summary>
    public void DoDash()
    {
        this.Sprite.FlipH = this.MoveDirection.X == 1;

        //La velocidad se calcula con la direccion de movimiento normalizada por la velocidad aplicando un fuerza a mayores si se ha ejecutado el dash
        this.Velocity =
            this.MoveDirection.Normalized()
            * this.PlayerResource.MoveSpeed
            * this.PlayerResource.DashSpeed;
        this.MoveAndSlide();
    }

    /// <summary>
    /// Realiza el ataque del jugador
    /// </summary>
    public void DoAttack()
    {
        // Comprueba la posicion del raton en relacion de la posicion global, -1 (izquierda de la pantalla) y 1 (derecha)
        var attackDirection = this.GlobalPosition.X > this.GetGlobalMousePosition().X ? -1 : 1;

        // Calcula la posicion de la HitBox al golpear
        var x = 16 * attackDirection; //TODO Cambiar el 16 por un valor Range vinculado al tipo de arma que se este utilizando
        var newHitBoxPosition = new Vector2(x, this.HitBox.CollisionShape2D.Position.Y);

        this.Sprite.FlipH = attackDirection != -1;
        this.HitBox.CollisionShape2D.Position = newHitBoxPosition;
        this.HitBox.CollisionShape2D.Disabled = false;
    }

    /// <summary>
    /// Resetea los paremtros de ataque
    /// </summary>
    public void ResetAttack()
    {
        var resetHitBoxPosition = new Vector2(0, this.HitBox.CollisionShape2D.Position.Y);

        this.HitBox.CollisionShape2D.Disabled = true;
        this.HitBox.CollisionShape2D.Position = resetHitBoxPosition;
    }

    /// <summary>
    /// Selecciona una nueva direccion de movimiento dependiendo de las teclas que se esten pulsando
    /// </summary>
    public void SelectNewDirection()
    {
        var movX = Input.GetActionStrength("Right") - Input.GetActionStrength("Left");
        var movY = Input.GetActionStrength("Down") - Input.GetActionStrength("Up");

        this.MoveDirection = new Vector2(movX, movY);
    }

    /// <summary>
    /// Cambia la direccion de la animacion
    /// </summary>
    //private void UpdateAnimationParameters() //Revisar si me compensa hacer animaciones para todas las direcciones
    //{
    //this.animationTree.Set("parameters/Idle/blend_position", this.MoveDirection);
    //}

    /// <summary>
    /// Evento que se ejecuta al comenzar una animacion
    /// </summary>
    /// <param name="animName">Nombre de la animacion</param>
    private void OnAniamtionPlayerStarted(StringName animName)
    {
        switch (animName)
        {
            case ATTACK_ANIMATION_NAME:
                this.IsAttackAnimationDone = false;
                break;
            case DASH_ANIMATION_NAME:
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
            case ATTACK_ANIMATION_NAME:
                this.IsAttackAnimationDone = true;
                break;
            case DASH_ANIMATION_NAME:
                this.IsDashAnimationDone = true;
                break;
        }
    }

    /// <summary>
    /// Evento que ejecuta al recibir danyo
    /// </summary>
    /// <param name="damage">Danyo recibido</param>
    private void OnHurtBoxHurt(double damage)
    {
        this.PlayerResource.Life -= damage;
    }
}
