using System;
using System.Collections.Generic;
using Core;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterInspector : Modal
{
    private int seenConveyorVersion = -1;
    private Character character;
    private InventoryGrid inventoryGrid;

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

            inventoryGrid = new InventoryGrid(
                 new InventoryGrid.Props
                 {
                     inventory = character.Inventory,
                     height = character.Inventory.Height,
                     width = character.Inventory.Width,
                     HideBorder = true,
                     SlotBorderWidth = 1,
                 }
             );
            sections[Section.Inventory].Add(inventoryGrid);
            sections[Section.Inventory].style.marginBottom = 10f;
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

            var conveyorElements = new ScrollView(ScrollViewMode.Horizontal)
            {
                verticalScrollerVisibility = ScrollerVisibility.Hidden,
                horizontalScrollerVisibility = ScrollerVisibility.Hidden,
            };
            if (character.Inventory != null)
            {
                conveyorElements.style.width = InventorySlot.Size * character.Inventory.Width;
            }
            sections[Section.Conveyor].Add(conveyorElements);

            int numPlaceholder = 4;
            int i = 0;
            foreach (var item in character.Conveyor.Items)
            {
                var container = new VisualElement();
                StyleConveyorItemContainer(container, i >= numPlaceholder - 1 && i == character.Conveyor.Items.Count - 1);

                var itemElement = new SlotItemIcon();
                itemElement.Update(item.Item);
                container.Add(itemElement);
                container.style.backgroundColor = UIManager.ColorTheme.OccupiedInventorySlot;
                conveyorElements.contentContainer.Add(container);
                i += 1;
            }

            for (; i < numPlaceholder; i++)
            {
                var container = new VisualElement();
                StyleConveyorItemContainer(container, i == numPlaceholder - 1);
                conveyorElements.contentContainer.Add(container);
            }
        }
    }

    private void StyleConveyorItemContainer(VisualElement container, bool isLast)
    {
        container.style.flexDirection = FlexDirection.Row;
        container.style.alignItems = Align.Center;
        container.style.justifyContent = Justify.Center;
        container.style.backgroundImage = new StyleBackground(UIElements.GetElement(UIElementType.Vignette));
        container.style.minWidth = InventorySlot.Size;
        container.style.minHeight = InventorySlot.Size;
        container.SetAllBorderRadius(5);
        container.SetAllBorderWidth(1);
        container.SetAllBorderColor(UIManager.ColorTheme.PanelOutlineColorMid);

        if (!isLast)
            container.style.marginRight = 10;
    }

    public override void Update()
    {
        inventoryGrid?.Update();

        if (character.Conveyor != null && character.Conveyor?.Version != seenConveyorVersion)
        {
            SetupConveyor(character);
            seenConveyorVersion = character.Conveyor?.Version ?? -1;
        }
    }
}