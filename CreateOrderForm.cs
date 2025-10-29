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
    public partial class CreateOrderForm : Form
    {
        private DatabaseHelper dbHelper;
        private User currentUser;
        private Product selectedProduct;
        public CreateOrderForm(User user)
        {
            InitializeComponent();
            this.dbHelper = new DatabaseHelper();
            this.currentUser = user;
            LoadProducts();
        }
        private void LoadProducts()
        {
            try
            {
                var products = dbHelper.GetProducts();
                cmbProducts.Items.Clear();

                foreach (DataRow row in products.Rows)
                {
                    string productInfo = $"{row["Наименование_продукции"]} ({row["Артикул"]}) - {Convert.ToDecimal(row["Минимальная_стоимость_для_партнера"]):C}";
                    cmbProducts.Items.Add(new ProductItem
                    {
                        DisplayText = productInfo,
                        Article = row["Артикул"].ToString(),
                        Name = row["Наименование_продукции"].ToString(),
                        Price = Convert.ToDecimal(row["Минимальная_стоимость_для_партнера"])
                    });
                }

                if (cmbProducts.Items.Count > 0)
                    cmbProducts.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки товаров: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProducts.SelectedItem is ProductItem productItem)
            {
                txtProductInfo.Text = $"Артикул: {productItem.Article}\r\n" +
                                    $"Наименование_продукции: {productItem.Name}\r\n" +
                                    $"Минимальная_стоимость_для_партнера: {productItem.Price:C}\r\n";

                selectedProduct = new Product
                {
                    Article = productItem.Article,
                    Name = productItem.Name,
                    Price = productItem.Price
                };
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (selectedProduct == null)
            {
                MessageBox.Show("Выберите товар", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var order = new Order
                {
                    OrderNumber = dbHelper.GenerateOrderNumber(),
                    ClientLogin = currentUser.Login,
                    ProductArticle = selectedProduct.Article,
                    ProductName = selectedProduct.Name,
                    Status = "Создана",
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                };

                if (dbHelper.CreateOrder(order))
                {
                    MessageBox.Show("Заявка успешно создана!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Ошибка при создании заявки", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private class ProductItem
        {
            public string DisplayText { get; set; }
            public string Article { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }

            public override string ToString() => DisplayText;
        }
    }
}
