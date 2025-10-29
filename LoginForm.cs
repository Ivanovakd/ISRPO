using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class LoginForm : Form
    {
        private DatabaseHelper dbHelper;
        private int loginAttempts = 0;
        private bool captchaRequired = false;
        private string currentCaptcha;
        private DateTime? blockTime = null;
        private Timer blockTimer;
        private bool permanentlyBlocked = false;

        public LoginForm()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            InitializeCaptchaControls();
            ApplyStyles();
            UpdateUI();
        }

        private void InitializeCaptchaControls()
        {
            // Создаем таймер для блокировки
            blockTimer = new Timer();
            blockTimer.Interval = 1000; // 1 секунда
            blockTimer.Tick += BlockTimer_Tick;

            // Инициализируем элементы CAPTCHA (они будут скрыты изначально)
            lblCaptcha = new Label();
            txtCaptcha = new TextBox();
            picCaptcha = new PictureBox();
            btnRefreshCaptcha = new Button();
            lblTimer = new Label();

            // Настройка элементов CAPTCHA
            lblCaptcha.Text = "Ввод:";
            lblCaptcha.Location = new Point(50, 160);
            lblCaptcha.Size = new Size(80, 20);
            lblCaptcha.Visible = false;

            txtCaptcha.Location = new Point(130, 158);
            txtCaptcha.Size = new Size(120, 25);
            txtCaptcha.Visible = false;

            picCaptcha.Location = new Point(260, 150);
            picCaptcha.Size = new Size(120, 40);
            picCaptcha.BorderStyle = BorderStyle.FixedSingle;
            picCaptcha.Visible = false;
            picCaptcha.Click += (s, e) => GenerateCaptcha();

            btnRefreshCaptcha.Text = "🔄";
            btnRefreshCaptcha.Location = new Point(390, 150);
            btnRefreshCaptcha.Size = new Size(30, 30);
            btnRefreshCaptcha.Visible = false;
            btnRefreshCaptcha.Click += (s, e) => GenerateCaptcha();

            lblTimer.Text = "";
            lblTimer.Location = new Point(50, 190);
            lblTimer.Size = new Size(300, 20);
            lblTimer.ForeColor = Color.Red;
            lblTimer.Font = new Font("Candara", 9, FontStyle.Bold);
            lblTimer.Visible = false;

            // Добавляем элементы на форму
            this.Controls.Add(lblCaptcha);
            this.Controls.Add(txtCaptcha);
            this.Controls.Add(picCaptcha);
            this.Controls.Add(btnRefreshCaptcha);
            this.Controls.Add(lblTimer);
        }

        private void ApplyStyles()
        {
            this.BackColor = Color.White;
            this.Font = new Font("Candara", 10);
            this.Text = "Авторизация - Комфорт";
            this.Size = new Size(450, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void UpdateUI()
        {
            if (permanentlyBlocked)
            {
                lblTitle.Text = "СИСТЕМА ЗАБЛОКИРОВАНА";
                lblTitle.ForeColor = Color.Red;
                btnLogin.Enabled = false;
                btnGuest.Enabled = false;
                txtLogin.Enabled = false;
                txtPassword.Enabled = false;
                lblTimer.Text = "Требуется перезапуск приложения";
                lblTimer.Visible = true;
                return;
            }

            if (blockTime.HasValue && DateTime.Now < blockTime.Value)
            {
                TimeSpan remaining = blockTime.Value - DateTime.Now;
                lblTimer.Text = $"Система заблокирована. Осталось: {remaining:mm\\:ss}";
                lblTimer.Visible = true;
                btnLogin.Enabled = false;
                btnGuest.Enabled = false;
                blockTimer.Start();
            }
            else
            {
                lblTimer.Visible = false;
                btnLogin.Enabled = true;
                btnGuest.Enabled = true;
                blockTimer.Stop();
            }

            // Показываем/скрываем CAPTCHA
            lblCaptcha.Visible = captchaRequired;
            txtCaptcha.Visible = captchaRequired;
            picCaptcha.Visible = captchaRequired;
            btnRefreshCaptcha.Visible = captchaRequired;

            if (captchaRequired && string.IsNullOrEmpty(currentCaptcha))
            {
                GenerateCaptcha();
            }
        }

        private void BlockTimer_Tick(object sender, EventArgs e)
        {
            if (blockTime.HasValue)
            {
                if (DateTime.Now >= blockTime.Value)
                {
                    UpdateUI();
                }
                else
                {
                    TimeSpan remaining = blockTime.Value - DateTime.Now;
                    lblTimer.Text = $"Система заблокирована. Осталось: {remaining:mm\\:ss}";
                }
            }
        }

        private void GenerateCaptcha()
        {
            var random = new Random();
            var captchaBuilder = new StringBuilder();
            string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

            for (int i = 0; i < 4; i++)
            {
                captchaBuilder.Append(chars[random.Next(chars.Length)]);
            }
            currentCaptcha = captchaBuilder.ToString();

            // Создаем изображение CAPTCHA
            var bmp = new Bitmap(picCaptcha.Width, picCaptcha.Height);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Добавляем фон с шумом
                for (int i = 0; i < 100; i++)
                {
                    int x1 = random.Next(bmp.Width);
                    int y1 = random.Next(bmp.Height);
                    int x2 = random.Next(bmp.Width);
                    int y2 = random.Next(bmp.Height);

                    using (var pen = new Pen(Color.FromArgb(random.Next(100, 200), random.Next(100, 200), random.Next(100, 200)), 1))
                    {
                        g.DrawLine(pen, x1, y1, x2, y2);
                    }
                }

                // Добавляем точки-шум
                for (int i = 0; i < 200; i++)
                {
                    int x = random.Next(bmp.Width);
                    int y = random.Next(bmp.Height);
                    bmp.SetPixel(x, y, Color.FromArgb(random.Next(150, 255), random.Next(150, 255), random.Next(150, 255)));
                }

                // Рисуем текст с искажениями
                using (var font = new Font("Arial", 16, FontStyle.Bold))
                {
                    for (int i = 0; i < currentCaptcha.Length; i++)
                    {
                        // Случайное смещение и поворот для каждого символа
                        float x = 10 + i * 25 + random.Next(-3, 3);
                        float y = 10 + random.Next(-5, 5);
                        float angle = random.Next(-15, 15);

                        // Сохраняем текущую трансформацию
                        var transform = g.Transform;
                        g.TranslateTransform(x, y);
                        g.RotateTransform(angle);

                        // Рисуем символ со случайным цветом
                        using (var brush = new SolidBrush(Color.FromArgb(random.Next(50, 150), random.Next(50, 150), random.Next(50, 150))))
                        {
                            g.DrawString(currentCaptcha[i].ToString(), font, brush, 0, 0);
                        }

                        // Восстанавливаем трансформацию
                        g.Transform = transform;
                    }
                }

                // Добавляем линии, перечеркивающие текст
                for (int i = 0; i < 3; i++)
                {
                    int x1 = random.Next(5, bmp.Width - 5);
                    int y1 = random.Next(5, bmp.Height - 5);
                    int x2 = random.Next(5, bmp.Width - 5);
                    int y2 = random.Next(5, bmp.Height - 5);

                    using (var pen = new Pen(Color.FromArgb(random.Next(100, 200), random.Next(100, 200), random.Next(100, 200)), 2))
                    {
                        g.DrawLine(pen, x1, y1, x2, y2);
                    }
                }
            }

            picCaptcha.Image = bmp;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (permanentlyBlocked)
            {
                MessageBox.Show("Система заблокирована. Перезапустите приложение.", "Блокировка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (blockTime.HasValue && DateTime.Now < blockTime.Value)
            {
                MessageBox.Show($"Система заблокирована до {blockTime.Value:HH:mm:ss}", "Блокировка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка CAPTCHA если требуется
            if (captchaRequired)
            {
                if (string.IsNullOrEmpty(txtCaptcha.Text) ||
                    !txtCaptcha.Text.Equals(currentCaptcha, StringComparison.OrdinalIgnoreCase))
                {
                    loginAttempts++;
                    MessageBox.Show("Неверная CAPTCHA! Попробуйте еще раз.", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    GenerateCaptcha();
                    txtCaptcha.Clear();
                    txtCaptcha.Focus();

                    // Логика блокировки после CAPTCHA
                    if (loginAttempts >= 3) // 1 обычная + 2 с CAPTCHA
                    {
                        if (loginAttempts == 3)
                        {
                            blockTime = DateTime.Now.AddMinutes(3);
                            MessageBox.Show("3 неудачные попытки! Система заблокирована на 3 минуты.", "Блокировка",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            UpdateUI();
                        }
                        else if (loginAttempts >= 4)
                        {
                            permanentlyBlocked = true;
                            MessageBox.Show("Превышено количество попыток! Перезапустите приложение.", "Блокировка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            UpdateUI();
                        }
                    }
                    return;
                }
            }

            string login = txtLogin.Text;
            string password = txtPassword.Text;

            if (dbHelper.ValidateUser(login, password))
            {
                User user = dbHelper.GetUser(login);
                dbHelper.AddLoginHistory(login, true);

                // Сброс счетчиков при успешном входе
                loginAttempts = 0;
                captchaRequired = false;
                permanentlyBlocked = false;
                blockTime = null;

                this.Hide();
                MainForm mainForm = new MainForm(user);
                mainForm.ShowDialog();
                this.Close();
            }
            else
            {
                loginAttempts++;
                dbHelper.AddLoginHistory(login, false);

                if (loginAttempts == 1)
                {
                    // Первая ошибка - включаем CAPTCHA
                    captchaRequired = true;
                    GenerateCaptcha();
                    MessageBox.Show("Неверный логин или пароль! Теперь требуется ввод CAPTCHA.", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    UpdateUI();
                    txtCaptcha.Focus();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    if (captchaRequired)
                    {
                        GenerateCaptcha();
                        txtCaptcha.Clear();
                        txtCaptcha.Focus();
                    }
                }
            }
        }

        private void btnGuest_Click(object sender, EventArgs e)
        {
            if (permanentlyBlocked)
            {
                MessageBox.Show("Система заблокирована. Перезапустите приложение.", "Блокировка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (blockTime.HasValue && DateTime.Now < blockTime.Value)
            {
                MessageBox.Show($"Система заблокирована до {blockTime.Value:HH:mm:ss}", "Блокировка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            User guestUser = new User
            {
                Login = "Гость",
                FullName = "Гость",
                Role = "Гость"
            };

            dbHelper.AddLoginHistory("Гость", true);
            this.Hide();
            MainForm mainForm = new MainForm(guestUser);
            mainForm.ShowDialog();
            this.Close();
        }
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            blockTimer?.Stop();
            blockTimer?.Dispose();
            base.OnFormClosed(e);
        }
    }
}
