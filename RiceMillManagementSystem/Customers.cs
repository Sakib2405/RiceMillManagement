using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace RiceMillManagementSystem
{
    public partial class Customers : Form
    {
        private readonly string connectionString = "Data Source=Afridi;Initial Catalog=ricemillmanagement;Integrated Security=True;TrustServerCertificate=True";
        private readonly int userId;
        private int selectedCustomerId = 0;

        public Customers(int userId)
        {
            InitializeComponent();
            this.userId = userId;
        }

        private void Customers_Load(object sender, EventArgs e)
        {
            LoadUserData();
            PopulateCustomerGrid();
            btnCustomers.Enabled = false; // Disable the button for the current page
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
                    lblWelcomeUser.Text = "Error";
                }
            }
        }

        private void PopulateCustomerGrid()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT CustomerID, Name, ContactNumber, Address, Email FROM Customers";
                    SqlDataAdapter sda = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    dgvCustomers.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading customer data: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ClearForm()
        {
            txtName.Clear();
            txtContact.Clear();
            txtAddress.Clear();
            txtEmail.Clear();
            selectedCustomerId = 0;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Customer Name is a required field.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Customers (Name, ContactNumber, Address, Email) VALUES (@Name, @ContactNumber, @Address, @Email)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Name", txtName.Text);
                    cmd.Parameters.AddWithValue("@ContactNumber", txtContact.Text);
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    PopulateCustomerGrid();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding customer: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedCustomerId == 0)
            {
                MessageBox.Show("Please select a customer from the list to update.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Customer Name is a required field.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE Customers SET Name = @Name, ContactNumber = @ContactNumber, Address = @Address, Email = @Email WHERE CustomerID = @CustomerID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CustomerID", selectedCustomerId);
                    cmd.Parameters.AddWithValue("@Name", txtName.Text);
                    cmd.Parameters.AddWithValue("@ContactNumber", txtContact.Text);
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    PopulateCustomerGrid();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating customer: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCustomers.Rows[e.RowIndex];
                selectedCustomerId = Convert.ToInt32(row.Cells["CustomerID"].Value);
                txtName.Text = row.Cells["Name"].Value.ToString();
                txtContact.Text = row.Cells["ContactNumber"].Value.ToString();
                txtAddress.Text = row.Cells["Address"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintPreviewDialog printPreview = new PrintPreviewDialog();
            printPreview.Document = printDocument1;
            printPreview.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Font titleFont = new Font("Arial", 18, FontStyle.Bold);
            Font headerFont = new Font("Arial", 12, FontStyle.Bold);
            Font bodyFont = new Font("Arial", 10);

            float yPos = 50;
            int startX = 50;

            g.DrawString("Customer List", titleFont, Brushes.Black, startX, yPos);
            yPos += 50;

            g.DrawString("ID".PadRight(5), headerFont, Brushes.Black, startX, yPos);
            g.DrawString("Name".PadRight(30), headerFont, Brushes.Black, startX + 50, yPos);
            g.DrawString("Contact".PadRight(20), headerFont, Brushes.Black, startX + 250, yPos);
            g.DrawString("Email".PadRight(30), headerFont, Brushes.Black, startX + 400, yPos);
            g.DrawString("Address", headerFont, Brushes.Black, startX + 600, yPos);
            yPos += 30;

            foreach (DataGridViewRow row in dgvCustomers.Rows)
            {
                if (row.IsNewRow) continue;

                string id = (row.Cells["CustomerID"].Value?.ToString() ?? "").PadRight(5);
                string name = (row.Cells["Name"].Value?.ToString() ?? "").PadRight(30);
                string contact = (row.Cells["ContactNumber"].Value?.ToString() ?? "").PadRight(20);
                string email = (row.Cells["Email"].Value?.ToString() ?? "").PadRight(30);
                string address = row.Cells["Address"].Value?.ToString() ?? "";

                g.DrawString(id, bodyFont, Brushes.Black, startX, yPos);
                g.DrawString(name, bodyFont, Brushes.Black, startX + 50, yPos);
                g.DrawString(contact, bodyFont, Brushes.Black, startX + 250, yPos);
                g.DrawString(email, bodyFont, Brushes.Black, startX + 400, yPos);
                g.DrawString(address, bodyFont, Brushes.Black, startX + 600, yPos);
                yPos += 20;
            }
        }

        // --- Navigation Button Clicks ---

        private void btnInventory_Click(object sender, EventArgs e)
        {
            // Assuming Inventory form also takes userId for consistency
            Inventory inventoryForm = new Inventory(this.userId);
            inventoryForm.Show();
            this.Hide();
            // MessageBox.Show("Inventory form would open here.");
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
            Login loginForm = new Login();
            loginForm.Show();
        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void panelMenu_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard(userId);
            dashboard.Show();
            this.Hide();
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Customers Page Would Open Here!");
            
        }

        private void btnSuppliers_Click(object sender, EventArgs e)
        {
            Suppliers suppliersForm = new Suppliers(userId);
            suppliersForm.Show();
            this.Hide();
        }

        private void btnPurchases_Click(object sender, EventArgs e)
        {
            Purchases pur = new Purchases(userId);
            pur.Show();
            this.Hide();
        }

        private void btnSales_Click(object sender, EventArgs e)
        {
            Sales salesForm = new Sales(userId);
            salesForm.Show();
            this.Hide();
        }

        private void btnPayments_Click(object sender, EventArgs e)
        {
            Payments paymentsForm = new Payments(userId);
            paymentsForm.Show();
            this.Hide();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            Reports reportsForm = new Reports(userId);
            reportsForm.Show();
            this.Hide();
        }
    }
}