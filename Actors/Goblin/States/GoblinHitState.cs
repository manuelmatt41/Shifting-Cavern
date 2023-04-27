/// <summary>
/// Clase que representa el estado de <c>Goblin</c> al ser golpeado
/// </summary>
public class GoblinHitState : GoblinState
{
    private static readonly GoblinHitState goblinHitState = new();

    private GoblinHitState() { }

    /// <summary>
    /// Devuelve la misma instancia <c>GoblinHitState</c> para no repetir instancias del mismo objeto
    /// </summary>
    /// <returns>Intancia <c>GoblinHitState</c></returns>
    public static GoblinHitState Instance() => goblinHitState;

    /// <summary>
    /// Al entrar en el estado <c>GoblinHitState</c> transiciona a la animacion 'Hit' de <c>Goblin</c> y ejecuta el sonido <c>GoblinHitSound</c>
    /// </summary>
    /// <param name="entity">Entidad <c>Goblin</c></param>
    public void Enter(Goblin entity)
    {
        entity.AnimationStateMachineTree.Travel(Goblin.HIT_ANIMATION_NAME);
        SoundManager.Instance.PlayGoblinHitSound();
    }

    public void Exit(Goblin entity) { }

    /// <summary>
    /// Al actualizarse el estado comprueba que la animacion 'Hit' de <c>Goblin</c> haya acabado para poder cambiar a otro estado
    /// </summary>
    /// <param name="entity">Entidad <c>Goblin</c></param>
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
