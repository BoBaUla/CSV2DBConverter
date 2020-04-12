using CSV2DBConverter;
using CSV2DBConverter.Adapter;
using CSV2DBConverter.DBHandling;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
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

        BackgroundWorker _backgroundWorker;
        List<Task> _runningTasks;

        public ClickUI()
        {
            InitializeComponent();
            _fileReader = new FileReader();
            _CSVReader = new CSVReader(tbCSVPath.Text, _fileReader);

            _connectionString = new DBConncetionString(name, path);

            _runningTasks = new List<Task>();

            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.RunWorkerAsync();
            _backgroundWorker.DoWork += _backgroundWorker_DoWork;
            _backgroundWorker.RunWorkerCompleted += _backgroundWorker_RunWorkerCompleted;
        }

        private void _backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var restart = false;
            foreach(var task in _runningTasks)
            {
                btnCSV2DB.Enabled = task.IsCompleted;
                restart = !task.IsCompleted;
            }

            if (restart)
                _backgroundWorker.RunWorkerAsync();
            else
                _runningTasks = new List<Task>();
        }

        private void _backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(1000);
        }

        private void ClickUI_Load(object sender, EventArgs e)
        {

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

        private void btnInsertTable_Click(object sender, EventArgs e)
        {
            var tablePattern = new string[] { "col1", "col2" };
            var tableName = "testName";
            var dbTableCreator = new DBTableCreator(
                _connectionString, 
                tablePattern,
                tableName);

            var csvRow = new CSVRow();
            csvRow.Fill(tablePattern, new string[] { "Val1", "2"});
                
            dbTableCreator.Create();
            var dbInsertCommand = new InsertCommand(
                _connectionString,
                tableName,
                csvRow);
            dbInsertCommand.Insert();
            dbInsertCommand.Insert();
        }

        private void btnCSV2DB_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbCSVPath.Text) ||
                string.IsNullOrEmpty(tbTableLine.Text))
            {
                return;
            }

            CSVEntryParser csvEntryParser = CreateCSVEntryParser();

            csvEntryParser.Initialize();

            var tableName = "testName";
            var dbTableCreator = CreateDBTableCreator(csvEntryParser.TablePattern, tableName);

            dbTableCreator.Create();

            var fillDBTask = Task.Run(() => FillDB(csvEntryParser.TableEntries, tableName));

            _runningTasks.Add(fillDBTask);
            _backgroundWorker.RunWorkerAsync();
        }

        #region extracted methods
        private DBTableCreator CreateDBTableCreator(List<string> tablePattern, string tableName)
        {
            return new DBTableCreator(
                _connectionString,
                tablePattern,
                tableName);
        }

        private CSVReader CrearteCSVReader()
        {
            return new CSVReader(tbCSVPath.Text, _fileReader);
        }

        private CSVEntryParser CreateCSVEntryParser()
        {
            CSVReader csvReader = CrearteCSVReader();
            return new CSVEntryParser(
                tbCSVPath.Text,
                int.Parse(tbTableLine.Text),
                csvReader,
                new string[] { "Währung" });
        }

        private void FillDB(List<CSVRow> entries,  string tableName)
        {
            foreach (var csvRow in entries)
            {
                var dbInsertCommand = new InsertCommand(
                    _connectionString,
                    tableName,
                    csvRow);
                dbInsertCommand.Insert();
            }
        }
        #endregion
    }
}
