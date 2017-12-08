using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace SinglePcActivationDemo.Common
{
    // You probably can copy-paste the whole MachineInfo class into your project without any change
    
    public class MachineInfo
    {
        public string GetMachineKey()
        {
            var machineValues = new List<string>();

            var biosId = UniqueBiosId();
            if (!string.IsNullOrEmpty(biosId)) machineValues.Add(biosId);

            var diskId = UniqueDiskId();
            if (!string.IsNullOrEmpty(diskId)) machineValues.Add(diskId);

            var processorId = UniqueProcessorId();
            if (!string.IsNullOrEmpty(processorId)) machineValues.Add(processorId);

            var macAddress = UniqueMacAddress();
            if (!string.IsNullOrEmpty(macAddress)) machineValues.Add(macAddress);

            var windowsProductId = UniqueWindowsProductId();
            if (!string.IsNullOrEmpty(windowsProductId)) machineValues.Add(windowsProductId);

            return string.Join("|", machineValues);
        }

        private static string UniqueWindowsProductId()
        {
            try
            {
                using (var registryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", false))
                {
                    if (registryKey != null)
                    {
                        var productId = registryKey.GetValue("ProductId", string.Empty);
                        return productId.ToString().Trim().ToUpper();
                    }
                }

                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private static string UniqueBiosId()
        {
            try
            {
                const string query = "SELECT * FROM Win32_BIOS";

                var builder = new StringBuilder();
                var searcher = new ManagementObjectSearcher(query);

                foreach (var item in searcher.Get())
                {
                    var obj = item["SerialNumber"];
                    builder.Append(Convert.ToString(obj));
                    break;
                }

                return builder.ToString().Trim().ToUpper();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private static string UniqueDiskId()
        {
            try
            {
                const string query = "SELECT * FROM Win32_DiskDrive";

                var builder = new StringBuilder();
                var searcher = new ManagementObjectSearcher(query);

                foreach (var item in searcher.Get())
                {
                    var obj = item["SerialNumber"];
                    builder.Append(Convert.ToString(obj));
                    break;
                }

                return builder.ToString().Trim().ToUpper();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private static string UniqueProcessorId()
        {
            try
            {
                const string query = "SELECT * FROM Win32_Processor";

                var builder = new StringBuilder();
                var searcher = new ManagementObjectSearcher(query);

                foreach (var item in searcher.Get())
                {
                    var obj = item["processorID"];
                    builder.Append(Convert.ToString(obj));
                    break;
                }

                return builder.ToString().Trim().ToUpper();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private static string UniqueMacAddress()
        {
            try
            {
                var mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                var moc = mc.GetInstances();

                var macAddress = string.Empty;

                foreach (var mo in moc)
                {
                    if (macAddress == string.Empty) // only return MAC Address from first card
                    {
                        if ((bool) mo["IPEnabled"] == true) macAddress = mo["MacAddress"].ToString();
                    }

                    mo.Dispose();
                }

                return macAddress.Trim().ToUpper();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}