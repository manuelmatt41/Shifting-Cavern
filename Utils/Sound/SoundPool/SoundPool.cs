using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class SoundPool : Node //TODO Hacer que revise si existen nodos hijos
{
    private List<SoundQueue> _sounds = new();
    private RandomNumberGenerator _randomNumberGenerator = new();
    private int _lastIndex = -1;

    public override void _Ready()
    {
        foreach (var child in this.GetChildren())
        {
            if (child is SoundQueue soundQueue)
            {
                this._sounds.Add(soundQueue);
            }
        }
    }

    public override string[] _GetConfigurationWarnings()
    {
        int numberOfSoundQueueChildrens = Array.FindAll(this.GetChildren().ToArray(), c => c is SoundQueue).Length;

        if (numberOfSoundQueueChildrens < 2)
        {
            return new[] { "Se esperaban dos o mas nodos SoundQueue" };
        }

        return base._GetConfigurationWarnings();
    }

    public void PlayRandomSound()
    {
        int index;

        do
        {
            index = _randomNumberGenerator.RandiRange(0, this._sounds.Count - 1);
        } while (index == this._lastIndex);

        this._lastIndex = index;
        this._sounds[index].PlaySound();
    }
}
