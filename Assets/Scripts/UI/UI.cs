using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    private VisualElement root;
    private InGameHud inGameHud;

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        this.inGameHud = new InGameHud();
        root.Add(this.inGameHud);
    }

    void Update()
    {
        this.inGameHud.Update();
    }
}
