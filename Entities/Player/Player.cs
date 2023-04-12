using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;

public partial class Player : CharacterBody2D
{
    [Export]
    public float MoveSpeed { get; set; } = 100f;
    public State CurrentState { get; set; } = State.IDLE;

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

        if (this.CurrentState == State.WALK)
        {
            this.Position += this.Velocity.Normalized() * (float)delta;

            this.Position = new Vector2(
                Mathf.Clamp(this.Position.X, 0, this.GetViewportRect().Size.X),
                Mathf.Clamp(this.Position.Y, 0, this.GetViewportRect().Size.Y)
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

    private void PickNewState(State nextState) //TODO Arreglar de mejor manera los estados
    {
        switch (nextState)
        {
            case State.WALK:
        this.stateMachine.Travel(this.Velocity != Vector2.Zero ? "Walk" : "Idle");

        }
    }

    public enum State
    {
        IDLE,
        WALK
    }
}
