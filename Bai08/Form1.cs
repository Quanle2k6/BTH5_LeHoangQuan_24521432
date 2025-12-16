using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Bai08
{
    public class OptimizedPanel : Panel
    {
        public OptimizedPanel()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);
            UpdateStyles();
        }
    }

    public partial class Form1 : Form
    {
        private Bitmap backgroundBuffer;

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            this.Resize += Form1_Resize;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (backgroundBuffer != null)
            {
                backgroundBuffer.Dispose();
                backgroundBuffer = null;
            }
            drawingPanel.Invalidate();
        }

        private void CreateClockFace(int width, int height)
        {
            backgroundBuffer = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(backgroundBuffer))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.Black);

                int cx = width / 2;
                int cy = height / 2;
                int outerRadius = Math.Min(cx, cy) - 20;

                for (int i = 1; i <= 60; i++)
                {
                    double angle = i * 6;
                    double rad = (angle - 90) * Math.PI / 180;
                    int x = (int)(cx + outerRadius * Math.Cos(rad));
                    int y = (int)(cy + outerRadius * Math.Sin(rad));

                    if (i % 5 == 0)
                        g.FillEllipse(Brushes.White, x - 5, y - 5, 10, 10);
                    else
                        g.FillEllipse(Brushes.White, x - 2, y - 2, 4, 4);
                }

                g.FillRectangle(Brushes.White, cx - 5, cy - 5, 10, 10);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            drawingPanel.Invalidate();
        }

        private Point GetHandCoords(double angle, double length, int cx, int cy)
        {
            double rad = (angle - 90) * Math.PI / 180;
            int x = (int)(cx + length * Math.Cos(rad));
            int y = (int)(cy + length * Math.Sin(rad));
            return new Point(x, y);
        }

        private void drawingPanel_Paint(object sender, PaintEventArgs e)
        {
            if (backgroundBuffer == null || backgroundBuffer.Width != drawingPanel.Width || backgroundBuffer.Height != drawingPanel.Height)
            {
                CreateClockFace(drawingPanel.Width, drawingPanel.Height);
            }

            Graphics g = e.Graphics;

            g.DrawImage(backgroundBuffer, 0, 0);

            g.SmoothingMode = SmoothingMode.AntiAlias;

            int cx = drawingPanel.Width / 2;
            int cy = drawingPanel.Height / 2;
            int handRadius = Math.Min(cx, cy) - 10;
            DateTime now = DateTime.Now;

            Point secEnd = GetHandCoords(now.Second * 6, handRadius * 0.9, cx, cy);
            g.DrawLine(Pens.White, cx, cy, secEnd.X, secEnd.Y);

            double minAngle = now.Minute * 6 + now.Second * 0.1;
            Point minEnd = GetHandCoords(minAngle, handRadius * 0.7, cx, cy);
            g.DrawLine(new Pen(Color.White, 2), cx, cy, minEnd.X, minEnd.Y);

            double hourAngle = (now.Hour % 12) * 30 + now.Minute * 0.5;
            Point hourEnd = GetHandCoords(hourAngle, handRadius * 0.4, cx, cy);
            g.DrawLine(new Pen(Color.White, 3), cx, cy, hourEnd.X, hourEnd.Y);
        }
        private void comboBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            ComboBox comboBox = comboBox2;
            if (comboBox == null) return;
            if (comboBox.Text == string.Empty)
            {
                comboBox2.Text = "14";
            }
            if (char.IsControl(e.KeyChar))
                return;
            if (char.IsDigit(e.KeyChar))
                return;
            if (e.KeyChar == '.')
            {
                if (!comboBox.Text.Contains("."))
                    return;
                e.Handled = true;
                return;
            }
            e.Handled = true;

        }
    }
}