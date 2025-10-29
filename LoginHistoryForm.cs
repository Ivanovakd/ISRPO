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
    public partial class LoginHistoryForm : Form
    {
        private DatabaseHelper dbHelper;
        private string currentSort = "Время_входа";
        private bool sortAscending = false;

        public LoginHistoryForm()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            LoadLoginHistory();
        }

        private void LoadLoginHistory()
        {
            try
            {
                var history = dbHelper.GetLoginHistory(currentSort, sortAscending);

                DataTable table = new DataTable();
                table.Columns.Add("Время входа", typeof(string));
                table.Columns.Add("Логин", typeof(string));
                table.Columns.Add("Статус", typeof(string));

                foreach (var record in history)
                {
                    table.Rows.Add(
                        record.LoginTime.ToString("dd.MM.yyyy HH:mm:ss"),
                        record.Login,
                        record.Status
                    );
                }

                dataGridView.DataSource = table;
                ConfigureDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки истории: {ex.Message}");
            }
        }

        private void ConfigureDataGridView()
        {
            if (dataGridView == null) return;

            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(53, 92, 189);
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Candara", 10, FontStyle.Bold);
            dataGridView.EnableHeadersVisualStyles = false;

            // Безопасная подписка на событие
            dataGridView.RowPrePaint -= DataGridView_RowPrePaint; // Отписываемся сначала
            dataGridView.RowPrePaint += DataGridView_RowPrePaint;
        }

        private void DataGridView_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dataGridView.Rows.Count) return;

            var row = dataGridView.Rows[e.RowIndex];
            if (row.IsNewRow) return; // Пропускаем новую строку

            // Безопасная проверка ячеек
            if (row.Cells.Count > 2 && row.Cells[2].Value != null)
            {
                var status = row.Cells[2].Value.ToString();
                row.DefaultCellStyle.BackColor = status == "Успешно" ? Color.LightGreen : Color.LightPink;
            }
        }

        private void cmbSortBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSortBy.SelectedIndex == 0)
                currentSort = "Время_входа";
            else
                currentSort = "Логин";

            LoadLoginHistory();
        }

        private void btnSortDirection_Click(object sender, EventArgs e)
        {
            sortAscending = !sortAscending;
            btnSortDirection.Text = sortAscending ? "↑" : "↓";
            LoadLoginHistory();
        }
    }
}
