using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace RiceMillManagementSystem
{
    public partial class Inventory : Form
    {
        private readonly string connectionString = "Data Source=Afridi;Initial Catalog=ricemillmanagement;Integrated Security=True;TrustServerCertificate=True";
        private int selectedItemId = 0;
        private int userId;

        public Inventory(int userId)
        {
            InitializeComponent();
            this.userId = userId;
        }

        private void Inventory_Load(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        private void PopulateGrid()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT ItemID, ItemName, Quantity, Unit, MinimumStockLevel FROM Inventory";
                    SqlDataAdapter sda = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    dataGridViewInventory.DataSource = dt;
                    string query1 = "SELECT Name FROM Users where UserID=@userId";
                    SqlCommand cmd = new SqlCommand(query1, conn);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    string name = cmd.ExecuteScalar()?.ToString();
                    if (name != null)
                    {
                        label6.Text = name;
                    }
                    else
                    {
                        label6.Text = "User name not found!";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading inventory data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ClearForm()
        {
            comboBoxItemName.SelectedIndex = -1;
            comboBoxUnit.SelectedIndex = -1;
            textBoxQuantity.Clear();
            textBoxMinQuantity.Clear();
            selectedItemId = 0;
        }

        private void buttonAddItem_Click(object sender, EventArgs e)
        {
            if (comboBoxItemName.SelectedItem == null || comboBoxUnit.SelectedItem == null || string.IsNullOrWhiteSpace(textBoxQuantity.Text))
            {
                MessageBox.Show("Please fill all the required fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Inventory (ItemName, Quantity, Unit, MinimumStockLevel) VALUES (@ItemName, @Quantity, @Unit, @MinimumStockLevel)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ItemName", comboBoxItemName.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@Quantity", Convert.ToDecimal(textBoxQuantity.Text));
                    cmd.Parameters.AddWithValue("@Unit", comboBoxUnit.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@MinimumStockLevel", string.IsNullOrWhiteSpace(textBoxMinQuantity.Text) ? (object)DBNull.Value : Convert.ToDecimal(textBoxMinQuantity.Text));
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Item added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    PopulateGrid();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding item: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridViewInventory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewInventory.Rows[e.RowIndex];
                selectedItemId = Convert.ToInt32(row.Cells["ItemID"].Value);
                comboBoxItemName.SelectedItem = row.Cells["ItemName"].Value.ToString();
                textBoxQuantity.Text = row.Cells["Quantity"].Value.ToString();
                comboBoxUnit.SelectedItem = row.Cells["Unit"].Value.ToString();
                textBoxMinQuantity.Text = row.Cells["MinimumStockLevel"].Value.ToString();
            }
        }

        private void buttonUpdateItem_Click(object sender, EventArgs e)
        {
            if (selectedItemId == 0)
            {
                MessageBox.Show("Please select an item from the list to update.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBoxItemName.SelectedItem == null || comboBoxUnit.SelectedItem == null || string.IsNullOrWhiteSpace(textBoxQuantity.Text))
            {
                MessageBox.Show("Please fill all the required fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE Inventory SET ItemName = @ItemName, Quantity = @Quantity, Unit = @Unit, MinimumStockLevel = @MinimumStockLevel WHERE ItemID = @ItemID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ItemID", selectedItemId);
                    cmd.Parameters.AddWithValue("@ItemName", comboBoxItemName.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@Quantity", Convert.ToDecimal(textBoxQuantity.Text));
                    cmd.Parameters.AddWithValue("@Unit", comboBoxUnit.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@MinimumStockLevel", string.IsNullOrWhiteSpace(textBoxMinQuantity.Text) ? (object)DBNull.Value : Convert.ToDecimal(textBoxMinQuantity.Text));
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Item updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    PopulateGrid();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating item: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonPrint_Click(object sender, EventArgs e)
        {
            PrintPreviewDialog printPreviewDialog1 = new PrintPreviewDialog();
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Font font = new Font("Courier New", 12);
            float fontHeight = font.GetHeight();
            int startX = 10;
            int startY = 10;
            int offset = 40;

            g.DrawString("Inventory Report", new Font("Courier New", 18, FontStyle.Bold), Brushes.Black, startX, startY);
            offset += 40;

            // Headers
            g.DrawString("ItemID".PadRight(10) + "ItemName".PadRight(20) + "Quantity".PadRight(15) + "Unit".PadRight(15) + "Min Stock", font, Brushes.Black, startX, startY + offset);
            offset += (int)fontHeight + 5;

            // Data
            foreach (DataGridViewRow row in dataGridViewInventory.Rows)
            {
                if (row.IsNewRow) continue;

                string itemID = (row.Cells["ItemID"].Value?.ToString() ?? "").PadRight(10);
                string itemName = (row.Cells["ItemName"].Value?.ToString() ?? "").PadRight(20);
                string quantity = (row.Cells["Quantity"].Value?.ToString() ?? "").PadRight(15);
                string unit = (row.Cells["Unit"].Value?.ToString() ?? "").PadRight(15);
                string minStock = row.Cells["MinimumStockLevel"].Value?.ToString() ?? "";

                g.DrawString(itemID + itemName + quantity + unit + minStock, font, Brushes.Black, startX, startY + offset);
                offset += (int)fontHeight;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dashboard dashboardForm = new Dashboard(userId);
            dashboardForm.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Customers customersForm = new Customers(userId);
            customersForm.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Suppliers suppliersForm = new Suppliers(userId);
            suppliersForm.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Purchases pur= new Purchases(userId);
            pur.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Sales sales = new Sales(userId);
            sales.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Payments payments = new Payments(userId);
            payments.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Reports reports = new Reports(userId);
            reports.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
            Login loginForm = new Login();
            loginForm.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Customers Page Would Open Here!");
        }
    }
}