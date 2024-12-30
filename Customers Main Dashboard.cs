using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bank_Management_System
{
    public partial class Customers_Main_Dashboard : Form
    {
        public static Customers_Main_Dashboard Instance;

        public Guna2TextBox GetLoggedUserNIC;

        private int currentImageIndex = 0;
        private Image[] images = new Image[4];

        public Customers_Main_Dashboard()
        {
            InitializeComponent();
            Instance = this;
            GetLoggedUserNIC = txtNIC;
            LoadImages();
        }

        // Load images from resources
        private void LoadImages()
        {
            // Assuming you have added image1, image2, image3, image4 to resources
            images[0] = Properties.Resources.Banner_1;  // Replace with actual resource name
            images[1] = Properties.Resources.Banner_2;
            images[2] = Properties.Resources.Banner_3;
            images[3] = Properties.Resources.Banner_4;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Update the PictureBox with the current image
            picBanners.Image = images[currentImageIndex];

            // Update the index to the next image (loop back to the first image after the last one)
            currentImageIndex = (currentImageIndex + 1) % images.Length;
        }

        private void AddPanelsDock()
        {
            pnlBase.Controls.Add(pnlDashboard);
            pnlBase.Controls.Add(pnlProfile);
            pnlBase.Controls.Add(pnlSendMoney);
            pnlBase.Controls.Add(pnlMobilePayment);
            pnlBase.Controls.Add(pnlBillPayment);
            pnlBase.Controls.Add(pnlHistory);
        }

        private void UserDetailLoad()
        {
            try
            {
                string Connection = @"Data Source=Mavinda-PC\SQLEXPRESS;Initial Catalog=""NEXGEN Bank Management System"";Integrated Security=True;";
                string Query = "Select CustomerID, FirstName, LastName, DateOfBirth, Gender, UserName, PhoneNumber, Email, Address, CustomerPhoto From Customers Where NIC = '" + txtNIC.Text + "'";

                SqlConnection conn = new SqlConnection(Connection);
                SqlCommand cmd = new SqlCommand(Query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    btnProfile.Text = reader.GetString(1) + " " + reader.GetString(2);
                    lblWelcome.Text = "Hello " + reader.GetString(1) + ", Welcome back";

                    txtCustomerID.Text = reader.GetString(0);
                    txtFullName.Text = reader.GetString(1) + " " + reader.GetString(2);
                    txtAlertEmail.Text = reader.GetString(7);

                    txtFirstName.Text = reader.GetString(0);
                    txtLastName.Text = reader.GetString(1);
                    txtUserName.Text = reader.GetString(5);
                    txtGender.Text = reader.GetString(4);
                    dtpDOB.Text = reader.GetString(3);
                    txtMobile.Text = reader.GetString(6);
                    txtEmail.Text = reader.GetString(7);
                    txtAddress.Text = reader.GetString(8);


                    DateTime BirthDate = DateTime.Parse(dtpDOB.Text);
                    DateTime CurrentDate = DateTime.Today;
                    int Age = CurrentDate.Year - BirthDate.Year;
                    string str = Age.ToString();
                    txtAge.Text = "Age : " + Age.ToString();

                    if (txtAlertEmail.Text != "")
                    {
                        picEmailAlertStatus.FillColor = Color.FromArgb(13, 238, 100);
                    }
                    else
                    {
                        picEmailAlertStatus.FillColor = Color.Red;
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Customers_Main_Dashboard_Load(object sender, EventArgs e)
        {
            this.Text = "NEXGEN Bank";

            AddPanelsDock();

            timer1.Start();

            UserDetailLoad();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult userResult = MessageBox.Show("Are you sure you want to exit?", "Exit?", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (userResult == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            DialogResult userResult = MessageBox.Show("Are You Sure To Logout?", "User Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (userResult == DialogResult.Yes)
            {
                frmLogIn_SignUp login = new frmLogIn_SignUp();
                this.Hide();
                login.Show();
            }
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            pnlBase.Controls.Add(pnlDashboard);

            pnlDashboard.Visible = true;
            pnlProfile.Visible = false;
            pnlSendMoney.Visible = false;
            pnlMobilePayment.Visible = false;
            pnlBillPayment.Visible = false;
            pnlHistory.Visible = false;
        }

        private void DefaultbtnEdit_Save()
        {
            btnEdit_Save.FillColor = Color.DimGray;
            btnEdit_Save.Text = "Edit";
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            pnlBase.Controls.Add(pnlDashboard);

            pnlDashboard.Visible = true;
            pnlProfile.Visible = true;
            pnlSendMoney.Visible = false;
            pnlMobilePayment.Visible = false;
            pnlBillPayment.Visible = false;
            pnlHistory.Visible = false;
        }

        private void btnEdit_Save_Click(object sender, EventArgs e)
        {
            if (btnEdit_Save.Text == "Edit")
            {
                txtFirstName.Enabled = true;
                txtLastName.Enabled = true;
                dtpDOB.Enabled = true;
                txtMobile.Enabled = true;

                btnEdit_Save.FillColor = Color.FromArgb(23, 45, 157);
                btnEdit_Save.Text = "Save";
            }
            else
            {
                string UpdatedFirstName = txtFirstName.Text;
                string UpdatedLastName = txtLastName.Text;
                string UpdatedDOB = dtpDOB.Text;
                string UpdatedPhoneNumber = txtMobile.Text;

                DateTime BirthDate = DateTime.Parse(UpdatedDOB);
                DateTime CurrentDate = DateTime.Today;
                int Age = CurrentDate.Year - BirthDate.Year;
                string str = Age.ToString();

                if (Age > 18)
                {
                    try
                    {
                        string Connection = @"Data Source=Mavinda-PC\SQLEXPRESS;Initial Catalog=""NEXGEN Bank Management System"";Integrated Security=True;";
                        string Query = "Update Customers Set FirstName = '" + UpdatedFirstName + "', LastName = '" + UpdatedLastName + "', DateOfBirth = '" + UpdatedDOB + "', PhoneNumber = '" + UpdatedPhoneNumber + "' Where NIC = '" + txtNIC.Text + "'";

                        SqlConnection conn = new SqlConnection(Connection);
                        SqlCommand cmd = new SqlCommand(Query, conn);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();

                        MessageBox.Show("Your Personal Information Updated", "Record Updated!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        txtFirstName.Enabled = false;
                        txtLastName.Enabled = false;
                        dtpDOB.Enabled = false;
                        txtMobile.Enabled = false;

                        DefaultbtnEdit_Save();
                        UserDetailLoad();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    lblUpdateError.Text = $"Your Age: {Age}  - Your age must be at least 18.";
                }
            }
        }

        public void UserAccountIDLoad()
        {
            try
            {
                string Connection = @"Data Source=Mavinda-PC\SQLEXPRESS;Initial Catalog=""NEXGEN Bank Management System"";Integrated Security=True;";
                string Query = "Select AccountID From Accounts Where CustomerID = '" + txtCustomerID.Text + "'";

                SqlConnection conn = new SqlConnection(Connection);
                SqlCommand cmd = new SqlCommand(Query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    txtFD_AccID.Text = reader.GetString(0);
                    txtMP_AccID.Text = reader.GetString(0);
                    txtBP_AccID.Text = reader.GetString(0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSendMoney_Click(object sender, EventArgs e)
        {
            TextBoxesClearForFDPanel();

            pnlBase.Controls.Add(pnlDashboard);
            pnlBase.Controls.Add(pnlProfile);

            pnlDashboard.Visible = true;
            pnlProfile.Visible = true;
            pnlSendMoney.Visible = true;
            pnlMobilePayment.Visible = false;
            pnlBillPayment.Visible = false;
            pnlHistory.Visible = false;

            UserAccountIDLoad();
        }

        private string GenerateTransactionID()
        {
            string prefix = "TRANS"; // Customize the prefix as needed
            int year = DateTime.Now.Year;
            string uniquePart = DateTime.Now.ToString("MMddHHmmss"); // Month, Day, Hour, Minute, Second
            return $"{prefix}{year}{uniquePart}";
        }

        private void TextBoxesClearForFDPanel()
        {
            txtFD_PayToID.Clear();
            txtFD_Amount.Clear();
        }

        private void btnFD_Pay_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult userResult = MessageBox.Show("Are you sure you entered correct PayID?", "User Confimation?", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (userResult == DialogResult.Yes)
                {
                    if (txtFD_AccID.Text != "")
                    {
                        if (txtFD_PayToID.Text != "")
                        {
                            if (txtFD_Amount.Text != "")
                            {
                                string Connection = @"Data Source=Mavinda-PC\SQLEXPRESS;Initial Catalog=""NEXGEN Bank Management System"";Integrated Security=True;";
                                string Query4 = "Select Status From Accounts Where AccountID = @AccountID";

                                SqlConnection conn4 = new SqlConnection(Connection);

                                using (SqlCommand cmd5 = new SqlCommand(Query4, conn4))
                                {
                                    conn4.Open();
                                    cmd5.Parameters.AddWithValue("@AccountID", txtFD_AccID.Text);
                                    string result1 = (string)cmd5.ExecuteScalar();

                                    if (result1 == "Active")
                                    {
                                        conn4.Close();
                                        // Transaction save to Accounts Table & Update Balance
                                        float CurrentAmount = 0;

                                        string Query2 = "Select Balance From Accounts Where AccountID = @AccountID";
                                        SqlConnection conn2 = new SqlConnection(Connection);
                                        using (SqlCommand cmd2 = new SqlCommand(Query2, conn2))
                                        {
                                            conn2.Open();
                                            cmd2.Parameters.AddWithValue("@AccountId", txtFD_AccID.Text);
                                            object result = cmd2.ExecuteScalar();
                                            if (result != null)
                                            {
                                                CurrentAmount = Convert.ToSingle(result);
                                            }
                                        }

                                        float NewAmount = CurrentAmount - float.Parse(txtFD_Amount.Text);

                                        string updateQuery = "Update Accounts SET Balance = @Balance WHERE AccountID = @AccountID";
                                        using (SqlCommand cmd3 = new SqlCommand(updateQuery, conn2))
                                        {
                                            cmd3.Parameters.AddWithValue("@Balance", NewAmount);
                                            cmd3.Parameters.AddWithValue("@AccountID", txtFD_AccID.Text);
                                            cmd3.ExecuteNonQuery();
                                            conn2.Close();
                                        }

                                        // Transaction Save to Transactions Table
                                        string TransactionID = GenerateTransactionID();
                                        string TransactionType = txtFD_PayToID.Text;
                                        float Amount = float.Parse(txtFD_Amount.Text);
                                        string TransactionDate = dtpFT_Date.Text;

                                        string Query3 = "Insert Into Transactions Values ('" + TransactionID + "', '" + TransactionType + "', '" + txtFD_AccID.Text + "', " + Amount + ", '" + TransactionDate + "')";

                                        SqlConnection conn3 = new SqlConnection(Connection);
                                        SqlCommand cmd4 = new SqlCommand(Query3, conn3);

                                        conn3.Open();
                                        cmd4.ExecuteNonQuery();
                                        conn3.Close();

                                        MessageBox.Show("Transaction Successfully.");

                                        TextBoxesClearForFDPanel();
                                    }
                                    else
                                    {
                                        MessageBox.Show("This Account Supended or Closed. Check Account Status.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnFD_Cancel_Click(object sender, EventArgs e)
        {
            TextBoxesClearForFDPanel();
        }

        private void btnMobilePayment_Click(object sender, EventArgs e)
        {
            TextBoxesClearForMPPanel();

            pnlBase.Controls.Add(pnlDashboard);
            pnlBase.Controls.Add(pnlProfile);
            pnlBase.Controls.Add(pnlSendMoney);

            pnlDashboard.Visible = true;
            pnlProfile.Visible = true;
            pnlSendMoney.Visible = true;
            pnlMobilePayment.Visible = true;
            pnlBillPayment.Visible = false;
            pnlHistory.Visible = false;

            UserAccountIDLoad();
        }

        private void TextBoxesClearForMPPanel()
        {
            cmbMP_ServicePro.SelectedItem = null;
            txtMP_Number.Clear();
            txtMP_Amount.Clear();
        }

        private void btnMP_Pay_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult userResult = MessageBox.Show("Are you sure you entered correct PayID?", "User Confimation?", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (userResult == DialogResult.Yes)
                {
                    if (txtMP_AccID.Text != "")
                    {
                        if (cmbMP_ServicePro.SelectedItem != null)
                        {
                            if (txtMP_Number.Text != "")
                            {
                                if (txtMP_Amount.Text != "")
                                {
                                    string Connection = @"Data Source=Mavinda-PC\SQLEXPRESS;Initial Catalog=""NEXGEN Bank Management System"";Integrated Security=True;";
                                    string Query4 = "Select Status From Accounts Where AccountID = @AccountID";

                                    SqlConnection conn4 = new SqlConnection(Connection);

                                    using (SqlCommand cmd5 = new SqlCommand(Query4, conn4))
                                    {
                                        conn4.Open();
                                        cmd5.Parameters.AddWithValue("@AccountID", txtMP_AccID.Text);
                                        string result1 = (string)cmd5.ExecuteScalar();

                                        if (result1 == "Active")
                                        {
                                            conn4.Close();
                                            // Transaction save to Accounts Table & Update Balance
                                            float CurrentAmount = 0;

                                            string Query2 = "Select Balance From Accounts Where AccountID = @AccountID";
                                            SqlConnection conn2 = new SqlConnection(Connection);
                                            using (SqlCommand cmd2 = new SqlCommand(Query2, conn2))
                                            {
                                                conn2.Open();
                                                cmd2.Parameters.AddWithValue("@AccountId", txtMP_AccID.Text);
                                                object result = cmd2.ExecuteScalar();
                                                if (result != null)
                                                {
                                                    CurrentAmount = Convert.ToSingle(result);
                                                }
                                            }

                                            float NewAmount = CurrentAmount - float.Parse(txtMP_Amount.Text);

                                            string updateQuery = "Update Accounts SET Balance = @Balance WHERE AccountID = @AccountID";
                                            using (SqlCommand cmd3 = new SqlCommand(updateQuery, conn2))
                                            {
                                                cmd3.Parameters.AddWithValue("@Balance", NewAmount);
                                                cmd3.Parameters.AddWithValue("@AccountID", txtMP_AccID.Text);
                                                cmd3.ExecuteNonQuery();
                                                conn2.Close();
                                            }

                                            // Transaction Save to Transactions Table
                                            string TransactionID = GenerateTransactionID();
                                            string TransactionType = cmbMP_ServicePro.SelectedItem.ToString()+", "+ txtMP_Number.Text;
                                            float Amount = float.Parse(txtMP_Amount.Text);
                                            string TransactionDate = dtpMP_Date.Text;

                                            string Query3 = "Insert Into Transactions Values ('" + TransactionID + "', '" + TransactionType + "', '" + txtMP_AccID.Text + "', " + Amount + ", '" + TransactionDate + "')";

                                            SqlConnection conn3 = new SqlConnection(Connection);
                                            SqlCommand cmd4 = new SqlCommand(Query3, conn3);

                                            conn3.Open();
                                            cmd4.ExecuteNonQuery();
                                            conn3.Close();

                                            MessageBox.Show("Mobile Payment Successfully.");

                                            TextBoxesClearForMPPanel();
                                        }
                                        else
                                        {
                                            MessageBox.Show("This Account Supended or Closed. Check Account Status.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnMP_Cancel_Click(object sender, EventArgs e)
        {
            TextBoxesClearForMPPanel();
        }

        private void btnBillPayment_Click(object sender, EventArgs e)
        {
            TextBoxesClearForBPPanel();

            pnlBase.Controls.Add(pnlDashboard);
            pnlBase.Controls.Add(pnlProfile);
            pnlBase.Controls.Add(pnlSendMoney);
            pnlBase.Controls.Add(pnlMobilePayment);

            pnlDashboard.Visible = true;
            pnlProfile.Visible = true;
            pnlSendMoney.Visible = true;
            pnlMobilePayment.Visible = true;
            pnlBillPayment.Visible = true;
            pnlHistory.Visible = false;

            UserAccountIDLoad();
        }

        private void TextBoxesClearForBPPanel()
        {
            cmbBP_PayTo.SelectedItem = null;
            txtBP_BillNum.Clear();
            txtBP_Amount.Clear();
        }

        private void btnBP_Pay_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult userResult = MessageBox.Show("Are you sure you entered correct PayID?", "User Confimation?", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (userResult == DialogResult.Yes)
                {
                    if (txtBP_AccID.Text != "")
                    {
                        if (cmbBP_PayTo.SelectedItem != null)
                        {
                            if (txtBP_BillNum.Text != "")
                            {
                                if (txtBP_Amount.Text != "")
                                {
                                    string Connection = @"Data Source=Mavinda-PC\SQLEXPRESS;Initial Catalog=""NEXGEN Bank Management System"";Integrated Security=True;";
                                    string Query = "Select Status From Accounts Where AccountID = @AccountID";

                                    SqlConnection conn = new SqlConnection(Connection);

                                    using (SqlCommand cmd = new SqlCommand(Query, conn))
                                    {
                                        conn.Open();
                                        cmd.Parameters.AddWithValue("@AccountID", txtBP_AccID.Text);
                                        string result1 = (string)cmd.ExecuteScalar();

                                        if (result1 == "Active")
                                        {
                                            conn.Close();
                                            // Transaction save to Accounts Table & Update Balance
                                            float CurrentAmount = 0;

                                            string Query2 = "Select Balance From Accounts Where AccountID = @AccountID";
                                            SqlConnection conn2 = new SqlConnection(Connection);
                                            using (SqlCommand cmd2 = new SqlCommand(Query2, conn2))
                                            {
                                                conn2.Open();
                                                cmd2.Parameters.AddWithValue("@AccountId", txtBP_AccID.Text);
                                                object result = cmd2.ExecuteScalar();
                                                if (result != null)
                                                {
                                                    CurrentAmount = Convert.ToSingle(result);
                                                }
                                            }

                                            float NewAmount = CurrentAmount - float.Parse(txtBP_Amount.Text);

                                            string updateQuery = "Update Accounts SET Balance = @Balance WHERE AccountID = @AccountID";
                                            using (SqlCommand cmd3 = new SqlCommand(updateQuery, conn2))
                                            {
                                                cmd3.Parameters.AddWithValue("@Balance", NewAmount);
                                                cmd3.Parameters.AddWithValue("@AccountID", txtBP_AccID.Text);
                                                cmd3.ExecuteNonQuery();
                                                conn2.Close();
                                            }

                                            // Transaction Save to Transactions Table
                                            string TransactionID = GenerateTransactionID();
                                            string TransactionType = cmbBP_PayTo.SelectedItem.ToString() + ", " + txtBP_BillNum.Text;
                                            float Amount = float.Parse(txtBP_Amount.Text);
                                            string TransactionDate = dtpBP_Date.Text;

                                            string Query3 = "Insert Into Transactions Values ('" + TransactionID + "', '" + TransactionType + "', '" + txtBP_AccID.Text + "', " + Amount + ", '" + TransactionDate + "')";

                                            SqlConnection conn3 = new SqlConnection(Connection);
                                            SqlCommand cmd4 = new SqlCommand(Query3, conn3);

                                            conn3.Open();
                                            cmd4.ExecuteNonQuery();
                                            conn3.Close();

                                            MessageBox.Show("Bill Payment Successfully.");

                                            TextBoxesClearForBPPanel();
                                        }
                                        else
                                        {
                                            MessageBox.Show("This Account Supended or Closed. Check Account Status.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnBP_Cancel_Click(object sender, EventArgs e)
        {
            TextBoxesClearForBPPanel();
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            pnlBase.Controls.Add(pnlDashboard);
            pnlBase.Controls.Add(pnlProfile);
            pnlBase.Controls.Add(pnlSendMoney);
            pnlBase.Controls.Add(pnlMobilePayment);
            pnlBase.Controls.Add(pnlBillPayment);

            pnlDashboard.Visible = true;
            pnlProfile.Visible = true;
            pnlSendMoney.Visible = true;
            pnlMobilePayment.Visible = true;
            pnlBillPayment.Visible = true;
            pnlHistory.Visible = true;

            TextBoxesClearForHPanel();
            try
            {
                string connection = @"Data Source=Mavinda-PC\SQLEXPRESS;Initial Catalog=""NEXGEN Bank Management System"";Integrated Security=True;";
                string query = "select * from Transactions where AccountID = '"+txtBP_AccID.Text+"'";

                SqlConnection conn = new SqlConnection(connection);
                SqlDataAdapter adpter = new SqlDataAdapter(query, conn);

                DataSet dataSet = new DataSet();

                conn.Open();
                adpter.Fill(dataSet, "AccountID");
                conn.Close();

                dgvTransactions.DataSource = dataSet;
                dgvTransactions.DataMember = "AccountID";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnH_Search_Click(object sender, EventArgs e)
        {
            try
            {
                string searchText = txtH_Search.Text;
                string connection = @"Data Source=Mavinda-PC\SQLEXPRESS;Initial Catalog=""NEXGEN Bank Management System"";Integrated Security=True;";
                string query = "select * from Transactions where TransactionDate = '" + searchText + "'";

                SqlConnection conn = new SqlConnection(connection);
                SqlDataAdapter adpter = new SqlDataAdapter(query, conn);

                DataSet dataSet = new DataSet();

                conn.Open();
                adpter.Fill(dataSet, "TransactionDate");
                conn.Close();

                dgvTransactions.DataSource = dataSet;
                dgvTransactions.DataMember = "TransactionDate";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TextBoxesClearForHPanel()
        {
            txtH_TransID.Clear();
            txtH_AccID.Clear();
            txtH_TransType.Clear();
            txtH_TransDate.Clear();
            txtH_Amount.Clear();
            txtH_Search.Clear();
        }

        private void btnH_Reset_Click(object sender, EventArgs e)
        {
            TextBoxesClearForHPanel();
        }
    }
}
