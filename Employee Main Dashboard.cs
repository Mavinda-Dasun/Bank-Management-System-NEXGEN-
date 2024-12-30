using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;

namespace Bank_Management_System
{
    public partial class Employee_Main_Dashboard : Form
    {
        public static Employee_Main_Dashboard Instance;

        public Guna2TextBox GetLoggedUserNIC;

        private int currentImageIndex = 0;
        private Image[] images = new Image[4];

        string fileName;

        public Employee_Main_Dashboard()
        {
            InitializeComponent();
            Instance = this;
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
            pnlBase.Controls.Add(pnlAccount);
            pnlBase.Controls.Add(pnlWithdraw_Deposit);
            pnlBase.Controls.Add(pnlHistory);
            pnlBase.Controls.Add(pnlCustomerRequest);
        }

        private void ClearTextBoxesForUPICPanel()
        {
            txtSearch.Clear();

            // User Personal Information Customize panel textboxes 
            txtUserName.Clear();
            txtPassword.Clear();
            txtNIC.Clear();
            dtpDOB.Text = null;
            txtFirstName.Clear();
            txtLastName.Clear();
            txtMobile.Clear();
            txtEmail.Clear();
            txtAddress.Clear();
            txtSearch.Clear();
        }

        private void ClearTextBoxesForONAPanel()
        {
            // Open New Account panel textboxes
            txtOAcc_UserName.Clear();
            txtOAcc_Password.Clear();
            txtOAcc_NIC.Clear();
            dtpOAcc_DOB.Text = null;
            txtOAcc_FirstName.Clear();
            txtOAcc_LastName.Clear();
            txtOAcc_Mobile.Clear();
            txtOAcc_Email.Clear();
            txtOAcc_Address.Clear();
            txtOAcc_Gender.Clear();
            picCustomer.Image = null;
            lblFilename.Text = "?";
        }

        private void ClearTextBoxesMAPanel()
        {
            txtMA_Search.Clear();
            txtMA_CustomerID.Clear();
            txtMA_FirstName.Clear();
            txtMA_LastName.Clear();
            txtMA_AccountType.Clear();
            txtMA_AccountID.Clear();
            txtMA_Balance.Clear();
            txtMA_Status.Clear();
            cmbMA_Status.SelectedItem = null;
        }

        private void ClearTextBoxesWDPanel()
        {
            txtWD_AccountID.Clear();
            txtWD_NIC.Clear();
            cmbWD_TransactionType.SelectedItem = null;
            txtWD_Amount.Clear();
        }

        private void ClearTextBoxesHPanel()
        {
            txtH_TransID.Clear();
            txtH_AccID.Clear();
            txtH_TransType.Clear();
            txtH_TransDate.Clear();
            txtH_Amount.Clear();
            txtH_Search.Clear();
        }

