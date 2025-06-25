using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Xml.Linq;

namespace RiceMillManagementSystem
{
    public partial class Suppliers : Form
    {
        private readonly string connectionString = "Data Source=Afridi;Initial Catalog=ricemillmanagement;Integrated Security=True;TrustServerCertificate=True";
        private readonly int userId;
        private int selectedSupplierId = 0;

        public Suppliers(int userId)
        {
            InitializeComponent();
            this.userId = userId;
        }

        private void Suppliers_Load(object sender, EventArgs e)
        {
            LoadUserData();
            PopulateSupplierGrid();
            btnSuppliers.Enabled = false; // Disable button for the current page
        }

        private void LoadUserData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT Name FROM Users WHERE UserID=@userId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    string name = cmd.ExecuteScalar()?.ToString();
                    lblWelcomeUser.Text = !string.IsNullOrEmpty(name) ? name : "User Not Found";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading user data: " + ex.Message);
                }
            }
        }

        private void PopulateSupplierGrid(string searchTerm = "")
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT SupplierID, Name, ContactNumber, Address, Email FROM Suppliers";
                    if (!string.IsNullOrWhiteSpace(searchTerm))
                    {
                        query += " WHERE Name LIKE @SearchTerm";
                    }

                    SqlDataAdapter sda = new SqlDataAdapter(query, conn);
                    if (!string.IsNullOrWhiteSpace(searchTerm))
                    {
                        sda.SelectCommand.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
                    }

                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    dgvSuppliers.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading suppliers: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ClearForm()
        {
            txtName.Clear();
            txtContact.Clear();
            txtAddress.Clear();
            txtEmail.Clear();
            txtSearch.Clear();
            selectedSupplierId = 0;
            dgvPurchaseHistory.DataSource = null;
            lblTotalBusiness.Text = "0.00";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Supplier Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Suppliers (Name, ContactNumber, Address, Email) VALUES (@Name, @Contact, @Address, @Email)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Name", txtName.Text);
                    cmd.Parameters.AddWithValue("@Contact", txtContact.Text);
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Supplier added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    PopulateSupplierGrid();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding supplier: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedSupplierId == 0)
            {
                MessageBox.Show("Please select a supplier to update.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Add other validations as needed...

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE Suppliers SET Name=@Name, ContactNumber=@Contact, Address=@Address, Email=@Email WHERE SupplierID=@ID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", selectedSupplierId);
                    cmd.Parameters.AddWithValue("@Name", txtName.Text);
                    cmd.Parameters.AddWithValue("@Contact", txtContact.Text);
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Supplier updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    PopulateSupplierGrid();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating supplier: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
            PopulateSupplierGrid(); // Reset grid if search was active
        }

        private void dgvSuppliers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSuppliers.Rows[e.RowIndex];
                selectedSupplierId = Convert.ToInt32(row.Cells["SupplierID"].Value);
                txtName.Text = row.Cells["Name"].Value.ToString();
                txtContact.Text = row.Cells["ContactNumber"].Value.ToString();
                txtAddress.Text = row.Cells["Address"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();

                LoadPurchaseHistory(selectedSupplierId);
            }
        }

        private void LoadPurchaseHistory(int supplierId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Load purchase grid
                    string historyQuery = @"SELECT p.PurchaseDate, i.ItemName, p.Quantity, p.UnitPrice, p.TotalAmount 
                                           FROM Purchases p 
                                           JOIN Inventory i ON p.ItemID = i.ItemID 
                                           WHERE p.SupplierID = @SupplierID ORDER BY p.PurchaseDate DESC";
                    SqlDataAdapter sda = new SqlDataAdapter(historyQuery, conn);
                    sda.SelectCommand.Parameters.AddWithValue("@SupplierID", supplierId);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    dgvPurchaseHistory.DataSource = dt;

                    // Load total business
                    string totalQuery = "SELECT ISNULL(SUM(TotalAmount), 0) FROM Purchases WHERE SupplierID = @SupplierID";
                    SqlCommand cmd = new SqlCommand(totalQuery, conn);
                    cmd.Parameters.AddWithValue("@SupplierID", supplierId);
                    object result = cmd.ExecuteScalar();
                    lblTotalBusiness.Text = Convert.ToDecimal(result).ToString("C"); // "C" for currency format
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading purchase history: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            PopulateSupplierGrid(txtSearch.Text);
        }

        // --- Printing and Navigation ---

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintPreviewDialog printPreview = new PrintPreviewDialog();
            printPreview.Document = printDocument1;
            printPreview.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            // This is a basic print implementation. You can customize it further.
            Graphics g = e.Graphics;
            Font font = new Font("Arial", 10);
            float fontHeight = font.GetHeight();
            int startX = 50;
            int startY = 50;
            int offset = 40;

            g.DrawString("Supplier List", new Font("Arial", 16, FontStyle.Bold), Brushes.Black, startX, startY);

            foreach (DataGridViewRow row in dgvSuppliers.Rows)
            {
                if (row.IsNewRow) continue;
                string line = $"ID: {row.Cells["SupplierID"].Value}, Name: {row.Cells["Name"].Value}, Contact: {row.Cells["ContactNumber"].Value}";
                g.DrawString(line, font, Brushes.Black, startX, startY + offset);
                offset += (int)fontHeight + 5;
            }
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            Customers customersForm = new Customers(this.userId);
            customersForm.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
            Login loginForm = new Login();
            loginForm.Show();
        }

        private void panelMenu_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dashboard dashboardForm = new Dashboard(userId);
            dashboardForm.Show();
            this.Hide();
        }

        private void btnInventory_Click(object sender, EventArgs e)
        {
            Inventory inventoryForm = new Inventory(this.userId);
            inventoryForm.Show();   
            this.Hide();    

        }

        private void btnSuppliers_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You are in Suppliers Page!");

        }

        private void btnPurchases_Click(object sender, EventArgs e)
        {
            Purchases pur = new Purchases(this.userId);
            pur.Show();
            this.Hide();
        }

        private void btnSales_Click(object sender, EventArgs e)
        {
            Sales sales = new Sales(this.userId);
            sales.Show();
            this.Hide();
        }

        private void btnPayments_Click(object sender, EventArgs e)
        {
            Payments payments = new Payments(this.userId);
            payments.Show();
            this.Hide();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            Reports reports = new Reports(this.userId);
            reports.Show();
            this.Hide();
        }
    }
}