using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SinglePcActivationDemo.Common.DataObjects;
using SinglePcActivationDemo.Common.OperationMessages;

namespace SinglePcActivationDemo.Web.Controllers
{
    public class AppController : ApiController
    {
        [HttpPost]
        [Route("api/app/login")]
        public LoginResponse Login(LoginRequest request)
        {
            // Calling DataAccess layer method for Login
            var data = new DataAccess.AppData().Login(new LoginInput
            {
                UserEmail = request.UserEmail,
                UserPasswordEncrypted = request.UserPasswordEncrypted
            });

            // Populating LoginResponse object from the Data returned from DataAccess method call
            return new LoginResponse
            {
                UserId = data.UserId,
                UserUniqueId = data.UserUniqueId,
                UserEmail = data.UserEmail,
                Successful = data.Successful,
                Message = data.Message,
                Activated = data.Activated,
                ActivatedMachineKey = data.ActivatedMachineKey,
                ActivateDateTime = data.ActivateDateTime,
                IsEmailVerified = data.IsEmailVerified
            };
        }

        [HttpPost]
        [Route("api/app/activate")]
        public ActivateResponse Activate(ActivateRequest request)
        {
            // Calling DataAccess layer method for Activate
            var data = new DataAccess.AppData().Activate(new ActivateInput
            {
                UserEmail = request.UserEmail,
                MachineKey = request.MachineKey
            });

            // Populating ActivateResponse object from the Data returned from DataAccess method call
            return new ActivateResponse
            {
                UserId = data.UserId,
                UserUniqueId = data.UserUniqueId,
                UserEmail = data.UserEmail,
                Successful = data.Successful,
                Message = data.Message,
                Activated = data.Activated,
                ActivatedMachineKey = data.ActivatedMachineKey,
                ActivateDateTime = data.ActivateDateTime
            };
        }
    }
}