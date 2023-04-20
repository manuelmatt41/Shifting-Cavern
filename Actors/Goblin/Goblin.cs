using System;
using Godot;

public partial class Goblin : CharacterBody2D
{
    [Export]
    public float MoveSpeed { get; set; } = 50;

    [Export]
    public State CurrentState { get; set; } = State.WALK;

    [Export]
    public Vector2 MoveDirection { get; set; } = Vector2.Zero;

    [Export]
    public int Life { get; set; } = 100;

    private Player _player;
    private AnimationTree animationTree;
    private Sprite2D sprite;
    private AnimationNodeStateMachinePlayback stateMachine;

    public override void _Ready()
    {
        this.animationTree = this.GetNode<AnimationTree>("AnimationTree");
        this.sprite = this.GetNode<Sprite2D>("Sprite2D");
        this._player = this.GetTree().GetFirstNodeInGroup("Player") as Player;

        this.stateMachine = this.animationTree
            .Get("parameters/playback")
            .As<AnimationNodeStateMachinePlayback>();

        this.PickNewState(this.CurrentState);
        //this.ChangeState(this.CurrentState);
    }

    public override void _PhysicsProcess(double delta)
    {
        this.MoveDirection = this.GlobalPosition.DirectionTo(this._player.GlobalPosition);

        if (CurrentState == State.WALK)
        {
            this.Velocity = this.MoveSpeed * this.MoveDirection;

            UpdateAnimationParameters();

            this.MoveAndSlide();
        }
    }

    private void PickNewState(State nextState)
    {
        switch (nextState)
        {
            case State.WALK:
                this.stateMachine.Travel("Walk");
                this.CurrentState = State.WALK;
                break;
        }
    }

    private void UpdateAnimationParameters()
    {
        this.sprite.FlipH = this.MoveDirection.X == 1;

        animationTree.Set("parameters/Walk/blend_position", this.MoveDirection);
    }

    public enum State
    {
        WALK
    }
}
