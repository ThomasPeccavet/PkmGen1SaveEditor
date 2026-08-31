using System.Globalization;
using System.Text;

namespace PkmGen1SaveEditor;

internal sealed partial class Gen1SaveFile
{
    private const int PlayerIdOffset = 0x2605;
    private const int CurrentBoxNumberOffset = 0x284C;
    private const int CurrentBoxDataOffset = 0x30C0;

    private const int BoxDataSize = 0x462;
    private const int BoxSpeciesListRelativeOffset = 0x01;
    private const int BoxPokemonDataRelativeOffset = 0x16;
    private const int BoxOtNamesRelativeOffset = 0x2AA;
    private const int BoxNicknamesRelativeOffset = 0x386;
    private const int BoxPokemonSize = 33;
    private const int MaximumBoxSize = 20;
    private const int BoxCount = 12;

    private const int Bank2BoxesOffset = 0x4000;
    private const int Bank2ChecksumOffset = 0x5A4C;
    private const int Bank2IndividualChecksumsOffset = 0x5A4D;
    private const int Bank3BoxesOffset = 0x6000;
    private const int Bank3ChecksumOffset = 0x7A4C;
    private const int Bank3IndividualChecksumsOffset = 0x7A4D;

    public int CurrentBoxNumber =>
        (Data[CurrentBoxNumberOffset] & 0x7F) + 1;

    public IReadOnlyList<Gen1Pokemon> ReadBox(int boxNumber)
    {
        int boxOffset = GetAuthoritativeBoxOffset(boxNumber);
        int count = ReadBoxCount(boxOffset, boxNumber);
        List<Gen1Pokemon> pokemon = [];

        for (int index = 0; index < count; index++)
        {
            int pokemonOffset =
                boxOffset + BoxPokemonDataRelativeOffset +
                index * BoxPokemonSize;

            pokemon.Add(ReadPokemon(
                pokemonOffset,
                boxOffset + BoxNicknamesRelativeOffset + index * PartyNameSize,
                index + 1,
                boxNumber,
                includePartyStats: false));
        }

        return pokemon;
    }

    public IReadOnlyList<int> ReadBoxCounts()
    {
        int[] counts = new int[BoxCount];

        for (int boxNumber = 1; boxNumber <= BoxCount; boxNumber++)
        {
            int offset = GetAuthoritativeBoxOffset(boxNumber);
            counts[boxNumber - 1] = ReadBoxCount(offset, boxNumber);
        }

        return counts;
    }

    private Gen1Pokemon ReadPokemon(
        int pokemonOffset,
        int nicknameOffset,
        int slot,
        int? boxNumber,
        bool includePartyStats)
    {
        List<Gen1MoveSlot> moves = [];

        for (int moveIndex = 0; moveIndex < 4; moveIndex++)
        {
            byte ppData = Data[pokemonOffset + 29 + moveIndex];
            moves.Add(new Gen1MoveSlot
            {
                Slot = moveIndex + 1,
                MoveId = Data[pokemonOffset + 8 + moveIndex],
                CurrentPp = ppData & 0x3F,
                PpUps = (ppData >> 6) & 0x03
            });
        }

        return new Gen1Pokemon
        {
            Slot = slot,
            BoxNumber = boxNumber,
            SpeciesId = Data[pokemonOffset],
            Type1 = Data[pokemonOffset + 5],
            Type2 = Data[pokemonOffset + 6],
            CurrentHp = ReadBigEndianUInt16(pokemonOffset + 1),
            Status = DecodePokemonStatus(Data[pokemonOffset + 4]),
            OriginalTrainerId = ReadBigEndianUInt16(pokemonOffset + 12),
            Experience = ReadBigEndianUInt24(pokemonOffset + 14),
            Level = includePartyStats
                ? Data[pokemonOffset + 33]
                : Data[pokemonOffset + 3],
            MaximumHp = includePartyStats
                ? ReadBigEndianUInt16(pokemonOffset + 34)
                : (ushort)0,
            Attack = includePartyStats
                ? ReadBigEndianUInt16(pokemonOffset + 36)
                : (ushort)0,
            Defense = includePartyStats
                ? ReadBigEndianUInt16(pokemonOffset + 38)
                : (ushort)0,
            Speed = includePartyStats
                ? ReadBigEndianUInt16(pokemonOffset + 40)
                : (ushort)0,
            Special = includePartyStats
                ? ReadBigEndianUInt16(pokemonOffset + 42)
                : (ushort)0,
            Nickname = DecodeText(nicknameOffset, PartyNameSize),
            Moves = moves
        };
    }

