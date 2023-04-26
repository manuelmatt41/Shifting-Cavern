using Godot;

public class GoblinHitState : GoblinState
{
    private static readonly GoblinHitState goblinHitState = new();

    private GoblinHitState() { }

    public static GoblinHitState Instance() => goblinHitState;

    public void Enter(Goblin entity)
    {
        entity.AnimationStateMachineTree.Travel("Hit");
        SoundManager.Instance.PlayGoblinHitSound();
    }

    public void Exit(Goblin entity) { }

    public void Update(Goblin entity)
    {
        if (entity.IsHitAnimationDone)
        {
            if (entity.WantToWalk)
            {
                entity.NextState = GoblinWalkState.Instance();
                return;
            }

            if (entity.WantToIdle)
            {
                entity.NextState = GoblinIdleState.Instance();
                return;
            }
        }
    }
}
