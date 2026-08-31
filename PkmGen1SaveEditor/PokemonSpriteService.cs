using System.Collections.Concurrent;

namespace PkmGen1SaveEditor;

internal static class PokemonSpriteService
{
    private static readonly HttpClient Client = new()
    {
        Timeout = TimeSpan.FromSeconds(5)
    };

    private static readonly ConcurrentDictionary<byte, Image?> Cache = new();

    public static async Task<Image?> GetAsync(byte speciesId)
    {
        if (Cache.TryGetValue(speciesId, out Image? cached))
            return cached;

        int dexNumber = Gen1SpeciesCatalog.GetNationalDexNumber(speciesId);

        if (dexNumber == 0)
            return null;

        try
        {
            string url =
                "https://raw.githubusercontent.com/PokeAPI/sprites/master/" +
                "sprites/pokemon/versions/generation-i/red-blue/transparent/" +
                $"{dexNumber}.png";

            byte[] bytes = await Client.GetByteArrayAsync(url);
            using MemoryStream stream = new(bytes);
            using Image source = Image.FromStream(stream);
            Image result = new Bitmap(source);
            Cache.TryAdd(speciesId, result);
            return result;
        }
        catch
        {
            return null;
        }
    }
}
