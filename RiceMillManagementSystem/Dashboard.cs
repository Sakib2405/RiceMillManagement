using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace RiceMillManagementSystem
{
    public partial class Dashboard : Form
    {
        private readonly int userId;
        private readonly string connectionString = "Data Source=Afridi;Initial Catalog=ricemillmanagement;Integrated Security=True;TrustServerCertificate=True";

        public Dashboard(int userId)
        {
            InitializeComponent();
            this.userId = userId;
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            LoadUserData();
            LoadDashboardMetrics();
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

        private void LoadDashboardMetrics()
        {
            LoadTotalSalesToday();
            LoadTotalPurchasesMonth();
            LoadTotalCustomers();
            LoadTotalSuppliers();
            LoadSalesChart();
            LoadRecentSales();
            LoadLowStockItems();
        }

        private object ExecuteScalar(string query)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    return cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database query error: " + ex.Message);
                    return null;
                }
            }
        }

        private DataTable ExecuteDataTable(string query)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database query error: " + ex.Message);
                    return new DataTable();
                }
            }
        }


        private void LoadTotalSalesToday()
        {
            string query = $"SELECT ISNULL(SUM(TotalAmount), 0) FROM Sales WHERE CONVERT(date, SaleDate) = CONVERT(date, GETDATE())";
            object result = ExecuteScalar(query);
            lblTotalSales.Text = Convert.ToDecimal(result).ToString("0.00");
        }

        private void LoadTotalPurchasesMonth()
        {
            string query = $"SELECT ISNULL(SUM(TotalAmount), 0) FROM Purchases WHERE MONTH(PurchaseDate) = MONTH(GETDATE()) AND YEAR(PurchaseDate) = YEAR(GETDATE())";
            object result = ExecuteScalar(query);
            lblTotalPurchases.Text = Convert.ToDecimal(result).ToString("0.00");
        }

        private void LoadTotalCustomers()
        {
            string query = "SELECT COUNT(CustomerID) FROM Customers";
            object result = ExecuteScalar(query);
            lblTotalCustomers.Text = result.ToString();
        }

        private void LoadTotalSuppliers()
        {
            string query = "SELECT COUNT(SupplierID) FROM Suppliers";
            object result = ExecuteScalar(query);
            lblTotalSuppliers.Text = result.ToString();
        }

        private void LoadSalesChart()
        {
            string query = @"
                SELECT FORMAT(SaleDate, 'yyyy-MM') AS SaleMonth, SUM(TotalAmount) AS MonthlySales
                FROM Sales
                WHERE SaleDate >= DATEADD(month, -6, GETDATE())
                GROUP BY FORMAT(SaleDate, 'yyyy-MM')
                ORDER BY SaleMonth;";

            DataTable dt = ExecuteDataTable(query);

            salesChart.Series["Sales"].Points.Clear();
            salesChart.Series["Sales"].ChartType = SeriesChartType.Column;

            foreach (DataRow row in dt.Rows)
            {
                salesChart.Series["Sales"].Points.AddXY(row["SaleMonth"].ToString(), Convert.ToDouble(row["MonthlySales"]));
            }
        }

        private void LoadRecentSales()
        {
            string query = @"
                SELECT TOP 10 s.InvoiceNumber, c.Name AS Customer, i.ItemName, s.Quantity, s.TotalAmount, s.SaleDate 
                FROM Sales s
                JOIN Customers c ON s.CustomerID = c.CustomerID
                JOIN Inventory i ON s.ItemID = i.ItemID
                ORDER BY s.SaleDate DESC;";
            dgvRecentSales.DataSource = ExecuteDataTable(query);
        }

        private void LoadLowStockItems()
        {
            string query = @"
                SELECT ItemName, Quantity, MinimumStockLevel 
                FROM Inventory 
                WHERE Quantity <= MinimumStockLevel;";
            dgvLowStock.DataSource = ExecuteDataTable(query);
        }

        private void btnInventory_Click(object sender, EventArgs e)
        {
            Inventory inventoryForm = new Inventory(userId);
            inventoryForm.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
            Login loginForm = new Login();
            loginForm.Show();
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
            Purchases purchasesForm = new Purchases(userId);
            purchasesForm.Show();
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

        private void panelMenu_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You are in Dashboard Page!");
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            Reports reportForm = new Reports(userId);
            reportForm.Show();
            this.Hide();
        }
    }
}