namespace Courier_Management
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new Label();
            this.txtUsername = new TextBox();
            this.txtPassword = new TextBox();
            this.lblUsername = new Label();
            this.lblPassword = new Label();
            this.btnLogin = new Button();
            this.btnExit = new Button();
            this.lblMessage = new Label();

            // Ustawienia główne formularza
            this.ClientSize = new Size(800, 600);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Logowanie";

            // Nagłówek aplikacji
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Arial Rounded MT Bold", 28F, FontStyle.Bold, GraphicsUnit.Point);
            this.lblTitle.ForeColor = Color.Orange;
            this.lblTitle.BackColor = Color.Transparent;
            this.lblTitle.Location = new Point(180, 50);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Text = "Courier Management";

            // Etykieta nazwy użytkownika
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            this.lblUsername.ForeColor = Color.Orange;
            this.lblUsername.BackColor = Color.Transparent;
            this.lblUsername.Location = new Point(250, 150);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Text = "Nazwa użytkownika";

            // Pole wprowadzania nazwy użytkownika
            this.txtUsername.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            this.txtUsername.Location = new Point(250, 180);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new Size(300, 29);

            // Etykieta hasła
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            this.lblPassword.ForeColor = Color.Orange;
            this.lblPassword.BackColor = Color.Transparent;
            this.lblPassword.Location = new Point(250, 230);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Text = "Hasło";

            // Pole wprowadzania hasła
            this.txtPassword.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            this.txtPassword.Location = new Point(250, 260);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new Size(300, 29);

            // Przycisk logowania
            this.btnLogin.BackColor = Color.Green;
            this.btnLogin.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            this.btnLogin.ForeColor = Color.White;
            this.btnLogin.Location = new Point(250, 320);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new Size(140, 40);
            this.btnLogin.Text = "Zaloguj";
            this.btnLogin.UseVisualStyleBackColor = false;

            // Przycisk wyjścia
            this.btnExit.BackColor = Color.IndianRed;
            this.btnExit.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            this.btnExit.ForeColor = Color.White;
            this.btnExit.Location = new Point(410, 320);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new Size(140, 40);
            this.btnExit.Text = "Wyjście";
            this.btnExit.UseVisualStyleBackColor = false;

            // Etykieta komunikatów
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblMessage.ForeColor = Color.Red;
            this.lblMessage.BackColor = Color.Transparent;
            this.lblMessage.Location = new Point(250, 380);
            this.lblMessage.Name = "lblMessage";

            // Dodanie kontrolek do formularza
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.lblMessage);

            // Przypisanie zdarzeń
            this.btnLogin.Click += new EventHandler(this.btnLogin_Click);
            this.btnExit.Click += new EventHandler(this.btnExit_Click);
        }

     
        private Label lblTitle;
        private Label lblUsername;
        private Label lblPassword;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnExit;
        private Label lblMessage;
    }
}