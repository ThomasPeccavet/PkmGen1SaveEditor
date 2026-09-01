namespace PkmGen1SaveEditor;

internal sealed class PokemonDetailsForm : Form
{
    private readonly Gen1SaveFile _saveFile;
    private readonly Gen1Pokemon _pokemon;

    private readonly NumericUpDown _levelInput;
    private readonly NumericUpDown _experienceInput;
    private readonly NumericUpDown _currentHpInput;
    private readonly NumericUpDown _maximumHpInput;
    private readonly NumericUpDown _attackInput;
    private readonly NumericUpDown _defenseInput;
    private readonly NumericUpDown _speedInput;
    private readonly NumericUpDown _specialInput;

    private readonly NumericUpDown[] _dvInputs = new NumericUpDown[4];
    private readonly NumericUpDown[] _evInputs = new NumericUpDown[5];
    private readonly ComboBox[] _moveInputs = new ComboBox[4];
    private readonly NumericUpDown[] _ppInputs = new NumericUpDown[4];
    private readonly NumericUpDown[] _ppUpsInputs = new NumericUpDown[4];
    private readonly Label[] _maximumPpLabels = new Label[4];
    private readonly Label _hpDvValue = new();
    private readonly PictureBox _sprite = new();

    internal PokemonDetailsForm(
        Gen1SaveFile saveFile,
        Gen1Pokemon pokemon)
    {
        _saveFile = saveFile
            ?? throw new ArgumentNullException(nameof(saveFile));
        _pokemon = pokemon
            ?? throw new ArgumentNullException(nameof(pokemon));

        _levelInput = CreateNumericInput(1, 100, pokemon.Level);
        _experienceInput = CreateNumericInput(0, 0xFFFFFF, pokemon.Experience);
        _currentHpInput = CreateNumericInput(0, ushort.MaxValue, pokemon.CurrentHp);
        _maximumHpInput = CreateNumericInput(
            1,
            ushort.MaxValue,
            Math.Max(1, pokemon.MaximumHp));
        _attackInput = CreateNumericInput(
            1,
            ushort.MaxValue,
            Math.Max(1, pokemon.Attack));
        _defenseInput = CreateNumericInput(
            1,
            ushort.MaxValue,
            Math.Max(1, pokemon.Defense));
        _speedInput = CreateNumericInput(
            1,
            ushort.MaxValue,
            Math.Max(1, pokemon.Speed));
        _specialInput = CreateNumericInput(
            1,
            ushort.MaxValue,
            Math.Max(1, pokemon.Special));

        InitializeTrainingInputs();
        InitializeMoveInputs();
        InitializeInterface();

        if (!pokemon.IsInParty)
        {
            _maximumHpInput.Enabled = false;
            _attackInput.Enabled = false;
            _defenseInput.Enabled = false;
            _speedInput.Enabled = false;
            _specialInput.Enabled = false;
            RecalculateStats();
        }

        UpdateDerivedHpDv();
    }

    private void InitializeTrainingInputs()
    {
        byte[] dvs =
        [
            _pokemon.AttackDv,
            _pokemon.DefenseDv,
            _pokemon.SpeedDv,
            _pokemon.SpecialDv
        ];

        ushort[] evs =
        [
            _pokemon.HpEv,
            _pokemon.AttackEv,
            _pokemon.DefenseEv,
            _pokemon.SpeedEv,
            _pokemon.SpecialEv
        ];

        for (int index = 0; index < _dvInputs.Length; index++)
        {
            _dvInputs[index] = CreateNumericInput(0, 15, dvs[index]);
            _dvInputs[index].ValueChanged += (_, _) => UpdateDerivedHpDv();
        }

        for (int index = 0; index < _evInputs.Length; index++)
            _evInputs[index] = CreateNumericInput(0, ushort.MaxValue, evs[index]);
    }

