using System.Collections.Generic;
using UnityEngine;

public class MaterialDepot
{
    public enum Material
    {
        Invalid,
        Preview
    }

    private static Dictionary<Material, UnityEngine.Material> _materials = new();
    public static UnityEngine.Material GetMaterial(Material m)
    {
        if (!_materials.ContainsKey(m))
        {
            _materials.Add(m, Resources.Load<UnityEngine.Material>($"Materials/{m}"));
        }

        return _materials[m];
    }
}