namespace PkmGen1SaveEditor;

internal sealed class Gen1Pokemon
{
    public int Slot { get; init; }

    public byte SpeciesId { get; init; }

    public byte Type1 { get; init; }

    public byte Type2 { get; init; }

    public int? BoxNumber { get; init; }

    public bool IsInParty => BoxNumber is null;

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

    public ushort HpEv { get; init; }

    public ushort AttackEv { get; init; }

    public ushort DefenseEv { get; init; }

    public ushort SpeedEv { get; init; }

    public ushort SpecialEv { get; init; }

    public byte AttackDv { get; init; }

    public byte DefenseDv { get; init; }

    public byte SpeedDv { get; init; }

    public byte SpecialDv { get; init; }

    public byte HpDv => (byte)(
        ((AttackDv & 1) << 3) |
        ((DefenseDv & 1) << 2) |
        ((SpeedDv & 1) << 1) |
        (SpecialDv & 1));

    public string Status { get; init; } = string.Empty;

    public IReadOnlyList<Gen1MoveSlot> Moves { get; init; } =
        Array.Empty<Gen1MoveSlot>();

    public string SpeciesName =>
        Gen1SpeciesCatalog.GetName(SpeciesId);

    public string Types => Type1 == Type2
        ? Gen1TypeCatalog.GetName(Type1)
        : $"{Gen1TypeCatalog.GetName(Type1)} / {Gen1TypeCatalog.GetName(Type2)}";

    public string MovesSummary => string.Join(
        ", ",
        Moves.Where(move => !move.IsEmpty)
            .Select(move => Gen1MoveCatalog.GetName(move.MoveId)));

    public override string ToString()
    {
        return string.IsNullOrWhiteSpace(Nickname)
            ? SpeciesName
            : Nickname;
    }
}
