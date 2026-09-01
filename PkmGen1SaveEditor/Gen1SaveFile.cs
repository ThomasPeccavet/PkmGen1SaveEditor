using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Text;

namespace PkmGen1SaveEditor;

internal sealed partial class Gen1SaveFile
{
    public const int ExpectedSize = 32 * 1024;

    // Informations du dresseur
    private const int PlayerNameOffset = 0x2598;
    private const int RivalNameOffset = 0x25F6;
    private const int MoneyOffset = 0x25F3;
    private const int BadgesOffset = 0x2602;

    // Temps de jeu
    private const int PlayTimeHoursOffset = 0x2CED;
    private const int PlayTimeMaxedOffset = 0x2CEE;
    private const int PlayTimeMinutesOffset = 0x2CEF;
    private const int PlayTimeSecondsOffset = 0x2CF0;

    // Checksum principal
    private const int MainDataStartOffset = 0x2598;
    private const int MainChecksumOffset = 0x3523;

    // Équipe Pokémon
    private const int PartyCountOffset = 0x2F2C;
    private const int PartyPokemonDataOffset = 0x2F34;
    private const int PartyNicknamesOffset = 0x307E;

    private const int PartyPokemonSize = 44;
    private const int PartyNameSize = 11;
    private const int MaximumPartySize = 6;

    private const int PartySpeciesListOffset = 0x2F2D;
    private const int PartyOtNamesOffset = 0x303C;

    public string FilePath { get; }

    public byte[] Data { get; }

    public string FileName =>
        Path.GetFileName(FilePath);

    public string PlayerName =>
        DecodeText(PlayerNameOffset, 11);

    public string RivalName =>
        DecodeText(RivalNameOffset, 11);

    public int Money =>
        DecodeBcdMoney();

    public byte Badges =>
        Data[BadgesOffset];

    public int PlayTimeHours =>
        Data[PlayTimeHoursOffset];

    public int PlayTimeMinutes =>
        Data[PlayTimeMinutesOffset];

    public int PlayTimeSeconds =>
        Data[PlayTimeSecondsOffset];

    public bool PlayTimeIsMaxed =>
        Data[PlayTimeMaxedOffset] != 0;

    public string FormattedPlayTime =>
        $"{PlayTimeHours:D2}:{PlayTimeMinutes:D2}:{PlayTimeSeconds:D2}";

    public byte StoredChecksum =>
        Data[MainChecksumOffset];

    public byte CalculatedChecksum =>
        CalculateChecksum();

    public bool IsChecksumValid =>
        StoredChecksum == CalculatedChecksum;

    public Gen1SaveFile(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        byte[] data = File.ReadAllBytes(filePath);

        if (data.Length != ExpectedSize)
        {
            throw new InvalidDataException(
                $"Taille incorrecte : {data.Length:N0} octets.\n" +
                $"La taille attendue est {ExpectedSize:N0} octets.");
        }

        FilePath = filePath;
        Data = data;

        if (!IsChecksumValid)
        {
            throw new InvalidDataException(
                "Le checksum de la sauvegarde est incorrect.\n" +
                $"Valeur enregistrée : 0x{StoredChecksum:X2}\n" +
                $"Valeur calculée : 0x{CalculatedChecksum:X2}");
        }
    }

    // =========================================================
    // Informations du dresseur
    // =========================================================

    public bool HasBadge(int badgeIndex)
    {
        if (badgeIndex is < 0 or > 7)
        {
            throw new ArgumentOutOfRangeException(
                nameof(badgeIndex));
        }

        int mask = 1 << badgeIndex;

        return (Badges & mask) != 0;
    }

    public void SetPlayerName(string playerName)
    {
        EncodeText(
            PlayerNameOffset,
            11,
            playerName);
    }

    public void SetRivalName(string rivalName)
    {
        EncodeText(
            RivalNameOffset,
            11,
            rivalName);
    }

