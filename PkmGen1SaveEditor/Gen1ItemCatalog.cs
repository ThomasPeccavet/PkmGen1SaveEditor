namespace PkmGen1SaveEditor;

internal static class Gen1ItemCatalog
{
    private static readonly IReadOnlyList<Gen1ItemDefinition> Items =
        BuildItems();

    private static readonly IReadOnlyDictionary<byte, Gen1ItemDefinition>
        ItemsById = Items.ToDictionary(item => item.Id);

    public static IReadOnlyList<Gen1ItemDefinition> GetAll() => Items;

    public static IReadOnlyList<Gen1ItemDefinition> GetSafeItems() =>
        Items
            .Where(item => item.IsInventorySafe)
            .OrderBy(item => GetCategoryOrder(item.Category))
            .ThenBy(item => item.Name, StringComparer.CurrentCulture)
            .ToArray();

    public static Gen1ItemDefinition? Find(byte itemId) =>
        ItemsById.GetValueOrDefault(itemId);

    public static string GetName(byte itemId) =>
        Find(itemId)?.Name ?? $"Objet inconnu (0x{itemId:X2})";

    public static string GetCategoryName(Gen1ItemCategory category) =>
        category switch
        {
            Gen1ItemCategory.PokeBall => "Poké Balls",
            Gen1ItemCategory.Healing => "Soins",
            Gen1ItemCategory.Evolution => "Évolution",
            Gen1ItemCategory.Training => "Entraînement",
            Gen1ItemCategory.Battle => "Combat",
            Gen1ItemCategory.Utility => "Divers",
            Gen1ItemCategory.KeyItem => "Objet clé",
            Gen1ItemCategory.TechnicalMachine => "CT",
            Gen1ItemCategory.HiddenMachine => "CS",
            _ => "Autre"
        };

