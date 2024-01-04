using System.Collections.Generic;
using System.Linq;
using Core;
using DG.Tweening;
using UnityEngine;

public class PlayerMono : MonoBehaviour
{
    public Inventory SelectedInventory;
    public int SelectedInventoryIndex;
    public Player Actual;
    public const float MovementSpeed = 5f;

    public Item? SelectedItem => SelectedInventory.GetItemAt(SelectedInventoryIndex);
    private Building? previewBuilding;
    private Dictionary<Point2Int, GameObject> conveyorArrows = new();
    private PreviewBlockState blockState = new PreviewBlockState();
    private class PreviewBlockState
    {
        public Point3Int? pos;
        public HexSide? side;
        public ItemType? type;
        public GameObject? block;
    }

    private static PlayerMono? instance;
    public static PlayerMono Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindAnyObjectByType<PlayerMono>();
            }
            return instance;
        }
    }

    void Awake()
    {
        Actual = new Player(WorldMono.Instance.Context, 0);
        WorldMono.Instance.World.AddCharacter(Actual);
        SelectedInventory = Actual.GetComponent<ActiveItems>();
        SelectedInventoryIndex = 0;
        InputManager.Instance.RegisterKeyDown(KeyCode.RightArrow, () => IncrementInventoryIndex(1));
        InputManager.Instance.RegisterKeyDown(KeyCode.LeftArrow, () => IncrementInventoryIndex(-1));
        InputManager.Instance.RegisterKeyDown(
            KeyCode.UpArrow,
            () => IncrementInventoryIndex(-SelectedInventory.Width));
        InputManager.Instance.RegisterKeyDown(
            KeyCode.DownArrow,
            () => IncrementInventoryIndex(SelectedInventory.Width));

#if UNITY_EDITOR
        Cursor.SetCursor(UIElements.GetElement(UIElementType.Cursor).texture, Vector2.zero, CursorMode.ForceSoftware);
#endif
    }

    void Update()
    {
        Actual.Context.Api.SetUnitLocation(Actual.Id, this.transform.position.ToPoint3Float());
        Vector3 movementVector = GetMovementVector();
        this.transform.position += movementVector;
        ObeyCommands();
    }

    private void UpdateArrows()
    {
        ClearArrows();

        if (previewBuilding?.Conveyor != null)
        {
            var arrow = Instantiate(DecalLoader.GetDecalPrefab(DecalLoader.Decal.Arrow));
            arrow.transform.position = WorldConversions.HexToUnityPosition(previewBuilding.GridPosition);
            arrow.transform.rotation = Quaternion.Euler(0, 60 * (int)previewBuilding.Rotation, 0);
            conveyorArrows.Add((Point2Int)previewBuilding.GridPosition, arrow);
        }
    }

    private void ClearArrows()
    {
        foreach (var arrow in conveyorArrows.Values)
        {
            Destroy(arrow);
        }
        conveyorArrows.Clear();
    }

    private void IncrementInventoryIndex(int amount)
    {
        SelectedInventoryIndex += amount;
        if (SelectedInventoryIndex > SelectedInventory.Size)
        {
            SelectedInventoryIndex %= SelectedInventory.Size;
        }

        if (SelectedInventoryIndex < 0)
        {
            SelectedInventoryIndex = SelectedInventory.Size + SelectedInventoryIndex;
        }
    }

    public void MoveCommand(Vector3 pos)
    {
        Actual.Command!.ReplaceCommands(
            new MoveCommand(
                pos.ToPoint3Float(),
                Actual));
    }

    public void PluckBush(Point2Int pos)
    {
        Debug.Log("Player given pluck command");
        Point3Int topHex = Actual.Context.World.GetTopHex(pos);
        Actual.Command!.ReplaceCommands(new MoveCommand(topHex.ToPoint3Float(), Actual));
        Actual.Command!.AddCommand(new PluckBushCommand(pos, Actual));
    }

    public void PickupItem(ulong id)
    {
        Debug.Log("Player given pickup command");
        if (!Actual.Context.World.ItemObjects.ContainsKey(id))
        {
            return;
        }

        Point3Float itemPos = Actual.Context.World.ItemObjects[id].Position;
        Actual.Command!.ReplaceCommands(new MoveCommand(itemPos, Actual));
        Actual.Command!.AddCommand(new PickupItem(id, Actual));
    }

    private void ObeyCommands()
    {
        if (Actual.Command == null)
        {
            return;
        }

        if (Actual.Command.CurrentCommand is MoveCommand moveCommand)
        {
            Vector3 delta = (moveCommand.TargetPosition - Actual.Location).ToVector3().normalized * MovementSpeed;
            this.transform.position += delta * Time.deltaTime;
        }
        else if (Actual.Command.CurrentCommand is PluckBushCommand pluck)
        {
            Actual.Context.Api.PluckBush(Actual.Id, pluck.Pos);
        }
        else if (Actual.Command.CurrentCommand is PickupItem pickup)
        {
            Actual.Context.Api.PickupItem(Actual.Id, pickup.Id);
        }
    }

    private Vector3 GetMovementVector()
    {
        Vector3 movement = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            movement += Vector3.forward;
        }

        if (Input.GetKey(KeyCode.D))
        {
            movement += Vector3.right;
        }

        if (Input.GetKey(KeyCode.S))
        {
            movement += Vector3.back;
        }

        if (Input.GetKey(KeyCode.A))
        {
            movement += Vector3.left;
        }

        if (movement != Vector3.zero && Actual.Command?.CurrentCommand != null)
        {
            Actual.Command?.ClearCommands();
        }

        movement = movement.normalized * MovementSpeed * Time.deltaTime;
        return movement;
    }
}