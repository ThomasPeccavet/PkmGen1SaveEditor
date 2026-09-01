namespace PkmGen1SaveEditor;

internal sealed partial class Gen1SaveFile
{
    private const int PokedexCaughtOffset = 0x25A3;
    private const int PokedexSeenOffset = 0x25B6;
    private const int PokedexSpeciesCount = 151;
    private const int PokedexByteCount = 19;

    public IReadOnlyList<Gen1PokedexEntry> ReadPokedex()
    {
        return Gen1SpeciesCatalog.GetAll()
            .Select(species => new
            {
                species.Id,
                species.Name,
                DexNumber = Gen1SpeciesCatalog.GetNationalDexNumber(species.Id)
            })
            .Where(species => species.DexNumber is >= 1 and <= PokedexSpeciesCount)
            .OrderBy(species => species.DexNumber)
            .Select(species => new Gen1PokedexEntry
            {
                DexNumber = species.DexNumber,
                SpeciesId = species.Id,
                SpeciesName = species.Name,
                Seen = ReadPokedexFlag(PokedexSeenOffset, species.DexNumber),
                Caught = ReadPokedexFlag(PokedexCaughtOffset, species.DexNumber)
            })
            .ToArray();
    }

    public void SetPokedex(IReadOnlyList<Gen1PokedexEntry> entries)
    {
        ArgumentNullException.ThrowIfNull(entries);

        if (entries.Count != PokedexSpeciesCount ||
            entries.Select(entry => entry.DexNumber).Distinct().Count() != PokedexSpeciesCount ||
            entries.Any(entry => entry.DexNumber is < 1 or > PokedexSpeciesCount))
        {
            throw new ArgumentException(
                "Le Pokédex doit contenir exactement les 151 espèces.",
                nameof(entries));
        }

        Array.Fill(Data, (byte)0x00, PokedexCaughtOffset, PokedexByteCount);
        Array.Fill(Data, (byte)0x00, PokedexSeenOffset, PokedexByteCount);

        foreach (Gen1PokedexEntry entry in entries)
        {
            bool seen = entry.Seen || entry.Caught;
            WritePokedexFlag(PokedexSeenOffset, entry.DexNumber, seen);
            WritePokedexFlag(PokedexCaughtOffset, entry.DexNumber, entry.Caught);
        }

        UpdateChecksum();
    }

    private bool ReadPokedexFlag(int arrayOffset, int dexNumber)
    {
        int bitIndex = dexNumber - 1;
        int byteOffset = arrayOffset + bitIndex / 8;
        int bitMask = 1 << (bitIndex % 8);

        return (Data[byteOffset] & bitMask) != 0;
    }

    private void WritePokedexFlag(
        int arrayOffset,
        int dexNumber,
        bool enabled)
    {
        int bitIndex = dexNumber - 1;
        int byteOffset = arrayOffset + bitIndex / 8;
        byte bitMask = (byte)(1 << (bitIndex % 8));

        if (enabled)
            Data[byteOffset] |= bitMask;
        else
            Data[byteOffset] &= (byte)~bitMask;
    }
}
