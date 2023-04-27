using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// Clase de una sola instancia para guardar los sonidos del juego y poder reproducirlos desde cualquier parte
/// </summary>
public partial class SoundManager : Node
{
    /// <summary>
    /// Instancia de la clase <c>SoundManager</c>
    /// </summary>
    public static SoundManager Instance { get; private set; }

    /// <summary>
    /// Lista de sonidos <c>SoundQueue</c>
    /// </summary>
    private Dictionary<SoundQueueType, SoundQueue> _soundQueues = new();

    /// <summary>
    /// Lista de sonidos <c>SoundPool</c>
    /// </summary>
    private Dictionary<SoundPoolType, SoundPool> _soundPools = new();

    public override void _Ready()
    {
        Instance = this;

        //Recorre los valores de SoundQueueType o SoundPoolType y guarda el sonido, tienen que llevar el mismo nombre
        Array.ForEach(
            Enum.GetValues<SoundQueueType>(),
            soundQueue =>
                this._soundQueues[soundQueue] = this.GetNode<SoundQueue>(soundQueue.ToString())
        );
        Array.ForEach(
            Enum.GetValues<SoundPoolType>(),
            soundPool => this._soundPools[soundPool] = this.GetNode<SoundPool>(soundPool.ToString())
        );
    }

    #region Player sounds
    /// <summary>
    /// Ejecuta el sonido <c>SoundPoolType.PlayerWalkSounds</c>
    /// </summary>
    public void PlayRandomPlayerWalkSound()
    {
        this._soundPools[SoundPoolType.PlayerWalkSounds].PlayRandomSound();
    }
    #endregion

    #region Goblin sounds
    /// <summary>
    /// Ejecuta el sonido <c>SoundQueueType.GoblinHitSound</c>
    /// </summary>
    public void PlayGoblinHitSound()
    {
        this._soundQueues[SoundQueueType.GoblinHitSound].PlaySound();
    }

    /// <summary>
    /// Ejecuta el sonido <c>SoundQueueType.GoblinDeadSound</c>
    /// </summary>
    public void PlayGoblinDeadSound()
    {
        this._soundQueues[SoundQueueType.GoblinDeadSound].PlaySound();
    }
    #endregion
}

public enum SoundQueueType
{
    GoblinHitSound,
    GoblinDeadSound,
}

public enum SoundPoolType
{
    PlayerWalkSounds,
}
