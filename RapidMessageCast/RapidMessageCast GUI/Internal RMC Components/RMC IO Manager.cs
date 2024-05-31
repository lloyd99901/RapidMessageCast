using System.Text;
using System.Text.RegularExpressions;

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
    internal class RMC_IO_Manager
    {
        private static readonly string[] RMCseparator = ["\r\n\r\n"]; //Used for RMC file IO parsing.
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
                    DontSaveHistoryCheck.ToString()
                ];
            }
            catch (Exception ex)
            {
                // Return an error message along with the exception
                MessageBox.Show($"Error - RMC_IO_Manager: Failure in loading RMSG file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return [$"Error: {ex}"];
            }
        }

        public static void SaveRMSGFile(string filePath, string messageContent, string pcList, string expiryHour, string expiryMinutes, string expirySeconds, bool emergencyModeEnabled, bool enableMessagingOfPCs, bool enableEmail, bool enablePSExec, bool reattemptOnError, bool dontSaveHistory)
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
                        value = FilterInvalidCharacters(value);
                    }

                    return value;
                }
            }
            return string.Empty;
        }
        private static string FilterInvalidCharacters(string text)
        {
            // Regular expression to match characters that are not allowed in NetBIOS or Windows hostnames
            string pattern = @"[^\p{L}\p{N}\-\._\n\r]";

            // Replace invalid characters with empty string
            return Regex.Replace(text, pattern, "");
        }

    }
}