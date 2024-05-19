using System.Text.RegularExpressions;

namespace RapidMessageCast_Manager.Modules
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

            //Declare variables to store the extracted values.
            bool EmergencyModeEnabled;
            bool EnableMessagingOfPCs;
            bool EnableEmail;
            bool EnablePSExec;
            string MessageContent;
            string PCList;
            decimal expiryHourTime = 0;
            decimal expiryMinutesTime = 0;
            decimal expirySecondsTime = 0;
            bool ReattemptOnErrorCheck;
            bool DontSaveHistoryCheck;

            try
            {
                // Read the contents of the selected file
                string fileContents = File.ReadAllText(filePath);

                // Split the file contents by section headers
                string[] sections = fileContents.Split(RMCseparator, StringSplitOptions.RemoveEmptyEntries);

                // Extract message, PC list, and message duration from sections
                string message = GetValueFromSection(sections, "[Message]");
                string pcList = GetValueFromSection(sections, "[PCList]");
                string messageDuration = GetValueFromSection(sections, "[MessageDuration]");
                //Check if emergency mode is enabled in the file.
                string emergencyMode = GetValueFromSection(sections, "[EmergencyMode]");
                if (emergencyMode == "Enabled")
                {
                    EmergencyModeEnabled = true;
                }
                else
                {
                    EmergencyModeEnabled = false;
                }

                //Check module states in the file. if it exists, enable it. if not, disable it.
                string messagePC = GetValueFromSection(sections, "[MessagePC]");
                if (messagePC == "Enabled")
                {
                    EnableMessagingOfPCs = true;
                }
                else
                {
                    EnableMessagingOfPCs = false;
                }
                string messageEmail = GetValueFromSection(sections, "[MessageEmail]");
                if (messageEmail == "Enabled")
                {
                    EnableEmail = true;
                }
                else
                {
                    EnableEmail = false;
                }
                string messagePSExec = GetValueFromSection(sections, "[MessagePSExec]");
                if (messagePSExec == "Enabled")
                {
                    EnablePSExec = true;
                }
                else
                {
                    EnablePSExec = false;
                }
                //Get the reattempt on error state from the file.
                string reattemptOnError = GetValueFromSection(sections, "[ReattemptOnError]");
                if (reattemptOnError == "Enabled")
                {
                    ReattemptOnErrorCheck = true;
                }
                else
                {
                    ReattemptOnErrorCheck = false;
                }
                //Get the dont save history state from the file.
                string dontSaveHistory = GetValueFromSection(sections, "[DontSaveHistory]");
                if (dontSaveHistory == "Enabled")
                {
                    DontSaveHistoryCheck = true;
                }
                else
                {
                    DontSaveHistoryCheck = false;
                }

                // Populate the TextBoxes with the extracted values
                MessageContent = message;
                PCList = pcList;

                string[] durationParts = messageDuration.Split(':');
                if (durationParts.Length == 3)
                {
                    expiryHourTime = Convert.ToDecimal(durationParts[0]);
                    expiryMinutesTime = Convert.ToDecimal(durationParts[1]);
                    expirySecondsTime = Convert.ToDecimal(durationParts[2]);
                }
                //Return the extracted values via an array.
                return ["", MessageContent, PCList, expiryHourTime.ToString(), expiryMinutesTime.ToString(), expirySecondsTime.ToString(), EmergencyModeEnabled.ToString(), EnableMessagingOfPCs.ToString(), EnableEmail.ToString(), EnablePSExec.ToString(), ReattemptOnErrorCheck.ToString(), DontSaveHistoryCheck.ToString()];
            }
            catch (Exception ex)
            {
                //Return an error message along with the exception.
                MessageBox.Show("Error - RMC_IO_Manager: Failure in loading RMSG file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return ["Error" + ex.ToString()];
            }
        }

        public static void SaveRMSGFile(string filePath, string messageContent, string pcList, string expiryHour, string expiryMinutes, string expirySeconds, bool emergencyModeEnabled, bool enableMessagingOfPCs, bool enableEmail, bool enablePSExec, bool reattemptOnError, bool dontSaveHistory)
        {
            // Create a string builder to store the contents of the RMC file
            //StringBuilder rmcFileContent = new StringBuilder();

            // Append the message content section
            string rmcFileContent = $"[Message]\r\n{messageContent}\r\n\r\n";

            // Append the PC list section
            rmcFileContent += $"[PCList]\r\n{pcList}\r\n\r\n";

            // Append the message duration section
            rmcFileContent += $"[MessageDuration]\r\n{expiryHour}:{expiryMinutes}:{expirySeconds}";

            //Add emergency mode to the message content if it's enabled.
            if (emergencyModeEnabled)
            {
                rmcFileContent += "\r\n\r\n[EmergencyMode]\r\nEnabled";
            }
            //Add the Module States to the file (Message, Email, PSExec), If it enabled, add it to the file. if not, do not add it.
            if (enableMessagingOfPCs)
            {
                rmcFileContent += "\r\n\r\n[MessagePC]\r\nEnabled";
            }
            if (enableEmail)
            {
                rmcFileContent += "\r\n\r\n[MessageEmail]\r\nEnabled";
            }
            if (enablePSExec)
            {
                rmcFileContent += "\r\n\r\n[MessagePSExec]\r\nEnabled";
            }
            if (reattemptOnError)
            {
                rmcFileContent += "\r\n\r\n[ReattemptOnError]\r\nEnabled";
            }
            if (dontSaveHistory)
            {
                rmcFileContent += "\r\n\r\n[DontSaveHistory]\r\nEnabled";
            }
            // Write the contents to the specified file
            File.WriteAllText(filePath, rmcFileContent);
        }

        public static string AttemptToCreateRMCDirectories()
        {
            //Create a directory called BroadcastHistory if it doesn't exist.
            if (!Directory.Exists(Application.StartupPath + "\\BroadcastHistory") || !Directory.Exists(Application.StartupPath + "\\RMSGFiles") || !Directory.Exists(Application.StartupPath + "\\RMC Runtime Logs"))
            {
                try
                {
                    Directory.CreateDirectory(Application.StartupPath + "\\BroadcastHistory");
                    Directory.CreateDirectory(Application.StartupPath + "\\RMSGFiles");
                    Directory.CreateDirectory(Application.StartupPath + "\\RMC Runtime Logs");
                    //Show a welcome msgbox to the user and also allow them to agree to the MIT License.
                    MessageBox.Show("Welcome to RapidMessageCast!\r\n\r\nBy using this software, you agree to the MIT License.\r\n\r\n" +
                        "This software is provided as-is, without any warranty or guarantee of any kind.\r\n\r\n" +
                        "Please read the license agreement in the 'License' folder for more information. This messagebox will only appear once.", "Welcome to RapidMessageCast", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return "Info - RMC_IO_Manager: Required directories created. (RMSGFiles, RMC Runtime Logs, BroadcastHistory)";
                }
                catch (Exception ex)
                {
                    return "Error - RMC_IO_Manager: Failure in creating required directories: " + ex.Message;
                }
            }
            else
            {
                //Add to loglist that rmsg files are being loaded.
                return "Info - RMC_IO_Manager: Loading list of RMSG files.";
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
