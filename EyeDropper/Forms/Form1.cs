using System;
using System.Drawing;
using System.Windows.Forms;
using EyeDropper.Properties;
using Gma.System.MouseKeyHook;

namespace EyeDropper
{
    public partial class Form1 : Form
    {
        private IKeyboardMouseEvents globalHook;
        private Image overlayImage; // Image for the overlay
        private Timer timer;

        private Form currentForm;
        private string currentHexCode;

        // Event handler field for KeyDown event
        private KeyEventHandler keyDownHandler;

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Create a new transparent form
            Form transparentForm = new Form();

            // Set properties of the transparent form
            transparentForm.FormBorderStyle = FormBorderStyle.None;
            transparentForm.BackColor = Color.Black;
            transparentForm.Opacity = 0.01;
            transparentForm.TopMost = true;
            transparentForm.Cursor = Cursors.Cross;

            // Set the size of the transparent form to cover the entire screen
            Rectangle screenBounds = Screen.PrimaryScreen.Bounds;
            transparentForm.Bounds = screenBounds;

            // Show the transparent form
            transparentForm.Show();

            this.StartPosition = FormStartPosition.Manual;
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            this.BackColor = Color.White;
            this.TransparencyKey = Color.White;
            //this.Bounds = new Rectangle(Cursor.Position.X + 50, Cursor.Position.Y - (Height / 2), 200, 200);

            // Load the overlay image
            overlayImage = Resources.newcross;

            globalHook = Hook.GlobalEvents();
            globalHook.MouseMove += GlobalHook_MouseMove;
            globalHook.MouseClick += GlobalHook_MouseClick;
            globalHook.MouseDown += GlobalHook_MouseDown;

            // Create and start the timer
            timer = new Timer();
            timer.Interval = 500; // Adjust the interval as needed
            timer.Tick += Timer_Tick;
            timer.Start();

            // Assign the event handler to the field
            keyDownHandler = GlobalHook_KeyDown;
            globalHook.KeyDown += keyDownHandler;
            renderForm(new Magnify());

        }

        private void GlobalHook_MouseMove(object sender, MouseEventArgs e)
        {
            // Update the program position based on the cursor movement
            this.Location = new Point(Cursor.Position.X + 50, Cursor.Position.Y - (Height / 2));
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Point mousePosition = Control.MousePosition;
            Color pixelColor = GetPixelColor(mousePosition);

            // Display the hex code of the original color
            string hexCode = ColorToHexCode(pixelColor);
            labelHexCode.Text = hexCode;

            // Update the current hex code
            currentHexCode = hexCode;
        }

        private Color GetPixelColor(Point position)
        {
            using (Bitmap bitmap = new Bitmap(1, 1))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(position, Point.Empty, new Size(1, 1));
                }
                return bitmap.GetPixel(0, 0);
            }
        }

        private string ColorToHexCode(Color color)
        {
            return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }

        private void renderForm(Form form)
        {
            if (currentForm != null)
            {
                currentForm.Close();
            }
            currentForm = form;

            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            //form.BringToFront();

            panelMagnify.Controls.Add(form);
            panelMagnify.Tag = form;

            form.Show();
        }

        private void panelMagnify_Paint(object sender, PaintEventArgs e)
        {
            // Draw the image overlay on the panel
            e.Graphics.DrawImage(overlayImage, 0, 0, panelMagnify.Width, panelMagnify.Height);
        }

        private void GlobalHook_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Copy current hex code to clipboard
                Clipboard.SetText(currentHexCode);

                // Check if the hex code was successfully set in the clipboard
                if (Clipboard.ContainsText() && Clipboard.GetText() == currentHexCode)
                {
                    // Hex code was successfully copied, exit the application
                    Application.Exit();
                }
            }
        }

        private void GlobalHook_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Close the application
                Application.Exit();
            }
        }

        private void GlobalHook_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                // Close the application
                Application.Exit();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Unsubscribe from global hook events
            globalHook.MouseMove -= GlobalHook_MouseMove;
            globalHook.MouseClick -= GlobalHook_MouseClick;
            globalHook.MouseDown -= GlobalHook_MouseDown;
            globalHook.KeyDown -= keyDownHandler;

            // Dispose the global hook
            globalHook.Dispose();

            // Stop and dispose the timer
            timer.Stop();
            timer.Dispose();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Visible = true;
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
        }
    }
}
