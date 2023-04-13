using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;

public partial class Player : CharacterBody2D
{
    [Export]
    public float MoveSpeed { get; set; } = 100f;
    public State CurrentState { get; set; } = State.Idle;
    [Export]
    public Camera2D Camera { get; set; }

    public Vector2 MoveDirection { get; set; } = new Vector2(0, 1);
    private AnimationTree animationTree;
    private Sprite2D sprite;
    private AnimationNodeStateMachinePlayback stateMachine;

    public override void _Ready()
    {
        this.animationTree = this.GetNode<AnimationTree>("AnimationTree");
        this.sprite = this.GetNode<Sprite2D>("Sprite2D");

        this.stateMachine = this.animationTree
            .Get("parameters/playback")
            .As<AnimationNodeStateMachinePlayback>();

        UpdateAnimationParameters();
    }

    public override void _PhysicsProcess(double delta)
    {
        SelectNewDirection();
        UpdateAnimationParameters();
        this.Velocity = MoveDirection * MoveSpeed;

        if (this.CurrentState == State.Walk)
        {
            this.Position += this.Velocity * (float)delta;

            this.Position = new Vector2(
                Mathf.Clamp(this.Position.X, 0, this.Camera.LimitRight),
                Mathf.Clamp(this.Position.Y, 0, this.Camera.LimitBottom)
            );
        }

        this.PickNewState();
    }

    private void SelectNewDirection()
    {
        this.MoveDirection = new Vector2(
            Input.GetActionStrength("Right") - Input.GetActionStrength("Left"),
            Input.GetActionStrength("Down") - Input.GetActionStrength("Up")
        );
    }

    private void UpdateAnimationParameters()
    {
        if (this.MoveDirection == Vector2.Zero)
        {
            return;
        }

        this.sprite.FlipH = this.MoveDirection.X == 1;

        animationTree.Set("parameters/Idle/blend_position", this.MoveDirection);
    }

    private void PickNewState() //TODO Arreglar de mejor manera los estados
    {
        switch (this.CurrentState)
        {
            case State.Idle:
                if (this.Velocity != Vector2.Zero)
                {
                    this.ChangeState(State.Walk);
                    GD.Print("Walk");
                }
                break;
            case State.Walk:
                if (this.Velocity == Vector2.Zero)
                {
                    this.ChangeState(State.Idle);
                    GD.Print("Idle");
                }
                break;
        }
    }

    private void ChangeState(State nextState)
    {
        switch (nextState)
        {
            case State.Idle:
                this.stateMachine.Travel(nextState.ToString());
                break;
            case State.Walk:
                this.stateMachine.Travel(nextState.ToString());
                break;
        }

        this.CurrentState = nextState;
    }

    public enum State
    {
        Idle,
        Walk
    }
}
