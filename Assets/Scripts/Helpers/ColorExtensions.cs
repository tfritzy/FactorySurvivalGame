public class ColorExtensions
{
    public static UnityEngine.Color FromHex(string hex)
    {
        if (hex.StartsWith("#"))
        {
            hex = hex.Substring(1);
        }
        if (hex.StartsWith("0x"))
        {
            hex = hex.Substring(2);
        }
        if (hex.Length > 8)
        {
            throw new System.Exception("Invalid hex string length");
        }
        var r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        var g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        var b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        var a =
            hex.Length == 8 ?
            byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber) :
            (byte)255;
        return new UnityEngine.Color32(r, g, b, 255);
    }
}