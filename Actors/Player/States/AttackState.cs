public class AttackState : PlayerState
{
    private static readonly AttackState attackState = new();

    private AttackState() { }

    public static AttackState Instance() => attackState;

    public void Enter(Player entity)
    {
        entity.AnimationStateMachineTree.Travel("Attack");
    }

    public void Exit(Player entity) { }

    public void Update(Player entity)
    {
        if (entity.IsAttackAnimationDone)
        {
            if (entity.WantToWalk)
            {
                entity.DefaultStateMachine.ChangeState(WalkState.Instance());

                return;
            }

            if (entity.WantToIdle)
            {
                entity.DefaultStateMachine.ChangeState(IdleState.Instance());

                return;
            }
        }
    }
}
