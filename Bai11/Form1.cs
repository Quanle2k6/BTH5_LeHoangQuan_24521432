using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Bai11
{
    public partial class Form1 : Form
    {
        private int iSelectedShapes = 1;
        private int iPenWidth = 1;
        private Color colorShape = Color.Black;
        private int iSelectBrush = 1;
        private Point Start;
        private Point End;
        private bool isDrawing = false;
        private Bitmap canva;

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            canva = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics g = Graphics.FromImage(canva))
            {
                g.Clear(Color.White);
            }
            pictureBox1.Image = canva;
        }

        private void ApplyShapeSettings()
        {
            if (iSelectedShapes == 2 || iSelectedShapes == 3)
            {
                groupBox2.Enabled = false;
                groupBox3.Enabled = true;
            }
            else
            {
                groupBox2.Enabled = true;
                groupBox3.Enabled = false;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e) { iSelectedShapes = 1; ApplyShapeSettings(); }
        private void radioButton2_CheckedChanged(object sender, EventArgs e) { iSelectedShapes = 2; ApplyShapeSettings(); }
        private void radioButton3_CheckedChanged(object sender, EventArgs e) { iSelectedShapes = 3; ApplyShapeSettings(); }

        private void button1_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                colorShape = colorDialog.Color;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e) { iSelectBrush = 1; }
        private void radioButton5_CheckedChanged(object sender, EventArgs e) { iSelectBrush = 2; }
        private void radioButton6_CheckedChanged(object sender, EventArgs e) { iSelectBrush = 3; }
        private void radioButton7_CheckedChanged(object sender, EventArgs e) { iSelectBrush = 4; }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox1.Text, out iPenWidth)) iPenWidth = 1;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Start = e.Location;
                End = e.Location;
                isDrawing = true;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                End = e.Location;
                pictureBox1.Invalidate();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                End = e.Location;
                isDrawing = false;
                using (Graphics g = Graphics.FromImage(canva))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    DrawShape(g);
                }
                pictureBox1.Invalidate();
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (isDrawing)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                DrawShape(e.Graphics);
            }
        }

        private void DrawShape(Graphics g)
        {
            if (iSelectedShapes == 1)
            {
                using (Pen pen = new Pen(colorShape, iPenWidth))
                {
                    g.DrawLine(pen, Start, End);
                }
            }
            else
            {
                Rectangle rect = new Rectangle(
                    Math.Min(Start.X, End.X),
                    Math.Min(Start.Y, End.Y),
                    Math.Max(1, Math.Abs(End.X - Start.X)),
                    Math.Max(1, Math.Abs(End.Y - Start.Y))
                );

                using (Brush brush = GetBrush(rect))
                {
                    if (iSelectedShapes == 2) g.FillRectangle(brush, rect);
                    else if (iSelectedShapes == 3) g.FillEllipse(brush, rect);
                }
            }
        }

        private Brush GetBrush(Rectangle rect)
        {
            switch (iSelectBrush)
            {
                case 2:
                    return new HatchBrush(HatchStyle.Horizontal, Color.Green, Color.Blue);
                case 3:
                    Image resImg = Properties.Resources.texture1;
                    if (resImg != null)
                    {
                        TextureBrush tb = new TextureBrush(resImg);
                        tb.TranslateTransform(rect.X, rect.Y);
                        return tb;
                    }
                    return new SolidBrush(colorShape);
                case 4:
                    return new LinearGradientBrush(rect, Color.Red, Color.Green, LinearGradientMode.Vertical);
                default:
                    return new SolidBrush(Color.Green);
            }
        }
    }
}