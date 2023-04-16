
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.UIElements;

public class CraftingMenu : Modal
{
    private ItemType?[,] craftableItems = new ItemType?[3, 4] {
        { ItemType.IronPickaxe, ItemType.IronPickaxe, ItemType.IronPickaxe, null },
        { ItemType.ToolShaft, ItemType.ToolShaft, ItemType.ToolShaft, ItemType.ToolShaft },
        { ItemType.IronPickaxe, ItemType.IronPickaxe, null, null },
    };

    public CraftingMenu()
    {
        this.SetAllPadding(10);

        InitHeader();
        InitTabs();
        InitCraftable();
    }

    private void InitHeader()
    {
        var header = new VisualElement();
        header.style.flexDirection = FlexDirection.Row;
        header.style.alignItems = Align.Center;
        header.style.justifyContent = Justify.Center;
        header.style.height = 30;
        header.style.backgroundColor = Color.black;

        var title = new Label("Crafting");
        title.style.fontSize = 20;
        title.style.color = Color.white;
        header.Add(title);

        this.modal.Add(header);
    }

    private void InitTabs()
    {
        var tabs = new VisualElement();
        tabs.style.flexDirection = FlexDirection.Row;

        var tab1 = new Label("Tab 1");
        tab1.style.fontSize = 20;
        tab1.style.color = Color.white;
        tab1.style.flexGrow = 1;
        tabs.Add(tab1);

        var tab2 = new Label("Tab 2");
        tab2.style.fontSize = 20;
        tab2.style.color = Color.white;
        tab1.style.flexGrow = 1;
        tabs.Add(tab2);

        this.modal.Add(tabs);
    }

    private void InitCraftable()
    {
        var craftableSection = new VisualElement();

        for (int y = 0; y < this.craftableItems.GetLength(0); y++)
        {
            var row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;

            for (int x = 0; x < this.craftableItems.GetLength(1); x++)
            {
                if (this.craftableItems[y, x] == null)
                    continue;

                var slot = new CraftingItemSlot(this.craftableItems[y, x].Value);
                row.Add(slot);
            }

            craftableSection.Add(row);
        }

        this.modal.Add(craftableSection);
    }

    public override void Update()
    {
    }
}