    public void SetMoney(int money)
    {
        if (money is < 0 or > 999_999)
        {
            throw new ArgumentOutOfRangeException(
                nameof(money),
                "L'argent doit être compris entre 0 et 999 999.");
        }

        Data[MoneyOffset] =
            EncodeBcdByte(money / 10_000);

        Data[MoneyOffset + 1] =
            EncodeBcdByte((money / 100) % 100);

        Data[MoneyOffset + 2] =
            EncodeBcdByte(money % 100);
    }

    public void SetBadge(
        int badgeIndex,
        bool obtained)
    {
        if (badgeIndex is < 0 or > 7)
        {
            throw new ArgumentOutOfRangeException(
                nameof(badgeIndex));
        }

        byte mask = (byte)(1 << badgeIndex);

        if (obtained)
        {
            Data[BadgesOffset] |= mask;
        }
        else
        {
            Data[BadgesOffset] &= (byte)~mask;
        }
    }

    // =========================================================
    // Lecture de l'équipe Pokémon
    // =========================================================

    public IReadOnlyList<Gen1Pokemon> ReadParty()
{
    List<Gen1Pokemon> party = [];

    int pokemonCount = Data[PartyCountOffset];

    if (pokemonCount > MaximumPartySize)
    {
        throw new InvalidDataException(
            "Le nombre de Pokémon dans l'équipe est invalide.\n" +
            $"Valeur trouvée : {pokemonCount}");
    }

    for (int index = 0; index < pokemonCount; index++)
    {
        int pokemonOffset =
            PartyPokemonDataOffset +
            index * PartyPokemonSize;

        int nicknameOffset =
            PartyNicknamesOffset +
            index * PartyNameSize;

        List<Gen1MoveSlot> moves = [];

        for (int moveIndex = 0;
             moveIndex < 4;
             moveIndex++)
        {
            byte moveId =
                Data[pokemonOffset + 8 + moveIndex];

            byte ppData =
                Data[pokemonOffset + 29 + moveIndex];

            moves.Add(new Gen1MoveSlot
            {
                Slot = moveIndex + 1,
                MoveId = moveId,

                // Les 6 premiers bits contiennent les PP.
                CurrentPp = ppData & 0x3F,

                // Les 2 derniers bits contiennent les PP Plus.
                PpUps = (ppData >> 6) & 0x03
            });
        }

        Gen1Pokemon pokemon = new()
        {
            Slot = index + 1,

            SpeciesId =
                Data[pokemonOffset],

            Type1 =
                Data[pokemonOffset + 5],

            Type2 =
                Data[pokemonOffset + 6],

            CurrentHp =
                ReadBigEndianUInt16(pokemonOffset + 1),

            Status =
                DecodePokemonStatus(Data[pokemonOffset + 4]),

            OriginalTrainerId =
                ReadBigEndianUInt16(pokemonOffset + 12),

            Experience =
                ReadBigEndianUInt24(pokemonOffset + 14),

            HpEv =
                ReadBigEndianUInt16(pokemonOffset + 17),

            AttackEv =
                ReadBigEndianUInt16(pokemonOffset + 19),

            DefenseEv =
                ReadBigEndianUInt16(pokemonOffset + 21),

            SpeedEv =
                ReadBigEndianUInt16(pokemonOffset + 23),

            SpecialEv =
                ReadBigEndianUInt16(pokemonOffset + 25),

            AttackDv =
                (byte)(Data[pokemonOffset + 27] >> 4),

            DefenseDv =
                (byte)(Data[pokemonOffset + 27] & 0x0F),

            SpeedDv =
                (byte)(Data[pokemonOffset + 28] >> 4),

            SpecialDv =
                (byte)(Data[pokemonOffset + 28] & 0x0F),

            Level =
                Data[pokemonOffset + 33],

            MaximumHp =
                ReadBigEndianUInt16(pokemonOffset + 34),

            Attack =
                ReadBigEndianUInt16(pokemonOffset + 36),

            Defense =
                ReadBigEndianUInt16(pokemonOffset + 38),

            Speed =
                ReadBigEndianUInt16(pokemonOffset + 40),

            Special =
                ReadBigEndianUInt16(pokemonOffset + 42),

            Nickname =
                DecodeText(nicknameOffset, PartyNameSize),

            Moves = moves
        };

        party.Add(pokemon);
    }

    return party;
}

