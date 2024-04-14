using System.Diagnostics;
using System.DirectoryServices;
using System.Security.Principal;
using System.Text.RegularExpressions;

namespace RapidMessageCast_Manager
{
    public partial class RMCManager : Form
    {
        public string versionNumb = "v0.1 indev 2024";
        public RMCManager()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            AddItemToListBox("RMC Manager " + versionNumb + ". Welcome.");
            // Determine whether RMSGFiles directory exists, if not, create it. Error if it fails.
            if (!Directory.Exists(Application.StartupPath + "\\RMSGFiles"))
            {
                try
                {
                    Directory.CreateDirectory(Application.StartupPath + "\\RMSGFiles");
                    AddItemToListBox("Info - RMSGFiles directory created.");
                }
                catch (Exception ex)
                {
                    AddItemToListBox("Error - Failure in creating RMSGFiles directory. " + ex.Message);
                }
            }
            else
            {
                RefreshRMSGFileList();
            }
            //Check if the user is running the program as an administrator. If not, display a message.
            if (!IsAdministrator())
            {
                AddItemToListBox("Warning - The program is not running as an administrator. Broadcasting messages may not work.");
            }
            //If there is a RMSG file called default.rmsg, load it into the program.
            if (File.Exists(Application.StartupPath + "\\RMSGFiles\\default.rmsg"))
            {
                LoadRMSGFileFunction(Application.StartupPath + "\\RMSGFiles\\default.rmsg");
                AddItemToListBox("Info - Default.rmsg file loaded.");
            }
            Text = "RapidMessageCast Manager - " + versionNumb;
            AddItemToListBox("RMC Manager is now ready.");
        }

