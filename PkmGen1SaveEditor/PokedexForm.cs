namespace PkmGen1SaveEditor;

internal sealed class PokedexForm : Form
{
    private readonly Gen1SaveFile _saveFile;
    private readonly List<Gen1PokedexEntry> _entries;
    private readonly DataGridView _grid = new();
    private readonly TextBox _searchInput = new();
    private readonly Label _progressLabel = new();
    private bool _updatingGrid;

    internal PokedexForm(Gen1SaveFile saveFile)
    {
        _saveFile = saveFile ?? throw new ArgumentNullException(nameof(saveFile));
        _entries = saveFile.ReadPokedex().ToList();

        Text = "Pokédex de Kanto";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(920, 780);
        MinimumSize = new Size(760, 650);
        ShowInTaskbar = false;
        ModernTheme.Apply(this);

        BuildInterface();
        RefreshGrid();
        ModernTheme.StyleTree(this);
    }

    private void BuildInterface()
    {
        TableLayoutPanel root = new()
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(22),
            ColumnCount = 1,
            RowCount = 4
        };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 84F));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 68F));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));

        root.Controls.Add(CreateHeader(), 0, 0);
        root.Controls.Add(CreateToolbar(), 0, 1);
        root.Controls.Add(CreatePokedexCard(), 0, 2);
        root.Controls.Add(CreateDialogButtons(), 0, 3);
        Controls.Add(root);
    }

    private Control CreateHeader()
    {
        TableLayoutPanel header = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 2,
            Padding = new Padding(8, 0, 8, 0)
        };
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 310F));
        header.RowStyles.Add(new RowStyle(SizeType.Percent, 62F));
        header.RowStyles.Add(new RowStyle(SizeType.Percent, 38F));

        header.Controls.Add(new Label
        {
            Text = "Pokédex de Kanto",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI Semibold", 21F, FontStyle.Bold),
            ForeColor = ModernTheme.TextColor,
            TextAlign = ContentAlignment.BottomLeft
        }, 0, 0);
        header.Controls.Add(new Label
        {
            Text = "Gérez les Pokémon vus et capturés parmi les 151 espèces.",
            Dock = DockStyle.Fill,
            ForeColor = ModernTheme.MutedTextColor,
            TextAlign = ContentAlignment.TopLeft
        }, 0, 1);

        _progressLabel.Dock = DockStyle.Fill;
        _progressLabel.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
        _progressLabel.ForeColor = ModernTheme.AccentColor;
        _progressLabel.TextAlign = ContentAlignment.MiddleRight;
        header.Controls.Add(_progressLabel, 1, 0);
        header.SetRowSpan(_progressLabel, 2);
        return header;
    }

    private Control CreateToolbar()
    {
        TableLayoutPanel toolbar = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            Padding = new Padding(8, 8, 8, 8)
        };
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        _searchInput.Dock = DockStyle.Fill;
        _searchInput.PlaceholderText = "Rechercher par numéro ou par nom…";
        _searchInput.Margin = new Padding(0, 3, 16, 3);
        _searchInput.TextChanged += (_, _) => RefreshGrid();
        toolbar.Controls.Add(_searchInput, 0, 0);

        FlowLayoutPanel actions = new()
        {
            AutoSize = true,
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false
        };
        actions.Controls.Add(CreateButton("Tout voir", (_, _) => SetAll(seen: true, caught: false)));
        actions.Controls.Add(CreateButton("Tout capturer", (_, _) => SetAll(seen: true, caught: true), "primary"));
        actions.Controls.Add(CreateButton("Réinitialiser", ResetButton_Click, "danger"));
        toolbar.Controls.Add(actions, 1, 0);
        return toolbar;
    }

    private Control CreatePokedexCard()
    {
        ConfigureGrid();
        return ModernTheme.CreateCard(
            "Progression détaillée",
            "Cocher « Capturé » active automatiquement « Vu »; retirer « Vu » retire aussi la capture.",
            _grid);
    }

    private void ConfigureGrid()
    {
        _grid.Dock = DockStyle.Fill;
        _grid.AllowUserToAddRows = false;
        _grid.AllowUserToDeleteRows = false;
        _grid.AllowUserToResizeRows = false;
        _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _grid.MultiSelect = false;
        _grid.RowHeadersVisible = false;
        _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "N°",
            FillWeight = 12F,
            ReadOnly = true
        });
        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Pokémon",
            FillWeight = 58F,
            ReadOnly = true
        });
        _grid.Columns.Add(new DataGridViewCheckBoxColumn
        {
            HeaderText = "Vu",
            FillWeight = 15F
        });
        _grid.Columns.Add(new DataGridViewCheckBoxColumn
        {
            HeaderText = "Capturé",
            FillWeight = 15F
        });

        _grid.CurrentCellDirtyStateChanged += (_, _) =>
        {
            if (_grid.IsCurrentCellDirty)
                _grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        };
        _grid.CellValueChanged += Grid_CellValueChanged;
    }

    private Control CreateDialogButtons()
    {
        FlowLayoutPanel buttons = new()
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false,
            Padding = new Padding(8, 9, 8, 0)
        };

        Button applyButton = CreateButton("Appliquer", ApplyButton_Click, "primary");
        Button cancelButton = CreateButton("Annuler", (_, _) => Close());
        buttons.Controls.Add(applyButton);
        buttons.Controls.Add(cancelButton);
        AcceptButton = applyButton;
        CancelButton = cancelButton;
        return buttons;
    }

    private void RefreshGrid()
    {
        string query = _searchInput.Text.Trim();
        IEnumerable<Gen1PokedexEntry> filtered = _entries;

        if (query.Length > 0)
        {
            filtered = filtered.Where(entry =>
                entry.SpeciesName.Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
                entry.DexNumber.ToString().Contains(query, StringComparison.Ordinal));
        }

        _updatingGrid = true;
        _grid.Rows.Clear();

        foreach (Gen1PokedexEntry entry in filtered)
        {
            int rowIndex = _grid.Rows.Add(
                entry.DexNumber.ToString("D3"),
                entry.SpeciesName,
                entry.Seen,
                entry.Caught);
            _grid.Rows[rowIndex].Tag = entry;
        }

        _updatingGrid = false;
        UpdateProgress();
    }

    private void Grid_CellValueChanged(
        object? sender,
        DataGridViewCellEventArgs e)
    {
        if (_updatingGrid || e.RowIndex < 0 || e.ColumnIndex < 2)
            return;

        DataGridViewRow row = _grid.Rows[e.RowIndex];
        if (row.Tag is not Gen1PokedexEntry entry)
            return;

        bool value = Convert.ToBoolean(row.Cells[e.ColumnIndex].Value);

        _updatingGrid = true;
        if (e.ColumnIndex == 2)
        {
            entry.Seen = value;
            if (!value)
            {
                entry.Caught = false;
                row.Cells[3].Value = false;
            }
        }
        else
        {
            entry.Caught = value;
            if (value)
            {
                entry.Seen = true;
                row.Cells[2].Value = true;
            }
        }
        _updatingGrid = false;
        UpdateProgress();
    }

    private void SetAll(bool seen, bool caught)
    {
        foreach (Gen1PokedexEntry entry in _entries)
        {
            entry.Seen = seen || caught;
            entry.Caught = caught;
        }

        RefreshGrid();
    }

    private void ResetButton_Click(object? sender, EventArgs e)
    {
        if (MessageBox.Show(
                "Réinitialiser les indicateurs vus et capturés des 151 Pokémon ?\n\n" +
                "Vous pourrez encore fermer la fenêtre avec Annuler.",
                "Réinitialiser le Pokédex",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) != DialogResult.Yes)
        {
            return;
        }

        SetAll(seen: false, caught: false);
    }

    private void ApplyButton_Click(object? sender, EventArgs e)
    {
        try
        {
            _saveFile.SetPokedex(_entries);
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

    private void UpdateProgress()
    {
        int seenCount = _entries.Count(entry => entry.Seen);
        int caughtCount = _entries.Count(entry => entry.Caught);
        _progressLabel.Text =
            $"Vus  {seenCount} / 151     •     Capturés  {caughtCount} / 151";
    }

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
            Margin = new Padding(4, 0, 4, 0)
        };
        button.Click += onClick;
        return button;
    }
}
