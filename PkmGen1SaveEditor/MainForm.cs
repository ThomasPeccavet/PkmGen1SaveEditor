using System;
using System.IO;
using System.Windows.Forms;

namespace PkmGen1SaveEditor;

public partial class MainForm : Form
{
    private Gen1SaveFile? _currentSave;
    private readonly Button _inventoryButton = new();
    private readonly Button _pokedexButton = new();

    public MainForm()
    {
        InitializeComponent();
        InitializeInterface();
    }

    private void InitializeInterface()
    {
        BuildModernInterface();

        cmbGameVersion.Items.Clear();
        cmbGameVersion.Items.Add("Pokémon Rouge / Bleu — Français");
        cmbGameVersion.Items.Add("Pokémon Red / Blue — English");
        cmbGameVersion.SelectedIndex = 0;
        cmbGameVersion.DropDownStyle = ComboBoxStyle.DropDownList;

        lblCurrentFile.Text = "Aucun fichier chargé";
        tslStatus.Text = "Prêt — ouvrez un fichier .sav";

        SetEditorEnabled(false);
    }

    private CheckBox[] BadgeCheckBoxes =>
    [
        chkBadgeBoulder,
        chkBadgeCascade,
        chkBadgeThunder,
        chkBadgeRainbow,
        chkBadgeSoul,
        chkBadgeMarsh,
        chkBadgeVolcano,
        chkBadgeEarth
    ];

