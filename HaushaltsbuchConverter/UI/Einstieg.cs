using HaushaltsbuchConverter.Logic.DBInitialisation;
using System;
using System.IO;
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
            if (File.Exists(Path.Combine(_cwd, _dbName)))
                return;

            DisableSender(sender);
            btnInitializeLogic();
            EnableSender(sender);
        }

        private void btnInitializeLogic()
        {
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

        private static void EnableSender(object sender)
        {
            ((Control)sender).Enabled = true;
        }

        private static void DisableSender(object sender)
        {
            ((Control)sender).Enabled = false;
        }
    }
}
