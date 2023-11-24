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
        nameLabel.pickingMode = PickingMode.Ignore;
        modal.Add(nameLabel);

        if (character.Inventory != null)
        {
            var outerBorder = new VisualElement();
            UIManager.ColorTheme.Apply3DPanelBorderColor(outerBorder, inverse: true);
            outerBorder.SetAllBorderWidth(1);
            modal.Add(outerBorder);

            var innerBorder = new VisualElement();
            UIManager.ColorTheme.Apply3DPanelBorderColor(innerBorder);
            innerBorder.SetAllBorderWidth(1);
            outerBorder.Add(innerBorder);

            innerBorder.Add(inventory);

            inventory = new InventoryGrid(
                 new InventoryGrid.Props
                 {
                     inventory = character.Inventory,
                     height = character.Inventory.Height,
                     width = character.Inventory.Width,
                     HideBorder = true,
                     SlotBorderWidth = 1,
                 }
             );
            innerBorder.Add(inventory);
        }
    }

    public override void Update()
    {
        inventory?.Update();
    }
}