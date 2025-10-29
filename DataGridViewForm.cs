using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class DataGridViewForm : Form
    {
        public DataGridViewForm(string title, DataTable data)
        {
            InitializeComponent(title, data);
        }
        private void InitializeComponent(string title, DataTable data)
        {
            this.SuspendLayout();
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = title;
            this.BackColor = Color.White;

            Label lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.Font = new Font("Candara", 14, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(53, 92, 189);
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Height = 40;
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            DataGridView dataGridView = new DataGridView();
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.DataSource = data;
            dataGridView.ReadOnly = true;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.BackgroundColor = Color.White;

            // Стилизация
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(53, 92, 189);
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Candara", 10, FontStyle.Bold);
            dataGridView.EnableHeadersVisualStyles = false;

            this.Controls.Add(dataGridView);
            this.Controls.Add(lblTitle);
            this.ResumeLayout(false);
        }
    }
}
