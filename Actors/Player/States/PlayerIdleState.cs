public class PlayerIdleState : PlayerState
{
    private static readonly PlayerIdleState idleState = new();

    private PlayerIdleState() { }

    public static PlayerIdleState Instance() => idleState;

    public void Enter(Player entity)
    {
        entity.AnimationStateMachineTree.Travel("Idle");
    }

    public void Exit(Player entity) { }

    public void Update(Player entity)
    {
        if (entity.WantToWalk)
        {
            entity.DefaultStateMachine.ChangeState(PlayerWalkState.Instance());
            return;
        }
        if (entity.WantToAttack)
        {
            entity.DefaultStateMachine.ChangeState(PlayerAttackState.Instance());
            return;
        }
    }
}
