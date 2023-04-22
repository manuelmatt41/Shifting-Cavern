using Godot;

public class AttackState : PlayerState
{
    private static readonly AttackState attackState = new();

    private AttackState() { }

    public static AttackState Instance() => attackState;

    public void Enter(Player entity)
    {
        entity.AnimationStateMachineTree.Travel("Attack");

        var x = 16 * (entity.Sprite.FlipH ? 1 : -1);

        entity.HitBox.CollisionShape.Position = new Vector2(x, entity.HitBox.CollisionShape.Position.Y);
        entity.HitBox.CollisionShape.Disabled = false;
    }

    public void Exit(Player entity) { }

    public void Update(Player entity)
    {
        if (entity.IsAttackAnimationDone)
        {
            if (entity.WantToWalk)
            {
                entity.DefaultStateMachine.ChangeState(WalkState.Instance());
                entity.HitBox.CollisionShape.Position = new Vector2(0, entity.HitBox.CollisionShape.Position.Y);
                entity.HitBox.CollisionShape.Disabled = true;
                return;
            }

            if (entity.WantToIdle)
            {
                entity.DefaultStateMachine.ChangeState(IdleState.Instance());
                entity.HitBox.CollisionShape.Position = new Vector2(0, entity.HitBox.CollisionShape.Position.Y);
                entity.HitBox.CollisionShape.Disabled = true;

                return;
            }
        }
    }
}
