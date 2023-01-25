using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static Combainer.FrmOprations;

namespace Combainer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmOprations frmPdf = new FrmOprations(PdfTypes.Combaine);
            frmPdf.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmOprations frmPdf = new FrmOprations(PdfTypes.Split);
            frmPdf.ShowDialog();
        }
    }
}
