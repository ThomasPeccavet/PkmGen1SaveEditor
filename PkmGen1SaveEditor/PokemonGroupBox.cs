using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using AppResources = PkmGen1SaveEditor.Properties.Resources;

namespace PkmGen1SaveEditor;

[DesignerCategory("Code")]
internal sealed class PokemonGroupBox : GroupBox
{
    private const int OriginalTileSize = 8;

    private int _pixelScale = 3;
    private Control? _observedParent;

    [Category("Pokemon Style")]
    [Description("Facteur d'agrandissement des sprites 8x8.")]
    [DefaultValue(3)]
    public int PixelScale
    {
        get => _pixelScale;

        set
        {
            _pixelScale = Math.Max(1, value);

            UpdatePadding();
            Invalidate();
        }
    }

    private int TileSize =>
        Math.Max(
            OriginalTileSize,
            OriginalTileSize * PixelScale * DeviceDpi / 96);

    /*
     * Utilise la couleur du contrôle contenant le GroupBox :
     * Form, Panel, TableLayoutPanel, etc.
     */
    private Color ContainerBackColor =>
        Parent?.BackColor ?? SystemColors.Control;

    public PokemonGroupBox()
    {
        DoubleBuffered = true;

        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw,
            true);

        ForeColor = Color.FromArgb(26, 39, 27);

