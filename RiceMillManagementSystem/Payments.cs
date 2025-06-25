using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace RiceMillManagementSystem
{
    public partial class Payments : Form
    {
        private readonly string connectionString = "Data Source=Afridi;Initial Catalog=ricemillmanagement;Integrated Security=True;TrustServerCertificate=True";
        private readonly int userId;
        private int selectedSaleId = 0;
        private decimal balanceDueForSelectedSale = 0;
        private Dictionary<string, string> paymentToPrint = new Dictionary<string, string>();


        public Payments(int userId)
        {
            InitializeComponent();
            this.userId = userId;
        }

        private void Payments_Load(object sender, EventArgs e)
        {
            LoadUserData();
            PopulateCustomerComboBox();
            btnPayments.Enabled = false;
        }

        private void LoadUserData()
        {
            // Code to load user's name, same as other forms
        }

        private void PopulateCustomerComboBox()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT CustomerID, Name FROM Customers ORDER BY Name", conn);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    cmbCustomers.DataSource = dt;
                    cmbCustomers.DisplayMember = "Name";
                    cmbCustomers.ValueMember = "CustomerID";
                    cmbCustomers.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error populating customers: " + ex.Message);
                }
            }
        }

        private void cmbCustomers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCustomers.SelectedValue == null) return;

            DataRowView drv = (DataRowView)cmbCustomers.SelectedItem;
            int customerId = Convert.ToInt32(drv["CustomerID"]);
            LoadCustomerSalesAndPayments(customerId);
        }

        private void LoadCustomerSalesAndPayments(int customerId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    string query = @"
                        SELECT 
                            s.SaleID, s.InvoiceNumber, s.SaleDate, s.TotalAmount,
                            ISNULL((SELECT SUM(Amount) FROM Payments p WHERE p.SaleID = s.SaleID), 0) AS AmountPaid,
                            s.TotalAmount - ISNULL((SELECT SUM(Amount) FROM Payments p WHERE p.SaleID = s.SaleID), 0) AS BalanceDue
                        FROM Sales s
                        WHERE s.CustomerID = @CustomerID
                        ORDER BY s.SaleDate DESC";

                    SqlDataAdapter sda = new SqlDataAdapter(query, conn);
                    sda.SelectCommand.Parameters.AddWithValue("@CustomerID", customerId);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    dgvCustomerSales.DataSource = dt;

                    // Calculate total outstanding
                    decimal totalOutstanding = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        totalOutstanding += Convert.ToDecimal(row["BalanceDue"]);
                    }
                    lblTotalOutstanding.Text = totalOutstanding.ToString("C"); // "C" for currency
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading customer sales data: " + ex.Message);
                }
            }
        }

        private void dgvCustomerSales_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCustomerSales.Rows[e.RowIndex];
                selectedSaleId = Convert.ToInt32(row.Cells["SaleID"].Value);
                balanceDueForSelectedSale = Convert.ToDecimal(row.Cells["BalanceDue"].Value);

                lblSelectedInvoice.Text = row.Cells["InvoiceNumber"].Value.ToString();
                txtPaymentAmount.Text = balanceDueForSelectedSale.ToString("0.00");

                btnRecordPayment.Enabled = true;
            }
        }

        private void btnRecordPayment_Click(object sender, EventArgs e)
        {
            if (selectedSaleId == 0) { MessageBox.Show("Please select an invoice from the list."); return; }
            if (!decimal.TryParse(txtPaymentAmount.Text, out decimal paymentAmount) || paymentAmount <= 0) { MessageBox.Show("Please enter a valid payment amount."); return; }
            if (paymentAmount > balanceDueForSelectedSale) { MessageBox.Show("Payment amount cannot be greater than the balance due."); return; }
            if (cmbPaymentMethod.SelectedItem == null) { MessageBox.Show("Please select a payment method."); return; }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Payments (SaleID, CustomerID, Amount, PaymentDate, PaymentMethod) VALUES (@SaleID, @CustomerID, @Amount, @PaymentDate, @PaymentMethod)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@SaleID", selectedSaleId);
                    cmd.Parameters.AddWithValue("@CustomerID", cmbCustomers.SelectedValue);
                    cmd.Parameters.AddWithValue("@Amount", paymentAmount);
                    cmd.Parameters.AddWithValue("@PaymentDate", dtpPaymentDate.Value);
                    cmd.Parameters.AddWithValue("@PaymentMethod", cmbPaymentMethod.SelectedItem.ToString());
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Payment recorded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Prepare for receipt printing
                    paymentToPrint["Customer"] = cmbCustomers.Text;
                    paymentToPrint["InvoiceNumber"] = lblSelectedInvoice.Text;
                    paymentToPrint["PaymentAmount"] = paymentAmount.ToString("C");
                    paymentToPrint["PaymentDate"] = dtpPaymentDate.Value.ToLongDateString();
                    btnPrintReceipt.Enabled = true;

                    // Refresh and reset
                    LoadCustomerSalesAndPayments(Convert.ToInt32(cmbCustomers.SelectedValue));
                    ClearPaymentForm();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error recording payment: " + ex.Message);
                }
            }
        }

        private void ClearPaymentForm()
        {
            selectedSaleId = 0;
            balanceDueForSelectedSale = 0;
            lblSelectedInvoice.Text = "none";
            txtPaymentAmount.Clear();
            cmbPaymentMethod.SelectedIndex = -1;
            btnRecordPayment.Enabled = false;
        }

        private void btnPrintReceipt_Click(object sender, EventArgs e)
        {
            if (paymentToPrint.Count > 0)
            {
                PrintPreviewDialog printPreview = new PrintPreviewDialog();
                printPreview.Document = printDocument1;
                printPreview.ShowDialog();
            }
            else
            {
                MessageBox.Show("Record a payment or select a previous one to print a receipt.");
            }
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Font titleFont = new Font("Arial", 20, FontStyle.Bold);
            Font headerFont = new Font("Arial", 12, FontStyle.Bold);
            Font bodyFont = new Font("Arial", 12);
            float yPos = 50;
            int startX = 50;
            int offset = 35;

            g.DrawString("Payment Receipt", titleFont, Brushes.Black, startX, yPos);
            yPos += offset * 2;

            g.DrawString("Date: " + paymentToPrint["PaymentDate"], bodyFont, Brushes.Black, startX, yPos);
            yPos += offset * 2;

            g.DrawString("Received From: " + paymentToPrint["Customer"], bodyFont, Brushes.Black, startX, yPos);
            yPos += offset;
            g.DrawString("Amount: " + paymentToPrint["PaymentAmount"], headerFont, Brushes.Black, startX, yPos);
            yPos += offset;
            g.DrawString("For Invoice #: " + paymentToPrint["InvoiceNumber"], bodyFont, Brushes.Black, startX, yPos);
            yPos += offset * 3;

            g.DrawString("Thank you for your business!", bodyFont, Brushes.Black, startX, yPos);
        }

        private void panelMenu_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            // Close the current form
            this.Hide();

            // Create a new instance of the Login form and show it
            Login loginForm = new Login();
            loginForm.Show();
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
            MessageBox.Show("You are in Payments Page!");
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            Reports reportForm = new Reports(userId);
            reportForm.Show();
            this.Hide();
        }
    }
}