    public void MovePartyPokemon(int fromIndex, int toIndex)
    {
        ValidatePartyIndex(fromIndex);
        ValidatePartyIndex(toIndex);

        if (fromIndex == toIndex)
            return;

        SwapBytes(PartySpeciesListOffset + fromIndex,
            PartySpeciesListOffset + toIndex, 1);
        SwapBytes(PartyPokemonDataOffset + fromIndex * PartyPokemonSize,
            PartyPokemonDataOffset + toIndex * PartyPokemonSize,
            PartyPokemonSize);
        SwapBytes(PartyOtNamesOffset + fromIndex * PartyNameSize,
            PartyOtNamesOffset + toIndex * PartyNameSize,
            PartyNameSize);
        SwapBytes(PartyNicknamesOffset + fromIndex * PartyNameSize,
            PartyNicknamesOffset + toIndex * PartyNameSize,
            PartyNameSize);

        UpdateChecksum();
    }

    public void DuplicatePartyPokemon(int partyIndex)
    {
        ValidatePartyIndex(partyIndex);

        if (!CanAddPartyPokemon)
            throw new InvalidOperationException("L'équipe contient déjà six Pokémon.");

        AppendPartyRecord(CapturePartyRecord(partyIndex));
        UpdateChecksum();
    }

    public void ReplacePartyPokemon(
        int partyIndex,
        byte speciesId,
        byte level,
        string? nickname)
    {
        ValidatePartyIndex(partyIndex);
        PokemonRecord newPokemon = CreatePokemonRecord(speciesId, level, nickname);
        WritePartyRecord(partyIndex, ConvertToPartyRecord(newPokemon));
        UpdateChecksum();
    }

    public void HealParty()
    {
        for (int index = 0; index < PartyCount; index++)
        {
            int offset = PartyPokemonDataOffset + index * PartyPokemonSize;
            ushort maximumHp = ReadBigEndianUInt16(offset + 34);
            WriteBigEndianUInt16(offset + 1, maximumHp);
            Data[offset + 4] = 0;

            for (int moveIndex = 0; moveIndex < 4; moveIndex++)
            {
                byte moveId = Data[offset + 8 + moveIndex];
                byte ppData = Data[offset + 29 + moveIndex];
                int ppUps = (ppData >> 6) & 0x03;
                int basePp = Gen1MoveCatalog.GetBasePp(moveId);
                int maximumPp = Math.Min(63, basePp * (5 + ppUps) / 5);
                Data[offset + 29 + moveIndex] =
                    (byte)((ppUps << 6) | maximumPp);
            }
        }

        UpdateChecksum();
    }

    public void DepositPartyPokemon(int partyIndex, int boxNumber)
    {
        ValidatePartyIndex(partyIndex);

        if (PartyCount <= 1)
        {
            throw new InvalidOperationException(
                "Le dernier Pokémon de l'équipe ne peut pas être déposé.");
        }

        PokemonRecord record = CapturePartyRecord(partyIndex);
        AppendBoxRecord(boxNumber, new PokemonRecord(
            record.PokemonData[..BoxPokemonSize],
            record.OtName,
            record.Nickname));
        DeletePartyPokemon(partyIndex);
        UpdateAllChecksums();
    }

    public void WithdrawBoxPokemon(int boxNumber, int boxIndex)
    {
        if (!CanAddPartyPokemon)
            throw new InvalidOperationException("L'équipe contient déjà six Pokémon.");

        PokemonRecord record = CaptureBoxRecord(boxNumber, boxIndex);
        AppendPartyRecord(ConvertToPartyRecord(record));
        DeleteBoxPokemon(boxNumber, boxIndex);
        UpdateAllChecksums();
    }

    public void MoveBoxPokemon(
        int sourceBoxNumber,
        int sourceIndex,
        int destinationBoxNumber)
    {
        if (sourceBoxNumber == destinationBoxNumber)
            return;

        PokemonRecord record = CaptureBoxRecord(sourceBoxNumber, sourceIndex);
        AppendBoxRecord(destinationBoxNumber, record);
        DeleteBoxPokemon(sourceBoxNumber, sourceIndex);
        UpdateAllChecksums();
    }

    public void AddBoxPokemon(
        int boxNumber,
        byte speciesId,
        byte level,
        string? nickname)
    {
        AppendBoxRecord(boxNumber, CreatePokemonRecord(speciesId, level, nickname));
        UpdateAllChecksums();
    }

