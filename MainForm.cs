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
    public partial class MainForm : Form
    {
        private User currentUser;
        private ProductEditForm activeEditForm = null;
        private DatabaseHelper dbHelper;
        private string currentSortColumn = "Наименование_продукции";
        private bool sortAscending = true;

        public MainForm(User user)
        {
            currentUser = user;
            dbHelper = new DatabaseHelper();
            InitializeComponent();
            ApplyStyles();
            LoadUserInfo();
            LoadProducts();
        }
        private void ApplyStyles()
        {
            this.BackColor = Color.White;
            this.Font = new Font("Candara", 9);

            btnWorkshops.BackColor = Color.FromArgb(53, 92, 189);
            btnWorkshops.ForeColor = Color.White;
            btnWorkshops.FlatStyle = FlatStyle.Flat;

            btnProductTypes.BackColor = Color.FromArgb(53, 92, 189);
            btnProductTypes.ForeColor = Color.White;
            btnProductTypes.FlatStyle = FlatStyle.Flat;

            btnHistory.BackColor = Color.FromArgb(53, 92, 189);
            btnHistory.ForeColor = Color.White;
            btnHistory.FlatStyle = FlatStyle.Flat;

            btnAddProduct.BackColor = Color.FromArgb(0, 150, 0);
            btnAddProduct.ForeColor = Color.White;
            btnAddProduct.FlatStyle = FlatStyle.Flat;

            btnLogout.BackColor = Color.FromArgb(210, 223, 255);
            btnLogout.FlatStyle = FlatStyle.Flat;

            btnSortDirection.BackColor = Color.FromArgb(53, 92, 189);
            btnSortDirection.ForeColor = Color.White;
            btnSortDirection.FlatStyle = FlatStyle.Flat;

            btnOrders.BackColor = Color.FromArgb(53, 92, 189);
            btnOrders.ForeColor = Color.White;
            btnOrders.FlatStyle = FlatStyle.Flat;
        }

        private void LoadUserInfo()
        {
            if (currentUser != null)
            {
                lblUserName.Text = $"Пользователь: {currentUser.FullName}";
                lblUserRole.Text = $"Роль: {currentUser.Role}";

                bool isGuest = !(currentUser.IsAdmin || currentUser.IsManager || currentUser.IsClient);
                workshopsToolStripMenuItem.Enabled = !isGuest;
                productTypesToolStripMenuItem.Enabled = !isGuest;
                historyToolStripMenuItem.Enabled = !isGuest;
                btnWorkshops.Enabled = !isGuest;
                btnProductTypes.Enabled = !isGuest;
                btnHistory.Enabled = !isGuest;

                btnOrders.Visible = currentUser.IsClient || currentUser.IsAdmin || currentUser.IsManager;

                // Настройка видимости кнопки добавления
                btnAddProduct.Visible = currentUser.IsAdmin;
                dataGridViewProducts.ContextMenuStrip.Enabled = currentUser.IsAdmin;
            }
        }
        private void btnOrders_Click(object sender, EventArgs e)
        {
            var ordersForm = new OrdersForm(currentUser);
            ordersForm.ShowDialog();
        }
        private void LoadProducts()
        {
            try
            {
                DataTable products = dbHelper.GetProducts(currentSortColumn, sortAscending);
                dataGridViewProducts.DataSource = products;
                ConfigureDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки продукции: {ex.Message}");
            }
        }

        private void ConfigureDataGridView()
        {
            if (dataGridViewProducts == null) return;

            dataGridViewProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewProducts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(53, 92, 189);
            dataGridViewProducts.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewProducts.ColumnHeadersDefaultCellStyle.Font = new Font("Candara", 10, FontStyle.Bold);
            dataGridViewProducts.EnableHeadersVisualStyles = false;

            // Форматирование колонки цены
            if (dataGridViewProducts.Columns["Минимальная_стоимость_для_партнера"] != null)
            {
                dataGridViewProducts.Columns["Минимальная_стоимость_для_партнера"].DefaultCellStyle.Format = "N2";
                dataGridViewProducts.Columns["Минимальная_стоимость_для_партнера"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }

        private void cmbSortBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentSortColumn = cmbSortBy.SelectedItem.ToString();
            ApplyFilters();
        }

        private void btnSortDirection_Click(object sender, EventArgs e)
        {
            sortAscending = !sortAscending;
            btnSortDirection.Text = sortAscending ? "↑" : "↓";
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            string searchText = txtSearch.Text.Trim();

            try
            {
                DataTable products;
                if (string.IsNullOrEmpty(searchText))
                {
                    products = dbHelper.GetProducts(currentSortColumn, sortAscending);
                }
                else
                {
                    products = dbHelper.SearchProducts(searchText, currentSortColumn, sortAscending);
                }
                dataGridViewProducts.DataSource = products;
                ConfigureDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка применения фильтров: {ex.Message}");
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void dataGridViewProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && currentUser.IsAdmin)
            {
                var row = dataGridViewProducts.Rows[e.RowIndex];
                string article = row.Cells["Артикул"].Value.ToString();

                OpenProductEditForm(article);
            }
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            OpenProductEditForm(null);
        }

        private void OpenProductEditForm(string article)
        {
            if (activeEditForm != null && !activeEditForm.IsDisposed)
            {
                activeEditForm.Focus();
                return;
            }

            activeEditForm = new ProductEditForm(article, currentUser);
            activeEditForm.FormClosed += (s, args) =>
            {
                activeEditForm = null;
                LoadProducts();
            };
            activeEditForm.Show();
        }

        private void deleteMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewProducts.SelectedRows.Count > 0 && currentUser.IsAdmin)
            {
                var row = dataGridViewProducts.SelectedRows[0];
                string article = row.Cells["Артикул"].Value.ToString();
                string productName = row.Cells["Наименование_продукции"].Value.ToString();

                var result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить товар '{productName}'?",
                    "Подтверждение удаления",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        if (dbHelper.DeleteProduct(article))
                        {
                            MessageBox.Show("Товар успешно удален", "Успех",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadProducts();
                        }
                        else
                        {
                            MessageBox.Show("Не удалось удалить товар", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ShowWorkshops()
        {
            try
            {
                DataTable workshops = dbHelper.GetWorkshops();
                DataGridViewForm form = new DataGridViewForm("Информация о цехах", workshops);
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки цехов: {ex.Message}");
            }
        }

        private void ShowProductTypes()
        {
            try
            {
                DataTable productTypes = dbHelper.GetProductTypes();
                DataGridViewForm form = new DataGridViewForm("Типы продукции и коэффициенты", productTypes);
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки типов продукции: {ex.Message}");
            }
        }

        private void ShowHistoryForm()
        {
            LoginHistoryForm form = new LoginHistoryForm();
            form.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
