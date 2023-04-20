using Godot;

public partial class Player : CharacterBody2D
{
    [Export]
    public float MoveSpeed { get; set; } = 100f;

    [Export]
    public PlayerState CurrentState { get; set; } = PlayerState.Idle;

    [Export]
    public bool IsRooted { get; set; } = false;

    [Export]
    public double Life { get; set; } = 100;

    [Export]
    public float DashSpeed { get; set; } = 2f;

    private Camera2D Camera;

    private Vector2 moveDirection = new(0, 1);

    private AnimationTree animationTree;

    private AnimationPlayer animationPlayer;

    private Sprite2D sprite;

    private AnimationNodeStateMachinePlayback stateMachine;

    private Timer dasCooldownTimer;

    private bool isAttackAnimationDone = false;

    private bool isDashAnimationDone = false;

    private bool wantToWalk
    {
        get => this.moveDirection != Vector2.Zero;
    }
    private bool wantToIdle
    {
        get => this.moveDirection == Vector2.Zero;
    }

    private bool wantToAttack
    {
        get => Input.GetActionStrength("Attack") == 1;
    }

    private bool wantToDash
    {
        get => Input.GetActionStrength("Dash") == 1 && this.dasCooldownTimer.IsStopped();
    }

    public override void _Ready()
    {
        this.animationTree = this.GetNode<AnimationTree>("AnimationTree");
        this.animationPlayer = this.GetNode<AnimationPlayer>("AnimationPlayer");
        this.sprite = this.GetNode<Sprite2D>("Sprite2D");
        this.Camera = this.GetNode<Camera2D>("Camera2D");
        this.dasCooldownTimer = this.GetNode<Timer>("DashCooldownTimer");

        this.stateMachine = this.animationTree
            .Get("parameters/playback")
            .As<AnimationNodeStateMachinePlayback>();

        UpdateAnimationParameters();
    }

    public override void _PhysicsProcess(double delta)
    {
        SelectNewDirection();
        UpdateAnimationParameters();

        if (this.CurrentState == PlayerState.Walk || this.CurrentState == PlayerState.Dash)
        {
            this.Velocity = moveDirection.Normalized() * (MoveSpeed * (this.CurrentState == PlayerState.Dash ? DashSpeed : 1)); //TODO Cambiar los algoritmos de cada State a UpdateState antes de comprobar si se puede cambiar
            this.MoveAndSlide();
        }

        UpdateState();
    }

    private void SelectNewDirection()
    {
        var movX = Input.GetActionStrength("Right") - Input.GetActionStrength("Left");
        var movY = Input.GetActionStrength("Down") - Input.GetActionStrength("Up");

        this.moveDirection = new Vector2(movX, movY);
    }

    private void UpdateAnimationParameters()
    {
        this.sprite.FlipH = this.moveDirection.X == 1;

        animationTree.Set("parameters/Idle/blend_position", this.moveDirection);
    }

    public void UpdateState() //TODO Cambiar la state machine por el addons o rehacer parte del addon
    {
        switch (this.CurrentState)
        {
            case PlayerState.Idle:
                if (this.wantToWalk)
                {
                    this.ChangeState(PlayerState.Walk);
                    return;
                }
                if (this.wantToAttack)
                {
                    this.ChangeState(PlayerState.Attack);
                    return;
                }
                break;
            case PlayerState.Walk:
                if (this.wantToIdle)
                {
                    this.ChangeState(PlayerState.Idle);

                    return;
                }
                if (this.wantToAttack)
                {
                    this.ChangeState(PlayerState.Attack);

                    return;
                }

                if (this.wantToDash)
                {
                    this.ChangeState(PlayerState.Dash);

                    return;
                }
                break;
            case PlayerState.Attack:
                if (this.isAttackAnimationDone)
                {
                    if (this.wantToWalk)
                    {
                        this.ChangeState(PlayerState.Walk);

                        return;
                    }

                    if (this.wantToIdle)
                    {
                        this.ChangeState(PlayerState.Idle);

                        return;
                    }
                }
                break;
            case PlayerState.Dash:
                if (this.isDashAnimationDone)
                {
                    if (this.wantToWalk)
                    {
                        this.ChangeState(PlayerState.Walk);

                        return;
                    }

                    if (this.wantToIdle)
                    {
                        this.ChangeState(PlayerState.Idle);

                        return;
                    }
                }
                break;
        }
    }

    public void ChangeState(PlayerState nextState)
    {
        switch (nextState)
        {
            case PlayerState.Idle:
                this.stateMachine.Travel(nextState.ToString());
                break;
            case PlayerState.Walk:
                this.stateMachine.Travel(nextState.ToString());
                break;
            case PlayerState.Attack:
                this.stateMachine.Travel(nextState.ToString());
                break;
            case PlayerState.Dash:
                this.stateMachine.Travel(nextState.ToString());


                break;
        }

        this.CurrentState = nextState;
    }

    private void OnAniamtionPlayerStarted(StringName animName)
    {
        switch (animName)
        {
            case nameof(PlayerState.Attack):
                this.isAttackAnimationDone = false;
                break;
            case nameof(PlayerState.Dash):
                this.isDashAnimationDone = false;
                break;
        }
    }

    private void OnAnimationPlayerFinished(StringName animName)
    {
        switch (animName)
        {
            case nameof(PlayerState.Attack):
                this.isAttackAnimationDone = true;
                break;
            case nameof(PlayerState.Dash):
                this.isDashAnimationDone = true;
                this.dasCooldownTimer.Start();
                break;
        }
    }

    private void OnHurtBoxHurt(double damage)
    {
        this.Life -= damage;
        GD.Print(this.Life);
    }
}

public enum PlayerState
{
    Idle,
    Walk,
    Attack,
    Dash
}
