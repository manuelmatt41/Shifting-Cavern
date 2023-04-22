public class IdleState : PlayerState
{
    private static readonly IdleState idleState = new();

    private IdleState() { }

    public static IdleState Instance() => idleState;

    public void Enter(Player entity)
    {
        entity.AnimationStateMachineTree.Travel("Idle");
    }

    public void Exit(Player entity) { }

    public void Update(Player entity)
    {
        if (entity.WantToWalk)
        {
            entity.DefaultStateMachine.ChangeState(WalkState.Instance());
            return;
        }
        if (entity.WantToAttack)
        {
            entity.DefaultStateMachine.ChangeState(AttackState.Instance());
            return;
        }
    }
}
