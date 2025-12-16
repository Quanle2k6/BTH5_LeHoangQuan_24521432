using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Bai10
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

    public class LineSegment
    {
        public Point Start { get; set; }
        public Point End { get; set; }
        public DashStyle DashStyle { get; set; }
        public LineJoin LineJoin { get; set; }
        public LineCap StartCap { get; set; }
        public LineCap EndCap { get; set; }
        public DashCap DashCap { get; set; }
        public float Width { get; set; }
    }

    public partial class Form1 : Form
    {
        private List<LineSegment> drawnLines = new List<LineSegment>();
        private Point startPoint;
        private Point endPoint;
        private bool isDrawing = false;

        public Form1()
        {
            InitializeComponent();
            InitializeControl();
        }

        private void InitializeControl()
        {
            comboBox1.DataSource = Enum.GetValues(typeof(DashStyle));
            comboBox1.SelectedItem = DashStyle.Solid;
            comboBox1.SelectedIndexChanged += Control_Changed;

            comboBox3.DataSource = Enum.GetValues(typeof(LineJoin));
            comboBox3.SelectedItem = LineJoin.Round;
            comboBox3.SelectedIndexChanged += Control_Changed;

            comboBox4.DataSource = Enum.GetValues(typeof(DashCap));
            comboBox4.SelectedItem = DashCap.Flat;
            comboBox4.SelectedIndexChanged += Control_Changed;

            var capValues = new LineCap[] { LineCap.Flat, LineCap.Square, LineCap.Round, LineCap.ArrowAnchor, LineCap.DiamondAnchor, LineCap.RoundAnchor, LineCap.Triangle };

            comboBox5.DataSource = capValues.Clone();
            comboBox5.SelectedItem = LineCap.Round;
            comboBox5.SelectedIndexChanged += Control_Changed;

            comboBox6.DataSource = capValues.Clone();
            comboBox6.SelectedItem = LineCap.Flat;
            comboBox6.SelectedIndexChanged += Control_Changed;

            for (int i = 1; i <= 30; i++)
            {
                comboBox2.Items.Add(i);
            }
            comboBox2.SelectedItem = 9;
            comboBox2.SelectedIndexChanged += Control_Changed;

            panel1.MouseDown += panel1_MouseDown;
            panel1.MouseMove += panel1_MouseMove;
            panel1.MouseUp += panel1_MouseUp;
        }

        private void Control_Changed(object sender, EventArgs e)
        {
            panel1.Invalidate();
        }

        private Pen GetCurrentDemoPen(Color color)
        {
            float width = 1f;
            if (comboBox2.SelectedItem != null && float.TryParse(comboBox2.SelectedItem.ToString(), out float parsedWidth))
            {
                width = parsedWidth;
            }

            Pen demoPen = new Pen(color, width);

            demoPen.DashStyle = (DashStyle)comboBox1.SelectedItem;
            demoPen.DashCap = (DashCap)comboBox4.SelectedItem;
            demoPen.LineJoin = (LineJoin)comboBox3.SelectedItem;
            demoPen.StartCap = (LineCap)comboBox5.SelectedItem;
            demoPen.EndCap = (LineCap)comboBox6.SelectedItem;

            return demoPen;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            foreach (var line in drawnLines)
            {
                using (Pen savedPen = new Pen(Color.Gray, line.Width))
                {
                    savedPen.DashStyle = line.DashStyle;
                    savedPen.LineJoin = line.LineJoin;
                    savedPen.DashCap = line.DashCap;
                    savedPen.StartCap = line.StartCap;
                    savedPen.EndCap = line.EndCap;

                    g.DrawLine(savedPen, line.Start, line.End);
                }
            }

            if (isDrawing)
            {
                using (Pen interactivePen = GetCurrentDemoPen(Color.Black))
                {
                    g.DrawLine(interactivePen, startPoint, endPoint);
                }
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                startPoint = e.Location;
                endPoint = e.Location;
                isDrawing = true;
            }
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                endPoint = e.Location;
                panel1.Invalidate();
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                var currentPen = GetCurrentDemoPen(Color.Black);

                drawnLines.Add(new LineSegment
                {
                    Start = startPoint,
                    End = endPoint,
                    DashStyle = currentPen.DashStyle,
                    LineJoin = currentPen.LineJoin,
                    DashCap = currentPen.DashCap,
                    StartCap = currentPen.StartCap,
                    EndCap = currentPen.EndCap,
                    Width = currentPen.Width
                });

                isDrawing = false;
                panel1.Invalidate();
            }
        }
    }
}