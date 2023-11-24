using Core;
using UnityEngine;
using UnityEngine.UIElements;

public class SlotItemIcon : VisualElement
{
    private Label quantityLabel;

    public SlotItemIcon()
    {
        this.style.width = 55;
        this.style.height = 55;

        this.pickingMode = PickingMode.Ignore;

        InitQuantityLabel();
    }

    private void InitQuantityLabel()
    {
        this.quantityLabel = new Label();
        this.Add(this.quantityLabel);
        this.quantityLabel.style.position = Position.Absolute;
        this.quantityLabel.style.right = -11;
        this.quantityLabel.style.bottom = -11;
        this.quantityLabel.pickingMode = PickingMode.Ignore;
        this.quantityLabel.style.fontSize = 20;
        this.quantityLabel.style.color = UIManager.ColorTheme.ItemSlotTextColor;
        this.quantityLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
        this.quantityLabel.style.unityTextOutlineColor = Color.black;
        this.quantityLabel.style.unityTextOutlineWidth = 1;
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
            this.style.backgroundImage = new StyleBackground(Icons.GetIcon(item.Type));

            if (item.Quantity > 1)
                this.quantityLabel.text = item.Quantity.ToString();
            else
                this.quantityLabel.text = "";
        }
    }
}