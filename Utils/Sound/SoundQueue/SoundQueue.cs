using System.Collections.Generic;
using Godot;

[Tool]
public partial class SoundQueue : Node
{
    [Export]
    public int InstancesCount { get; set; } = 1;

    private int _next = 0;
    private List<AudioStreamPlayer> _audioStreamPlayers = new();

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

    public void PlaySound()
    {
        if (!this._audioStreamPlayers[this._next].Playing)
        {
            this._audioStreamPlayers[this._next++].Play();
            this._next %= this.InstancesCount;
        }
    }
}
