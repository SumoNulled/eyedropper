using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EyeDropper.Forms
{
    public partial class OverlayForm : Form
    {
        public OverlayForm()
        {
            // Set the form properties
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.Manual;
            this.BackColor = Color.White;
            this.TransparencyKey = Color.White;
            this.TopMost = true;

            // Set the form size to cover the entire screen
            Rectangle screenBounds = Screen.PrimaryScreen.Bounds;
            this.Bounds = screenBounds;
            this.Size = screenBounds.Size;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Bring the form to the front
            this.BringToFront();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            // Prevent the overlay form from closing
            e.Cancel = true;
        }
    }

}
