using Godot;

/// <summary>
/// Clase que representa un globo con texto en el juego
/// </summary>
public partial class BalloonTextControl : Control
{
    /// <summary>
    /// Tamanio en px del texto
    /// </summary>
    public const int LINE_SIZE = 7;

    /// <summary>
    /// Velocidad en la que aparece el texto en el globo
    /// </summary>
    [Export]
    public float TextSpeed { get; set; }

    /// <summary>
    /// Textura que se muestra en el fondo del globo
    /// </summary>
    public NinePatchRect NinePatchRect { get; private set; }

    /// <summary>
    /// Texto que se muestra dentro del globo
    /// </summary>
    public RichTextLabel RichTextLabel { get; private set; }

    /// <summary>
    /// Tiempo que ha pasado desde que se escribio la anterior letra
    /// </summary>
    private double _timeToNextLetter = 0;

    /// <summary>
    /// Cuenta las lineas que ha escrito el componente
    /// </summary>
    private int _lines = 0;

    /// <summary>
    /// Funcion integrada de Godot que se ejecuta al crear el nodo en la escena, se usa para iniciar las variables de nodos subyacentes de <c>BalloonTextControl</c>
    /// </summary>
    public override void _Ready()
    {
        this.NinePatchRect = this.GetNode<NinePatchRect>("NinePatchRect");
        this.RichTextLabel = this.NinePatchRect.GetNode<RichTextLabel>(nameof(this.RichTextLabel));
    }

    /// <summary>
    /// Funcion integrada de Godot que se ejecuta 1 / 60 frames para trabajar con mas facilmente con las fisicas del juego, comprueba que haya pasado el tiempo establecido entre letra y letra para mostrar la siguiente
    /// <param name="delta">Valor del tiempo entre frames</param>
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

    /// <summary>
    /// Comienza el componente haciendolo visible
    /// </summary>
    public void Start()
    {
        this.Visible = true;
    }

    /// <summary>
    /// Reinicia el componente y su comportamiento ocultando el componente
    /// </summary>
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

    /// <summary>
    /// Actualiza el tamanio del fondo para cuadrar con el texto que se esta mostrando
    /// </summary>
    /// <param name="newSize">Nuevo tamanio del fondo</param>
    /// <param name="newPosition">Nueva posicion del componente</param>
    public void Update(Vector2 newSize, Vector2 newPosition)
    {
        this.NinePatchRect.Size = newSize;
        this.NinePatchRect.GlobalPosition = newPosition;
    }

    /// <summary>
    /// Funcion que se ejecuta al cambiar de tamanio la etiqueta con el texto y reescala el tamanio del fondo para cuadrar con el texto
    /// </summary>
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
        }
    }
}
