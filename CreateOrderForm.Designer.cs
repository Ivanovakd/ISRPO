using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    partial class CreateOrderForm
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
            this.Size = new Size(500, 350);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Text = "Создание заявки";

            // Заголовок
            lblTitle = new Label();
            lblTitle.Text = "Создание заявки на товар";
            lblTitle.Font = new Font("Candara", 14, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(53, 92, 189);
            lblTitle.Location = new Point(20, 20);
            lblTitle.Size = new Size(400, 30);

            // Выбор товара
            var lblProduct = new Label { Text = "Товар:", Location = new Point(20, 70), Size = new Size(100, 20) };
            cmbProducts = new ComboBox { Location = new Point(130, 68), Size = new Size(300, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbProducts.SelectedIndexChanged += cmbProducts_SelectedIndexChanged;

            // Информация о товаре
            var lblInfo = new Label { Text = "Информация:", Location = new Point(20, 110), Size = new Size(100, 20) };
            txtProductInfo = new TextBox { Location = new Point(130, 108), Size = new Size(300, 60), Multiline = true, ReadOnly = true, BackColor = Color.WhiteSmoke };

            // Кнопки
            btnCreate = new Button { Text = "Создать заявку", Location = new Point(150, 250), Size = new Size(120, 35) };
            btnCreate.BackColor = Color.FromArgb(53, 92, 189);
            btnCreate.ForeColor = Color.White;
            btnCreate.FlatStyle = FlatStyle.Flat;
            btnCreate.Click += btnCreate_Click;

            btnCancel = new Button { Text = "Отмена", Location = new Point(280, 250), Size = new Size(80, 35) };
            btnCancel.BackColor = Color.FromArgb(210, 223, 255);
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Click += (s, e) => this.Close();

            // Добавление элементов
            this.Controls.AddRange(new Control[] {
                lblTitle, lblProduct, cmbProducts, lblInfo, txtProductInfo, btnCreate, btnCancel
            });

            this.ResumeLayout(false);
        }

        #endregion
        private Label lblTitle;
        private ComboBox cmbProducts;
        private TextBox txtProductInfo;
        private Button btnCreate;
        private Button btnCancel;
    }
}