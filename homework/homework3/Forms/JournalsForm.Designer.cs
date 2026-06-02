namespace Homework3.Forms
{
    partial class JournalsForm
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView dgvJournals;
        private TextBox txtName;
        private ComboBox cmbPublisher;
        private NumericUpDown numCirculation;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;
        private Button btnClose;
        private Label lblName;
        private Label lblPublisher;
        private Label lblCirculation;
        private Label lblCount;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvJournals = new DataGridView();
            this.txtName = new TextBox();
            this.cmbPublisher = new ComboBox();
            this.numCirculation = new NumericUpDown();
            this.btnAdd = new Button();
            this.btnEdit = new Button();
            this.btnDelete = new Button();
            this.btnRefresh = new Button();
            this.btnClose = new Button();
            this.lblName = new Label();
            this.lblPublisher = new Label();
            this.lblCirculation = new Label();
            this.lblCount = new Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvJournals)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCirculation)).BeginInit();
            this.SuspendLayout();

            this.dgvJournals.AllowUserToAddRows = false;
            this.dgvJournals.AllowUserToDeleteRows = false;
            this.dgvJournals.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.dgvJournals.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvJournals.Location = new Point(12, 60);
            this.dgvJournals.MultiSelect = false;
            this.dgvJournals.ReadOnly = true;
            this.dgvJournals.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvJournals.Size = new Size(550, 350);
            this.dgvJournals.SelectionChanged += new EventHandler(this.dgvJournals_SelectionChanged);

            this.lblCount.AutoSize = true;
            this.lblCount.Location = new Point(15, 420);
            this.lblCount.Text = "Всего журналов: 0";

            this.lblName.Location = new Point(580, 70);
            this.lblName.Size = new Size(120, 25);
            this.lblName.Text = "Название журнала:";

            this.txtName.Location = new Point(580, 95);
            this.txtName.Size = new Size(220, 23);

            this.lblPublisher.Location = new Point(580, 130);
            this.lblPublisher.Size = new Size(100, 25);
            this.lblPublisher.Text = "Издательство:";

            this.cmbPublisher.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbPublisher.Location = new Point(580, 155);
            this.cmbPublisher.Size = new Size(220, 23);

            this.lblCirculation.Location = new Point(580, 190);
            this.lblCirculation.Size = new Size(150, 25);
            this.lblCirculation.Text = "Тираж (тыс. экз.):";

            this.numCirculation.Location = new Point(580, 215);
            this.numCirculation.Maximum = 10000;
            this.numCirculation.Size = new Size(120, 23);

            this.btnAdd.Location = new Point(580, 255);
            this.btnAdd.Size = new Size(220, 35);
            this.btnAdd.Text = "Добавить";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);

            this.btnEdit.Enabled = false;
            this.btnEdit.Location = new Point(580, 300);
            this.btnEdit.Size = new Size(220, 35);
            this.btnEdit.Text = "Редактировать";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new EventHandler(this.btnEdit_Click);

            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new Point(580, 345);
            this.btnDelete.Size = new Size(220, 35);
            this.btnDelete.Text = "Удалить";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);

            this.btnRefresh.Location = new Point(580, 400);
            this.btnRefresh.Size = new Size(105, 35);
            this.btnRefresh.Text = " Обновить";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new EventHandler(this.btnRefresh_Click);

            this.btnClose.Location = new Point(695, 400);
            this.btnClose.Size = new Size(105, 35);
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new EventHandler(this.btnClose_Click);

            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(834, 461);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.numCirculation);
            this.Controls.Add(this.lblCirculation);
            this.Controls.Add(this.cmbPublisher);
            this.Controls.Add(this.lblPublisher);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.dgvJournals);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "📖 Управление журналами";
            ((System.ComponentModel.ISupportInitialize)(this.dgvJournals)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCirculation)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}