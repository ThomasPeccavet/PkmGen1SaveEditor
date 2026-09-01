using System;
using System.IO;
using System.Windows.Forms;

namespace PkmGen1SaveEditor;

public partial class MainForm : Form
{
    private Gen1SaveFile? _currentSave;

    public MainForm()
    {
        InitializeComponent();
        InitializeInterface();
    }

    private void InitializeInterface()
    {
        PokemonTheme.Apply(this);

        cmbGameVersion.Items.Clear();
        cmbGameVersion.Items.Add("Pokémon Rouge / Bleu — Français");
        cmbGameVersion.Items.Add("Pokémon Red / Blue — English");
        cmbGameVersion.SelectedIndex = 0;
        cmbGameVersion.DropDownStyle = ComboBoxStyle.DropDownList;

        lblCurrentFile.Text = "Aucun fichier chargé";
        tslStatus.Text = "Prêt — ouvrez un fichier .sav";

        SetEditorEnabled(false);
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

        foreach (Control control in grpBadges.Controls)
        {
            if (control is CheckBox checkBox)
                checkBox.Checked = false;
        }
    }

    private void ResetLoadedSave()
    {
        _currentSave = null;

        lblCurrentFile.Text = "Aucun fichier chargé";
        tslStatus.Text = "Aucune sauvegarde compatible chargée";

        btnSaveAs.Enabled = false;
        grpTrainer.Enabled = false;
        grpBadges.Enabled = false;

        ResetDisplayedValues();
    }

    private void DisplaySaveData(Gen1SaveFile saveFile)
    {
        txtPlayerName.Text = saveFile.PlayerName;
        txtRivalName.Text = saveFile.RivalName;
        numMoney.Value = saveFile.Money;
        txtPlayTime.Text = saveFile.FormattedPlayTime;

        CheckBox[] badgeCheckBoxes = grpBadges.Controls
            .OfType<CheckBox>()
            .OrderBy(checkBox => checkBox.Top)
            .ToArray();

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
        grpTrainer.Enabled = enabled;
        grpBadges.Enabled = enabled;
        btnSaveAs.Enabled = enabled;

        cmbGameVersion.Enabled = enabled;
        txtPlayerName.Enabled = enabled;
        txtRivalName.Enabled = enabled;
        numMoney.Enabled = enabled;

        foreach (CheckBox checkBox in
                 grpBadges.Controls.OfType<CheckBox>())
        {
            checkBox.Enabled = enabled;
        }

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

        CheckBox[] badgeCheckBoxes = grpBadges.Controls
            .OfType<CheckBox>()
            .OrderBy(checkBox => checkBox.Top)
            .ToArray();

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
}
