using Core;
using UnityEngine;

public class SorterMono : BuildingMono
{
    public ItemType FilterType;
    private GameObject? renderType;

    void Update()
    {
        if (((Sorter)Actual).Filter != FilterType)
        {
            ((Sorter)Actual).Filter = FilterType;
            UpdateRenderType();
        }
    }

    private void UpdateRenderType()
    {
        if (renderType != null)
        {
            Destroy(renderType);
        }

        if (FilterType == ItemType.InvalidItemType)
        {
            return;
        }

        renderType = Instantiate(Models.GetItemModel(FilterType), this.transform);
        renderType.transform.localPosition = Vector3.zero + Vector3.up * .5f;
    }
}