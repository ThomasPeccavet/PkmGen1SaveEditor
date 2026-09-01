namespace PkmGen1SaveEditor;

internal static class PokemonTheme
{
    public static Color WindowBackColor => Color.FromArgb(232, 239, 199);

    public static Color PanelBackColor => Color.FromArgb(216, 226, 179);

    public static Color InputBackColor => Color.FromArgb(248, 250, 231);

    public static Color TextColor => Color.FromArgb(26, 39, 27);

    public static Color AccentColor => Color.FromArgb(71, 91, 63);

    public static Color GridSelectionColor => Color.FromArgb(157, 178, 121);

    public static void Apply(Form form)
    {
        form.BackColor = WindowBackColor;
        form.ForeColor = TextColor;
        form.Font = new Font("Segoe UI", 9F);

        StyleDescendants(form);
    }

    public static void StyleDescendants(Control parent)
    {
        foreach (Control control in parent.Controls)
        {
            switch (control)
            {
                case Button button:
                    StyleButton(button);
                    break;

                case DataGridView grid:
                    StyleGrid(grid);
                    break;

                case TextBoxBase textBox:
                    textBox.BackColor = InputBackColor;
                    textBox.ForeColor = TextColor;
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    break;

                case ComboBox comboBox:
                    comboBox.BackColor = InputBackColor;
                    comboBox.ForeColor = TextColor;
                    comboBox.FlatStyle = FlatStyle.Flat;
                    break;

                case NumericUpDown numericInput:
                    numericInput.BackColor = InputBackColor;
                    numericInput.ForeColor = TextColor;
                    numericInput.BorderStyle = BorderStyle.FixedSingle;
                    break;

                case StatusStrip statusStrip:
                    statusStrip.BackColor = PanelBackColor;
                    statusStrip.ForeColor = TextColor;
                    statusStrip.SizingGrip = false;
                    break;
            }

            if (control.HasChildren)
                StyleDescendants(control);
        }
    }

    public static void StyleButton(Button button)
    {
        button.UseVisualStyleBackColor = false;
        button.BackColor = InputBackColor;
        button.ForeColor = TextColor;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderColor = AccentColor;
        button.FlatAppearance.BorderSize = 2;
        button.FlatAppearance.MouseOverBackColor = PanelBackColor;
        button.FlatAppearance.MouseDownBackColor = GridSelectionColor;
        button.Font = new Font(button.Font, FontStyle.Bold);
        button.Cursor = Cursors.Hand;
    }

    public static void StyleGrid(DataGridView grid)
    {
        grid.BackgroundColor = WindowBackColor;
        grid.BorderStyle = BorderStyle.FixedSingle;
        grid.GridColor = AccentColor;
        grid.EnableHeadersVisualStyles = false;

        grid.ColumnHeadersDefaultCellStyle.BackColor = AccentColor;
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        grid.ColumnHeadersDefaultCellStyle.Font =
            new Font(grid.Font, FontStyle.Bold);
        grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = AccentColor;

        grid.DefaultCellStyle.BackColor = InputBackColor;
        grid.DefaultCellStyle.ForeColor = TextColor;
        grid.DefaultCellStyle.SelectionBackColor = GridSelectionColor;
        grid.DefaultCellStyle.SelectionForeColor = TextColor;
        grid.AlternatingRowsDefaultCellStyle.BackColor = PanelBackColor;
    }

    public static PokemonGroupBox CreateGroup(string title) => new()
    {
        Text = title,
        Dock = DockStyle.Fill
    };
}
