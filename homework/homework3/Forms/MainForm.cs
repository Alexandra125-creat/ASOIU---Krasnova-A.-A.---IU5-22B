using System;
using System.Windows.Forms;

namespace Homework3.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnPublishers_Click(object sender, EventArgs e)
        {
            var form = new PublishersForm();
            form.ShowDialog();
        }

        private void btnJournals_Click(object sender, EventArgs e)
        {
            var form = new JournalsForm();
            form.ShowDialog();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            var form = new ReportForm();
            form.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}