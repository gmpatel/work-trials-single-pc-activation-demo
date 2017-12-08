using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SinglePcActivationDemo.Common.OperationMessages;

namespace SinglePcActivationDemo.App.Common
{
    // Global storage acccessible by whole app
    public static class Global
    {
        public static string MachineKey { get; set; }
        public static LoginResponse LoginResponse { get; set; }
        public static ActivateResponse ActivateResponse { get; set; }
    }
}