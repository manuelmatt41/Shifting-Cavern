using Godot;

public class DefaultStateMachine<E, S> : StateMachine<E, S> where E : class where S : State<E>
{

    public S CurrentState { get; set; }
    public S PreviousState { get; set; }

    public E Entity { get; set; }

    public S GlobalState { get; set; }

    public DefaultStateMachine() : this(default(E), default(S), default(S)) { }
    public DefaultStateMachine(E owner) : this(owner, default(S), default(S)) { }
    public DefaultStateMachine(E owner, S initialState) : this(owner, initialState, default(S)) { }
    public DefaultStateMachine(E owner, S initialState, S globalState)
    {
        Entity = owner;
        CurrentState = default(S);
        GlobalState = globalState;
    }

    public void ChangeState(S state)
    {
        PreviousState = CurrentState;

        if (CurrentState != null)
        {
            CurrentState.Exit(Entity);
        }

        CurrentState = state;

        if (CurrentState != null)
        {
            CurrentState.Enter(Entity);
        }
    }

    public bool IsInState(S state)
    {
        return this.CurrentState.Equals(state);
    }

    public bool RevertToPreviousState()
    {
        if (PreviousState == null)
        {
            return false;
        }

        ChangeState(PreviousState);
        return true;
    }

    public void Update()
    {
        if (CurrentState != null)
        {
            CurrentState.Update(Entity);
        }
    }
}