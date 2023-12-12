
using System;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.UIElements;

public class CraftingMenu : Modal
{
    private CraftingHoverMenu hoverMenu;

    public CraftingMenu()
    {
        this.SetAllPadding(10);
        InitCraftable();
    }

    private void InitCraftable()
    {
        modal.style.maxHeight = 350;
        modal.style.minHeight = 350;

        var items = new List<Item> {
            new IronOre(),
            new Stone(),
            new ConveyorItem(),
            new Coal(),
            new ToolShaft(),
            new IronBar(),
            new IronPickaxe(),
            new Log(),
            new DepotItem(),
            new MineshaftItem(),
            new Arrowhead(),
        };
        var search = new ItemSearch(
            items,
            onHover: BuildHoverMenu,
            onExit: CloseHoverMenu,
            onClick: CraftItem);
        this.modal.Add(search);
    }

    private void BuildHoverMenu(Item item, VisualElement target)
    {
        CloseHoverMenu();
        this.hoverMenu = new CraftingHoverMenu(
            item,
            target.worldBound.xMax + 10,
            target.worldBound.yMin);
        this.Add(this.hoverMenu);
    }

    private void CloseHoverMenu()
    {
        if (this.hoverMenu != null)
        {
            this.hoverMenu.RemoveFromHierarchy();
            this.hoverMenu = null;
        }
    }

    private void CraftItem(Item item)
    {
        try
        {
            Crafting.CraftItem(item.Type, PlayerMono.Instance.Actual.ActiveItems);
            this.hoverMenu.Update();
        }
        catch (System.InvalidOperationException e)
        {
            Debug.Log(e.Message);
        }
    }
}