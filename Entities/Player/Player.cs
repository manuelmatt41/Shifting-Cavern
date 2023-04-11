using Godot;

public partial class Player : CharacterBody2D
{
    [Export]
    public float MoveSpeed { get; set; } = 100f;

    [Export]
    public Vector2 StartPosition { get; set; } = new Vector2(0, 1);
    private AnimationTree animationTree;

    public override void _Ready()
    {
        this.animationTree = this.GetNode<AnimationTree>("AnimationTree");
        animationTree.Set("parameters/Idle/blend_position", StartPosition);
    }

    public override void _PhysicsProcess(double delta)
    {
        var inputDirection = new Vector2(
            Input.GetActionStrength("Right") - Input.GetActionStrength("Left"),
            Input.GetActionStrength("Down") - Input.GetActionStrength("Up")
        );

        UpdateAnimationParameters(inputDirection);

        this.Velocity = inputDirection * MoveSpeed;
        this.MoveAndSlide();
    }

    public void UpdateAnimationParameters(Vector2 moveInto)
    {
        if (moveInto == Vector2.Zero) return;

        animationTree.Set("parameters/Idle/blend_position", moveInto);
        animationTree.Set("parameters/Walk/blend_position", moveInto);
    }
}
