using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bai04
{
    public partial class Form1 : Form
    {
        private InstalledFontCollection installFonts;
        public Font CustomFont;
        private bool isBold = false;
        private bool isItalic = false;
        private bool isUnderlined = false;
        private float[] SelectedSize = { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
        private float fSize = 14;
        private string FilePath = string.Empty;
        private string FamilyFont = "Arial";
        private string sString = "Hello";
        public Form1()
        {
            InitializeComponent();
            installFonts = new InstalledFontCollection();
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            FamilyFont = comboBox1.Text;
            ApplyFontStyle();
        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text == string.Empty)
                comboBox2.Text = "14";

            fSize = float.Parse(comboBox2.Text);
            ApplyFontStyle();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.ForeColor = colorDialog.Color;
            }
            button1.BackColor = colorDialog.Color;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            isBold = !isBold;
            ApplyFontStyle();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            isItalic = !isItalic;
            ApplyFontStyle();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            isUnderlined = !isUnderlined;
            ApplyFontStyle();

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
            richTextBox1.SelectionAlignment = HorizontalAlignment.Left;
            richTextBox1.DeselectAll();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox1.DeselectAll();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
            richTextBox1.SelectionAlignment = HorizontalAlignment.Right;
            richTextBox1.DeselectAll();
        }

        private void ApplyFontStyle()
        {
            FontStyle style = FontStyle.Regular;
            if (isBold)
                style |= FontStyle.Bold;
            if (isItalic)
                style |= FontStyle.Italic;
            if (isUnderlined)
                style |= FontStyle.Underline;
            CustomFont = new Font(FamilyFont, fSize, style);
            richTextBox1.Font = CustomFont;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CustomFont = new Font(FamilyFont, fSize, FontStyle.Regular);
            richTextBox1.Text = sString;
            FontFamily[] fontFamilies = installFonts.Families;
            foreach (FontFamily font in fontFamilies)
            {
                comboBox1.Items.Add(font.Name);
            }
            foreach (float size in SelectedSize)
            {
                comboBox2.Items.Add(size.ToString());
            }
            comboBox1.Text = FamilyFont;
            comboBox2.Text = fSize.ToString();
            richTextBox1.SelectAll();
            richTextBox1.Font = CustomFont;
            richTextBox1.DeselectAll();

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
