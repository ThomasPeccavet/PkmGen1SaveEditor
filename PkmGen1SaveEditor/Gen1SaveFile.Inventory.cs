namespace PkmGen1SaveEditor;

internal sealed partial class Gen1SaveFile
{
    private const int BagCountOffset = 0x25C9;
    private const int BagItemsOffset = 0x25CA;
    private const int BagCapacity = 20;

    private const int PcItemCountOffset = 0x27E6;
    private const int PcItemsOffset = 0x27E7;
    private const int PcItemCapacity = 50;

    private const int CoinsOffset = 0x2850;

    public int Coins =>
        DecodeBcdByte(Data[CoinsOffset]) * 100 +
        DecodeBcdByte(Data[CoinsOffset + 1]);

    public IReadOnlyList<Gen1InventoryEntry> ReadBagItems() =>
        ReadInventory(BagCountOffset, BagItemsOffset, BagCapacity, "sac");

    public IReadOnlyList<Gen1InventoryEntry> ReadPcItems() =>
        ReadInventory(PcItemCountOffset, PcItemsOffset, PcItemCapacity, "PC");

    public IReadOnlyList<Gen1InventoryIssue> ValidateInventories(
        IReadOnlyList<Gen1InventoryEntry> bagItems,
        IReadOnlyList<Gen1InventoryEntry> pcItems)
    {
        ArgumentNullException.ThrowIfNull(bagItems);
        ArgumentNullException.ThrowIfNull(pcItems);

        List<Gen1InventoryIssue> issues = [];

        ValidateContainer(bagItems, BagCapacity, "Sac", issues);
        ValidateContainer(pcItems, PcItemCapacity, "PC", issues);

        HashSet<byte> bagKeyItems = bagItems
            .Where(entry => Gen1ItemCatalog.Find(entry.ItemId)?.IsKeyItem == true)
            .Select(entry => entry.ItemId)
            .ToHashSet();

        foreach (byte itemId in pcItems
                     .Where(entry => Gen1ItemCatalog.Find(entry.ItemId)?.IsKeyItem == true)
                     .Select(entry => entry.ItemId)
                     .Distinct())
        {
            if (!bagKeyItems.Contains(itemId))
                continue;

            issues.Add(new Gen1InventoryIssue(
                Gen1InventoryIssueSeverity.Error,
                "Sac et PC",
                $"{Gen1ItemCatalog.GetName(itemId)} est un objet unique présent dans les deux inventaires."));
        }

        return issues;
    }

    public void SetInventory(
        IReadOnlyList<Gen1InventoryEntry> bagItems,
        IReadOnlyList<Gen1InventoryEntry> pcItems,
        int money,
        int coins)
    {
        IReadOnlyList<Gen1InventoryIssue> issues =
            ValidateInventories(bagItems, pcItems);

        Gen1InventoryIssue? firstError = issues.FirstOrDefault(
            issue => issue.Severity == Gen1InventoryIssueSeverity.Error);

        if (firstError is not null)
        {
            throw new InvalidDataException(
                "L’inventaire contient une valeur qui ne peut pas être enregistrée.\n\n" +
                firstError);
        }

        if (money is < 0 or > 999_999)
        {
            throw new ArgumentOutOfRangeException(
                nameof(money),
                "L’argent doit être compris entre 0 et 999 999.");
        }

        if (coins is < 0 or > 9_999)
        {
            throw new ArgumentOutOfRangeException(
                nameof(coins),
                "Les Jetons du Casino doivent être compris entre 0 et 9 999.");
        }

        WriteInventory(BagCountOffset, BagItemsOffset, BagCapacity, bagItems);
        WriteInventory(PcItemCountOffset, PcItemsOffset, PcItemCapacity, pcItems);
        SetMoney(money);
        SetCoins(coins);
        UpdateChecksum();
    }

    public void SetCoins(int coins)
    {
        if (coins is < 0 or > 9_999)
        {
            throw new ArgumentOutOfRangeException(
                nameof(coins),
                "Les Jetons du Casino doivent être compris entre 0 et 9 999.");
        }

        Data[CoinsOffset] = EncodeBcdByte(coins / 100);
        Data[CoinsOffset + 1] = EncodeBcdByte(coins % 100);
    }

