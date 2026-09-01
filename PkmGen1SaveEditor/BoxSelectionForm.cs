namespace PkmGen1SaveEditor;

internal static class BoxSelectionForm
{
    public static int? SelectBox(IWin32Window owner, int excludedBox)
    {
        using Form form = new()
        {
            Text = "Déplacer vers une boîte",
            StartPosition = FormStartPosition.CenterParent,
            ClientSize = new Size(500, 300),
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MaximizeBox = false,
            MinimizeBox = false
        };

        ModernTheme.Apply(form);

        ComboBox input = new()
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Dock = DockStyle.Fill
        };

        for (int box = 1; box <= 12; box++)
            if (box != excludedBox) input.Items.Add(box);
        input.SelectedIndex = 0;

        Button confirm = new()
        {
            Text = "Déplacer",
            DialogResult = DialogResult.OK,
            AutoSize = true,
            Tag = "primary"
        };
        Button cancel = new()
        {
            Text = "Annuler",
            DialogResult = DialogResult.Cancel,
            AutoSize = true
        };

        TableLayoutPanel root = new()
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(22),
            ColumnCount = 1,
            RowCount = 2
        };
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));

        FlowLayoutPanel buttons = new()
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false,
            Padding = new Padding(0, 6, 0, 0)
        };
        buttons.Controls.Add(confirm);
        buttons.Controls.Add(cancel);

        root.Controls.Add(ModernTheme.CreateCard(
            "Boîte de destination",
            "Sélectionnez une autre boîte PC pour déplacer ce Pokémon.",
            input), 0, 0);
        root.Controls.Add(buttons, 0, 1);
        form.Controls.Add(root);
        ModernTheme.StyleTree(form);
        form.AcceptButton = confirm;
        form.CancelButton = cancel;
        return form.ShowDialog(owner) == DialogResult.OK
            ? (int?)input.SelectedItem
            : null;
    }
}
