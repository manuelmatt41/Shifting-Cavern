/// <summary>
/// Maquina que cambia entre diferentes la entidad T
/// </summary>
/// <typeparam name="E">Representa una entidad genrica</typeparam>
/// <typeparam name="S">Representa un estado genrico</typeparam>
public interface IStateMachine<E, S> where E : class where S : IState<E>
{
    /// <summary>
    /// Cambia el estado actual por el pasado por parametro ejecutando las funciones correspondientes de los estados
    /// </summary>
    /// <param name="state">Esatdo que va a remplaazar al actual</param>
    public void ChangeState(S state);

    /// <summary>
    /// Actualiza el estado actual como el global
    /// </summary>
    public void Update();

    public bool RevertToPreviousState();

    public bool IsInState(S state);
}
