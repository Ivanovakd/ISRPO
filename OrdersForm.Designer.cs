using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    partial class OrdersForm
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
            this.Size = new Size(900, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Заявки";
            this.BackColor = Color.White;

            // Панель управления
            var controlPanel = new Panel();
            controlPanel.Dock = DockStyle.Top;
            controlPanel.Height = 50;
            controlPanel.BackColor = Color.FromArgb(210, 223, 255);

            btnRefresh = new Button { Text = "Обновить", Location = new Point(20, 12), Size = new Size(80, 25) };
            btnRefresh.BackColor = Color.FromArgb(53, 92, 189);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Click += (s, e) => LoadOrders();

            btnCreateOrder = new Button { Text = "Новая заявка", Location = new Point(110, 12), Size = new Size(100, 25) };
            btnCreateOrder.BackColor = Color.FromArgb(0, 150, 0);
            btnCreateOrder.ForeColor = Color.White;
            btnCreateOrder.FlatStyle = FlatStyle.Flat;
            btnCreateOrder.Click += btnCreateOrder_Click;

            btnCancelOrder = new Button { Text = "Отменить", Location = new Point(220, 12), Size = new Size(80, 25) };
            btnCancelOrder.BackColor = Color.FromArgb(200, 0, 0);
            btnCancelOrder.ForeColor = Color.White;
            btnCancelOrder.FlatStyle = FlatStyle.Flat;
            btnCancelOrder.Click += btnCancelOrder_Click;

            // Для менеджера/администратора
            var lblStatus = new Label { Text = "Статус:", Location = new Point(310, 15), Size = new Size(50, 20), Visible = !isClientView };
            cmbStatus = new ComboBox { Location = new Point(360, 12), Size = new Size(120, 25), Visible = !isClientView };
            cmbStatus.Items.AddRange(new string[] { "На согласовании", "Ожидает предоплаты", "В производстве", "Готово к отгрузке", "Выполнена" });

            btnUpdateStatus = new Button { Text = "Обновить статус", Location = new Point(490, 12), Size = new Size(120, 25), Visible = !isClientView };
            btnUpdateStatus.BackColor = Color.FromArgb(53, 92, 189);
            btnUpdateStatus.ForeColor = Color.White;
            btnUpdateStatus.FlatStyle = FlatStyle.Flat;
            btnUpdateStatus.Click += btnUpdateStatus_Click;

            controlPanel.Controls.AddRange(new Control[] {
                btnRefresh, btnCreateOrder, btnCancelOrder, lblStatus, cmbStatus, btnUpdateStatus
            });

            // DataGridView
            dataGridView = new DataGridView();
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.ReadOnly = true;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.MultiSelect = false;

            this.Controls.Add(dataGridView);
            this.Controls.Add(controlPanel);
            this.ResumeLayout(false);

        }

        #endregion
        private DataGridView dataGridView;
        private Button btnRefresh;
        private Button btnCreateOrder;
        private Button btnCancelOrder;
        private Button btnUpdateStatus;
        private ComboBox cmbStatus;
    }
}