using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics.Eventing.Reader;

namespace Bank_Management_System
{
    public partial class frmLogIn_SignUp : Form
    {
        public frmLogIn_SignUp()
        {
            InitializeComponent();
        }

        private void Exit_Application()
        {
            DialogResult userResult = MessageBox.Show("Are you sure you want to exit?", "Exit?", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (userResult == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void ClearTextBoxes()
        {
            txtL_UserName.Clear();
            txtL_Password.Clear();
            txtF_UserName.Clear();
            txtF_NIC.Clear();
            txtF_Password.Clear();
            txtF_PassConfirm.Clear();
            txtS_FirstName.Clear();
            txtS_LastName.Clear();
            txtS_UserName.Clear();
            txtPassword.Clear();
            txtS_NIC.Clear();
            dtpS_DOB.Text = null;
            txtS_Mobile.Clear();
            txtS_Email.Clear();
            txtEmailOTP.Clear();
            txtShowFirstName.Clear();
            txtShowLastName.Clear();
            txtShowUserName.Clear();
            txtShowPassword.Clear();
            txtShowNIC.Clear();
            txtShowDOB.Clear();
            txtShowMobile.Clear();
            txtShowEmail.Clear();
        }

        private void ClearErrorLabels()
        {
            lblLoginError.Text = null;
            lblForgotPassError.Text = null;
            lblSignUpError1.Text = null;
            lblSignUpError2.Text = null;
            lblSignUpError3.Text = null;
            lblSignUpError4.Text = null;
            lblSignUpError5.Text = null;
            lblSignUpError6.Text = null;
            lblSignUpError7.Text = null;
            lblSignUpError8.Text = null;
            lblSignUpError9.Text = null;
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            try
            {

                lblLoginError.Text = null;

                string UserEnterUserName = txtL_UserName.Text;
                string UserEnterPassword = txtL_Password.Text;


                if (UserEnterUserName != "" && UserEnterPassword != "")
                {
                    // Check UserName and Password for login from database
                    string connection = @"Data Source=Mavinda-PC\SQLEXPRESS;Initial Catalog=""NEXGEN Bank Management System"";Integrated Security=True;";
                    string query = "Select Username, Password From Customers Where Username COLLATE Latin1_General_BIN = '" + txtL_UserName.Text + "' And Password COLLATE Latin1_General_BIN = '" + txtL_Password.Text + "'";

                    SqlConnection conn = new SqlConnection(connection);
                    SqlDataAdapter sda = new SqlDataAdapter(query, conn);
                    DataTable dataTable = new DataTable();

                    conn.Open();
                    sda.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        string LoggedUserNIC;

                        // Get Logged User's NIC from database                        
                        string Query1 = "Select NIC From Customers Where UserName = '" + UserEnterUserName + "'";

                        SqlCommand cmd = new SqlCommand(Query1, conn);
                        SqlDataReader reader1 = cmd.ExecuteReader();

                        if (reader1.Read())
                        {
                            Customers_Main_Dashboard Customers_MainDashboard = new Customers_Main_Dashboard();
                            // Transfer the NIC to Dashboard
                            LoggedUserNIC = reader1.GetValue(0).ToString();
                            Customers_Main_Dashboard.Instance.GetLoggedUserNIC.Text = LoggedUserNIC;

                            // Load the Customer Dashboard
                            reader1.Close();
                            conn.Close();
                            Customers_MainDashboard.Show();
                            this.Hide();
                        }
                        else
                        {
                            
                        }
                        


                    }
                    else
                    {
                        // Check UserName and Password for login from database
                        string connection2 = @"Data Source=Mavinda-PC\SQLEXPRESS;Initial Catalog=""NEXGEN Bank Management System"";Integrated Security=True;";
                        string query2 = "Select Username, Password From Employees Where Username COLLATE Latin1_General_BIN = '" + txtL_UserName.Text + "' And Password COLLATE Latin1_General_BIN = '" + txtL_Password.Text + "'";

                        SqlConnection conn2 = new SqlConnection(connection2);
                        SqlDataAdapter sda2 = new SqlDataAdapter(query2, conn2);
                        DataTable dataTable2 = new DataTable();

                        conn2.Open();
                        sda2.Fill(dataTable2);

                        if (dataTable2.Rows.Count > 0)
                        {
                            UserEnterUserName = txtL_UserName.Text;
                            UserEnterPassword = txtL_Password.Text;

                            // Load the Customer Dashboard
                            Employee_Main_Dashboard Employees_MainDashboard = new Employee_Main_Dashboard();
                            Manager_Main_Dashboard manager_Main_Dashboard = new Manager_Main_Dashboard();

                            // Get Logged User's NIC from database                        
                            string Query3 = "Select Role From Employees Where UserName = '" + UserEnterUserName + "'";

                            SqlCommand cmd3 = new SqlCommand(Query3, conn2);
                            SqlDataReader reader3 = cmd3.ExecuteReader();

                            if (reader3.Read())
                            {
                                // Transfer the NIC to Dashboard
                                string LoggedUserRole = reader3.GetValue(0).ToString();
                                if (LoggedUserRole == "Manager")
                                {
                                    conn2.Close();
                                    manager_Main_Dashboard.Show();
                                    this.Hide();
                                }
                                else
                                {
                                    conn2.Close();
                                    Employees_MainDashboard.Show();
                                    this.Hide();
                                }

                            }
                        }
                        else
                        {
                            lblLoginError.Text = "Please enter username and password.";
                        }
                    }
                }
                else
                {

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    
        private void btnClose0_Click(object sender, EventArgs e)
        {
            Exit_Application();
        }

        private void btnL_PassShow_Click(object sender, EventArgs e)
        {
            if (txtL_Password.PasswordChar == '●')
            {
                txtL_Password.PasswordChar = '\0';                          // Show the password
                btnL_PassShow.Image = Properties.Resources.PassShow;                                                           // Change icon btnShowPassword 
            }
            else
            {
                txtL_Password.PasswordChar = '●';                           // Hide the password
                btnL_PassShow.Image = Properties.Resources.PassHide;                                                            // Change icon btnShowPassword
            }
        }

        private void btnForgotPass_Click(object sender, EventArgs e)
        {
            ClearTextBoxes();
            ClearErrorLabels();

            pnlLogIn.Visible = true;
            pnlForgetPassword.Visible = true;
            pnlSignUp.Visible = false;
            pnlSignUp1.Visible = false;
            pnlSignUp2.Visible = false;
            pnlSignUp3.Visible = false;
            pnlSignUp4.Visible = false;
            pnlSignUpSuccess.Visible = false;
            pnlSignUpError.Visible = false;
        }

        private void btnUpdatePassword_Click(object sender, EventArgs e)
        {
            try
            {
                ClearErrorLabels();

                string UserEnterUserName = txtF_UserName.Text;
                string UserEnterNIC = txtF_NIC.Text;
                string UserEnterPassword = txtF_Password.Text;
                string UserEnterConfirmPassword = txtF_PassConfirm.Text;

                // Check UserName and Password for login from database
                string connection = @"Data Source=Mavinda-PC\SQLEXPRESS;Initial Catalog=""NEXGEN Bank Management System"";Integrated Security=True;";
                string query = "Select UserName, NIC From Customers Where UserName COLLATE Latin1_General_BIN = '" + txtF_UserName.Text + "' And NIC = '" + txtF_NIC.Text + "'";

                SqlConnection conn = new SqlConnection(connection);
                SqlDataAdapter sda = new SqlDataAdapter(query, conn);
                DataTable dataTable = new DataTable();

                conn.Open();
                sda.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    UserEnterUserName = txtF_UserName.Text;
                    UserEnterNIC = txtF_NIC.Text;

                    if (UserEnterPassword != "" && UserEnterConfirmPassword != "")
                    {
                        if (UserEnterPassword == UserEnterConfirmPassword)
                        {
                            string query1 = "Update Customer_Details Set Password = '" + UserEnterConfirmPassword + "' Where NIC = '" + UserEnterNIC + "'";

                            SqlCommand cmd = new SqlCommand(query1, conn);
                            cmd.ExecuteNonQuery();
                            conn.Close();

                            ClearTextBoxes();
                            ClearErrorLabels();

                            pnlLogIn.Visible = true;
                            pnlForgetPassword.Visible = false;
                            pnlSignUp.Visible = false;
                            pnlSignUp1.Visible = false;
                            pnlSignUp2.Visible = false;
                            pnlSignUp3.Visible = false;
                            pnlSignUp4.Visible = false;
                            pnlSignUpSuccess.Visible = false;
                            pnlSignUpError.Visible = false;
                        }
                        else
                        {
                            lblForgotPassError.Text = null;
                            lblForgotPassError.Text = "Doesn't match passwords.";
                        }
                    }
                    else
                    {
                        lblForgotPassError.Text = null;
                        lblForgotPassError.Text = "Please enter your new password.";
                    }
                }
                else
                {
                    lblForgotPassError.Text = "Doesn't match you entered username and nic.";
                    ClearTextBoxes();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnF_Cancel_Click(object sender, EventArgs e)
        {
            ClearTextBoxes();
            ClearErrorLabels();

            pnlLogIn.Visible = true;
            pnlForgetPassword.Visible = false;
            pnlSignUp.Visible = false;
            pnlSignUp1.Visible = false;
            pnlSignUp2.Visible = false;
            pnlSignUp3.Visible = false;
            pnlSignUp4.Visible = false;
            pnlSignUpSuccess.Visible = false;
            pnlSignUpError.Visible = false;
        }

        private void btnClose1_Click(object sender, EventArgs e)
        {
            Exit_Application();
        }

        private void btnF_PassShow_Click(object sender, EventArgs e)
        {
            if (txtF_Password.PasswordChar == '●')
            {
                txtF_Password.PasswordChar = '\0';                          // Show the password
                btnF_PassShow.Image = Properties.Resources.PassShow;                                                            // Change icon btnShowPassword 
            }
            else
            {
                txtF_Password.PasswordChar = '●';                           // Hide the password
                btnF_PassShow.Image = Properties.Resources.PassHide;                                                           // Change icon btnShowPassword
            }
        }

        private void btnF_PassConfirmShow_Click(object sender, EventArgs e)
        {
            if (txtF_PassConfirm.PasswordChar == '●')
            {
                txtF_PassConfirm.PasswordChar = '\0';                          // Show the password
                btnF_PassConfirmShow.Image = Properties.Resources.PassShow;                                                               // Change icon btnShowPassword 
            }
            else
            {
                txtF_PassConfirm.PasswordChar = '●';                           // Hide the password
                btnF_PassConfirmShow.Image = Properties.Resources.PassHide;                                                               // Change icon btnShowPassword
            }
        }

        private void btnL_SignIn_Click(object sender, EventArgs e)
        {

        }

        private void btnL_SignUP_Click(object sender, EventArgs e)
        {
            ClearTextBoxes();
            ClearErrorLabels();

            pnlLogIn.Visible = true;
            pnlForgetPassword.Visible = true;
            pnlSignUp.Visible = true;
            pnlSignUp1.Visible = false;
            pnlSignUp2.Visible = false;
            pnlSignUp3.Visible = false;
            pnlSignUp4.Visible = false;
            pnlSignUpSuccess.Visible = false;
            pnlSignUpError.Visible = false;

            btnL_SignUP.Checked = false;
        }

        private void frmLogIn_SignUp_Load(object sender, EventArgs e)
        {
            this.Text = "NEXGEN Bank";

            ClearTextBoxes();
            ClearErrorLabels();

            pnlLogIn.Visible = true;
            pnlForgetPassword.Visible = false;
            pnlSignUp.Visible = false;
            pnlSignUp1.Visible = false;
            pnlSignUp2.Visible = false;
            pnlSignUp3.Visible = false;
            pnlSignUp4.Visible = false;
            pnlSignUpSuccess.Visible = false;
            pnlSignUpError.Visible = false;
        }

        private void btnS_LogIn_Click(object sender, EventArgs e)
        {
            ClearTextBoxes();
            ClearErrorLabels();

            pnlLogIn.Visible = true;
            pnlForgetPassword.Visible = false;
            pnlSignUp.Visible = false;
            pnlSignUp1.Visible = false;
            pnlSignUp2.Visible = false;
            pnlSignUp3.Visible = false;
            pnlSignUp4.Visible = false;
            pnlSignUpSuccess.Visible = false;
            pnlSignUpError.Visible = false;

            btnS_LogIn.Checked = false;
        }

        private void btnS_SignUP_Click(object sender, EventArgs e)
        {

        }

        private void btnCreateAccount_Click(object sender, EventArgs e)
        {
            try
            {
                ClearErrorLabels();

                string FillFirstName = txtS_FirstName.Text;
                string FillLastName = txtS_LastName.Text;
                string FillUserName = txtS_UserName.Text;
                string FillPassword = txtPassword.Text;

                // Check if the UserName entered or empty
                if (FillFirstName != "")
                {
                    lblSignUpError1.Text = null;

                    if (FillLastName != "")
                    {
                        lblSignUpError2.Text = null;

                        if (FillUserName != "")
                        {
                            lblSignUpError3.Text = null;

                            // Check if the UserName already used
                            string Connection = @"Data Source=Mavinda-PC\SQLEXPRESS;Initial Catalog=""NEXGEN Bank Management System"";Integrated Security=True;";
                            string Query = "Select Username From Customers Where Username = @Username";

                            SqlConnection conn = new SqlConnection(Connection);
                            SqlCommand cmd = new SqlCommand(Query, conn);
                            cmd.Parameters.AddWithValue("Username", txtS_UserName.Text);

                            SqlDataReader reader1;
                            conn.Open();
                            reader1 = cmd.ExecuteReader();

                            if (reader1.Read())
                            {
                                conn.Close();
                                lblSignUpError3.Text = null;
                                lblSignUpError3.Text = "That username is already being used.";
                            }
                            else
                            {
                                lblSignUpError3.Text = null;

                                // Check if the Password entered or empty
                                if (FillPassword != "")
                                {
                                    if (FillPassword.Length >= 8)
                                    {
                                        ClearErrorLabels();

                                        pnlLogIn.Visible = true;
                                        pnlForgetPassword.Visible = true;
                                        pnlSignUp.Visible = true;
                                        pnlSignUp1.Visible = true;
                                        pnlSignUp2.Visible = false;
                                        pnlSignUp3.Visible = false;
                                        pnlSignUp4.Visible = false;
                                        pnlSignUpSuccess.Visible = false;
                                        pnlSignUpError.Visible = false;

                                    }
                                    else
                                    {
                                        lblSignUpError4.Text = null;
                                        lblSignUpError4.Text = "Password must be 8 or more characters long.";
                                    }
                                }
                                else
                                {
                                    lblSignUpError4.Text = "Please enter a password.";
                                }
                            }
                        }
                        else
                        {
                            lblSignUpError3.Text = "Please enter your username.";
                        }
                    }
                    else
                    {
                        lblSignUpError2.Text = "Please enter your last name.";
                    }
                }
                else
                {
                    lblSignUpError1.Text = "Please enter your first name.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnS_PassShow_Click(object sender, EventArgs e)
        {
            if (txtPassword.PasswordChar == '●')
            {
                txtPassword.PasswordChar = '\0';                          // Show the password
                btnS_PassShow.Image = Properties.Resources.PassShow;                                                          // Change icon btnShowPassword 
            }
            else
            {
                txtPassword.PasswordChar = '●';                           // Hide the password
                btnS_PassShow.Image = Properties.Resources.PassHide;                                                           // Change icon btnShowPassword
            }
        }

        private void btnClose2_Click(object sender, EventArgs e)
        {
            Exit_Application();
        }

        private void btnS_Next_Click(object sender, EventArgs e)
        {
            string FillNIC = txtS_NIC.Text;

            DateTime BirthDate = DateTime.Parse(dtpS_DOB.Text);
            DateTime CurrentDate = DateTime.Today;
            int Age = CurrentDate.Year - BirthDate.Year;
            string str = Age.ToString();

            string FillMobileNumber = txtS_Mobile.Text;

            // Check if the NIC is 10 or 12 digits long
            if (FillNIC.Length == 12 || FillNIC.Length == 10)
            {
                if (Age > 18)                     // Check age 18+
                {
                    if (FillMobileNumber.Length == 10)
                    {
                        ClearErrorLabels();

                        pnlLogIn.Visible = true;
                        pnlForgetPassword.Visible = true;
                        pnlSignUp.Visible = true;
                        pnlSignUp1.Visible = true;
                        pnlSignUp2.Visible = true;
                        pnlSignUp3.Visible = false;
                        pnlSignUp4.Visible = false;
                        pnlSignUpSuccess.Visible = false;
                        pnlSignUpError.Visible = false;
                    }
                    else
                    {
                        // Phone number is invalid, show error message
                        lblSignUpError7.Text = "Invalid Phone Number! must be 10 digits.";
                    }
                }
                else
                {
                    lblSignUpError6.Text = $"Your Age: {Age}  - Your age must be at least 18.";
                }
            }
            else
            {
                // NIC is invalid, show error message
                lblSignUpError5.Text = "Please enter valid nic number! \nThe nic must be 10 or 12 digits.";
            }
        }

        private void btnS_Back_Click(object sender, EventArgs e)
        {
            ClearErrorLabels();

            pnlLogIn.Visible = true;
            pnlForgetPassword.Visible = true;
            pnlSignUp.Visible = true;
            pnlSignUp1.Visible = false;
            pnlSignUp2.Visible = false;
            pnlSignUp3.Visible = false;
            pnlSignUp4.Visible = false;
            pnlSignUpSuccess.Visible = false;
            pnlSignUpError.Visible = false;
        }

        private void btnClose3_Click(object sender, EventArgs e)
        {
            Exit_Application();
        }

        int OtpCode = 1000;
        private void btnS_SendOTP_Click(object sender, EventArgs e)
        {
            ClearErrorLabels();

            if (txtS_Email.Text == "")
            {
                lblSignUpError8.Text = "Please enter your email after click SEND OTP!";
            }
            else
            {
                string UserEnterdFirstName = txtS_FirstName.Text;
                string UserEnterdLastName = txtS_LastName.Text;

                OTPCode.Stop();
                string to, from, pass, mail;
                to = txtS_Email.Text;
                from = "nexgenbank@gmail.com";
                mail = OtpCode.ToString();
                pass = "pvyp dgod qfiq aths";
                MailMessage message = new MailMessage();
                message.To.Add(to);
                message.From = new MailAddress(from);
                message.IsBodyHtml = true;         //This will enable using HTML elements in email body
                message.Body = "<p>Hi, " + UserEnterdFirstName + " " + UserEnterdLastName + "!,</p>" +
                               "<p>Here is a temporary verification code for NEXGEN Bank Account. It can only be used once within the next 1 minutes, after which it will expire:" + "</p>" +
                               "<p>Your verification code is: <b>" + OtpCode + "</b></p>" +
                               "</br>" +
                               "<p>Thank you for using NEXGEN Bank.<br>If you have any issues, please contact support.</p>" +
                               "</br>" +
                               "<p>Sincerely,</p>" +
                               "<p>NEXGEN Bank</p>";
                message.Subject = "NEXGEN Bank - Email Verification Code";
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.EnableSsl = true;
                smtp.Port = 587;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential(from, pass);

                try
                {
                    smtp.Send(message);
                    ClearErrorLabels();
                    MessageBox.Show("Verification code sent successful! Check your mailbox.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtEmailOTP.Enabled = true;
                    btnVerifyOTP.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                ClearErrorLabels();

                // Show the 4th signup page and hide the other pages
                pnlLogIn.Visible = true;
                pnlForgetPassword.Visible = true;
                pnlSignUp.Visible = true;
                pnlSignUp1.Visible = true;
                pnlSignUp2.Visible = true;
                pnlSignUp3.Visible = true;
                pnlSignUp4.Visible = false;
                pnlSignUpSuccess.Visible = false;
                pnlSignUpError.Visible = false;
            }

            string UserEmail = txtS_Email.Text;

            lblEmail.Text = "We will send you an One Time Passcode via this\n " + UserEmail + " \nemail address.";
        }

        private void btnS_Skip_Click(object sender, EventArgs e)
        {
            ClearErrorLabels();

            txtShowFirstName.Text = txtS_FirstName.Text;
            txtShowLastName.Text = txtS_LastName.Text;
            txtShowUserName.Text = txtS_UserName.Text;
            txtShowPassword.Text = txtPassword.Text;
            txtShowNIC.Text = txtS_NIC.Text;
            txtShowDOB.Text = dtpS_DOB.Text;
            txtShowMobile.Text = txtS_Mobile.Text;

            txtShowEmail.Visible = false;

            pnlLogIn.Visible = true;
            pnlForgetPassword.Visible = true;
            pnlSignUp.Visible = true;
            pnlSignUp1.Visible = true;
            pnlSignUp2.Visible = true;
            pnlSignUp3.Visible = true;
            pnlSignUp4.Visible = true;
            pnlSignUpSuccess.Visible = false;
            pnlSignUpError.Visible = false;
        }

        private void btnS_Back2_Click(object sender, EventArgs e)
        {
            ClearErrorLabels();

            pnlLogIn.Visible = true;
            pnlForgetPassword.Visible = true;
            pnlSignUp.Visible = true;
            pnlSignUp1.Visible = true;
            pnlSignUp2.Visible = false;
            pnlSignUp3.Visible = false;
            pnlSignUp4.Visible = false;
            pnlSignUpSuccess.Visible = false;
            pnlSignUpError.Visible = false;
        }

        private void btnClose4_Click(object sender, EventArgs e)
        {
            Exit_Application();
        }

        private void OTPCode_Tick(object sender, EventArgs e)
        {
            OtpCode += 10;
            if (OtpCode == 9999)
            {
                OtpCode = 1000;
            }
        }

        private void btnVerifyOTP_Click(object sender, EventArgs e)
        {
            if (txtEmailOTP.Text == OtpCode.ToString())
            {
                ClearErrorLabels();

                txtShowFirstName.Text = txtS_FirstName.Text;
                txtShowLastName.Text = txtS_LastName.Text;
                txtShowUserName.Text = txtS_UserName.Text;
                txtShowPassword.Text = txtPassword.Text;
                txtShowNIC.Text = txtS_NIC.Text;
                txtShowDOB.Text = dtpS_DOB.Text;
                txtShowMobile.Text = txtS_Mobile.Text;
                txtShowEmail.Text = txtS_Email.Text;

                pnlLogIn.Visible = true;
                pnlForgetPassword.Visible = true;
                pnlSignUp.Visible = true;
                pnlSignUp1.Visible = true;
                pnlSignUp2.Visible = true;
                pnlSignUp3.Visible = true;
                pnlSignUp4.Visible = true;
                pnlSignUpSuccess.Visible = false;
                pnlSignUpError.Visible = false;
            }
            else
            {
                lblSignUpError9.Text = "Please enter correct OTP code!";
            }
        }

        private void btnS_Back3_Click(object sender, EventArgs e)
        {
            pnlLogIn.Visible = true;
            pnlForgetPassword.Visible = true;
            pnlSignUp.Visible = true;
            pnlSignUp1.Visible = true;
            pnlSignUp2.Visible = true;
            pnlSignUp3.Visible = false;
            pnlSignUp4.Visible = false;
            pnlSignUpSuccess.Visible = false;
            pnlSignUpError.Visible = false;
        }

        private void btnClose5_Click(object sender, EventArgs e)
        {
            Exit_Application();
        }

        private string GenerateCustomerID()
        {
            // Example: CUST202311240001
            string prefix = "CUST"; // Customize the prefix as needed
            int year = DateTime.Now.Year;
            string uniquePart = DateTime.Now.ToString("MMddHHmmss"); // Month, Day, Hour, Minute, Second
            return $"{prefix}{year}{uniquePart}";
        }

        private void btnCreateAccount1_Click(object sender, EventArgs e)
        {
            try
            {
                string FirstName = txtShowFirstName.Text;
                string LastName = txtShowLastName.Text;
                string UserName = txtShowUserName.Text;
                string Password = txtShowPassword.Text;
                string NIC = txtS_NIC.Text;

                string GenderForNIC = txtS_NIC.Text;
                if (GenderForNIC.Length == 12)
                {
                    // We need digits from positions 5 to 8, which correspond to indices 4 to 7 (0-based index)
                    int startIndex = 4; // Starting index (0-based)
                    int length = 3; // Length of the substring (4 digits)

                    // Extract the substring (digits from position 5-8)
                    string selectedDigits = txtShowNIC.Text.Substring(startIndex, length);

                    // Display the selected digits in Label2
                    txtShowNIC.Text = selectedDigits;

                    string GenderC = txtShowNIC.Text;
                    int GenderCode = int.Parse(GenderC);

                    if (GenderCode > 500)
                    {
                        string Gender;
                        Gender = "Female";
                        txtShowNIC.Text = null;
                        txtShowNIC.Text = Gender;
                    }
                    else
                    {
                        if (GenderCode <= 500)
                        {
                            string Gender;
                            Gender = "Male";
                            txtShowNIC.Text = null;
                            txtShowNIC.Text = Gender;
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
                        string selectedDigits = txtShowNIC.Text.Substring(startIndex, length);

                        // Display the selected digits in Label2
                        txtShowNIC.Text = selectedDigits;

                        string GenderC = txtShowNIC.Text;
                        int GenderCode = int.Parse(GenderC);

                        if (GenderCode <= 500)
                        {
                            string Gender;
                            Gender = "Male";
                            txtShowNIC.Text = null;
                            txtShowNIC.Text = Gender;
                        }
                        else
                        {
                            if (GenderCode > 500)
                            {
                                string Gender;
                                Gender = "Female";
                                txtShowNIC.Text = null;
                                txtShowNIC.Text = Gender;
                            }
                        }
                    }
                }

                string GetGender = txtShowNIC.Text;
                string DOB = txtShowDOB.Text;
                string PhoneNumber = txtShowMobile.Text;
                string Email = txtShowEmail.Text;
                string CustomerID = GenerateCustomerID();

                string Connection = @"Data Source=Mavinda-PC\SQLEXPRESS;Initial Catalog=""NEXGEN Bank Management System"";Integrated Security=True;";
                string Query = "insert into Customers values ('" + FirstName + "', '" + LastName + "', '" + DOB + "', '" + NIC + "', '" + GetGender + "', '" + PhoneNumber + "', '" + Email + "', '" + null + "', '" + null + "', '" + UserName + "', '" + Password + "', '"+ CustomerID +"');";

                SqlConnection conn = new SqlConnection(Connection);
                SqlCommand cmd = new SqlCommand(Query, conn);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                pnlLogIn.Visible = true;
                pnlForgetPassword.Visible = true;
                pnlSignUp.Visible = true;
                pnlSignUp1.Visible = true;
                pnlSignUp2.Visible = true;
                pnlSignUp3.Visible = true;
                pnlSignUp4.Visible = true;
                pnlSignUpSuccess.Visible = true;
                pnlSignUpError.Visible = false;

                //Method  - Function Call
                ClearTextBoxes();
                ClearErrorLabels();
            }
            catch
            {
                pnlLogIn.Visible = true;
                pnlForgetPassword.Visible = true;
                pnlSignUp.Visible = true;
                pnlSignUp1.Visible = true;
                pnlSignUp2.Visible = true;
                pnlSignUp3.Visible = true;
                pnlSignUp4.Visible = true;
                pnlSignUpSuccess.Visible = true;
                pnlSignUpError.Visible = true;

                //Method  - Function Call
                ClearTextBoxes();
                ClearErrorLabels();
            }
        }

        private void btnS_Back4_Click(object sender, EventArgs e)
        {
            pnlLogIn.Visible = true;
            pnlForgetPassword.Visible = true;
            pnlSignUp.Visible = true;
            pnlSignUp1.Visible = true;
            pnlSignUp2.Visible = true;
            pnlSignUp3.Visible = false;
            pnlSignUp4.Visible = false;
            pnlSignUpSuccess.Visible = false;
            pnlSignUpError.Visible = false;
        }

        private void btnClose6_Click(object sender, EventArgs e)
        {
            Exit_Application();
        }

        private void btnS_PassShow1_Click(object sender, EventArgs e)
        {
            if (txtShowPassword.PasswordChar == '●')
            {
                txtShowPassword.PasswordChar = '\0';                          // Show the password
                btnS_PassShow1.Image = Properties.Resources.PassShow;                                                              // Change icon btnShowPassword 
            }
            else
            {
                txtShowPassword.PasswordChar = '●';                           // Hide the password
                btnS_PassShow1.Image = Properties.Resources.PassHide;                                                                // Change icon btnShowPassword
            }
        }

        private void btnS_Continue_Click(object sender, EventArgs e)
        {
            frmLogIn_SignUp_Load(sender, e);
        }

        private void btnTryAgain_Click(object sender, EventArgs e)
        {
            btnL_SignUP_Click(sender, e);
        }

        private void txtL_Password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogIn_Click(sender, e);
            }
        }
    }
}
