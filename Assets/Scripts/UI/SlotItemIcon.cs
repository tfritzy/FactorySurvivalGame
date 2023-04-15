using Core;
using UnityEngine.UIElements;

public class SlotItemIcon : VisualElement
{
    private Label quantityLabel;

    public SlotItemIcon()
    {
        this.style.width = 40;
        this.style.height = 40;

        this.pickingMode = PickingMode.Ignore;

        InitQuantityLabel();
    }

    private void InitQuantityLabel()
    {
        this.quantityLabel = new Label();
        this.Add(this.quantityLabel);
        this.quantityLabel.style.position = Position.Absolute;
        this.quantityLabel.style.right = -5;
        this.quantityLabel.style.bottom = -5;
    }

    public void Update(Item item)
    {
        if (item == null)
        {
            this.style.backgroundImage = null;
            this.quantityLabel.text = "";
        }
        else
        {
            // TODO: Caching
            this.style.backgroundImage = new StyleBackground(Icons.GetIcon(ItemType.Stone));

            if (item.Quantity > 1)
                this.quantityLabel.text = item.Quantity.ToString();
            else
                this.quantityLabel.text = "";
        }
    }
}