using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PkmGen1SaveEditor;

internal partial class TeamForm : Form
{
    private readonly Gen1SaveFile _saveFile;

    private readonly DataGridView _partyGrid = new();
    private readonly Label _titleLabel = new();
    private readonly Label _informationLabel = new();
    private readonly Button _closeButton = new();
    private readonly Button _deleteButton = new();
    private readonly Button _addButton = new();

    internal TeamForm(Gen1SaveFile saveFile)
    {
        _saveFile = saveFile
            ?? throw new ArgumentNullException(nameof(saveFile));

        InitializeInterface();
        LoadParty();
    }

    private void InitializeInterface()
    {
        Text = "Équipe Pokémon";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(760, 390);
        MinimumSize = new Size(680, 360);

        BackColor = Color.FromArgb(232, 239, 199);
        ForeColor = Color.FromArgb(26, 39, 27);
        Font = new Font("Segoe UI", 9F);

        _titleLabel.Text = "ÉQUIPE POKÉMON";
        _titleLabel.Font = new Font(
            Font.FontFamily,
            14F,
            FontStyle.Bold);

        _titleLabel.AutoSize = true;
        _titleLabel.Location = new Point(20, 18);

        _informationLabel.AutoSize = true;
        _informationLabel.Location = new Point(22, 53);
        _informationLabel.ForeColor = Color.DimGray;

        ConfigurePartyGrid();

        _closeButton.Text = "Fermer";
        _closeButton.Size = new Size(110, 32);
        _closeButton.Anchor =
            AnchorStyles.Bottom |
            AnchorStyles.Right;

        _closeButton.Location = new Point(
            ClientSize.Width - _closeButton.Width - 20,
            ClientSize.Height - _closeButton.Height - 18);

        _closeButton.Click += (_, _) => Close();

        _deleteButton.Text = "Supprimer";
        _deleteButton.Size = new Size(110, 32);

        _deleteButton.Anchor =
            AnchorStyles.Bottom |
            AnchorStyles.Left;

        _deleteButton.Location = new Point(
            20,
            ClientSize.Height - _deleteButton.Height - 18);

        _addButton.Location = new Point(
            _deleteButton.Right + 10,
            _deleteButton.Top);

        _deleteButton.Enabled = false;
        _deleteButton.Click += DeleteButton_Click;

        _addButton.Text = "Ajouter";
        _addButton.Size = new Size(110, 32);
        _addButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _addButton.Location = new Point(
            _deleteButton.Right + 10,
            _deleteButton.Top);
        _addButton.Click += AddButton_Click;

        Controls.Add(_deleteButton);
        Controls.Add(_addButton);

        Controls.Add(_titleLabel);
        Controls.Add(_informationLabel);
        Controls.Add(_partyGrid);
        Controls.Add(_closeButton);

        Resize += TeamForm_Resize;

    }

    private void ConfigurePartyGrid()
    {
        _partyGrid.Location = new Point(20, 80);
        _partyGrid.Size = new Size(
            ClientSize.Width - 40,
            ClientSize.Height - 145);

        _partyGrid.Anchor =
            AnchorStyles.Top |
            AnchorStyles.Bottom |
            AnchorStyles.Left |
            AnchorStyles.Right;

        _partyGrid.AllowUserToAddRows = false;
        _partyGrid.AllowUserToDeleteRows = false;
        _partyGrid.AllowUserToResizeRows = false;
        _partyGrid.MultiSelect = false;
        _partyGrid.ReadOnly = true;
        _partyGrid.RowHeadersVisible = false;
        _partyGrid.SelectionMode =
            DataGridViewSelectionMode.FullRowSelect;

        _partyGrid.AutoSizeColumnsMode =
            DataGridViewAutoSizeColumnsMode.Fill;

        _partyGrid.BackgroundColor = BackColor;
        _partyGrid.BorderStyle = BorderStyle.FixedSingle;

        _partyGrid.ColumnHeadersDefaultCellStyle.BackColor =
            Color.FromArgb(26, 39, 27);

        _partyGrid.ColumnHeadersDefaultCellStyle.ForeColor =
            Color.White;

        _partyGrid.ColumnHeadersDefaultCellStyle.Font =
            new Font(Font, FontStyle.Bold);

        _partyGrid.EnableHeadersVisualStyles = false;

        _partyGrid.Columns.Add(
            new DataGridViewTextBoxColumn
            {
                Name = "Slot",
                HeaderText = "N°",
                FillWeight = 35
            });

        _partyGrid.Columns.Add(
            new DataGridViewTextBoxColumn
            {
                Name = "Nickname",
                HeaderText = "Surnom",
                FillWeight = 120
            });

        _partyGrid.Columns.Add(
            new DataGridViewTextBoxColumn
            {
                Name = "Species",
                HeaderText = "Espèce",
                FillWeight = 90
            });

        _partyGrid.Columns.Add(
            new DataGridViewTextBoxColumn
            {
                Name = "Level",
                HeaderText = "Niveau",
                FillWeight = 65
            });

        _partyGrid.Columns.Add(
            new DataGridViewTextBoxColumn
            {
                Name = "Hp",
                HeaderText = "PV",
                FillWeight = 90
            });

        _partyGrid.Columns.Add(
            new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "Statut",
                FillWeight = 90
            });

