using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace PkmGen1SaveEditor;

[DesignerCategory("Code")]
internal sealed class GlassPanel : Panel
{
    private const int CornerRadius = 18;

    public GlassPanel()
    {
        DoubleBuffered = true;
        BackColor = Color.Transparent;
        Padding = new Padding(20);

        SetStyle(
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.SupportsTransparentBackColor,
            true);
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
        base.OnPaintBackground(e);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        Rectangle bounds = new(0, 0, Width - 1, Height - 1);
        if (bounds.Width <= 0 || bounds.Height <= 0)
            return;

        using GraphicsPath path = CreateRoundedPath(bounds, CornerRadius);
        using LinearGradientBrush fill = new(
            bounds,
            Color.FromArgb(246, 255, 255, 255),
            Color.FromArgb(225, 236, 243, 252),
            LinearGradientMode.Vertical);
        using Pen border = new(
            Color.FromArgb(190, 255, 255, 255),
            1.4F);

        e.Graphics.FillPath(fill, path);
        e.Graphics.DrawPath(border, path);
    }

    private static GraphicsPath CreateRoundedPath(
        Rectangle bounds,
        int radius)
    {
        int diameter = Math.Max(2, Math.Min(radius * 2,
            Math.Min(bounds.Width, bounds.Height)));
        Rectangle arc = new(bounds.Location, new Size(diameter, diameter));
        GraphicsPath path = new();

        path.AddArc(arc, 180, 90);
        arc.X = bounds.Right - diameter;
        path.AddArc(arc, 270, 90);
        arc.Y = bounds.Bottom - diameter;
        path.AddArc(arc, 0, 90);
        arc.X = bounds.Left;
        path.AddArc(arc, 90, 90);
        path.CloseFigure();
        return path;
    }
}
