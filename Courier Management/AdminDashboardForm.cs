using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;
using MySql.Data.MySqlClient;
using Courier_Management.Models;

namespace Courier_Management
{
    public partial class AdminDashboardForm : Form
    {
        private List<Package> packagesList;
        private List<Users> usersList;
        private List<Users> couriersList;
        private const bool AllowAddingUsers = false;

        public AdminDashboardForm()
        {
            InitializeComponent();
            InitializeDataGridViews();
            LoadCouriersData();
            LoadPackagesData();
            LoadUsersData();
        }

        private void InitializeDataGridViews()
        {
            // Konfiguracja DataGridView dla paczek
            dgvParcels.AutoGenerateColumns = false;
            dgvParcels.AllowUserToAddRows = false;
            dgvParcels.AllowUserToDeleteRows = true;
            dgvParcels.EditMode = DataGridViewEditMode.EditOnEnter;
            StyleDataGridView(dgvParcels);

            // Konfiguracja DataGridView dla u¿ytkowników
            dgvUsers.AutoGenerateColumns = false;
            dgvUsers.AllowUserToAddRows = AllowAddingUsers;
            dgvUsers.AllowUserToDeleteRows = false;
            StyleDataGridView(dgvUsers);
        }

        private void StyleDataGridView(DataGridView dgv)
        {
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.RowTemplate.Height = 35;
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
        }

        private void LoadCouriersData()
        {
            try
            {
                couriersList = new List<Users>();

                using (var connection = new MySqlConnection(GetConnectionString()))
                {
                    connection.Open();
                    var query = "SELECT UserID, Name FROM Users WHERE Role = 'Courier'";

                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            couriersList.Add(new Users
                            {
                                UserID = reader.GetInt32("UserID"),
                                Name = reader.GetString("Name")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"B³¹d podczas ³adowania kurierów:\n{ex.Message}", "B³¹d", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPackagesData()
        {
            try
            {
                packagesList = new List<Package>();

                using (var connection = new MySqlConnection(GetConnectionString()))
                {
                    connection.Open();
                    var query = "SELECT PackageID, Status, Address, City, ClientID, CourierID FROM Packages";

                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            packagesList.Add(new Package
                            {
                                PackageID = reader.GetInt32("PackageID"),
                                Status = reader.GetString("Status"),
                                Address = reader.GetString("Address"),
                                City = reader.GetString("City"),
                                ClientID = reader.GetInt32("ClientID"),
                                CourierID = reader.IsDBNull(reader.GetOrdinal("CourierID")) ? (int?)null : reader.GetInt32("CourierID")
                            });
                        }
                    }
                }

                dgvParcels.Columns.Clear();

            
                dgvParcels.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "PackageID",
                    HeaderText = "ID Paczki",
                    DataPropertyName = "PackageID",
                    ReadOnly = true
                });

              
                AddStatusColumn();

                dgvParcels.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Address",
                    HeaderText = "Adres",
                    DataPropertyName = "Address",
                    ReadOnly = false
                });

                dgvParcels.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "City",
                    HeaderText = "Miasto",
                    DataPropertyName = "City",
                    ReadOnly = false
                });

