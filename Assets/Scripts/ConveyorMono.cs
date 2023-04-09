using System.Collections.Generic;
using Core;
using UnityEngine;

public class ConveyorMono : CharacterMono
{
    private Conveyor conveyor => (Conveyor)this.Actual;
    private LinkedList<ItemMono> itemBodies = new LinkedList<ItemMono>();


    public override void Spawn()
    {
        base.Spawn();
    }

    override public void Despawn()
    {
        base.Despawn();

        foreach (ItemMono itemMono in this.itemBodies)
        {
            itemMono.Despawn();
        }
    }

    public override void Tick(float deltaTime)
    {
        MaintainBodyParity();

        var current = conveyor.ConveyorComponent.Items.First;
        var currentBody = itemBodies.First;
        while (current != null)
        {
            currentBody.Value.transform.position = this.transform.position + Vector3.up;
            current = current.Next;
            currentBody = currentBody.Next;
        }
    }

    private void MaintainBodyParity()
    {

        if (itemBodies.Count == 0 || conveyor.ConveyorComponent.Items.Count == 0)
        {
            FullReset();
            return;
        }

        int deltaCount = itemBodies.Count - conveyor.ConveyorComponent.Items.Count;

        if (deltaCount == 1)
        {
            if (itemBodies.First.Value.Actual.Type != conveyor.ConveyorComponent.Items.First.Value.Item.Type)
            {
                itemBodies.First.Value.Despawn();
                itemBodies.RemoveFirst();
            }
            else if (itemBodies.Last.Value.Actual.Type != conveyor.ConveyorComponent.Items.Last.Value.Item.Type)
            {
                itemBodies.Last.Value.Despawn();
                itemBodies.RemoveLast();
            }
            else
            {
                FullReset();
                return;
            }
        }
        else if (deltaCount == -1)
        {
            if (itemBodies.First.Value.Actual.Type != conveyor.ConveyorComponent.Items.First.Value.Item.Type)
            {
                ItemMono item = BuildItem(conveyor.ConveyorComponent.Items.First.Value.Item);
                itemBodies.AddFirst(item);
            }
            else if (itemBodies.Last.Value.Actual.Type != conveyor.ConveyorComponent.Items.Last.Value.Item.Type)
            {
                ItemMono item = BuildItem(conveyor.ConveyorComponent.Items.Last.Value.Item);
                itemBodies.AddLast(item);
            }
            else
            {
                FullReset();
                return;
            }
        }
        else if (System.Math.Abs(deltaCount) > 1)
        {
            FullReset();
            return;
        }
    }

    private ItemMono BuildItem(Item item)
    {
        GameObject itemShell = new GameObject("Item");
        ItemMono itemMono = itemShell.AddComponent<ItemMono>();
        itemMono.Actual = item;
        itemMono.transform.position = this.transform.position;
        itemMono.Spawn();
        return itemMono;
    }

    private void FullReset()
    {
        foreach (ItemMono itemMono in itemBodies)
        {
            itemMono.Despawn();
        }

        itemBodies = new LinkedList<ItemMono>();
        foreach (ItemOnBelt conveyorItem in conveyor.ConveyorComponent.Items)
        {
            ItemMono item = BuildItem(conveyorItem.Item);
            itemBodies.AddLast(item);
        }
    }
}