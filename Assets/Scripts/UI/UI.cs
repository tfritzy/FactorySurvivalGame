using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    private VisualElement root;
    private InventoryDrawer inventoryDrawer;

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        this.inventoryDrawer = new InventoryDrawer();
        root.Add(this.inventoryDrawer);
    }

    void Update()
    {
        this.inventoryDrawer.Update();
    }
}
