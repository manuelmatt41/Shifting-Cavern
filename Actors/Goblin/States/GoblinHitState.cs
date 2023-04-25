public class GoblinHitState : GoblinState
{
    private static readonly GoblinHitState goblinHitState = new();

    private GoblinHitState() { }

    public static GoblinHitState Instance() => goblinHitState;

    public void Enter(Goblin entity)
    {
        entity.AnimationStateMachineTree.Travel("Hit");
    }

    public void Exit(Goblin entity) { }

    public void Update(Goblin entity)
    {
        if (entity.IsHitAnimationDone)
        {
            if (entity.WantToWalk)
            {
                entity.DefaultStateMachine.ChangeState(GoblinWalkState.Instance());
            }

            if (entity.WantToIdle)
            {
                entity.DefaultStateMachine.ChangeState(GoblinIdleState.Instance());
            }
        }
    }
}
