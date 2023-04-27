/// <summary>
/// Clase que representa el estado de <c>Player</c> al usar el movimiento Dash
/// </summary>
public class PlayerDashState : PlayerState
{
    private static readonly PlayerDashState dashState = new();

    private PlayerDashState() { }

    /// <summary>
    /// Devuelve la misma instancia <c>PlayerDashState</c> para no repetir instancias del mismo objeto
    /// </summary>
    /// <returns>Intancia <c>PlayerDashState</c></returns>
    public static PlayerDashState Instance() => dashState;

    /// <summary>
    /// Al entrar en el estado <c>PlayerDashState</c> transiciona a la animacion 'Dash' de <c>Player</c>
    /// </summary>
    /// <param name="entity">Entidad <c>Player</c></param>
    public void Enter(Player entity)
    {
        entity.AnimationStateMachineTree.Travel(Player.DASH_ANIMATION_NAME);
        entity.DashCooldownTimer.Start();
    }

    public void Exit(Player entity) { }

    /// <summary>
    /// Al actualizarse el estado comprueba que la animacion 'Dash' de <c>Player</c> haya acabado para poder cambiar a otro estado, diciendo a <c>Player</c> haga el movimiento Dash
    /// </summary>
    /// <param name="entity">Entidad <c>Player</c></param>
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

        entity.DoDash();
    }
}
