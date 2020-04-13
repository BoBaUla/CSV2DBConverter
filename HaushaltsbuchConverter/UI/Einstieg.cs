using CSV2DBConverter.DBHandling;
using HaushaltsbuchConverter.Logic.DBInitialisation;
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

namespace HaushaltsbuchConverter.UI
{
    public partial class Einstieg : Form
    {
        private string _cwd = Directory.GetCurrentDirectory();
        private string _dbName = "Haushaltsbuch.db";

        public Einstieg()
        {
            InitializeComponent();
        }

        private void btnInitialize_Click(object sender, EventArgs e)
        {
            if(File.Exists(Path.Combine(_cwd, _dbName)))
                return;

            var dbInitilsator = new DBInitialisator(_dbName, _cwd);
            dbInitilsator.Initialise();

            var fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == DialogResult.OK &&
                fileDialog.FileNames.Length == 1 &&
                fileDialog.FileName.ToLower().EndsWith(".csv"))
            {
                dbInitilsator.CreateTables(fileDialog.FileName);
            }
        }
    }
}
