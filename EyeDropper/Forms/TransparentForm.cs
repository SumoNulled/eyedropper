using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EyeDropper.Forms
{
    public class TransparentForm : Form
    {
        private const int WS_EX_LAYERED = 0x80000;
        private const int WS_EX_TRANSPARENT = 0x20;

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        private static extern bool SetLayeredWindowAttributes(IntPtr hWnd, int crKey, byte bAlpha, int dwFlags);

        public TransparentForm()
        {
            InitializeForm();
        }

        private void InitializeForm()
        {
            // Set form properties
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            TopMost = true;

            // Make the form layered and transparent
            int extendedStyle = GetWindowLong(Handle, -20);
            extendedStyle |= WS_EX_LAYERED | WS_EX_TRANSPARENT;
            SetWindowLong(Handle, -20, extendedStyle);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= WS_EX_LAYERED | WS_EX_TRANSPARENT;
                return cp;
            }
        }

        public void SetOpacity(byte opacity)
        {
            SetLayeredWindowAttributes(Handle, 0, opacity, 2);
        }
    }
}
