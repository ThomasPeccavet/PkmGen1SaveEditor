namespace PkmGen1SaveEditor;

internal sealed class Gen1PokedexEntry
{
    public required int DexNumber { get; init; }

    public required byte SpeciesId { get; init; }

    public required string SpeciesName { get; init; }

    public bool Seen { get; set; }

    public bool Caught { get; set; }
}