        private bool IsAdministrator()
        {
            //Check if the user is running the program as an administrator.
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private void RefreshRMSGFileList()
        {
            //Add all files that exists in the application startup + RMSGFiles directory into the RMSGFiles listbox.
            try
            {
                RMSGFileListBox.Items.Clear();
                string[] files = Directory.GetFiles(Application.StartupPath + "\\RMSGFiles\\");
                foreach (string file in files)
                {
                    RMSGFileListBox.Items.Add(Path.GetFileName(file));
                }
            }
            catch (Exception ex)
            {
                AddItemToListBox("Error - Failure in creating QuickLoad folder(s). " + ex.Message);
            }
        }

        private void ActiveDirectorySelectBtn_Click(object sender, EventArgs e)
        {
            //If possible, I will create a form to select the OU's from the Active Directory. For now, it will just add all computers from the Computers OU.
            //--Connect to the Active Directory, show a form window with all the available OU's, let the user select one or all. Then add all the computers from the OU that the user selected to the pclist.--
            AddItemToListBox("Info - Attempting to connect to Active Directory.");
            try
            {
                DirectoryEntry entry = new DirectoryEntry("LDAP://OU=Computers,DC=example,DC=com");
                DirectorySearcher mySearcher = new DirectorySearcher(entry);
                mySearcher.Filter = ("(objectClass=computer)");
                mySearcher.SizeLimit = int.MaxValue;
                mySearcher.PageSize = int.MaxValue;
                foreach (SearchResult resEnt in mySearcher.FindAll())
                {
                    ComputerSelectList.Text += resEnt.GetDirectoryEntry().Name + "\r\n";
                }
                AddItemToListBox("Info - Computers from Active Directory added to the list.");
            }
            catch (Exception ex)
            {
                AddItemToListBox("Error - Failure in connecting to Active Directory. " + ex.Message);
            }

        }

        private void messageTxt_TextChanged(object sender, EventArgs e)
        {
            int remainingCharacters = 255 - MessageTxt.TextLength;
            if (remainingCharacters >= 0)
            {
                MessageLimitLbl.Text = "Length Remaining: " + remainingCharacters.ToString();
                MessageLimitLbl.ForeColor = Color.White;
            }
            else
            {
                // Prevent entering more characters than the limit
                MessageTxt.Text = MessageTxt.Text.Substring(0, 255);
            }
            if (remainingCharacters <= 30)
            {
                MessageLimitLbl.ForeColor = Color.Red;
            }
        }

        private void messageTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; // Suppress the Enter key press
            }
        }

        private string GetValueFromSection(string[] sections, string sectionHeader)
        {
            foreach (string section in sections)
            {
                if (section.StartsWith(sectionHeader))
                {
                    // Get the value following the section header
                    string value = section.Substring(sectionHeader.Length).Trim();

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
        private string FilterInvalidCharacters(string text)
        {
            // Regular expression to match characters that are not allowed in NetBIOS or Windows hostnames
            string pattern = @"[^\p{L}\p{N}\-\._\n\r]";

            // Replace invalid characters with empty string
            return Regex.Replace(text, pattern, "");
        }

        private void openMessageTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                // Set the initial directory and file filter
                InitialDirectory = Application.StartupPath,
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Read the selected file and display its contents in the TextBox
                    string filePath = openFileDialog.FileName;
                    string fileContents = File.ReadAllText(filePath);
                    MessageTxt.Text = fileContents;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error reading the file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void openSendComputerListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                // Set the initial directory and file filter
                InitialDirectory = Application.StartupPath,
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Read the selected file and filter out invalid characters
                    string filePath = openFileDialog.FileName;
                    string fileContents = File.ReadAllText(filePath);

                    // Filter out invalid characters
                    string filteredContents = FilterInvalidCharacters(fileContents);

                    // Display the filtered contents in the TextBox
                    ComputerSelectList.Text = filteredContents;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error reading the file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ComputerSelectList_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\n' || e.KeyChar == '\r' || e.KeyChar == '\b')
            {
                return;
            }

            // Check if the pressed key is invalid
            string invalidCharsPattern = @"[^\p{L}\p{N}\-\._\n\r]";
            if (Regex.IsMatch(e.KeyChar.ToString(), invalidCharsPattern))
            {
                // Suppress the invalid key press
                e.Handled = true;
            }
        }
        private void OpenRMSGFileBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set the initial directory and file filter
            openFileDialog.InitialDirectory = Application.StartupPath;
            openFileDialog.Filter = "Rapid Message Files (*.rmsg)|*.rmsg|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadRMSGFileFunction(openFileDialog.FileName);
            }
        }

        private void LoadRMSGFileFunction(string filePath)
        {
            try
            {
                // Read the contents of the selected file
                string fileContents = File.ReadAllText(filePath);

                // Split the file contents by section headers
                string[] sections = fileContents.Split(new[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                // Extract message, PC list, and message duration from sections
                string message = GetValueFromSection(sections, "[Message]");
                string pcList = GetValueFromSection(sections, "[PCList]");
                string messageDuration = GetValueFromSection(sections, "[MessageDuration]");

                // Populate the TextBoxes with the extracted values
                MessageTxt.Text = message;
                ComputerSelectList.Text = pcList;

                string[] durationParts = messageDuration.Split(':');
                if (durationParts.Length == 3)
                {
                    expiryHourTime.Value = Convert.ToDecimal(durationParts[0]);
                    expiryMinutesTime.Value = Convert.ToDecimal(durationParts[1]);
                    expirySecondsTime.Value = Convert.ToDecimal(durationParts[2]);
                }

                Console.WriteLine("Message loaded successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading message: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveComputerListBtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // Set the initial directory and file filter
            saveFileDialog.InitialDirectory = Application.StartupPath;
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Get the file path selected by the user
                    string filePath = saveFileDialog.FileName;

                    // Write the PC list text to the file
                    File.WriteAllText(filePath, ComputerSelectList.Text);

                    MessageBox.Show("PC list saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving PC list: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SaveMessageBtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // Set the initial directory and file filter
            saveFileDialog.InitialDirectory = Application.StartupPath;
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Get the file path selected by the user
                    string filePath = saveFileDialog.FileName;

                    // Write the message text to the file
                    File.WriteAllText(filePath, MessageTxt.Text);

                    MessageBox.Show("Message text saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving message text: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void StartBroadcastBtn_Click(object sender, EventArgs e)
        {
            int totalSeconds = ((int)expiryHourTime.Value * 3600) + ((int)expiryMinutesTime.Value * 60) + (int)expirySecondsTime.Value; //Calculate the total seconds from the hours, minutes and seconds for the message duration.
            BeginMessageCast(MessageTxt.Text, ComputerSelectList.Text, totalSeconds); //Start the message cast.

        }
        public void BeginMessageCast(string message, string pcList, int duration)
        {
            string[] pcNames = pcList.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string pcName in pcNames)
            {
                Task.Run(() =>
                {
                    try
                    {
                        AddItemToListBox($"Initialising to message PC: {pcName}");

                        var processInfo = new ProcessStartInfo
                        {
                            FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "msg.exe"),
                            Arguments = $"* /TIME:{duration} /SERVER:{pcName} \"{message}\"",
                            CreateNoWindow = true,
                            UseShellExecute = false
                        };

                        var process = new Process { StartInfo = processInfo };

                        // Start the process
                        process.Start();

                        AddItemToListBox($"MSG process sent for PC: {pcName}");
                    }
                    catch (Exception ex)
                    {
                        AddItemToListBox($"ERROR - Failure to send command for PC: {pcName}");
                        AddItemToListBox(ex.ToString());
                    }
                });
            }
        }

        private void clearLogBtn_Click(object sender, EventArgs e)
        {
            logList.Items.Clear();
        }

        private void AddItemToListBox(string item)
        {
            if (logList.InvokeRequired)
            {
                logList.Invoke(new MethodInvoker(() => AddItemToListBox(item)));
            }
            else
            {
                logList.Items.Add(item);
                logList.TopIndex = logList.Items.Count - 1; // Scroll to the latest item
            }
        }

        private void AboutLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("RapidMessageCast [RMC] " + versionNumb + " created by lloyd99901 on GitHub. Under MIT license. Free to use for personal and commerical settings.");
        }

        private void LoadSelectedRMSGBtn_Click(object sender, EventArgs e)
        {
            //Get the name of the selected item on the listbox, and load it into the LoadRMSGFile function.
            string? selectedFile = RMSGFileListBox.SelectedItem?.ToString();
            if (selectedFile != null)
            {
                LoadRMSGFileFunction(Path.Combine(Application.StartupPath, "RMSGFiles", selectedFile));
            }
        }

        private void SaveRMSGBttn(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // Set the initial directory and file filter
            saveFileDialog.InitialDirectory = Application.StartupPath;
            saveFileDialog.Filter = "Rapid Message Files (*.rmsg)|*.rmsg|All Files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                SaveRMSGFileFunction(saveFileDialog.FileName);
            }
        }

        private void SaveRMSGFileFunction(string filePath)
        {
            try
            {
                // Prepare the message content
                string messageContent = $"[Message]\r\n{MessageTxt.Text}\r\n\r\n";
                messageContent += $"[PCList]\r\n{ComputerSelectList.Text}\r\n\r\n";
                messageContent += $"[MessageDuration]\r\n{expiryHourTime.Value}:{expiryMinutesTime.Value}:{expirySecondsTime.Value}";

                // Write the message content to the file
                File.WriteAllText(filePath, messageContent);
                //Add to loglist 
                AddItemToListBox("Message saved successfully!");
                // Refresh the list of RMSG files
                RefreshRMSGFileList();
            }
            catch (Exception ex)
            {
                AddItemToListBox("Error saving message: " + ex.Message);
            }
        }

        private void QuickSaveRMSGBtn_Clicked(object sender, EventArgs e)
        {
            //Save a quick save file name based on the current date and time.
            string quickSaveFileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".rmsg";

            SaveRMSGFileFunction(Path.Combine(Application.StartupPath, "RMSGFiles", quickSaveFileName));
        }

        private void RefreshRMSGListBtn_Click(object sender, EventArgs e)
        {
            RefreshRMSGFileList();
        }

        private void DeleteSelectedRMSGFileBtn_Click(object sender, EventArgs e)
        {
            //Delete the selected file from the listbox.
            string? selectedFile = RMSGFileListBox.SelectedItem?.ToString();
            if (selectedFile != null)
            {
                try
                {
                    //Ask the user if they are sure they want to delete the file. If yes, delete it.
                    DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete the file: " + selectedFile + "?", "Delete File", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        File.Delete(Path.Combine(Application.StartupPath, "RMSGFiles", selectedFile));
                        AddItemToListBox("File deleted successfully.");
                        RefreshRMSGFileList();
                    }
                }
                catch (Exception ex)
                {
                    AddItemToListBox("Error deleting file: " + ex.Message);
                }
            }
        }

        private void RMSGFileListBox_DoubleClick(object sender, EventArgs e)
        {
            //Load the selected file when double clicked.
            LoadSelectedRMSGBtn_Click(sender, e);
        }

        private void RenameSelectedRMSGBtn_Click(object sender, EventArgs e)
        {
            //Rename the selected file from the listbox with a new name that the user inputs. If the .RMSG is missing, put it back in.
            string? selectedFile = RMSGFileListBox.SelectedItem?.ToString();
            if (selectedFile != null)
            {
                string newFileName = Microsoft.VisualBasic.Interaction.InputBox("Enter the new name for the file. (the .rmsg at the end will be added if it's missing)", "Rename File", selectedFile);
                if (newFileName != "")
                {
                    if (!newFileName.EndsWith(".rmsg"))
                    {
                        newFileName += ".rmsg";
                    }
                    try
                    {
                        File.Move(Path.Combine(Application.StartupPath, "RMSGFiles", selectedFile), Path.Combine(Application.StartupPath, "RMSGFiles", newFileName));
                        AddItemToListBox("File renamed successfully.");
                        RefreshRMSGFileList();
                    }
                    catch (Exception ex)
                    {
                        AddItemToListBox("Error renaming file: " + ex.Message);
                    }
                }
            }
        }

        private void MessageDurationHelpLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //Open up a messagebox with information on what the message duration is. (all 0 is never expires/closes)
            MessageBox.Show("The message duration is how long the message will be displayed on the screen for the user. The format is in HH:MM:SS. If it's set to all 0's, the message will never close until the user closes it.");
        }

        private void RMSGHelpLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //Open up a messagebox with information on what the RMSG file is.
            MessageBox.Show("The RMSG file is a file that contains the message, the PC list and the message duration. It's used to quickly send messages to a list of computers without having to type it out every time.");
        }

        private void RMCManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Ask the user if they want to close the program if they press the X button. However, if the MessageTxt and pclist is empty, close the program without asking.
            if (MessageTxt.Text == "" && ComputerSelectList.Text == "")
            {
                return;
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to close the program? Any unsaved data will be lost.", "Close Program - RapidMessageCast Manager", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
