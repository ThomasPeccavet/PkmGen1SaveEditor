namespace PkmGen1SaveEditor;

internal enum Gen1ItemCategory
{
    PokeBall,
    Healing,
    Evolution,
    Training,
    Battle,
    Utility,
    KeyItem,
    TechnicalMachine,
    HiddenMachine
}

internal sealed record Gen1ItemDefinition(
    byte Id,
    string Name,
    Gen1ItemCategory Category,
    bool IsKeyItem = false,
    bool IsInventorySafe = true,
    string? SafetyMessage = null)
{
    public string DisplayName => $"{Name}  (0x{Id:X2})";
}

internal sealed class Gen1InventoryEntry
{
    public byte ItemId { get; set; }

    public byte Quantity { get; set; }

    public Gen1InventoryEntry Clone() =>
        new()
        {
            ItemId = ItemId,
            Quantity = Quantity
        };
}

internal enum Gen1InventoryIssueSeverity
{
    Warning,
    Error
}

internal sealed record Gen1InventoryIssue(
    Gen1InventoryIssueSeverity Severity,
    string Location,
    string Message)
{
    public override string ToString()
    {
        string marker = Severity == Gen1InventoryIssueSeverity.Error
            ? "Erreur"
            : "Attention";

        return $"{marker} — {Location} : {Message}";
    }
}
