public class DashState : PlayerState
{
    private static readonly DashState dashState = new();

    private DashState() { }

    public static DashState Instance() => dashState;

    public void Enter(Player entity)
    {
        entity.AnimationStateMachineTree.Travel("Dash");
    }

    public void Exit(Player entity) { }

    public void Update(Player entity)
    {
        if (entity.IsDashAnimationDone)
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
