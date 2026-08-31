namespace PkmGen1SaveEditor;

internal sealed class Gen1Pokemon
{
    public int Slot { get; init; }

    public byte SpeciesId { get; init; }

    public string Nickname { get; init; } = string.Empty;

    public byte Level { get; init; }

    public ushort CurrentHp { get; init; }

    public ushort MaximumHp { get; init; }

    public ushort Attack { get; init; }

    public ushort Defense { get; init; }

    public ushort Speed { get; init; }

    public ushort Special { get; init; }

    public ushort OriginalTrainerId { get; init; }

    public uint Experience { get; init; }

    public string Status { get; init; } = string.Empty;

    public IReadOnlyList<Gen1MoveSlot> Moves { get; init; } =
        Array.Empty<Gen1MoveSlot>();

    public string SpeciesName =>
        Gen1SpeciesCatalog.GetName(SpeciesId);

    public override string ToString()
    {
        return string.IsNullOrWhiteSpace(Nickname)
            ? SpeciesName
            : Nickname;
    }
}