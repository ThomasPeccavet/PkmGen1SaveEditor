using System.Drawing.Drawing2D;

namespace PkmGen1SaveEditor;

internal static class ModernTheme
{
    public static Color WindowBackColor => Color.FromArgb(235, 241, 249);
    public static Color SurfaceColor => Color.FromArgb(248, 251, 255);
    public static Color TextColor => Color.FromArgb(24, 35, 52);
    public static Color MutedTextColor => Color.FromArgb(91, 106, 128);
    public static Color AccentColor => Color.FromArgb(70, 112, 255);
    public static Color AccentHoverColor => Color.FromArgb(52, 91, 222);
    public static Color SoftAccentColor => Color.FromArgb(222, 231, 255);
    public static Color DangerColor => Color.FromArgb(210, 67, 86);
    public static Color BorderColor => Color.FromArgb(205, 215, 229);

    public static void Apply(Form form)
    {
        form.BackColor = WindowBackColor;
        form.ForeColor = TextColor;
        form.Font = new Font("Segoe UI", 9.5F);
        form.AutoScaleMode = AutoScaleMode.Dpi;
    }

    public static void StyleTree(Control parent)
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
                    textBox.BackColor = SurfaceColor;
                    textBox.ForeColor = TextColor;
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    break;

                case ComboBox comboBox:
                    comboBox.BackColor = SurfaceColor;
                    comboBox.ForeColor = TextColor;
                    comboBox.FlatStyle = FlatStyle.Flat;
                    break;

                case NumericUpDown numericInput:
                    numericInput.BackColor = SurfaceColor;
                    numericInput.ForeColor = TextColor;
                    numericInput.BorderStyle = BorderStyle.FixedSingle;
                    break;

                case TabControl tabs:
                    tabs.Padding = new Point(18, 7);
                    tabs.ItemSize = new Size(130, 34);
                    tabs.SizeMode = TabSizeMode.Fixed;
                    break;

                case TabPage page:
                    page.BackColor = WindowBackColor;
                    page.Padding = new Padding(8);
                    break;

                case StatusStrip statusStrip:
                    statusStrip.BackColor = Color.FromArgb(225, 233, 244);
                    statusStrip.ForeColor = MutedTextColor;
                    statusStrip.SizingGrip = false;
                    break;
            }

            if (control.HasChildren)
                StyleTree(control);
        }
    }

    public static void StyleButton(Button button)
    {
        bool primary = Equals(button.Tag, "primary");
        bool danger = Equals(button.Tag, "danger");

        button.UseVisualStyleBackColor = false;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = primary || danger ? 0 : 1;
        button.FlatAppearance.BorderColor = BorderColor;
        button.BackColor = primary
            ? AccentColor
            : danger
                ? DangerColor
                : Color.FromArgb(245, 248, 253);
        button.ForeColor = primary || danger ? Color.White : TextColor;
        button.FlatAppearance.MouseOverBackColor = primary
            ? AccentHoverColor
            : danger
                ? Color.FromArgb(188, 51, 70)
                : SoftAccentColor;
        button.FlatAppearance.MouseDownBackColor = primary
            ? Color.FromArgb(40, 75, 190)
            : Color.FromArgb(210, 221, 245);
        button.Font = new Font(button.Font, FontStyle.Bold);
        button.Cursor = Cursors.Hand;
        button.MinimumSize = new Size(104, 38);

        ApplyRoundedRegion(button, 10);
        button.SizeChanged += (_, _) => ApplyRoundedRegion(button, 10);
    }

    public static void StyleGrid(DataGridView grid)
    {
        grid.BackgroundColor = Color.FromArgb(239, 244, 251);
        grid.BorderStyle = BorderStyle.None;
        grid.GridColor = BorderColor;
        grid.EnableHeadersVisualStyles = false;
        grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        grid.RowTemplate.Height = 36;
        grid.ColumnHeadersHeight = 40;
        grid.ColumnHeadersHeightSizeMode =
            DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

        grid.ColumnHeadersDefaultCellStyle.BackColor =
            Color.FromArgb(225, 233, 245);
        grid.ColumnHeadersDefaultCellStyle.ForeColor = TextColor;
        grid.ColumnHeadersDefaultCellStyle.Font =
            new Font(grid.Font, FontStyle.Bold);
        grid.ColumnHeadersDefaultCellStyle.SelectionBackColor =
            Color.FromArgb(225, 233, 245);
        grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(6, 0, 6, 0);

        grid.DefaultCellStyle.BackColor = Color.FromArgb(250, 252, 255);
        grid.DefaultCellStyle.ForeColor = TextColor;
        grid.DefaultCellStyle.SelectionBackColor = SoftAccentColor;
        grid.DefaultCellStyle.SelectionForeColor = TextColor;
        grid.DefaultCellStyle.Padding = new Padding(6, 2, 6, 2);
        grid.AlternatingRowsDefaultCellStyle.BackColor =
            Color.FromArgb(243, 247, 252);
    }

    public static GlassPanel CreateCard(
        string title,
        string? subtitle,
        Control content)
    {
        GlassPanel card = new()
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(8)
        };

        TableLayoutPanel layout = new()
        {
            Dock = DockStyle.Fill,
            BackColor = Color.Transparent,
            ColumnCount = 1,
            RowCount = subtitle is null ? 2 : 3
        };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));

        Label titleLabel = new()
        {
            Text = title,
            Dock = DockStyle.Fill,
            AutoEllipsis = true,
            Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
            ForeColor = TextColor,
            TextAlign = ContentAlignment.MiddleLeft
        };

        layout.Controls.Add(titleLabel, 0, 0);

        int contentRow = 1;
        if (subtitle is not null)
        {
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            layout.Controls.Add(new Label
            {
                Text = subtitle,
                Dock = DockStyle.Fill,
                AutoEllipsis = true,
                ForeColor = MutedTextColor,
                TextAlign = ContentAlignment.MiddleLeft
            }, 0, 1);
            contentRow = 2;
        }

        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        content.Dock = DockStyle.Fill;
        content.Margin = new Padding(0, 8, 0, 0);
        layout.Controls.Add(content, 0, contentRow);
        card.Controls.Add(layout);
        return card;
    }

    public static Label CreateSectionTitle(string title, string subtitle)
    {
        return new Label
        {
            AutoSize = false,
            Dock = DockStyle.Fill,
            Text = $"{title}\n{subtitle}",
            Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
            ForeColor = TextColor,
            TextAlign = ContentAlignment.MiddleLeft
        };
    }

    private static void ApplyRoundedRegion(Control control, int radius)
    {
        if (control.Width <= 0 || control.Height <= 0)
            return;

        int diameter = radius * 2;
        Rectangle bounds = new(0, 0, control.Width, control.Height);
        GraphicsPath path = new();
        path.AddArc(0, 0, diameter, diameter, 180, 90);
        path.AddArc(bounds.Right - diameter, 0, diameter, diameter, 270, 90);
        path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter,
            diameter, diameter, 0, 90);
        path.AddArc(0, bounds.Bottom - diameter,
            diameter, diameter, 90, 90);
        path.CloseFigure();

        Region? previous = control.Region;
        control.Region = new Region(path);
        previous?.Dispose();
        path.Dispose();
    }
}
