using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Eyedropper
{
    public class ColorPicker
    {
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out Point lpPoint);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        public static Color GetColorAtCursor()
        {
            // Get cursor position
            GetCursorPos(out Point cursorPosition);

            // Create bitmap and graphics objects
            Bitmap bitmap = new Bitmap(1, 1);
            Graphics graphics = Graphics.FromImage(bitmap);

            // Copy screen pixel to bitmap
            BitBlt(graphics.GetHdc(), 0, 0, 1, 1, GetDesktopWindowDC(), cursorPosition.X, cursorPosition.Y, 0x00CC0020);
            graphics.ReleaseHdc();
            Color color = bitmap.GetPixel(0, 0);

            return color;
        }

        private static IntPtr GetDesktopWindowDC()
        {
            return GetDC(GetDesktopWindow());
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hWnd);

    }
}
