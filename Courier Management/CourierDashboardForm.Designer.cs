namespace Courier_Management
{
    partial class CourierDashboardForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvParcels;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnSave; 
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblInfo;

        private void InitializeComponent()
        {
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.dgvParcels = new System.Windows.Forms.DataGridView();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button(); 
            this.lblInfo = new System.Windows.Forms.Label();

            // Panel nag³ówka
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(51, 102, 204);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(784, 60);

            // Tytu³
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(300, 32);
            this.lblTitle.Text = "Przesy³ki przypisane do Ciebie";

            // Tabela przesy³ek
            this.dgvParcels.BackgroundColor = System.Drawing.Color.White;
            this.dgvParcels.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvParcels.ColumnHeadersHeight = 40;
            this.dgvParcels.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvParcels.Location = new System.Drawing.Point(20, 80);
            this.dgvParcels.Name = "dgvParcels";
            this.dgvParcels.RowHeadersVisible = false;
            this.dgvParcels.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvParcels.Size = new System.Drawing.Size(744, 300);

            // Etykieta z informacj¹
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblInfo.ForeColor = System.Drawing.Color.Gray;
            this.lblInfo.Location = new System.Drawing.Point(20, 400);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(400, 15);
            this.lblInfo.Text = "Kliknij na kolumnê 'Status', aby zmieniæ status przesy³ki.";

            // Przycisk Zapisz
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(51, 153, 51);
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(550, 420); 
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 30);
            this.btnSave.Text = "Zapisz";
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);

            // Przycisk wylogowania
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(204, 51, 51);
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(664, 420);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(100, 30);
            this.btnLogout.Text = "Wyloguj";
            this.btnLogout.Click += new System.EventHandler(this.BtnLogout_Click);

            // G³ówne ustawienia formularza
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.dgvParcels);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.btnSave); 
            this.Controls.Add(this.btnLogout);
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Panel Kuriera - System Kurierski";
        }
    }
}
