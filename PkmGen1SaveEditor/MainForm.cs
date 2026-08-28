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
        cmbGameVersion.Items.Clear();
        cmbGameVersion.Items.Add("Pokémon Rouge / Bleu — Français");
        cmbGameVersion.Items.Add("Pokémon Red / Blue — English");
        cmbGameVersion.SelectedIndex = 0;
        cmbGameVersion.DropDownStyle = ComboBoxStyle.DropDownList;

        lblCurrentFile.Text = "Aucun fichier chargé";
        tslStatus.Text = "Prêt — ouvrez un fichier .sav";

        btnSaveAs.Enabled = false;
        grpTrainer.Enabled = false;
        grpBadges.Enabled = false;
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

            lblCurrentFile.Text = saveFile.FileName;

            tslStatus.Text =
                $"Fichier chargé — {saveFile.Data.Length:N0} octets";

            grpTrainer.Enabled = true;
            grpBadges.Enabled = true;
            btnSaveAs.Enabled = true;

            ResetDisplayedValues();
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
            File.WriteAllBytes(dialog.FileName, _currentSave.Data);

            tslStatus.Text =
                $"Sauvegarde enregistrée : {Path.GetFileName(dialog.FileName)}";

            MessageBox.Show(
                "La sauvegarde a correctement été enregistrée.",
                "Enregistrement terminé",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
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
}