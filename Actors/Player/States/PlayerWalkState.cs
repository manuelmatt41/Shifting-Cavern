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
        SoundManager.Instance.PlayRandomPlayerWalkSound();
        if (entity.WantToAttack)
        {
            entity.NextState = PlayerAttackState.Instance();

            return;
        }

        if (entity.WantToIdle)
        {
            entity.NextState = PlayerIdleState.Instance();

            return;
        }

        if (entity.WantToDash)
        {
            entity.NextState = PlayerDashState.Instance();

            return;
        }
    }
}
