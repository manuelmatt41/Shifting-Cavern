/// <summary>
/// Clase que representa el estado de <c>Player</c> al qeudarse quieto
/// </summary>
public class PlayerIdleState : PlayerState
{
    private static readonly PlayerIdleState idleState = new();

    private PlayerIdleState() { }

    /// <summary>
    /// Devuelve la misma instancia <c>PlayerIdleState</c> para no repetir instancias del mismo objeto
    /// </summary>
    /// <returns>Intancia <c>PlayerIdleState</c></returns>
    public static PlayerIdleState Instance() => idleState;

    /// <summary>
    /// Al entrar en el estado <c>PlayerIdleState</c> transiciona a la animacion 'Idle' de <c>Player</c>
    /// </summary>
    /// <param name="entity">Entidad <c>Player</c></param>
    public void Enter(Player entity)
    {
        entity.AnimationStateMachineTree.Travel(Player.IDLE_ANIMATION_NAME);
    }

    public void Exit(Player entity) { }

    /// <summary>
    /// Al actualizarse el estado comprueba que se quiere cambiar a otro estado
    /// </summary>
    /// <param name="entity">Entidad <c>Player</c></param>
    public void Update(Player entity)
    {
        if (entity.WantToAttack)
        {
            entity.NextState = PlayerAttackState.Instance();
            return;
        }

        if (entity.WantToWalk)
        {
            entity.NextState = PlayerWalkState.Instance();
            return;
        }
    }
}
