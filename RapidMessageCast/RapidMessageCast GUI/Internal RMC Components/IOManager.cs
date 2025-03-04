﻿using System.Diagnostics;
using System.Xml;

//--RapidMessageCast Software--
//IOManager.cs - RapidMessageCast Manager

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
    internal class IOManager(Action<string> logAction)
    {
        private readonly Action<string> _logAction = logAction;

        public static string[] LoadRMSGFile(string filePath)
        {
            //Here is the return array structure:
            // [0] = Error message (if any)
            // [1] = Message content
            // [2] = PC list
            // [3] = Expiry hour
            // [4] = Expiry minutes
            // [5] = Expiry seconds
            // [6] = Emergency mode enabled
            // [7] = Enable messaging of PCs
            // [8] = Enable email
            // [9] = Enable PSExec
            // [10] = Reattempt on error
            // [11] = Dont save history
            // [12] = WOL list
            // [13] = WOL port
            // [14] = FQDN Address
            // [15] = Email Port
            // [16] = Email From Address
            // [17] = Auth Mode
            // [18] = Email Account
            // [19] = Encrypted Email Password, this will be decrypted when the email module is enabled.
            // [20] = RMC Software Version

            if (string.IsNullOrEmpty(filePath))
            {
                return ["Error - File path is null or empty"];
            }

            try
            {
                XmlDocument doc = new();
                doc.Load(filePath);

                // Helper function to get element's inner text by tag name
                string GetElementValue(string tagName)
                {
                    var node = doc.GetElementsByTagName(tagName).Item(0);
                    return node?.InnerText ?? string.Empty;
                }

                // Helper function to check if a module is enabled
                bool IsEnabled(string tagName) => GetElementValue(tagName).Equals("Enabled", StringComparison.OrdinalIgnoreCase);

                // Extract values
                string RMCSoftwareVersion = GetElementValue("RMCSoftwareVersion");
                string message = GetElementValue("Message");
                string pcList = GetElementValue("PCList");
                string WOLlist = GetElementValue("WOLList");
                string WOLPort = GetElementValue("WOLPort");
                string expiryHour = GetElementValue("ExpiryHour");
                string expiryMinutes = GetElementValue("ExpiryMinutes");
                string expirySeconds = GetElementValue("ExpirySeconds");
                bool emergencyModeEnabled = IsEnabled("EmergencyModeEnabled");
                bool enableMessagingOfPCs = IsEnabled("EnableMessagingOfPCs");
                bool enableEmail = IsEnabled("EnableEmail");
                bool enablePSExec = IsEnabled("EnablePSExec");
                bool reattemptOnError = IsEnabled("ReattemptOnError");
                bool dontSaveHistory = IsEnabled("DontSaveHistory");
                string FQDNAddress = GetElementValue("FQDNAddress");
                string EmailPort = GetElementValue("EmailPort");
                string EmailFromAddress = GetElementValue("EmailFromAddress");
                string AuthMode = GetElementValue("AuthMode");
                string EmailAccount = GetElementValue("EmailAccount");
                string EmailPassword = GetElementValue("EmailPassword");

                // Return the extracted values via an array
                return
                [
                    "",
                    message,
                    pcList,
                    expiryHour,
                    expiryMinutes,
                    expirySeconds,
                    emergencyModeEnabled.ToString(),
                    enableMessagingOfPCs.ToString(),
                    enableEmail.ToString(),
                    enablePSExec.ToString(),
                    reattemptOnError.ToString(),
                    dontSaveHistory.ToString(),
                    WOLlist,
                    WOLPort,
                    FQDNAddress,
                    EmailPort,
                    EmailFromAddress,
                    AuthMode,
                    EmailAccount,
                    EmailPassword,
                    RMCSoftwareVersion
                ];
            }
            catch (Exception ex)
            {
                // Return an error message along with the exception
                return [$"Error - IO Manager Exception: {ex.Message}"];
            }
        }

        public static void SaveRMSGFile(string filePath, string messageContent, string pcList, string WOLlist, int WOLPort, string expiryHour, string expiryMinutes, string expirySeconds, bool emergencyModeEnabled, bool enableMessagingOfPCs, bool enableEmail, bool enablePSExec, bool reattemptOnError, bool dontSaveHistory, string FQDNAddress, decimal EmailPort, string EmailFromAddress, AuthMode authMode, string EmailAccount, string EmailPassword)
        {
            // Create a new XML document
            XmlDocument doc = new();
            // Create the root element
            XmlElement root = doc.CreateElement("RMCSettings");
            doc.AppendChild(root);

            // Helper function to add elements to the root
            void AddElement(string name, string value)
            {
                XmlElement elem = doc.CreateElement(name);
                elem.InnerText = value;
                root.AppendChild(elem);
            }

            // Helper function to add boolean elements as "Enabled" or "Disabled"
            void AddBoolElement(string name, bool value)
            {
                AddElement(name, value ? "Enabled" : "Disabled");
            }
            //Add version of the software to the document, this is for future compatibility.
            AddElement("RMCSoftwareVersion", Application.ProductVersion);

            // Add elements to the document
            AddElement("Message", messageContent);
            AddElement("PCList", pcList);
            AddElement("WOLList", WOLlist);
            AddElement("WOLPort", WOLPort.ToString());
            AddElement("ExpiryHour", expiryHour);
            AddElement("ExpiryMinutes", expiryMinutes);
            AddElement("ExpirySeconds", expirySeconds);
            AddBoolElement("EmergencyModeEnabled", emergencyModeEnabled);
            AddBoolElement("EnableMessagingOfPCs", enableMessagingOfPCs);
            AddBoolElement("EnableEmail", enableEmail);
            AddBoolElement("EnablePSExec", enablePSExec);
            AddBoolElement("ReattemptOnError", reattemptOnError);
            AddBoolElement("DontSaveHistory", dontSaveHistory);
            AddElement("FQDNAddress", FQDNAddress);
            AddElement("EmailPort", EmailPort.ToString());
            AddElement("EmailFromAddress", EmailFromAddress);
            AddElement("AuthMode", authMode.ToString());
            AddElement("EmailAccount", EmailAccount);
            AddElement("EmailPassword", EmailPassword); //NOTE: This is encrypted by the main form class.

            // Save the document to the specified file path
            doc.Save(filePath);
        }
        public static string[] LoadRMCEmailFile(string filePath)
        {
            //Load the email file from the specified file path.
            //Here is the return array structure:
            // [0] = Error message (if any)
            // [1] = RMC Software Version
            // [2] = Email Subject
            // [3] = Email Body
            // [4] = Is HTML enabled
            try
            {
                XmlDocument doc = new();
                doc.Load(filePath);

                // Helper function to get element's inner text by tag name
                string GetElementValue(string tagName)
                {
                    var node = doc.GetElementsByTagName(tagName).Item(0);
                    return node?.InnerText ?? string.Empty;
                }

                // Helper function to check if a module is enabled
                bool IsEnabled(string tagName) => GetElementValue(tagName).Equals("Enabled", StringComparison.OrdinalIgnoreCase);

                // Extract values
                string RMCSoftwareVersion = GetElementValue("RMCSoftwareVersion");
                string emailSubject = GetElementValue("EmailSubject");
                string emailBody = GetElementValue("EmailBody");
                bool isHTML = IsEnabled("IsHTML");

                //return as an array
                return ["", RMCSoftwareVersion, emailSubject, emailBody, isHTML ? "Enabled" : "Disabled"];
            }
            catch (Exception ex)
            {
                return [$"Error - IO Manager Exception: {ex.Message}"];
            }
        }

        public static void SaveRMCEmailFile(string filepath, string subject, string body, bool isHTML)
        {
            //Create a new XML document to store the email document.
            XmlDocument doc = new();
            //Create the root element for the email document.
            XmlElement root = doc.CreateElement("RMCEmailSettings");
            doc.AppendChild(root);
            // Helper function to add elements to the root
            void AddElement(string name, string value)
            {
                XmlElement elem = doc.CreateElement(name);
                elem.InnerText = value;
                root.AppendChild(elem);
            }

            // Helper function to add boolean elements as "Enabled" or "Disabled"
            void AddBoolElement(string name, bool value)
            {
                AddElement(name, value ? "Enabled" : "Disabled");
            }
            //Add version of the software to the document, this is for future compatibility.
            AddElement("RMCSoftwareVersion", Application.ProductVersion);
            AddElement("EmailSubject", subject);
            AddElement("EmailBody", body);
            AddBoolElement("IsHTML", isHTML);
            //Save
            doc.Save(filepath);
        }
        public static string AttemptToCreateRMCDirectories()
        {
            bool directoriesCreated = false;
            string[] directories = [
                Path.Combine(Application.StartupPath, "Broadcast History Logs"),
                Path.Combine(Application.StartupPath, "RMSG Files"),
                Path.Combine(Application.StartupPath, "RMC Runtime Logs"),
                Path.Combine(Application.StartupPath, "Email Files")
            ];
            try
            {
                foreach (var dir in directories)
                {
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                        directoriesCreated = true;
                    }
                }

                if (directoriesCreated) //If the directories were created, show a welcome message to the user.
                {
                    // Show a welcome message to the user and prompt them to agree to the MIT License.

                    MessageBox.Show($"Welcome to RapidMessageCast!\r\n\r\nBy using this software, you agree to the MIT License.\r\n\r\n" +
                                    $"This software is provided as-is, without any warranty or guarantee of any kind.\r\n\r\n" +
                                    $"Please read the license agreement in the 'License' folder for more information. This messagebox will only appear once.",
                                    $"Welcome to RapidMessageCast", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    return "Info - [IOManager]: Required directories created. " + directories.Length + " directories created.";
                }
                
                return "Info - [IOManager]: Required RMC directories already exist. No action needed."; //This message is returned since the next instruction is to load the list of RMSG files on the main form.
            }
            catch (Exception ex)
            {
                return $"Error - [IOManager]: Failure in creating required directories: {ex.Message}";
            }
        }

        public void OpenFileAndProcessContents(TextBox targetTextBox, string logInfo, string logError, Func<string, string>? processFileContents = null)
        {
            OpenFileDialog openFileDialog = new()
            {
                InitialDirectory = Application.StartupPath,
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
            };
            
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string filePath = openFileDialog.FileName;
                    string fileContents = File.ReadAllText(filePath);

                    if (processFileContents != null)
                    {
                        fileContents = processFileContents(fileContents);
                    }

                    targetTextBox.Text = fileContents;
                    _logAction($"Info - [{logInfo}]: Loaded from file: {Path.GetFileName(filePath)}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error reading the file: {ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _logAction($"Error - [{logError}]: Failure in reading the file: {ex}");
                }
            }
        }
        public void SaveFileFromTextBox(TextBox sourceTextBox, string logInfo, string logError)
        {
            SaveFileDialog saveFileDialog = new()
            {
                InitialDirectory = Application.StartupPath,
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string filePath = saveFileDialog.FileName;
                    File.WriteAllText(filePath, sourceTextBox.Text);

                    MessageBox.Show($"{logInfo} saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _logAction($"Info - [{logInfo}]: {logInfo} saved successfully: {Path.GetFileName(filePath)}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving {logError}: {ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _logAction($"Error - [{logError}]: Failure in saving {logError}: {ex}");
                }
            }
        }

        public static void OpenLinkOrFolder(string location)
        {
            //Create a process 
            Process process = new()
            {
                //Set the startInfo to the process.
                StartInfo = new ProcessStartInfo(location)
                {
                    UseShellExecute = true
                }
            };
            //Start the process.
            process.Start();
        }
    }
}