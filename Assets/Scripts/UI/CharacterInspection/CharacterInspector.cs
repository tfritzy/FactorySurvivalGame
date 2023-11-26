using System;
using System.Collections.Generic;
using Core;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterInspector : Modal
{
    private int seenInventoryVersion = -1;
    private int seenConveyorVersion = -1;
    private Character character;

    private enum Section { Inventory, Conveyor }

    private Dictionary<Section, VisualElement> sections = new();

    public CharacterInspector(Character character, Action onClose) : base(onClose)
    {
        this.character = character;

        var nameLabel = new Label(character.Name);
        nameLabel.style.color = UIManager.ColorTheme.PrimaryText;
        nameLabel.style.fontSize = 25;
        nameLabel.style.marginBottom = 10;
        nameLabel.pickingMode = PickingMode.Ignore;
        nameLabel.style.minWidth = 200;
        modal.Add(nameLabel);

        if (character.Inventory != null)
        {
            var inventorySection = new VisualElement();
            modal.Add(inventorySection);
            sections.Add(Section.Inventory, inventorySection);
            SetupInventory(character);
        }

        if (character.Conveyor != null)
        {
            var conveyorSection = new VisualElement();
            modal.Add(conveyorSection);
            sections.Add(Section.Conveyor, conveyorSection);
            SetupConveyor(character);
        }
    }

    private void SetupInventory(Character character)
    {
        sections[Section.Inventory]?.Clear();

        if (character.Inventory != null)
        {
            if (sections.Count > 1)
            {
                var label = new Label("Inventory");
                label.style.color = UIManager.ColorTheme.PrimaryText;
                label.style.fontSize = 20;
                label.pickingMode = PickingMode.Ignore;
                sections[Section.Inventory].Add(label);
            }

            var inventory = new InventoryGrid(
                 new InventoryGrid.Props
                 {
                     inventory = character.Inventory,
                     height = character.Inventory.Height,
                     width = character.Inventory.Width,
                     HideBorder = true,
                     SlotBorderWidth = 1,
                 }
             );
            sections[Section.Inventory].Add(inventory);
            sections[Section.Inventory].style.marginBottom = 10f;
            inventory.Update();
        }
    }

    private void SetupConveyor(Character character)
    {
        sections[Section.Conveyor]?.Clear();

        if (character.Conveyor != null)
        {
            if (sections.Count > 1)
            {
                var label = new Label("Conveyor");
                label.style.color = UIManager.ColorTheme.PrimaryText;
                label.style.fontSize = 20;
                label.pickingMode = PickingMode.Ignore;
                sections[Section.Conveyor].Add(label);
            }

            var conveyorElements = new VisualElement();
            conveyorElements.style.flexDirection = FlexDirection.Row;
            conveyorElements.SetAllBorderColor(UIManager.ColorTheme.PanelBackgroundColor);
            conveyorElements.SetAllBorderWidth(1);
            conveyorElements.SetAllPadding(5);
            sections[Section.Conveyor].Add(conveyorElements);

            int i = 0;
            foreach (var item in character.Conveyor.Items)
            {
                var container = new VisualElement();
                StyleConveyorItemContainer(container);
                if (i != character.Conveyor.Items.Count - 1)
                    container.style.marginRight = 5;
                var itemElement = new SlotItemIcon();
                itemElement.Update(item.Item);
                container.Add(itemElement);
                conveyorElements.Add(container);
                i += 1;
            }

            for (; i < 5; i++)
            {
                var container = new VisualElement();
                StyleConveyorItemContainer(container);
                container.style.width = SlotItemIcon.SIZE;
                container.style.height = SlotItemIcon.SIZE;
                if (i != 4)
                    container.style.marginRight = 5;
                conveyorElements.Add(container);
            }
        }
    }

    private void StyleConveyorItemContainer(VisualElement container)
    {
        container.style.flexDirection = FlexDirection.Row;
        container.style.alignItems = Align.Center;
        container.style.justifyContent = Justify.Center;
        container.style.backgroundColor = UIManager.ColorTheme.PanelForegroundColor;
        container.SetAllBorderRadius(5);
    }

    public override void Update()
    {
        if (character.Inventory != null && character.Inventory?.Version != seenInventoryVersion)
        {
            SetupInventory(character);
            seenInventoryVersion = character.Inventory?.Version ?? -1;
        }

        if (character.Conveyor != null && character.Conveyor?.Version != seenConveyorVersion)
        {
            SetupConveyor(character);
            seenConveyorVersion = character.Conveyor?.Version ?? -1;
        }
    }
}