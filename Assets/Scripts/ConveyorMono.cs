using System.Collections.Generic;
using Core;
using UnityEngine;

public class ConveyorMono : CharacterMono
{
    private Conveyor conveyor => (Conveyor)this.Actual;

    private List<ItemMono> items = new List<ItemMono>();

    public override void Spawn()
    {
        base.Spawn();

        foreach (ItemOnBelt item in this.conveyor.ConveyorComponent.Items)
        {
            GameObject itemShell = new GameObject("Item");
            ItemMono itemMono = itemShell.AddComponent<ItemMono>();
            itemMono.Actual = item.Item;
            itemMono.Spawn();
            itemMono.transform.position = this.transform.position;
            this.items.Add(itemMono);
        }
    }

    override public void Despawn()
    {
        base.Despawn();

        foreach (ItemMono itemMono in this.items)
        {
            itemMono.Despawn();
        }
    }
}