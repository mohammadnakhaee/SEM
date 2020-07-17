using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace HelloWorld
{
    public partial class Form4 : Form
    {
        public FormMain myFormMain;
        bool isCreateAccount = false;
        bool isLoginCalled = false;
        bool isSuccessful = false;
        bool isGiveUp = false;
        bool SwitchToGuest = false;

        public Form4(bool isItCreateAccount, FormMain MyFormMain)
        {
            myFormMain = MyFormMain;
            isCreateAccount = isItCreateAccount;
            InitializeComponent();
            //comboBox1.SelectedIndex = 0;
            buttonFB.Focus();
            textBox2.Visible = isCreateAccount;
            label3.Visible = isCreateAccount;
            textBox3.Visible = isCreateAccount;
            comboBox1.Visible = !isCreateAccount;
            //button1.Visible = isCreateAccount;
            if (isCreateAccount)
                buttonFB.Text = "Create Account";
            else
            {
                if (!AccountExists("Guest")) myFormMain.CreateAccount("Guest", "12345678900987654321");
                buttonFB.Text = "Login";
                if (Directory.Exists(".\\Accounts"))
                {
                    string[] fileEntries = Directory.GetFiles(".\\Accounts");
                    foreach (string filePath in fileEntries)
                    {
                        string filename = Path.GetFileName(filePath);
                        string[] parts = filename.Split('.');
                        if (parts[0] != "Guest" && parts[0] != "guest") comboBox1.Items.Add(parts[0]);
                    }
                }
                comboBox1.SelectedIndex = 0;
            }

        }

        private void buttonFB_Click(object sender, EventArgs e)
        {
            Execute();
        }

        private void Execute()
        {
            if(isCreateAccount)
                ValidatingNewAccount();
            else
                Login();
        }

        private void ValidatingNewAccount()
        {
            bool isValid = false;
            string newUser = textBox3.Text;
            string pass1 = textBox1.Text;
            string pass2 = textBox2.Text;
            
            if (newUser == "") { MessageBox.Show("Please choose a name for your account ..."); return; }
            if (newUser.Contains('%') || newUser.Contains('/') || newUser.Contains('\\') ||
                newUser.Contains('.') || newUser.Contains('\r') || newUser.Contains(' ') ||
                newUser.Contains('`') || newUser.Contains('\'') || newUser.Contains('"') ||
                newUser.Contains('&') || newUser.Contains('#') || newUser.Contains('$') ||
                newUser.Contains('^') || newUser.Contains('*') || newUser.Contains('!') ||
                newUser.Contains('(') || newUser.Contains(')') || newUser.Contains(',')) { MessageBox.Show("The chosen name is not valid ..."); return; }
            if (AccountExists(newUser)) { MessageBox.Show("There is already an account with this username ..."); return; }
            if (newUser == "Guest" || newUser == "guest") { MessageBox.Show("Please choose another name for your account ..."); return; }
            if (pass1 == "") { MessageBox.Show("Please choose a password for your account ..."); return; }
            if (pass2 == "") { MessageBox.Show("Please confirm your password ..."); return; }
            if (pass1 != pass2) { MessageBox.Show("The entered passwords do not match ..."); return; }
            if (pass1.Length < 6) { MessageBox.Show("The chosen password should be at least 6 characters ..."); return; }

            string hash1 = SecurePasswordHasher.Hash(pass1);
            myFormMain.CreateAccount(newUser, hash1);
            this.Close();
        }

        private bool AccountExists(string UserName)
        {
            string filename = ".\\Accounts\\" + UserName + ".xml";
            return File.Exists(filename);
        }

        private void Login()
        {
            if (comboBox1.Text == "") return;
            isLoginCalled = true;
            
            if (comboBox1.Text == "Guest")
            {
                isSuccessful = true;
            }
            else
            {
                isSuccessful = CheckUserNamePass();
            }
            
            this.Close();
        }

        private bool CheckUserNamePass()
        {
            bool isSuccessful = false;
            string username = comboBox1.Text;
            string pass = textBox1.Text;

            string filename = ".\\Accounts\\" + username + ".xml";
            if (!AccountExists(username)) { MessageBox.Show("There is no account with this name ..."); return false; }
            XmlSerializer mySerializer = new XmlSerializer(typeof(List<UserSettings>));
            StreamReader xmlReader = new StreamReader(filename);
            List<UserSettings> TestAllUserSettings = new List<UserSettings>();
            TestAllUserSettings = (List<UserSettings>)mySerializer.Deserialize(xmlReader);
            string hash = TestAllUserSettings[0].PassHashCode;
            isSuccessful = SecurePasswordHasher.Verify(pass, hash);
            xmlReader.Close();
            if (!isSuccessful) MessageBox.Show("The password you entered is incorrect ...");

            return isSuccessful;
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Execute();
            }
        }

        private void comboBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Execute();
            }
        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((isLoginCalled && isSuccessful) || (isGiveUp && SwitchToGuest))
            {
                if (myFormMain.UserName != "")
                    myFormMain.Logout();
                else
                    myFormMain.TryToConnect();

                myFormMain.UserName = comboBox1.Text;
                myFormMain.UpdateUserName(false);
            }
            else
            {
                isLoginCalled = false;
                if (!isCreateAccount && !isGiveUp) e.Cancel = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            isGiveUp = true;
            if (myFormMain.UserName == "")
            {
                SwitchToGuest = true;
                comboBox1.SelectedIndex = 0;
                MessageBox.Show("Logging in as a Guest ...");
            }
            this.Close();
        }

        private void textBox3_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Execute();
            }
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Execute();
            }
        }
    }
}