        private void ClearTextBoxesCRPanel()
        {
            txtCR_AccID.Clear();
            txtCR_Contact.Clear();
            txtCR_Fullname.Clear();
            txtCR_NIC.Clear();
            rtxtCR_Request.Clear();
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

        private void CustomerDataLoadToDataGridView()
        {
            try
            {
                string connection = @"Data Source=Mavinda-PC\SQLEXPRESS;Initial Catalog=""NEXGEN Bank Management System"";Integrated Security=True;";
                string query = "Select * From Customers";

                SqlConnection conn = new SqlConnection(connection);
                SqlDataAdapter adpter = new SqlDataAdapter(query, conn);

                DataSet dataSet = new DataSet();

                conn.Open();
                adpter.Fill(dataSet, "NIC");
                conn.Close();

                dgvCustomers.DataSource = dataSet;
                dgvCustomers.DataMember = "NIC";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Employee_Main_Dashboard_Load(object sender, EventArgs e)
        {
            AddPanelsDock();
            CustomerDataLoadToDataGridView();
            timer1.Start();
            btnDashboard_Click(sender, e);
        }

        private void btnOpenNewAccount_Click(object sender, EventArgs e)
        {
            if (pnlAccountOpen.Visible == false)
            {
                pnlUserInfoCustomize.Visible = true;
                pnlAccountOpen.Visible = true;
            }
        }

        private void pnlProfile_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnAccountCustomize_Click(object sender, EventArgs e)
        {
            CustomerDataLoadToDataGridView();

            if (pnlUserInfoCustomize.Visible == false)
            {
                pnlUserInfoCustomize.Visible = true;
                pnlAccountOpen.Visible = false;

                if (txtSearch.Visible == false)
                {
                    txtSearch.Visible = true;
                    btnSearch.Visible = true;
                }
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "JPEG|*.jpg", ValidateNames = true, Multiselect = false })

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    fileName = ofd.FileName;
                    lblFilename.Text = fileName;
                    picCustomer.Image = Image.FromFile(fileName);
                }
        }

        byte[] ConvertImageToBinary(Image img)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        private string GenerateCustomerID()
        {
            string prefix = "CUST"; // Customize the prefix as needed
            int year = DateTime.Now.Year;
            string uniquePart = DateTime.Now.ToString("MMddHHmmss"); // Month, Day, Hour, Minute, Second
            return $"{prefix}{year}{uniquePart}";
        }

        private string GenerateAccountID()
        {
            string prefix = "ACC"; // Customize the prefix as needed
            int year = DateTime.Now.Year;
            string uniquePart = DateTime.Now.ToString("MMddHHmmss"); // Month, Day, Hour, Minute, Second
            return $"{prefix}{year}{uniquePart}";
        }

        private void GenarateCustomerGender()
        {
            string GenderForNIC = txtOAcc_NIC.Text;
            if (GenderForNIC.Length == 12)
            {
                // We need digits from positions 5 to 8, which correspond to indices 4 to 7 (0-based index)
                int startIndex = 4; // Starting index (0-based)
                int length = 3; // Length of the substring (4 digits)

                // Extract the substring (digits from position 5-8)
                string selectedDigits = txtOAcc_NIC.Text.Substring(startIndex, length);

                // Display the selected digits in Label2
                txtOAcc_Gender.Text = selectedDigits;

                string GenderC = txtOAcc_Gender.Text;
                int GenderCode = int.Parse(GenderC);

                if (GenderCode > 500)
                {
                    string Gender;
                    Gender = "Female";
                    txtOAcc_Gender.Text = null;
                    txtOAcc_Gender.Text = Gender;
                }
                else
                {
                    if (GenderCode <= 500)
                    {
                        string Gender;
                        Gender = "Male";
                        txtOAcc_Gender.Text = null;
                        txtOAcc_Gender.Text = Gender;
                    }
                }
            }
            else
            {
                if (GenderForNIC.Length == 10)
                {
                    // We need digits from positions 5 to 8, which correspond to indices 4 to 7 (0-based index)
                    int startIndex = 2; // Starting index (0-based)
                    int length = 3; // Length of the substring (4 digits)

                    // Extract the substring (digits from position 5-8)
                    string selectedDigits = txtOAcc_NIC.Text.Substring(startIndex, length);

                    // Display the selected digits in Label2
                    txtOAcc_Gender.Text = selectedDigits;

                    string GenderC = txtOAcc_Gender.Text;
                    int GenderCode = int.Parse(GenderC);

                    if (GenderCode <= 500)
                    {
                        string Gender;
                        Gender = "Male";
                        txtOAcc_Gender.Text = null;
                        txtOAcc_Gender.Text = Gender;
                    }
                    else
                    {
                        if (GenderCode > 500)
                        {
                            string Gender;
                            Gender = "Female";
                            txtOAcc_Gender.Text = null;
                            txtOAcc_Gender.Text = Gender;
                        }
                    }
                }
            }
        }

        // New Customer Account Save
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbAccountType.SelectedItem != null)
                {
                    DateTime BirthDate = DateTime.Parse(dtpOAcc_DOB.Text);
                    DateTime CurrentDate = DateTime.Today;
                    int Age = CurrentDate.Year - BirthDate.Year;
                    string str = Age.ToString();

                    if (txtOAcc_FirstName.Text != "" && txtOAcc_LastName.Text != "")
                    {
                        string NewCustomerID = GenerateCustomerID();

                        if (txtOAcc_UserName.Text != "")
                        {
                            // Check if the UserName already used
                            string Connection = @"Data Source=Mavinda-PC\SQLEXPRESS;Initial Catalog=""NEXGEN Bank Management System"";Integrated Security=True;";
                            string Query = "Select Username From Customers Where Username = @Username";

                            SqlConnection conn = new SqlConnection(Connection);
                            SqlCommand cmd = new SqlCommand(Query, conn);
                            cmd.Parameters.AddWithValue("Username", txtOAcc_UserName.Text);

                            SqlDataReader reader;
                            conn.Open();
                            reader = cmd.ExecuteReader();

                            if (reader.Read())
                            {
                                conn.Close();
                                lblError.Text = null;
                                lblError.Text = "That username is already being used.";
                            }
                            else
                            {
                                conn.Close();

                                if (txtOAcc_NIC.Text != "")
                                {
                                    // Check if the UserName already used                                
                                    string Query1 = "Select NIC From Customers Where NIC = @NIC";

                                    SqlCommand cmd1 = new SqlCommand(Query1, conn);
                                    cmd1.Parameters.AddWithValue("NIC", txtOAcc_NIC.Text);

                                    SqlDataReader reader1;
                                    conn.Open();
                                    reader1 = cmd.ExecuteReader();

                                    if (reader1.Read())
                                    {
                                        conn.Close();
                                        lblError.Text = null;
                                        lblError.Text = "This Customer has already Account.";
                                    }
                                    else
                                    {
                                        conn.Close();

                                        if (txtOAcc_NIC.Text.Length == 12 || txtOAcc_NIC.Text.Length == 10)
                                        {
                                            if (cmbAccountType.SelectedItem != null)
                                            {
                                                // Accounts Details
                                                string UserAccountID = GenerateAccountID();
                                                string AccountStatus = "Active";
                                                decimal AccBalance = 1000.00M;

                                                string Query3 = "Insert Into Accounts (AccountID, CustomerID, AccountType, Balance, Status) Values (@AccountID, @CustomerID, @AccountType, @Balance, @Status)";

                                                SqlCommand cmd3 = new SqlCommand(Query3, conn);

                                                cmd3.Parameters.AddWithValue("@AccountID", UserAccountID);
                                                cmd3.Parameters.AddWithValue("@CustomerID", NewCustomerID);
                                                cmd3.Parameters.AddWithValue("@AccountType", cmbAccountType.SelectedItem);
                                                cmd3.Parameters.AddWithValue("@Balance", AccBalance);
                                                cmd3.Parameters.AddWithValue("@Status", AccountStatus);

                                                conn.Open();
                                                cmd3.ExecuteNonQuery();
                                                conn.Close();

                                                // Customer Details
                                                GenarateCustomerGender();

                                                Image pimg = picCustomer.Image;
                                                ImageConverter Converter = new ImageConverter();
                                                var ImageConvert = Converter.ConvertTo(pimg, typeof(byte[]));

                                                string Query2 = "Insert Into Customers (CustomerID, FirstName, LastName, DateOfBirth, NIC, Gender, PhoneNumber, Email, Address, CustomerPhoto, Username, Password, FileName) Values (@CustomerID, @FirstName, @LastName, @DateOfBirth, @NIC, @Gender, @PhoneNumber, @Email, @Address, @CustomerPhoto, @Username, @Password, @FileName)";

                                                SqlCommand cmd2 = new SqlCommand(Query2, conn);

                                                cmd2.CommandType = CommandType.Text;
                                                cmd2.Parameters.AddWithValue("@CustomerID", NewCustomerID);
                                                cmd2.Parameters.AddWithValue("@FirstName", txtOAcc_FirstName.Text);
                                                cmd2.Parameters.AddWithValue("@LastName", txtOAcc_LastName.Text);
                                                cmd2.Parameters.AddWithValue("@DateOfBirth", dtpOAcc_DOB.Text);
                                                cmd2.Parameters.AddWithValue("@NIC", txtOAcc_NIC.Text);
                                                cmd2.Parameters.AddWithValue("@Gender", txtOAcc_Gender.Text);
                                                cmd2.Parameters.AddWithValue("@PhoneNumber", txtOAcc_Mobile.Text);
                                                cmd2.Parameters.AddWithValue("@Email", txtOAcc_Email.Text);
                                                cmd2.Parameters.AddWithValue("@Address", txtOAcc_Address.Text);
                                                cmd2.Parameters.AddWithValue("@CustomerPhoto", ImageConvert);
                                                cmd2.Parameters.AddWithValue("@Username", txtOAcc_UserName.Text);
                                                cmd2.Parameters.AddWithValue("@Password", txtOAcc_Password.Text);
                                                cmd2.Parameters.AddWithValue("@FileName", lblFilename.Text);

                                                conn.Open();
                                                cmd2.ExecuteNonQuery();
                                                conn.Close();

                                                MessageBox.Show("New Account Open Successfully");

                                                ClearTextBoxesForONAPanel();
                                            }
                                            else
                                            {

                                            }
                                        }
                                        else
                                        {
                                            lblError.Text = "Please enter valid nic number.";
                                        }
                                    }
                                }
                                else
                                {
                                    lblError.Text = "Please enter customer nic number.";
                                }
                            }
                        }
                        else
                        {
                            lblError.Text = "Please enter customer name.";
                        }
                    }
                    else
                    {
                        lblError.Text = "Please enter customer first name & last name.";
                    }
                }
                else
                {
                    MessageBox.Show("Please select Account Type..");
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ClearTextBoxesForONAPanel();
        }

