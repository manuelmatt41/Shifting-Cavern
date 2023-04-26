using Godot;
using Godot.Collections;
using System;

public partial class SoundManager : Node
{
    public static SoundManager Instance { get; private set; }

    private Dictionary<SoundQueueType, SoundQueue> _soundQueues = new();
    private Dictionary<SoundPoolType, SoundPool> _soundPools = new();

    public override void _Ready()
    {
        Instance = this;

        #region Player sound nodes
        this._soundPools[SoundPoolType.PlayerWalkSounds] = this.GetNode<SoundPool>(
            nameof(SoundPoolType.PlayerWalkSounds)
        );
        #endregion

        #region Goblin sound nodes
        this._soundQueues[SoundQueueType.GoblinHitSound] = this.GetNode<SoundQueue>(
            nameof(SoundQueueType.GoblinHitSound)
        );
        #endregion
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

    #endregion
}

public enum SoundQueueType
{
    GoblinHitSound,
}

public enum SoundPoolType
{
    PlayerWalkSounds,
}