    public void DeleteBoxPokemon(int boxNumber, int boxIndex)
    {
        int boxOffset = GetAuthoritativeBoxOffset(boxNumber);
        int count = ReadBoxCount(boxOffset, boxNumber);
        ValidateIndex(boxIndex, count, nameof(boxIndex));

        ShiftLeft(boxOffset + BoxSpeciesListRelativeOffset, 1,
            boxIndex, count, 0x00);
        ShiftLeft(boxOffset + BoxPokemonDataRelativeOffset, BoxPokemonSize,
            boxIndex, count, 0x00);
        ShiftLeft(boxOffset + BoxOtNamesRelativeOffset, PartyNameSize,
            boxIndex, count, 0x50);
        ShiftLeft(boxOffset + BoxNicknamesRelativeOffset, PartyNameSize,
            boxIndex, count, 0x50);

        int newCount = count - 1;
        Data[boxOffset] = (byte)newCount;
        Data[boxOffset + BoxSpeciesListRelativeOffset + newCount] = 0xFF;
        UpdateAllChecksums();
    }

    public void AddPartyPokemonCoherent(
        byte speciesId,
        byte level,
        string? nickname = null)
    {
        if (!CanAddPartyPokemon)
            throw new InvalidOperationException("L'équipe contient déjà six Pokémon.");

        AppendPartyRecord(ConvertToPartyRecord(
            CreatePokemonRecord(speciesId, level, nickname)));
        UpdateChecksum();
    }

    private PokemonRecord CreatePokemonRecord(
        byte speciesId,
        byte level,
        string? nickname)
    {
        if (level is < 1 or > 100)
            throw new ArgumentOutOfRangeException(nameof(level));

        Gen1SpeciesData species = Gen1SpeciesDataCatalog.Get(speciesId);
        byte[] pokemonData = new byte[BoxPokemonSize];
        pokemonData[0] = speciesId;
        pokemonData[3] = level;
        pokemonData[5] = species.Type1;
        pokemonData[6] = species.Type2;
        pokemonData[7] = species.CatchRate;

        for (int index = 0; index < 4; index++)
        {
            byte moveId = species.StartingMoves[index];
            pokemonData[8 + index] = moveId;
            pokemonData[29 + index] = Gen1MoveCatalog.GetBasePp(moveId);
        }

        ushort trainerId = ReadBigEndianUInt16(PlayerIdOffset);
        WriteBigEndianUInt16(pokemonData, 12, trainerId);
        WriteBigEndianUInt24(pokemonData, 14,
            Gen1SpeciesDataCatalog.GetExperience(species.GrowthRate, level));

        pokemonData[27] = (byte)Random.Shared.Next(0, 256);
        pokemonData[28] = (byte)Random.Shared.Next(0, 256);

        ushort hp = CalculateStats(pokemonData).MaximumHp;
        WriteBigEndianUInt16(pokemonData, 1, hp);

        byte[] otName = Data[PlayerNameOffset..(PlayerNameOffset + PartyNameSize)];
        string chosenNickname = string.IsNullOrWhiteSpace(nickname)
            ? MakeEncodablePokemonName(Gen1SpeciesCatalog.GetName(speciesId))
            : nickname.Trim();

        return new PokemonRecord(
            pokemonData,
            otName,
            EncodeTextBytes(chosenNickname));
    }

    private PokemonRecord ConvertToPartyRecord(PokemonRecord record)
    {
        byte[] partyData = new byte[PartyPokemonSize];
        Array.Copy(record.PokemonData, partyData,
            Math.Min(BoxPokemonSize, record.PokemonData.Length));

        byte level = partyData[3];
        PokemonStats stats = CalculateStats(partyData);
        partyData[33] = level;
        WriteBigEndianUInt16(partyData, 34, stats.MaximumHp);
        WriteBigEndianUInt16(partyData, 36, stats.Attack);
        WriteBigEndianUInt16(partyData, 38, stats.Defense);
        WriteBigEndianUInt16(partyData, 40, stats.Speed);
        WriteBigEndianUInt16(partyData, 42, stats.Special);

        ushort currentHp = ReadBigEndianUInt16(partyData, 1);
        if (currentHp > stats.MaximumHp)
            WriteBigEndianUInt16(partyData, 1, stats.MaximumHp);

        return new PokemonRecord(partyData, record.OtName, record.Nickname);
    }