    private static IReadOnlyList<Gen1ItemDefinition> BuildItems()
    {
        List<Gen1ItemDefinition> items =
        [
            Item(0x01, "Master Ball", Gen1ItemCategory.PokeBall),
            Item(0x02, "Hyper Ball", Gen1ItemCategory.PokeBall),
            Item(0x03, "Super Ball", Gen1ItemCategory.PokeBall),
            Item(0x04, "Poké Ball", Gen1ItemCategory.PokeBall),

            Key(0x05, "Carte"),
            Key(0x06, "Bicyclette"),
            Unsafe(0x07, "Planche Surf", "objet inutilisé par le jeu"),
            Unsafe(0x08, "Safari Ball", "objet temporaire réservé au Parc Safari"),
            Unsafe(0x09, "Pokédex", "le Pokédex est géré par un indicateur distinct"),

            Item(0x0A, "Pierre Lune", Gen1ItemCategory.Evolution),
            Item(0x0B, "Antidote", Gen1ItemCategory.Healing),
            Item(0x0C, "Anti-Brûle", Gen1ItemCategory.Healing),
            Item(0x0D, "Antigel", Gen1ItemCategory.Healing),
            Item(0x0E, "Réveil", Gen1ItemCategory.Healing),
            Item(0x0F, "Anti-Para", Gen1ItemCategory.Healing),
            Item(0x10, "Guérison", Gen1ItemCategory.Healing),
            Item(0x11, "Potion Max", Gen1ItemCategory.Healing),
            Item(0x12, "Hyper Potion", Gen1ItemCategory.Healing),
            Item(0x13, "Super Potion", Gen1ItemCategory.Healing),
            Item(0x14, "Potion", Gen1ItemCategory.Healing),

            Unsafe(0x15, "Badge Roche", "les badges ne doivent pas être placés dans l’inventaire"),
            Unsafe(0x16, "Badge Cascade", "les badges ne doivent pas être placés dans l’inventaire"),
            Unsafe(0x17, "Badge Foudre", "les badges ne doivent pas être placés dans l’inventaire"),
            Unsafe(0x18, "Badge Prisme", "les badges ne doivent pas être placés dans l’inventaire"),
            Unsafe(0x19, "Badge Âme", "les badges ne doivent pas être placés dans l’inventaire"),
            Unsafe(0x1A, "Badge Marais", "les badges ne doivent pas être placés dans l’inventaire"),
            Unsafe(0x1B, "Badge Volcan", "les badges ne doivent pas être placés dans l’inventaire"),
            Unsafe(0x1C, "Badge Terre", "les badges ne doivent pas être placés dans l’inventaire"),

            Item(0x1D, "Corde Sortie", Gen1ItemCategory.Utility),
            Item(0x1E, "Repousse", Gen1ItemCategory.Utility),
            Key(0x1F, "Vieil Ambre"),
            Item(0x20, "Pierre Feu", Gen1ItemCategory.Evolution),
            Item(0x21, "Pierre Foudre", Gen1ItemCategory.Evolution),
            Item(0x22, "Pierre Eau", Gen1ItemCategory.Evolution),
            Item(0x23, "PV Plus", Gen1ItemCategory.Training),
            Item(0x24, "Protéine", Gen1ItemCategory.Training),
            Item(0x25, "Fer", Gen1ItemCategory.Training),
            Item(0x26, "Carbone", Gen1ItemCategory.Training),
            Item(0x27, "Calcium", Gen1ItemCategory.Training),
            Item(0x28, "Super Bonbon", Gen1ItemCategory.Training),
            Key(0x29, "Fossile Dôme"),
            Key(0x2A, "Nautile"),
            Key(0x2B, "Clé Secrète"),
            Unsafe(0x2C, "Objet inutilisé 2C", "identifiant inutilisé"),
            Key(0x2D, "Bon Commande"),
            Item(0x2E, "Précision +", Gen1ItemCategory.Battle),
            Item(0x2F, "Pierre Plante", Gen1ItemCategory.Evolution),
            Key(0x30, "Carte Magn."),
            Item(0x31, "Pépite", Gen1ItemCategory.Utility),
            Unsafe(0x32, "Objet inutilisé 32", "identifiant inutilisé malgré son libellé interne"),
            Item(0x33, "Poké Poupée", Gen1ItemCategory.Utility),
            Item(0x34, "Total Soin", Gen1ItemCategory.Healing),
            Item(0x35, "Rappel", Gen1ItemCategory.Healing),
            Item(0x36, "Rappel Max", Gen1ItemCategory.Healing),
            Item(0x37, "Défense Spéc.", Gen1ItemCategory.Battle),
            Item(0x38, "Super Repousse", Gen1ItemCategory.Utility),
            Item(0x39, "Repousse Max", Gen1ItemCategory.Utility),
            Item(0x3A, "Muscle +", Gen1ItemCategory.Battle),
            Unsafe(0x3B, "Jeton", "les Jetons du Casino sont stockés dans un compteur BCD distinct"),
            Item(0x3C, "Eau Fraîche", Gen1ItemCategory.Healing),
            Item(0x3D, "Soda Cool", Gen1ItemCategory.Healing),
            Item(0x3E, "Limonade", Gen1ItemCategory.Healing),
            Key(0x3F, "Passe Bateau"),
            Key(0x40, "Dent d’Or"),
            Item(0x41, "Attaque +", Gen1ItemCategory.Battle),
            Item(0x42, "Défense +", Gen1ItemCategory.Battle),
            Item(0x43, "Vitesse +", Gen1ItemCategory.Battle),
            Item(0x44, "Spécial +", Gen1ItemCategory.Battle),
            Key(0x45, "Boîte Jetons"),
            Key(0x46, "Colis de Chen"),
            Key(0x47, "Cherch’Objet"),
            Key(0x48, "Scope Sylphe"),
            Key(0x49, "Poké Flûte"),
            Key(0x4A, "Clé Ascenseur"),
            Item(0x4B, "Multi Exp.", Gen1ItemCategory.Training),
            Key(0x4C, "Canne"),
            Key(0x4D, "Super Canne"),
            Key(0x4E, "Méga Canne"),
            Item(0x4F, "PP Plus", Gen1ItemCategory.Training),
            Item(0x50, "Huile", Gen1ItemCategory.Healing),
            Item(0x51, "Huile Max", Gen1ItemCategory.Healing),
            Item(0x52, "Élixir", Gen1ItemCategory.Healing),
            Item(0x53, "Élixir Max", Gen1ItemCategory.Healing)
        ];

        byte[] hmMoves = [0x0F, 0x13, 0x39, 0x46, 0x94];
        for (int index = 0; index < hmMoves.Length; index++)
        {
            items.Add(new Gen1ItemDefinition(
                (byte)(0xC4 + index),
                $"CS{index + 1:D2} — {Gen1MoveCatalog.GetName(hmMoves[index])}",
                Gen1ItemCategory.HiddenMachine,
                IsKeyItem: true));
        }

        byte[] tmMoves =
        [
            0x05, 0x0D, 0x0E, 0x12, 0x19, 0x5C, 0x20, 0x22, 0x24, 0x26,
            0x3D, 0x37, 0x3A, 0x3B, 0x3F, 0x06, 0x42, 0x44, 0x45, 0x63,
            0x48, 0x4C, 0x52, 0x55, 0x57, 0x59, 0x5A, 0x5B, 0x5E, 0x64,
            0x77, 0x68, 0x73, 0x75, 0x76, 0x78, 0x79, 0x7E, 0x81, 0x82,
            0x87, 0x8A, 0x8F, 0x9C, 0x56, 0x95, 0x99, 0x9D, 0xA1, 0xA4
        ];

        for (int index = 0; index < tmMoves.Length; index++)
        {
            items.Add(new Gen1ItemDefinition(
                (byte)(0xC9 + index),
                $"CT{index + 1:D2} — {Gen1MoveCatalog.GetName(tmMoves[index])}",
                Gen1ItemCategory.TechnicalMachine));
        }

        return items.ToArray();
    }

    private static Gen1ItemDefinition Item(
        byte id,
        string name,
        Gen1ItemCategory category) =>
        new(id, name, category);

    private static Gen1ItemDefinition Key(byte id, string name) =>
        new(id, name, Gen1ItemCategory.KeyItem, IsKeyItem: true);

    private static Gen1ItemDefinition Unsafe(
        byte id,
        string name,
        string reason) =>
        new(
            id,
            name,
            Gen1ItemCategory.KeyItem,
            IsKeyItem: true,
            IsInventorySafe: false,
            SafetyMessage: reason);

    private static int GetCategoryOrder(Gen1ItemCategory category) =>
        category switch
        {
            Gen1ItemCategory.PokeBall => 0,
            Gen1ItemCategory.Healing => 1,
            Gen1ItemCategory.Evolution => 2,
            Gen1ItemCategory.Training => 3,
            Gen1ItemCategory.Battle => 4,
            Gen1ItemCategory.Utility => 5,
            Gen1ItemCategory.KeyItem => 6,
            Gen1ItemCategory.HiddenMachine => 7,
            Gen1ItemCategory.TechnicalMachine => 8,
            _ => 9
        };
}
