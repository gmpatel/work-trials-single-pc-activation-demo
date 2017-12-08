using System.Windows;
using SinglePcActivationDemo.App.Common;
using SinglePcActivationDemo.Common.OperationMessages;

namespace SinglePcActivationDemo.App
{
    public partial class Activation : Window
    {
        public DialogResults Result;

        public Activation()
        {
            InitializeComponent();
        }

        // Cancel Button Click of the Activation window
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Result = DialogResults.Cancel;
            this.Close();
        }

        // Activate Button Click of the Activation window
        private void ButtonActivate_Click(object sender, RoutedEventArgs e)
        {
            // Calling Activate api helper method of AppServices class
            var response = new AppServices().Activate(new ActivateRequest
            {
                UserEmail = Global.LoginResponse.UserEmail,
                MachineKey = Global.MachineKey
            });

            // Storing Activate call response to Global variable
            Global.ActivateResponse = response;

            // Checking whether the Activation was successful
            if (!response.Successful)
            {
                // If Activation failed; displaying message returned from servers and stays at the Activation screen
                MessageBox.Show(response.Message, "Activation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // If Activation was successful continuing with the application flow
            this.Result = DialogResults.Successful;
            this.Close();
        }
    }
}