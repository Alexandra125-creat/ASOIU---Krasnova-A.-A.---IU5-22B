using System;
using System.Linq;
using System.Windows.Forms;
using Homework3.Models;
using Microsoft.EntityFrameworkCore;

namespace Homework3.Forms
{
    public partial class PublishersForm : Form
    {
        private Publisher? _selectedPublisher;

        public PublishersForm()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                using var context = new AppDbContext();
                // Сортировка по названию (а не по ID)
                var publishers = context.Publishers
                    .OrderBy(p => p.Name)
                    .ToList();

                // Привязка данных
                dgvPublishers.DataSource = null;
                dgvPublishers.DataSource = publishers;

                // Скрываем колонку Journals (навигационное свойство)
                if (dgvPublishers.Columns["Journals"] != null)
                {
                    dgvPublishers.Columns["Journals"].Visible = false;
                }

                // Обновляем счетчик
                lblCount.Text = $"Всего издательств: {publishers.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvPublishers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPublishers.CurrentRow != null && dgvPublishers.CurrentRow.DataBoundItem != null)
            {
                _selectedPublisher = dgvPublishers.CurrentRow.DataBoundItem as Publisher;
                if (_selectedPublisher != null)
                {
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    txtName.Text = _selectedPublisher.Name;
                }
            }
            else
            {
                _selectedPublisher = null;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                txtName.Clear();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Введите название!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var context = new AppDbContext();
                context.Publishers.Add(new Publisher { Name = name });
                context.SaveChanges();
                MessageBox.Show("Добавлено!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Clear();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_selectedPublisher == null)
            {
                MessageBox.Show("Выберите издательство!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string newName = txtName.Text.Trim();

            if (string.IsNullOrWhiteSpace(newName))
            {
                MessageBox.Show("Введите название!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var context = new AppDbContext();
                var publisher = context.Publishers.Find(_selectedPublisher.Id);
                if (publisher != null)
                {
                    publisher.Name = newName;
                    context.SaveChanges();
                    MessageBox.Show("Обновлено!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtName.Clear();
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedPublisher == null)
            {
                MessageBox.Show("Выберите издательство!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var context = new AppDbContext();
                var publisher = context.Publishers
                    .Include(p => p.Journals)
                    .FirstOrDefault(p => p.Id == _selectedPublisher.Id);

                if (publisher == null) return;

                if (publisher.Journals.Any())
                {
                    MessageBox.Show($"Нельзя удалить \"{publisher.Name}\"! Связано {publisher.Journals.Count} журналов.",
                        "Запрет удаления", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show($"Удалить \"{publisher.Name}\"?", "Подтверждение",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    context.Publishers.Remove(publisher);
                    context.SaveChanges();
                    MessageBox.Show("Удалено!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtName.Clear();
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
            txtName.Clear();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}