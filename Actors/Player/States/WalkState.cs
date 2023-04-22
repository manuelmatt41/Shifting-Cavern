public class WalkState : PlayerState
{
    private static readonly WalkState walkState = new();

    private WalkState() { }

    public static WalkState Instance() => walkState;

    public void Enter(Player entity)
    {
        entity.AnimationStateMachineTree.Travel("Walk");
    }

    public void Exit(Player entity) { }

    public void Update(Player entity)
    {
        if (entity.WantToIdle)
        {
            entity.DefaultStateMachine.ChangeState(IdleState.Instance());

            return;
        }
        if (entity.WantToAttack)
        {
            entity.DefaultStateMachine.ChangeState(AttackState.Instance());

            return;
        }

        if (entity.WantToDash)
        {
            entity.DefaultStateMachine.ChangeState(DashState.Instance());

            return;
        }
    }
}
