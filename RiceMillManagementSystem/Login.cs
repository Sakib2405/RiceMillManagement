using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace RiceMillManagementSystem
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }




        private void Login_Load(object sender, EventArgs e)
        {
            panel1.BackColor = Color.FromArgb(100, 0, 0, 0);
            panel2.BackColor = Color.FromArgb(190, 0, 0, 0);
            // dkdk

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source=Afridi;Initial Catalog=ricemillmanagement;Integrated Security=True;TrustServerCertificate=True");
            conn.Open();
            string query = "SELECT COUNT(*) FROM Users WHERE Username=@username AND Password=@password";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@username", textBox1.Text);
            cmd.Parameters.AddWithValue("@password", textBox2.Text);
            int count = (int)cmd.ExecuteScalar();
            string query1 = "SELECT UserID FROM Users WHERE Username=@username";
            SqlCommand cmd1 =new SqlCommand(query1, conn);
            cmd1.Parameters.AddWithValue("@username", textBox1.Text);
            int userId = Convert.ToInt32(cmd1.ExecuteScalar());
            conn.Close();
            if (count > 0)
            {
               Dashboard dashboard = new Dashboard(userId);
                dashboard.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Error in Login");
            }
        }

        

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Signup signup = new Signup();
            signup.Show();
            this.Hide();

        }
    }
}
