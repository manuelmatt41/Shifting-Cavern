using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GoblinIdleState : GoblinState
{
    private static readonly GoblinIdleState goblinIdleState = new();

    private GoblinIdleState() { }

    public static GoblinIdleState Instance() => goblinIdleState;

    public void Enter(Goblin entity)
    {
        entity.AnimationStateMachineTree.Travel("Idle");
    }

    public void Exit(Goblin entity) { }

    public void Update(Goblin entity)
    {
        if (entity.WantToWalk)
        {
            entity.DefaultStateMachine.ChangeState(GoblinWalkState.Instance());
        }
    }
}
