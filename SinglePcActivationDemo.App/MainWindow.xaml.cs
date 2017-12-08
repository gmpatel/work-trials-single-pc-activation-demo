using System.Windows;
using SinglePcActivationDemo.App.Common;
using SinglePcActivationDemo.Common;

namespace SinglePcActivationDemo.App
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            // Generating and storing MachineKey as a Global variable to access anywhere from the app
            Global.MachineKey = new MachineInfo().GetMachineKey();

            // Opening Login dialog box
            var login = new Login();
            login.ShowDialog();

            // Checking whether the login failed - will terminate the app from that point
            if (login.Result == DialogResults.Cancel)
            {
                this.Close();
                Application.Current.Shutdown();

                return;
            }

            // Checking that the login was successful and the app is yet not activated for the login user - will display app activation screen
            // Please check all three fields of the activation information as in the condition below
            // If the activation info on the server is available for the user trying to login the app will skip to display activation window
            if (login.Result == DialogResults.Successful && 
                !Global.LoginResponse.Activated && 
                string.IsNullOrEmpty(Global.LoginResponse.ActivatedMachineKey) &&
                Global.LoginResponse.ActivateDateTime == null)
            {
                // Opening Activation dialog box
                var activation = new Activation();
                activation.ShowDialog();

                // Checking whether the activation failed - will terminate the app from that point
                if (activation.Result == DialogResults.Cancel)
                {
                    this.Close();
                    Application.Current.Shutdown();
                    return;
                }

                return;
            }

            // Checking that the login was successful and the app activation information for the login user is available
            // Please check all three fields of the activation information as in the condition below
            if (login.Result == DialogResults.Successful && 
                Global.LoginResponse.Activated &&
                !string.IsNullOrEmpty(Global.LoginResponse.ActivatedMachineKey) &&
                Global.LoginResponse.ActivateDateTime != null)
            {
                // Checking that the MachineKey of this machine generated at the time of app start is same as the MachineKey stored in the database for this user or not
                // If generated MachineKey and stored MachineKey are not same then user is trying to run the app from anohter machine with same user; we are not allowing it; user must run the app from the machine they activated on
                if (!Global.LoginResponse.ActivatedMachineKey.Equals(Global.MachineKey))
                {
                    
                    // Display appropriate message box (Rajiv will finalize the text) but put something like this as of now
                    MessageBox.Show(
                        "This user has activated the app on the different computer. You can not use the application with same user on a different computer. If you are intended to use application on another computer then where you activated it or you have upgraded computer then please contact our support team to resolve the issue",
                        "Invalid Computer", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                    // Terminate the app from this point
                    this.Close();
                    Application.Current.Shutdown();

                    return;
                }
            }

            // If code execution reaches at this point that means, the user login is successful, email address is verified and app is running from the correct machine
            // So continue executing the main window of the application to place bets

            // Please make sure to check trial and subscription logic to check before we display the main window 
            // I have not included the logic of the trial period check and the subscription check in this demo
            // This demo only includes the logic for App Activation to restrict the user to activate only one app from one machine
            // Code is written not very great. Haven't use any IoC, DI concepts so that you can easily understnad the flow of the logic
            // Feel free to use code as your way as far as you are checking all the points mentioned in this demo
            // The 'SinglePcActivationDemo.DataAccess' project will create a database for you which this demo is using
            // Demo is fully working end-to-end
            // Haven't used any Async logic to keep the flow simple and understandable, you please do Api calls as Async to keep the Ui responsive
        }
    }
}