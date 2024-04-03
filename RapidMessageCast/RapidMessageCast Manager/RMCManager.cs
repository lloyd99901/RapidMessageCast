using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Text.RegularExpressions;

namespace RapidMessageCast_Manager
{
    public partial class RMCManager : Form
    {

        public RMCManager()
        {
            InitializeComponent();
        }
        public string versionNumb = "v0.1 indev 2024";
        private void Form1_Load(object sender, EventArgs e)
        {
            Text = "RapidMessageCast Manager - " + versionNumb;
            try
            {
                if (string.IsNullOrEmpty(Domain.GetCurrentDomain().Name))
                {
                    domainText.Text = "Domain Status: Computer isn't joined to a domain.";
                }
                else
                {
                    domainText.Text = "Domain Status: Connected to " + Domain.GetCurrentDomain().Name;
                }
            }
            catch (ActiveDirectoryOperationException)
            {
                domainText.Text = "Domain Status: Current security context is not associated with an Active Directory domain or forest.";
            }
        }

        private void ActiveDirectorySelectBtn_Click(object sender, EventArgs e)
        {

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

        private void ExitToolButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AboutStripButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("RapidMessageCast [RMC] " + versionNumb + " created by lloyd99901 on GitHub. Under MIT license. Free to use for personal and commerical settings.");
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
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
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
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set the initial directory and file filter
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

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

        private void SaveMSG(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // Set the initial directory and file filter
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog.Filter = "Rapid Message Files (*.rmsg)|*.rmsg|All Files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Get the file path selected by the user
                    string filePath = saveFileDialog.FileName;

                    // Prepare the message content
                    string messageContent = $"[Message]\r\n{MessageTxt.Text}\r\n\r\n";
                    messageContent += $"[PCList]\r\n{ComputerSelectList.Text}\r\n\r\n";
                    messageContent += $"[MessageDuration]\r\n{expiryHourTime.Value}:{expiryMinutesTime.Value}:{expirySecondsTime.Value}";

                    // Write the message content to the file
                    File.WriteAllText(filePath, messageContent);

                    MessageBox.Show("Message saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving message: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void OpenRMSGFileBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set the initial directory and file filter
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.Filter = "Rapid Message Files (*.rmsg)|*.rmsg|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Get the file path selected by the user
                    string filePath = openFileDialog.FileName;

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
        }

        private void SaveComputerListBtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // Set the initial directory and file filter
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
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
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
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
            int totalSeconds = ((int)expiryHourTime.Value * 3600) + ((int)expiryMinutesTime.Value * 60) + (int)expirySecondsTime.Value;
            BeginMessageCast(MessageTxt.Text, ComputerSelectList.Text, totalSeconds);

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
                        AddItemToListBox("Initialising to message PC: " + pcName);

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

                        AddItemToListBox("MSG process sent for PC: " + pcName);
                    }
                    catch (Exception ex)
                    {
                        AddItemToListBox("ERROR - Failure to send command for PC: " + pcName);
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
    }
}
