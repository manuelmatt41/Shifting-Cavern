using System;

/// <summary>
/// Estado abstracto que ejecuta difentes funciones en una State Machine al entrar, al salir o ir actualizandose para procesar diferentes parametros de la entidad T
/// </summary>
/// <typeparam name="E">Parametro generico que representa una entidad</typeparam>
public interface State<E> where E : class
{
    /// <summary>
    /// Funcion que se ejecuta al establecer el estado en la State Machine
    /// </summary>
    public abstract void Enter(E entity);

	/// <summary>
	/// Funcion que se ejecuta al actualizar el estado en la State Machine para observar si se quiere transicionar a otro estado
	/// </summary>
    public abstract void Update(E entity);

	// <summary>
    /// Funcion que se ejecuta al quitar el estado en la State Machine
    /// </summary>
    public abstract void Exit(E entity);
}