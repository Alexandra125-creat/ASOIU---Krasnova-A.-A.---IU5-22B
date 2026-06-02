using System;
using System.Linq;
using System.Windows.Forms;
using Homework3.Models;
using Microsoft.EntityFrameworkCore;

namespace Homework3.Forms
{
    public partial class ReportForm : Form
    {
        public ReportForm()
        {
            InitializeComponent();
            LoadReports();
        }

        private void LoadReports()
        {
            using var context = new AppDbContext();
            LoadReport1(context);
            LoadReport2(context);
            LoadReport3(context);
        }

        private void LoadReport1(AppDbContext context)
        {
            var report1 = context.Journals
                .Include(j => j.Publisher)
                .OrderBy(j => j.Name)
                .Select(j => new
                {
                    Название = j.Name,
                    Издательство = j.Publisher != null ? j.Publisher.Name : "Не указано",
                    Тираж_тыс_экз = j.CirculationK
                })
                .ToList();

            dgvReport1.DataSource = report1;
            lblReport1Count.Text = $"Всего записей: {report1.Count}";
        }

        private void LoadReport2(AppDbContext context)
        {
            var report2 = context.Journals
                .Include(j => j.Publisher)
                .GroupBy(j => j.Publisher != null ? j.Publisher.Name : "Не указано")
                .Select(g => new
                {
                    Издательство = g.Key,
                    Количество = g.Count()
                })
                .OrderBy(r => r.Издательство)
                .ToList();

            dgvReport2.DataSource = report2;
            int total = report2.Sum(r => r.Количество);
            lblReport2Count.Text = $"Всего журналов: {total}";
        }

        private void LoadReport3(AppDbContext context)
        {
            var report3 = context.Journals
                .Include(j => j.Publisher)
                .GroupBy(j => j.Publisher != null ? j.Publisher.Name : "Не указано")
                .Select(g => new
                {
                    Издательство = g.Key,
                    Средний_тираж = g.Average(j => j.CirculationK)
                })
                .OrderByDescending(r => r.Средний_тираж)
                .ToList();

            dgvReport3.DataSource = report3;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadReports();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}