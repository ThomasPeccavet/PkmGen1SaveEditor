namespace PkmGen1SaveEditor;

internal static class BoxSelectionForm
{
    public static int? SelectBox(IWin32Window owner, int excludedBox)
    {
        using Form form = new()
        {
            Text = "Déplacer vers une boîte",
            StartPosition = FormStartPosition.CenterParent,
            ClientSize = new Size(340, 135),
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MaximizeBox = false,
            MinimizeBox = false,
            Font = new Font("Segoe UI", 9F)
        };

        ComboBox input = new()
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Location = new Point(20, 20),
            Width = 300
        };

        for (int box = 1; box <= 12; box++)
            if (box != excludedBox) input.Items.Add(box);
        input.SelectedIndex = 0;

        Button confirm = new()
        {
            Text = "Déplacer",
            DialogResult = DialogResult.OK,
            Location = new Point(110, 75),
            Size = new Size(100, 32)
        };
        Button cancel = new()
        {
            Text = "Annuler",
            DialogResult = DialogResult.Cancel,
            Location = new Point(220, 75),
            Size = new Size(100, 32)
        };

        form.Controls.AddRange([input, confirm, cancel]);
        form.AcceptButton = confirm;
        form.CancelButton = cancel;
        return form.ShowDialog(owner) == DialogResult.OK
            ? (int?)input.SelectedItem
            : null;
    }
}
