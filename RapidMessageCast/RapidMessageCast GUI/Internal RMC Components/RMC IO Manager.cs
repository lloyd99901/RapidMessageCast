using System.Text;

//--RapidMessageCast Software--
//RMC_IO_Manager.cs - RapidMessageCast Manager

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
    internal class RMC_IO_Manager(Action<string> logAction)
    {
        private static readonly string[] RMCseparator = ["\r\n\r\n"]; //Used for RMC file IO parsing.
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
            
            if (filePath == null)
            {
                return ["Error: File path is null"];
            }

            try
            {
                // Read the contents of the selected file
                string fileContents = File.ReadAllText(filePath);

                // Split the file contents by section headers
                string[] sections = fileContents.Split(RMCseparator, StringSplitOptions.RemoveEmptyEntries);

                // Helper function to get section value and check if it is "Enabled"
                bool IsEnabled(string section) => GetValueFromSection(sections, section) == "Enabled";

                // Extract values
                string message = GetValueFromSection(sections, "[Message]");
                string pcList = GetValueFromSection(sections, "[PCList]");
                string messageDuration = GetValueFromSection(sections, "[MessageDuration]");
                string[] durationParts = messageDuration.Split(':');

                decimal expiryHourTime = durationParts.Length == 3 ? Convert.ToDecimal(durationParts[0]) : 0;
                decimal expiryMinutesTime = durationParts.Length == 3 ? Convert.ToDecimal(durationParts[1]) : 0;
                decimal expirySecondsTime = durationParts.Length == 3 ? Convert.ToDecimal(durationParts[2]) : 0;

                // Check module states
                bool EmergencyModeEnabled = IsEnabled("[EmergencyMode]");
                bool EnableMessagingOfPCs = IsEnabled("[MessagePC]");
                bool EnableEmail = IsEnabled("[MessageEmail]");
                bool EnablePSExec = IsEnabled("[MessagePSExec]");
                bool ReattemptOnErrorCheck = IsEnabled("[ReattemptOnError]");
                bool DontSaveHistoryCheck = IsEnabled("[DontSaveHistory]");

                // Get WOL list and port
                string WOLlist = GetValueFromSection(sections, "[WOL]");
                string WOLPort = GetValueFromSection(sections, "[WOLPort]");

                // Return the extracted values via an array
                return
                [
                    "",
                    message,
                    pcList,
                    expiryHourTime.ToString(),
                    expiryMinutesTime.ToString(),
                    expirySecondsTime.ToString(),
                    EmergencyModeEnabled.ToString(),
                    EnableMessagingOfPCs.ToString(),
                    EnableEmail.ToString(),
                    EnablePSExec.ToString(),
                    ReattemptOnErrorCheck.ToString(),
                    DontSaveHistoryCheck.ToString(),
                    WOLlist,
                    WOLPort
                ];
            }
            catch (Exception ex)
            {
                // Return an error message along with the exception
                MessageBox.Show($"Error - RMC_IO_Manager: Failure in loading RMSG file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return [$"Error: {ex}"];
            }
        }

        public static void SaveRMSGFile(string filePath, string messageContent, string pcList, string WOLlist, int WOLPort, string expiryHour, string expiryMinutes, string expirySeconds, bool emergencyModeEnabled, bool enableMessagingOfPCs, bool enableEmail, bool enablePSExec, bool reattemptOnError, bool dontSaveHistory)
        {
            // Create a StringBuilder to store the contents of the RMC file
            var rmcFileContent = new StringBuilder();

            // Append the message content section
            rmcFileContent.AppendLine("[Message]");
            rmcFileContent.AppendLine(messageContent);
            rmcFileContent.AppendLine();

            // Append the PC list section
            rmcFileContent.AppendLine("[PCList]");
            rmcFileContent.AppendLine(pcList);
            rmcFileContent.AppendLine();

            // Append the message duration section
            rmcFileContent.AppendLine("[MessageDuration]");
            rmcFileContent.AppendLine($"{expiryHour}:{expiryMinutes}:{expirySeconds}");

            // Helper function to append sections if enabled
            void AppendSectionIfEnabled(string sectionName, bool isEnabled)
            {
                if (isEnabled)
                {
                    rmcFileContent.AppendLine();
                    rmcFileContent.AppendLine(sectionName);
                    rmcFileContent.AppendLine("Enabled");
                }
            }

            // Add emergency mode and module states to the file if enabled
            AppendSectionIfEnabled("[EmergencyMode]", emergencyModeEnabled);
            AppendSectionIfEnabled("[MessagePC]", enableMessagingOfPCs);
            AppendSectionIfEnabled("[MessageEmail]", enableEmail);
            AppendSectionIfEnabled("[MessagePSExec]", enablePSExec);
            AppendSectionIfEnabled("[ReattemptOnError]", reattemptOnError);
            AppendSectionIfEnabled("[DontSaveHistory]", dontSaveHistory);

            //Add WOL list but if not empty, this fixes a problem where the RMC Manager fails to parse the WOL list if it is empty.
            if (!string.IsNullOrEmpty(WOLlist))
            {
                rmcFileContent.AppendLine();
                rmcFileContent.AppendLine("[WOL]");
                rmcFileContent.AppendLine(WOLlist);
            }

            //Add WOL port
            rmcFileContent.AppendLine();
            rmcFileContent.AppendLine("[WOLPort]");
            rmcFileContent.AppendLine(WOLPort.ToString());

            // Write the contents to the specified file
            File.WriteAllText(filePath, rmcFileContent.ToString());
        }
        public static string AttemptToCreateRMCDirectories()
        {
            string[] directories = [
                Path.Combine(Application.StartupPath, "BroadcastHistory"),
                Path.Combine(Application.StartupPath, "RMSGFiles"),
                Path.Combine(Application.StartupPath, "RMC Runtime Logs")
            ];

            bool directoriesCreated = false;

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

                    return "Info - [RMC_IO_Manager]: Required directories created. (RMSGFiles, RMC Runtime Logs, BroadcastHistory)";
                }
                
                return "Info - [RMC_IO_Manager]: Loading list of RMSG files."; //This message is returned since the next instruction is to load the list of RMSG files on the main form.
            }
            catch (Exception ex)
            {
                return $"Error - [RMC_IO_Manager]: Failure in creating required directories: {ex.Message}";
            }
        }

        private static string GetValueFromSection(string[] sections, string sectionHeader)
        {
            foreach (string section in sections)
            {
                if (section.StartsWith(sectionHeader))
                {
                    // Get the value following the section header
                    string value = section[sectionHeader.Length..].Trim();

                    // Filter out invalid characters only for PC list section
                    if (sectionHeader == "[PCList]")
                    {
                        value = InternalRegexFilters.FilterInvalidPCNames(value);
                    }
                    if (sectionHeader == "[Message]")
                    {
                        value = InternalRegexFilters.FilterInvalidMessage(value);
                    }
                    if (sectionHeader == "[WOL]")
                    {
                        value = InternalRegexFilters.FilterInvalidMACAddresses(value);
                    }

                    return value;
                }
            }
            return string.Empty;
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
    }
}