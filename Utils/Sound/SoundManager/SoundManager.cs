using Godot;
using System;
using System.Collections.Generic;

public partial class SoundManager : Node
{
    public static SoundManager Instance { get; private set; }

    private Dictionary<SoundQueueType, SoundQueue> _soundQueues = new();
    private Dictionary<SoundPoolType, SoundPool> _soundPools = new();

    public override void _Ready()
    {
        Instance = this;

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
    public void PlayRandomPlayerWalkSound()
    {
        this._soundPools[SoundPoolType.PlayerWalkSounds].PlayRandomSound();
    }
    #endregion

    #region Goblin sounds
    public void PlayGoblinHitSound()
    {
        this._soundQueues[SoundQueueType.GoblinHitSound].PlaySound();
    }

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
