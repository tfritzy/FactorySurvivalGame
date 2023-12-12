using System.Collections.Generic;
using Core;
using UnityEngine;

public enum UIElementType
{
    Vignette,
    Cursor,
    SearchIcon,
}

public static class UIElements
{
    private static Dictionary<UIElementType, Sprite> _elements = new Dictionary<UIElementType, Sprite>();
    public static Sprite GetElement(UIElementType type)
    {
        if (_elements == null)
            _elements = new Dictionary<UIElementType, Sprite>();

        if (!_elements.ContainsKey(type))
        {
            _elements[type] = Resources.Load<Sprite>("UIElements/" + type.ToString());
        }

        return _elements[type];
    }
}