
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.UIElements;

public class CraftingMenu : Modal
{
    public CraftingMenu()
    {
        this.SetAllPadding(10);

        InitCraftable();
    }

    private void InitCraftable()
    {
        var search = new ItemSearch(new List<Item> {
            new IronOre(),
            new Stone(),
            new ConveyorItem(),
            new Coal(),
        });
        search.style.marginBottom = 10;

        this.modal.Add(search);
    }

    public override void Update()
    {
    }
}