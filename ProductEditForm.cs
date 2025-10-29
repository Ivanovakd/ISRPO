using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WindowsFormsApp1
{
    public partial class ProductEditForm : Form
    {
        private string currentArticle;
        private DatabaseHelper dbHelper;
        private User currentUser;
        private bool isEditMode;

        public ProductEditForm(string article, User user)
        {
            InitializeComponent(); // Сначала инициализируем элементы
            this.currentArticle = article;
            this.dbHelper = new DatabaseHelper();
            this.currentUser = user;
            this.isEditMode = !string.IsNullOrEmpty(article);

            SetupForm(); // Затем настраиваем форму
            LoadProductData(); // И загружаем данные
        }

        private void SetupForm()
        {
            this.Text = isEditMode ? "Редактирование товара" : "Добавление товара";
            lblTitle.Text = isEditMode ? "Редактирование товара" : "Добавление нового товара";

            if (isEditMode)
            {
                txtArticle.Enabled = false;
                txtArticle.BackColor = Color.LightGray;
            }
            else
            {
                txtArticle.Text = dbHelper.GetNextArticle();
            }

            // Заполняем комбобокс типами продукции
            if (cmbType != null)
            {
                cmbType.Items.Clear();
                cmbType.Items.AddRange(new string[] { "Гостиные", "Прихожие", "Мягкая мебель", "Кровати", "Шкафы", "Комоды" });
            }
        }

        private void LoadProductData()
        {
            if (isEditMode && !string.IsNullOrEmpty(currentArticle))
            {
                var product = dbHelper.GetProductById(currentArticle);
                if (product != null)
                {
                    if (txtArticle != null) txtArticle.Text = product.Article;
                    if (txtName != null) txtName.Text = product.Name;
                    if (cmbType != null) cmbType.Text = product.Type;
                    if (txtPrice != null) txtPrice.Text = product.Price.ToString("F2");
                    if (txtMaterial != null) txtMaterial.Text = product.Material;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                var product = new Product
                {
                    Article = txtArticle.Text.Trim(),
                    Name = txtName.Text.Trim(),
                    Type = cmbType.Text,
                    Price = decimal.Parse(txtPrice.Text),
                    Material = txtMaterial.Text.Trim()
                };

                bool success;
                if (isEditMode)
                {
                    success = dbHelper.UpdateProduct(product);
                }
                else
                {
                    success = dbHelper.AddProduct(product);
                }

                if (success)
                {
                    MessageBox.Show("Данные успешно сохранены", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Ошибка сохранения данных", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtArticle.Text))
            {
                MessageBox.Show("Введите артикул", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtArticle.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите наименование", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cmbType.Text))
            {
                MessageBox.Show("Выберите тип продукции", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbType.Focus();
                return false;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price < 0)
            {
                MessageBox.Show("Введите корректную цену (неотрицательное число)", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPrice.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtMaterial.Text))
            {
                MessageBox.Show("Введите материал", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMaterial.Focus();
                return false;
            }

            return true;
        }
    }
}
