public class PlayerWalkState : PlayerState
{
    private static readonly PlayerWalkState walkState = new();

    private PlayerWalkState() { }

    public static PlayerWalkState Instance() => walkState;

    public void Enter(Player entity)
    {
        entity.AnimationStateMachineTree.Travel("Walk");
    }

    public void Exit(Player entity) { }

    public void Update(Player entity)
    {
        if (entity.WantToIdle)
        {
            entity.DefaultStateMachine.ChangeState(PlayerIdleState.Instance());

            return;
        }
        if (entity.WantToAttack)
        {
            entity.DefaultStateMachine.ChangeState(PlayerAttackState.Instance());

            return;
        }

        if (entity.WantToDash)
        {
            entity.DefaultStateMachine.ChangeState(PlayerDashState.Instance());

            return;
        }
    }
}
