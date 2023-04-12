using System;
using Godot;

public partial class Goblin : CharacterBody2D
{
    [Export]
    public float MoveSpeed { get; set; } = 50;

    public State CurrentState { get; set; } = State.WALK;
    public Vector2 MoveDirection { get; set; } = Vector2.Zero;

    private AnimationTree animationTree;
    private Sprite2D sprite;
    private AnimationNodeStateMachinePlayback stateMachine;
    private Timer timer;

    public override void _Ready()
    {
        this.animationTree = this.GetNode<AnimationTree>("AnimationTree");
        this.sprite = this.GetNode<Sprite2D>("Sprite2D");
        this.timer = this.GetNode<Timer>("Timer");

        this.stateMachine = this.animationTree
            .Get("parameters/playback")
            .As<AnimationNodeStateMachinePlayback>();

        this.SelectNewDirection();
        this.PickNewState(this.CurrentState);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (CurrentState == State.WALK)
        {
            this.Velocity = this.MoveSpeed * this.MoveDirection;

            UpdateAnimationParameters();

            this.MoveAndSlide();
        }
    }

    private void SelectNewDirection()
    {
        Random r = new();
        this.MoveDirection = new(r.Next(-1, 2), r.Next(-1, 2));
    }

    private void PickNewState(State nextState)
    {
        switch (nextState)
        {
            case State.WALK:
                this.stateMachine.Travel("Walk");
                this.CurrentState = State.WALK;
                this.timer.Start();
                break;
        }
    }

    private void UpdateAnimationParameters()
    {
        if (this.MoveDirection == Vector2.Zero)
        {
            return;
        }

        this.sprite.FlipH = this.MoveDirection.X == 1;

        animationTree.Set("parameters/Idle/blend_position", this.MoveDirection);
        animationTree.Set("parameters/Walk/blend_position", this.MoveDirection);
    }

    public void OnTimerTimeOut()
    {
        SelectNewDirection();
    }

    public enum State
    {
        WALK
    }
}
