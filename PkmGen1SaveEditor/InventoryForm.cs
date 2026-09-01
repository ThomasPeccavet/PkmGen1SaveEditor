namespace PkmGen1SaveEditor;

internal sealed class InventoryForm : Form
{
    private readonly Gen1SaveFile _saveFile;
    private readonly List<Gen1InventoryEntry> _bagItems;
    private readonly List<Gen1InventoryEntry> _pcItems;

    private readonly DataGridView _bagGrid = CreateInventoryGrid();
    private readonly DataGridView _pcGrid = CreateInventoryGrid();
    private readonly NumericUpDown _moneyInput = new();
    private readonly NumericUpDown _coinsInput = new();
    private readonly Label _bagCountLabel = new();
    private readonly Label _pcCountLabel = new();
    private readonly ListBox _issuesList = new();

    internal InventoryForm(Gen1SaveFile saveFile)
    {
        _saveFile = saveFile ?? throw new ArgumentNullException(nameof(saveFile));
        _bagItems = saveFile.ReadBagItems().Select(entry => entry.Clone()).ToList();
        _pcItems = saveFile.ReadPcItems().Select(entry => entry.Clone()).ToList();

        Text = "Inventaire complet";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(1220, 840);
        MinimumSize = new Size(1000, 760);
        ShowInTaskbar = false;
        ModernTheme.Apply(this);

        BuildInterface();
        RefreshAll();
        ModernTheme.StyleTree(this);
    }

