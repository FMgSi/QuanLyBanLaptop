using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace QuanLyBanLaptop.Helpers
{
    public static class IconHelper
    {
        public static Bitmap CreateLaptopLogo(int size = 52)
        {
            var bmp = new Bitmap(size, size);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);

                // Laptop body outline
                using (var pen = new Pen(Color.FromArgb(92, 160, 245), 2.2f))
                using (var screenBrush = new SolidBrush(Color.FromArgb(40, 110, 200)))
                using (var baseBrush = new SolidBrush(Color.FromArgb(160, 200, 255)))
                {
                    // Screen outer
                    float screenW = size * 0.72f;
                    float screenH = size * 0.48f;
                    float screenX = (size - screenW) / 2f;
                    float screenY = size * 0.18f;
                    g.FillRectangle(screenBrush, screenX + 2, screenY + 2, screenW - 4, screenH - 4);
                    g.DrawRectangle(pen, screenX, screenY, screenW, screenH);

                    // Base / Keyboard part
                    float baseW = size * 0.88f;
                    float baseH = size * 0.09f;
                    float baseX = (size - baseW) / 2f;
                    float baseY = screenY + screenH + 1;
                    g.FillRectangle(baseBrush, baseX, baseY, baseW, baseH);
                    g.DrawRectangle(pen, baseX, baseY, baseW, baseH);

                    // Notch / Trackpad
                    using (var notchBrush = new SolidBrush(Color.FromArgb(11, 36, 71)))
                    {
                        float notchW = baseW * 0.28f;
                        float notchH = baseH * 0.5f;
                        float notchX = (size - notchW) / 2f;
                        g.FillRectangle(notchBrush, notchX, baseY, notchW, notchH);
                    }
                }
            }
            return bmp;
        }

        public static Bitmap CreateHomeIcon(int size = 20, Color? color = null)
        {
            Color c = color ?? Color.White;
            var bmp = new Bitmap(size, size);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);

                using (var pen = new Pen(c, 1.8f))
                using (var brush = new SolidBrush(c))
                {
                    // Roof points
                    PointF pTop = new PointF(size / 2f, size * 0.15f);
                    PointF pLeft = new PointF(size * 0.12f, size * 0.52f);
                    PointF pRight = new PointF(size * 0.88f, size * 0.52f);

                    g.DrawLine(pen, pLeft, pTop);
                    g.DrawLine(pen, pTop, pRight);

                    // House walls
                    float w = size * 0.60f;
                    float h = size * 0.40f;
                    float x = (size - w) / 2f;
                    float y = size * 0.50f;
                    g.DrawRectangle(pen, x, y, w, h);

                    // Door
                    float dw = w * 0.36f;
                    float dh = h * 0.60f;
                    float dx = (size - dw) / 2f;
                    float dy = y + h - dh;
                    g.FillRectangle(brush, dx, dy, dw, dh);
                }
            }
            return bmp;
        }

        public static Bitmap CreateLaptopIcon(int size = 20, Color? color = null)
        {
            Color c = color ?? Color.White;
            var bmp = new Bitmap(size, size);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);

                using (var pen = new Pen(c, 1.8f))
                using (var brush = new SolidBrush(c))
                {
                    // Screen
                    float sw = size * 0.76f;
                    float sh = size * 0.52f;
                    float sx = (size - sw) / 2f;
                    float sy = size * 0.16f;
                    g.DrawRectangle(pen, sx, sy, sw, sh);

                    // Base
                    float bw = size * 0.94f;
                    float bh = size * 0.10f;
                    float bx = (size - bw) / 2f;
                    float by = sy + sh + 2;
                    g.FillRectangle(brush, bx, by, bw, bh);
                }
            }
            return bmp;
        }

        public static Bitmap CreateCartIcon(int size = 20, Color? color = null)
        {
            Color c = color ?? Color.White;
            var bmp = new Bitmap(size, size);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);

                using (var pen = new Pen(c, 1.8f))
                using (var brush = new SolidBrush(c))
                {
                    // Handle and cart basket
                    PointF[] points = new PointF[]
                    {
                        new PointF(size * 0.08f, size * 0.20f),
                        new PointF(size * 0.24f, size * 0.20f),
                        new PointF(size * 0.40f, size * 0.65f),
                        new PointF(size * 0.86f, size * 0.65f),
                        new PointF(size * 0.94f, size * 0.32f),
                        new PointF(size * 0.28f, size * 0.32f)
                    };
                    g.DrawLines(pen, points);

                    // Wheels
                    float r = size * 0.12f;
                    g.FillEllipse(brush, size * 0.38f, size * 0.72f, r * 2, r * 2);
                    g.FillEllipse(brush, size * 0.72f, size * 0.72f, r * 2, r * 2);
                }
            }
            return bmp;
        }

        public static Bitmap CreateImportCartIcon(int size = 20, Color? color = null)
        {
            Color c = color ?? Color.White;
            var bmp = new Bitmap(size, size);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);

                using (var pen = new Pen(c, 1.8f))
                using (var brush = new SolidBrush(c))
                {
                    // Cart basket
                    PointF[] points = new PointF[]
                    {
                        new PointF(size * 0.08f, size * 0.20f),
                        new PointF(size * 0.24f, size * 0.20f),
                        new PointF(size * 0.40f, size * 0.65f),
                        new PointF(size * 0.86f, size * 0.65f),
                        new PointF(size * 0.94f, size * 0.32f),
                        new PointF(size * 0.28f, size * 0.32f)
                    };
                    g.DrawLines(pen, points);

                    // Down arrow indicating inbound/nhap hang inside basket
                    float arrowX = size * 0.60f;
                    g.DrawLine(pen, arrowX, size * 0.18f, arrowX, size * 0.48f);
                    PointF[] arrowHead = new PointF[]
                    {
                        new PointF(arrowX - size * 0.12f, size * 0.40f),
                        new PointF(arrowX, size * 0.52f),
                        new PointF(arrowX + size * 0.12f, size * 0.40f)
                    };
                    g.DrawLines(pen, arrowHead);

                    // Wheels
                    float r = size * 0.12f;
                    g.FillEllipse(brush, size * 0.38f, size * 0.72f, r * 2, r * 2);
                    g.FillEllipse(brush, size * 0.72f, size * 0.72f, r * 2, r * 2);
                }
            }
            return bmp;
        }

        public static Bitmap CreateUserIcon(int size = 20, Color? color = null)
        {
            Color c = color ?? Color.White;
            var bmp = new Bitmap(size, size);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);

                using (var brush = new SolidBrush(c))
                {
                    // Head
                    float headD = size * 0.36f;
                    float headX = (size - headD) / 2f;
                    float headY = size * 0.12f;
                    g.FillEllipse(brush, headX, headY, headD, headD);

                    // Shoulders / Body
                    float bodyW = size * 0.74f;
                    float bodyH = size * 0.40f;
                    float bodyX = (size - bodyW) / 2f;
                    float bodyY = size * 0.52f;
                    using (var path = new GraphicsPath())
                    {
                        path.AddArc(bodyX, bodyY, bodyW, bodyH * 2, 180, 180);
                        path.CloseFigure();
                        g.FillPath(brush, path);
                    }
                }
            }
            return bmp;
        }

        public static Bitmap CreateChartIcon(int size = 20, Color? color = null)
        {
            Color c = color ?? Color.White;
            var bmp = new Bitmap(size, size);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);

                using (var pen = new Pen(c, 1.8f))
                using (var brush = new SolidBrush(c))
                {
                    // Axis
                    g.DrawLine(pen, size * 0.12f, size * 0.88f, size * 0.90f, size * 0.88f);
                    g.DrawLine(pen, size * 0.12f, size * 0.88f, size * 0.12f, size * 0.15f);

                    // Bar 1
                    float barW = size * 0.18f;
                    g.FillRectangle(brush, size * 0.22f, size * 0.56f, barW, size * 0.30f);

                    // Bar 2
                    g.FillRectangle(brush, size * 0.45f, size * 0.38f, barW, size * 0.48f);

                    // Bar 3
                    g.FillRectangle(brush, size * 0.68f, size * 0.20f, barW, size * 0.66f);
                }
            }
            return bmp;
        }

        public static void EnsureIconsSaved(string baseDirectory)
        {
            try
            {
                string buttonDir = Path.Combine(baseDirectory, "Images", "Button");
                if (!Directory.Exists(buttonDir))
                    Directory.CreateDirectory(buttonDir);

                string[] fileNames = { "logo.png", "home.png", "laptop.png", "cart.png", "nhaphang.png", "user.png", "chart.png" };
                Func<Bitmap>[] creators = {
                    () => CreateLaptopLogo(56),
                    () => CreateHomeIcon(24),
                    () => CreateLaptopIcon(24),
                    () => CreateCartIcon(24),
                    () => CreateImportCartIcon(24),
                    () => CreateUserIcon(24),
                    () => CreateChartIcon(24)
                };

                for (int i = 0; i < fileNames.Length; i++)
                {
                    string path = Path.Combine(buttonDir, fileNames[i]);
                    if (!File.Exists(path))
                    {
                        using (var bmp = creators[i]())
                        {
                            bmp.Save(path, System.Drawing.Imaging.ImageFormat.Png);
                        }
                    }
                }
            }
            catch
            {
                // Ignore if IO issues in dev sandbox
            }
        }
    }
}