    private IReadOnlyList<Gen1InventoryEntry> ReadInventory(
        int countOffset,
        int itemsOffset,
        int capacity,
        string inventoryName)
    {
        int count = Data[countOffset];

        if (count > capacity)
        {
            throw new InvalidDataException(
                $"Le nombre d’objets du {inventoryName} est invalide : " +
                $"{count} pour une capacité de {capacity}.");
        }

        List<Gen1InventoryEntry> entries = [];

        for (int index = 0; index < count; index++)
        {
            int offset = itemsOffset + index * 2;
            entries.Add(new Gen1InventoryEntry
            {
                ItemId = Data[offset],
                Quantity = Data[offset + 1]
            });
        }

        int terminatorOffset = itemsOffset + count * 2;
        if (Data[terminatorOffset] != 0xFF)
        {
            throw new InvalidDataException(
                $"La liste d’objets du {inventoryName} ne possède pas " +
                "de marqueur de fin valide.");
        }

        return entries;
    }

    private static void ValidateContainer(
        IReadOnlyList<Gen1InventoryEntry> entries,
        int capacity,
        string location,
        ICollection<Gen1InventoryIssue> issues)
    {
        if (entries.Count > capacity)
        {
            issues.Add(new Gen1InventoryIssue(
                Gen1InventoryIssueSeverity.Error,
                location,
                $"{entries.Count} lignes pour une capacité maximale de {capacity}."));
        }

        foreach (Gen1InventoryEntry entry in entries)
        {
            Gen1ItemDefinition? definition =
                Gen1ItemCatalog.Find(entry.ItemId);

            if (definition is null)
            {
                issues.Add(new Gen1InventoryIssue(
                    Gen1InventoryIssueSeverity.Error,
                    location,
                    $"l’identifiant 0x{entry.ItemId:X2} ne correspond à aucun objet utilisable."));
                continue;
            }

            if (!definition.IsInventorySafe)
            {
                issues.Add(new Gen1InventoryIssue(
                    Gen1InventoryIssueSeverity.Error,
                    location,
                    $"{definition.Name} est impossible ici ({definition.SafetyMessage})."));
            }

            if (entry.Quantity is < 1 or > 99)
            {
                issues.Add(new Gen1InventoryIssue(
                    Gen1InventoryIssueSeverity.Error,
                    location,
                    $"{definition.Name} possède une quantité {entry.Quantity}; la plage valide est 1–99."));
            }

            if (definition.IsKeyItem && entry.Quantity != 1)
            {
                issues.Add(new Gen1InventoryIssue(
                    Gen1InventoryIssueSeverity.Error,
                    location,
                    $"{definition.Name} est unique et doit avoir la quantité 1."));
            }
        }

        foreach (IGrouping<byte, Gen1InventoryEntry> duplicate in
                 entries.GroupBy(entry => entry.ItemId).Where(group => group.Count() > 1))
        {
            Gen1ItemDefinition? definition = Gen1ItemCatalog.Find(duplicate.Key);
            Gen1InventoryIssueSeverity severity = definition?.IsKeyItem == true
                ? Gen1InventoryIssueSeverity.Error
                : Gen1InventoryIssueSeverity.Warning;

            issues.Add(new Gen1InventoryIssue(
                severity,
                location,
                $"{Gen1ItemCatalog.GetName(duplicate.Key)} apparaît {duplicate.Count()} fois."));
        }
    }

    private void WriteInventory(
        int countOffset,
        int itemsOffset,
        int capacity,
        IReadOnlyList<Gen1InventoryEntry> entries)
    {
        Data[countOffset] = (byte)entries.Count;
        Array.Fill(Data, (byte)0x00, itemsOffset, capacity * 2 + 1);

        for (int index = 0; index < entries.Count; index++)
        {
            int offset = itemsOffset + index * 2;
            Data[offset] = entries[index].ItemId;
            Data[offset + 1] = entries[index].Quantity;
        }

        Data[itemsOffset + entries.Count * 2] = 0xFF;
    }
}
