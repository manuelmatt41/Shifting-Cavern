using Godot;
using System;

public partial class BalloonTextControl : Control
{
    public const int LINE_SIZE = 7;

    [Export]
    public float TextSpeed { get; set; }

    public NinePatchRect NinePatchRect { get; private set; }
    public RichTextLabel RichTextLabel { get; private set; }

    private double _timeToNextLetter = 0;

    private int _lines = 0;

    public override void _Ready()
    {
        this.NinePatchRect = this.GetNode<NinePatchRect>("NinePatchRect");
        this.RichTextLabel = this.NinePatchRect.GetNode<RichTextLabel>(nameof(this.RichTextLabel));
    }

    public override void _PhysicsProcess(double delta)
    {
        if (this.Visible)
        {
            this._timeToNextLetter += delta;

            if (this._timeToNextLetter >= this.TextSpeed)
            {
                this.RichTextLabel.VisibleCharacters++;
                this._timeToNextLetter = 0;
            }
        }
    }

    public void Start()
    {
        this.Visible = true;
    }

    public void Reset()
    {
        this.Visible = false;
        this._timeToNextLetter = 0;
        this.RichTextLabel.VisibleCharacters = 0;

        for (int i = 0; i < this._lines; i++)
        {
            var newHeight = this.NinePatchRect.Size.Y - LINE_SIZE;
            var newSize = new Vector2(this.NinePatchRect.Size.X, newHeight);

            var newY = this.NinePatchRect.GlobalPosition.Y + LINE_SIZE;
            var newPosition = new Vector2(this.NinePatchRect.GlobalPosition.X, newY);

            this.Update(newSize, newPosition);
        }

        this._lines = 0;
    }

    public void Update(Vector2 newSize, Vector2 newPosition)
    {
        this.NinePatchRect.Size = newSize;
        this.NinePatchRect.GlobalPosition = newPosition;
    }

    private void OnRichTextResized()
    {
        if (this.Visible)
        {
            var newHeight = this.NinePatchRect.Size.Y + LINE_SIZE; //TODO Hacer contante o intentar hacer el calcula por codigo pero seria peor.
            var newSize = new Vector2(this.NinePatchRect.Size.X, newHeight);

            var newY = this.NinePatchRect.GlobalPosition.Y - LINE_SIZE;
            var newPosition = new Vector2(this.NinePatchRect.GlobalPosition.X, newY);

            this.Update(newSize, newPosition);
            this._lines++;
            GD.Print(this._lines);
        }
    }
}