    private void BuildInterface()
    {
        TableLayoutPanel root = new()
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(22),
            ColumnCount = 1,
            RowCount = 5
        };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 108F));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 96F));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));

        root.Controls.Add(CreateHeader(), 0, 0);
        root.Controls.Add(CreateCurrencyCard(), 0, 1);
        root.Controls.Add(CreateInventories(), 0, 2);
        root.Controls.Add(CreateIssuesCard(), 0, 3);
        root.Controls.Add(CreateDialogButtons(), 0, 4);
        Controls.Add(root);
    }

    private Control CreateHeader()
    {
        TableLayoutPanel layout = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            Padding = new Padding(8, 0, 8, 0)
        };
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
        layout.Controls.Add(new Label
        {
            Text = "Inventaire complet",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI Semibold", 21F, FontStyle.Bold),
            ForeColor = ModernTheme.TextColor,
            TextAlign = ContentAlignment.BottomLeft
        }, 0, 0);
        layout.Controls.Add(new Label
        {
            Text = "Modifiez le sac, le PC objets et les monnaies. Les contrôles d’intégrité sont actualisés en direct.",
            Dock = DockStyle.Fill,
            ForeColor = ModernTheme.MutedTextColor,
            TextAlign = ContentAlignment.TopLeft
        }, 0, 1);
        return layout;
    }

    private Control CreateCurrencyCard()
    {
        TableLayoutPanel fields = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 4,
            RowCount = 1,
            Padding = new Padding(8)
        };
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 115F));
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 155F));
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

        ConfigureNumberInput(_moneyInput, 999_999, _saveFile.Money);
        ConfigureNumberInput(_coinsInput, 9_999, _saveFile.Coins);

        fields.Controls.Add(CreateFieldLabel("Argent"), 0, 0);
        fields.Controls.Add(_moneyInput, 1, 0);
        fields.Controls.Add(CreateFieldLabel("Jetons Casino"), 2, 0);
        fields.Controls.Add(_coinsInput, 3, 0);

        return ModernTheme.CreateCard(
            "Monnaies",
            "Valeurs BCD compatibles avec les limites du jeu.",
            fields);
    }

    private Control CreateInventories()
    {
        TableLayoutPanel layout = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        layout.Controls.Add(CreateInventoryCard(
            "Sac",
            "20 lignes maximum",
            _bagGrid,
            _bagCountLabel,
            _bagItems,
            20), 0, 0);
        layout.Controls.Add(CreateInventoryCard(
            "PC objets",
            "50 lignes maximum",
            _pcGrid,
            _pcCountLabel,
            _pcItems,
            50), 1, 0);
        return layout;
    }

    private Control CreateInventoryCard(
        string title,
        string subtitle,
        DataGridView grid,
        Label countLabel,
        List<Gen1InventoryEntry> entries,
        int capacity)
    {
        TableLayoutPanel content = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3
        };
        content.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        content.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        content.RowStyles.Add(new RowStyle(SizeType.Absolute, 92F));

        countLabel.Dock = DockStyle.Fill;
        countLabel.ForeColor = ModernTheme.MutedTextColor;
        countLabel.TextAlign = ContentAlignment.MiddleRight;
        content.Controls.Add(countLabel, 0, 0);

        grid.Dock = DockStyle.Fill;
        grid.CellDoubleClick += (_, eventArgs) =>
        {
            if (eventArgs.RowIndex >= 0)
                EditSelected(grid, entries);
        };
        content.Controls.Add(grid, 0, 1);
        content.Controls.Add(CreateInventoryActions(grid, entries, capacity), 0, 2);
        return ModernTheme.CreateCard(title, subtitle, content);
    }

    private Control CreateInventoryActions(
        DataGridView grid,
        List<Gen1InventoryEntry> entries,
        int capacity)
    {
        TableLayoutPanel actions = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2
        };
        actions.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
        actions.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        FlowLayoutPanel standard = CreateActionRow();
        standard.Controls.Add(CreateButton("Ajouter", (_, _) =>
            AddItem(grid, entries, capacity, null)));
        standard.Controls.Add(CreateButton("Modifier", (_, _) =>
            EditSelected(grid, entries)));
        standard.Controls.Add(CreateButton("Supprimer", (_, _) =>
            DeleteSelected(grid, entries), "danger"));

        FlowLayoutPanel quick = CreateActionRow();
        quick.Controls.Add(new Label
        {
            Text = "Ajout rapide :",
            AutoSize = true,
            ForeColor = ModernTheme.MutedTextColor,
            Padding = new Padding(0, 9, 4, 0)
        });

        Button quickButton = CreateButton("Choisir une catégorie…", (_, _) => { });
        ContextMenuStrip quickMenu = new();
        quickMenu.Items.Add("CT / CS", null, (_, _) =>
            AddItem(grid, entries, capacity,
                [Gen1ItemCategory.TechnicalMachine, Gen1ItemCategory.HiddenMachine]));
        quickMenu.Items.Add("Poké Balls", null, (_, _) =>
            AddItem(grid, entries, capacity, [Gen1ItemCategory.PokeBall]));
        quickMenu.Items.Add("Soins", null, (_, _) =>
            AddItem(grid, entries, capacity, [Gen1ItemCategory.Healing]));
        quickMenu.Items.Add("Objets clés", null, (_, _) =>
            AddItem(grid, entries, capacity, [Gen1ItemCategory.KeyItem]));
        quickButton.ContextMenuStrip = quickMenu;
        quickButton.Click += (_, _) =>
            quickMenu.Show(quickButton, new Point(0, quickButton.Height));
        quickButton.Disposed += (_, _) => quickMenu.Dispose();
        quick.Controls.Add(quickButton);

        actions.Controls.Add(standard, 0, 0);
        actions.Controls.Add(quick, 0, 1);
        return actions;
    }

    private Control CreateIssuesCard()
    {
        _issuesList.Dock = DockStyle.Fill;
        _issuesList.BorderStyle = BorderStyle.None;
        _issuesList.IntegralHeight = false;
        _issuesList.BackColor = ModernTheme.SurfaceColor;
        _issuesList.ForeColor = ModernTheme.TextColor;

        return ModernTheme.CreateCard(
            "Contrôle d’intégrité",
            "Les erreurs doivent être corrigées; les doublons ordinaires sont signalés sans bloquer l’enregistrement.",
            _issuesList);
    }

    private Control CreateDialogButtons()
    {
        FlowLayoutPanel buttons = new()
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false,
            Padding = new Padding(8, 8, 8, 0)
        };

        Button applyButton = CreateButton("Appliquer", ApplyButton_Click, "primary");
        Button cancelButton = CreateButton("Annuler", (_, _) => Close());
        buttons.Controls.Add(applyButton);
        buttons.Controls.Add(cancelButton);
        AcceptButton = applyButton;
        CancelButton = cancelButton;
        return buttons;
    }

    private void AddItem(
        DataGridView grid,
        List<Gen1InventoryEntry> entries,
        int capacity,
        IReadOnlyCollection<Gen1ItemCategory>? categories)
    {
        if (entries.Count >= capacity)
        {
            MessageBox.Show(
                $"Cet inventaire contient déjà ses {capacity} lignes.",
                "Inventaire plein",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        IEnumerable<Gen1ItemDefinition> definitions =
            Gen1ItemCatalog.GetSafeItems();

        if (categories is not null)
        {
            definitions = definitions.Where(item =>
                categories.Contains(item.Category));
        }

        using InventoryItemForm dialog = new(
            categories is null ? "Ajouter un objet" : "Ajout rapide",
            definitions);

        if (dialog.ShowDialog(this) != DialogResult.OK ||
            dialog.SelectedEntry is null)
        {
            return;
        }

        Gen1InventoryEntry? existing = entries.FirstOrDefault(
            entry => entry.ItemId == dialog.SelectedEntry.ItemId);

        if (existing is not null)
        {
            existing.Quantity = dialog.SelectedEntry.Quantity;
        }
        else
        {
            entries.Add(dialog.SelectedEntry);
        }

        RefreshAll();
        SelectEntry(grid, dialog.SelectedEntry.ItemId);
    }

    private void EditSelected(
        DataGridView grid,
        List<Gen1InventoryEntry> entries)
    {
        if (GetSelectedEntry(grid) is not Gen1InventoryEntry selected)
            return;

        Gen1ItemDefinition? selectedDefinition =
            Gen1ItemCatalog.Find(selected.ItemId);

        if (selectedDefinition?.IsInventorySafe != true)
        {
            MessageBox.Show(
                "Cet objet est inconnu ou impossible. Supprimez-le puis ajoutez un objet valide.",
                "Objet non modifiable",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        using InventoryItemForm dialog = new(
            "Modifier un objet",
            Gen1ItemCatalog.GetSafeItems(),
            selected);

        if (dialog.ShowDialog(this) != DialogResult.OK ||
            dialog.SelectedEntry is null)
        {
            return;
        }

        selected.ItemId = dialog.SelectedEntry.ItemId;
        selected.Quantity = dialog.SelectedEntry.Quantity;
        RefreshAll();
        SelectEntry(grid, selected.ItemId);
    }

    private void DeleteSelected(
        DataGridView grid,
        List<Gen1InventoryEntry> entries)
    {
        if (GetSelectedEntry(grid) is not Gen1InventoryEntry selected)
            return;

        entries.Remove(selected);
        RefreshAll();
    }

    private void ApplyButton_Click(object? sender, EventArgs e)
    {
        IReadOnlyList<Gen1InventoryIssue> issues =
            _saveFile.ValidateInventories(_bagItems, _pcItems);

        if (issues.Any(issue => issue.Severity == Gen1InventoryIssueSeverity.Error))
        {
            MessageBox.Show(
                "Corrigez les erreurs indiquées dans le contrôle d’intégrité avant d’appliquer.",
                "Inventaire invalide",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        if (issues.Count > 0 && MessageBox.Show(
                "Des doublons ordinaires ont été détectés. Voulez-vous conserver ces lignes séparées ?",
                "Doublons détectés",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes)
        {
            return;
        }

        try
        {
            _saveFile.SetInventory(
                _bagItems,
                _pcItems,
                decimal.ToInt32(_moneyInput.Value),
                decimal.ToInt32(_coinsInput.Value));

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception exception)
        {
            MessageBox.Show(
                exception.Message,
                "Modification impossible",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void RefreshAll()
    {
        RefreshGrid(_bagGrid, _bagItems);
        RefreshGrid(_pcGrid, _pcItems);
        _bagCountLabel.Text = $"{_bagItems.Count} / 20 lignes utilisées";
        _pcCountLabel.Text = $"{_pcItems.Count} / 50 lignes utilisées";

        IReadOnlyList<Gen1InventoryIssue> issues =
            _saveFile.ValidateInventories(_bagItems, _pcItems);

        _issuesList.Items.Clear();
        if (issues.Count == 0)
            _issuesList.Items.Add("✓ Aucun objet impossible, quantité invalide ou doublon détecté.");
        else
            _issuesList.Items.AddRange(issues.Select(issue => (object)issue.ToString()).ToArray());
    }

    private static void RefreshGrid(
        DataGridView grid,
        IReadOnlyList<Gen1InventoryEntry> entries)
    {
        grid.Rows.Clear();

        foreach (Gen1InventoryEntry entry in entries)
        {
            Gen1ItemDefinition? definition = Gen1ItemCatalog.Find(entry.ItemId);
            string state = GetEntryState(entry, entries);
            int rowIndex = grid.Rows.Add(
                definition?.Name ?? $"Inconnu (0x{entry.ItemId:X2})",
                entry.Quantity,
                definition is null
                    ? "Inconnu"
                    : Gen1ItemCatalog.GetCategoryName(definition.Category),
                state);
            grid.Rows[rowIndex].Tag = entry;

            if (state != "Valide")
                grid.Rows[rowIndex].DefaultCellStyle.ForeColor = ModernTheme.DangerColor;
        }
    }

    private static string GetEntryState(
        Gen1InventoryEntry entry,
        IReadOnlyList<Gen1InventoryEntry> entries)
    {
        Gen1ItemDefinition? definition = Gen1ItemCatalog.Find(entry.ItemId);
        if (definition is null || !definition.IsInventorySafe)
            return "Impossible";
        if (entry.Quantity is < 1 or > 99)
            return "Quantité invalide";
        if (definition.IsKeyItem && entry.Quantity != 1)
            return "Doit valoir 1";
        if (entries.Count(candidate => candidate.ItemId == entry.ItemId) > 1)
            return "Dupliqué";
        return "Valide";
    }

    private static Gen1InventoryEntry? GetSelectedEntry(DataGridView grid) =>
        grid.SelectedRows.Count > 0
            ? grid.SelectedRows[0].Tag as Gen1InventoryEntry
            : grid.CurrentRow?.Tag as Gen1InventoryEntry;

    private static void SelectEntry(DataGridView grid, byte itemId)
    {
        foreach (DataGridViewRow row in grid.Rows)
        {
            if (row.Tag is not Gen1InventoryEntry entry || entry.ItemId != itemId)
                continue;

            row.Selected = true;
            grid.CurrentCell = row.Cells[0];
            break;
        }
    }

    private static DataGridView CreateInventoryGrid()
    {
        DataGridView grid = new()
        {
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeRows = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            MultiSelect = false,
            ReadOnly = true,
            RowHeadersVisible = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect
        };
        grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Objet",
            FillWeight = 45F
        });
        grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Qté",
            FillWeight = 12F
        });
        grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Type",
            FillWeight = 21F
        });
        grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Contrôle",
            FillWeight = 22F
        });
        return grid;
    }

    private static void ConfigureNumberInput(
        NumericUpDown input,
        decimal maximum,
        decimal value)
    {
        input.Dock = DockStyle.Fill;
        input.Minimum = 0;
        input.Maximum = maximum;
        input.Value = Math.Clamp(value, 0, maximum);
        input.ThousandsSeparator = true;
        input.Margin = new Padding(8, 10, 24, 10);
    }

    private static Label CreateFieldLabel(string text) =>
        new()
        {
            Text = text,
            Dock = DockStyle.Fill,
            ForeColor = ModernTheme.MutedTextColor,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = new Padding(6)
        };

    private static FlowLayoutPanel CreateActionRow(bool wrapContents = false) =>
        new()
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = wrapContents,
            Padding = new Padding(0, 3, 0, 0)
        };

    private static Button CreateButton(
        string text,
        EventHandler onClick,
        string? tag = null)
    {
        Button button = new()
        {
            Text = text,
            AutoSize = true,
            Tag = tag,
            Margin = new Padding(3)
        };
        button.Click += onClick;
        return button;
    }

}
