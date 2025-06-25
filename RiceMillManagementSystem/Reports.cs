using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Xml;

namespace RiceMillManagementSystem
{
    public partial class Reports : Form
    {
        private readonly string connectionString = "Data Source=Afridi;Initial Catalog=ricemillmanagement;Integrated Security=True;TrustServerCertificate=True";
        private readonly int userId;

        public Reports(int userId)
        {
            InitializeComponent();
            this.userId = userId;
        }

        private void Reports_Load(object sender, EventArgs e)
        {
            LoadUserData();
            btnReports.Enabled = false;

            // Populate Report Types
            cmbReportType.Items.Add("Sales Report");
            cmbReportType.Items.Add("Purchase Report");
            cmbReportType.Items.Add("Inventory Status");
            cmbReportType.Items.Add("Customer Ledger");
        }

        private void LoadUserData()
        {
            // Code to load user's name from DB
        }

        private void cmbReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedReport = cmbReportType.SelectedItem.ToString();

            // Hide all filter panels initially
            pnlDateFilters.Visible = false;
            pnlEntityFilter.Visible = false;

            // Show filters based on report type
            switch (selectedReport)
            {
                case "Sales Report":
                case "Purchase Report":
                    pnlDateFilters.Visible = true;
                    break;
                case "Customer Ledger":
                    pnlDateFilters.Visible = true;
                    pnlEntityFilter.Visible = true;
                    lblEntity.Text = "Customer:";
                    PopulateEntityComboBox("SELECT CustomerID, Name FROM Customers", "Name", "CustomerID");
                    break;
                case "Inventory Status":
                    // No filters needed
                    break;
            }
        }

