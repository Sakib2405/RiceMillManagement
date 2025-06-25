namespace RiceMillManagementSystem
{
    partial class Dashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.panelMenu = new System.Windows.Forms.Panel();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnReports = new System.Windows.Forms.Button();
            this.btnPayments = new System.Windows.Forms.Button();
            this.btnSales = new System.Windows.Forms.Button();
            this.btnPurchases = new System.Windows.Forms.Button();
            this.btnSuppliers = new System.Windows.Forms.Button();
            this.btnCustomers = new System.Windows.Forms.Button();
            this.btnInventory = new System.Windows.Forms.Button();
            this.lblWelcomeUser = new System.Windows.Forms.Label();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.panelContent = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgvLowStock = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvRecentSales = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.salesChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblTotalSuppliers = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblTotalCustomers = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblTotalPurchases = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTotalSales = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.panelMenu.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLowStock)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecentSales)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.salesChart)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMenu
            // 
            this.panelMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(32)))), ((int)(((byte)(44)))));
            this.panelMenu.Controls.Add(this.button1);
            this.panelMenu.Controls.Add(this.btnLogout);
            this.panelMenu.Controls.Add(this.btnReports);
            this.panelMenu.Controls.Add(this.btnPayments);
            this.panelMenu.Controls.Add(this.btnSales);
            this.panelMenu.Controls.Add(this.btnPurchases);
            this.panelMenu.Controls.Add(this.btnSuppliers);
            this.panelMenu.Controls.Add(this.btnCustomers);
            this.panelMenu.Controls.Add(this.btnInventory);
            this.panelMenu.Controls.Add(this.lblWelcomeUser);
            this.panelMenu.Controls.Add(this.lblWelcome);
            this.panelMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelMenu.Location = new System.Drawing.Point(0, 0);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.Size = new System.Drawing.Size(250, 774);
            this.panelMenu.TabIndex = 0;
            this.panelMenu.Paint += new System.Windows.Forms.PaintEventHandler(this.panelMenu_Paint);
            // 
            // btnLogout
            // 
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnLogout.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(0, 724);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(250, 50);
            this.btnLogout.TabIndex = 9;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnReports
            // 
            this.btnReports.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnReports.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.btnReports.ForeColor = System.Drawing.Color.White;
            this.btnReports.Location = new System.Drawing.Point(0, 541);
            this.btnReports.Name = "btnReports";
            this.btnReports.Size = new System.Drawing.Size(250, 50);
            this.btnReports.TabIndex = 8;
            this.btnReports.Text = "Reports";
            this.btnReports.UseVisualStyleBackColor = false;
            this.btnReports.Click += new System.EventHandler(this.btnReports_Click);
            // 
            // btnPayments
            // 
            this.btnPayments.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnPayments.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.btnPayments.ForeColor = System.Drawing.Color.White;
            this.btnPayments.Location = new System.Drawing.Point(0, 488);
            this.btnPayments.Name = "btnPayments";
            this.btnPayments.Size = new System.Drawing.Size(250, 50);
            this.btnPayments.TabIndex = 7;
            this.btnPayments.Text = "Payments";
            this.btnPayments.UseVisualStyleBackColor = false;
            this.btnPayments.Click += new System.EventHandler(this.btnPayments_Click);
            // 
            // btnSales
            // 
            this.btnSales.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnSales.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.btnSales.ForeColor = System.Drawing.Color.White;
            this.btnSales.Location = new System.Drawing.Point(0, 435);
            this.btnSales.Name = "btnSales";
            this.btnSales.Size = new System.Drawing.Size(250, 50);
            this.btnSales.TabIndex = 6;
            this.btnSales.Text = "Sales";
            this.btnSales.UseVisualStyleBackColor = false;
            this.btnSales.Click += new System.EventHandler(this.btnSales_Click);
            // 
            // btnPurchases
            // 
            this.btnPurchases.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnPurchases.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.btnPurchases.ForeColor = System.Drawing.Color.White;
            this.btnPurchases.Location = new System.Drawing.Point(0, 382);
            this.btnPurchases.Name = "btnPurchases";
            this.btnPurchases.Size = new System.Drawing.Size(250, 50);
            this.btnPurchases.TabIndex = 5;
            this.btnPurchases.Text = "Purchases";
            this.btnPurchases.UseVisualStyleBackColor = false;
            this.btnPurchases.Click += new System.EventHandler(this.btnPurchases_Click);
            // 
            // btnSuppliers
            // 
            this.btnSuppliers.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnSuppliers.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.btnSuppliers.ForeColor = System.Drawing.Color.White;
            this.btnSuppliers.Location = new System.Drawing.Point(0, 328);
            this.btnSuppliers.Name = "btnSuppliers";
            this.btnSuppliers.Size = new System.Drawing.Size(250, 50);
            this.btnSuppliers.TabIndex = 4;
            this.btnSuppliers.Text = "Suppliers";
            this.btnSuppliers.UseVisualStyleBackColor = false;
            this.btnSuppliers.Click += new System.EventHandler(this.btnSuppliers_Click);
            // 
            // btnCustomers
            // 
            this.btnCustomers.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnCustomers.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.btnCustomers.ForeColor = System.Drawing.Color.White;
            this.btnCustomers.Location = new System.Drawing.Point(0, 274);
            this.btnCustomers.Name = "btnCustomers";
            this.btnCustomers.Size = new System.Drawing.Size(250, 50);
            this.btnCustomers.TabIndex = 3;
            this.btnCustomers.Text = "Customers";
            this.btnCustomers.UseVisualStyleBackColor = false;
            this.btnCustomers.Click += new System.EventHandler(this.btnCustomers_Click);
            // 
            // btnInventory
            // 
            this.btnInventory.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnInventory.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.btnInventory.ForeColor = System.Drawing.Color.White;
            this.btnInventory.Location = new System.Drawing.Point(0, 219);
            this.btnInventory.Name = "btnInventory";
            this.btnInventory.Size = new System.Drawing.Size(250, 50);
            this.btnInventory.TabIndex = 2;
            this.btnInventory.Text = "Inventory";
            this.btnInventory.UseVisualStyleBackColor = false;
            this.btnInventory.Click += new System.EventHandler(this.btnInventory_Click);
            // 
            // lblWelcomeUser
            // 
            this.lblWelcomeUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.lblWelcomeUser.ForeColor = System.Drawing.Color.White;
            this.lblWelcomeUser.Location = new System.Drawing.Point(3, 60);
            this.lblWelcomeUser.Name = "lblWelcomeUser";
            this.lblWelcomeUser.Size = new System.Drawing.Size(244, 26);
            this.lblWelcomeUser.TabIndex = 1;
            this.lblWelcomeUser.Text = "User Name";
            this.lblWelcomeUser.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.ForeColor = System.Drawing.Color.White;
            this.lblWelcome.Location = new System.Drawing.Point(70, 20);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(98, 24);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Welcome";
            // 
            // panelContent
            // 
            this.panelContent.Controls.Add(this.groupBox3);
            this.panelContent.Controls.Add(this.groupBox2);
            this.panelContent.Controls.Add(this.groupBox1);
            this.panelContent.Controls.Add(this.panel4);
            this.panelContent.Controls.Add(this.panel3);
            this.panelContent.Controls.Add(this.panel2);
            this.panelContent.Controls.Add(this.panel1);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(250, 0);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(1212, 774);
            this.panelContent.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dgvLowStock);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.groupBox3.Location = new System.Drawing.Point(630, 440);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(545, 280);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Low Stock Items";
            // 
            // dgvLowStock
            // 
            this.dgvLowStock.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLowStock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLowStock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLowStock.Location = new System.Drawing.Point(3, 19);
            this.dgvLowStock.Name = "dgvLowStock";
            this.dgvLowStock.RowHeadersWidth = 62;
            this.dgvLowStock.Size = new System.Drawing.Size(539, 258);
            this.dgvLowStock.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvRecentSales);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.groupBox2.Location = new System.Drawing.Point(20, 440);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(590, 280);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Recent Sales";
            // 
            // dgvRecentSales
            // 
            this.dgvRecentSales.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRecentSales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRecentSales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRecentSales.Location = new System.Drawing.Point(3, 19);
            this.dgvRecentSales.Name = "dgvRecentSales";
            this.dgvRecentSales.RowHeadersWidth = 62;
            this.dgvRecentSales.Size = new System.Drawing.Size(584, 258);
            this.dgvRecentSales.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.salesChart);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(20, 150);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1155, 275);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Monthly Sales Chart";
            // 
            // salesChart
            // 
            chartArea5.Name = "ChartArea1";
            this.salesChart.ChartAreas.Add(chartArea5);
            this.salesChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend5.Name = "Legend1";
            this.salesChart.Legends.Add(legend5);
            this.salesChart.Location = new System.Drawing.Point(3, 19);
            this.salesChart.Name = "salesChart";
            series5.ChartArea = "ChartArea1";
            series5.Legend = "Legend1";
            series5.Name = "Sales";
            this.salesChart.Series.Add(series5);
            this.salesChart.Size = new System.Drawing.Size(1149, 253);
            this.salesChart.TabIndex = 0;
            this.salesChart.Text = "chart1";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.panel4.Controls.Add(this.lblTotalSuppliers);
            this.panel4.Controls.Add(this.label8);
            this.panel4.Location = new System.Drawing.Point(900, 20);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(275, 110);
            this.panel4.TabIndex = 3;
            // 
            // lblTotalSuppliers
            // 
            this.lblTotalSuppliers.AutoSize = true;
            this.lblTotalSuppliers.Font = new System.Drawing.Font("Microsoft Sans Serif", 22F, System.Drawing.FontStyle.Bold);
            this.lblTotalSuppliers.ForeColor = System.Drawing.Color.White;
            this.lblTotalSuppliers.Location = new System.Drawing.Point(110, 50);
            this.lblTotalSuppliers.Name = "lblTotalSuppliers";
            this.lblTotalSuppliers.Size = new System.Drawing.Size(33, 36);
            this.lblTotalSuppliers.TabIndex = 1;
            this.lblTotalSuppliers.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(60, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(150, 24);
            this.label8.TabIndex = 0;
            this.label8.Text = "Total Suppliers";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Goldenrod;
            this.panel3.Controls.Add(this.lblTotalCustomers);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Location = new System.Drawing.Point(610, 20);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(275, 110);
            this.panel3.TabIndex = 2;
            // 
            // lblTotalCustomers
            // 
            this.lblTotalCustomers.AutoSize = true;
            this.lblTotalCustomers.Font = new System.Drawing.Font("Microsoft Sans Serif", 22F, System.Drawing.FontStyle.Bold);
            this.lblTotalCustomers.ForeColor = System.Drawing.Color.White;
            this.lblTotalCustomers.Location = new System.Drawing.Point(120, 50);
            this.lblTotalCustomers.Name = "lblTotalCustomers";
            this.lblTotalCustomers.Size = new System.Drawing.Size(33, 36);
            this.lblTotalCustomers.TabIndex = 1;
            this.lblTotalCustomers.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(55, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(161, 24);
            this.label6.TabIndex = 0;
            this.label6.Text = "Total Customers";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.panel2.Controls.Add(this.lblTotalPurchases);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Location = new System.Drawing.Point(320, 20);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(275, 110);
            this.panel2.TabIndex = 1;
            // 
            // lblTotalPurchases
            // 
            this.lblTotalPurchases.AutoSize = true;
            this.lblTotalPurchases.Font = new System.Drawing.Font("Microsoft Sans Serif", 22F, System.Drawing.FontStyle.Bold);
            this.lblTotalPurchases.ForeColor = System.Drawing.Color.White;
            this.lblTotalPurchases.Location = new System.Drawing.Point(40, 50);
            this.lblTotalPurchases.Name = "lblTotalPurchases";
            this.lblTotalPurchases.Size = new System.Drawing.Size(78, 36);
            this.lblTotalPurchases.TabIndex = 1;
            this.lblTotalPurchases.Text = "0.00";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(15, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(238, 24);
            this.label4.TabIndex = 0;
            this.label4.Text = "Total Purchases (Month)";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.ForestGreen;
            this.panel1.Controls.Add(this.lblTotalSales);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(20, 20);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(275, 110);
            this.panel1.TabIndex = 0;
            // 
            // lblTotalSales
            // 
            this.lblTotalSales.AutoSize = true;
            this.lblTotalSales.Font = new System.Drawing.Font("Microsoft Sans Serif", 22F, System.Drawing.FontStyle.Bold);
            this.lblTotalSales.ForeColor = System.Drawing.Color.White;
            this.lblTotalSales.Location = new System.Drawing.Point(40, 50);
            this.lblTotalSales.Name = "lblTotalSales";
            this.lblTotalSales.Size = new System.Drawing.Size(78, 36);
            this.lblTotalSales.TabIndex = 1;
            this.lblTotalSales.Text = "0.00";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(40, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(191, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Total Sales (Today)";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(0, 164);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(250, 50);
            this.button1.TabIndex = 10;
            this.button1.Text = "Dashboard";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1462, 774);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelMenu);
            this.Name = "Dashboard";
            this.Text = "Dashboard";
            this.Load += new System.EventHandler(this.Dashboard_Load);
            this.panelMenu.ResumeLayout(false);
            this.panelMenu.PerformLayout();
            this.panelContent.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLowStock)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecentSales)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.salesChart)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Label lblWelcomeUser;
        private System.Windows.Forms.Button btnInventory;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnReports;
        private System.Windows.Forms.Button btnPayments;
        private System.Windows.Forms.Button btnSales;
        private System.Windows.Forms.Button btnPurchases;
        private System.Windows.Forms.Button btnSuppliers;
        private System.Windows.Forms.Button btnCustomers;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTotalSales;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lblTotalSuppliers;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblTotalCustomers;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblTotalPurchases;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataVisualization.Charting.Chart salesChart;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvRecentSales;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dgvLowStock;
        private System.Windows.Forms.Button button1;
    }
}