    private ushort ReadBigEndianUInt16(int offset)
    {
        return (ushort)(
            (Data[offset] << 8) |
            Data[offset + 1]);
    }
    private uint ReadBigEndianUInt24(int offset)
    {
        return ((uint)Data[offset] << 16)
             | ((uint)Data[offset + 1] << 8)
             | Data[offset + 2];
    }

    private static string DecodePokemonStatus(byte status)
    {
        if (status == 0)
            return "OK";

        // Les trois premiers bits représentent le sommeil.
        if ((status & 0x07) != 0)
            return "Sommeil";

        if ((status & 0x08) != 0)
            return "Poison";

        if ((status & 0x10) != 0)
            return "Brûlure";

        if ((status & 0x20) != 0)
            return "Gel";

        if ((status & 0x40) != 0)
            return "Paralysie";

        return $"Inconnu (0x{status:X2})";
    }

    // =========================================================
    // Argent BCD
    // =========================================================

    private int DecodeBcdMoney()
    {
        int firstPart =
            DecodeBcdByte(Data[MoneyOffset]);

        int secondPart =
            DecodeBcdByte(Data[MoneyOffset + 1]);

        int thirdPart =
            DecodeBcdByte(Data[MoneyOffset + 2]);

        return firstPart * 10_000
             + secondPart * 100
             + thirdPart;
    }

    private static int DecodeBcdByte(byte value)
    {
        int tens = (value >> 4) & 0x0F;
        int units = value & 0x0F;

        if (tens > 9 || units > 9)
        {
            throw new InvalidDataException(
                $"Valeur BCD invalide : 0x{value:X2}");
        }

        return tens * 10 + units;
    }