        UpdatePadding();
    }

    protected override void OnParentChanged(EventArgs e)
    {
        /*
         * Désabonnement de l'ancien parent pour éviter de
         * conserver une référence vers un contrôle supprimé.
         */
        if (_observedParent is not null)
        {
            _observedParent.BackColorChanged -=
                Parent_BackColorChanged;
        }

        base.OnParentChanged(e);

        _observedParent = Parent;

        /*
         * Le GroupBox sera redessiné si la couleur du parent
         * est modifiée depuis le concepteur ou pendant l'exécution.
         */
        if (_observedParent is not null)
        {
            _observedParent.BackColorChanged +=
                Parent_BackColorChanged;
        }

        Invalidate();
    }

    private void Parent_BackColorChanged(
        object? sender,
        EventArgs e)
    {
        Invalidate();
    }

    private void UpdatePadding()
    {
        int tileSize = TileSize;

        Padding = new Padding(
            tileSize + 12,
            tileSize + 18,
            tileSize + 12,
            tileSize + 12);
    }

    protected override void OnPaintBackground(
        PaintEventArgs e)
    {
        e.Graphics.Clear(ContainerBackColor);
    }

    protected override void OnPaint(
        PaintEventArgs e)
    {
        Graphics graphics = e.Graphics;

        graphics.Clear(ContainerBackColor);

        graphics.InterpolationMode =
            InterpolationMode.NearestNeighbor;

        graphics.PixelOffsetMode =
            PixelOffsetMode.Half;

        graphics.CompositingMode =
            CompositingMode.SourceOver;

        DrawPixelBorder(graphics);
        DrawTitle(graphics);
    }

    private void DrawPixelBorder(
        Graphics graphics)
    {
        int tileSize = TileSize;

        if (Width < tileSize * 2 ||
            Height < tileSize * 2)
        {
            return;
        }

        int right = Width - tileSize;
        int bottom = Height - tileSize;

        using ImageAttributes attributes =
            CreateTransparencyAttributes();

        /*
         * Les segments sont dessinés avant les coins.
         * Les coins recouvrent ensuite les extrémités.
         */

        // Bordure horizontale supérieure.
        DrawHorizontalTiles(
            graphics,
            AppResources.Border_Horizontal,
            tileSize,
            0,
            Width - tileSize * 2,
            attributes);

        // Bordure horizontale inférieure.
        DrawHorizontalTiles(
            graphics,
            AppResources.Border_Horizontal,
            tileSize,
            bottom,
            Width - tileSize * 2,
            attributes);

        // Bordure verticale gauche.
        DrawVerticalTiles(
            graphics,
            AppResources.Border_Vertical,
            0,
            tileSize,
            Height - tileSize * 2,
            attributes);

        // Bordure verticale droite.
        DrawVerticalTiles(
            graphics,
            AppResources.Border_Vertical,
            right,
            tileSize,
            Height - tileSize * 2,
            attributes);

        // Coin supérieur gauche.
        DrawTile(
            graphics,
            AppResources.Border_TopLeft,
            0,
            0,
            attributes);

        // Coin supérieur droit.
        DrawTile(
            graphics,
            AppResources.Border_TopRight,
            right,
            0,
            attributes);

        // Coin inférieur gauche.
        DrawTile(
            graphics,
            AppResources.Border_BottomLeft,
            0,
            bottom,
            attributes);

        // Coin inférieur droit.
        DrawTile(
            graphics,
            AppResources.Border_BottomRight,
            right,
            bottom,
            attributes);
    }

    private void DrawHorizontalTiles(
        Graphics graphics,
        Image image,
        int startX,
        int y,
        int availableWidth,
        ImageAttributes attributes)
    {
        int tileSize = TileSize;

        GraphicsState state = graphics.Save();

        graphics.SetClip(
            new Rectangle(
                startX,
                y,
                availableWidth,
                tileSize),
            CombineMode.Intersect);

        int endX = startX + availableWidth;

        for (int x = startX;
             x < endX;
             x += tileSize)
        {
            DrawTile(
                graphics,
                image,
                x,
                y,
                attributes);
        }

        graphics.Restore(state);
    }

    private void DrawVerticalTiles(
        Graphics graphics,
        Image image,
        int x,
        int startY,
        int availableHeight,
        ImageAttributes attributes)
    {
        int tileSize = TileSize;

        GraphicsState state = graphics.Save();

        graphics.SetClip(
            new Rectangle(
                x,
                startY,
                tileSize,
                availableHeight),
            CombineMode.Intersect);

        int endY = startY + availableHeight;

        for (int y = startY;
             y < endY;
             y += tileSize)
        {
            DrawTile(
                graphics,
                image,
                x,
                y,
                attributes);
        }

        graphics.Restore(state);
    }

    private void DrawTile(
        Graphics graphics,
        Image image,
        int x,
        int y,
        ImageAttributes attributes)
    {
        int tileSize = TileSize;

        Rectangle destination = new(
            x,
            y,
            tileSize,
            tileSize);

        graphics.DrawImage(
            image,
            destination,
            0,
            0,
            image.Width,
            image.Height,
            GraphicsUnit.Pixel,
            attributes);
    }

    private void DrawTitle(
        Graphics graphics)
    {
        if (string.IsNullOrWhiteSpace(Text))
            return;

        string title = Text.ToUpperInvariant();

        Size textSize = TextRenderer.MeasureText(
            title,
            Font,
            Size.Empty,
            TextFormatFlags.NoPadding);

        int tileSize = TileSize;

        /*
         * Le fond derrière le titre reprend exactement
         * la couleur du contrôle parent.
         */
        Rectangle backgroundRectangle = new(
            tileSize + 8,
            0,
            textSize.Width + 20,
            tileSize);

        using SolidBrush backgroundBrush =
            new(ContainerBackColor);

        graphics.FillRectangle(
            backgroundBrush,
            backgroundRectangle);

        Rectangle textRectangle = new(
            backgroundRectangle.X + 10,
            backgroundRectangle.Y,
            textSize.Width,
            tileSize);

        TextRenderer.DrawText(
            graphics,
            title,
            Font,
            textRectangle,
            ForeColor,
            TextFormatFlags.Left |
            TextFormatFlags.VerticalCenter |
            TextFormatFlags.NoPadding);
    }

    private static ImageAttributes
        CreateTransparencyAttributes()
    {
        ImageAttributes attributes = new();

        /*
         * Les sprites ont un fond blanc opaque.
         * Le blanc devient transparent pendant le dessin.
         */
        attributes.SetColorKey(
            Color.White,
            Color.White,
            ColorAdjustType.Bitmap);

        return attributes;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing &&
            _observedParent is not null)
        {
            _observedParent.BackColorChanged -=
                Parent_BackColorChanged;

            _observedParent = null;
        }

        base.Dispose(disposing);
    }
}