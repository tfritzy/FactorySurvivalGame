using Core;
using UnityEngine;
using UnityEngine.UIElements;

public class UiItem : VisualElement
{
    private Label quantityLabel;
    public const int SIZE = 55;
    public Item Item { get; private set; }

    public UiItem()
    {
        this.style.width = SIZE;
        this.style.height = SIZE;

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
        this.quantityLabel.style.unityTextOutlineColor = Color.white;
        this.quantityLabel.style.unityTextOutlineWidth = 1;
    }

    private int renderedQuantity = -1;
    private ItemType? renderedItemType = null;
    public void Update(Item item)
    {
        if (renderedItemType == item?.Type && renderedQuantity == item?.Quantity)
            return;

        Item = item;
        renderedItemType = item?.Type;
        renderedQuantity = item?.Quantity ?? -1;
        if (item == null)
        {
            this.style.backgroundImage = null;
            this.quantityLabel.text = "";
        }
        else
        {
            this.style.backgroundImage = new StyleBackground(Icons.GetIcon(item.Type));

            if (item.Quantity > 1)
                this.quantityLabel.text = item.Quantity.ToString();
            else
                this.quantityLabel.text = "";
        }
    }
}