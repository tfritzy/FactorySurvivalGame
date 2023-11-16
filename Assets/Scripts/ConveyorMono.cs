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
    private GameObject _straightBody;
    public GameObject StraightBody
    {
        get
        {
            if (_straightBody == null)
            {
                _straightBody = this.transform.Find("Straight").gameObject;
            }
            return _straightBody;

        }
    }
    private GameObject _curvedBody;
    public GameObject CurvedBody
    {
        get
        {
            if (_curvedBody == null)
            {
                _curvedBody = this.transform.Find("Curved").gameObject;
            }
            return _curvedBody;

        }
    }

    public override void Setup(Entity entity)
    {
        base.Setup(entity);
        UpdateOwnBody();
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);

        MaintainBodyParity();
        UpdatePath();
        UpdateOwnBody();

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

        float progress = 0f;
        int index = 0;
        while (progress < item.ProgressMeters && index < pathProgress.Count)
        {
            progress += pathProgress[index];
            index++;
        }
        index -= 1;

        float progressAlongSegment = item.ProgressMeters - pathProgress[index - 1];
        float segmentLength = pathProgress[index] - pathProgress[index - 1];
        return Vector3.Lerp(path[index - 1], path[index], progressAlongSegment / segmentLength);
    }

    private bool? cachedCurved;
    private HexSide? cachedPrev;
    private HexSide? cachedNext;
    public void UpdateOwnBody()
    {
        if (Actual.Conveyor.IsCurved() != cachedCurved ||
            Actual.Conveyor.PrevSide != cachedPrev ||
            Actual.Conveyor.NextSide != cachedNext)
        {
            Debug.Log(
                "Conveyor at " + ((Building)Actual).GridPosition +
                " has next: " + Actual.Conveyor.NextSide +
                " and prev: " + Actual.Conveyor.PrevSide +
                " and is curved: " + Actual.Conveyor.IsCurved());
            cachedNext = Actual.Conveyor.NextSide;
            cachedPrev = Actual.Conveyor.PrevSide;
            cachedCurved = Actual.Conveyor.IsCurved();
            if (cachedCurved.Value)
            {
                CurvedBody.SetActive(true);
                StraightBody.gameObject.SetActive(false);

                int inSide = (int)Actual.Conveyor.PrevSide;
                int outSide = (int)Actual.Conveyor.NextSide;
                if (outSide < 2 && inSide > 3)
                    outSide += 6;
                if (inSide > 3 && outSide < 2)
                    inSide -= 6;
                int delta = outSide - inSide;
                bool flipped = delta != 2;

                Debug.Log("Has curved prev side on: " + Actual.Conveyor.PrevSide);
                var rotation = ((int)Actual.Conveyor.PrevSide + (flipped ? 4 : 0)) * 60;
                CurvedBody.transform.rotation = Quaternion.Euler(0, rotation, 0);
            }
            else
            {
                StraightBody.gameObject.SetActive(true);
                CurvedBody.SetActive(false);

                if (Actual.Conveyor.PrevSide != null)
                {
                    Debug.Log("Has prev side on: " + Actual.Conveyor.PrevSide);
                    var rotation = ((int)Actual.Conveyor.PrevSide + 3) * 60;
                    StraightBody.transform.rotation = Quaternion.Euler(0, rotation, 0);
                }
                else if (Actual.Conveyor.NextSide != null)
                {
                    Debug.Log("Has next side on: " + Actual.Conveyor.NextSide);
                    var rotation = (int)Actual.Conveyor.NextSide * 60;
                    StraightBody.transform.rotation = Quaternion.Euler(0, rotation, 0);
                }
            }
        }
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
                // itemBodies.First.Value.Despawn();
                itemBodies.RemoveFirst();
            }
            else if (itemBodies.Last.Value.Actual.Type != conveyor.Items.Last.Value.Item.Type)
            {
                // itemBodies.Last.Value.Despawn();
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
        // itemMono.Spawn();
        return itemMono;
    }

    private void FullReset()
    {
        foreach (ItemMono itemMono in itemBodies)
        {
            // itemMono.Despawn();
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

        cachedPathInputSide = conveyor.PrevSide;
        cachedPathOutputSide = conveyor.NextSide;

        Vector3 center = WorldConversions.HexToUnityPosition(owner.GridPosition);
        Vector3 prev = center;
        Vector3 next = center;

        if (this.prev != null)
        {
            Vector3 prevConveyor = WorldConversions.HexToUnityPosition(this.prev.GridPosition);
            Vector3 delta = (prevConveyor - center) / 2;
            prev = center + delta;

            if (this.next == null)
            {
                next = center - delta;
            }
        }

        if (this.next != null)
        {
            Vector3 nextConveyor = WorldConversions.HexToUnityPosition(this.next.GridPosition);
            Vector3 delta = (nextConveyor - center) / 2;
            next = center + delta;

            if (this.prev == null)
            {
                prev = center - delta;
            }
        }

        this.path = new List<Vector3>
        {
            prev,
            center,
            next
        };

        for (int i = 0; i < path.Count; i++)
        {
            path[i] += new Vector3(0f, 0.5f, 0f);
        }

        this.pathProgress = new List<float>(path.Count);
        pathProgress.Add(0f);
        for (int i = 1; i < path.Count; i++)
        {
            pathProgress.Add(pathProgress[i - 1] + Vector3.Distance(path[i - 1], path[i]));
        }
    }
}