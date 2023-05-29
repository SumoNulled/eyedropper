using System;
using System.Drawing;
using System.Windows.Forms;

namespace EyeDropper
{
    public partial class Magnify : Form
    {
        private Timer timer;
        private Bitmap magnifierBitmap;
        private Graphics magnifierGraphics;
        private const int MagnificationFactor = 9; // Adjust the magnification factor as desired

        public Magnify()
        {
            InitializeComponent();
            InitializeMagnifier();
        }

        private void InitializeMagnifier()
        {
            Size magnifierSize = new Size(9, 9); // Adjust the magnifier size as desired
            magnifierBitmap = new Bitmap(magnifierSize.Width, magnifierSize.Height);
            magnifierGraphics = Graphics.FromImage(magnifierBitmap);

            timer = new Timer();
            timer.Interval = 50; // Adjust the interval as desired (in milliseconds)
            timer.Tick += Timer_Tick;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            StartMagnifier();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            StopMagnifier();
            magnifierGraphics.Dispose();
            magnifierBitmap.Dispose();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Point cursorPosition = Cursor.Position;
            int magnifierSize = 9; // Adjust the magnifier size as desired
            int halfMagnifierSize = magnifierSize / 2;
            Rectangle magnifierRect = new Rectangle(cursorPosition.X - halfMagnifierSize, cursorPosition.Y - halfMagnifierSize, magnifierSize, magnifierSize);
            magnifierGraphics.CopyFromScreen(magnifierRect.Location, Point.Empty, magnifierRect.Size);

            Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.ScaleTransform(MagnificationFactor, MagnificationFactor);
            e.Graphics.DrawImage(magnifierBitmap, 0, 0, magnifierBitmap.Width, magnifierBitmap.Height);
        }

        private void StartMagnifier()
        {
            timer.Start();
        }

        private void StopMagnifier()
        {
            timer.Stop();
        }

        private void Magnify_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.White;
            this.TransparencyKey = Color.White;
        }
    }
}
