using System;
using Core;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterInspector : Modal
{
    private InventoryGrid inventory;

    public CharacterInspector(Character character, Action onClose) : base(onClose)
    {
        var nameLabel = new Label(character.Type.ToString());
        nameLabel.style.color = UIManager.ColorTheme.PrimaryText;
        nameLabel.style.fontSize = 25;
        nameLabel.style.marginBottom = 20;
        modal.Add(nameLabel);

        if (character.Inventory != null)
        {
            var inventoryGrid = new InventoryGrid(
                new InventoryGrid.Props
                {
                    inventory = character.Inventory,
                    height = character.Inventory.Height,
                    width = character.Inventory.Width,
                    Gap = 10,
                    HideBorder = true,
                    SlotBorderWidth = 1,
                }
            );
            modal.Add(inventoryGrid);
        }


    }

    public override void Update()
    {
        inventory.Update();
    }
}