using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    partial class ProductEditForm
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
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Инициализируем элементы управления
            lblTitle = new Label();
            txtArticle = new TextBox();
            txtName = new TextBox();
            cmbType = new ComboBox();
            txtPrice = new TextBox();
            txtMaterial = new TextBox();
            btnSave = new Button();
            btnCancel = new Button();

            // Заголовок
            lblTitle.Text = "Товар";
            lblTitle.Font = new Font("Candara", 14, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(53, 92, 189);
            lblTitle.Location = new Point(20, 20);
            lblTitle.Size = new Size(400, 30);

            // Поля формы
            var lblArticle = new Label { Text = "Артикул:", Location = new Point(20, 70), Size = new Size(100, 20) };
            txtArticle.Location = new Point(130, 68);
            txtArticle.Size = new Size(200, 25);

            var lblName = new Label { Text = "Наименование:", Location = new Point(20, 110), Size = new Size(100, 20) };
            txtName.Location = new Point(130, 108);
            txtName.Size = new Size(300, 25);

            var lblType = new Label { Text = "Тип продукции:", Location = new Point(20, 150), Size = new Size(100, 20) };
            cmbType.Location = new Point(130, 148);
            cmbType.Size = new Size(200, 25);
            cmbType.DropDownStyle = ComboBoxStyle.DropDownList;

            var lblPrice = new Label { Text = "Цена:", Location = new Point(20, 190), Size = new Size(100, 20) };
            txtPrice.Location = new Point(130, 188);
            txtPrice.Size = new Size(200, 25);

            var lblMaterial = new Label { Text = "Материал:", Location = new Point(20, 230), Size = new Size(100, 20) };
            txtMaterial.Location = new Point(130, 228);
            txtMaterial.Size = new Size(300, 25);

            // Кнопки
            btnSave.Text = "Сохранить";
            btnSave.Location = new Point(150, 320);
            btnSave.Size = new Size(100, 35);
            btnSave.BackColor = Color.FromArgb(53, 92, 189);
            btnSave.ForeColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Click += btnSave_Click;

            btnCancel.Text = "Отмена";
            btnCancel.Location = new Point(260, 320);
            btnCancel.Size = new Size(100, 35);
            btnCancel.BackColor = Color.FromArgb(210, 223, 255);
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Click += (s, e) => this.Close();

            // Добавление элементов
            this.Controls.AddRange(new Control[] {
                lblTitle, lblArticle, txtArticle, lblName, txtName, lblType, cmbType,
                lblPrice, txtPrice, lblMaterial, txtMaterial,btnSave, btnCancel
            });

            this.ResumeLayout(false);
        }

        #endregion
        private Label lblTitle;
        private TextBox txtArticle;
        private TextBox txtName;
        private ComboBox cmbType;
        private TextBox txtPrice;
        private TextBox txtMaterial;
        private Button btnSave;
        private Button btnCancel;
    }
}