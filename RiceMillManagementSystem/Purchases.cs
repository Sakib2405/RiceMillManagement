using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace RiceMillManagementSystem
{
    public partial class Purchases : Form
    {
        private readonly string connectionString = "Data Source=Afridi;Initial Catalog=ricemillmanagement;Integrated Security=True;TrustServerCertificate=True";
        private readonly int userId;

        public Purchases(int userId)
        {
            InitializeComponent();
            this.userId = userId;
        }

        private void Purchases_Load(object sender, EventArgs e)
        {
            LoadUserData();
            PopulateComboBoxes();
            PopulatePurchasesGrid();
            btnPurchases.Enabled = false; // Disable self
        }

        private void LoadUserData()
        {
            // Code to load user's name into lblWelcomeUser (same as other forms)
        }

        private void PopulateComboBoxes()
        {
            // Populate Suppliers
            PopulateComboBox(cmbSuppliers, "SELECT SupplierID, Name FROM Suppliers", "Name", "SupplierID");
            // Populate Inventory Items
            PopulateComboBox(cmbItems, "SELECT ItemID, ItemName FROM Inventory", "ItemName", "ItemID");
        }

        private void PopulateComboBox(ComboBox cmb, string query, string displayMember, string valueMember)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlDataAdapter sda = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    cmb.DataSource = dt;
                    cmb.DisplayMember = displayMember;
                    cmb.ValueMember = valueMember;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error populating dropdown: " + ex.Message);
                }
            }
        }

        private void PopulatePurchasesGrid(DateTime? startDate = null, DateTime? endDate = null)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    string query = @"SELECT p.PurchaseID, p.PurchaseDate, s.Name AS Supplier, i.ItemName, p.Quantity, p.UnitPrice, p.TotalAmount 
                                   FROM Purchases p
                                   JOIN Suppliers s ON p.SupplierID = s.SupplierID
                                   JOIN Inventory i ON p.ItemID = i.ItemID";

                    if (startDate.HasValue && endDate.HasValue)
                    {
                        query += " WHERE p.PurchaseDate BETWEEN @StartDate AND @EndDate";
                    }
                    query += " ORDER BY p.PurchaseDate DESC";

                    SqlDataAdapter sda = new SqlDataAdapter(query, conn);
                    if (startDate.HasValue && endDate.HasValue)
                    {
                        sda.SelectCommand.Parameters.AddWithValue("@StartDate", startDate.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@EndDate", endDate.Value);
                    }

                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    dgvPurchases.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading purchase history: " + ex.Message);
                }
            }
        }

        private void CalculateTotal(object sender, EventArgs e)
        {
            decimal quantity = 0;
            decimal unitPrice = 0;

            decimal.TryParse(txtQuantity.Text, out quantity);
            decimal.TryParse(txtUnitPrice.Text, out unitPrice);

            decimal totalAmount = quantity * unitPrice;
            lblTotalAmount.Text = totalAmount.ToString("0.00");
        }

        private void btnRecordPurchase_Click(object sender, EventArgs e)
        {
            // --- Validations ---
            if (cmbSuppliers.SelectedValue == null || cmbItems.SelectedValue == null)
            {
                MessageBox.Show("Please select a supplier and an item.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!decimal.TryParse(txtQuantity.Text, out decimal quantity) || quantity <= 0)
            {
                MessageBox.Show("Please enter a valid quantity.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!decimal.TryParse(txtUnitPrice.Text, out decimal unitPrice) || unitPrice < 0)
            {
                MessageBox.Show("Please enter a valid unit price.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // --- Database Transaction ---
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // 1. Insert into Purchases table
                    string purchaseQuery = "INSERT INTO Purchases (SupplierID, ItemID, Quantity, UnitPrice, TotalAmount, PurchaseDate) VALUES (@SupplierID, @ItemID, @Quantity, @UnitPrice, @TotalAmount, @PurchaseDate)";
                    SqlCommand purchaseCmd = new SqlCommand(purchaseQuery, conn, transaction);
                    purchaseCmd.Parameters.AddWithValue("@SupplierID", cmbSuppliers.SelectedValue);
                    purchaseCmd.Parameters.AddWithValue("@ItemID", cmbItems.SelectedValue);
                    purchaseCmd.Parameters.AddWithValue("@Quantity", quantity);
                    purchaseCmd.Parameters.AddWithValue("@UnitPrice", unitPrice);
                    purchaseCmd.Parameters.AddWithValue("@TotalAmount", quantity * unitPrice);
                    purchaseCmd.Parameters.AddWithValue("@PurchaseDate", dtpPurchaseDate.Value);
                    purchaseCmd.ExecuteNonQuery();

                    // 2. Update Inventory table
                    string inventoryQuery = "UPDATE Inventory SET Quantity = Quantity + @PurchaseQuantity, LastUpdated = GETDATE() WHERE ItemID = @ItemID";
                    SqlCommand inventoryCmd = new SqlCommand(inventoryQuery, conn, transaction);
                    inventoryCmd.Parameters.AddWithValue("@PurchaseQuantity", quantity);
                    inventoryCmd.Parameters.AddWithValue("@ItemID", cmbItems.SelectedValue);
                    inventoryCmd.ExecuteNonQuery();

                    // If both commands succeed, commit the transaction
                    transaction.Commit();
                    MessageBox.Show("Purchase recorded and stock updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    // If anything fails, roll back the entire transaction
                    transaction.Rollback();
                    MessageBox.Show("An error occurred. The transaction was rolled back. Error: " + ex.Message, "Transaction Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // Refresh grid and clear form
            PopulatePurchasesGrid();
            txtQuantity.Clear();
            txtUnitPrice.Clear();
            lblTotalAmount.Text = "0.00";
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            PopulatePurchasesGrid(dtpStartDate.Value, dtpEndDate.Value);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintPreviewDialog printPreview = new PrintPreviewDialog();
            printPreview.Document = printDocument1;
            printPreview.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Basic printing logic - customize as needed
            Graphics g = e.Graphics;
            Font font = new Font("Arial", 10);
            float fontHeight = font.GetHeight();
            int startX = 50;
            int startY = 50;
            int offset = 40;

            g.DrawString("Purchase History", new Font("Arial", 16, FontStyle.Bold), Brushes.Black, startX, startY);

            foreach (DataGridViewRow row in dgvPurchases.Rows)
            {
                if (row.IsNewRow) continue;
                string date = Convert.ToDateTime(row.Cells["PurchaseDate"].Value).ToShortDateString();
                string line = $"Date: {date}, Supplier: {row.Cells["Supplier"].Value}, Item: {row.Cells["ItemName"].Value}, Total: {Convert.ToDecimal(row.Cells["TotalAmount"].Value):C}";
                g.DrawString(line, font, Brushes.Black, startX, startY + offset);
                offset += (int)fontHeight + 5;
            }
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
            Inventory inventoryForm = new Inventory(userId);
            inventoryForm.Show();
            this.Hide();
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            Customers customersForm = new Customers(userId);
            customersForm.Show();
            this.Hide();

        }

        private void btnSuppliers_Click(object sender, EventArgs e)
        {
            Suppliers suppliersForm = new Suppliers(userId);
            suppliersForm.Show();
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

        private void btnPurchases_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You are in Purchases Page!");
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            // Close the current form
            this.Hide();

            // Create a new instance of the Login form and show it
            Login loginForm = new Login();
            loginForm.Show();
        }
    }
}