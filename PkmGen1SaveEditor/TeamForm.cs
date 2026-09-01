namespace PkmGen1SaveEditor;

internal partial class TeamForm : Form
{
    private readonly Gen1SaveFile _save;
    private readonly DataGridView _party = NewGrid();
    private readonly DataGridView _box = NewGrid();
    private readonly ComboBox _boxChoice = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 190 };
    private readonly TextBox _search = new() { Width = 220, PlaceholderText = "Nom, type ou attaque…" };
    private readonly Label _partyInfo = new() { AutoSize = true, ForeColor = ModernTheme.MutedTextColor };
    private readonly Label _boxInfo = new() { AutoSize = true, ForeColor = ModernTheme.MutedTextColor };
    private readonly Label _selectedInfo = new() { AutoSize = true, ForeColor = ModernTheme.MutedTextColor };
    private readonly PictureBox _sprite = new()
    {
        Size = new Size(78, 78),
        SizeMode = PictureBoxSizeMode.Zoom,
        BackColor = Color.Transparent
    };

    private readonly Dictionary<string, Button> _buttons = [];
    private byte _requestedSpriteSpecies;

    internal TeamForm(Gen1SaveFile saveFile)
    {
        _save = saveFile ?? throw new ArgumentNullException(nameof(saveFile));
        BuildInterface();
        LoadEverything();
    }

    private void BuildInterface()
    {
        Text = "Équipe et boîtes PC";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(1360, 780);
        MinimumSize = new Size(1120, 660);
        ModernTheme.Apply(this);

        TableLayoutPanel root = new()
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(22),
            RowCount = 3,
            ColumnCount = 1
        };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 104));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 58));

        root.Controls.Add(BuildHeader(), 0, 0);
        root.Controls.Add(BuildStorage(), 0, 1);
        root.Controls.Add(BuildFooter(), 0, 2);
        Controls.Add(root);
        ModernTheme.StyleTree(this);
    }

    private Control BuildHeader()
    {
        GlassPanel panel = new()
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(8, 4, 8, 8),
            Padding = new Padding(22, 12, 110, 12)
        };
        panel.Controls.Add(new Label
        {
            Text = "Équipe et stockage PC",
            AutoSize = true,
            Location = new Point(24, 14),
            Font = new Font("Segoe UI Semibold", 20, FontStyle.Bold)
        });
        _selectedInfo.Location = new Point(26, 58);
        _selectedInfo.Text = "Sélectionnez un Pokémon pour afficher ses informations.";
        _sprite.Location = new Point(panel.Width - 92, 8);
        _sprite.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        panel.Controls.Add(_selectedInfo);
        panel.Controls.Add(_sprite);
        return panel;
    }

    private Control BuildStorage()
    {
        SplitContainer split = new()
        {
            Dock = DockStyle.Fill
        };

        split.SizeChanged += (_, _) => ConfigureStorageSplitter(split);
        split.Panel1.Padding = new Padding(0, 0, 8, 0);
        split.Panel2.Padding = new Padding(8, 0, 0, 0);
        split.Panel1.Controls.Add(BuildPartyPanel());
        split.Panel2.Controls.Add(BuildBoxPanel());
        return split;
    }

    private static void ConfigureStorageSplitter(SplitContainer split)
    {
        const int desiredPanelMinimum = 470;
        int availableWidth = split.ClientSize.Width - split.SplitterWidth;

        // Pendant la construction du formulaire, le contrôle possède encore
        // sa petite largeur par défaut. On attend sa taille réellement allouée.
        if (availableWidth < desiredPanelMinimum * 2)
            return;

        int centeredDistance = availableWidth / 2;

        // La distance doit d'abord devenir compatible avec les futures
        // tailles minimales, sinon les setters peuvent eux-mêmes échouer.
        split.SplitterDistance = centeredDistance;
        split.Panel1MinSize = desiredPanelMinimum;
        split.Panel2MinSize = desiredPanelMinimum;

        int maximumDistance =
            split.ClientSize.Width - split.Panel2MinSize - split.SplitterWidth;

        split.SplitterDistance = Math.Clamp(
            centeredDistance,
            split.Panel1MinSize,
            maximumDistance);
    }

    private Control BuildPartyPanel()
    {
        ConfigurePartyGrid();
        TableLayoutPanel layout = NewSectionLayout(3, 28, 78);

        FlowLayoutPanel actions = NewActions();
        AddAction(actions, "add", "Ajouter", AddPokemon);
        AddAction(actions, "replace", "Remplacer", ReplacePokemon);
        AddAction(actions, "delete", "Supprimer", DeletePokemon);
        AddAction(actions, "up", "Monter", (_, _) => ReorderParty(-1));
        AddAction(actions, "down", "Descendre", (_, _) => ReorderParty(1));
        AddAction(actions, "duplicate", "Dupliquer", DuplicatePokemon);
        AddAction(actions, "heal", "Soigner tout", (_, _) => Run(_save.HealParty, "Soin impossible"));
        AddAction(actions, "deposit", "Déposer →", DepositPokemon);

        layout.Controls.Add(_partyInfo, 0, 0);
        layout.Controls.Add(_party, 0, 1);
        layout.Controls.Add(actions, 0, 2);
        return ModernTheme.CreateCard(
            "Équipe active",
            "Double-cliquez sur un Pokémon pour modifier ses stats, attaques, PP, DV et EV.",
            layout);
    }

    private Control BuildBoxPanel()
    {
        ConfigureBoxGrid();
        TableLayoutPanel layout = NewSectionLayout(4, 38, 28, 78);
        FlowLayoutPanel filters = NewActions();
        filters.WrapContents = false;
        filters.Controls.Add(new Label { Text = "Boîte :", AutoSize = true, Padding = new Padding(0, 7, 2, 0) });
        filters.Controls.Add(_boxChoice);
        filters.Controls.Add(_search);

        _boxChoice.SelectedIndexChanged += (_, _) => LoadBox();
        _search.TextChanged += (_, _) => LoadBox();

        FlowLayoutPanel actions = NewActions();
        AddAction(actions, "boxAdd", "Ajouter au PC", AddBoxPokemon);
        AddAction(actions, "boxDelete", "Supprimer", DeleteBoxPokemon);
        AddAction(actions, "withdraw", "← Retirer", WithdrawPokemon);
        AddAction(actions, "boxMove", "Déplacer", MoveBoxPokemon);

        layout.Controls.Add(filters, 0, 0);
        layout.Controls.Add(_boxInfo, 0, 1);
        layout.Controls.Add(_box, 0, 2);
        layout.Controls.Add(actions, 0, 3);
        return ModernTheme.CreateCard(
            "Boîtes PC",
            "Parcourez les 12 boîtes, recherchez un Pokémon ou transférez-le vers l’équipe.",
            layout);
    }

    private Control BuildFooter()
    {
        FlowLayoutPanel footer = NewActions();
        footer.FlowDirection = FlowDirection.RightToLeft;
        footer.Padding = new Padding(0, 6, 0, 0);
        Button close = NewButton("Fermer");
        close.Click += (_, _) => Close();
        footer.Controls.Add(close);
        return footer;
    }

    private void ConfigurePartyGrid()
    {
        AddColumns(_party,
            ("N°", 34), ("Surnom", 92), ("Espèce", 92),
            ("Niveau", 52), ("PV", 78), ("Statut", 65));
        _party.SelectionChanged += (_, _) => SelectionChanged(PartySelection());
        _party.CellDoubleClick += PartyDoubleClick;
    }

    private void ConfigureBoxGrid()
    {
        AddColumns(_box,
            ("N°", 34), ("Surnom", 92), ("Espèce", 92),
            ("Niveau", 52), ("Statut", 65), ("ID OT", 60));
        _box.SelectionChanged += (_, _) => SelectionChanged(BoxSelection());
        _box.CellDoubleClick += BoxDoubleClick;
    }

    private void LoadEverything()
    {
        int selectedBox = SelectedBox;
        IReadOnlyList<int> counts = _save.ReadBoxCounts();
        _boxChoice.BeginUpdate();
        _boxChoice.Items.Clear();
        for (int number = 1; number <= 12; number++)
        {
            string active = number == _save.CurrentBoxNumber ? " — active" : "";
            _boxChoice.Items.Add(new BoxItem(number,
                $"Boîte {number} ({counts[number - 1]}/20){active}"));
        }
        _boxChoice.EndUpdate();
        _boxChoice.SelectedIndex = Math.Clamp(selectedBox - 1, 0, 11);
        LoadParty();
        LoadBox();
        UpdateButtons();
    }

    private void LoadParty()
    {
        _party.Rows.Clear();
        IReadOnlyList<Gen1Pokemon> list = _save.ReadParty();
        foreach (Gen1Pokemon pokemon in list)
        {
            int row = _party.Rows.Add(
                pokemon.Slot, pokemon.Nickname, pokemon.SpeciesName,
                pokemon.Level, $"{pokemon.CurrentHp} / {pokemon.MaximumHp}",
                pokemon.Status);
            _party.Rows[row].Tag = pokemon;
        }
        _partyInfo.Text = $"{list.Count}/6 Pokémon — double-cliquez pour modifier les statistiques";
    }

    private void LoadBox()
    {
        if (_boxChoice.SelectedItem is not BoxItem item)
            return;

        _box.Rows.Clear();
        IReadOnlyList<Gen1Pokemon> all = _save.ReadBox(item.Number);
        string search = _search.Text.Trim();
        IEnumerable<Gen1Pokemon> shown = all;
        if (search.Length > 0)
        {
            shown = shown.Where(p =>
                p.Nickname.Contains(search, StringComparison.CurrentCultureIgnoreCase) ||
                p.SpeciesName.Contains(search, StringComparison.CurrentCultureIgnoreCase) ||
                p.Types.Contains(search, StringComparison.CurrentCultureIgnoreCase) ||
                p.MovesSummary.Contains(search, StringComparison.CurrentCultureIgnoreCase));
        }

        foreach (Gen1Pokemon pokemon in shown)
        {
            int row = _box.Rows.Add(
                pokemon.Slot, pokemon.Nickname, pokemon.SpeciesName,
                pokemon.Level, pokemon.Status,
                pokemon.OriginalTrainerId);
            _box.Rows[row].Tag = pokemon;
        }

        _boxInfo.Text = search.Length == 0
            ? $"{all.Count}/20 Pokémon"
            : $"{_box.Rows.Count} résultat(s) sur {all.Count}";
        UpdateButtons();
    }

    private void AddPokemon(object? sender, EventArgs e)
    {
        using AddPokemonForm form = new();
        if (form.ShowDialog(this) == DialogResult.OK)
            Run(() => _save.AddPartyPokemonCoherent(form.SelectedSpeciesId,
                form.SelectedLevel, form.SelectedNickname), "Ajout impossible");
    }

    private void ReplacePokemon(object? sender, EventArgs e)
    {
        Gen1Pokemon? selected = PartySelection();
        if (selected is null) return;
        using AddPokemonForm form = new("Remplacer le Pokémon", "Remplacer");
        if (form.ShowDialog(this) == DialogResult.OK)
            Run(() => _save.ReplacePartyPokemon(selected.Slot - 1,
                form.SelectedSpeciesId, form.SelectedLevel, form.SelectedNickname),
                "Remplacement impossible");
    }

    private void DeletePokemon(object? sender, EventArgs e)
    {
        Gen1Pokemon? selected = PartySelection();
        if (selected is null || _save.PartyCount <= 1 || !Confirm($"Supprimer {selected} de l'équipe ?")) return;
        Run(() => _save.DeletePartyPokemon(selected.Slot - 1), "Suppression impossible");
    }

    private void DuplicatePokemon(object? sender, EventArgs e)
    {
        Gen1Pokemon? selected = PartySelection();
        if (selected is not null)
            Run(() => _save.DuplicatePartyPokemon(selected.Slot - 1), "Duplication impossible");
    }

    private void DepositPokemon(object? sender, EventArgs e)
    {
        Gen1Pokemon? selected = PartySelection();
        if (selected is not null)
            Run(() => _save.DepositPartyPokemon(selected.Slot - 1, SelectedBox), "Dépôt impossible");
    }

    private void AddBoxPokemon(object? sender, EventArgs e)
    {
        using AddPokemonForm form = new("Ajouter dans la boîte", "Ajouter au PC");
        if (form.ShowDialog(this) == DialogResult.OK)
            Run(() => _save.AddBoxPokemon(SelectedBox, form.SelectedSpeciesId,
                form.SelectedLevel, form.SelectedNickname), "Ajout impossible");
    }

    private void DeleteBoxPokemon(object? sender, EventArgs e)
    {
        Gen1Pokemon? selected = BoxSelection();
        if (selected is null || !Confirm($"Supprimer définitivement {selected} du PC ?")) return;
        Run(() => _save.DeleteBoxPokemon(SelectedBox, selected.Slot - 1), "Suppression impossible");
    }

    private void WithdrawPokemon(object? sender, EventArgs e)
    {
        Gen1Pokemon? selected = BoxSelection();
        if (selected is not null)
            Run(() => _save.WithdrawBoxPokemon(SelectedBox, selected.Slot - 1), "Retrait impossible");
    }

    private void MoveBoxPokemon(object? sender, EventArgs e)
    {
        Gen1Pokemon? selected = BoxSelection();
        if (selected is null) return;
        int? destination = BoxSelectionForm.SelectBox(this, SelectedBox);
        if (destination is not null)
            Run(() => _save.MoveBoxPokemon(SelectedBox, selected.Slot - 1, destination.Value),
                "Déplacement impossible");
    }

    private void ReorderParty(int direction)
    {
        Gen1Pokemon? selected = PartySelection();
        if (selected is null) return;
        int from = selected.Slot - 1;
        int to = from + direction;
        if (to >= 0 && to < _save.PartyCount)
            Run(() => _save.MovePartyPokemon(from, to), "Réorganisation impossible");
    }

    private void PartyDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || _party.Rows[e.RowIndex].Tag is not Gen1Pokemon pokemon) return;
        using PokemonDetailsForm form = new(_save, pokemon);
        if (form.ShowDialog(this) == DialogResult.OK) LoadEverything();
    }

    private void BoxDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 ||
            _box.Rows[e.RowIndex].Tag is not Gen1Pokemon pokemon)
        {
            return;
        }

        using PokemonDetailsForm form = new(_save, pokemon);
        if (form.ShowDialog(this) == DialogResult.OK)
            LoadEverything();
    }

    private void Run(Action action, string title)
    {
        try { action(); LoadEverything(); }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void SelectionChanged(Gen1Pokemon? pokemon)
    {
        UpdateButtons();
        if (pokemon is null) return;
        _selectedInfo.Text = $"{pokemon} — {pokemon.SpeciesName} — {pokemon.Types} — Niveau {pokemon.Level}";
        byte requested = pokemon.SpeciesId;
        _requestedSpriteSpecies = requested;
        Image? image = await PokemonSpriteService.GetAsync(requested);
        if (_requestedSpriteSpecies == requested) _sprite.Image = image;
    }

    private void UpdateButtons()
    {
        Gen1Pokemon? party = PartySelection();
        Gen1Pokemon? box = BoxSelection();
        Set("add", _save.CanAddPartyPokemon);
        Set("replace", party is not null);
        Set("delete", party is not null && _save.PartyCount > 1);
        Set("up", party is { Slot: > 1 });
        Set("down", party is not null && party.Slot < _save.PartyCount);
        Set("duplicate", party is not null && _save.CanAddPartyPokemon);
        Set("heal", _save.PartyCount > 0);
        Set("deposit", party is not null && _save.PartyCount > 1);
        Set("boxDelete", box is not null);
        Set("withdraw", box is not null && _save.CanAddPartyPokemon);
        Set("boxMove", box is not null);
    }

    private void Set(string key, bool enabled)
    {
        if (_buttons.TryGetValue(key, out Button? button)) button.Enabled = enabled;
    }

    private Gen1Pokemon? PartySelection() => _party.CurrentRow?.Tag as Gen1Pokemon;
    private Gen1Pokemon? BoxSelection() => _box.CurrentRow?.Tag as Gen1Pokemon;
    private int SelectedBox => _boxChoice.SelectedItem is BoxItem item ? item.Number : 1;
    private bool Confirm(string text) => MessageBox.Show(text, "Confirmation",
        MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
        MessageBoxDefaultButton.Button2) == DialogResult.Yes;

    private void AddAction(FlowLayoutPanel panel, string key, string text, EventHandler handler)
    {
        Button button = NewButton(text);
        if (key is "add" or "boxAdd" or "heal")
            button.Tag = "primary";
        else if (key is "delete" or "boxDelete")
            button.Tag = "danger";
        button.Click += handler;
        _buttons[key] = button;
        panel.Controls.Add(button);
    }

    private static DataGridView NewGrid() => new()
    {
        Dock = DockStyle.Fill, ReadOnly = true, MultiSelect = false,
        AllowUserToAddRows = false, AllowUserToDeleteRows = false,
        AllowUserToResizeRows = false, RowHeadersVisible = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        BackgroundColor = ModernTheme.WindowBackColor
    };

    private static void AddColumns(DataGridView grid, params (string Header, float Weight)[] columns)
    {
        foreach ((string header, float weight) in columns)
            grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = header, FillWeight = weight });
    }

    private static TableLayoutPanel NewSectionLayout(int rows, params int[] fixedHeights)
    {
        TableLayoutPanel panel = new() { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = rows };
        foreach (int height in fixedHeights.Take(rows - 1))
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, height));
        panel.RowStyles.Insert(rows - 1, new RowStyle(SizeType.Percent, 100));
        if (fixedHeights.Length == rows - 1)
        {
            panel.RowStyles.Clear();
            for (int index = 0; index < rows; index++)
                panel.RowStyles.Add(index == rows - 2
                    ? new RowStyle(SizeType.Percent, 100)
                    : new RowStyle(SizeType.Absolute, fixedHeights[index < rows - 2 ? index : ^1]));
        }
        return panel;
    }

    private static FlowLayoutPanel NewActions() => new()
    {
        Dock = DockStyle.Fill, AutoScroll = true, WrapContents = true
    };

    private static Button NewButton(string text) => new()
    {
        Text = text, AutoSize = true, MinimumSize = new Size(104, 38), Margin = new Padding(4)
    };

    private sealed record BoxItem(int Number, string Label)
    {
        public override string ToString() => Label;
    }
}
