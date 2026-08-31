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

    internal PokemonDetailsForm(
        Gen1SaveFile saveFile,
        Gen1Pokemon pokemon)
    {
        _saveFile = saveFile
            ?? throw new ArgumentNullException(nameof(saveFile));

        _pokemon = pokemon
            ?? throw new ArgumentNullException(nameof(pokemon));

        _levelInput = CreateNumericInput(
            1,
            100,
            pokemon.Level);

        _experienceInput = CreateNumericInput(
            0,
            0xFFFFFF,
            pokemon.Experience);

        _currentHpInput = CreateNumericInput(
            0,
            ushort.MaxValue,
            pokemon.CurrentHp);

        _maximumHpInput = CreateNumericInput(
            1,
            ushort.MaxValue,
            pokemon.MaximumHp);

        _attackInput = CreateNumericInput(
            1,
            ushort.MaxValue,
            pokemon.Attack);

        _defenseInput = CreateNumericInput(
            1,
            ushort.MaxValue,
            pokemon.Defense);

        _speedInput = CreateNumericInput(
            1,
            ushort.MaxValue,
            pokemon.Speed);

        _specialInput = CreateNumericInput(
            1,
            ushort.MaxValue,
            pokemon.Special);

        InitializeInterface();
    }

    private void InitializeInterface()
    {
        Text = $"Détails de {_pokemon.Nickname}";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(650, 570);
        MinimumSize = new Size(610, 540);

        BackColor = Color.FromArgb(232, 239, 199);
        ForeColor = Color.FromArgb(26, 39, 27);
        Font = new Font("Segoe UI", 9F);

        TableLayoutPanel mainLayout = new()
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(18),
            ColumnCount = 1,
            RowCount = 4
        };

        mainLayout.RowStyles.Add(
            new RowStyle(SizeType.Absolute, 75F));

        mainLayout.RowStyles.Add(
            new RowStyle(SizeType.Percent, 55F));

        mainLayout.RowStyles.Add(
            new RowStyle(SizeType.Percent, 45F));

        mainLayout.RowStyles.Add(
            new RowStyle(SizeType.Absolute, 48F));

        mainLayout.Controls.Add(
            CreateHeader(),
            0,
            0);

        mainLayout.Controls.Add(
            CreateStatisticsGroup(),
            0,
            1);

        mainLayout.Controls.Add(
            CreateMovesGroup(),
            0,
            2);

        mainLayout.Controls.Add(
            CreateButtonsPanel(),
            0,
            3);

        Controls.Add(mainLayout);
    }

    private Control CreateHeader()
    {
        Panel header = new()
        {
            Dock = DockStyle.Fill
        };

        Label speciesLabel = new()
        {
            AutoSize = true,
            Location = new Point(4, 2),
            Text = _pokemon.SpeciesName.ToUpperInvariant(),
            Font = new Font(
                Font.FontFamily,
                16F,
                FontStyle.Bold)
        };

        string nickname = string.IsNullOrWhiteSpace(
            _pokemon.Nickname)
                ? "Sans surnom"
                : _pokemon.Nickname;

        Label nicknameLabel = new()
        {
            AutoSize = true,
            Location = new Point(6, 39),
            Text =
                $"Surnom : {nickname}   |   " +
                $"ID Dresseur : {_pokemon.OriginalTrainerId}",
            ForeColor = Color.DimGray
        };

        header.Controls.Add(speciesLabel);
        header.Controls.Add(nicknameLabel);

        return header;
    }

    private Control CreateStatisticsGroup()
    {
        GroupBox group = new()
        {
            Text = "STATISTIQUES MODIFIABLES",
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 0, 0, 10),
            Padding = new Padding(14)
        };

        TableLayoutPanel table = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 4,
            RowCount = 5
        };

        table.ColumnStyles.Add(
            new ColumnStyle(SizeType.Percent, 20F));

        table.ColumnStyles.Add(
            new ColumnStyle(SizeType.Percent, 30F));

        table.ColumnStyles.Add(
            new ColumnStyle(SizeType.Percent, 20F));

        table.ColumnStyles.Add(
            new ColumnStyle(SizeType.Percent, 30F));

        for (int row = 0; row < 5; row++)
        {
            table.RowStyles.Add(
                new RowStyle(SizeType.Percent, 20F));
        }

        AddEditorRow(
            table,
            0,
            "Niveau",
            _levelInput,
            "Expérience",
            _experienceInput);

        AddEditorRow(
            table,
            1,
            "PV actuels",
            _currentHpInput,
            "PV maximums",
            _maximumHpInput);

        AddEditorRow(
            table,
            2,
            "Attaque",
            _attackInput,
            "Défense",
            _defenseInput);

        AddEditorRow(
            table,
            3,
            "Vitesse",
            _speedInput,
            "Spécial",
            _specialInput);

        Label statusTitle = CreateFieldLabel("Statut");

        Label statusValue = new()
        {
            AutoSize = true,
            Dock = DockStyle.Fill,
            Text = _pokemon.Status,
            TextAlign = ContentAlignment.MiddleLeft,
            Font = new Font(
                Font,
                FontStyle.Bold)
        };

        table.Controls.Add(
            statusTitle,
            0,
            4);

        table.Controls.Add(
            statusValue,
            1,
            4);

        group.Controls.Add(table);

        return group;
    }

    private static void AddEditorRow(
        TableLayoutPanel table,
        int row,
        string firstTitle,
        Control firstControl,
        string secondTitle,
        Control secondControl)
    {
        table.Controls.Add(
            CreateFieldLabel(firstTitle),
            0,
            row);

        table.Controls.Add(
            firstControl,
            1,
            row);

        table.Controls.Add(
            CreateFieldLabel(secondTitle),
            2,
            row);

        table.Controls.Add(
            secondControl,
            3,
            row);
    }

    private static Label CreateFieldLabel(string text)
    {
        return new Label
        {
            AutoSize = true,
            Dock = DockStyle.Fill,
            Text = text,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = new Padding(4)
        };
    }

    private static NumericUpDown CreateNumericInput(
        decimal minimum,
        decimal maximum,
        decimal value)
    {
        return new NumericUpDown
        {
            Minimum = minimum,
            Maximum = maximum,
            Value = Math.Clamp(
                value,
                minimum,
                maximum),
            DecimalPlaces = 0,
            ThousandsSeparator = true,
            Dock = DockStyle.Fill,
            Margin = new Padding(4)
        };
    }

    private Control CreateMovesGroup()
    {
        GroupBox group = new()
        {
            Text = "ATTAQUES",
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 0, 0, 10),
            Padding = new Padding(12)
        };

        TableLayoutPanel table = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            RowCount = 5
        };

        table.ColumnStyles.Add(
            new ColumnStyle(SizeType.Percent, 18F));

        table.ColumnStyles.Add(
            new ColumnStyle(SizeType.Percent, 52F));

        table.ColumnStyles.Add(
            new ColumnStyle(SizeType.Percent, 30F));

        table.Controls.Add(
            CreateMoveLabel("Emplacement", true),
            0,
            0);

        table.Controls.Add(
            CreateMoveLabel("Attaque", true),
            1,
            0);

        table.Controls.Add(
            CreateMoveLabel("PP", true),
            2,
            0);

        foreach (Gen1MoveSlot move in _pokemon.Moves)
        {
            int row = move.Slot;

            string moveName = move.IsEmpty
                ? "—"
                : Gen1MoveCatalog.GetName(move.MoveId);

            string ppText = move.IsEmpty
                ? "—"
                : $"{move.CurrentPp} " +
                  $"(PP Plus : {move.PpUps})";

            table.Controls.Add(
                CreateMoveLabel($"N° {move.Slot}", false),
                0,
                row);

            table.Controls.Add(
                CreateMoveLabel(moveName, false),
                1,
                row);

            table.Controls.Add(
                CreateMoveLabel(ppText, false),
                2,
                row);
        }

        group.Controls.Add(table);

        return group;
    }

    private static Label CreateMoveLabel(
        string text,
        bool bold)
    {
        return new Label
        {
            AutoSize = true,
            Dock = DockStyle.Fill,
            Text = text,
            Font = bold
                ? new Font(
                    SystemFonts.MessageBoxFont,
                    FontStyle.Bold)
                : SystemFonts.MessageBoxFont,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = new Padding(4)
        };
    }

    private Control CreateButtonsPanel()
    {
        FlowLayoutPanel panel = new()
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false,
            Padding = new Padding(0, 8, 0, 0)
        };

        Button applyButton = new()
        {
            Text = "Appliquer",
            Size = new Size(115, 32)
        };

        applyButton.Click += ApplyButton_Click;

        Button cancelButton = new()
        {
            Text = "Annuler",
            Size = new Size(115, 32)
        };

        cancelButton.Click += (_, _) =>
        {
            DialogResult = DialogResult.Cancel;
            Close();
        };

        panel.Controls.Add(applyButton);
        panel.Controls.Add(cancelButton);

        AcceptButton = applyButton;
        CancelButton = cancelButton;

        return panel;
    }

    private void ApplyButton_Click(
        object? sender,
        EventArgs e)
    {
        try
        {
            ushort currentHp =
                decimal.ToUInt16(_currentHpInput.Value);

            ushort maximumHp =
                decimal.ToUInt16(_maximumHpInput.Value);

            if (currentHp > maximumHp)
            {
                MessageBox.Show(
                    "Les PV actuels ne peuvent pas dépasser " +
                    "les PV maximums.",
                    "Valeur incorrecte",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                _currentHpInput.Focus();
                return;
            }

            _saveFile.SetPartyPokemonStats(
                partyIndex: _pokemon.Slot - 1,

                level:
                    decimal.ToByte(_levelInput.Value),

                currentHp:
                    currentHp,

                maximumHp:
                    maximumHp,

                attack:
                    decimal.ToUInt16(_attackInput.Value),

                defense:
                    decimal.ToUInt16(_defenseInput.Value),

                speed:
                    decimal.ToUInt16(_speedInput.Value),

                special:
                    decimal.ToUInt16(_specialInput.Value),

                experience:
                    decimal.ToUInt32(_experienceInput.Value));

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
}