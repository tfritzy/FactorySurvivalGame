using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

public class ConveyorMono : CharacterMono
{
    private Building owner => (Building)this.Actual;
    private ConveyorComponent conveyor => owner.GetComponent<ConveyorComponent>();
    private Building next => (Building)conveyor.Next?.Owner;
    private Building prev => (Building)conveyor.Prev?.Owner;
    private LinkedList<ItemMono> itemBodies = new LinkedList<ItemMono>();
    private List<Vector3> path = new List<Vector3>();
    private List<float> pathProgress = new List<float>();

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
        UpdatePath();

        var current = conveyor.Items.First;
        var currentBody = itemBodies.First;
        while (current != null)
        {
            currentBody.Value.transform.position = GetCalculatedPosition(current.Value);
            current = current.Next;
            currentBody = currentBody.Next;
        }
    }

    private Vector3 GetCalculatedPosition(ItemOnBelt item)
    {
        if (path.Count == 0)
        {
            return this.transform.position;
        }

        float progress = item.ProgressMeters;
        int index = 0;
        while (progress < item.ProgressMeters && index < pathProgress.Count)
        {
            progress += pathProgress[index];
            index++;
        }

        if (index >= pathProgress.Count)
        {
            return path.Last();
        }

        Debug.Log($"Progress: {progress}, index: {index}");
        float progressAlongSegment = item.ProgressMeters - pathProgress[index - 1];
        float segmentLength = pathProgress[index] - pathProgress[index - 1];
        return Vector3.Lerp(path[index - 1], path[index], progressAlongSegment / segmentLength);
    }

    private void MaintainBodyParity()
    {

        if (itemBodies.Count == 0 || conveyor.Items.Count == 0)
        {
            FullReset();
            return;
        }

        int deltaCount = itemBodies.Count - conveyor.Items.Count;

        if (deltaCount == 1)
        {
            if (itemBodies.First.Value.Actual.Type != conveyor.Items.First.Value.Item.Type)
            {
                itemBodies.First.Value.Despawn();
                itemBodies.RemoveFirst();
            }
            else if (itemBodies.Last.Value.Actual.Type != conveyor.Items.Last.Value.Item.Type)
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
            if (itemBodies.First.Value.Actual.Type != conveyor.Items.First.Value.Item.Type)
            {
                ItemMono item = BuildItem(conveyor.Items.First.Value.Item);
                itemBodies.AddFirst(item);
            }
            else if (itemBodies.Last.Value.Actual.Type != conveyor.Items.Last.Value.Item.Type)
            {
                ItemMono item = BuildItem(conveyor.Items.Last.Value.Item);
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
        foreach (ItemOnBelt conveyorItem in conveyor.Items)
        {
            ItemMono item = BuildItem(conveyorItem.Item);
            itemBodies.AddLast(item);
        }
    }

    private HexSide? cachedPathInputSide;
    private HexSide? cachedPathOutputSide;
    private void UpdatePath()
    {
        if (cachedPathInputSide == conveyor.PrevSide &&
            cachedPathOutputSide == conveyor.NextSide)
        {
            return;
        }

        Debug.Log("Calculating path");
        cachedPathInputSide = conveyor.PrevSide;
        cachedPathOutputSide = conveyor.NextSide;

        Vector3 center = WorldConversions.HexToUnityPosition(WorldConversions.TopPosOf2D(owner.GridPosition));
        Vector3 prevPos = center;
        Vector3 nextPos = center;

        if (this.prev != null)
        {
            prevPos = WorldConversions.HexToUnityPosition(WorldConversions.TopPosOf2D(prev.GridPosition));

            if (next == null)
            {
                Vector3 delta = prevPos - center;
                nextPos = center - delta;
            }
        }

        if (this.next != null)
        {
            nextPos = WorldConversions.HexToUnityPosition(WorldConversions.TopPosOf2D(next.GridPosition));

            if (prev == null)
            {
                Vector3 delta = nextPos - center;
                prevPos = center - delta;
            }
        }

        this.path = new List<Vector3>
        {
            prevPos,
            center,
            nextPos
        };

        this.pathProgress = new List<float>(path.Count);
        pathProgress.Add(0f);
        for (int i = 1; i < path.Count; i++)
        {
            pathProgress.Add(pathProgress[i - 1] + Vector3.Distance(path[i - 1], path[i]));
        }
    }
}