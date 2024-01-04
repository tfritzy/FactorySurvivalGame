using Core;
using UnityEngine;
using UnityEngine.UIElements;

public class UiItem : VisualElement
{
    private Label quantityLabel;
    public const int SIZE = 55;
    public Item? Item { get; private set; }

    public UiItem(bool withLabel = false)
    {
        this.style.width = SIZE;
        this.style.height = SIZE;

        this.pickingMode = PickingMode.Ignore;

        InitQuantityLabel();
    }

    private void InitQuantityLabel()
    {
        quantityLabel = new Label();
        Add(quantityLabel);
        quantityLabel.style.position = Position.Absolute;
        quantityLabel.style.right = -5;
        quantityLabel.style.bottom = -9;
        quantityLabel.pickingMode = PickingMode.Ignore;
        quantityLabel.style.fontSize = 15;
        quantityLabel.style.color = UIManager.ColorTheme.ItemSlotTextColor;
        quantityLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
        quantityLabel.style.unityTextOutlineColor = UIManager.ColorTheme.PanelBackgroundColor;
        quantityLabel.style.unityTextOutlineWidth = 1;
        quantityLabel.SetAllMargin(0);
    }

    private ulong? renderedQuantity = null;
    private ItemType? renderedItemType = null;
    public void Update(Item item)
    {
        if (renderedItemType == item?.Type && renderedQuantity == item?.Quantity)
            return;

        Item = item;
        renderedItemType = item?.Type;
        renderedQuantity = item?.Quantity;
        if (item == null)
        {
            this.style.backgroundImage = null;
            this.quantityLabel.text = "";
        }
        else
        {
            this.style.backgroundImage = new StyleBackground(Icons.GetIcon(item.Type));

            if (item.Quantity > 1)
            {
                if (item.Units == Item.UnitType.Unit)
                {
                    this.quantityLabel.text = item.Quantity.ToString();
                }
                else
                {
                    this.quantityLabel.text = (item.Quantity / 1_000_000f).ToString("0.#") + "kg";
                }

            }
            else
                this.quantityLabel.text = "";
        }
    }
}