        private void btnOpenAccBack_Click(object sender, EventArgs e)
        {
            ClearTextBoxesForONAPanel();

            if (pnlAccountOpen.Visible == true)
            {
                pnlUserInfoCustomize.Visible = false;
                pnlAccountOpen.Visible = false;
            }
        }

        // Customer Info Customize Back
        private void guna2Button3_Click(object sender, EventArgs e)
        {
            ClearTextBoxesForUPICPanel();

            if (pnlUserInfoCustomize.Visible == true)
            {
                pnlUserInfoCustomize.Visible = false;

                if (txtSearch.Visible == true)
                {
                    txtSearch.Visible = false;
                    btnSearch.Visible = false;
                }
            }
        }

        // Delete Account
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            try
            {
                string CustomerToBeDeleted = txtSearch.Text;

                DialogResult userResult = MessageBox.Show("Are you sure to Delete this Customer Account: " + CustomerToBeDeleted + " ?", "User Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (userResult == DialogResult.Yes)
                {
                    if (CustomerToBeDeleted != "")
                    {
                        string Connection = @"Data Source=Mavinda-PC\SQLEXPRESS;Initial Catalog=""NEXGEN Bank Management System"";Integrated Security=True;";
                        string Query = "Delete From Customers Where NIC = '" + CustomerToBeDeleted + "' ";

                        SqlConnection conn = new SqlConnection(Connection);
                        SqlCommand cmd = new SqlCommand(Query, conn);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();

                        MessageBox.Show("This Customer Account: " + CustomerToBeDeleted + ", Successfully Deleted.", "Account Deleted!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        //Method  - Function Call
                        ClearTextBoxesForUPICPanel();
                        CustomerDataLoadToDataGridView();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Customer Account Info Update
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                string UpdatedFirstName = txtFirstName.Text;
                string UpdatedLastName = txtLastName.Text;
                string UpdatedUserName = txtUserName.Text;
                string UpdatedPassword = txtPassword.Text;
                string UpdatedDOB = dtpDOB.Text;
                string UpdatedNIC = txtNIC.Text;
                string UpdatedPhoneNumber = txtMobile.Text;
                string UpdatedEmail = txtEmail.Text;
                string UpdatedAddress = txtAddress.Text;

                string connection = @"Data Source=Mavinda-PC\SQLEXPRESS;Initial Catalog=""NEXGEN Bank Management System"";Integrated Security=True;";
                string query = "update Customers Set FirstName = '" + UpdatedFirstName + "', LastName = '" + UpdatedLastName + "', DateOfBirth = '" + UpdatedDOB + "', PhoneNumber = '" + UpdatedPhoneNumber + "', Email = '" + UpdatedEmail + "', Address = '" + UpdatedAddress + "', Username = '" + UpdatedUserName + "', Password = '" + UpdatedPassword + "' Where NIC = '" + UpdatedNIC + "' ";

                SqlConnection conn = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("This Customer:" + UpdatedNIC + ", Personal Information Updated", "Record Updated!",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtNIC.ReadOnly = false;

                //Method  - Function Call
                ClearTextBoxesForUPICPanel();
                CustomerDataLoadToDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Save_Click(object sender, EventArgs e)
        {
            if (txtNIC.Text != "")
            {
                txtFirstName.Enabled = true;
                txtLastName.Enabled = true;
                txtUserName.Enabled = true;
                txtPassword.Enabled = true;
                dtpDOB.Enabled = true;
                txtMobile.Enabled = true;
                txtEmail.Enabled = true;
                txtAddress.Enabled = true;
            }
            else
            {
                MessageBox.Show("Please Select befor Customer", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Reset Button For 
        private void guna2Button4_Click(object sender, EventArgs e)
        {
            ClearTextBoxesForUPICPanel();
        }

        private void dgvCustomerAccounts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string selectedFirstName;
                string selectedLastName;
                string selectedDOB;
                string selectedNIC;
                string selectedPhoneNumber;
                string selectedEmail;
                string selectedAddress;
                string selectedUsername;
                string selectedPassword;

                selectedFirstName = dgvCustomers.Rows[e.RowIndex].Cells[1].Value.ToString();
                selectedLastName = dgvCustomers.Rows[e.RowIndex].Cells[2].Value.ToString();
                selectedDOB = dgvCustomers.Rows[e.RowIndex].Cells[3].Value.ToString();
                selectedNIC = dgvCustomers.Rows[e.RowIndex].Cells[4].Value.ToString();
                selectedPhoneNumber = dgvCustomers.Rows[e.RowIndex].Cells[6].Value.ToString();
                selectedEmail = dgvCustomers.Rows[e.RowIndex].Cells[7].Value.ToString();
                selectedAddress = dgvCustomers.Rows[e.RowIndex].Cells[8].Value.ToString();
                selectedUsername = dgvCustomers.Rows[e.RowIndex].Cells[10].Value.ToString();
                selectedPassword = dgvCustomers.Rows[e.RowIndex].Cells[11].Value.ToString();

                //Fill Data
                txtFirstName.Text = selectedFirstName;
                txtLastName.Text = selectedLastName;
                dtpDOB.Text = selectedDOB;
                txtNIC.Text = selectedNIC;
                txtMobile.Text = selectedPhoneNumber;
                txtEmail.Text = selectedEmail;
                txtAddress.Text = selectedAddress;
                txtUserName.Text = selectedUsername;
                txtPassword.Text = selectedPassword;

                txtNIC.ReadOnly = true;
            }
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            pnlBase.Controls.Add(pnlDashboard);

            pnlDashboard.Visible = true;
            pnlProfile.Visible = false;
            pnlAccount.Visible = false;
            pnlWithdraw_Deposit.Visible = false;
            pnlHistory.Visible = false;
            pnlCustomerRequest.Visible = false;
        }

        // Account Button
        private void btnCards_Click(object sender, EventArgs e)
        {
            pnlBase.Controls.Add(pnlDashboard);

            pnlDashboard.Visible = true;
            pnlProfile.Visible = true;
            pnlAccount.Visible = false;
            pnlWithdraw_Deposit.Visible = false;
            pnlHistory.Visible = false;
            pnlCustomerRequest.Visible = false;

            pnlAccountOpen.Visible = false;
            pnlUserInfoCustomize.Visible = false;
        }

        private void AccountDataLoadToDataGridView()
        {
            try
            {
                string connection = @"Data Source=Mavinda-PC\SQLEXPRESS;Initial Catalog=""NEXGEN Bank Management System"";Integrated Security=True;";
                string query = "Select * From Accounts";

                SqlConnection conn = new SqlConnection(connection);
                SqlDataAdapter adpter = new SqlDataAdapter(query, conn);

                DataSet dataSet = new DataSet();

                conn.Open();
                adpter.Fill(dataSet, "AccountID");
                conn.Close();

                dgvAccount.DataSource = dataSet;
                dgvAccount.DataMember = "AccountID";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTopUp_Click(object sender, EventArgs e)
        {
            AccountDataLoadToDataGridView();

            pnlBase.Controls.Add(pnlDashboard);
            pnlBase.Controls.Add(pnlProfile);

            pnlDashboard.Visible = true;
            pnlProfile.Visible = true;
            pnlAccount.Visible = true;
            pnlWithdraw_Deposit.Visible = false;
            pnlHistory.Visible = false;
            pnlCustomerRequest.Visible = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string searchText = txtSearch.Text;
                string connection = @"Data Source=Mavinda-PC\SQLEXPRESS;Initial Catalog=""NEXGEN Bank Management System"";Integrated Security=True;";
                string query = "Select * From Customers where NIC = '" + searchText + "' ";

                SqlConnection conn = new SqlConnection(connection);
                SqlDataAdapter adpter = new SqlDataAdapter(query, conn);

                DataSet dataSet = new DataSet();

                conn.Open();
                adpter.Fill(dataSet, "NIC");
                conn.Close();

                dgvCustomers.DataSource = dataSet;
                dgvCustomers.DataMember = "NIC";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(sender, e);
            }
        }

        private void dgvAccount_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string selectedCustomerID;
                string selectedAccountType;
                string selectedAccountID;
                string selectedBalance;
                string selectedStatus;

                selectedCustomerID = dgvAccount.Rows[e.RowIndex].Cells[0].Value.ToString();
                selectedAccountID = dgvAccount.Rows[e.RowIndex].Cells[1].Value.ToString();
                selectedAccountType = dgvAccount.Rows[e.RowIndex].Cells[2].Value.ToString();
                selectedBalance = dgvAccount.Rows[e.RowIndex].Cells[3].Value.ToString();
                selectedStatus = dgvAccount.Rows[e.RowIndex].Cells[4].Value.ToString();

                // Fill Data
                txtMA_CustomerID.Text = selectedCustomerID;
                txtMA_AccountType.Text = selectedAccountType;
                txtMA_AccountID.Text = selectedAccountID;
                txtMA_Balance.Text = selectedBalance;
                txtMA_Status.Text = selectedStatus;

                txtNIC.ReadOnly = true;
                txtMA_CustomerID.ReadOnly = true;
                txtMA_AccountType.ReadOnly = true;
                txtMA_AccountID.ReadOnly = true;

                // Show account status 
                if (txtMA_Status.Text == "Active")
                {
                    picMA_Status.FillColor = Color.Green;

                    if (txtMA_Status.Text == "Suspended")
                    {
                        picMA_Status.FillColor = Color.Yellow;

                        if (txtMA_Status.Text == "Closed")
                        {
                            picMA_Status.FillColor = Color.Red;
                        }
                    }
                }
            }
        }

        private void pnlAccount_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnMA_Reset_Click(object sender, EventArgs e)
        {
            ClearTextBoxesMAPanel();
        }

        private void btnMA_Edit_Click(object sender, EventArgs e)
        {
            if (txtMA_AccountID.Text != "")
            {
                cmbMA_Status.Enabled = true;
            }
            else
            {
                MessageBox.Show("Please Select befor Account", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnMA_Update_Click(object sender, EventArgs e)
        {
            try
            {
                string AccountID = txtMA_AccountID.Text;
                string CustomerID = txtMA_CustomerID.Text;
                string AccountType = txtMA_AccountType.Text;

                string UpdatedAccountStatus = txtMA_Status.Text;
                decimal.TryParse(txtMA_Balance.Text, out decimal Balance);

                string connection = @"Data Source=Mavinda-PC\SQLEXPRESS;Initial Catalog=""NEXGEN Bank Management System"";Integrated Security=True;";
                string query = "update Accounts set CustomerID = '" + CustomerID + "', AccountType = '" + AccountType + "', Balance = " + Balance + ", Status = '" + UpdatedAccountStatus + "' where AccountID = '" + AccountID + "' ";

                SqlConnection conn = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("This Account:" + AccountID + ", now " + UpdatedAccountStatus + "", "Record Updated!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //Method  - Function Call

                AccountDataLoadToDataGridView();
                dgvAccount.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pnlWithdraw_Deposit_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnWithdraw_Deposit_Click(object sender, EventArgs e)
        {
            pnlBase.Controls.Add(pnlDashboard);
            pnlBase.Controls.Add(pnlProfile);
            pnlBase.Controls.Add(pnlAccount);

            pnlDashboard.Visible = true;
            pnlProfile.Visible = true;
            pnlAccount.Visible = true;
            pnlWithdraw_Deposit.Visible = true;
            pnlHistory.Visible = false;
            pnlCustomerRequest.Visible = false;
        }

        private string GenerateTransactionID()
        {
            string prefix = "TRANS"; // Customize the prefix as needed
            int year = DateTime.Now.Year;
            string uniquePart = DateTime.Now.ToString("MMddHHmmss"); // Month, Day, Hour, Minute, Second
            return $"{prefix}{year}{uniquePart}";
        }



        private void btnWD_Process_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtWD_AccountID.Text != "")
                {
                    if (txtWD_NIC.Text != "")
                    {
                        if (cmbWD_TransactionType.SelectedItem != null)
                        {
                            if (txtWD_Amount.Text != "")
                            {
                                string CustomerAccountID = txtWD_AccountID.Text;

                                string Connection = @"Data Source=Mavinda-PC\SQLEXPRESS;Initial Catalog=""NEXGEN Bank Management System"";Integrated Security=True;";
                                string Query = "Select AccountID, CustomerID From Accounts Where AccountID = '" + txtWD_AccountID.Text + "'";

                                SqlConnection conn = new SqlConnection(Connection);
                                SqlDataAdapter sda = new SqlDataAdapter(Query, conn);
                                SqlCommand cmd = new SqlCommand(Query, conn);
                                DataTable dataTable = new DataTable();

                                conn.Open();
                                sda.Fill(dataTable);

                                if (dataTable.Rows.Count > 0)
                                {
                                    CustomerAccountID = txtWD_AccountID.Text;

                                    SqlDataReader reader = cmd.ExecuteReader();

                                    while (reader.Read())
                                    {
                                        // Transfer the NIC to Dashboard
                                        textBox1.Text = reader.GetValue(1).ToString();

                                        string CustomerNIC = txtWD_NIC.Text;
                                        string CustomerID = textBox1.Text;



                                        string query1 = "SELECT CustomerID, NIC FROM Customers WHERE CustomerID = @CustomerID AND NIC = @NIC";

                                        using (SqlConnection conn1 = new SqlConnection(Connection))
                                        {
                                            using (SqlCommand cmd1 = new SqlCommand(query1, conn1))
                                            {
                                                cmd1.Parameters.AddWithValue("@CustomerID", textBox1.Text);
                                                cmd1.Parameters.AddWithValue("@NIC", txtWD_NIC.Text);

                                                SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
                                                DataTable dataTable1 = new DataTable();
                                                sda1.Fill(dataTable1);


                                                if (dataTable1.Rows.Count > 0)
                                                {
                                                    string Query4 = "Select Status From Accounts Where AccountID = @AccountID";

                                                    SqlConnection conn4 = new SqlConnection(Connection);

                                                    using (SqlCommand cmd5 = new SqlCommand(Query4, conn4))
                                                    {
                                                        conn4.Open();
                                                        cmd5.Parameters.AddWithValue("@AccountID", txtWD_AccountID.Text);
                                                        string result1 = (string)cmd5.ExecuteScalar();

                                                        if (result1 == "Active")
                                                        {
                                                            if (cmbWD_TransactionType.SelectedItem.ToString() == "Deposit")
                                                            {
                                                                // Transaction save to Accounts Table & Update Balance
                                                                float CurrentAmount = 0;

                                                                string Query2 = "Select Balance From Accounts Where AccountID = @AccountID";

                                                                SqlConnection conn2 = new SqlConnection(Connection);

                                                                using (SqlCommand cmd2 = new SqlCommand(Query2, conn2))

                                                                {
                                                                    conn2.Open();
                                                                    cmd2.Parameters.AddWithValue("@AccountId", txtWD_AccountID.Text);
                                                                    object result = cmd2.ExecuteScalar();
                                                                    if (result != null)
                                                                    {
                                                                        CurrentAmount = Convert.ToSingle(result);
                                                                    }
                                                                }

                                                                float NewAmount = CurrentAmount + float.Parse(txtWD_Amount.Text);

                                                                string updateQuery = "Update Accounts SET Balance = @Balance WHERE AccountID = @AccountID";
                                                                using (SqlCommand cmd3 = new SqlCommand(updateQuery, conn2))
                                                                {
                                                                    cmd3.Parameters.AddWithValue("@Balance", NewAmount);
                                                                    cmd3.Parameters.AddWithValue("@AccountID", txtWD_AccountID.Text);
                                                                    cmd3.ExecuteNonQuery();
                                                                    conn2.Close();
                                                                }

                                                                // Transaction Save to Transactions Table
                                                                string TransactionID = GenerateTransactionID();
                                                                string TransactionType = cmbWD_TransactionType.SelectedItem.ToString();
                                                                float Amount = float.Parse(txtWD_Amount.Text);
                                                                string TransactionDate = dtpWD_TransactionDate.Text;

                                                                string Query3 = "Insert Into Transactions Values ('" + TransactionID + "', '" + TransactionType + "', '" + CustomerAccountID + "', " + Amount + ", '" + TransactionDate + "')";

                                                                SqlConnection conn3 = new SqlConnection(Connection);
                                                                SqlCommand cmd4 = new SqlCommand(Query3, conn3);

                                                                conn3.Open();
                                                                cmd4.ExecuteNonQuery();
                                                                conn3.Close();

                                                                MessageBox.Show("Transaction Successfully Completed.");

                                                            }
                                                            else
                                                            {
                                                                // Transaction save to Accounts Table & Update Balance
                                                                float CurrentAmount = 0;

                                                                string Query2 = "Select Balance From Accounts Where AccountID = @AccountID";
                                                                SqlConnection conn2 = new SqlConnection(Connection);
                                                                using (SqlCommand cmd2 = new SqlCommand(Query2, conn2))
                                                                {
                                                                    conn2.Open();
                                                                    cmd2.Parameters.AddWithValue("@AccountId", txtWD_AccountID.Text);
                                                                    object result = cmd2.ExecuteScalar();
                                                                    if (result != null)
                                                                    {
                                                                        CurrentAmount = Convert.ToSingle(result);
                                                                    }
                                                                }

                                                                float NewAmount = CurrentAmount - float.Parse(txtWD_Amount.Text);

                                                                string updateQuery = "Update Accounts SET Balance = @Balance WHERE AccountID = @AccountID";
                                                                using (SqlCommand cmd3 = new SqlCommand(updateQuery, conn2))
                                                                {
                                                                    cmd3.Parameters.AddWithValue("@Balance", NewAmount);
                                                                    cmd3.Parameters.AddWithValue("@AccountID", txtWD_AccountID.Text);
                                                                    cmd3.ExecuteNonQuery();
                                                                    conn2.Close();
                                                                }

                                                                // Transaction Save to Transactions Table
                                                                string TransactionID = GenerateTransactionID();
                                                                string TransactionType = cmbWD_TransactionType.SelectedItem.ToString();
                                                                float Amount = float.Parse(txtWD_Amount.Text);
                                                                string TransactionDate = dtpWD_TransactionDate.Text;

                                                                string Query3 = "Insert Into Transactions Values ('" + TransactionID + "', '" + TransactionType + "', '" + CustomerAccountID + "', " + Amount + ", '" + TransactionDate + "')";

                                                                SqlConnection conn3 = new SqlConnection(Connection);
                                                                SqlCommand cmd4 = new SqlCommand(Query3, conn3);

                                                                conn3.Open();
                                                                cmd4.ExecuteNonQuery();
                                                                conn3.Close();

                                                                MessageBox.Show("Transaction Successfully.");

                                                                ClearTextBoxesWDPanel();
                                                            }
                                                        }
                                                        else
                                                        {
                                                            MessageBox.Show("This Account Supended or Closed. Check Account Status.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBox.Show("Account and NIC don't match.");
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Account detail wrong");
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



        private void btnWD_Cancel_Click(object sender, EventArgs e)
        {
            ClearTextBoxesWDPanel();
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            TransactionsDataLoadToDataGridView();
            ClearTextBoxesHPanel();

            pnlBase.Controls.Add(pnlDashboard);
            pnlBase.Controls.Add(pnlProfile);
            pnlBase.Controls.Add(pnlAccount);
            pnlBase.Controls.Add(pnlWithdraw_Deposit);

            pnlDashboard.Visible = true;
            pnlProfile.Visible = true;
            pnlAccount.Visible = true;
            pnlWithdraw_Deposit.Visible = true;
            pnlHistory.Visible = true;
            pnlCustomerRequest.Visible = false;
        }

        private void dgvTransactions_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string selectedTransactionID;
                string selectedTransactionType;
                string selectedAccountID;
                string selectedAmount;
                string selectedTransactionDate;

                selectedTransactionID = dgvTransactions.Rows[e.RowIndex].Cells[0].Value.ToString();
                selectedTransactionType = dgvTransactions.Rows[e.RowIndex].Cells[1].Value.ToString();
                selectedAccountID = dgvTransactions.Rows[e.RowIndex].Cells[2].Value.ToString();
                selectedAmount = dgvTransactions.Rows[e.RowIndex].Cells[3].Value.ToString();
                selectedTransactionDate = dgvTransactions.Rows[e.RowIndex].Cells[4].Value.ToString();

                // Fill Data
                txtH_TransID.Text = selectedTransactionID;
                txtH_TransType.Text = selectedTransactionType;
                txtH_AccID.Text = selectedAccountID;
                txtH_Amount.Text = selectedAmount;
                txtH_TransDate.Text = selectedTransactionDate;
            }
        }

        private void TransactionsDataLoadToDataGridView()
        {
            try
            {
                string connection = @"Data Source=Mavinda-PC\SQLEXPRESS;Initial Catalog=""NEXGEN Bank Management System"";Integrated Security=True;";
                string query = "Select * From Transactions";

                SqlConnection conn = new SqlConnection(connection);
                SqlDataAdapter adpter = new SqlDataAdapter(query, conn);

                DataSet dataSet = new DataSet();

                conn.Open();
                adpter.Fill(dataSet, "TransactionID");
                conn.Close();

                dgvTransactions.DataSource = dataSet;
                dgvTransactions.DataMember = "TransactionID";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnH_Reset_Click(object sender, EventArgs e)
        {
            ClearTextBoxesHPanel();
            TransactionsDataLoadToDataGridView();
        }

        private void btnH_Search_Click(object sender, EventArgs e)
        {
            try
            {
                string searchText = txtH_Search.Text;
                string connection = @"Data Source=Mavinda-PC\SQLEXPRESS;Initial Catalog=""NEXGEN Bank Management System"";Integrated Security=True;";
                string query = "Select * From Transactions where AccountID = '" + searchText + "' ";

                SqlConnection conn = new SqlConnection(connection);
                SqlDataAdapter adpter = new SqlDataAdapter(query, conn);

                DataSet dataSet = new DataSet();

                conn.Open();
                adpter.Fill(dataSet, "AccountID");
                conn.Close();

                dgvCustomers.DataSource = dataSet;
                dgvCustomers.DataMember = "AccountID";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtH_Search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnH_Search_Click(sender, e);
            }
        }

        private void btnCR_Send_Click(object sender, EventArgs e)
        {
            try
            {
                string AccountID = txtCR_AccID.Text;
                string Fullname = txtCR_Fullname.Text;
                string ContactNO = txtCR_Contact.Text;
                string RequestSubmitDate = dtpCR_Date.Text;
                string NIC = txtCR_NIC.Text;
                string CustomerRequest = rtxtCR_Request.Text;

                if (AccountID != "")
                {
                    if (Fullname != "")
                    {
                        if (ContactNO != "")
                        {
                            if (NIC != "")
                            {
                                if (CustomerRequest != "")
                                {
                                    string Connection = @"Data Source=Mavinda-PC\SQLEXPRESS;Initial Catalog=""NEXGEN Bank Management System"";Integrated Security=True;";
                                    string Query = "Insert Into Customer_Requests Values ('" + AccountID + "', '" + Fullname + "', '" + ContactNO + "', '" + NIC + "', '" + RequestSubmitDate + "', '" + CustomerRequest + "');";

                                    SqlConnection conn = new SqlConnection(Connection);
                                    SqlCommand cmd = new SqlCommand(Query, conn);

                                    conn.Open();
                                    cmd.ExecuteNonQuery();
                                    conn.Close();

                                    //User Friendly
                                    MessageBox.Show("Request Send Sucessfully", "Record Inserted", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    ClearTextBoxesCRPanel();
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

        private void btnCR_Reset_Click(object sender, EventArgs e)
        {
            ClearTextBoxesCRPanel();
        }

        private void btnCustomerRequest_Click(object sender, EventArgs e)
        {
            pnlBase.Controls.Add(pnlDashboard);
            pnlBase.Controls.Add(pnlProfile);
            pnlBase.Controls.Add(pnlAccount);
            pnlBase.Controls.Add(pnlWithdraw_Deposit);
            pnlBase.Controls.Add(pnlHistory);

            pnlDashboard.Visible = true;
            pnlProfile.Visible = true;
            pnlAccount.Visible = true;
            pnlWithdraw_Deposit.Visible = true;
            pnlHistory.Visible = true;
            pnlCustomerRequest.Visible = true;
        }

        private void btnMA_Search_Click(object sender, EventArgs e)
        {
            try
            {
                string searchText = txtH_Search.Text;
                string connection = @"Data Source=Mavinda-PC\SQLEXPRESS;Initial Catalog=""NEXGEN Bank Management System"";Integrated Security=True;";
                string query = "Select * From Accounts where AccountID = '" + searchText + "' ";

                SqlConnection conn = new SqlConnection(connection);
                SqlDataAdapter adpter = new SqlDataAdapter(query, conn);

                DataSet dataSet = new DataSet();

                conn.Open();
                adpter.Fill(dataSet, "AccountID");
                conn.Close();

                dgvCustomers.DataSource = dataSet;
                dgvCustomers.DataMember = "AccountID";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}




