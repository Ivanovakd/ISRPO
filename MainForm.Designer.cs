using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip = new MenuStrip();
            dataGridViewProducts = new DataGridView();
            txtSearch = new TextBox();
            lblUserName = new Label();
            lblUserRole = new Label();
            btnLogout = new Button();
            btnWorkshops = new Button();
            btnProductTypes = new Button();
            btnHistory = new Button();
            btnAddProduct = new Button();
            cmbSortBy = new ComboBox();
            btnSortDirection = new Button();

            // Настройка формы
            this.SuspendLayout();
            this.Size = new Size(1100, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Комфорт - Производственная система";

            // MenuStrip
            var fileMenu = new ToolStripMenuItem("Файл");
            var exitItem = new ToolStripMenuItem("Выход");
            exitItem.Click += (s, e) => Application.Exit();
            fileMenu.DropDownItems.Add(exitItem);

            ordersToolStripMenuItem = new ToolStripMenuItem("Заявки");
            ordersToolStripMenuItem.Click += (s, ev) => btnOrders_Click(s, ev);
            menuStrip.Items.Add(ordersToolStripMenuItem);

            productsToolStripMenuItem = new ToolStripMenuItem("Продукция");
            productsToolStripMenuItem.Click += (s, e) => LoadProducts();

            workshopsToolStripMenuItem = new ToolStripMenuItem("Цеха");
            workshopsToolStripMenuItem.Click += (s, e) => ShowWorkshops();

            productTypesToolStripMenuItem = new ToolStripMenuItem("Типы продукции");
            productTypesToolStripMenuItem.Click += (s, e) => ShowProductTypes();

            historyToolStripMenuItem = new ToolStripMenuItem("История входов");
            historyToolStripMenuItem.Click += (s, e) => ShowHistoryForm();

            menuStrip.Items.AddRange(new ToolStripItem[] {
                fileMenu, ordersToolStripMenuItem, productsToolStripMenuItem, workshopsToolStripMenuItem,
                productTypesToolStripMenuItem, historyToolStripMenuItem
            });

            // Панель пользователя
            var userPanel = new Panel();
            userPanel.Dock = DockStyle.Top;
            userPanel.Height = 80;
            userPanel.BackColor = Color.FromArgb(210, 223, 255);

            lblUserName.Location = new Point(20, 10);
            lblUserName.Size = new Size(240, 25);
            lblUserName.Font = new Font("Candara", 12, FontStyle.Bold);

            lblUserRole.Location = new Point(20, 40);
            lblUserRole.Size = new Size(200, 20);
            lblUserRole.Font = new Font("Candara", 10);

            btnLogout.Text = "Выход";
            btnLogout.Location = new Point(900, 20);
            btnLogout.Size = new Size(80, 35);
            btnLogout.Click += btnLogout_Click;

            btnWorkshops.Text = "Цеха";
            btnWorkshops.Location = new Point(300, 20);
            btnWorkshops.Size = new Size(80, 35);
            btnWorkshops.Click += (s, e) => ShowWorkshops();

            btnProductTypes.Text = "Типы продукции";
            btnProductTypes.Location = new Point(390, 20);
            btnProductTypes.Size = new Size(120, 35);
            btnProductTypes.Click += (s, e) => ShowProductTypes();

            btnHistory.Text = "История входов";
            btnHistory.Location = new Point(520, 20);
            btnHistory.Size = new Size(120, 35);
            btnHistory.Click += (s, e) => ShowHistoryForm();

            btnAddProduct.Text = "Добавить товар";
            btnAddProduct.Location = new Point(650, 20);
            btnAddProduct.Size = new Size(120, 35);
            btnAddProduct.Click += btnAddProduct_Click;
            btnAddProduct.Visible = currentUser.IsAdmin;

            userPanel.Controls.Add(lblUserName);
            userPanel.Controls.Add(lblUserRole);
            userPanel.Controls.Add(btnLogout);
            userPanel.Controls.Add(btnWorkshops);
            userPanel.Controls.Add(btnProductTypes);
            userPanel.Controls.Add(btnHistory);
            userPanel.Controls.Add(btnAddProduct);
            userPanel.Controls.Add(btnOrders);

            // Панель поиска и сортировки
            var searchPanel = new Panel();
            searchPanel.Dock = DockStyle.Top;
            searchPanel.Height = 50;
            searchPanel.BackColor = Color.White;

            var lblSearch = new Label();
            lblSearch.Text = "Поиск:";
            lblSearch.Location = new Point(20, 15);
            lblSearch.Size = new Size(50, 20);

            txtSearch.Location = new Point(80, 12);
            txtSearch.Size = new Size(200, 25);
            txtSearch.TextChanged += txtSearch_TextChanged;

            var lblSort = new Label();
            lblSort.Text = "Сортировка:";
            lblSort.Location = new Point(290, 15);
            lblSort.Size = new Size(85, 20);

            cmbSortBy.Location = new Point(380, 12);
            cmbSortBy.Size = new Size(120, 25);
            cmbSortBy.Items.AddRange(new string[] { "Наименование_продукции", "Артикул", "Тип" });
            cmbSortBy.SelectedIndex = 0;
            cmbSortBy.SelectedIndexChanged += cmbSortBy_SelectedIndexChanged;

            btnSortDirection.Text = "↑";
            btnSortDirection.Location = new Point(510, 12);
            btnSortDirection.Size = new Size(30, 25);
            btnSortDirection.Click += btnSortDirection_Click;

            searchPanel.Controls.Add(lblSearch);
            searchPanel.Controls.Add(txtSearch);
            searchPanel.Controls.Add(lblSort);
            searchPanel.Controls.Add(cmbSortBy);
            searchPanel.Controls.Add(btnSortDirection);

            // DataGridView
            dataGridViewProducts.Dock = DockStyle.Fill;
            dataGridViewProducts.Location = new Point(0, 130);
            dataGridViewProducts.Size = new Size(1100, 470);
            dataGridViewProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewProducts.MultiSelect = false;
            dataGridViewProducts.CellDoubleClick += dataGridViewProducts_CellDoubleClick;

            // Контекстное меню для удаления
            var contextMenu = new ContextMenuStrip();
            var deleteMenuItem = new ToolStripMenuItem("Удалить товар");
            deleteMenuItem.Click += deleteMenuItem_Click;
            contextMenu.Items.Add(deleteMenuItem);
            dataGridViewProducts.ContextMenuStrip = contextMenu;

            // Добавление на форму
            this.Controls.Add(dataGridViewProducts);
            this.Controls.Add(searchPanel);
            this.Controls.Add(userPanel);
            this.Controls.Add(menuStrip);

            this.MainMenuStrip = menuStrip;
            this.ResumeLayout(false);

            //Заказ
            btnOrders = new Button();
            btnOrders.Text = "Мои заявки";
            btnOrders.Location = new Point(650, 20);
            btnOrders.Size = new Size(100, 35);
            btnOrders.Click += btnOrders_Click;
        }

        #endregion

        // Объявление элементов управления
        private MenuStrip menuStrip;
        private DataGridView dataGridViewProducts;
        private TextBox txtSearch;
        private Label lblUserName;
        private Label lblUserRole;
        private Button btnLogout;
        private Button btnWorkshops;
        private Button btnProductTypes;
        private Button btnHistory;
        private Button btnAddProduct;
        private ComboBox cmbSortBy;
        private Button btnSortDirection;
        private ToolStripMenuItem productsToolStripMenuItem;
        private ToolStripMenuItem workshopsToolStripMenuItem;
        private ToolStripMenuItem productTypesToolStripMenuItem;
        private ToolStripMenuItem historyToolStripMenuItem;
        private Button btnOrders;
        private ToolStripMenuItem ordersToolStripMenuItem;
    }
}

