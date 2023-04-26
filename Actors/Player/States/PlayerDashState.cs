﻿public class PlayerDashState : PlayerState
{
    private static readonly PlayerDashState dashState = new();

    private PlayerDashState() { }

    public static PlayerDashState Instance() => dashState;

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
                entity.NextState = PlayerWalkState.Instance();
                return;
            }

            if (entity.WantToIdle)
            {
                entity.NextState = PlayerIdleState.Instance();
                return;
            }
        }
    }
}
