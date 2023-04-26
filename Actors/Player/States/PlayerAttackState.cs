using Godot;

public class PlayerAttackState : PlayerState
{
    private static readonly PlayerAttackState attackState = new();

    private PlayerAttackState() { }

    public static PlayerAttackState Instance() => attackState;

    public void Enter(Player entity)
    {
        entity.AnimationStateMachineTree.Travel("Attack");
        // Comprueba la posicion del raton en relacion de la posicion global, -1 (izquierda de la pantalla) y 1 (derecha)
        var attackDirection = entity.GlobalPosition.X > entity.GetGlobalMousePosition().X ? -1 : 1;
        // Calcula la posicion de la hitbox al golpear con la mitad del ancho de la textura en posicion negativa (izquierda) o positiva (derecha)
        var x = (int)(16 * attackDirection); //TODO Cambiar el 16 por un valor Range vinculado al tipo de arma que se este utilizando

        entity.Sprite.FlipH = attackDirection != -1;
        entity.HitBox.CollisionShape.Position = new Vector2(
            x,
            entity.HitBox.CollisionShape.Position.Y
        );
        entity.HitBox.CollisionShape.Disabled = false;
    }

    public void Exit(Player entity)
    {
        entity.HitBox.CollisionShape.Disabled = true;
        entity.HitBox.CollisionShape.Position = new Vector2(
            0,
            entity.HitBox.CollisionShape.Position.Y
        );
    }

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
