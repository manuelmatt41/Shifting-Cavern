/// <summary>
/// Clase que representa el estado de <c>Player</c> al moverse
/// </summary>
public class PlayerWalkState : PlayerState
{
    private static readonly PlayerWalkState walkState = new();

    private PlayerWalkState() { }

    /// <summary>
    /// Devuelve la misma instancia <c>PlayerWalkState</c> para no repetir instancias del mismo objeto
    /// </summary>
    /// <returns>Intancia <c>PlayerWalkState</c></returns>
    public static PlayerWalkState Instance() => walkState;

    /// <summary>
    /// Al entrar en el estado <c>PlayerWalkState</c> transiciona a la animacion 'Walk' de <c>Player</c>
    /// </summary>
    /// <param name="entity">Entidad <c>Player</c></param>
    public void Enter(Player entity)
    {
        entity.AnimationStateMachineTree.Travel(Player.WALK_ANIMATION_NAME);
    }

    public void Exit(Player entity) { }

    /// <summary>
    /// Al actualizarse el estado comprueba que se quiere cambiar a otro estado, diciendo a <c>Player</c> que se mueva
    /// </summary>
    /// <param name="entity">Entidad <c>Player</c></param>
    public void Update(Player entity)
    {
        entity.DoWalk();

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
