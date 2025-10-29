using System.Drawing;
using System.Windows.Forms;
using System;

namespace WindowsFormsApp1
{
    partial class LoginForm
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
            // Основные элементы
            lblTitle = new Label();
            Label lblLogin = new Label();
            Label lblPassword = new Label();
            txtLogin = new TextBox();
            txtPassword = new TextBox();
            btnLogin = new Button();
            btnGuest = new Button();
            chkShowPassword = new CheckBox();

            // Настройка формы
            this.SuspendLayout();

            // Заголовок
            lblTitle.Text = "Вход в систему Комфорт";
            lblTitle.Font = new Font("Candara", 14, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(53, 92, 189);
            lblTitle.Location = new Point(80, 20);
            lblTitle.Size = new Size(300, 30);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // Логин
            lblLogin.Text = "Логин:";
            lblLogin.Location = new Point(50, 70);
            lblLogin.Size = new Size(80, 20);

            txtLogin.Location = new Point(130, 68);
            txtLogin.Size = new Size(200, 25);
            txtLogin.TabIndex = 1;

            // Пароль
            lblPassword.Text = "Пароль:";
            lblPassword.Location = new Point(50, 105);
            lblPassword.Size = new Size(80, 20);

            txtPassword.Location = new Point(130, 103);
            txtPassword.Size = new Size(200, 25);
            txtPassword.TabIndex = 2;
            txtPassword.UseSystemPasswordChar = true;

            // Чекбокс показа пароля
            chkShowPassword.Text = "Показать пароль";
            chkShowPassword.Location = new Point(130, 130);
            chkShowPassword.Size = new Size(150, 20);
            chkShowPassword.CheckedChanged += (s, e) =>
            {
                txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
            };

            // Кнопки
            btnLogin.Text = "Войти";
            btnLogin.Location = new Point(80, 220);
            btnLogin.Size = new Size(100, 35);
            btnLogin.TabIndex = 3;
            btnLogin.BackColor = Color.FromArgb(53, 92, 189);
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Click += new EventHandler(btnLogin_Click);

            btnGuest.Text = "Войти как гость";
            btnGuest.Location = new Point(200, 220);
            btnGuest.Size = new Size(120, 35);
            btnGuest.TabIndex = 4;
            btnGuest.BackColor = Color.FromArgb(210, 223, 255);
            btnGuest.FlatStyle = FlatStyle.Flat;
            btnGuest.Click += new EventHandler(btnGuest_Click);

            // Добавление элементов на форму
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblLogin);
            this.Controls.Add(txtLogin);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(chkShowPassword);
            this.Controls.Add(btnLogin);
            this.Controls.Add(btnGuest);

            this.ResumeLayout(false);

        }

        #endregion

        // Объявление элементов управления
        private Label lblTitle;
        private TextBox txtLogin;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnGuest;
        private CheckBox chkShowPassword;

        // Элементы CAPTCHA
        private Label lblCaptcha;
        private TextBox txtCaptcha;
        private PictureBox picCaptcha;
        private Button btnRefreshCaptcha;
        private Label lblTimer;
    }
}