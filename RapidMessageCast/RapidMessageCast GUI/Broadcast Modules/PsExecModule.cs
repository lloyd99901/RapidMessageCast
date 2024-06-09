using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text;

//--RapidMessageCast Software--
//PsExecModule.cs - RapidMessageCast Manager

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

namespace RapidMessageCast_Manager.BroadcastModules
{
    internal class PSExecModule
    {
        public static bool IsPSExecPresent()
        {
            //Check if PSexec is present and has get the Product Name Sysinternals PsExec
            if (File.Exists("PsExec.exe"))
            {
                FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo("PsExec.exe");
                if (myFileVersionInfo.ProductName == "Sysinternals PsExec")
                {
                    //Check cert
                    X509Certificate2 cert = new("PsExec.exe");
                    if (cert.Subject.Contains("Microsoft"))
                    {
                        return true; //Offically signed by Microsoft
                    }
                    else
                    {
                        MessageBox.Show("Warning! RMC has detected that PsExec has not been signed by Microsoft. This could be a fake PsExec program. Please download PsExec from Sysinternals to ensure you have the correct version.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Warning! RMC has detected that PsExec's product name is not 'Sysinternals PsExec'. This could be a fake PsExec program. Please download PsExec from Sysinternals to ensure you have the correct version.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            else
            {
                return false; //PsExec not present.
            }
        }
        public static string WritePsExecCommand(string remoteComputerTarget,
            CheckBox checkboxNonInteractive, CheckBox checkboxCopyExecutable, CheckBox checkboxDontWait,
            CheckBox checkboxNoProfile, CheckBox checkboxForceCopy, CheckBox checkboxInteractive,
            CheckBox checkboxElevatedToken, CheckBox checkboxLimitedUser, CheckBox checkboxTimeout,
            TextBox textBoxTimeout, CheckBox checkboxPassword, TextBox textBoxPassword,
            CheckBox checkboxRemoteService, TextBox textBoxRemoteService, CheckBox checkboxSystemAccount,
            CheckBox checkboxUserName, TextBox textBoxUserName, CheckBox checkboxVersionCopy,
            CheckBox checkboxWorkingDirectory, TextBox textBoxWorkingDirectory, CheckBox checkboxSecureDesktop,
            CheckBox checkboxPriority, TextBox textBoxPriority, CheckBox checkboxAcceptEula,
            CheckBox checkboxNoBanner)
        {
            var options = new Dictionary<CheckBox, Func<string>> //Dictionary of options with the checkbox as the key and a lambda function as the value
            {
                { checkboxNonInteractive, () => "-d" },
                { checkboxCopyExecutable, () => "-c" },
                { checkboxDontWait, () => "-d" },
                { checkboxNoProfile, () => "-e" },
                { checkboxForceCopy, () => "-f" },
                { checkboxInteractive, () => "-i" },
                { checkboxElevatedToken, () => "-h" },
                { checkboxLimitedUser, () => "-l" },
                { checkboxTimeout, () => !string.IsNullOrEmpty(textBoxTimeout.Text) ? $"-n {textBoxTimeout.Text}" : string.Empty },
                { checkboxPassword, () => !string.IsNullOrEmpty(textBoxPassword.Text) ? $"-p {textBoxPassword.Text}" : string.Empty },
                { checkboxRemoteService, () => !string.IsNullOrEmpty(textBoxRemoteService.Text) ? $"-r {textBoxRemoteService.Text}" : string.Empty },
                { checkboxSystemAccount, () => "-s" },
                { checkboxUserName, () => !string.IsNullOrEmpty(textBoxUserName.Text) ? $"-u {textBoxUserName.Text}" : string.Empty },
                { checkboxVersionCopy, () => "-v" },
                { checkboxWorkingDirectory, () => !string.IsNullOrEmpty(textBoxWorkingDirectory.Text) ? $"-w {textBoxWorkingDirectory.Text}" : string.Empty },
                { checkboxSecureDesktop, () => "-x" },
                { checkboxPriority, () => !string.IsNullOrEmpty(textBoxPriority.Text) ? $"-priority {textBoxPriority.Text}" : string.Empty },
                { checkboxAcceptEula, () => "-accepteula" },
                { checkboxNoBanner, () => "-nobanner" }
            };

            var command = new StringBuilder($"Psexec.exe {remoteComputerTarget}");
            
            foreach (var option in options) //Iterate through the options dictionary
            {
                if (option.Key.Checked)
                {
                    //If the option checkbox is checked, add the option to the command, and if the option has a value, add the value to the command
                    string optionValue = option.Value();
                    if (!string.IsNullOrEmpty(optionValue))
                    {
                        command.Append($" {optionValue}");
                    }
                }
            }
            return command.ToString();
        }


        public static void CreatePsExecInstance(string command)
        {
            ProcessStartInfo startInfo = new() //This could be changed to psexec.exe directly, but this is just a test to see if the command works.
            {
                FileName = "cmd.exe",
                Arguments = $"/c {command}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            Process process = new()
            {
                StartInfo = startInfo
            };
            process.Start();
        }

        public static void BroadcastMessage(string message, string remoteComputer, string target, bool onlyRunOnePsExecInstance)
        {
            //Notes for function:
            //If onlyRunOneInstance is true, the class will check if the computerlist is just one computer, if it is, just run the command on that computer.
            //If it is more than one computer, the command will be run on all computers in the list via just one command line.
            //e.g. Psexec @list.txt -d -c -nobanner -accepteula -i -h -u username -p password -w C:\Windows\System32\ cmd /c echo Hello World
            //However, if onlyRunOneInstance is false, the command will be run on each computer in the list via separate command lines.
            //Similar to how the PCBroadcast code works.
            //e.g. for each computer, run psexec \\computername -d -c -nobanner -accepteula -i -h -u username -p password -w C:\Windows\System32\ cmd /c echo Hello World
            //This is just an idea, so this may not be the final implementations, but it will be added here to test the idea.
            //For now, the WritePsExecCommand will be used for one remote computer at a time, unless the broadcastmessage sets the remoteComputerTarget to "@list.txt"
            //Implement async task for the broadcast message, so the GUI doesn't freeze up.
            //Also, this function or functions that call this function should check via isPsexecPresent() if psexec is not only present, but also made by Sysinternals.
            //This will prevent issues with other programs that may have the same name as psexec. Or even a fake psexec program that could be used for malicious purposes.
        }
    }
}
