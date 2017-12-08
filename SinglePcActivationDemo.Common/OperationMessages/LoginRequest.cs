using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinglePcActivationDemo.Common.OperationMessages
{
    public class LoginRequest
    {
        public string UserEmail { get; set; }
        public string UserPasswordEncrypted { get; set; }
    }
}