using System.Collections.Generic;
using System.Linq;
using Core;
using DG.Tweening;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMono : MonoBehaviour
{
    public Inventory? SelectedInventory;
    public int SelectedInventoryIndex;
    public Player Actual;
    public const float MovementSpeed = 5f;

    public Item? SelectedItem => SelectedInventory?.GetItemAt(SelectedInventoryIndex);
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

    void Start()
    {
        SetupControllable(); // Only controllable players have playerMono.
        var cinemachine = GameObject.Find("PlayerFollowCamera").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        cinemachine.LookAt = this.transform;
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
        Actual.World.SetUnitLocation(
            this.Actual.Id,
            this.transform.position.ToPoint3Float(),
            Point3Float.Zero);
    }

    void SetupControllable()
    {
        GetComponent<CharacterController>().enabled = true;
        GetComponent<ThirdPersonController>().enabled = true;
        GetComponent<BasicRigidBodyPush>().enabled = true;
        GetComponent<StarterAssetsInputs>().enabled = true;
        GetComponent<PlayerInput>().enabled = true;
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

    public void PluckBush(Point2Int pos)
    {
        Point3Int topHex = Actual.Context.World.GetTopHex(pos);
        float distance_sq = (WorldConversions.HexToUnityPosition(topHex) - this.transform.position).sqrMagnitude;


        Actual.Command!.ReplaceCommands(new MoveCommand(topHex.ToPoint3Float(), Actual));
        Actual.Command!.AddCommand(new PluckBushCommand(pos, Actual));
    }

    public void PickupItem(ulong id)
    {
        if (!Actual.Context.World.ItemObjects.ContainsKey(id))
        {
            return;
        }

        Point3Float itemPos = Actual.Context.World.ItemObjects[id].Position;
        Actual.Command!.ReplaceCommands(new MoveCommand(itemPos, Actual));
        Actual.Command!.AddCommand(new PickupItem(id, Actual));
    }
}