    private PokemonStats CalculateStats(byte[] pokemonData)
    {
        Gen1SpeciesData species = Gen1SpeciesDataCatalog.Get(pokemonData[0]);
        int level = pokemonData[3];
        int attackDv = pokemonData[27] >> 4;
        int defenseDv = pokemonData[27] & 0x0F;
        int speedDv = pokemonData[28] >> 4;
        int specialDv = pokemonData[28] & 0x0F;
        int hpDv = ((attackDv & 1) << 3) |
                   ((defenseDv & 1) << 2) |
                   ((speedDv & 1) << 1) |
                   (specialDv & 1);

        return new PokemonStats(
            CalculateStat(species.BaseHp, hpDv,
                ReadBigEndianUInt16(pokemonData, 17), level, true),
            CalculateStat(species.BaseAttack, attackDv,
                ReadBigEndianUInt16(pokemonData, 19), level, false),
            CalculateStat(species.BaseDefense, defenseDv,
                ReadBigEndianUInt16(pokemonData, 21), level, false),
            CalculateStat(species.BaseSpeed, speedDv,
                ReadBigEndianUInt16(pokemonData, 23), level, false),
            CalculateStat(species.BaseSpecial, specialDv,
                ReadBigEndianUInt16(pokemonData, 25), level, false));
    }

    private static ushort CalculateStat(
        int baseStat,
        int dv,
        int statExperience,
        int level,
        bool hp)
    {
        int effort = (int)Math.Floor(Math.Sqrt(statExperience)) / 4;
        int value = (((baseStat + dv) * 2 + effort) * level) / 100;
        value += hp ? level + 10 : 5;
        return (ushort)Math.Clamp(value, 1, ushort.MaxValue);
    }

    private PokemonRecord CapturePartyRecord(int index)
    {
        ValidatePartyIndex(index);
        return new PokemonRecord(
            Slice(PartyPokemonDataOffset + index * PartyPokemonSize,
                PartyPokemonSize),
            Slice(PartyOtNamesOffset + index * PartyNameSize, PartyNameSize),
            Slice(PartyNicknamesOffset + index * PartyNameSize, PartyNameSize));
    }

    private PokemonRecord CaptureBoxRecord(int boxNumber, int index)
    {
        int boxOffset = GetAuthoritativeBoxOffset(boxNumber);
        int count = ReadBoxCount(boxOffset, boxNumber);
        ValidateIndex(index, count, nameof(index));

        return new PokemonRecord(
            Slice(boxOffset + BoxPokemonDataRelativeOffset + index * BoxPokemonSize,
                BoxPokemonSize),
            Slice(boxOffset + BoxOtNamesRelativeOffset + index * PartyNameSize,
                PartyNameSize),
            Slice(boxOffset + BoxNicknamesRelativeOffset + index * PartyNameSize,
                PartyNameSize));
    }

    private void AppendPartyRecord(PokemonRecord record)
    {
        if (!CanAddPartyPokemon)
            throw new InvalidOperationException("L'équipe contient déjà six Pokémon.");

        int index = PartyCount;
        WritePartyRecord(index, record);
        Data[PartyCountOffset] = (byte)(index + 1);
        Data[PartySpeciesListOffset + index + 1] = 0xFF;
    }

    private void WritePartyRecord(int index, PokemonRecord record)
    {
        if (record.PokemonData.Length != PartyPokemonSize)
            throw new ArgumentException("La structure d'équipe doit contenir 44 octets.");

        Data[PartySpeciesListOffset + index] = record.PokemonData[0];
        Array.Copy(record.PokemonData, 0, Data,
            PartyPokemonDataOffset + index * PartyPokemonSize, PartyPokemonSize);
        Array.Copy(record.OtName, 0, Data,
            PartyOtNamesOffset + index * PartyNameSize, PartyNameSize);
        Array.Copy(record.Nickname, 0, Data,
            PartyNicknamesOffset + index * PartyNameSize, PartyNameSize);
    }

    private void AppendBoxRecord(int boxNumber, PokemonRecord record)
    {
        int boxOffset = GetAuthoritativeBoxOffset(boxNumber);
        int count = ReadBoxCount(boxOffset, boxNumber);

        if (count >= MaximumBoxSize)
            throw new InvalidOperationException($"La boîte {boxNumber} est pleine.");

        if (record.PokemonData.Length < BoxPokemonSize)
            throw new ArgumentException("La structure PC est incomplète.");

        Data[boxOffset + BoxSpeciesListRelativeOffset + count] =
            record.PokemonData[0];
        Data[boxOffset + BoxSpeciesListRelativeOffset + count + 1] = 0xFF;
        Array.Copy(record.PokemonData, 0, Data,
            boxOffset + BoxPokemonDataRelativeOffset + count * BoxPokemonSize,
            BoxPokemonSize);
        Array.Copy(record.OtName, 0, Data,
            boxOffset + BoxOtNamesRelativeOffset + count * PartyNameSize,
            PartyNameSize);
        Array.Copy(record.Nickname, 0, Data,
            boxOffset + BoxNicknamesRelativeOffset + count * PartyNameSize,
            PartyNameSize);
        Data[boxOffset] = (byte)(count + 1);
    }

