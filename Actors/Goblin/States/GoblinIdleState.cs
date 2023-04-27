/// <summary>
/// Clase que representa el estado de <c>Goblin</c> al estar quieto
/// </summary>
public class GoblinIdleState : GoblinState
{
    private static readonly GoblinIdleState goblinIdleState = new();

    private GoblinIdleState() { }

    /// <summary>
    /// Devuelve la misma instancia <c>GoblinIdleState</c> para no repetir instancias del mismo objeto
    /// </summary>
    /// <returns>Intancia <c>GoblinIdleState</c></returns>
    public static GoblinIdleState Instance() => goblinIdleState;

    /// <summary>
    /// Al entrar en el estado <c>GoblinIdleState</c> transiciona a la animacion 'Idle' de <c>Goblin</c>
    /// </summary>
    /// <param name="entity">Entidad <c>Goblin</c></param>
    public void Enter(Goblin entity)
    {
        entity.AnimationStateMachineTree.Travel(Goblin.IDLE_ANIMATION_NAME);
    }

    public void Exit(Goblin entity) { }

    /// <summary>
    /// Al actualizarse el estado comprueba el estado al que quiere cambiarse
    /// </summary>
    /// <param name="entity">Entidad <c>Goblin</c></param>
    public void Update(Goblin entity)
    {
        if (entity.WantToWalk)
        {
            entity.NextState = GoblinWalkState.Instance();
            return;
        }
    }
}
