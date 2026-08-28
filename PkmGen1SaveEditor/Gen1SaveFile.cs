using System;
using System.Collections.Generic;
using System.Text;

namespace PkmGen1SaveEditor;

internal sealed class Gen1SaveFile
{
    public const int ExpectedSize = 32 * 1024;

    public string FilePath { get; }
    public byte[] Data { get; }

    public string FileName => Path.GetFileName(FilePath);

    public Gen1SaveFile(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        byte[] data = File.ReadAllBytes(filePath);

        if (data.Length != ExpectedSize)
        {
            throw new InvalidDataException(
                $"Taille incorrecte : {data.Length:N0} octets. " +
                $"La taille attendue est {ExpectedSize:N0} octets.");
        }

        FilePath = filePath;
        Data = data;
    }
}