    private int GetAuthoritativeBoxOffset(int boxNumber)
    {
        ValidateBoxNumber(boxNumber);

        if (boxNumber == CurrentBoxNumber)
            return CurrentBoxDataOffset;

        return boxNumber <= 6
            ? Bank2BoxesOffset + (boxNumber - 1) * BoxDataSize
            : Bank3BoxesOffset + (boxNumber - 7) * BoxDataSize;
    }

    private int ReadBoxCount(int boxOffset, int boxNumber)
    {
        int count = Data[boxOffset];

        if (count > MaximumBoxSize)
        {
            throw new InvalidDataException(
                $"Le nombre de Pokémon de la boîte {boxNumber} est invalide : {count}.");
        }

        return count;
    }

    private void UpdateAllChecksums()
    {
        UpdateChecksum();

        Data[Bank2ChecksumOffset] = CalculateRangeChecksum(
            Bank2BoxesOffset, BoxDataSize * 6);
        Data[Bank3ChecksumOffset] = CalculateRangeChecksum(
            Bank3BoxesOffset, BoxDataSize * 6);

        for (int index = 0; index < 6; index++)
        {
            Data[Bank2IndividualChecksumsOffset + index] =
                CalculateRangeChecksum(
                    Bank2BoxesOffset + index * BoxDataSize, BoxDataSize);
            Data[Bank3IndividualChecksumsOffset + index] =
                CalculateRangeChecksum(
                    Bank3BoxesOffset + index * BoxDataSize, BoxDataSize);
        }
    }

    private byte CalculateRangeChecksum(int offset, int length)
    {
        int sum = 0;
        for (int index = 0; index < length; index++)
            sum = (sum + Data[offset + index]) & 0xFF;
        return (byte)(sum ^ 0xFF);
    }

    private void ValidatePartyIndex(int index) =>
        ValidateIndex(index, PartyCount, nameof(index));

    private static void ValidateIndex(int index, int count, string parameterName)
    {
        if (index < 0 || index >= count)
            throw new ArgumentOutOfRangeException(parameterName);
    }

    private static void ValidateBoxNumber(int boxNumber)
    {
        if (boxNumber is < 1 or > BoxCount)
            throw new ArgumentOutOfRangeException(nameof(boxNumber));
    }

    private void SwapBytes(int firstOffset, int secondOffset, int length)
    {
        byte[] temporary = Slice(firstOffset, length);
        Array.Copy(Data, secondOffset, Data, firstOffset, length);
        Array.Copy(temporary, 0, Data, secondOffset, length);
    }

    private void ShiftLeft(
        int startOffset,
        int elementSize,
        int removedIndex,
        int elementCount,
        byte emptyValue)
    {
        for (int index = removedIndex; index < elementCount - 1; index++)
        {
            Array.Copy(Data,
                startOffset + (index + 1) * elementSize,
                Data,
                startOffset + index * elementSize,
                elementSize);
        }

        Array.Fill(Data, emptyValue,
            startOffset + (elementCount - 1) * elementSize,
            elementSize);
    }

    private byte[] Slice(int offset, int length)
    {
        byte[] result = new byte[length];
        Array.Copy(Data, offset, result, 0, length);
        return result;
    }

    private static ushort ReadBigEndianUInt16(byte[] data, int offset) =>
        (ushort)((data[offset] << 8) | data[offset + 1]);

    private static void WriteBigEndianUInt16(
        byte[] data,
        int offset,
        ushort value)
    {
        data[offset] = (byte)(value >> 8);
        data[offset + 1] = (byte)value;
    }

    private static void WriteBigEndianUInt24(
        byte[] data,
        int offset,
        uint value)
    {
        data[offset] = (byte)(value >> 16);
        data[offset + 1] = (byte)(value >> 8);
        data[offset + 2] = (byte)value;
    }

    private static byte[] EncodeTextBytes(string text)
    {
        text = text.Trim();
        if (text.Length is < 1 or >= PartyNameSize)
            throw new ArgumentException("Le nom doit contenir entre 1 et 10 caractères.");

        byte[] result = new byte[PartyNameSize];
        Array.Fill(result, (byte)0x50);
        for (int index = 0; index < text.Length; index++)
            result[index] = EncodeCharacter(text[index]);
        return result;
    }

    private sealed record PokemonRecord(
        byte[] PokemonData,
        byte[] OtName,
        byte[] Nickname);

    private sealed record PokemonStats(
        ushort MaximumHp,
        ushort Attack,
        ushort Defense,
        ushort Speed,
        ushort Special);
}
