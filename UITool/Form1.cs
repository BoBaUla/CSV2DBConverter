using CSV2DBConverter;
using CSV2DBConverter.Adapter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UITool
{
    public partial class Form1 : Form
    {
        CSVReader _CSVReader;
        IFileReader _fileReader;
        public Form1()
        {
            InitializeComponent();
            _fileReader = new FileReader();
            _CSVReader = new CSVReader(tbCSVPath.Text, _fileReader);
        }

        private void btnCSVPath_Click(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            var result = fileDialog.ShowDialog();
            if (result == DialogResult.OK && fileDialog.FileNames.Length == 1)
            {
                tbCSVPath.Text = fileDialog.FileName;
            }
        }

        private void btnReadHead_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbCSVPath.Text))
                return;

            rtbOutput.Clear();
            var csvReader = new CSVReader(tbCSVPath.Text, _fileReader);
            csvReader.Analyze(int.Parse(tbTableLine.Text));
            PrintLines(csvReader.Head);
        }

        private void btnReadTable_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbCSVPath.Text))
                return;

            rtbOutput.Clear();
            var csvReader = new CSVReader(tbCSVPath.Text, _fileReader);
            csvReader.Analyze(int.Parse(tbTableLine.Text));
            PrintLines(csvReader.Table);
        }

        private void btnReadBody_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbCSVPath.Text))
                return;

            rtbOutput.Clear();
            var csvReader = new CSVReader(tbCSVPath.Text, _fileReader);
            csvReader.Analyze(int.Parse(tbTableLine.Text));
            PrintLines(csvReader.Body);
        }

        private void PrintLines(string[] lines)
        {
            foreach (string line in lines)
            {
                rtbOutput.AppendText(line + "\n");
            }
        }

    }
}
