using System.Linq;
using Godot;

/// <summary>
/// Clase que representa la UI de un espacio de inventario
/// </summary>
public partial class SlotUI : PanelContainer
{
    /// <summary>
    /// Evento que se lanza al hacer click en <c>SlotUI</c>
    /// </summary>
    /// <param name="index">Indece del <c>SlotUI</c> que se ha pulsado</param>
    /// <param name="mouseButtonIndex">Boton del raton que se ha pulsado sobre <c>SlotUI</c></param>
    [Signal]
    public delegate void SlotClickedEventHandler(int index, int mouseButtonIndex);

    /// <summary>
    /// Textura que representa el item que contiene el <c>SlotUI</c>
    /// </summary>
    public TextureRect TextureRect { get; private set; }

    /// <summary>
    /// Etiqueta que representa la cantidad de items que contiene el <c>SlotUI</c>
    /// </summary>
    public Label QuantityLabel { get; private set; }

    /// <summary>
    /// Funcion integrada de Godot que se ejecuta al crear el nodo en la escena, se usa para iniciar las variables de nodos subyacentes de <c>SlotUI</c>
    /// </summary>
    public override void _Ready()
    {
        this.TextureRect = this.GetNode<MarginContainer>("MarginContainer")
            .GetNode<TextureRect>("TextureRect");
        this.QuantityLabel = this.GetNode<Label>("QuantityLabel");
    }

    /// <summary>
    /// Coloca la informacion que contiene la <c>SlotData</c> en <c>SlotUI</c> para representarlo de forma grafica
    /// </summary>
    /// <param name="slotData">Informacion a representar en <c>SlotUI</c></param>
    public void SetSlotData(SlotData slotData)
    {
        var itemData = slotData.ItemData;

        this.TextureRect.Texture = itemData.Texture;
        this.TooltipText = $"{itemData.Name}\n{itemData.Description}";

        if (slotData.Quantity > 1)
        {
            this.QuantityLabel.Text = $"{slotData.Quantity}";
            this.QuantityLabel.Show();
        }
        else
        {
            this.QuantityLabel.Hide();
        }
    }

    /// <summary>
    /// Funcion que se ejcuta al interacturar con el componente, comprueba que se ha hecho un click para lanzar el evento <c>SlotClicked</c>
    /// </summary>
    /// <param name="event">Tipo de evento que se lanzo</param>
    private void OnGuiInput(InputEvent @event)
    {
        if (
            @event is InputEventMouseButton mouseEvent
            && (
                mouseEvent.ButtonIndex == MouseButton.Left
                || mouseEvent.ButtonIndex == MouseButton.Right
            )
            && mouseEvent.IsPressed()
        )
        {
            this.EmitSignal(SignalName.SlotClicked, this.GetIndex(), (int)mouseEvent.ButtonIndex); // El enumerado de MouseButton hereda de long
        }
    }
}
