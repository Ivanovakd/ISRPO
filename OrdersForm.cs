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
    public partial class OrdersForm : Form
    {
        private DatabaseHelper dbHelper;
        private User currentUser;
        private bool isClientView;
        public OrdersForm(User user)
        {
            InitializeComponent();
            this.dbHelper = new DatabaseHelper();
            this.currentUser = user;
            this.isClientView = user.IsClient;
            LoadOrders();
            SetupPermissions();
        }
        private void SetupPermissions()
        {
            btnCreateOrder.Visible = isClientView;
            btnCancelOrder.Visible = isClientView;
        }

        private void LoadOrders()
        {
            try
            {
                var orders = isClientView ?
                    dbHelper.GetClientOrders(currentUser.Login) :
                    dbHelper.GetAllOrders();

                dataGridView.DataSource = orders.Select(o => new
                {
                    Номер = o.OrderNumber,
                    Товар = o.ProductName,
                    Артикул = o.ProductArticle,
                    Статус = o.StatusDisplay,
                    Дата_создания = o.CreateDate.ToString("dd.MM.yyyy HH:mm"),
                    Клиент = o.ClientLogin
                }).ToList();

                ConfigureDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки заявок: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureDataGridView()
        {
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(53, 92, 189);
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Candara", 10, FontStyle.Bold);
            dataGridView.EnableHeadersVisualStyles = false;
        }

        private void btnCreateOrder_Click(object sender, EventArgs e)
        {
            var createForm = new CreateOrderForm(currentUser);
            if (createForm.ShowDialog() == DialogResult.OK)
            {
                LoadOrders();
            }
        }

        private void btnCancelOrder_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите заявку для отмены", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectedRow = dataGridView.SelectedRows[0];
            string orderNumber = selectedRow.Cells["Номер"].Value.ToString();
            string status = selectedRow.Cells["Статус"].Value.ToString();

            if (status != "Создана" && status != "На согласовании")
            {
                MessageBox.Show("Можно отменять только заявки со статусом 'Создана' или 'На согласовании'", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var result = MessageBox.Show($"Отменить заявку {orderNumber}?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Находим ID заявки
                var orders = dbHelper.GetClientOrders(currentUser.Login);
                var order = orders.FirstOrDefault(o => o.OrderNumber == orderNumber);

                if (order != null && dbHelper.CancelOrder(order.Id, currentUser.Login))
                {
                    MessageBox.Show("Заявка отменена", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadOrders();
                }
                else
                {
                    MessageBox.Show("Ошибка при отмене заявки", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnUpdateStatus_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0 || string.IsNullOrEmpty(cmbStatus.Text))
            {
                MessageBox.Show("Выберите заявку и новый статус", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectedRow = dataGridView.SelectedRows[0];
            string orderNumber = selectedRow.Cells["Номер"].Value.ToString();

            var orders = dbHelper.GetAllOrders();
            var order = orders.FirstOrDefault(o => o.OrderNumber == orderNumber);

            if (order != null && dbHelper.UpdateOrderStatus(order.Id, cmbStatus.Text))
            {
                MessageBox.Show("Статус обновлен", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadOrders();
            }
            else
            {
                MessageBox.Show("Ошибка при обновлении статуса", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
