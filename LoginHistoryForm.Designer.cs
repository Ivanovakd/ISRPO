using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    partial class LoginHistoryForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "История входов в систему";
            this.BackColor = Color.White;

            // Панель управления
            var controlPanel = new Panel();
            controlPanel.Dock = DockStyle.Top;
            controlPanel.Height = 50;
            controlPanel.BackColor = Color.FromArgb(210, 223, 255);

            var lblSort = new Label { Text = "Сортировка:", Location = new Point(20, 15), Size = new Size(70, 20) };

            cmbSortBy = new ComboBox();
            cmbSortBy.Location = new Point(100, 12);
            cmbSortBy.Size = new Size(120, 25);
            cmbSortBy.Items.AddRange(new string[] { "По дате", "По логину" });
            cmbSortBy.SelectedIndex = 0;
            cmbSortBy.SelectedIndexChanged += cmbSortBy_SelectedIndexChanged;

            btnSortDirection = new Button();
            btnSortDirection.Text = "↓";
            btnSortDirection.Location = new Point(230, 12);
            btnSortDirection.Size = new Size(30, 25);
            btnSortDirection.BackColor = Color.FromArgb(53, 92, 189);
            btnSortDirection.ForeColor = Color.White;
            btnSortDirection.FlatStyle = FlatStyle.Flat;
            btnSortDirection.Click += btnSortDirection_Click;

            controlPanel.Controls.AddRange(new Control[] { lblSort, cmbSortBy, btnSortDirection });

            // DataGridView
            dataGridView = new DataGridView();
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.ReadOnly = true;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            this.Controls.Add(dataGridView);
            this.Controls.Add(controlPanel);
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridView dataGridView;
        private ComboBox cmbSortBy;
        private Button btnSortDirection;
    }
}