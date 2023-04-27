using Godot;

/// <summary>
/// Clase que representa el estado de <c>Player</c> al atacar
/// </summary>
public class PlayerAttackState : PlayerState
{
    private static readonly PlayerAttackState attackState = new();

    private PlayerAttackState() { }

    /// <summary>
    /// Devuelve la misma instancia <c>PlayerAttackState</c> para no repetir instancias del mismo objeto
    /// </summary>
    /// <returns>Intancia <c>PlayerAttackState</c></returns>
    public static PlayerAttackState Instance() => attackState;

    /// <summary>
    /// Al entrar en el estado <c>PlayerAttackState</c> transiciona a la animacion 'Attack' de <c>Player</c> y posicion la HitBox en la direccion en la que se va a atacar
    /// </summary>
    /// <param name="entity">Entidad <c>Player</c></param>
    public void Enter(Player entity)
    {
        entity.AnimationStateMachineTree.Travel(Player.ATTACK_ANIMATION_NAME);

        // Comprueba la posicion del raton en relacion de la posicion global, -1 (izquierda de la pantalla) y 1 (derecha)
        var attackDirection = entity.GlobalPosition.X > entity.GetGlobalMousePosition().X ? -1 : 1;

        // Calcula la posicion de la HitBox al golpear
        var x = 16 * attackDirection; //TODO Cambiar el 16 por un valor Range vinculado al tipo de arma que se este utilizando
        var newHitBoxPosition = new Vector2(x, entity.HitBox.CollisionShape.Position.Y);

        entity.Sprite.FlipH = attackDirection != -1;
        entity.HitBox.CollisionShape.Position = newHitBoxPosition;
        entity.HitBox.CollisionShape.Disabled = false;
    }

    /// <summary>
    /// Al salir en el estado <c>PlayerAttackState</c> resetea los parametros cambiados al entrar en el estado
    /// </summary>
    /// <param name="entity">Entidad <c>Player</c></param>
    public void Exit(Player entity)
    {
        var resetHitBoxPosition = new Vector2(0, entity.HitBox.CollisionShape.Position.Y);

        entity.HitBox.CollisionShape.Disabled = true;
        entity.HitBox.CollisionShape.Position = resetHitBoxPosition;
    }

    /// <summary>
    /// Al actualizarse el estado comprueba que la animacion 'Attack' de <c>Player</c> haya acabado para poder cambiar a otro estado
    /// </summary>
    /// <param name="entity">Entidad <c>Player</c></param>
    public void Update(Player entity)
    {
        if (entity.IsAttackAnimationDone)
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
