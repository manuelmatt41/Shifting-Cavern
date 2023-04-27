using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

/// <summary>
/// Clase que repoduce un sonido de una lista de <c>SoundQueue</c> de forma aleatoria
/// </summary>
[Tool]
public partial class SoundPool : Node
{
    /// <summary>
    /// Lista de sonidos donde se cogera el sonido a reproducir
    /// </summary>
    private List<SoundQueue> _sounds = new();

    /// <summary>
    /// Valor aleatorio que escogera el sonido a reproducir
    /// </summary>
    private RandomNumberGenerator _randomNumberGenerator = new();

    /// <summary>
    /// Valor que guarda el indice anterior para que no se reproduzca el mismo sonido dos veces seguidas
    /// </summary>
    private int _lastIndex = -1;

    /// <summary>
    /// Funcion integrada de Godot que se ejecuta al crear el nodo en la escena
    /// </summary>
    public override void _Ready()
    {
        if (this.GetChildCount() < 2)
        {
            GD.Print($"Se esperaban minimo dos nodos {nameof(SoundQueue)}");
            return;
        }

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
        int numberOfSoundQueueChildrens = Array
            .FindAll(this.GetChildren().ToArray(), c => c is SoundQueue)
            .Length;

        if (numberOfSoundQueueChildrens < 2)
        {
            return new[] { $"Se esperaban dos o mas nodos {nameof(SoundQueue)}" };
        }

        return base._GetConfigurationWarnings();
    }

    /// <summary>
    /// Reproduce un sonido aleatorio de la lista <c>_sounds</c>
    /// </summary>
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
