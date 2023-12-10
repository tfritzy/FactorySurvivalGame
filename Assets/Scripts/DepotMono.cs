using Core;
using UnityEngine;

public class DepotMono : BuildingMono
{
    private Renderer lightRenderer;
    private float targetFilledPercent = 0f;
    private int seenVersion = -1;
    private const string FilledPercent = "_FillAmount";
    public bool ShouldAddItem;

    public override void Setup(Entity entity)
    {
        base.Setup(entity);

        lightRenderer = transform.Find("Depot/Light").GetComponent<Renderer>();
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);

        if (ShouldAddItem)
        {
            Actual.Inventory.AddItem(new IronOre());
            ShouldAddItem = false;
        }

        lightRenderer.material.SetFloat(
            FilledPercent,
            Mathf.Lerp(lightRenderer.material.GetFloat(FilledPercent), targetFilledPercent, 0.1f));

        if (seenVersion != Actual.Inventory.Version)
        {
            seenVersion = Actual.Inventory.Version;
            targetFilledPercent =
                (Actual.Inventory.Size - Actual.Inventory.NumOpenSlots()) / (float)Actual.Inventory.Size;
        }
    }
}