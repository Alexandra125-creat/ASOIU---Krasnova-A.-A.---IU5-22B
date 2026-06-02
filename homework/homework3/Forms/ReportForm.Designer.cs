namespace Homework3.Forms
{
    partial class ReportForm
    {
        private System.ComponentModel.IContainer components = null;
        private TabControl tabControl;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private DataGridView dgvReport1;
        private DataGridView dgvReport2;
        private DataGridView dgvReport3;
        private Label lblReport1Count;
        private Label lblReport2Count;
        private Button btnClose;
        private Button btnRefresh;
        private Label lblTitle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tabControl = new TabControl();
            this.tabPage1 = new TabPage();
            this.dgvReport1 = new DataGridView();
            this.lblReport1Count = new Label();
            this.tabPage2 = new TabPage();
            this.dgvReport2 = new DataGridView();
            this.lblReport2Count = new Label();
            this.tabPage3 = new TabPage();
            this.dgvReport3 = new DataGridView();
            this.btnClose = new Button();
            this.btnRefresh = new Button();
            this.lblTitle = new Label();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport1)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport2)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport3)).BeginInit();
            this.SuspendLayout();

            // lblTitle - изменен текст (без эмодзи и LINQ)
            this.lblTitle.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.DarkBlue;
            this.lblTitle.Location = new Point(0, 15);
            this.lblTitle.Size = new Size(900, 35);
            this.lblTitle.Text = "Отчеты";
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // tabControl
            this.tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Controls.Add(this.tabPage3);
            this.tabControl.Location = new Point(12, 55);
            this.tabControl.Size = new Size(876, 400);
            this.tabControl.TabIndex = 0;

            // tabPage1
            this.tabPage1.Controls.Add(this.dgvReport1);
            this.tabPage1.Controls.Add(this.lblReport1Count);
            this.tabPage1.Text = "1. Полный список журналов";
            this.tabPage1.UseVisualStyleBackColor = true;

            // dgvReport1
            this.dgvReport1.AllowUserToAddRows = false;
            this.dgvReport1.AllowUserToDeleteRows = false;
            this.dgvReport1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReport1.Dock = DockStyle.Fill;
            this.dgvReport1.Location = new Point(0, 0);
            this.dgvReport1.ReadOnly = true;
            this.dgvReport1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvReport1.Size = new Size(876, 335);
            this.dgvReport1.TabIndex = 0;

            // lblReport1Count
            this.lblReport1Count.Dock = DockStyle.Bottom;
            this.lblReport1Count.Location = new Point(0, 335);
            this.lblReport1Count.Size = new Size(876, 25);
            this.lblReport1Count.TabIndex = 1;
            this.lblReport1Count.Text = "Всего записей: 0";

            // tabPage2
            this.tabPage2.Controls.Add(this.dgvReport2);
            this.tabPage2.Controls.Add(this.lblReport2Count);
            this.tabPage2.Text = "2. Количество по издательствам";
            this.tabPage2.UseVisualStyleBackColor = true;

            // dgvReport2
            this.dgvReport2.AllowUserToAddRows = false;
            this.dgvReport2.AllowUserToDeleteRows = false;
            this.dgvReport2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReport2.Dock = DockStyle.Fill;
            this.dgvReport2.Location = new Point(0, 0);
            this.dgvReport2.ReadOnly = true;
            this.dgvReport2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvReport2.Size = new Size(876, 335);
            this.dgvReport2.TabIndex = 0;

            // lblReport2Count
            this.lblReport2Count.Dock = DockStyle.Bottom;
            this.lblReport2Count.Location = new Point(0, 335);
            this.lblReport2Count.Size = new Size(876, 25);
            this.lblReport2Count.TabIndex = 1;
            this.lblReport2Count.Text = "Всего журналов: 0";

            // tabPage3
            this.tabPage3.Controls.Add(this.dgvReport3);
            this.tabPage3.Text = "3. Средний тираж по издательствам";
            this.tabPage3.UseVisualStyleBackColor = true;

            // dgvReport3
            this.dgvReport3.AllowUserToAddRows = false;
            this.dgvReport3.AllowUserToDeleteRows = false;
            this.dgvReport3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReport3.Dock = DockStyle.Fill;
            this.dgvReport3.Location = new Point(0, 0);
            this.dgvReport3.ReadOnly = true;
            this.dgvReport3.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvReport3.Size = new Size(876, 385);
            this.dgvReport3.TabIndex = 0;

            // btnRefresh
            this.btnRefresh.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.btnRefresh.Location = new Point(680, 470);
            this.btnRefresh.Size = new Size(100, 35);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "Обновить";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new EventHandler(this.btnRefresh_Click);

            // btnClose
            this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.btnClose.Location = new Point(788, 470);
            this.btnClose.Size = new Size(100, 35);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new EventHandler(this.btnClose_Click);

            // ReportForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(900, 520);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.lblTitle);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Отчеты";
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport2)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport3)).EndInit();
            this.ResumeLayout(false);
        }
    }
}