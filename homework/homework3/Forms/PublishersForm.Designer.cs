namespace Homework3.Forms
{
    partial class PublishersForm
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView dgvPublishers;
        private TextBox txtName;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;
        private Button btnClose;
        private Label lblName;
        private Label lblCount;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            dgvPublishers = new DataGridView();
            txtName = new TextBox();
            btnAdd = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            btnRefresh = new Button();
            btnClose = new Button();
            lblName = new Label();
            lblCount = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvPublishers).BeginInit();
            SuspendLayout();

            // dgvPublishers
            dgvPublishers.AllowUserToAddRows = false;
            dgvPublishers.AllowUserToDeleteRows = false;
            dgvPublishers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvPublishers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPublishers.Location = new Point(12, 60);
            dgvPublishers.MultiSelect = false;
            dgvPublishers.Name = "dgvPublishers";
            dgvPublishers.ReadOnly = true;
            dgvPublishers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPublishers.Size = new Size(450, 350);
            dgvPublishers.TabIndex = 0;
            dgvPublishers.SelectionChanged += dgvPublishers_SelectionChanged;

            // lblCount - внизу формы
            lblCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblCount.AutoSize = true;
            lblCount.Location = new Point(15, 420);
            lblCount.Name = "lblCount";
            lblCount.Size = new Size(100, 15);
            lblCount.TabIndex = 1;
            lblCount.Text = "Всего издательств: 0";

            // lblName
            lblName.AutoSize = true;
            lblName.Location = new Point(480, 80);
            lblName.Name = "lblName";
            lblName.Size = new Size(65, 15);
            lblName.Text = "Название:";

            // txtName
            txtName.Location = new Point(480, 110);
            txtName.Name = "txtName";
            txtName.Size = new Size(220, 23);

            // btnAdd
            btnAdd.Location = new Point(480, 150);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(220, 35);
            btnAdd.Text = "➕ Добавить";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;

            // btnEdit
            btnEdit.Enabled = false;
            btnEdit.Location = new Point(480, 195);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(220, 35);
            btnEdit.Text = "✏️ Редактировать";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += btnEdit_Click;

            // btnDelete
            btnDelete.Enabled = false;
            btnDelete.Location = new Point(480, 240);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(220, 35);
            btnDelete.Text = "🗑️ Удалить";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;

            // btnRefresh
            btnRefresh.Location = new Point(480, 300);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(220, 35);
            btnRefresh.Text = "🔄 Обновить";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;

            // btnClose
            btnClose.Location = new Point(480, 345);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(220, 35);
            btnClose.Text = "❌ Закрыть";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;

            // PublishersForm
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(734, 461);
            Controls.Add(btnClose);
            Controls.Add(btnRefresh);
            Controls.Add(btnDelete);
            Controls.Add(btnEdit);
            Controls.Add(btnAdd);
            Controls.Add(txtName);
            Controls.Add(lblName);
            Controls.Add(lblCount);
            Controls.Add(dgvPublishers);
            Name = "PublishersForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "📚 Управление издательствами";
            ((System.ComponentModel.ISupportInitialize)dgvPublishers).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}