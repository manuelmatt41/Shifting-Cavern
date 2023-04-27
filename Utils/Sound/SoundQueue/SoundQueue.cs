using System.Collections.Generic;
using Godot;

/// <summary>
/// Clase para repoducir un cola de un sonido
/// </summary>
[Tool]
public partial class SoundQueue : Node
{
    /// <summary>
    /// Cantidad de veces que se quiere escuchar un sonido
    /// </summary>
    /// <value>Por defecto: 1</value>
    [Export]
    public int InstancesCount { get; set; } = 1;

    /// <summary>
    /// Indice del sonido que se a a reproducir
    /// </summary>
    private int _next = 0;
    /// <summary>
    /// Lista de las instancias del sonido que se va repoducir
    /// </summary>
    private List<AudioStreamPlayer> _audioStreamPlayers = new();

    /// <summary>
    /// Funcion integrada de Godot que se ejecuta al crear el nodo en la escena, se crean las instancias del sonido y se añaden a <c>SoundQueue</c> y a <c>_audioStreamPlayers</c>
    /// </summary>
    public override void _Ready()
    {
        var child = this.GetChild(0);

        if (child is AudioStreamPlayer audioStreamPlayer)
        {
            this._audioStreamPlayers.Add(audioStreamPlayer);

            for (int i = 0; i < this.InstancesCount; i++)
            {
                var duplicate = audioStreamPlayer.Duplicate() as AudioStreamPlayer;

                this.AddChild(duplicate);

                this._audioStreamPlayers.Add(duplicate);
            }
        }
    }

    /// <summary>
    /// Ejecuta el siguiente sonido que se va a reproducir
    /// </summary>
    public void PlaySound()
    {
        if (!this._audioStreamPlayers[this._next].Playing)
        {
            this._audioStreamPlayers[this._next++].Play();

            // Asegura que cuando llegue al final de la lista el valor vuelva a 0
            this._next %= this.InstancesCount;
        }
    }
}
