namespace PkmGen1SaveEditor;

internal sealed record Gen1StatBlock(
    ushort MaximumHp,
    ushort Attack,
    ushort Defense,
    ushort Speed,
    ushort Special);
