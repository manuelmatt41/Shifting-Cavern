using System;

public class GoblinWalkState : GoblinState
{
    private static readonly GoblinWalkState goblinWalkState = new();

    private GoblinWalkState() { }

    public static GoblinWalkState Instance() => goblinWalkState;

    public void Enter(Goblin entity)
    {
        entity.AnimationStateMachineTree.Travel("Walk");
    }

    public void Exit(Goblin entity) { }

    public void Update(Goblin entity)
    {
        if (entity.WantToIdle)
        {
            entity.DefaultStateMachine.ChangeState(GoblinIdleState.Instance());
        }
    }
}