        _partyGrid.CellDoubleClick += PartyGrid_CellDoubleClick;
        _partyGrid.SelectionChanged += PartyGrid_SelectionChanged;
    }

    private void LoadParty()
    {
        _partyGrid.Rows.Clear();

        IReadOnlyList<Gen1Pokemon> party =
            _saveFile.ReadParty();

        foreach (Gen1Pokemon pokemon in party)
        {
            int rowIndex = _partyGrid.Rows.Add(
                pokemon.Slot,
                pokemon.Nickname,
                pokemon.SpeciesName,
                pokemon.Level,
                $"{pokemon.CurrentHp} / {pokemon.MaximumHp}",
                pokemon.Status);

            _partyGrid.Rows[rowIndex].Tag = pokemon;
        }

        _addButton.Enabled = party.Count < 6;

        _informationLabel.Text = party.Count switch
        {
            0 => "Aucun Pokémon trouvé dans l’équipe.",
            1 => "1 Pokémon dans l’équipe",
            _ => $"{party.Count} Pokémon dans l’équipe"
        };
    }

    private void TeamForm_Resize(
        object? sender,
        EventArgs e)
    {
        _closeButton.Location = new Point(
            ClientSize.Width - _closeButton.Width - 20,
            ClientSize.Height - _closeButton.Height - 18);

        _deleteButton.Location = new Point(
            20,
            ClientSize.Height - _deleteButton.Height - 18);
    }

    private void PartyGrid_CellDoubleClick(
    object? sender,
    DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0)
            return;

        DataGridViewRow row =
            _partyGrid.Rows[e.RowIndex];

        if (row.Tag is not Gen1Pokemon pokemon)
            return;

        using PokemonDetailsForm detailsForm =
            new(_saveFile, pokemon);

        DialogResult result =
            detailsForm.ShowDialog(this);

        if (result == DialogResult.OK)
        {
            // Relit les données modifiées et actualise la grille.
            LoadParty();
        }
    }
    private void PartyGrid_SelectionChanged(
    object? sender,
    EventArgs e)
    {
        _deleteButton.Enabled =
            GetSelectedPokemon() is not null;
    }

    private Gen1Pokemon? GetSelectedPokemon()
    {
        if (_partyGrid.CurrentRow?.Tag
            is Gen1Pokemon pokemon)
        {
            return pokemon;
        }

        return null;
    }

    private void DeleteButton_Click(
        object? sender,
        EventArgs e)
    {
        Gen1Pokemon? pokemon =
            GetSelectedPokemon();

        if (pokemon is null)
            return;

        string displayedName =
            string.IsNullOrWhiteSpace(pokemon.Nickname)
                ? pokemon.SpeciesName
                : pokemon.Nickname;

        DialogResult confirmation =
            MessageBox.Show(
                $"Supprimer {displayedName} de l'équipe ?\n\n" +
                "Le Pokémon sera définitivement retiré lorsque " +
                "la sauvegarde sera enregistrée.",
                "Confirmer la suppression",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

        if (confirmation != DialogResult.Yes)
            return;

        try
        {
            _saveFile.DeletePartyPokemon(
                pokemon.Slot - 1);

            LoadParty();

            _deleteButton.Enabled =
                GetSelectedPokemon() is not null;
        }
        catch (Exception exception)
        {
            MessageBox.Show(
                exception.Message,
                "Suppression impossible",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void AddButton_Click(
        object? sender,
        EventArgs e)
    {
        if (!_saveFile.CanAddPartyPokemon)
        {
            MessageBox.Show(
                "L'équipe contient déjà six Pokémon.",
                "Équipe complète",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        using AddPokemonForm form = new();

        if (form.ShowDialog(this) != DialogResult.OK)
            return;

        try
        {
            _saveFile.AddPartyPokemon(
                form.SelectedSpeciesId,
                form.SelectedLevel,
                form.SelectedNickname);

            LoadParty();
        }
        catch (Exception exception)
        {
            MessageBox.Show(
                exception.Message,
                "Ajout impossible",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

}
