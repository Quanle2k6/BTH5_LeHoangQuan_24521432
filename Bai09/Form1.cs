using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Bai09
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
        private Color stroke = Color.Blue;
        private Color fill = Color.Red;
        private bool isDrawing = false;
        private Point Start, End;
        private Bitmap canva;

        public Form1()
        {
            InitializeComponent();
            SetupComboBox();
            this.Load += Form1_Load;
        }

        private void SetupComboBox()
        {
            comboBox1.Items.AddRange(new object[] {"Circle", "Square", "Eclipse", "Pie", "Fill Circle", "Fill Square", "Fill Eclipse", "Fill Pie"});
            comboBox1.SelectedIndex = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            canva = new Bitmap(panel1.Width, panel1.Height);
            using (Graphics g = Graphics.FromImage(canva))
            {
                g.Clear(Color.White);
            }
            panel1.BackgroundImage = canva;
            panel1.BackgroundImageLayout = ImageLayout.None;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Start = e.Location;
                End = e.Location;
                isDrawing = true;
            }
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                End = e.Location;
                panel1.Invalidate();
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                isDrawing = false;
                End = e.Location;
                using (Graphics g = Graphics.FromImage(canva))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    DrawCurrentShape(g);
                }
                panel1.Invalidate();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (isDrawing)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                DrawCurrentShape(e.Graphics);
            }
        }

        private void DrawCurrentShape(Graphics g)
        {
            if (comboBox1.SelectedItem == null) return;
            string selectedShape = comboBox1.SelectedItem.ToString();

            Rectangle rect;

            if (selectedShape.Contains("Circle") || selectedShape.Contains("Square"))
            {
                rect = GetSquareRectangle();
            }
            else
            {
                rect = GetNormalRectangle();
            }

            using (Pen pen = new Pen(stroke, 2))
            using (Brush brush = new SolidBrush(fill))
            {
                switch (selectedShape)
                {
                    case "Circle":
                    case "Eclipse":
                        g.DrawEllipse(pen, rect);
                        break;
                    case "Square":
                        g.DrawRectangle(pen, rect);
                        break;
                    case "Pie":
                        g.DrawPie(pen, rect, 0, 90);
                        break;
                    case "Fill Circle":
                    case "Fill Eclipse":
                        g.FillEllipse(brush, rect);
                        break;
                    case "Fill Square":
                        g.FillRectangle(brush, rect);
                        break;
                    case "Fill Pie":
                        g.FillPie(brush, rect, 0, 90);
                        break;
                }
            }
        }

        private Rectangle GetNormalRectangle()
        {
            return new Rectangle(
                Math.Min(Start.X, End.X),
                Math.Min(Start.Y, End.Y),
                Math.Max(1, Math.Abs(End.X - Start.X)),
                Math.Max(1, Math.Abs(End.Y - Start.Y))
            );
        }

        private Rectangle GetSquareRectangle()
        {
            int width = Math.Abs(End.X - Start.X);
            int height = Math.Abs(End.Y - Start.Y);
            int side = Math.Max(width, height);

            int x = Start.X;
            int y = Start.Y;

            if (End.X < Start.X) x = Start.X - side;
            if (End.Y < Start.Y) y = Start.Y - side;

            return new Rectangle(x, y, side, side);
        }
    }
}