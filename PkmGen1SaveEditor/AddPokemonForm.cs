using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PkmGen1SaveEditor;

internal sealed class AddPokemonForm : Form
{
    private readonly ComboBox _speciesInput = new();
    private readonly NumericUpDown _levelInput = new();
    private readonly TextBox _nicknameInput = new();

    public byte SelectedSpeciesId =>
        _speciesInput.SelectedItem is SpeciesChoice choice
            ? choice.Id
            : throw new InvalidOperationException("Aucune espèce sélectionnée.");

    public byte SelectedLevel => decimal.ToByte(_levelInput.Value);

    public string? SelectedNickname =>
        string.IsNullOrWhiteSpace(_nicknameInput.Text)
            ? null
            : _nicknameInput.Text.Trim();

    internal AddPokemonForm(
        string title = "Ajouter un Pokémon",
        string confirmText = "Ajouter")
    {
        Text = title;
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(430, 245);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        BackColor = Color.FromArgb(232, 239, 199);
        Font = new Font("Segoe UI", 9F);

        TableLayoutPanel layout = new()
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(20),
            ColumnCount = 2,
            RowCount = 4
        };

        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        _speciesInput.DropDownStyle = ComboBoxStyle.DropDownList;
        _speciesInput.Dock = DockStyle.Fill;
        _speciesInput.DisplayMember = nameof(SpeciesChoice.Name);

        foreach ((byte id, string name) in Gen1SpeciesCatalog.GetAll())
        {
            _speciesInput.Items.Add(new SpeciesChoice(id, name));
        }

        if (_speciesInput.Items.Count > 0)
            _speciesInput.SelectedIndex = 0;

        _levelInput.Minimum = 1;
        _levelInput.Maximum = 100;
        _levelInput.Value = 5;
        _levelInput.Dock = DockStyle.Fill;

        _nicknameInput.MaxLength = 10;
        _nicknameInput.CharacterCasing = CharacterCasing.Upper;
        _nicknameInput.Dock = DockStyle.Fill;
        _nicknameInput.PlaceholderText = "Facultatif";

        layout.Controls.Add(CreateLabel("Espèce"), 0, 0);
        layout.Controls.Add(_speciesInput, 1, 0);
        layout.Controls.Add(CreateLabel("Niveau"), 0, 1);
        layout.Controls.Add(_levelInput, 1, 1);
        layout.Controls.Add(CreateLabel("Surnom"), 0, 2);
        layout.Controls.Add(_nicknameInput, 1, 2);

        FlowLayoutPanel buttons = new()
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft
        };

        Button addButton = new()
        {
            Text = confirmText,
            DialogResult = DialogResult.OK,
            Size = new Size(100, 32)
        };

        Button cancelButton = new()
        {
            Text = "Annuler",
            DialogResult = DialogResult.Cancel,
            Size = new Size(100, 32)
        };

        buttons.Controls.Add(addButton);
        buttons.Controls.Add(cancelButton);
        layout.Controls.Add(buttons, 0, 3);
        layout.SetColumnSpan(buttons, 2);

        AcceptButton = addButton;
        CancelButton = cancelButton;
        Controls.Add(layout);
    }

    private static Label CreateLabel(string text) => new()
    {
        Text = text,
        Dock = DockStyle.Fill,
        TextAlign = ContentAlignment.MiddleLeft
    };

    private sealed record SpeciesChoice(byte Id, string Name);
}
