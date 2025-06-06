using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Courier_Management
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            this.BackgroundImage = Image.FromFile("img/background.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblMessage.Text = "Uzupełnij wszystkie pola.";
                return;
            }

            try
            {
               
                string connectionString = ConfigurationManager.ConnectionStrings["CourierDbConnection"].ConnectionString;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Role, UserID FROM users WHERE Name = @username AND Password = @password";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string role = reader["Role"].ToString();
                                int userId = Convert.ToInt32(reader["UserID"]);

                                MessageBox.Show($"Zalogowano pomyślnie jako: {role}", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                if (role == "Admin")
                                {
                                    new AdminDashboardForm().Show();
                                }
                                else if (role == "Courier")
                                {
                                    var courierDashboard = new CourierDashboardForm(userId);   
                                    courierDashboard.Show();
                                }
                                else if (role == "Client")
                                {
                                    var clientDashboard = new ClientDashboardForm(userId);
                                    clientDashboard.Show();
                                }

                                this.Hide();
                            }
                            else
                            {
                                lblMessage.Text = "Nieprawidłowa nazwa użytkownika lub hasło.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