                dgvParcels.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "ClientID",
                    HeaderText = "ID Klienta",
                    DataPropertyName = "ClientID",
                    ReadOnly = true
                });

                AddCourierColumn();

                dgvParcels.DataSource = packagesList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"B³¹d podczas ³adowania paczek:\n{ex.Message}", "B³¹d", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddCourierColumn()
        {
            var courierComboColumn = new DataGridViewComboBoxColumn
            {
                Name = "CourierID",
                HeaderText = "Kurier",
                DataPropertyName = "CourierID",
                FlatStyle = FlatStyle.Flat,
                DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DisplayIndex = 5,
                ReadOnly = false
            };

            if (couriersList != null)
            {
                courierComboColumn.DataSource = couriersList;
                courierComboColumn.ValueMember = "UserID";
                courierComboColumn.DisplayMember = "Name";
            }

            dgvParcels.Columns.Add(courierComboColumn);
        }

        private void AddStatusColumn()
        {
            var statusComboColumn = new DataGridViewComboBoxColumn
            {
                Name = "Status",
                HeaderText = "Status",
                DataPropertyName = "Status",
                FlatStyle = FlatStyle.Flat,
                DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DataSource = new[] { "Dostarczono", "Oczekuje", "W drodze" },
                DisplayIndex = 1,
                ReadOnly = false
            };

            dgvParcels.Columns.Add(statusComboColumn);

            dgvParcels.DataError += (s, e) =>
            {
                if (e.Exception != null && e.Context == DataGridViewDataErrorContexts.Commit)
                {
                    MessageBox.Show("Wartoœæ w kolumnie Status jest nieprawid³owa.", "B³¹d danych",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.ThrowException = false;
                }
            };
        }

        private void LoadUsersData()
        {
            try
            {
                usersList = new List<Users>();

                using (var connection = new MySqlConnection(GetConnectionString()))
                {
                    connection.Open();
                    var query = "SELECT UserID, Name, PhoneNumber, Role FROM Users";

                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usersList.Add(new Users
                            {
                                UserID = reader.GetInt32("UserID"),
                                Name = reader.GetString("Name"),
                                PhoneNumber = reader.GetString("PhoneNumber"),
                                Role = reader.GetString("Role")
                            });
                        }
                    }
                }

                dgvUsers.Columns.Clear();

                dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "UserID",
                    HeaderText = "ID",
                    DataPropertyName = "UserID",
                    ReadOnly = true
                });

                dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Name",
                    HeaderText = "Nazwa",
                    DataPropertyName = "Name",
                    ReadOnly = true
                });

                dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "PhoneNumber",
                    HeaderText = "Telefon",
                    DataPropertyName = "PhoneNumber",
                    ReadOnly = true
                });

                dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Role",
                    HeaderText = "Rola",
                    DataPropertyName = "Role",
                    ReadOnly = true
                });

                dgvUsers.DataSource = usersList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"B³¹d podczas ³adowania u¿ytkowników:\n{ex.Message}", "B³¹d", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetConnectionString()
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["CourierDBConnection"]?.ConnectionString;
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new Exception("Brak zdefiniowanego connection stringa w pliku konfiguracyjnym.");
                }
                return connectionString;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"B³¹d podczas odczytu connection stringa: {ex.Message}", "B³¹d", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void SaveChanges(bool isUsersTab = false)
        {
            try
            {
                using (var connection = new MySqlConnection(GetConnectionString()))
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            if (isUsersTab)
                            {
                                foreach (var user in usersList)
                                {
                                    if (user.UserID == 0) 
                                    {
                                        using (var command = new MySqlCommand(
                                            "INSERT INTO Users (Name, PhoneNumber, Role) VALUES (@Name, @PhoneNumber, @Role); SELECT LAST_INSERT_ID();",
                                            connection, transaction))
                                        {
                                            command.Parameters.AddWithValue("@Name", user.Name);
                                            command.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                                            command.Parameters.AddWithValue("@Role", user.Role);
                                            user.UserID = Convert.ToInt32(command.ExecuteScalar());
                                        }
                                    }
                                    else 
                                    {
                                        using (var command = new MySqlCommand(
                                            "UPDATE Users SET Name = @Name, PhoneNumber = @PhoneNumber, Role = @Role WHERE UserID = @UserID",
                                            connection, transaction))
                                        {
                                            command.Parameters.AddWithValue("@Name", user.Name);
                                            command.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                                            command.Parameters.AddWithValue("@Role", user.Role);
                                            command.Parameters.AddWithValue("@UserID", user.UserID);
                                            command.ExecuteNonQuery();
                                        }
                                    }
                                }

                               
                                var deletedUsers = usersList.Where(u => u.UserID < 0).ToList();
                                foreach (var user in deletedUsers)
                                {
                                    using (var command = new MySqlCommand(
                                        "DELETE FROM Users WHERE UserID = @UserID",
                                        connection, transaction))
                                    {
                                        command.Parameters.AddWithValue("@UserID", -user.UserID);
                                        command.ExecuteNonQuery();
                                    }
                                }
                            }
                            else
                            {
                                foreach (var package in packagesList)
                                {
                                    if (package.PackageID == 0) 
                                    {
                                        using (var command = new MySqlCommand(
                                            "INSERT INTO Packages (Status, Address, City, ClientID, CourierID) VALUES (@Status, @Address, @City, @ClientID, @CourierID); SELECT LAST_INSERT_ID();",
                                            connection, transaction))
                                        {
                                            command.Parameters.AddWithValue("@Status", package.Status);
                                            command.Parameters.AddWithValue("@Address", package.Address);
                                            command.Parameters.AddWithValue("@City", package.City);
                                            command.Parameters.AddWithValue("@ClientID", package.ClientID);
                                            command.Parameters.AddWithValue("@CourierID", package.CourierID ?? (object)DBNull.Value);
                                            package.PackageID = Convert.ToInt32(command.ExecuteScalar());
                                        }
                                    }
                                    else 
                                    {
                                        using (var command = new MySqlCommand(
                                            "UPDATE Packages SET Status = @Status, Address = @Address, City = @City, CourierID = @CourierID WHERE PackageID = @PackageID",
                                            connection, transaction))
                                        {
                                            command.Parameters.AddWithValue("@Status", package.Status);
                                            command.Parameters.AddWithValue("@Address", package.Address);
                                            command.Parameters.AddWithValue("@City", package.City);
                                            command.Parameters.AddWithValue("@CourierID", package.CourierID ?? (object)DBNull.Value);
                                            command.Parameters.AddWithValue("@PackageID", package.PackageID);
                                            command.ExecuteNonQuery();
                                        }
                                    }
                                }

                               
                                var deletedPackages = packagesList.Where(p => p.PackageID < 0).ToList();
                                foreach (var package in deletedPackages)
                                {
                                    using (var command = new MySqlCommand(
                                        "DELETE FROM Packages WHERE PackageID = @PackageID",
                                        connection, transaction))
                                    {
                                        command.Parameters.AddWithValue("@PackageID", -package.PackageID);
                                        command.ExecuteNonQuery();
                                    }
                                }
                            }

                            transaction.Commit();
                            MessageBox.Show("Zmiany zosta³y zapisane.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            if (isUsersTab)
                                LoadUsersData();
                            else
                                LoadPackagesData();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show($"B³¹d podczas zapisywania zmian:\n{ex.Message}", "B³¹d", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"B³¹d po³¹czenia z baz¹ danych:\n{ex.Message}", "B³¹d", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            SaveChanges(tabControl1.SelectedTab == tabPageUsers);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPagePackages)
            {
                if (dgvParcels.SelectedRows.Count > 0 && !dgvParcels.SelectedRows[0].IsNewRow)
                {
                    if (MessageBox.Show("Czy na pewno chcesz usun¹æ wybran¹ przesy³kê?",
                        "Potwierdzenie", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        var selectedPackage = (Package)dgvParcels.SelectedRows[0].DataBoundItem;

                        packagesList.Remove(selectedPackage);
                        dgvParcels.DataSource = null;
                        dgvParcels.DataSource = packagesList;

                        DeletePackageFromDatabase(selectedPackage.PackageID);
                    }
                }
            }
            else
            {
                if (dgvUsers.SelectedRows.Count > 0 && !dgvUsers.SelectedRows[0].IsNewRow)
                {
                    if (MessageBox.Show("Czy na pewno chcesz usun¹æ wybranego u¿ytkownika?",
                        "Potwierdzenie", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        var selectedUser = (Users)dgvUsers.SelectedRows[0].DataBoundItem;

  
                        usersList.Remove(selectedUser);
                        dgvUsers.DataSource = null;
                        dgvUsers.DataSource = usersList;

       
                        DeleteUserFromDatabase(selectedUser.UserID);
                    }
                }
            }
        }

        private void DeletePackageFromDatabase(int packageId)
        {
            try
            {
                using (var connection = new MySqlConnection(GetConnectionString()))
                {
                    connection.Open();
                    using (var command = new MySqlCommand(
                        "DELETE FROM Packages WHERE PackageID = @PackageID",
                        connection))
                    {
                        command.Parameters.AddWithValue("@PackageID", packageId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"B³¹d podczas usuwania paczki:\n{ex.Message}", "B³¹d", MessageBoxButtons.OK, MessageBoxIcon.Error);

                LoadPackagesData();
            }
        }

        private void DeleteUserFromDatabase(int userId)
        {
            try
            {
                using (var connection = new MySqlConnection(GetConnectionString()))
                {
                    connection.Open();
                    using (var command = new MySqlCommand(
                        "DELETE FROM Users WHERE UserID = @UserID",
                        connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"B³¹d podczas usuwania u¿ytkownika:\n{ex.Message}", "B³¹d", MessageBoxButtons.OK, MessageBoxIcon.Error);
            
                LoadUsersData();
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPagePackages)
                LoadPackagesData();
            else
                LoadUsersData();
        }

        private void AdminDashboardForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                bool hasChanges = packagesList.Any(p => p.PackageID <= 0) ||
                                 usersList.Any(u => u.UserID <= 0);

                if (hasChanges)
                {
                    var result = MessageBox.Show("Istniej¹ niezapisane zmiany. Czy chcesz je zapisaæ?",
                        "Niezapisane zmiany", MessageBoxButtons.YesNoCancel);

                    if (result == DialogResult.Yes)
                    {
                        SaveChanges(tabControl1.SelectedTab == tabPageUsers);
                        ShowLoginForm();
                    }
                    else if (result == DialogResult.No)
                    {
                        ShowLoginForm();
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
                else
                {
                    ShowLoginForm();
                }
            }
        }

        private void ShowLoginForm()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(ShowLoginForm));
            }
            else
            {
                new LoginForm().Show();
            }
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Czy na pewno chcesz siê wylogowaæ?",
                "Potwierdzenie", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}