    private static byte EncodeBcdByte(int value)
    {
        if (value is < 0 or > 99)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value));
        }

        int tens = value / 10;
        int units = value % 10;

        return (byte)((tens << 4) | units);
    }

    // =========================================================
    // Texte Pokémon
    // =========================================================

    private string DecodeText(
        int offset,
        int maximumLength)
    {
        StringBuilder result = new();

        for (int index = 0;
             index < maximumLength;
             index++)
        {
            byte value = Data[offset + index];

            // Marqueur de fin du texte Pokémon.
            if (value == 0x50)
                break;

            result.Append(
                DecodeCharacter(value));
        }

        return result.ToString();
    }

    private static char DecodeCharacter(byte value)
    {
        if (value is >= 0x80 and <= 0x99)
        {
            return (char)(
                'A' + value - 0x80);
        }

        if (value is >= 0xA0 and <= 0xB9)
        {
            return (char)(
                'a' + value - 0xA0);
        }

        if (value is >= 0xF6 and <= 0xFF)
        {
            return (char)(
                '0' + value - 0xF6);
        }

        return value switch
        {
            0x7F => ' ',
            0xE0 => '\'',
            0xE3 => '-',
            0xE6 => '?',
            0xE7 => '!',
            0xE8 => '.',
            _ => '�'
        };
    }

    private void EncodeText(
        int offset,
        int maximumLength,
        string text)
    {
        text = text.Trim();

        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException(
                "Le nom ne peut pas être vide.");
        }

        // Une place est réservée au marqueur de fin 0x50.
        if (text.Length >= maximumLength)
        {
            throw new ArgumentException(
                $"Le nom ne peut pas dépasser " +
                $"{maximumLength - 1} caractères.");
        }

        Array.Fill(
            Data,
            (byte)0x50,
            offset,
            maximumLength);

        for (int index = 0;
             index < text.Length;
             index++)
        {
            Data[offset + index] =
                EncodeCharacter(text[index]);
        }
    }

    private static byte EncodeCharacter(char character)
    {
        if (character is >= 'A' and <= 'Z')
        {
            return (byte)(
                0x80 + character - 'A');
        }

        if (character is >= 'a' and <= 'z')
        {
            return (byte)(
                0xA0 + character - 'a');
        }

        if (character is >= '0' and <= '9')
        {
            return (byte)(
                0xF6 + character - '0');
        }

        return character switch
        {
            ' ' => 0x7F,
            '\'' => 0xE0,
            '-' => 0xE3,
            '?' => 0xE6,
            '!' => 0xE7,
            '.' => 0xE8,

            _ => throw new ArgumentException(
                $"Le caractère « {character} » " +
                "n'est pas pris en charge.")
        };
    }

    // =========================================================
    // Checksum
    // =========================================================

    public void UpdateChecksum()
    {
        Data[MainChecksumOffset] =
            CalculateChecksum();
    }

    private byte CalculateChecksum()
    {
        int sum = 0;

        for (int offset = MainDataStartOffset;
             offset < MainChecksumOffset;
             offset++)
        {
            sum = (sum + Data[offset]) & 0xFF;
        }

        return (byte)(sum ^ 0xFF);
    }
    public void SetPartyPokemonStats(
    int partyIndex,
    byte level,
    ushort currentHp,
    ushort maximumHp,
    ushort attack,
    ushort defense,
    ushort speed,
    ushort special,
    uint experience)
    {
        int pokemonCount = Data[PartyCountOffset];

        if (partyIndex < 0 || partyIndex >= pokemonCount)
        {
            throw new ArgumentOutOfRangeException(
                nameof(partyIndex),
                "L'emplacement du Pokémon est invalide.");
        }

        if (level is < 1 or > 100)
        {
            throw new ArgumentOutOfRangeException(
                nameof(level),
                "Le niveau doit être compris entre 1 et 100.");
        }

        if (maximumHp == 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(maximumHp),
                "Les PV maximums doivent être supérieurs à zéro.");
        }

        if (currentHp > maximumHp)
        {
            throw new ArgumentException(
                "Les PV actuels ne peuvent pas dépasser les PV maximums.");
        }

        if (attack == 0 ||
            defense == 0 ||
            speed == 0 ||
            special == 0)
        {
            throw new ArgumentException(
                "Les statistiques doivent être supérieures à zéro.");
        }

        if (experience > 0xFFFFFF)
        {
            throw new ArgumentOutOfRangeException(
                nameof(experience),
                "L'expérience dépasse la capacité de la sauvegarde.");
        }

        int pokemonOffset =
            PartyPokemonDataOffset +
            partyIndex * PartyPokemonSize;

        // Le niveau apparaît à deux endroits dans la structure
        // d'un Pokémon présent dans l'équipe.
        Data[pokemonOffset + 3] = level;
        Data[pokemonOffset + 33] = level;

        WriteBigEndianUInt16(
            pokemonOffset + 1,
            currentHp);

        WriteBigEndianUInt24(
            pokemonOffset + 14,
            experience);

        WriteBigEndianUInt16(
            pokemonOffset + 34,
            maximumHp);

        WriteBigEndianUInt16(
            pokemonOffset + 36,
            attack);

        WriteBigEndianUInt16(
            pokemonOffset + 38,
            defense);

        WriteBigEndianUInt16(
            pokemonOffset + 40,
            speed);

        WriteBigEndianUInt16(
            pokemonOffset + 42,
            special);

        // L'équipe fait partie des données couvertes par
        // le checksum principal.
        UpdateChecksum();
    }

    private void WriteBigEndianUInt16(
        int offset,
        ushort value)
    {
        Data[offset] =
            (byte)(value >> 8);

        Data[offset + 1] =
            (byte)(value & 0xFF);
    }

    private void WriteBigEndianUInt24(
        int offset,
        uint value)
    {
        if (value > 0xFFFFFF)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value));
        }

        Data[offset] =
            (byte)((value >> 16) & 0xFF);

        Data[offset + 1] =
            (byte)((value >> 8) & 0xFF);

        Data[offset + 2] =
            (byte)(value & 0xFF);
    }
    public void DeletePartyPokemon(int partyIndex)
    {
        int pokemonCount = Data[PartyCountOffset];

        if (partyIndex < 0 || partyIndex >= pokemonCount)
        {
            throw new ArgumentOutOfRangeException(
                nameof(partyIndex),
                "L'emplacement du Pokémon est invalide.");
        }

        // Décale la liste des identifiants d'espèces.
        for (int index = partyIndex;
             index < pokemonCount - 1;
             index++)
        {
            Data[PartySpeciesListOffset + index] =
                Data[PartySpeciesListOffset + index + 1];
        }

        // Décale les structures complètes des Pokémon.
        ShiftPartyDataLeft(
            PartyPokemonDataOffset,
            PartyPokemonSize,
            partyIndex,
            pokemonCount,
            0x00);

        // Décale les noms des Dresseurs d'Origine.
        ShiftPartyDataLeft(
            PartyOtNamesOffset,
            PartyNameSize,
            partyIndex,
            pokemonCount,
            0x50);

        // Décale les surnoms.
        ShiftPartyDataLeft(
            PartyNicknamesOffset,
            PartyNameSize,
            partyIndex,
            pokemonCount,
            0x50);

        int newPokemonCount = pokemonCount - 1;

        Data[PartyCountOffset] =
            (byte)newPokemonCount;

        // Marqueur de fin de la liste des espèces.
        Data[PartySpeciesListOffset + newPokemonCount] =
            0xFF;

        UpdateChecksum();
    }

    private void ShiftPartyDataLeft(
        int dataStartOffset,
        int elementSize,
        int removedIndex,
        int elementCount,
        byte emptyValue)
    {
        for (int index = removedIndex;
             index < elementCount - 1;
             index++)
        {
            int sourceOffset =
                dataStartOffset +
                (index + 1) * elementSize;

            int destinationOffset =
                dataStartOffset +
                index * elementSize;

            Array.Copy(
                Data,
                sourceOffset,
                Data,
                destinationOffset,
                elementSize);
        }

        int lastElementOffset =
            dataStartOffset +
            (elementCount - 1) * elementSize;

        Array.Fill(
            Data,
            emptyValue,
            lastElementOffset,
            elementSize);
    }

    public int PartyCount => Data[PartyCountOffset];

    public bool CanAddPartyPokemon => PartyCount < MaximumPartySize;

    public void AddPartyPokemon(
        byte speciesId,
        byte level,
        string? nickname = null)
    {
        AddPartyPokemonCoherent(speciesId, level, nickname);
    }
    private static string MakeEncodablePokemonName(string name)
    {
        string normalized = name
            .Normalize(NormalizationForm.FormD);

        StringBuilder result = new();

        foreach (char character in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(character) ==
                UnicodeCategory.NonSpacingMark)
            {
                continue;
            }

            char simplified = character switch
            {
                '♀' => 'F',
                '♂' => 'M',
                _ => character
            };

            if (simplified is >= 'A' and <= 'Z' ||
                simplified is >= 'a' and <= 'z' ||
                simplified is >= '0' and <= '9' ||
                simplified is ' ' or '-' or '\'' or '.' or '!' or '?')
            {
                result.Append(simplified);
            }

            if (result.Length == PartyNameSize - 1)
                break;
        }

        return result.Length == 0
            ? "POKEMON"
            : result.ToString();
    }

}
