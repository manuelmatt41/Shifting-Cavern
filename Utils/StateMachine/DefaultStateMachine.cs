/// <summary>
/// Maquina de estados basica
/// </summary>
/// <typeparam name="E">Tipo de entidad</typeparam>
/// <typeparam name="S">Tipo de estado</typeparam>
public class DefaultStateMachine<E, S> : IStateMachine<E, S>
    where E : class
    where S : IState<E>
{
    /// <summary>
    /// Estado actual de <c>DefaultStateMachine</c>
    /// </summary>
    public S CurrentState { get; set; }
    /// <summary>
    /// Estado anterior de <c>DefaultStateMachine</c>
    /// </summary>
    public S PreviousState { get; set; }

    /// <summary>
    /// Entidad que tiene la <c>DefaultStateMachine</c>
    /// </summary>
    public E Entity { get; set; }

    /// <summary>
    /// Estado global de <c>DefaultStateMachine</c>
    /// </summary>
    public S GlobalState { get; set; }

    public DefaultStateMachine()
        : this(default, default, default) { }

    public DefaultStateMachine(E owner)
        : this(owner, default, default) { }

    public DefaultStateMachine(E owner, S initialState)
        : this(owner, initialState, default) { }

    public DefaultStateMachine(E owner, S initialState, S globalState)
    {
        this.Entity = owner;
        this.CurrentState = initialState ?? default;
        this.GlobalState = globalState;
    }

    /// <summary>
    /// Cambia el esatdo actual por el <c>nextState</c> realizando las funciones Exit y Enter respectivamente
    /// </summary>
    /// <param name="nextState">Esatdo que se va a cambiar por el actual</param>
    public void ChangeState(S nextState)
    {
        this.PreviousState = this.CurrentState;

        if (this.CurrentState != null)
        {
            this.CurrentState.Exit(Entity);
        }

        this.CurrentState = nextState;

        if (this.CurrentState != null)
        {
            this.CurrentState.Enter(this.Entity);
        }
    }

    public bool IsInState(S state)
    {
        return this.CurrentState.Equals(state);
    }

    public bool RevertToPreviousState()
    {
        if (this.PreviousState == null)
        {
            return false;
        }

        ChangeState(this.PreviousState);
        return true;
    }

    public void Update()
    {
        if (this.CurrentState != null)
        {
            this.CurrentState.Update(Entity);
        }

        if (this.GlobalState != null)
        {
            this.GlobalState.Update(Entity);
        }
    }
}
