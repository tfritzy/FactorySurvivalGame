public static class Layers
{
    public const int Hex = 6;
    public const int HexMask = 1 << Hex;

    public const int Character = 7;
    public const int CharacterMask = 1 << Character;

    public const int Vegetation = 8;
    public const int VegetationMask = 1 << Vegetation;

    public const int Item = 9;
    public const int ItemMask = 1 << Item;

    public const int InteractableLayersMask = CharacterMask | VegetationMask | ItemMask;
}