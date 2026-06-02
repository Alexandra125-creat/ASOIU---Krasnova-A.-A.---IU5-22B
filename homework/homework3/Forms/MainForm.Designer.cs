namespace Homework3.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private Button btnPublishers;
        private Button btnJournals;
        private Button btnReport;
        private Button btnExit;
        private Label lblTitle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnPublishers = new Button();
            this.btnJournals = new Button();
            this.btnReport = new Button();
            this.btnExit = new Button();
            this.lblTitle = new Label();
            this.SuspendLayout();

            // lblTitle - исправлен размер и текст
            this.lblTitle.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.DarkBlue;
            this.lblTitle.Location = new Point(0, 20);
            this.lblTitle.Size = new Size(600, 50);
            this.lblTitle.Text = "Управление издательствами и журналами";
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            this.btnPublishers.Font = new Font("Microsoft Sans Serif", 12F);
            this.btnPublishers.Location = new Point(150, 100);
            this.btnPublishers.Size = new Size(300, 45);
            this.btnPublishers.Text = "📚 Издательства (справочник)";
            this.btnPublishers.UseVisualStyleBackColor = true;
            this.btnPublishers.Click += new EventHandler(this.btnPublishers_Click);

            this.btnJournals.Font = new Font("Microsoft Sans Serif", 12F);
            this.btnJournals.Location = new Point(150, 160);
            this.btnJournals.Size = new Size(300, 45);
            this.btnJournals.Text = "📖 Журналы (основная таблица)";
            this.btnJournals.UseVisualStyleBackColor = true;
            this.btnJournals.Click += new EventHandler(this.btnJournals_Click);

            this.btnReport.Font = new Font("Microsoft Sans Serif", 12F);
            this.btnReport.Location = new Point(150, 220);
            this.btnReport.Size = new Size(300, 45);
            this.btnReport.Text = "📊 Отчёты";
            this.btnReport.UseVisualStyleBackColor = true;
            this.btnReport.Click += new EventHandler(this.btnReport_Click);

            this.btnExit.Font = new Font("Microsoft Sans Serif", 12F);
            this.btnExit.Location = new Point(150, 290);
            this.btnExit.Size = new Size(300, 45);
            this.btnExit.Text = "🚪 Выход";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new EventHandler(this.btnExit_Click);

            // MainForm - увеличен размер окна
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(600, 400);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.btnJournals);
            this.Controls.Add(this.btnPublishers);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Домашнее задание №3";
            this.ResumeLayout(false);
        }
    }
}