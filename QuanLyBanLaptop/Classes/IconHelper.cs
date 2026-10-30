using System;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace QuanLyBanLaptop.Classes
{
    public static class IconHelper
    {
        private static Bitmap Draw(int size, Action<Graphics> render)
        {
            var bmp = new Bitmap(size, size);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);
                render(g);
            }
            return bmp;
        }
        public static Bitmap CreateLaptopLogo(int size = 52) => Draw(size, g =>
        {
            using (var pen = new Pen(Color.FromArgb(92, 160, 245), 2.2f))
            using (var screenBrush = new SolidBrush(Color.FromArgb(40, 110, 200)))
            using (var baseBrush = new SolidBrush(Color.FromArgb(160, 200, 255)))
            using (var notchBrush = new SolidBrush(Color.FromArgb(11, 36, 71)))
            {
                float sw = size * 0.72f, sh = size * 0.48f, sx = (size - sw) / 2f, sy = size * 0.18f;
                g.FillRectangle(screenBrush, sx + 2, sy + 2, sw - 4, sh - 4);
                g.DrawRectangle(pen, sx, sy, sw, sh);
                float bw = size * 0.88f, bh = size * 0.09f, bx = (size - bw) / 2f, by = sy + sh + 1;
                g.FillRectangle(baseBrush, bx, by, bw, bh);
                g.DrawRectangle(pen, bx, by, bw, bh);
                float nw = bw * 0.28f;
                g.FillRectangle(notchBrush, (size - nw) / 2f, by, nw, bh * 0.5f);
            }
        });
        public static Bitmap CreateHomeIcon(int size = 20, Color? color = null) => Draw(size, g =>
        {
            Color c = color ?? Color.White;
            using (var pen = new Pen(c, 1.8f))
            using (var brush = new SolidBrush(c))
            {
                PointF pTop = new PointF(size / 2f, size * 0.15f);
                g.DrawLine(pen, new PointF(size * 0.12f, size * 0.52f), pTop);
                g.DrawLine(pen, pTop, new PointF(size * 0.88f, size * 0.52f));
                float w = size * 0.60f, h = size * 0.40f, x = (size - w) / 2f, y = size * 0.50f;
                g.DrawRectangle(pen, x, y, w, h);
                float dw = w * 0.36f, dh = h * 0.60f;
                g.FillRectangle(brush, (size - dw) / 2f, y + h - dh, dw, dh);
            }
        });
        public static Bitmap CreateLaptopIcon(int size = 20, Color? color = null) => Draw(size, g =>
        {
            Color c = color ?? Color.White;
            using (var pen = new Pen(c, 1.8f))
            using (var brush = new SolidBrush(c))
            {
                float sw = size * 0.76f, sh = size * 0.52f, sx = (size - sw) / 2f, sy = size * 0.16f;
                g.DrawRectangle(pen, sx, sy, sw, sh);
                float bw = size * 0.94f;
                g.FillRectangle(brush, (size - bw) / 2f, sy + sh + 2, bw, size * 0.10f);
            }
        });
        public static Bitmap CreateCartIcon(int size = 20, Color? color = null) => Draw(size, g =>
        {
            Color c = color ?? Color.White;
            using (var pen = new Pen(c, 1.8f))
            using (var brush = new SolidBrush(c))
            {
                g.DrawLines(pen, new[]
                {
                    new PointF(size * 0.08f, size * 0.20f), new PointF(size * 0.24f, size * 0.20f),
                    new PointF(size * 0.40f, size * 0.65f), new PointF(size * 0.86f, size * 0.65f),
                    new PointF(size * 0.94f, size * 0.32f), new PointF(size * 0.28f, size * 0.32f)
                });
                float d = size * 0.24f;
                g.FillEllipse(brush, size * 0.38f, size * 0.72f, d, d);
                g.FillEllipse(brush, size * 0.72f, size * 0.72f, d, d);
            }
        });
        public static Bitmap CreateImportCartIcon(int size = 20, Color? color = null) => CreateCartIcon(size, color);
        public static Bitmap CreateUserIcon(int size = 20, Color? color = null) => Draw(size, g =>
        {
            using (var brush = new SolidBrush(color ?? Color.White))
            using (var path = new GraphicsPath())
            {
                float hd = size * 0.36f, bw = size * 0.74f;
                g.FillEllipse(brush, (size - hd) / 2f, size * 0.12f, hd, hd);
                path.AddArc((size - bw) / 2f, size * 0.52f, bw, size * 0.80f, 180, 180);
                path.CloseFigure();
                g.FillPath(brush, path);
            }
        });
        public static Bitmap CreateChartIcon(int size = 20, Color? color = null) => Draw(size, g =>
        {
            Color c = color ?? Color.White;
            using (var pen = new Pen(c, 1.8f))
            using (var brush = new SolidBrush(c))
            {
                g.DrawLine(pen, size * 0.12f, size * 0.88f, size * 0.90f, size * 0.88f);
                g.DrawLine(pen, size * 0.12f, size * 0.88f, size * 0.12f, size * 0.15f);
                float bw = size * 0.18f;
                g.FillRectangle(brush, size * 0.22f, size * 0.56f, bw, size * 0.30f);
                g.FillRectangle(brush, size * 0.45f, size * 0.38f, bw, size * 0.48f);
                g.FillRectangle(brush, size * 0.68f, size * 0.20f, bw, size * 0.66f);
            }
        });
        public static Bitmap CreateCustomerIcon(int size = 20, Color? color = null) => CreateUserIcon(size, color);
        public static Bitmap CreateInstallmentIcon(int size = 20, Color? color = null) => Draw(size, g =>
        {
            Color c = color ?? Color.FromArgb(245, 158, 11);
            using (var pen = new Pen(c, 1.8f))
            using (var brush = new SolidBrush(c))
            using (var font = new Font("Arial", size * 0.36f, FontStyle.Bold))
            {
                float x = size * 0.16f, y = size * 0.12f, w = size * 0.68f, h = size * 0.76f;
                g.DrawRectangle(pen, x, y, w, h);
                g.DrawLine(pen, x, y + h * 0.28f, x + w, y + h * 0.28f);
                var sz = g.MeasureString("$", font);
                g.DrawString("$", font, brush, x + (w - sz.Width) / 2f + 1, y + h * 0.32f);
            }
        });
        public static Bitmap CreateWrenchIcon(int size = 20, Color? color = null) => Draw(size, g =>
        {
            using (var pen = new Pen(color ?? Color.White, 2f))
            {
                g.DrawLine(pen, size * 0.28f, size * 0.72f, size * 0.65f, size * 0.35f);
                g.DrawArc(pen, size * 0.5f, size * 0.12f, size * 0.38f, size * 0.38f, 40, 260);
                g.DrawEllipse(pen, size * 0.16f, size * 0.68f, size * 0.18f, size * 0.18f);
            }
        });
        public static Bitmap CreateSettingsIcon(int size = 20, Color? color = null) => Draw(size, g =>
        {
            Color c = color ?? Color.White;
            using (var pen = new Pen(c, 1.8f))
            using (var gearPen = new Pen(c, 2.6f))
            {
                float cx = size / 2f, cy = size / 2f, rO = size * 0.40f, rI = size * 0.22f, rC = size * 0.11f;
                for (int i = 0; i < 6; i++)
                {
                    double a = i * Math.PI / 3.0;
                    g.DrawLine(gearPen, cx + (float)(Math.Cos(a) * (rO - 3)), cy + (float)(Math.Sin(a) * (rO - 3)),
                        cx + (float)(Math.Cos(a) * rO), cy + (float)(Math.Sin(a) * rO));
                }
                g.DrawEllipse(pen, cx - rI, cy - rI, rI * 2, rI * 2);
                g.DrawEllipse(pen, cx - rC, cy - rC, rC * 2, rC * 2);
            }
        });
    }
}
