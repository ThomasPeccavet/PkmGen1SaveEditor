namespace PkmGen1SaveEditor;

internal sealed class InventoryItemForm : Form
{
    private readonly ComboBox _itemInput = new();
    private readonly NumericUpDown _quantityInput = new();

    public Gen1InventoryEntry? SelectedEntry { get; private set; }

    public InventoryItemForm(
        string title,
        IEnumerable<Gen1ItemDefinition> definitions,
        Gen1InventoryEntry? existingEntry = null)
    {
        IReadOnlyList<Gen1ItemDefinition> availableItems = definitions
            .Where(item => item.IsInventorySafe)
            .DistinctBy(item => item.Id)
            .ToArray();

        if (availableItems.Count == 0)
            throw new ArgumentException("Aucun objet n’est disponible.", nameof(definitions));

        Text = title;
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(600, 330);
        MinimumSize = new Size(520, 310);
        ShowInTaskbar = false;
        ModernTheme.Apply(this);

        BuildInterface(availableItems, existingEntry);
        ModernTheme.StyleTree(this);
    }

    private void BuildInterface(
        IReadOnlyList<Gen1ItemDefinition> availableItems,
        Gen1InventoryEntry? existingEntry)
    {
        TableLayoutPanel root = new()
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(22),
            ColumnCount = 1,
            RowCount = 3
        };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 68F));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));

        root.Controls.Add(CreateHeader(), 0, 0);
        root.Controls.Add(ModernTheme.CreateCard(
            "Objet et quantité",
            "Les objets clés et les CS sont toujours uniques.",
            CreateFields(availableItems, existingEntry)), 0, 1);
        root.Controls.Add(CreateButtons(), 0, 2);
        Controls.Add(root);
    }

    private Control CreateHeader()
    {
        return new Label
        {
            Text = Text,
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold),
            ForeColor = ModernTheme.TextColor,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(8, 0, 0, 0)
        };
    }

    private Control CreateFields(
        IReadOnlyList<Gen1ItemDefinition> availableItems,
        Gen1InventoryEntry? existingEntry)
    {
        TableLayoutPanel table = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 2,
            Padding = new Padding(8)
        };
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 105F));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        table.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        table.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

        _itemInput.Dock = DockStyle.Fill;
        _itemInput.DropDownStyle = ComboBoxStyle.DropDownList;
        _itemInput.MaxDropDownItems = 16;
        _itemInput.DisplayMember = nameof(Gen1ItemDefinition.DisplayName);
        _itemInput.DataSource = availableItems.ToArray();
        _itemInput.SelectedIndexChanged += (_, _) => UpdateQuantityRules();

        _quantityInput.Dock = DockStyle.Left;
        _quantityInput.Width = 150;
        _quantityInput.Minimum = 1;
        _quantityInput.Maximum = 99;
        _quantityInput.Value = existingEntry?.Quantity ?? 1;

        AddField(table, 0, "Objet", _itemInput);
        AddField(table, 1, "Quantité", _quantityInput);

        if (existingEntry is not null)
        {
            int index = availableItems
                .Select((item, itemIndex) => (item, itemIndex))
                .FirstOrDefault(value => value.item.Id == existingEntry.ItemId)
                .itemIndex;

            if (availableItems[index].Id == existingEntry.ItemId)
                _itemInput.SelectedIndex = index;
        }

        UpdateQuantityRules();
        return table;
    }

    private static void AddField(
        TableLayoutPanel table,
        int row,
        string title,
        Control control)
    {
        table.Controls.Add(new Label
        {
            Text = title,
            Dock = DockStyle.Fill,
            ForeColor = ModernTheme.MutedTextColor,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = new Padding(6)
        }, 0, row);

        control.Margin = new Padding(6, 10, 6, 10);
        table.Controls.Add(control, 1, row);
    }

    private Control CreateButtons()
    {
        FlowLayoutPanel panel = new()
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false,
            Padding = new Padding(8, 7, 8, 0)
        };

        Button applyButton = new()
        {
            Text = "Valider",
            AutoSize = true,
            Tag = "primary"
        };
        applyButton.Click += ApplyButton_Click;

        Button cancelButton = new()
        {
            Text = "Annuler",
            AutoSize = true,
            DialogResult = DialogResult.Cancel
        };

        panel.Controls.Add(applyButton);
        panel.Controls.Add(cancelButton);
        AcceptButton = applyButton;
        CancelButton = cancelButton;
        return panel;
    }

    private void UpdateQuantityRules()
    {
        if (_itemInput.SelectedItem is not Gen1ItemDefinition item)
            return;

        _quantityInput.Maximum = item.IsKeyItem ? 1 : 99;
        if (item.IsKeyItem)
            _quantityInput.Value = 1;
    }

    private void ApplyButton_Click(object? sender, EventArgs e)
    {
        if (_itemInput.SelectedItem is not Gen1ItemDefinition item)
        {
            MessageBox.Show(
                "Sélectionnez un objet.",
                "Objet manquant",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        SelectedEntry = new Gen1InventoryEntry
        {
            ItemId = item.Id,
            Quantity = decimal.ToByte(_quantityInput.Value)
        };

        DialogResult = DialogResult.OK;
        Close();
    }
}
