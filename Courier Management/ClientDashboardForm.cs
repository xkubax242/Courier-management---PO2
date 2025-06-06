using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Configuration;
using MySql.Data.MySqlClient;
using Courier_Management.Models;

namespace Courier_Management
{
    public partial class ClientDashboardForm : Form
    {
        public int LoggedInUserId { get; }
        private readonly string _connectionString;

        public ClientDashboardForm(int clientId)
        {
            if (clientId <= 0)
                throw new ArgumentException("ID klienta musi być większe od zera", nameof(clientId));

            InitializeComponent();
            LoggedInUserId = clientId;
            _connectionString = GetConnectionString();

            ConfigureDataGridView();
            LoadClientPackages();
        }

        private string GetConnectionString()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["CourierDBConnection"]?.ConnectionString;
            return string.IsNullOrEmpty(connectionString)
                ? throw new ApplicationException("Brak zdefiniowanego connection stringa")
                : connectionString;
        }

        private void ConfigureDataGridView()
        {
            dgvOrders.DataSource = new List<Package>();

            dgvOrders.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgvOrders.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            dgvOrders.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOrders.RowTemplate.Height = 35;
            dgvOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOrders.ReadOnly = true;
            dgvOrders.AllowUserToAddRows = false;
            dgvOrders.AllowUserToDeleteRows = false;
            dgvOrders.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private List<Package> RetrieveClientPackages()
        {
            var packages = new List<Package>();

            const string query = @"
                SELECT PackageID, Status, Address, City, CourierID 
                FROM Packages 
                WHERE ClientID = @ClientID";

            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClientID", LoggedInUserId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            packages.Add(new Package
                            {
                                PackageID = reader.GetInt32(0),
                                Status = reader.GetString(1),
                                Address = reader.GetString(2),
                                City = reader.GetString(3),
                                ClientID = LoggedInUserId,
                                CourierID = reader.IsDBNull(4) ? null : (int?)reader.GetInt32(4)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas pobierania przesyłek: {ex.Message}",
                    "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return packages;
        }

        private void LoadClientPackages()
        {
            var packages = RetrieveClientPackages();

            if (packages.Count == 0)
            {
                MessageBox.Show("Brak przesyłek do wyświetlenia.", "Informacja",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            dgvOrders.DataSource = packages;
            FormatDataGridViewColumns();
        }

        private void FormatDataGridViewColumns()
        {
            dgvOrders.Columns["PackageID"].HeaderText = "ID Przesyłki";
            dgvOrders.Columns["Status"].HeaderText = "Status";
            dgvOrders.Columns["Address"].HeaderText = "Adres";
            dgvOrders.Columns["City"].HeaderText = "Miasto";
            dgvOrders.Columns["CourierID"].HeaderText = "ID Kuriera";
            dgvOrders.Columns["ClientID"].Visible = false;
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Czy na pewno chcesz się wylogować?", "Potwierdzenie",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                new LoginForm().Show();
            }
        }
    }
}