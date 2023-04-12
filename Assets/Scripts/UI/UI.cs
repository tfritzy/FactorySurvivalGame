using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    private VisualElement root;

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        root.Add(new InventoryDrawer());
    }
}