        private void PopulateEntityComboBox(string query, string displayMember, string valueMember)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlDataAdapter sda = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    cmbEntity.DataSource = dt;
                    cmbEntity.DisplayMember = displayMember;
                    cmbEntity.ValueMember = valueMember;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error populating selection list: " + ex.Message);
                }
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (cmbReportType.SelectedItem == null)
            {
                MessageBox.Show("Please select a report type.");
                return;
            }
            string selectedReport = cmbReportType.SelectedItem.ToString();

            switch (selectedReport)
            {
                case "Sales Report":
                    GenerateSalesReport();
                    break;
                case "Purchase Report":
                    GeneratePurchaseReport();
                    break;
                case "Inventory Status":
                    GenerateInventoryReport();
                    break;
                case "Customer Ledger":
                    if (cmbEntity.SelectedValue == null) { MessageBox.Show("Please select a customer."); return; }
                    GenerateCustomerLedger();
                    break;
            }
        }

        private DataTable GetReportData(string query, Action<SqlCommand> addParameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    addParameters?.Invoke(cmd);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error generating report: " + ex.Message);
                    return null;
                }
            }
        }

        private void GenerateSalesReport()
        {
            lblReportTitle.Text = $"Sales Report ({dtpStartDate.Value:d} to {dtpEndDate.Value:d})";
            string query = @"SELECT s.InvoiceNumber, c.Name AS Customer, i.ItemName, s.Quantity, s.TotalAmount, s.SaleDate 
                           FROM Sales s
                           JOIN Customers c ON s.CustomerID = c.CustomerID
                           JOIN Inventory i ON s.ItemID = i.ItemID
                           WHERE s.SaleDate BETWEEN @StartDate AND @EndDate ORDER BY s.SaleDate";

            dgvReportData.DataSource = GetReportData(query, cmd => {
                cmd.Parameters.AddWithValue("@StartDate", dtpStartDate.Value);
                cmd.Parameters.AddWithValue("@EndDate", dtpEndDate.Value);
            });
        }

        private void GeneratePurchaseReport()
        {
            lblReportTitle.Text = $"Purchase Report ({dtpStartDate.Value:d} to {dtpEndDate.Value:d})";
            string query = @"SELECT p.PurchaseDate, s.Name AS Supplier, i.ItemName, p.Quantity, p.UnitPrice, p.TotalAmount 
                           FROM Purchases p
                           JOIN Suppliers s ON p.SupplierID = s.SupplierID
                           JOIN Inventory i ON p.ItemID = i.ItemID
                           WHERE p.PurchaseDate BETWEEN @StartDate AND @EndDate ORDER BY p.PurchaseDate";

            dgvReportData.DataSource = GetReportData(query, cmd => {
                cmd.Parameters.AddWithValue("@StartDate", dtpStartDate.Value);
                cmd.Parameters.AddWithValue("@EndDate", dtpEndDate.Value);
            });
        }

        private void GenerateInventoryReport()
        {
            lblReportTitle.Text = "Current Inventory Status";
            string query = "SELECT ItemName, Quantity, Unit, MinimumStockLevel, LastUpdated FROM Inventory";
            dgvReportData.DataSource = GetReportData(query, null);
        }

        private void GenerateCustomerLedger()
        {
            if (!(cmbEntity.SelectedItem is DataRowView drv)) return;
            int customerId = Convert.ToInt32(drv["CustomerID"]);
            string customerName = drv["Name"].ToString();

            lblReportTitle.Text = $"Ledger for {customerName} ({dtpStartDate.Value:d} to {dtpEndDate.Value:d})";
            string query = @"
                SELECT Date, Particulars, Debit, Credit FROM (
                    SELECT SaleDate AS Date, 'Invoice # ' + InvoiceNumber AS Particulars, TotalAmount AS Debit, 0 AS Credit FROM Sales
                    WHERE CustomerID = @CustomerID AND SaleDate BETWEEN @StartDate AND @EndDate
                    UNION ALL
                    SELECT PaymentDate AS Date, 'Payment - ' + PaymentMethod AS Particulars, 0 AS Debit, Amount AS Credit FROM Payments
                    WHERE CustomerID = @CustomerID AND PaymentDate BETWEEN @StartDate AND @EndDate
                ) AS Ledger
                ORDER BY Date";

            DataTable ledgerData = GetReportData(query, cmd => {
                cmd.Parameters.AddWithValue("@CustomerID", customerId);
                cmd.Parameters.AddWithValue("@StartDate", dtpStartDate.Value);
                cmd.Parameters.AddWithValue("@EndDate", dtpEndDate.Value);
            });

            // Add running balance column
            ledgerData.Columns.Add("Balance", typeof(decimal));
            decimal balance = 0;
            foreach (DataRow row in ledgerData.Rows)
            {
                balance += Convert.ToDecimal(row["Debit"]) - Convert.ToDecimal(row["Credit"]);
                row["Balance"] = balance;
            }

            dgvReportData.DataSource = ledgerData;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dgvReportData.Rows.Count > 0)
            {
                PrintPreviewDialog printPreview = new PrintPreviewDialog();
                printPreview.Document = printDocument1;
                printPreview.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please generate a report before printing.");
            }
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            // This is a basic print logic. You can enhance it significantly.
            Graphics g = e.Graphics;
            Font font = new Font("Arial", 10);
            float fontHeight = font.GetHeight();
            int startX = 50;
            int startY = 50;
            int offset = 20;

            g.DrawString(lblReportTitle.Text, new Font("Arial", 16, FontStyle.Bold), Brushes.Black, startX, startY);
            startY += 50;

            // Print Headers
            for (int i = 0; i < dgvReportData.Columns.Count; i++)
            {
                g.DrawString(dgvReportData.Columns[i].HeaderText, new Font(font, FontStyle.Bold), Brushes.Black, startX + (i * 110), startY);
            }
            startY += offset;

            // Print Rows
            for (int i = 0; i < dgvReportData.Rows.Count; i++)
            {
                for (int j = 0; j < dgvReportData.Columns.Count; j++)
                {
                    object val = dgvReportData.Rows[i].Cells[j].Value;
                    string text = val is DateTime ? ((DateTime)val).ToShortDateString() : val.ToString();
                    g.DrawString(text, font, Brushes.Black, startX + (j * 110), startY);
                }
                startY += offset;
            }
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

        private void btnPurchases_Click(object sender, EventArgs e)
        {
            Purchases purchasesF = new Purchases(userId);
            purchasesF.Show();
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
            Payments paymentsF = new Payments(userId);
            paymentsF.Show();
            this.Hide();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You are in Report Page!");
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