    private void InitializeMoveInputs()
    {
        for (int index = 0; index < 4; index++)
        {
            Gen1MoveSlot move = _pokemon.Moves.Count > index
                ? _pokemon.Moves[index]
                : new Gen1MoveSlot { Slot = index + 1 };

            ComboBox moveInput = new()
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Dock = DockStyle.Fill,
                DisplayMember = nameof(MoveChoice.Label),
                Margin = new Padding(6)
            };

            foreach ((byte id, string name) in Gen1MoveCatalog.GetAll())
                moveInput.Items.Add(new MoveChoice(id, name));

            MoveChoice? selection = moveInput.Items
                .OfType<MoveChoice>()
                .FirstOrDefault(choice => choice.Id == move.MoveId);
            moveInput.SelectedItem = selection ?? moveInput.Items[0];

            NumericUpDown ppInput =
                CreateNumericInput(0, 63, move.CurrentPp);
            NumericUpDown ppUpsInput =
                CreateNumericInput(0, 3, move.PpUps);
            Label maximumPpLabel = new()
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = ModernTheme.MutedTextColor,
                Margin = new Padding(6)
            };

            int capturedIndex = index;
            moveInput.SelectedIndexChanged +=
                (_, _) => UpdateMaximumPp(capturedIndex);
            ppUpsInput.ValueChanged +=
                (_, _) => UpdateMaximumPp(capturedIndex);

