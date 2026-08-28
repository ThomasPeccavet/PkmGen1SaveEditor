using System;
using System.IO;
using System.Text;

namespace PkmGen1SaveEditor;

internal sealed class Gen1SaveFile
{
    public const int ExpectedSize = 32 * 1024;

    private const int PlayerNameOffset = 0x2598;
    private const int RivalNameOffset = 0x25F6;
    private const int MoneyOffset = 0x25F3;
    private const int BadgesOffset = 0x2602;

    private const int PlayTimeHoursOffset = 0x2CED;
    private const int PlayTimeMaxedOffset = 0x2CEE;
    private const int PlayTimeMinutesOffset = 0x2CEF;
    private const int PlayTimeSecondsOffset = 0x2CF0;

    private const int MainDataStartOffset = 0x2598;
    private const int MainChecksumOffset = 0x3523;

    public string FilePath { get; }
    public byte[] Data { get; }

    public string FileName => Path.GetFileName(FilePath);

    public string PlayerName => DecodeText(PlayerNameOffset, 11);
    public string RivalName => DecodeText(RivalNameOffset, 11);

    public int Money => DecodeBcdMoney();

    public byte Badges => Data[BadgesOffset];

    public int PlayTimeHours => Data[PlayTimeHoursOffset];
    public int PlayTimeMinutes => Data[PlayTimeMinutesOffset];
    public int PlayTimeSeconds => Data[PlayTimeSecondsOffset];

    public bool PlayTimeIsMaxed => Data[PlayTimeMaxedOffset] != 0;

    public string FormattedPlayTime =>
        $"{PlayTimeHours:D2}:{PlayTimeMinutes:D2}:{PlayTimeSeconds:D2}";

    public byte StoredChecksum => Data[MainChecksumOffset];

    public byte CalculatedChecksum => CalculateChecksum();

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

    public bool HasBadge(int badgeIndex)
    {
        if (badgeIndex is < 0 or > 7)
            throw new ArgumentOutOfRangeException(nameof(badgeIndex));

        int mask = 1 << badgeIndex;

        return (Badges & mask) != 0;
    }

    private int DecodeBcdMoney()
    {
        int firstPart = DecodeBcdByte(Data[MoneyOffset]);
        int secondPart = DecodeBcdByte(Data[MoneyOffset + 1]);
        int thirdPart = DecodeBcdByte(Data[MoneyOffset + 2]);

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

    private string DecodeText(int offset, int maximumLength)
    {
        StringBuilder result = new();

        for (int index = 0; index < maximumLength; index++)
        {
            byte value = Data[offset + index];

            // 0x50 marque la fin du texte Pokémon.
            if (value == 0x50)
                break;

            result.Append(DecodeCharacter(value));
        }

        return result.ToString();
    }

    private static char DecodeCharacter(byte value)
    {
        if (value is >= 0x80 and <= 0x99)
            return (char)('A' + value - 0x80);

        if (value is >= 0xA0 and <= 0xB9)
            return (char)('a' + value - 0xA0);

        if (value is >= 0xF6 and <= 0xFF)
            return (char)('0' + value - 0xF6);

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
    public void SetPlayerName(string playerName)
    {
        EncodeText(PlayerNameOffset, 11, playerName);
    }

    public void SetRivalName(string rivalName)
    {
        EncodeText(RivalNameOffset, 11, rivalName);
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

    public void SetBadge(int badgeIndex, bool obtained)
    {
        if (badgeIndex is < 0 or > 7)
            throw new ArgumentOutOfRangeException(nameof(badgeIndex));

        byte mask = (byte)(1 << badgeIndex);

        if (obtained)
            Data[BadgesOffset] |= mask;
        else
            Data[BadgesOffset] &= (byte)~mask;
    }

    public void UpdateChecksum()
    {
        Data[MainChecksumOffset] = CalculateChecksum();
    }

    private void EncodeText(
        int offset,
        int maximumLength,
        string text)
    {
        text = text.Trim();

        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Le nom ne peut pas être vide.");

        // Il faut garder une place pour le marqueur de fin 0x50.
        if (text.Length >= maximumLength)
        {
            throw new ArgumentException(
                $"Le nom ne peut pas dépasser {maximumLength - 1} caractères.");
        }

        Array.Fill(
            Data,
            (byte)0x50,
            offset,
            maximumLength);

        for (int index = 0; index < text.Length; index++)
        {
            Data[offset + index] =
                EncodeCharacter(text[index]);
        }
    }

    private static byte EncodeCharacter(char character)
    {
        if (character is >= 'A' and <= 'Z')
            return (byte)(0x80 + character - 'A');

        if (character is >= 'a' and <= 'z')
            return (byte)(0xA0 + character - 'a');

        if (character is >= '0' and <= '9')
            return (byte)(0xF6 + character - '0');

        return character switch
        {
            ' ' => 0x7F,
            '\'' => 0xE0,
            '-' => 0xE3,
            '?' => 0xE6,
            '!' => 0xE7,
            '.' => 0xE8,

            _ => throw new ArgumentException(
                $"Le caractère « {character} » n'est pas pris en charge.")
        };
    }

    private static byte EncodeBcdByte(int value)
    {
        if (value is < 0 or > 99)
            throw new ArgumentOutOfRangeException(nameof(value));

        int tens = value / 10;
        int units = value % 10;

        return (byte)((tens << 4) | units);
    }
}