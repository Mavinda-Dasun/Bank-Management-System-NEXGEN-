using Guna.UI2.WinForms;
using Guna.UI2.WinForms.Suite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bank_Management_System
{
    public partial class Manager_Main_Dashboard : Form
    {
        public static Manager_Main_Dashboard Instance;

        public Guna2TextBox GetLoggedUserNIC;

        private int currentImageIndex = 0;
        private Image[] images = new Image[4];

        string fileName;

        public Manager_Main_Dashboard()
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
            pnlBase.Controls.Add(pnlEmployee);
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

        private void Manager_Main_Dashboard_Load(object sender, EventArgs e)
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
        private void btnCustomizeBack_Click(object sender, EventArgs e)
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

        private void btnCustomizeDelete_Click(object sender, EventArgs e)
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

        private void btnCustomizeUpdate_Click(object sender, EventArgs e)
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

        private void btnCustomizeSave_Click(object sender, EventArgs e)
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

        private void btnCustomizeReset_Click(object sender, EventArgs e)
        {
            ClearTextBoxesForUPICPanel();
        }

        private void dgvCustomers_CellContentClick(object sender, DataGridViewCellEventArgs e)
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
            pnlEmployee.Visible = false;
        }

        private void btnCards_Click(object sender, EventArgs e)
        {
            pnlBase.Controls.Add(pnlDashboard);

            pnlDashboard.Visible = true;
            pnlProfile.Visible = true;
            pnlAccount.Visible = false;
            pnlWithdraw_Deposit.Visible = false;
            pnlHistory.Visible = false;
            pnlCustomerRequest.Visible = false;
            pnlEmployee.Visible = false;

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

        private void btnAccountStatus_Click(object sender, EventArgs e)
        {
            AccountDataLoadToDataGridView();

            ClearTextBoxesMAPanel();

            pnlBase.Controls.Add(pnlDashboard);
            pnlBase.Controls.Add(pnlProfile);

            pnlDashboard.Visible = true;
            pnlProfile.Visible = true;
            pnlAccount.Visible = true;
            pnlWithdraw_Deposit.Visible = false;
            pnlHistory.Visible = false;
            pnlCustomerRequest.Visible = false;
            pnlEmployee.Visible = false;
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

        private void btnSearch_KeyDown(object sender, KeyEventArgs e)
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

                selectedAccountID = dgvAccount.Rows[e.RowIndex].Cells[0].Value.ToString();
                selectedCustomerID = dgvAccount.Rows[e.RowIndex].Cells[1].Value.ToString();
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

        private void btnMA_Reset_Click(object sender, EventArgs e)
        {
            ClearTextBoxesMAPanel();
        }

        private void btnMA_Update_Click(object sender, EventArgs e)
        {
            try
            {
                string AccountID = txtMA_AccountID.Text;
                string CustomerID = txtMA_CustomerID.Text;
                string AccountType = txtMA_AccountType.Text;

                string UpdatedAccountStatus = cmbMA_Status.Text.ToString();
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
                ClearTextBoxesMAPanel();
                AccountDataLoadToDataGridView();
                dgvAccount.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
            pnlEmployee.Visible = false;
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
            pnlEmployee.Visible = false;
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



        private void btnH_Search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnH_Search_Click(sender, e);
            }
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
            catch (Exception ex)
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
            pnlEmployee.Visible = false;
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

        private void pnlAccountOpen_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            EmployeeDataLoadToDataGridView();

            pnlBase.Controls.Add(pnlDashboard);
            pnlBase.Controls.Add(pnlProfile);
            pnlBase.Controls.Add(pnlAccount);
            pnlBase.Controls.Add(pnlWithdraw_Deposit);
            pnlBase.Controls.Add(pnlHistory);
            pnlBase.Controls.Add(pnlCustomerRequest);

            pnlDashboard.Visible = true;
            pnlProfile.Visible = true;
            pnlAccount.Visible = true;
            pnlWithdraw_Deposit.Visible = true;
            pnlHistory.Visible = true;
            pnlCustomerRequest.Visible = true;
            pnlEmployee.Visible = true;
        }

        private void EmployeeDataLoadToDataGridView()
        {
            try
            {
                string connection = @"Data Source=Mavinda-PC\SQLEXPRESS;Initial Catalog=""NEXGEN Bank Management System"";Integrated Security=True;";
                string query = "Select * From Employees";

                SqlConnection conn = new SqlConnection(connection);
                SqlDataAdapter adpter = new SqlDataAdapter(query, conn);

                DataSet dataSet = new DataSet();

                conn.Open();
                adpter.Fill(dataSet, "EmployeeID");
                conn.Close();

                dgvEmployee.DataSource = dataSet;
                dgvEmployee.DataMember = "EmployeeID";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void btnEIC_AddEmployee_Click(object sender, EventArgs e)
        {
            if (pnlAddEmployee.Visible == false)
            {
                pnlEmployeeCustomize.Visible = true;
                pnlAddEmployee.Visible = true;
            }
        }

        private void btnEIC_EditEmployee_Click(object sender, EventArgs e)
        {
            EmployeeDataLoadToDataGridView();

            if (pnlEmployeeCustomize.Visible == false)
            {
                pnlEmployeeCustomize.Visible = true;
                pnlAddEmployee.Visible = false;

                if (guna2TextBox18.Visible == false)
                {
                    guna2TextBox18.Visible = true;
                    guna2CircleButton1.Visible = true;
                }
            }
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {

        }

        private void ClearTextBoxesForEICPanel()
        {
            txtEIC_Firstname.Clear();
            txtEIC_Lastname.Clear();
            txtEIC_Username.Clear();
            txtEIC_Password.Clear();
            txtEIC_NIC.Clear();
            dtpEIC_DOB = null;
            txtEIC_Mobile.Clear();
            txtEIC_Email.Clear();
            txtEIC_Address.Clear();
        }

        private void btnEIC_Reset_Click(object sender, EventArgs e)
        {
            ClearTextBoxesForEICPanel();
        }

        private void btnEIC_Edit_Click(object sender, EventArgs e)
        {
            if (txtEIC_NIC.Text != "")
            {
                txtEIC_Firstname.Enabled = true;
                txtEIC_Lastname.Enabled = true;
                txtEIC_Username.Enabled = true;
                txtEIC_Password.Enabled = true;
                dtpEIC_DOB.Enabled = true;
                txtEIC_Mobile.Enabled = true;
                txtEIC_Email.Enabled = true;
                txtEIC_Address.Enabled = true;
            }
            else
            {
                MessageBox.Show("Please Select befor Employee", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnEIC_Update_Click(object sender, EventArgs e)
        {
            try
            {
                string UpdatedFirstName = txtEIC_Firstname.Text;
                string UpdatedLastName = txtEIC_Lastname.Text;
                string UpdatedUserName = txtEIC_Username.Text;
                string UpdatedPassword = txtEIC_Password.Text;
                string UpdatedDOB = dtpEIC_DOB.Text;
                string UpdatedNIC = txtEIC_NIC.Text;
                string UpdatedPhoneNumber = txtEIC_Mobile.Text;
                string UpdatedEmail = txtEIC_Email.Text;
                string UpdatedAddress = txtEIC_Address.Text;

                string connection = @"Data Source=Mavinda-PC\SQLEXPRESS;Initial Catalog=""NEXGEN Bank Management System"";Integrated Security=True;";
                string query = "update Employees Set FirstName = '" + UpdatedFirstName + "', LastName = '" + UpdatedLastName + "', DateOfBirth = '" + UpdatedDOB + "', PhoneNumber = '" + UpdatedPhoneNumber + "', Email = '" + UpdatedEmail + "', Address = '" + UpdatedAddress + "', Username = '" + UpdatedUserName + "', Password = '" + UpdatedPassword + "' Where NIC = '" + UpdatedNIC + "' ";

                SqlConnection conn = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("This Employee:" + UpdatedNIC + ", Personal Information Updated", "Record Updated!",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                //Method  - Function Call
                ClearTextBoxesForUPICPanel();
                EmployeeDataLoadToDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEIC_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                string EmployeeToBeDeleted = txtSearch.Text;

                DialogResult userResult = MessageBox.Show("Are you sure to Delete this Employee: " + EmployeeToBeDeleted + " ?", "User Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (userResult == DialogResult.Yes)
                {
                    if (EmployeeToBeDeleted != "")
                    {
                        string Connection = @"Data Source=Mavinda-PC\SQLEXPRESS;Initial Catalog=""NEXGEN Bank Management System"";Integrated Security=True;";
                        string Query = "Delete From Employee Where NIC = '" + EmployeeToBeDeleted + "' ";

                        SqlConnection conn = new SqlConnection(Connection);
                        SqlCommand cmd = new SqlCommand(Query, conn);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();

                        MessageBox.Show("This Employee: " + EmployeeToBeDeleted + ", Successfully Deleted.", "Employee Deleted!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        //Method  - Function Call
                        ClearTextBoxesForUPICPanel();
                        EmployeeDataLoadToDataGridView();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEIC_Back_Click(object sender, EventArgs e)
        {
            ClearTextBoxesForEICPanel();

            if (pnlEmployeeCustomize.Visible == true)
            {
                pnlEmployeeCustomize.Visible = false;

                if (guna2TextBox18.Visible == true)
                {
                    guna2TextBox18.Visible = false;
                    guna2CircleButton1.Visible = false;
                }
            }
        }

        private void dgvEmployee_CellContentClick(object sender, DataGridViewCellEventArgs e)
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

                selectedFirstName = dgvEmployee.Rows[e.RowIndex].Cells[1].Value.ToString();
                selectedLastName = dgvEmployee.Rows[e.RowIndex].Cells[2].Value.ToString();
                selectedDOB = dgvEmployee.Rows[e.RowIndex].Cells[3].Value.ToString();
                selectedNIC = dgvEmployee.Rows[e.RowIndex].Cells[4].Value.ToString();
                selectedPhoneNumber = dgvEmployee.Rows[e.RowIndex].Cells[6].Value.ToString();
                selectedEmail = dgvEmployee.Rows[e.RowIndex].Cells[7].Value.ToString();
                selectedAddress = dgvEmployee.Rows[e.RowIndex].Cells[8].Value.ToString();
                selectedUsername = dgvEmployee.Rows[e.RowIndex].Cells[10].Value.ToString();
                selectedPassword = dgvEmployee.Rows[e.RowIndex].Cells[11].Value.ToString();

                //Fill Data
                txtEIC_Firstname.Text = selectedFirstName;
                txtEIC_Lastname.Text = selectedLastName;
                dtpEIC_DOB.Text = selectedDOB;
                txtEIC_NIC.Text = selectedNIC;
                txtEIC_Mobile.Text = selectedPhoneNumber;
                txtEIC_Email.Text = selectedEmail;
                txtEIC_Address.Text = selectedAddress;
                txtEIC_Username.Text = selectedUsername;
                txtEIC_Password.Text = selectedPassword;

                txtNIC.ReadOnly = true;
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd2 = new OpenFileDialog() { Filter = "JPEG|*.jpg", ValidateNames = true, Multiselect = false })

                if (ofd2.ShowDialog() == DialogResult.OK)
                {
                    fileName = ofd2.FileName;
                    lblFile.Text = fileName;
                    picAE.Image = Image.FromFile(fileName);
                }
        }

        private string GenerateEmployeeID()
        {
            string prefix = "EMP"; // Customize the prefix as needed
            int year = DateTime.Now.Year;
            string uniquePart = DateTime.Now.ToString("MMddHHmmss"); // Month, Day, Hour, Minute, Second
            return $"{prefix}{year}{uniquePart}";
        }

        private void GenarateEmployeeGender()
        {
            string GenderForNIC = txtAE_NIC.Text;
            if (GenderForNIC.Length == 12)
            {
                // We need digits from positions 5 to 8, which correspond to indices 4 to 7 (0-based index)
                int startIndex = 4; // Starting index (0-based)
                int length = 3; // Length of the substring (4 digits)

                // Extract the substring (digits from position 5-8)
                string selectedDigits = txtAE_NIC.Text.Substring(startIndex, length);

                // Display the selected digits in Label2
                txtAE_Gender.Text = selectedDigits;

                string GenderC = txtAE_Gender.Text;
                int GenderCode = int.Parse(GenderC);

                if (GenderCode > 500)
                {
                    string Gender;
                    Gender = "Female";
                    txtAE_Gender.Text = null;
                    txtAE_Gender.Text = Gender;
                }
                else
                {
                    if (GenderCode <= 500)
                    {
                        string Gender;
                        Gender = "Male";
                        txtAE_Gender.Text = null;
                        txtAE_Gender.Text = Gender;
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
                    string selectedDigits = txtAE_NIC.Text.Substring(startIndex, length);

                    // Display the selected digits in Label2
                    txtAE_Gender.Text = selectedDigits;

                    string GenderC = txtAE_Gender.Text;
                    int GenderCode = int.Parse(GenderC);

                    if (GenderCode <= 500)
                    {
                        string Gender;
                        Gender = "Male";
                        txtAE_Gender.Text = null;
                        txtAE_Gender.Text = Gender;
                    }
                    else
                    {
                        if (GenderCode > 500)
                        {
                            string Gender;
                            Gender = "Female";
                            txtAE_Gender.Text = null;
                            txtAE_Gender.Text = Gender;
                        }
                    }
                }
            }
        }

        private void btnAE_Save_Click(object sender, EventArgs e)
        {
            DateTime BirthDate = DateTime.Parse(dtpAE_DOB.Text);
            DateTime CurrentDate = DateTime.Today;
            int Age = CurrentDate.Year - BirthDate.Year;
            string str = Age.ToString();

            if (txtAE_FirstName.Text != "" && txtAE_LastName.Text != "")
            {
                string NewCustomerID = GenerateCustomerID();

                if (txtAE_UserName.Text != "")
                {
                    // Check if the UserName already used
                    string Connection = @"Data Source=Mavinda-PC\SQLEXPRESS;Initial Catalog=""NEXGEN Bank Management System"";Integrated Security=True;";
                    string Query = "Select Username From Customers Where Username = @Username";

                    SqlConnection conn = new SqlConnection(Connection);
                    SqlCommand cmd = new SqlCommand(Query, conn);
                    cmd.Parameters.AddWithValue("Username", txtAE_UserName.Text);

                    SqlDataReader reader;
                    conn.Open();
                    reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        conn.Close();
                        label1.Text = null;
                        label1.Text = "That username is already being used.";
                    }
                    else
                    {
                        conn.Close();

                        // Check if the UserName already used                        
                        string Query1 = "Select Username From Employees Where Username = @Username";

                        SqlConnection conn1 = new SqlConnection(Connection);
                        SqlCommand cmd1 = new SqlCommand(Query1, conn1);
                        cmd1.Parameters.AddWithValue("Username", txtAE_UserName.Text);

                        SqlDataReader reader1;
                        conn1.Open();
                        reader1 = cmd1.ExecuteReader();

                        if (reader1.Read())
                        {
                            conn1.Close();
                            label1.Text = null;
                            label1.Text = "That username is already being used.";
                        }
                        else
                        {
                            conn1.Close();

                            if (txtAE_NIC.Text != "")
                            {
                                // Check if the UserName already used                                
                                string Query2 = "Select NIC From Employees Where NIC = @NIC";
                                
                                SqlConnection conn2 = new SqlConnection(Connection);
                                SqlCommand cmd2 = new SqlCommand(Query2, conn2);
                                cmd2.Parameters.AddWithValue("NIC", txtAE_NIC.Text);

                                SqlDataReader reader2;
                                conn2.Open();
                                reader2 = cmd2.ExecuteReader();

                                if (reader2.Read())
                                {
                                    conn.Close();
                                    label1.Text = null;
                                    label1.Text = "This Customer has already Account.";
                                }
                                else
                                {
                                    conn.Close();

                                    if (txtAE_NIC.Text.Length == 12 || txtAE_NIC.Text.Length == 10)
                                    {
                                        if (txtAE_Mobile.Text != "")
                                        {
                                            // Accounts Details
                                            string EmployeeID = GenerateEmployeeID();
                                            string Role = "Employee";

                                            // Customer Details
                                            GenarateEmployeeGender();

                                            Image pimg = picAE.Image;
                                            ImageConverter Converter1 = new ImageConverter();
                                            var ImageConvert1 = Converter1.ConvertTo(pimg, typeof(byte[]));

                                            string Query3 = "Insert Into Employees (EmployeeID, FirstName, LastName, DateOfBirth, NIC, Gender, PhoneNumber, Email, Address, EmployeePhoto, Username, Password, FileName, Role) Values (@EmployeeID, @FirstName, @LastName, @DateOfBirth, @NIC, @Gender, @PhoneNumber, @Email, @Address, @EmployeePhoto, @Username, @Password, @FileName, @Role)";

                                            SqlConnection conn3 = new SqlConnection(Connection);
                                            SqlCommand cmd3 = new SqlCommand(Query3, conn3);

                                            cmd3.CommandType = CommandType.Text;
                                            cmd3.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                                            cmd3.Parameters.AddWithValue("@FirstName", txtAE_FirstName.Text);
                                            cmd3.Parameters.AddWithValue("@LastName", txtAE_LastName.Text);
                                            cmd3.Parameters.AddWithValue("@DateOfBirth", dtpAE_DOB.Text);
                                            cmd3.Parameters.AddWithValue("@NIC", txtAE_NIC.Text);
                                            cmd3.Parameters.AddWithValue("@Gender", txtAE_Gender.Text);
                                            cmd3.Parameters.AddWithValue("@PhoneNumber", txtAE_Mobile.Text);
                                            cmd3.Parameters.AddWithValue("@Email", txtAE_Email.Text);
                                            cmd3.Parameters.AddWithValue("@Address", txtAE_Address.Text);
                                            cmd3.Parameters.AddWithValue("@EmployeePhoto", ImageConvert1);
                                            cmd3.Parameters.AddWithValue("@Username", txtAE_UserName.Text);
                                            cmd3.Parameters.AddWithValue("@Password", txtAE_Password.Text);
                                            cmd3.Parameters.AddWithValue("@FileName", lblFile.Text);
                                            cmd3.Parameters.AddWithValue("@Role", Role);

                                            conn3.Open();
                                            cmd3.ExecuteNonQuery();
                                            conn3.Close();

                                            MessageBox.Show("New Employee Added Successfully");

                                            ClearTextBoxesForAEPanel();
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

        private void btnAE_Reset_Click(object sender, EventArgs e)
        {
            ClearTextBoxesForAEPanel();
        }

        private void btnAE_Back_Click(object sender, EventArgs e)
        {
            ClearTextBoxesForAEPanel();

            if (pnlAddEmployee.Visible == true)
            {
                pnlEmployeeCustomize.Visible = false;
                pnlAddEmployee.Visible = false;
            }
        }

        private void ClearTextBoxesForAEPanel()
        {
            txtAE_FirstName.Clear();
            txtAE_LastName.Clear();
            txtAE_UserName.Clear();
            txtAE_Password.Clear();
            txtAE_NIC.Clear();
            txtAE_Gender.Clear();
            dtpAE_DOB = null;
            txtAE_Mobile.Clear();
            txtAE_Email.Clear();
            txtAE_Address.Clear();
            
            lblFile = null;
            if (picAE.Image != null)
            {
                picAE.Image.Dispose();
                picAE.Image = null;
            }

        }
    }
}
