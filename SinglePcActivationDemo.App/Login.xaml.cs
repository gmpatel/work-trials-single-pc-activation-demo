using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SinglePcActivationDemo.App.Common;
using SinglePcActivationDemo.Common;
using SinglePcActivationDemo.Common.OperationMessages;

namespace SinglePcActivationDemo.App
{
    public partial class Login : Window
    {
        public DialogResults Result;
        public Login()
        {
            InitializeComponent();
        }

        // Cancel Button Click of the Login window
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Result = DialogResults.Cancel;
            this.Close();
        }

        // Login Button Click of the Login window
        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            // Calling Login api helper method of AppServices class
            var response = new AppServices().Login(new LoginRequest
            {
                UserEmail = TextUserEmail.Text,
                UserPasswordEncrypted = Cryptography.HashMD5(TextUserPassword.Password) 
                // Please Read: Encrypting Password to MD5Hash and passing to api call as I prefer to store password as MD5Hash into the database. Which is one kind of security for the user to keep their password secret. And always match MD5Hash string with stored password when user enters the password. So their original password is not stored or passed anywhere, not even passed to WebApi call.
            });

            // Storing Login call response to Global variable
            Global.LoginResponse = response;

            // Checking whether the Login was successful
            if (!response.Successful)
            {
                // If Login was failed; displaying message returned from servers and stays at the Login screen
                MessageBox.Show(response.Message, "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                TextUserEmail.Text = string.Empty;
                TextUserPassword.Password = string.Empty;
                TextUserEmail.Focus();
                return;
            }

            // If Login was successful continuing with the application flow
            this.Result = DialogResults.Successful;
            this.Close();
        }
    }
}