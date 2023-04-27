/// <summary>
/// Clase que representa el estado de <c>Goblin</c> al estar moviendose
/// </summary>
public class GoblinWalkState : GoblinState
{
    private static readonly GoblinWalkState goblinWalkState = new();

    private GoblinWalkState() { }

    /// <summary>
    /// Devuelve la misma instancia <c>GoblinWalkState</c> para no repetir instancias del mismo objeto
    /// </summary>
    /// <returns>Intancia <c>GoblinWalkState</c></returns>
    public static GoblinWalkState Instance() => goblinWalkState;

    /// <summary>
    /// Al entrar en el estado <c>GoblinWalkState</c> transiciona a la animacion 'Walk' de <c>Goblin</c>
    /// </summary>
    /// <param name="entity">Entidad <c>Goblin</c></param>
    public void Enter(Goblin entity)
    {
        entity.AnimationStateMachineTree.Travel(Goblin.WALK_ANIMATION_NAME);
    }

    public void Exit(Goblin entity) { }

    /// <summary>
    /// Al actualizarse el estado comprueba el estado al que quiere cambiarse
    /// </summary>
    /// <param name="entity">Entidad <c>Goblin</c></param>
    public void Update(Goblin entity)
    {
        if (entity.WantToIdle)
        {
            entity.NextState = GoblinIdleState.Instance();
            return;
        }

        entity.Sprite.FlipH = entity.MoveDirection.X < 0;

        entity.Velocity = entity.MoveSpeed * entity.MoveDirection;

        entity.MoveAndSlide();
    }
}
