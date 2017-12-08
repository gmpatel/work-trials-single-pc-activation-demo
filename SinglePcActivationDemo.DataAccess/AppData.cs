using System;
using System.Data.Entity.Migrations;
using System.Linq;
using SinglePcActivationDemo.Common.DataObjects;

namespace SinglePcActivationDemo.DataAccess
{
    public class AppData : IDisposable
    {
        private SinglePcActivationDemoDbEntities DataContext { get; set; }

        public AppData()
        {
            DataContext = new SinglePcActivationDemoDbEntities();
        }

        public void Dispose()
        {
            DataContext.Dispose();
        }

        // DataAccess\Business method for Login into app
        public LoginOutput Login(LoginInput input)
        {
            // Checking that the user exist with te email id entered to login?
            var user = DataContext.Users.FirstOrDefault(
                    x => x.UserEmail.Equals(input.UserEmail, StringComparison.InvariantCultureIgnoreCase)
                );

            // If user with signin Email Address exist in the database
            if (user != null)
            {
                // We match the MD5Hash encrypted password to the one stored in database to check if they are same or not
                if (!user.UserPassword.Equals(input.UserPasswordEncrypted, StringComparison.InvariantCultureIgnoreCase))
                {
                    // If MD5Hash encrypted passwords does not match; we return false response like below with appropriate message (will be confirmed by Rajiv)
                    return new LoginOutput
                    {                         
                        UserId = user.Id,
                        UserUniqueId = user.UniqueId,
                        UserEmail = user.UserEmail,
                        Successful = false,
                        Message = "The password you have entered is invalid."
                    };
                }

                // If MD5Hash encrypted passwords are matching; we check wheter the login Email Address is verified or not
                if (!user.IsEmailVerified)
                {
                    // If login Email Address is not verified; we return false response like below with appropriate message (will be confirmed by Rajiv)
                    return new LoginOutput
                    {
                        UserId = user.Id,
                        UserUniqueId = user.UniqueId,
                        UserEmail = user.UserEmail,
                        Successful = false,
                        Message = "Your register email address is not verified yet. Please check your email to verify the email address.",
                        IsEmailVerified = user.IsEmailVerified
                    };
                }

                // If login Email Address is exist, MD5Hash encrypted passwords are matching and login Email Address is already verified; we will send the true response with appropriate message (will be confirmed by Rajiv)
                return new LoginOutput
                {
                    UserId = user.Id,
                    UserUniqueId = user.UniqueId,
                    UserEmail = user.UserEmail,
                    Successful = true,
                    Message = "The login is successful.",
                    IsEmailVerified = user.IsEmailVerified,
                    Activated = user.Activated,
                    ActivatedMachineKey = user.ActivatedMachineKey,
                    ActivateDateTime = user.ActivatedDateTime
                };
            }

            // If user with the signin Email Address is not exists in the database we return false response like below with appropriate message (will be confirmed by Rajiv)
            return new LoginOutput
            {
                UserId = -1,
                UserUniqueId = null,
                UserEmail = input.UserEmail,
                Successful = false,
                Message = "The email address you are using to login is not registered with us.",
            };
        }

        // DataAccess\Business method for Activate the app
        public ActivateOutput Activate(ActivateInput input)
        {
            // Finding the users if any registered and activated with the same MachineKey of current Machine
            var machineUser = DataContext.Users.FirstOrDefault(
               x => x.ActivatedMachineKey.Equals(input.MachineKey, StringComparison.InvariantCultureIgnoreCase)
            );

            // Finding the user registerd with signin Email Address to populate the Activation Info against the record
            var user = DataContext.Users.FirstOrDefault(
               x => x.UserEmail.Equals(input.UserEmail, StringComparison.InvariantCultureIgnoreCase)
            );

            // Checking that we found the user registerd to activate the app for on the current machine
            if (user != null)
            {
                // Making sure that there is no other user has activated app on the current machine
                // This will prevent users to keep registering new user and keep using app free for a week
                // One user can regiter one Email Address from one machine and they can use app only for a week for free
                // This step is very important
                if (machineUser != null)
                {
                    // If someone has registered and activated the app from current machine we will return false response with appropriate message (Rajiv will confirm the message)
                    return new ActivateOutput
                    {
                        UserId = user.Id,
                        UserUniqueId = user.UniqueId,
                        UserEmail = user.UserEmail,
                        Successful = false,
                        Message = "Unable to activate application on this computer using this email address. There is already an application activated on this machine with different user. Please contact our support team to resolve the issue."
                    };
                }

                // If user is genuine and activating app for the first time from that machine then will will continue updating the activation recourd
                if (!user.Activated && string.IsNullOrEmpty(user.ActivatedMachineKey) && user.ActivatedDateTime == null)
                {
                    // Updating app Activation information for the user
                    user.Activated = true;
                    user.ActivatedMachineKey = input.MachineKey;
                    user.ActivatedDateTime = DateTime.Now;
                    DataContext.Users.AddOrUpdate(user);
                    DataContext.SaveChanges();

                    // Returning true response for the activation
                    return new ActivateOutput
                    {
                        UserId = user.Id,
                        UserUniqueId = user.UniqueId,
                        UserEmail = user.UserEmail,
                        Successful = true,
                        Message = "The application activated successfully on this computer using this email address.",
                        Activated = user.Activated,
                        ActivatedMachineKey = user.ActivatedMachineKey,
                        ActivateDateTime = user.ActivatedDateTime
                    };
                }

                return new ActivateOutput
                {
                    UserId = user.Id,
                    UserUniqueId = user.UniqueId,
                    UserEmail = user.UserEmail,
                    Successful = false,
                    Message = "Unable to activate application on this computer using this email address. The email address is already activated on another computer. Please contact our support team to resolve the issue."
                };
            }

            // If we couldn't find the signin Email Address (which will never be case but good to check) then returning false response with appropriate message (Rajiv will confirm the message text)
            return new ActivateOutput
            {
                UserId = -1,
                UserUniqueId = null,
                UserEmail = input.UserEmail,
                Successful = false,
                Message = "The email address you are using to login is not registered with us. Unable to activate application on this computer using this email address.",
            };
        }
    }
}                                                                                                                                                                                                         