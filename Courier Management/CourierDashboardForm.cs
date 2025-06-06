using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Configuration;
using Courier_Management.Models;
using MySql.Data.MySqlClient;

namespace Courier_Management
{
    public partial class CourierDashboardForm : Form
    {
        public int LoggedInCourierId { get; }
        private readonly string _connectionString;

        public CourierDashboardForm(int courierId)
        {
            if (courierId <= 0)
                throw new ArgumentException("ID kuriera musi byæ wiêksze od zera.");

            InitializeComponent();
            LoggedInCourierId = courierId;
            _connectionString = GetConnectionString();

            ConfigureDataGridView();
            LoadCourierPackages();
        }

        private string GetConnectionString()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["CourierDBConnection"]?.ConnectionString;
            return string.IsNullOrEmpty(connectionString)
                ? throw new ApplicationException("Brak connection stringa w konfiguracji")
                : connectionString;
        }

        private void ConfigureDataGridView()
        {
            dgvParcels.AutoGenerateColumns = false;
            ApplyCommonGridStyles(dgvParcels);

            AddReadOnlyColumn("PackageID", "ID Przesy³ki");
            AddReadOnlyColumn("Address", "Adres");
            AddReadOnlyColumn("City", "Miasto");
            AddReadOnlyColumn("ClientID", "ID Klienta");
            AddStatusComboBoxColumn();

            dgvParcels.DataError += HandleDataError;
        }

        private void ApplyCommonGridStyles(DataGridView grid)
        {
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.RowTemplate.Height = 35;
            grid.AllowUserToAddRows = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void AddReadOnlyColumn(string propertyName, string headerText)
        {
            dgvParcels.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = propertyName,
                HeaderText = headerText,
                DataPropertyName = propertyName,
                ReadOnly = true
            });
        }

        private void AddStatusComboBoxColumn()
        {
            dgvParcels.Columns.Add(new DataGridViewComboBoxColumn
            {
                Name = "Status",
                HeaderText = "Status",
                DataPropertyName = "Status",
                DataSource = new[] { "Dostarczono", "Oczekuje", "W drodze" },
                FlatStyle = FlatStyle.Flat,
                DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
        }

        private List<Package> GetCourierPackages()
        {
            var packages = new List<Package>();
            var query = $"SELECT PackageID, Status, Address, City, ClientID FROM Packages WHERE CourierID = {LoggedInCourierId}";

            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                using (var command = new MySqlCommand(query, connection))
                {
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
                                ClientID = reader.GetInt32(4),
                                CourierID = LoggedInCourierId
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("B³¹d podczas ³adowania danych", ex.Message);
            }

            return packages;
        }

        private void LoadCourierPackages()
        {
            dgvParcels.DataSource = GetCourierPackages();
            if (dgvParcels.Rows.Count == 0)
                ShowInfo("Brak przesy³ek do wyœwietlenia");
        }

        private void SaveChanges()
        {
            if (!(dgvParcels.DataSource is List<Package> packages) || packages.Count == 0)
            {
                ShowInfo("Brak danych do zapisania");
                return;
            }

            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    var updatedCount = 0;
                    var updateQuery = "UPDATE Packages SET Status = @Status WHERE PackageID = @PackageID";

                    foreach (var package in packages)
                    {
                        using (var command = new MySqlCommand(updateQuery, connection))
                        {
                            command.Parameters.AddWithValue("@Status", package.Status);
                            command.Parameters.AddWithValue("@PackageID", package.PackageID);
                            updatedCount += command.ExecuteNonQuery();
                        }
                    }

                    ShowInfo($"Zaktualizowano statusy przesy³ek");
                    LoadCourierPackages();
                }
            }
            catch (Exception ex)
            {
                ShowError("B³¹d podczas zapisywania zmian", ex.Message);
            }
        }

        private void HandleDataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception != null && e.Context == DataGridViewDataErrorContexts.Commit)
            {
                ShowWarning("Wybierz poprawny status z listy: Dostarczono, Oczekuje, W drodze");
                e.ThrowException = false;
            }
        }

        private void ShowError(string title, string message) =>
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);

        private void ShowInfo(string message) =>
            MessageBox.Show(message, "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);

        private void ShowWarning(string message) =>
            MessageBox.Show(message, "Ostrze¿enie", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        private void BtnSave_Click(object sender, EventArgs e) => SaveChanges();
        private void BtnRefresh_Click(object sender, EventArgs e) => LoadCourierPackages();

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Czy na pewno chcesz siê wylogowaæ?", "Potwierdzenie",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Hide();
                new LoginForm().Show();
            }
        }
    }
}