            _moveInputs[index] = moveInput;
            _ppInputs[index] = ppInput;
            _ppUpsInputs[index] = ppUpsInput;
            _maximumPpLabels[index] = maximumPpLabel;
            UpdateMaximumPp(index);
        }
    }

    private void InitializeInterface()
    {
        string displayName = string.IsNullOrWhiteSpace(_pokemon.Nickname)
            ? _pokemon.SpeciesName
            : _pokemon.Nickname;

        Text = $"Modifier {displayName}";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(980, 760);
        MinimumSize = new Size(860, 680);
        ModernTheme.Apply(this);

        TableLayoutPanel root = new()
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(22),
            ColumnCount = 1,
            RowCount = 3
        };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 112F));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 62F));

        root.Controls.Add(CreateHeader(), 0, 0);
        root.Controls.Add(CreateTabs(), 0, 1);
        root.Controls.Add(CreateButtonsPanel(), 0, 2);
        Controls.Add(root);

        ModernTheme.StyleTree(this);
        Shown += async (_, _) =>
        {
            _sprite.Image =
                await PokemonSpriteService.GetAsync(_pokemon.SpeciesId);
        };
    }

    private Control CreateHeader()
    {
        GlassPanel header = new()
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(8, 4, 8, 8),
            Padding = new Padding(22, 14, 22, 14)
        };

        TableLayoutPanel layout = new()
        {
            Dock = DockStyle.Fill,
            BackColor = Color.Transparent,
            ColumnCount = 2,
            RowCount = 2
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 82F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));

        string nickname = string.IsNullOrWhiteSpace(_pokemon.Nickname)
            ? "Sans surnom"
            : _pokemon.Nickname;

        layout.Controls.Add(new Label
        {
            Text = _pokemon.SpeciesName,
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI Semibold", 21F, FontStyle.Bold),
            ForeColor = ModernTheme.TextColor,
            TextAlign = ContentAlignment.BottomLeft
        }, 0, 0);

        layout.Controls.Add(new Label
        {
            Text = $"{nickname}  •  {_pokemon.Types}  •  " +
                   $"Niveau {_pokemon.Level}  •  ID Dresseur {_pokemon.OriginalTrainerId}",
            Dock = DockStyle.Fill,
            AutoEllipsis = true,
            ForeColor = ModernTheme.MutedTextColor,
            TextAlign = ContentAlignment.TopLeft
        }, 0, 1);

        _sprite.Dock = DockStyle.Fill;
        _sprite.SizeMode = PictureBoxSizeMode.Zoom;
        _sprite.BackColor = Color.Transparent;
        layout.Controls.Add(_sprite, 1, 0);
        layout.SetRowSpan(_sprite, 2);
        header.Controls.Add(layout);
        return header;
    }

    private Control CreateTabs()
    {
        TabControl tabs = new()
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(6)
        };

        tabs.TabPages.Add(CreateStatsTab());
        tabs.TabPages.Add(CreateMovesTab());
        tabs.TabPages.Add(CreateTrainingTab());
        return tabs;
    }

    private TabPage CreateStatsTab()
    {
        TabPage page = new("Statistiques");

        TableLayoutPanel fields = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 4,
            RowCount = 5
        };
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));

        for (int row = 0; row < 5; row++)
            fields.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));

        AddEditorRow(fields, 0, "Niveau", _levelInput,
            "Expérience", _experienceInput);
        AddEditorRow(fields, 1, "PV actuels", _currentHpInput,
            "PV maximums", _maximumHpInput);
        AddEditorRow(fields, 2, "Attaque", _attackInput,
            "Défense", _defenseInput);
        AddEditorRow(fields, 3, "Vitesse", _speedInput,
            "Spécial", _specialInput);

        Button calculateButton = new()
        {
            Text = "Recalculer depuis les DV / EV",
            AutoSize = true,
            Anchor = AnchorStyles.Left,
            Margin = new Padding(6),
            Tag = "primary"
        };
        calculateButton.Click += (_, _) => RecalculateStats();
        fields.Controls.Add(calculateButton, 0, 4);
        fields.SetColumnSpan(calculateButton, 2);

        Label status = new()
        {
            Text = _pokemon.IsInParty
                ? $"Statut actuel : {_pokemon.Status}"
                : "Dans une boîte : les statistiques finales seront recalculées au retrait.",
            Dock = DockStyle.Fill,
            ForeColor = ModernTheme.MutedTextColor,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = new Padding(6)
        };
        fields.Controls.Add(status, 2, 4);
        fields.SetColumnSpan(status, 2);

        page.Controls.Add(ModernTheme.CreateCard(
            "Valeurs de combat",
            _pokemon.IsInParty
                ? "Les valeurs peuvent être saisies ou recalculées à partir des DV et EV."
                : "Aperçu des statistiques ; seules les données stockées dans le PC seront enregistrées.",
            fields));
        return page;
    }

    private TabPage CreateMovesTab()
    {
        TabPage page = new("Attaques & PP");
        TableLayoutPanel table = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 5,
            RowCount = 5
        };
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 72F));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
        table.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        for (int row = 1; row < 5; row++)
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));

        string[] headers = ["Slot", "Attaque", "PP actuels", "PP Plus", "PP max"];
        for (int column = 0; column < headers.Length; column++)
            table.Controls.Add(CreateColumnHeader(headers[column]), column, 0);

        for (int index = 0; index < 4; index++)
        {
            table.Controls.Add(new Label
            {
                Text = $"{index + 1}",
                Dock = DockStyle.Fill,
                Font = new Font(Font, FontStyle.Bold),
                ForeColor = ModernTheme.MutedTextColor,
                TextAlign = ContentAlignment.MiddleCenter
            }, 0, index + 1);
            table.Controls.Add(_moveInputs[index], 1, index + 1);
            table.Controls.Add(_ppInputs[index], 2, index + 1);
            table.Controls.Add(_ppUpsInputs[index], 3, index + 1);
            table.Controls.Add(_maximumPpLabels[index], 4, index + 1);
        }

        page.Controls.Add(ModernTheme.CreateCard(
            "Set d’attaques",
            "Choisissez jusqu’à quatre attaques et ajustez leurs PP sans dépasser la limite calculée.",
            table));
        return page;
    }

    private TabPage CreateTrainingTab()
    {
        TabPage page = new("DV & EV");
        TableLayoutPanel table = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            RowCount = 6
        };
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38F));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 31F));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 31F));
        table.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        for (int row = 1; row < 6; row++)
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));

        table.Controls.Add(CreateColumnHeader("Statistique"), 0, 0);
        table.Controls.Add(CreateColumnHeader("DV (0–15)"), 1, 0);
        table.Controls.Add(CreateColumnHeader("EV / Stat Exp (0–65 535)"), 2, 0);

        string[] labels = ["PV", "Attaque", "Défense", "Vitesse", "Spécial"];
        for (int row = 0; row < labels.Length; row++)
        {
            table.Controls.Add(CreateFieldLabel(labels[row]), 0, row + 1);
            table.Controls.Add(row == 0 ? _hpDvValue : _dvInputs[row - 1], 1, row + 1);
            table.Controls.Add(_evInputs[row], 2, row + 1);
        }

        _hpDvValue.Dock = DockStyle.Fill;
        _hpDvValue.TextAlign = ContentAlignment.MiddleLeft;
        _hpDvValue.Font = new Font(Font, FontStyle.Bold);
        _hpDvValue.ForeColor = ModernTheme.AccentColor;
        _hpDvValue.Margin = new Padding(6);

        page.Controls.Add(ModernTheme.CreateCard(
            "Valeurs d’entraînement",
            "En génération I, le DV des PV est dérivé des quatre autres DV. Les EV correspondent à la Stat Exp.",
            table));
        return page;
    }

    private Control CreateButtonsPanel()
    {
        FlowLayoutPanel panel = new()
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false,
            Padding = new Padding(8, 10, 8, 4)
        };

        Button applyButton = new()
        {
            Text = "Appliquer les modifications",
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

    private void RecalculateStats()
    {
        Gen1StatBlock stats = _saveFile.CalculateStatsPreview(
            _pokemon.SpeciesId,
            decimal.ToByte(_levelInput.Value),
            decimal.ToByte(_dvInputs[0].Value),
            decimal.ToByte(_dvInputs[1].Value),
            decimal.ToByte(_dvInputs[2].Value),
            decimal.ToByte(_dvInputs[3].Value),
            decimal.ToUInt16(_evInputs[0].Value),
            decimal.ToUInt16(_evInputs[1].Value),
            decimal.ToUInt16(_evInputs[2].Value),
            decimal.ToUInt16(_evInputs[3].Value),
            decimal.ToUInt16(_evInputs[4].Value));

        _maximumHpInput.Value = stats.MaximumHp;
        _attackInput.Value = stats.Attack;
        _defenseInput.Value = stats.Defense;
        _speedInput.Value = stats.Speed;
        _specialInput.Value = stats.Special;

        if (_pokemon.IsInParty && _currentHpInput.Value > stats.MaximumHp)
            _currentHpInput.Value = stats.MaximumHp;
    }

    private void UpdateDerivedHpDv()
    {
        int hpDv =
            ((decimal.ToByte(_dvInputs[0].Value) & 1) << 3) |
            ((decimal.ToByte(_dvInputs[1].Value) & 1) << 2) |
            ((decimal.ToByte(_dvInputs[2].Value) & 1) << 1) |
            (decimal.ToByte(_dvInputs[3].Value) & 1);
        _hpDvValue.Text = $"{hpDv}  (calculé automatiquement)";
    }

    private void UpdateMaximumPp(int index)
    {
        if (_moveInputs[index]?.SelectedItem is not MoveChoice move)
            return;

        int ppUps = decimal.ToInt32(_ppUpsInputs[index].Value);
        int maximumPp = move.Id == 0
            ? 0
            : Math.Min(63, Gen1MoveCatalog.GetBasePp(move.Id) * (5 + ppUps) / 5);

        if (_ppInputs[index].Value > maximumPp)
            _ppInputs[index].Value = maximumPp;
        _ppInputs[index].Maximum = maximumPp;
        if (move.Id == 0)
        {
            _ppInputs[index].Value = 0;
            _ppUpsInputs[index].Value = 0;
        }

        _ppInputs[index].Enabled = move.Id != 0;
        _ppUpsInputs[index].Enabled = move.Id != 0;
        _maximumPpLabels[index].Text = maximumPp.ToString();
    }

    private void ApplyButton_Click(object? sender, EventArgs e)
    {
        try
        {
            ushort currentHp = decimal.ToUInt16(_currentHpInput.Value);
            ushort maximumHp = decimal.ToUInt16(_maximumHpInput.Value);

            if (_pokemon.IsInParty && currentHp > maximumHp)
            {
                MessageBox.Show(
                    "Les PV actuels ne peuvent pas dépasser les PV maximums.",
                    "Valeur incorrecte",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                _currentHpInput.Focus();
                return;
            }

            List<Gen1MoveSlot> moves = [];
            for (int index = 0; index < 4; index++)
            {
                MoveChoice move = (MoveChoice)_moveInputs[index].SelectedItem!;
                moves.Add(new Gen1MoveSlot
                {
                    Slot = index + 1,
                    MoveId = move.Id,
                    CurrentPp = decimal.ToInt32(_ppInputs[index].Value),
                    PpUps = decimal.ToInt32(_ppUpsInputs[index].Value)
                });
            }

            _saveFile.SetPokemonAdvancedData(
                _pokemon.Slot - 1,
                _pokemon.BoxNumber,
                decimal.ToByte(_levelInput.Value),
                decimal.ToUInt32(_experienceInput.Value),
                currentHp,
                maximumHp,
                decimal.ToUInt16(_attackInput.Value),
                decimal.ToUInt16(_defenseInput.Value),
                decimal.ToUInt16(_speedInput.Value),
                decimal.ToUInt16(_specialInput.Value),
                decimal.ToByte(_dvInputs[0].Value),
                decimal.ToByte(_dvInputs[1].Value),
                decimal.ToByte(_dvInputs[2].Value),
                decimal.ToByte(_dvInputs[3].Value),
                decimal.ToUInt16(_evInputs[0].Value),
                decimal.ToUInt16(_evInputs[1].Value),
                decimal.ToUInt16(_evInputs[2].Value),
                decimal.ToUInt16(_evInputs[3].Value),
                decimal.ToUInt16(_evInputs[4].Value),
                moves);

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

    private static void AddEditorRow(
        TableLayoutPanel table,
        int row,
        string firstTitle,
        Control firstControl,
        string secondTitle,
        Control secondControl)
    {
        table.Controls.Add(CreateFieldLabel(firstTitle), 0, row);
        table.Controls.Add(firstControl, 1, row);
        table.Controls.Add(CreateFieldLabel(secondTitle), 2, row);
        table.Controls.Add(secondControl, 3, row);
    }

    private static Label CreateFieldLabel(string text) => new()
    {
        AutoSize = true,
        Dock = DockStyle.Fill,
        Text = text,
        TextAlign = ContentAlignment.MiddleLeft,
        ForeColor = ModernTheme.MutedTextColor,
        Margin = new Padding(6)
    };

    private static Label CreateColumnHeader(string text) => new()
    {
        Text = text,
        Dock = DockStyle.Fill,
        Font = new Font("Segoe UI", 9F, FontStyle.Bold),
        ForeColor = ModernTheme.MutedTextColor,
        TextAlign = ContentAlignment.MiddleLeft,
        Margin = new Padding(6)
    };

    private static NumericUpDown CreateNumericInput(
        decimal minimum,
        decimal maximum,
        decimal value)
    {
        return new NumericUpDown
        {
            Minimum = minimum,
            Maximum = maximum,
            Value = Math.Clamp(value, minimum, maximum),
            DecimalPlaces = 0,
            ThousandsSeparator = maximum > 999,
            Dock = DockStyle.Fill,
            Margin = new Padding(6),
            TextAlign = HorizontalAlignment.Right
        };
    }

    private sealed record MoveChoice(byte Id, string Name)
    {
        public string Label => Id == 0
            ? Name
            : $"{Name}  ·  PP {Gen1MoveCatalog.GetBasePp(Id)}";
    }
}
