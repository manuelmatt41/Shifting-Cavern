using System;

public class GoblinWalkState : GoblinState
{
    private static readonly GoblinWalkState goblinWalkState = new GoblinWalkState();

    private GoblinWalkState() { }

    public static GoblinWalkState Instance() => goblinWalkState;

    public void Enter(Goblin entity)
    {
        entity.AnimationStateMachineTree.Travel("Walk");
    }

    public void Exit(Goblin entity)
    {
    }

    public void Update(Goblin entity)
    {

    }
}
