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
    /// <param name="nextState">Esatdo que va a remplaazar al actual</param>
    public void ChangeState(S nextState);

    /// <summary>
    /// Actualiza el estado actual y el global
    /// </summary>
    public void Update();

    /// <summary>
    /// Vuelve al estado anterior
    /// </summary>
    /// <returns></returns>
    public bool RevertToPreviousState();

    /// <summary>
    /// Comprueba que el esatdo actual es el mismo que el que se pasa
    /// </summary>
    /// <param name="state">Estado que se va comprobar</param>
    /// <returns><c>True</c> si coinciden los estados sino <c>false</c></returns>
    public bool IsInState(S state);
}
