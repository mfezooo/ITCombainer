using PdfSharp.Pdf;
using PdfSharp.Pdf.IO; 
using System.Xml.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static Combainer.FrmOprations;

namespace Combainer
{
    public partial class FrmOprations : Form
    {
       

        public FrmOprations(PdfTypes pdfTypes)
        {
            InitializeComponent();
            _pdfType = pdfTypes;
        }
        List<string> sFilePathes = new List<string>();
        OpenFileDialog openFileDialog1;
        private readonly PdfTypes _pdfType;
        public void MoveUp()
        {
            MoveItem(-1);
        }
        public void MoveDown()
        {
            MoveItem(1);
        }
        public void MoveItem(int direction)
        {
            // Checking selected item
            if (listBox1.SelectedItem == null || listBox1.SelectedIndex < 0)
                return; // No selected item - nothing to do

            // Calculate new index using move direction
            int newIndex = listBox1.SelectedIndex + direction;

            // Checking bounds of the range
            if (newIndex < 0 || newIndex >= listBox1.Items.Count)
                return; // Index out of range - nothing to do

            object selected = listBox1.SelectedItem;

            // Removing removable element
            listBox1.Items.Remove(selected);
            // Insert it in new position
            listBox1.Items.Insert(newIndex, selected);
            // Restore selection
            listBox1.SetSelected(newIndex, true);
        }
        public enum PdfTypes
        {
            Combaine,
            Split,
            Compress
        }
        private void btnCombine_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count < 1)
            { MessageBox.Show("Please add pdf files first"); return; }


            if (_pdfType == PdfTypes.Split)
            {
                string fNameSplited = "splited\\";
                SplitPDF(fNameSplited, listBox1.Items.Cast<String>().ToList());
            }
            if (_pdfType == PdfTypes.Combaine)
            {
                string fName = "combined\\";
                fName += "combined-" + Guid.NewGuid();
                MergePDFs(fName + ".pdf", listBox1.Items.Cast<String>().ToList());
            }
            if (_pdfType == PdfTypes.Compress)
            {
                string fName = "compress\\";
                //CompressPDFs(fName, listBox1.Items.Cast<String>().ToList());
            }
        }
        public static void MergePDFs(string targetPath, List<string> pdfs)
        {
            //System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var targetDoc = new PdfDocument())
            {
                foreach (var pdf in pdfs)
                {
                    using (var pdfDoc = PdfReader.Open(pdf, PdfDocumentOpenMode.Import))
                    {
                        for (var i = 0; i < pdfDoc.PageCount; i++)
                            targetDoc.AddPage(pdfDoc.Pages[i]);
                    }
                }
                targetDoc.Save(targetPath);
            }
            MessageBox.Show("Done");
        }

        public static void SplitPDF(string targetPath, List<string> pdfs)
        {
            string cfName;
            //System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            //PdfDocument targetDoc = new PdfDocument() tst;

            foreach (var pdf in pdfs)
            {
                using (var pdfDoc = PdfReader.Open(pdf, PdfDocumentOpenMode.Import))
                {
                    for (var i = 0; i < pdfDoc.PageCount; i++)
                    {
                        PdfDocument targetDoc = new PdfDocument();
                        targetDoc.AddPage(pdfDoc.Pages[i]);
                        cfName = targetPath + Path.GetFileNameWithoutExtension(pdf) + i + ".pdf";
                        targetDoc.Save(cfName);
                    }
                }
            }
            //targetDoc.Save(targetPath);

            MessageBox.Show("Done");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;
            openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = @"D:\";
            openFileDialog1.Filter = "PDF files (*.pdf)|*.pdf";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                listBox1.Items.AddRange(openFileDialog1.FileNames);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MoveUp();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MoveDown();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Please select file name first");
                return;
            }
            listBox1.Items.Remove(listBox1.SelectedItem);
        }

        private void FrmOprations_Load(object sender, EventArgs e)
        {
            if (_pdfType == PdfTypes.Split)
                btnCombine.Text = "Split";
            if (_pdfType == PdfTypes.Compress)
                btnCombine.Text = "Compress";
            if (_pdfType == PdfTypes.Compress)
                btnCombine.Text = "Compress";
        }
    }
}
