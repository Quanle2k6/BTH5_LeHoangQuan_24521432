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

namespace Bai06
{
    public partial class Form1 : Form
    {
        private InstalledFontCollection installFonts;
        public Form1()
        {
            InitializeComponent();
            installFonts = new InstalledFontCollection();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FontFamily[] fontFamilies = installFonts.Families;
            foreach (FontFamily font in fontFamilies)
            {
                richTextBox1.SelectionFont = new Font(font.Name, 12, FontStyle.Regular);
                richTextBox1.AppendText(font.Name + Environment.NewLine);
            }
        }
    }
}
