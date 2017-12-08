using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinglePcActivationDemo.Common.OperationMessages
{
    public class LoginResponse
    {
        public long UserId { get; set; }
        public Guid? UserUniqueId { get; set; }
        public string UserEmail { get; set; }
        public bool Successful { get; set; }
        public string Message { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool Activated { get; set; }
        public string ActivatedMachineKey { get; set; }
        public DateTime? ActivateDateTime { get; set; }
    }
}