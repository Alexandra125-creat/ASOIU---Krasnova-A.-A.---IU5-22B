using System;
using System.Linq;
using System.Windows.Forms;
using Homework3.Models;
using Microsoft.EntityFrameworkCore;

namespace Homework3.Forms
{
    public partial class JournalsForm : Form
    {
        private Journal? _selectedJournal;

        public JournalsForm()
        {
            InitializeComponent();
            LoadPublishers();
            LoadData();
        }

        private void LoadPublishers()
        {
            try
            {
                using var context = new AppDbContext();
                var publishers = context.Publishers.OrderBy(p => p.Name).ToList();
                cmbPublisher.DataSource = publishers;
                cmbPublisher.DisplayMember = "Name";
                cmbPublisher.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки издательств: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadData()
        {
            try
            {
                using var context = new AppDbContext();
                var journals = context.Journals
                    .Include(j => j.Publisher)
                    .OrderBy(j => j.Name)
                    .Select(j => new
                    {
                        j.Id,
                        j.Name,
                        PublisherName = j.Publisher != null ? j.Publisher.Name : "Не указано",
                        j.CirculationK,
                        j.PublisherId
                    })
                    .ToList();

                dgvJournals.DataSource = null;
                dgvJournals.DataSource = journals;

                // Скрываем колонку PublisherId
                if (dgvJournals.Columns["PublisherId"] != null)
                {
                    dgvJournals.Columns["PublisherId"].Visible = false;
                }

                lblCount.Text = $"Всего журналов: {journals.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvJournals_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvJournals.CurrentRow != null && dgvJournals.CurrentRow.DataBoundItem != null)
            {
                dynamic? row = dgvJournals.CurrentRow.DataBoundItem;
                if (row != null)
                {
                    int id = row.Id;
                    string name = row.Name;
                    int publisherId = row.PublisherId;
                    int circulation = row.CirculationK;

                    _selectedJournal = new Journal(id, publisherId, name, circulation);
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    txtName.Text = name;
                    cmbPublisher.SelectedValue = publisherId;
                    numCirculation.Value = circulation;
                }
            }
            else
            {
                ClearSelection();
            }
        }

        private void ClearSelection()
        {
            _selectedJournal = null;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            txtName.Clear();
            if (cmbPublisher.Items.Count > 0)
                cmbPublisher.SelectedIndex = 0;
            numCirculation.Value = 0;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Введите название журнала!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbPublisher.SelectedValue == null)
            {
                MessageBox.Show("Выберите издательство!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int publisherId = (int)cmbPublisher.SelectedValue;
            int circulation = (int)numCirculation.Value;

            if (circulation < 0)
            {
                MessageBox.Show("Тираж не может быть отрицательным!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var context = new AppDbContext();
                context.Journals.Add(new Journal(0, publisherId, name, circulation));
                context.SaveChanges();

                MessageBox.Show("Журнал добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearSelection();
                LoadData();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_selectedJournal == null)
            {
                MessageBox.Show("Выберите журнал!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string name = txtName.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Введите название журнала!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbPublisher.SelectedValue == null)
            {
                MessageBox.Show("Выберите издательство!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int publisherId = (int)cmbPublisher.SelectedValue;
            int circulation = (int)numCirculation.Value;

            if (circulation < 0)
            {
                MessageBox.Show("Тираж не может быть отрицательным!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var context = new AppDbContext();
                var journal = context.Journals.Find(_selectedJournal.Id);
                if (journal != null)
                {
                    journal.Name = name;
                    journal.PublisherId = publisherId;
                    journal.CirculationK = circulation;
                    context.SaveChanges();

                    MessageBox.Show("Журнал обновлён!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearSelection();
                    LoadData();
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedJournal == null)
            {
                MessageBox.Show("Выберите журнал!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show($"Удалить журнал \"{_selectedJournal.Name}\"?",
                "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using var context = new AppDbContext();
                    var journal = context.Journals.Find(_selectedJournal.Id);
                    if (journal != null)
                    {
                        context.Journals.Remove(journal);
                        context.SaveChanges();
                        MessageBox.Show("Журнал удалён!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearSelection();
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadPublishers();
            LoadData();
            ClearSelection();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}