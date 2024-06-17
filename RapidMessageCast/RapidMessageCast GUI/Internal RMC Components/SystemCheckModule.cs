using RapidMessageCast_Manager.BroadcastModules;
using System.ServiceProcess;

//--RapidMessageCast Software--
//SystemCheckModule.cs - RapidMessageCast Manager

//Copyright (c) 2024 Lunar/lloyd99901

//MIT License
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

namespace RapidMessageCast_Manager.Internal_RMC_Components
{
    public class SystemCheckModule(Action<string> logAction)
    {
        private readonly Action<string> _logAction = logAction;

        public void CheckFileExistence(string filePath, string logMessage, string userMessage)
        {
            if (!File.Exists(filePath))
            {
                _logAction(logMessage);
                MessageBox.Show(userMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void CheckAdministratorStatus()
        {
            if (!IsAdministrator())
            {
                _logAction("Notice - [CheckSystemState]: The program is not running as an administrator. If broadcasting a message doesn't work, try running this program as administrator.");
            }
        }

        public void CheckSystemMemory()
        {
            if (new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory < 1073741824)
            {
                string warningMessage = "The system has less than 1GB of RAM. Running the broadcast may freeze your computer or take longer to finish if your RAM continues to lower.";
                MessageBox.Show(warningMessage, "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _logAction("Warning - [CheckSystemState]: " + warningMessage);
            }
        }

        public void CheckNetworkAvailability()
        {
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                _logAction("Info - [CheckSystemState]: Network is available. The program is able to send messages.");
            }
            else
            {
                _logAction("Error - [CheckSystemState]: System network connectivity state is not available. Sending messages may not be possible.");
                MessageBox.Show("RMC has detected that your computer's network is not available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void CheckTCPPort(int port, string successLogMessage, string errorLogMessage, string errorMessage)
        {
            using System.Net.Sockets.TcpClient tcpClient = new();
            try
            {
                tcpClient.Connect("127.0.0.1", port);
                _logAction(successLogMessage);
            }
            catch (Exception)
            {
                _logAction(errorLogMessage);
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public bool CheckPSExecPresence()
        {
            if (!PSExecModule.IsPSExecPresent())
            {
                _logAction("Warning - [CheckSystemState]: PsExec.exe is not present or valid in the program directory. In order for the PsExec module to work, please ensure that PsExec is present and that the Product Name of the PsExec.exe program contains 'Sysinternals PsExec'.");
                return false;
            }
            return true;
        }

        public bool AreCriticalServicesRunning()
        {
            string[] criticalServices = { "Dnscache", "TermService", "RpcSs" };
            //Return message as well as log message and the return value
            if (SystemServiceManager.AreSpecifiedServicesRunning(criticalServices))
            {
                _logAction("Info - [CheckSystemState]: All critical services are running.");
                return true;
            }
            _logAction("Error - [CheckSystemState]: One or more critical services are not running. Please ensure that all critical services are running before broadcasting a message. Please check: Dnscache, TermService, RpcSs.");
            return false;
        }

        private static bool IsAdministrator()
        {
            var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            var principal = new System.Security.Principal.WindowsPrincipal(identity);
            return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
        }
    }

}