    private void BuildModernInterface()
    {
        SuspendLayout();
        Controls.Clear();

        Text = "Pkm Gen 1 Save Editor";
        ClientSize = new Size(1080, 700);
        MinimumSize = new Size(900, 620);
        StartPosition = FormStartPosition.CenterScreen;
        ModernTheme.Apply(this);

        TableLayoutPanel root = new()
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(22),
            ColumnCount = 1,
            RowCount = 4
        };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 122F));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 68F));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));

        root.Controls.Add(CreateModernHeader(), 0, 0);
        root.Controls.Add(CreateModernContent(), 0, 1);
        root.Controls.Add(CreateModernActions(), 0, 2);

        statusStrip1.Dock = DockStyle.Fill;
        statusStrip1.Margin = new Padding(8, 0, 8, 0);
        root.Controls.Add(statusStrip1, 0, 3);
        Controls.Add(root);

        // Les anciens conteneurs du Designer ne sont plus affichés.
        // Leurs contrôles utiles ont été replacés dans la nouvelle interface.
        pnlToolbar.Dispose();
        grpTrainer.Dispose();
        grpBadges.Dispose();

        ModernTheme.StyleTree(this);
        ResumeLayout(performLayout: true);
    }

    private Control CreateModernHeader()
    {
        GlassPanel header = new()
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(8, 4, 8, 8),
            Padding = new Padding(24, 16, 24, 16)
        };

        TableLayoutPanel layout = new()
        {
            Dock = DockStyle.Fill,
            BackColor = Color.Transparent,
            ColumnCount = 3,
            RowCount = 2
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 205F));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 205F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 58F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 42F));

        layout.Controls.Add(new Label
        {
            Text = "Pkm Gen 1 Save Editor",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold),
            ForeColor = ModernTheme.TextColor,
            TextAlign = ContentAlignment.BottomLeft
        }, 0, 0);

        lblCurrentFile.Dock = DockStyle.Fill;
        lblCurrentFile.AutoEllipsis = true;
        lblCurrentFile.ForeColor = ModernTheme.MutedTextColor;
        lblCurrentFile.TextAlign = ContentAlignment.TopLeft;
        layout.Controls.Add(lblCurrentFile, 0, 1);

        btnOpenSave.Text = "Ouvrir une sauvegarde";
        btnOpenSave.Dock = DockStyle.Fill;
        btnOpenSave.Margin = new Padding(8, 14, 8, 14);
        btnOpenSave.Tag = "primary";
        layout.Controls.Add(btnOpenSave, 1, 0);
        layout.SetRowSpan(btnOpenSave, 2);

        btnSaveAs.Text = "Enregistrer sous…";
        btnSaveAs.Dock = DockStyle.Fill;
        btnSaveAs.Margin = new Padding(8, 14, 0, 14);
        layout.Controls.Add(btnSaveAs, 2, 0);
        layout.SetRowSpan(btnSaveAs, 2);

        header.Controls.Add(layout);
        return header;
    }

    private Control CreateModernContent()
    {
        TableLayoutPanel content = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1
        };
        content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52F));
        content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48F));

        content.Controls.Add(ModernTheme.CreateCard(
            "Dresseur",
            "Informations générales contenues dans la sauvegarde.",
            CreateTrainerFields()), 0, 0);
        content.Controls.Add(ModernTheme.CreateCard(
            "Badges de Kanto",
            "Cochez les badges obtenus avant l’export.",
            CreateBadgeFields()), 1, 0);
        return content;
    }

    private Control CreateTrainerFields()
    {
        TableLayoutPanel table = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 5
        };
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
        for (int row = 0; row < 5; row++)
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));

        AddMainField(table, 0, "Version", cmbGameVersion);
        AddMainField(table, 1, "Nom du joueur", txtPlayerName);
        AddMainField(table, 2, "Nom du rival", txtRivalName);
        AddMainField(table, 3, "Argent", numMoney);
        AddMainField(table, 4, "Temps de jeu", txtPlayTime);
        return table;
    }

    private Control CreateBadgeFields()
    {
        TableLayoutPanel table = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 4,
            Padding = new Padding(10)
        };
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        for (int row = 0; row < 4; row++)
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));

        CheckBox[] badges = BadgeCheckBoxes;
        for (int index = 0; index < badges.Length; index++)
        {
            CheckBox badge = badges[index];
            badge.Dock = DockStyle.Fill;
            badge.Font = new Font("Segoe UI", 10F);
            badge.Padding = new Padding(12, 0, 0, 0);
            table.Controls.Add(badge, index % 2, index / 2);
        }

        return table;
    }

    private Control CreateModernActions()
    {
        FlowLayoutPanel actions = new()
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
            Padding = new Padding(8, 9, 8, 5)
        };

        btnViewParty.Text = "Gérer l’équipe et les boîtes PC";
        btnViewParty.AutoSize = true;
        btnViewParty.Tag = "primary";
        actions.Controls.Add(btnViewParty);

        _inventoryButton.Text = "Gérer l’inventaire";
        _inventoryButton.AutoSize = true;
        _inventoryButton.Click += InventoryButton_Click;
        actions.Controls.Add(_inventoryButton);

        _pokedexButton.Text = "Gérer le Pokédex";
        _pokedexButton.AutoSize = true;
        _pokedexButton.Click += PokedexButton_Click;
        actions.Controls.Add(_pokedexButton);
        return actions;
    }

    private static void AddMainField(
        TableLayoutPanel table,
        int row,
        string title,
        Control input)
    {
        table.Controls.Add(new Label
        {
            Text = title,
            Dock = DockStyle.Fill,
            ForeColor = ModernTheme.MutedTextColor,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = new Padding(6)
        }, 0, row);

        input.Dock = DockStyle.Fill;
        input.Margin = new Padding(6, 10, 6, 10);
        table.Controls.Add(input, 1, row);
    }

    private void btnOpenSave_Click(object sender, EventArgs e)
    {
        using OpenFileDialog dialog = new()
        {
            Title = "Ouvrir une sauvegarde Pokémon",
            Filter =
                "Sauvegardes Game Boy (*.sav)|*.sav|" +
                "Tous les fichiers (*.*)|*.*",
            CheckFileExists = true,
            Multiselect = false
        };

        if (dialog.ShowDialog() != DialogResult.OK)
            return;

        try
        {
            Gen1SaveFile saveFile = new(dialog.FileName);

            _currentSave = saveFile;

            ResetDisplayedValues();

            SetEditorEnabled(true);

            lblCurrentFile.Text = saveFile.FileName;

            DisplaySaveData(saveFile);

            tslStatus.Text =
                $"Sauvegarde valide — checksum 0x{saveFile.StoredChecksum:X2}";
            btnViewParty.Enabled = true;
        }
        catch (InvalidDataException ex)
        {
            ResetLoadedSave();

            MessageBox.Show(
                ex.Message,
                "Sauvegarde incompatible",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
        catch (UnauthorizedAccessException)
        {
            ResetLoadedSave();

            MessageBox.Show(
                "Windows refuse l'accès à ce fichier.",
                "Accès refusé",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        catch (IOException ex)
        {
            ResetLoadedSave();

            MessageBox.Show(
                $"Impossible de lire le fichier :\n\n{ex.Message}",
                "Erreur de lecture",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        catch (Exception ex)
        {
            ResetLoadedSave();

            MessageBox.Show(
                $"Une erreur inattendue est survenue :\n\n{ex.Message}",
                "Erreur",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void btnSaveAs_Click(object sender, EventArgs e)
    {
        if (_currentSave is null)
        {
            MessageBox.Show(
                "Aucune sauvegarde n'est actuellement chargée.",
                "Enregistrement impossible",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            return;
        }

        string originalName =
            Path.GetFileNameWithoutExtension(_currentSave.FileName);

        using SaveFileDialog dialog = new()
        {
            Title = "Enregistrer la sauvegarde modifiée",
            Filter = "Sauvegardes Game Boy (*.sav)|*.sav",
            DefaultExt = "sav",
            AddExtension = true,
            OverwritePrompt = true,
            FileName = $"{originalName}_edited.sav"
        };

        if (dialog.ShowDialog() != DialogResult.OK)
            return;

        try
        {
            ApplyInterfaceChanges();
            _currentSave.UpdateChecksum();
            File.WriteAllBytes(dialog.FileName, _currentSave.Data);

            tslStatus.Text =
                $"Sauvegarde enregistrée : {Path.GetFileName(dialog.FileName)}";

            MessageBox.Show(
                "La sauvegarde a correctement été enregistrée.",
                "Enregistrement terminé",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(
                ex.Message,
                "Valeur incorrecte",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
        catch (UnauthorizedAccessException)
        {
            MessageBox.Show(
                "Windows refuse l'écriture dans ce dossier.",
                "Accès refusé",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        catch (IOException ex)
        {
            MessageBox.Show(
                $"Impossible d'enregistrer le fichier :\n\n{ex.Message}",
                "Erreur d'enregistrement",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void ResetDisplayedValues()
    {
        txtPlayerName.Clear();
        txtRivalName.Clear();
        txtPlayTime.Clear();

        numMoney.Value = 0;

        foreach (CheckBox checkBox in BadgeCheckBoxes)
            checkBox.Checked = false;
    }

    private void ResetLoadedSave()
    {
        _currentSave = null;

        lblCurrentFile.Text = "Aucun fichier chargé";
        tslStatus.Text = "Aucune sauvegarde compatible chargée";

        SetEditorEnabled(false);
        ResetDisplayedValues();
    }

    private void DisplaySaveData(Gen1SaveFile saveFile)
    {
        txtPlayerName.Text = saveFile.PlayerName;
        txtRivalName.Text = saveFile.RivalName;
        numMoney.Value = saveFile.Money;
        txtPlayTime.Text = saveFile.FormattedPlayTime;

        CheckBox[] badgeCheckBoxes = BadgeCheckBoxes;

        for (int index = 0;
             index < badgeCheckBoxes.Length && index < 8;
             index++)
        {
            badgeCheckBoxes[index].Checked =
                saveFile.HasBadge(index);
        }

        tslStatus.Text =
            $"Sauvegarde valide — checksum 0x{saveFile.StoredChecksum:X2}";
    }
    private void SetEditorEnabled(bool enabled)
    {
        btnSaveAs.Enabled = enabled;
        btnViewParty.Enabled = enabled;
        _inventoryButton.Enabled = enabled;
        _pokedexButton.Enabled = enabled;

        cmbGameVersion.Enabled = enabled;
        txtPlayerName.Enabled = enabled;
        txtRivalName.Enabled = enabled;
        numMoney.Enabled = enabled;

        foreach (CheckBox checkBox in BadgeCheckBoxes)
            checkBox.Enabled = enabled;

        // Le temps de jeu reste affiché, mais non modifiable pour le moment.
        txtPlayTime.Enabled = true;
        txtPlayTime.ReadOnly = true;
    }
    private void ApplyInterfaceChanges()
    {
        if (_currentSave is null)
            throw new InvalidOperationException(
                "Aucune sauvegarde n'est chargée.");

        _currentSave.SetPlayerName(txtPlayerName.Text);
        _currentSave.SetRivalName(txtRivalName.Text);
        _currentSave.SetMoney((int)numMoney.Value);

        CheckBox[] badgeCheckBoxes = BadgeCheckBoxes;

        for (int index = 0;
             index < badgeCheckBoxes.Length && index < 8;
             index++)
        {
            _currentSave.SetBadge(
                index,
                badgeCheckBoxes[index].Checked);
        }

        _currentSave.UpdateChecksum();
    }

    private void btnViewParty_Click(
        object sender,
        EventArgs e)
    {
        if (_currentSave is null)
        {
            MessageBox.Show(
                "Ouvrez d'abord une sauvegarde.",
                "Aucune sauvegarde",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            return;
        }

        using TeamForm teamForm = new(_currentSave);
        teamForm.ShowDialog(this);
    }

    private void InventoryButton_Click(object? sender, EventArgs e)
    {
        if (_currentSave is null)
        {
            MessageBox.Show(
                "Ouvrez d’abord une sauvegarde.",
                "Aucune sauvegarde",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        // Conserve la valeur éventuellement modifiée dans la fenêtre principale
        // avant d’ouvrir l’éditeur d’inventaire.
        _currentSave.SetMoney(decimal.ToInt32(numMoney.Value));

        using InventoryForm inventoryForm = new(_currentSave);
        if (inventoryForm.ShowDialog(this) == DialogResult.OK)
        {
            numMoney.Value = _currentSave.Money;
            tslStatus.Text = "Inventaire modifié en mémoire — exportez pour enregistrer.";
        }
    }

    private void PokedexButton_Click(object? sender, EventArgs e)
    {
        if (_currentSave is null)
        {
            MessageBox.Show(
                "Ouvrez d’abord une sauvegarde.",
                "Aucune sauvegarde",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        using PokedexForm pokedexForm = new(_currentSave);
        if (pokedexForm.ShowDialog(this) == DialogResult.OK)
        {
            tslStatus.Text = "Pokédex modifié en mémoire — exportez pour enregistrer.";
        }
    }
}
