namespace PkmGen1SaveEditor;

internal static class Gen1TypeCatalog
{
    public static string GetName(byte typeId) => typeId switch
    {
        0x00 => "Normal",
        0x01 => "Combat",
        0x02 => "Vol",
        0x03 => "Poison",
        0x04 => "Sol",
        0x05 => "Roche",
        0x06 => "Oiseau",
        0x07 => "Insecte",
        0x08 => "Spectre",
        0x14 => "Feu",
        0x15 => "Eau",
        0x16 => "Plante",
        0x17 => "Électrik",
        0x18 => "Psy",
        0x19 => "Glace",
        0x1A => "Dragon",
        _ => $"0x{typeId:X2}"
    };
}
