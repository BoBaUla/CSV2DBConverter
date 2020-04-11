using CSV2DBConverter;
using CSV2DBConverter.Adapter;
using CSV2DBConverter.DBHandling;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UITool
{
    public partial class ClickUI : Form
    {
        CSVReader _CSVReader;
        IFileReader _fileReader;
        IDBConnectionString _connectionString;
        string path = Directory.GetCurrentDirectory();
        string name = "Test.db";

        public ClickUI()
        {
            InitializeComponent();
            _fileReader = new FileReader();
            _CSVReader = new CSVReader(tbCSVPath.Text, _fileReader);

            _connectionString = new DBConncetionString(name, path);

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

        private void CreateDB_Click(object sender, EventArgs e)
        {
            var dbCreator = new DBCreator(_connectionString);
            dbCreator.Create();
            var file = Path.Combine(path, name);
            var fileExists = File.Exists(file);

            rtbOutput.Clear();
            rtbOutput.Text = "DB created: " + fileExists.ToString();
            if (fileExists)
                File.Delete(file);

            rtbOutput.AppendText($"\nFile deleted: {!File.Exists(file) }");
        }

        private void btnCreateTable_Click(object sender, EventArgs e)
        {
            var dbTableCreator = new DBTableCreator(_connectionString, new string[]{"col1", "col2" }, "testName");
            dbTableCreator.Create();

        }
    }
}
