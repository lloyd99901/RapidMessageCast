using RapidMessageCast_Manager.Modules;
using System.Diagnostics;
using System.DirectoryServices;
using System.Security.Principal;
using System.Text.RegularExpressions;

//MIT License

//Copyright (c) 2024 Lunar

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

namespace RapidMessageCast_Manager
{
    public partial class RMCManager : Form
    {
        public string versionNumb = "v0.1 indev 2024";
        private static readonly char[] PCseparatorArray = ['\n', '\r']; //Used for PCList parsing.
        readonly List<string> broadcastHistoryBuffer = []; //Buffer for the broadcast history. This will be saved to a file after the broadcast has finished.
        readonly ImageList tabControlImageList = new();
        private bool isScheduledBroadcast = false;
        public RMCManager()
        {
            //Add images to the tabcontrol. Used for the icons on the tabs.
            tabControlImageList.Images.Add("Broadcast", Properties.Resources.icons8_radio_tower_24_black);
            tabControlImageList.Images.Add("PC", Properties.Resources.icons8_pc_24_black);
            tabControlImageList.Images.Add("Message", Properties.Resources.icons8_chat_bubble_24_black);
            tabControlImageList.Images.Add("Email", Properties.Resources.icons8_email_24_black);
            tabControlImageList.Images.Add("WakeOnLAN", Properties.Resources.icons8_turn_on_24_black);
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            AddItemToListBox("INFO - Starting RMC GUI " + versionNumb + ". Welcome. - " + DateTime.Now.ToString());
            //Check if the program has a argument that contains a .rmsg file and also contains schedule, if it does, immediately start the broadcast with that file and close the form.
            if (Environment.GetCommandLineArgs().Length > 1)
            {
                AddItemToListBox("INFO - Command line arguments detected.");
                //If program only has one argument, load the file into the program. But if it has two arguments, check if the second argument is schedule. If it is, start the broadcast.
                if (Environment.GetCommandLineArgs().Length == 2)
                {
                    AddItemToListBox("INFO - Loading RMSG file from command line argument.");
                    LoadRMSGFileInProgram(Environment.GetCommandLineArgs()[1]);
                }
                else if (Environment.GetCommandLineArgs().Length == 3 && Environment.GetCommandLineArgs()[2] == "schedule")
                {
                    isScheduledBroadcast = true; //Used to tell the broadcast code to close the program after the broadcast has finished.
                    RunScheduledBroastcast(Environment.GetCommandLineArgs()[1]);
                    //This is a scheduled message broadcast. Start the broadcast immediately.
                }
            }
            CheckSystemState();
            LoadGlobalSettings();
            AddIconsToTabControls();
            AddItemToListBox(RMC_IO_Manager.AttemptToCreateRMCDirectories()); //Create the directories for the program and then displays the status in the loglist.
            RefreshRMSGFileList();
            //If there is a RMSG file called default.rmsg, load it into the program.
            if (File.Exists(Application.StartupPath + "\\RMSGFiles\\default.rmsg"))
            {
                LoadRMSGFileInProgram(Application.StartupPath + "\\RMSGFiles\\default.rmsg");
                AddItemToListBox("Info - Default.rmsg file loaded.");
            }
            versionLbl.Text = versionNumb;
            Text = "RapidMessageCast GUI - " + versionNumb;
            AddItemToListBox("INFO - RMC GUI is now ready.");
        }
        #region Functions
        //Start of the functions.
        private void RunScheduledBroastcast(string RMSGFile)
        {
            //Hide form
            Hide();
            LoadRMSGFileInProgram(RMSGFile); //Load the RMSG file into the program.
            //Check if modules are selected. If not, close the program.
            if (!MessagePCcheckBox.Checked && !MessageEmailcheckBox.Checked && !MessagePSExecCheckBox.Checked)
            {
                Application.Exit();
            }
            //Check if message or pclist is empty. If it is, close the program.
            if (MessageTxt.Text == "" || ComputerSelectList.Text == "")
            {
                Application.Exit();
            }
            int totalSeconds = ((int)expiryHourTime.Value * 3600) + ((int)expiryMinutesTime.Value * 60) + (int)expirySecondsTime.Value; //Calculate the total seconds from the hours, minutes and seconds for the message duration.
            BeginPCMessageCast(MessageTxt.Text, ComputerSelectList.Text, totalSeconds, false); //Start the message cast.
            Hide();
        }

        private void CheckSystemState()
        {
            //This function will check the system state and display messages to the user if something that might impact messaging is detected.
            //Check if msg.exe exists in the system32 folder. If not, display a message to the user.
            if (!File.Exists("C:\\Windows\\System32\\msg.exe"))
            {
                AddItemToListBox("Error - msg.exe not found in the System32 folder. Please ensure that you have a supported operating system edition.");
                MessageBox.Show("msg.exe not found in the System32 folder. Please ensure that you have a supported operating system edition.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //Check if the user is running the program as an administrator. If not, display a message.
            if (!IsAdministrator())
            {
                AddItemToListBox("Notice - The program is not running as an administrator. If broadcasting a message doesn't work, try running this program as administrator.");
            }
            //Check System RAM, if less than 1GB, display a message to the user.
            if (new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory < 1073741824)
            {
                MessageBox.Show("The system has less than 1GB of RAM. Running the broadcast may freeze your computer or take longer to finish if your RAM continues to lower.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                AddItemToListBox("Warning - The system has less than 1GB of RAM. Running the broadcast may freeze your computer or take longer to finish if your RAM continues to lower.");
            }
            //Check if the computer is able to send messages. Check by seeing if the computer has an ip or an available network device.
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                AddItemToListBox("Info - Network is available. The program is able to send messages.");
            }
            else
            {
                AddItemToListBox("Error - Network is not available. The program is unable to send messages.");
                MessageBox.Show("RMC has detected that your computer's network is not available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddIconsToTabControls()
        {
            try
            {
                //Add the image to the TabControl.
                tabControlImageList.ImageSize = new Size(24, 24);
                tabControlImageList.ColorDepth = ColorDepth.Depth32Bit;
                ModulesTabControl.ImageList = tabControlImageList;
                ModulesTabControl.TabPages[0].ImageIndex = 1;
                ModulesTabControl.TabPages[1].ImageIndex = 3;
                //PCTabControl1 as well.
                PCtabControl1.ImageList = tabControlImageList;
                PCtabControl1.TabPages[0].ImageIndex = 2;
                PCtabControl1.TabPages[1].ImageIndex = 0;
                PCtabControl1.TabPages[2].ImageIndex = 4;
            }
            catch (Exception ex)
            {
                AddItemToListBox("Error adding images to the tabcontrol: " + ex.Message);
            }
        }

        public void AddItemToListBox(string item)
        {
            try //This is a failsafe to prevent the program from crashing if an error occurs.
            {
                if (logList.InvokeRequired)
                {
                    logList.Invoke(new MethodInvoker(() => AddItemToListBox(item))); //This was added to prevent crashing when there are multiple threads trying to access the listbox.
                }
                else
                {
                    logList.Items.Add(item);
                    logList.TopIndex = logList.Items.Count - 1; // Scroll to the latest item
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding item to listbox: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadGlobalSettings()
        {
            //Load the settings from the settings file.
            EmergencyModeCheckbox.Checked = Properties.Settings.Default.EmergencyMode;
            DontSaveBroadcastHistoryCheckbox.Checked = Properties.Settings.Default.DontSaveBroadcastHistory;
            MessagePCcheckBox.Checked = Properties.Settings.Default.MessagePCEnabled;
            MessagePSExecCheckBox.Checked = Properties.Settings.Default.MessagePSExecEnabled;
            MessageEmailcheckBox.Checked = Properties.Settings.Default.MessageEmailEnabled;
        }

        private static bool IsAdministrator()
        {
            //Check if the user is running the program as an administrator.
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private static string FilterInvalidCharacters(string text)
        {
            // Regular expression to match characters that are not allowed in NetBIOS or Windows hostnames
            string pattern = @"[^\p{L}\p{N}\-\._\n\r]";

            // Replace invalid characters with empty string
            return Regex.Replace(text, pattern, "");
        }

        private void LoadRMSGFileInProgram(string filePath)
        {
            //Use RMSG_IO_Manager.LoadRMSGFile(openFileDialog.FileName); and store the return values in a string array.
            string[] RMSGFileValues = RMC_IO_Manager.LoadRMSGFile(filePath);

            //Check if the first value in the array is "Error". If it is, report it to the user and add to loglist
            if (RMSGFileValues[0] != "") //If there is something in the first value, it means that there is an error. Break and report it to the user.
            {
                MessageBox.Show("Error loading message: " + RMSGFileValues[0], "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AddItemToListBox("Error loading message: " + RMSGFileValues[0]);
            }
            else
            {
                //Populate the textboxes with the values from the array.
                MessageTxt.Text = RMSGFileValues[1];
                ComputerSelectList.Text = RMSGFileValues[2];
                expiryHourTime.Value = Convert.ToDecimal(RMSGFileValues[3]);
                expiryMinutesTime.Value = Convert.ToDecimal(RMSGFileValues[4]);
                expirySecondsTime.Value = Convert.ToDecimal(RMSGFileValues[5]);
                //Check if emergency mode is enabled in the file.
                if (RMSGFileValues[6] == "True")
                {
                    EmergencyModeCheckbox.Checked = true;
                }
                else
                {
                    EmergencyModeCheckbox.Checked = false;
                }
                //Check module states in the file. if it exists, enable it. if not, disable it.
                if (RMSGFileValues[7] == "True")
                {
                    MessagePCcheckBox.Checked = true;
                }
                else
                {
                    MessagePCcheckBox.Checked = false;
                }
                if (RMSGFileValues[8] == "True")
                {
                    MessageEmailcheckBox.Checked = true;
                }
                else
                {
                    MessageEmailcheckBox.Checked = false;
                }
                if (RMSGFileValues[9] == "True")
                {
                    MessagePSExecCheckBox.Checked = true;
                }
                else
                {
                    MessagePSExecCheckBox.Checked = false;
                }
                //Add to loglist with the name of file that was loaded.
                AddItemToListBox("Info - File loaded successfully: " + Path.GetFileName(filePath));
            }
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
                //Add to loglist that the RMSG files have been loaded. with the amount of files.
                AddItemToListBox("Info - RMSG files loaded. Amount of files: " + files.Length);
            }
            catch (Exception ex)
            {
                AddItemToListBox("Error - Failure in creating QuickLoad folder(s). " + ex.Message);
            }
        }

        public void BeginPCMessageCast(string message, string pcList, int duration, bool Reattempted)
        {
            //Check if msg.exe exists in the system32 folder. If not, display a message to the user.
            if (!File.Exists("C:\\Windows\\System32\\msg.exe"))
            {
                AddItemToListBox("Error - msg.exe not found in the System32 folder. Please ensure that you have a supported operating system.");
                MessageBox.Show("msg.exe not found in the System32 folder. Please ensure that you have a supported operating system.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Clear BroadcastHistory list.
            broadcastHistoryBuffer.Clear();
            //Add that broadcast has started to the loglist.
            AddItemToListBox("Broadcast started.");
            //Check if isScheduledBroadcast is true. If it is, add to the broadcast history that it's a scheduled broadcast.
            if (isScheduledBroadcast)
            {
                broadcastHistoryBuffer.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - Info - Scheduled broadcast has started.");
            }
            //Add the message to the broadcast history.
            broadcastHistoryBuffer.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - Info - Broadcast has started - Message: " + message);
            //Add if emergency mode is enabled to the broadcast history.
            if (EmergencyModeCheckbox.Checked)
            {
                //add to loglist that emergency mode is enabled.
                AddItemToListBox("Emergency mode is enabled. RMC will not wait for the msg processes to exit.");
                broadcastHistoryBuffer.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - Warning - Emergency mode is enabled. RMC will not wait for the msg processes to exit.");
            }
            string[] pcNames = pcList.Split(PCseparatorArray, StringSplitOptions.RemoveEmptyEntries);
            foreach (string pcName in pcNames)
            {
                Task.Run(() =>
                {
                    try
                    {
                        AddItemToListBox($"Initialising to message PC: {pcName}");

                        var processInfo = new ProcessStartInfo
                        {
                            FileName = "C:\\Windows\\System32\\msg.exe",
                            Arguments = $"* /TIME:{duration} /SERVER:{pcName} \"{message}\"",
                            CreateNoWindow = true,
                            UseShellExecute = false
                        };

                        var process = new Process { StartInfo = processInfo };

                        // Start the process
                        process.Start();
                        //Add the PC to the broadcast history.
                        //broadcastHistory.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - Attempting to message - " + pcName);
                        AddItemToListBox($"MSG process sent for PC: {pcName}");

                        //Check if emergency mode is enabled. if it is, do not wait for the process to exit.
                        if (!EmergencyModeCheckbox.Checked)
                        {
                            //Check if the program exits without any errors.
                            if (!process.WaitForExit(1500))
                            {
                                //Add the error to the broadcast history.
                                broadcastHistoryBuffer.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + pcName + " - ERROR - The process did not exit in time.");
                                AddItemToListBox($"ERROR - The process did not exit in time for PC: {pcName}");
                            }
                            else
                            {
                                //Add the success to the broadcast history.
                                broadcastHistoryBuffer.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + pcName + " - SUCCESS - MSG.exe process exited within allocated timelimit.");
                                AddItemToListBox($"SUCCESS - MSG.exe process exited within allocated timelimit: {pcName}");
                            }
                        }
                        else
                        {
                            //Add PC name to the broadcast history. But write unknown if message was sent or not.
                            broadcastHistoryBuffer.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + pcName + " - Message attempted, Unknown if it worked due to emergency mode being enabled - ");
                        }
                    }
                    catch (Exception ex)
                    {
                        //Add the error to the broadcast history.
                        broadcastHistoryBuffer.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + pcName + " - ERROR - " + ex.Message);
                        AddItemToListBox($"ERROR - Failure to send command for PC: {pcName}");
                        AddItemToListBox(ex.ToString());
                        //Attempt to resend message to the PC, unless it's already been reattempted.
                        if (!Reattempted)
                        {
                            broadcastHistoryBuffer.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + pcName + " - Attempting to message PC again - ");
                            AddItemToListBox($"Reattempting to message PC again for a final time: {pcName}");
                            BeginPCMessageCast(message, pcName, duration, true);
                        }
                    }
                });
            }
            //Wait for all processes that contain msg.exe to close before saving the broadcast history.
            Task.Run(() =>
            {
                while (Process.GetProcessesByName("msg").Length > 0)
                {
                    Thread.Sleep(1000);
                }
                //Add end of broadcast to the broadcast history.
                broadcastHistoryBuffer.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - Broadcast has ended.");
                AddItemToListBox("Broadcast finished. Saving broadcast log...");
                SaveBroadcastHistory();
                if (isScheduledBroadcast)
                {
                    //Close the program if it's a scheduled broadcast.
                    AddItemToListBox("Scheduled broadcast finished. Closing program.");
                    Application.Exit();
                }
            });
            //Set the start broadcast button to green and start the timer to change it back to the original color.
            StartBroadcastBtn.BackColor = Color.Green;
            GreenButtonTimer.Start();
        }

        private void SaveBroadcastHistory()
        {
            //Save the broadcast history to a file in the directory called BroadcastHistory.
            //Check if dont save is checked. If it is, do not save the broadcast history.
            if (DontSaveBroadcastHistoryCheckbox.Checked)
            {
                AddItemToListBox("Broadcast history save halted. Don't save history checkbox is checked.");
                return;
            }
            string broadcastHistoryFileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".txt";
            try
            {
                File.WriteAllLines(Application.StartupPath + "\\BroadcastHistory\\" + broadcastHistoryFileName, broadcastHistoryBuffer);
                AddItemToListBox("Broadcast history saved to file: " + broadcastHistoryFileName);
            }
            catch (Exception ex)
            {
                AddItemToListBox("Error - Failure in saving broadcast history. " + ex.Message);
            }
        }

        //End of the functions, start of the events.
        #endregion Functions
        private void ActiveDirectorySelectBtn_Click(object sender, EventArgs e)
        {
            //If possible, I will create a form to select the OU's from the Active Directory. For now, it will just add all computers from the Computers OU.
            //--Connect to the Active Directory, show a form window with all the available OU's, let the user select one or all. Then add all the computers from the OU that the user selected to the pclist.--
            AddItemToListBox("Info - Attempting to connect to Active Directory.");
            try
            {
                DirectoryEntry entry = new("LDAP://OU=Computers,DC=example,DC=com");
                DirectorySearcher mySearcher = new(entry)
                {
                    Filter = ("(objectClass=computer)"),
                    SizeLimit = int.MaxValue,
                    PageSize = int.MaxValue
                };
                foreach (SearchResult resEnt in mySearcher.FindAll())
                {
                    ComputerSelectList.Text += resEnt.GetDirectoryEntry().Name + "\r\n";
                }
                AddItemToListBox("Info - Computers from Active Directory added to the list.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to Active Directory: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageTxt.Text = MessageTxt.Text[..255];
            }
            if (remainingCharacters <= 30)
            {
                MessageLimitLbl.ForeColor = Color.Red;
            }
        }

        private void messageTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            //invalidcharspattern is a regex pattern that will check if the key pressed is invalid. If it is, it will suppress the key press. BAD CHARACTERS: /
            string invalidCharsPattern = @"[\/]";
            if (Regex.IsMatch(e.KeyChar.ToString(), invalidCharsPattern))
            {
                // Suppress the invalid key press
                e.Handled = true;
            }
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
            OpenFileDialog openFileDialog = new()
            {
                // Set the initial directory and file filter
                InitialDirectory = Application.StartupPath,
                Filter = "Rapid Message Files (*.rmsg)|*.rmsg|All Files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadRMSGFileInProgram(openFileDialog.FileName);
            }
        }

        private void SaveComputerListBtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new()
            {
                // Set the initial directory and file filter
                InitialDirectory = Application.StartupPath,
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
            };

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
            SaveFileDialog saveFileDialog = new()
            {
                // Set the initial directory and file filter
                InitialDirectory = Application.StartupPath,
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
            };

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
            //Check if message module is enabled. If it is, start the message cast.
            if (MessagePCcheckBox.Checked)
            {
                //Check if message or pclist is empty. If it is, display a message to the user.
                if (MessageTxt.Text == "" || ComputerSelectList.Text == "")
                {
                    MessageBox.Show("Message or PC list is empty. Please fill in the message and PC list before starting the broadcast.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                int totalSeconds = ((int)expiryHourTime.Value * 3600) + ((int)expiryMinutesTime.Value * 60) + (int)expirySecondsTime.Value; //Calculate the total seconds from the hours, minutes and seconds for the message duration.
                //BeginPCMessageCast(MessageTxt.Text, ComputerSelectList.Text, totalSeconds, false); //Start the message cast.
                //Begin message cast asynchroniously. This will then allow the other code below to run at the same time.
                //Task.Run(() => BeginPCMessageCast(MessageTxt.Text, ComputerSelectList.Text, totalSeconds, false));
                BeginPCMessageCast(MessageTxt.Text, ComputerSelectList.Text, totalSeconds, false);
            }
            //Check if Email module is enabled. If it is, start the Email cast.
            if (MessageEmailcheckBox.Checked)
            {
                //Start the Email cast.
                //send test messagebox to the user.
                MessageBox.Show("Email module is not implemented yet. This is a placeholder message.", "Email Module", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //Check if PSExec module is enabled. If it is, start the PSExec cast.
            if (MessagePSExecCheckBox.Checked)
            {
                //Start the PSExec cast.
                //send test messagebox to the user.
                MessageBox.Show("PSExec module is not implemented yet. This is a placeholder message.", "PSExec Module", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //If nothing is enabled, display a message to the user.
            if (!MessagePCcheckBox.Checked && !MessageEmailcheckBox.Checked && !MessagePSExecCheckBox.Checked)
            {
                MessageBox.Show("No modules are enabled. Please enable at least one module before starting the broadcast.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void clearLogBtn_Click(object sender, EventArgs e)
        {
            logList.Items.Clear();
        }

        private void LoadSelectedRMSGBtn_Click(object sender, EventArgs e)
        {
            //Get the name of the selected item on the listbox, and load it into the LoadRMSGFile function.
            string? selectedFile = RMSGFileListBox.SelectedItem?.ToString();
            if (selectedFile != null)
            {
                LoadRMSGFileInProgram(Path.Combine(Application.StartupPath, "RMSGFiles", selectedFile));
            }
        }

        private void SaveRMSGBttn(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new()
            {
                // Set the initial directory and file filter
                InitialDirectory = Application.StartupPath,
                Filter = "Rapid Message Files (*.rmsg)|*.rmsg|All Files (*.*)|*.*"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                RMC_IO_Manager.SaveRMSGFile(saveFileDialog.FileName, MessageTxt.Text, ComputerSelectList.Text, expiryHourTime.Value.ToString(), expiryMinutesTime.Value.ToString(), expirySecondsTime.Value.ToString(), EmergencyModeCheckbox.Checked, MessagePCcheckBox.Checked, MessageEmailcheckBox.Checked, MessagePSExecCheckBox.Checked);
                RefreshRMSGFileList();
            }
        }

        private void QuickSaveRMSGBtn_Clicked(object sender, EventArgs e)
        {
            //Save a quick save file name based on the current date and time.
            string quickSaveFileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".rmsg";

            //Use the SaveRMSGFile in the RMC_IO_Manager to save the file.
            RMC_IO_Manager.SaveRMSGFile(Path.Combine(Application.StartupPath, "RMSGFiles", quickSaveFileName), MessageTxt.Text, ComputerSelectList.Text, expiryHourTime.Value.ToString(), expiryMinutesTime.Value.ToString(), expirySecondsTime.Value.ToString(), EmergencyModeCheckbox.Checked, MessagePCcheckBox.Checked, MessageEmailcheckBox.Checked, MessagePSExecCheckBox.Checked);
            RefreshRMSGFileList();
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
                        AddItemToListBox("File deleted successfully. - " + selectedFile);
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
                    //Check if the newfilename contains invalid characters.
                    if (newFileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                    {
                        AddItemToListBox("Error renaming file: The new file name contains invalid characters.");
                        return;
                    }
                    //Check if the file already exists. If it does, display a overwrite message.
                    if (File.Exists(Path.Combine(Application.StartupPath, "RMSGFiles", newFileName)))
                    {
                        DialogResult dialogResult = MessageBox.Show("The file already exists. Do you want to overwrite it?", "Overwrite File", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                        if (dialogResult == DialogResult.No)
                        {
                            return;
                        }
                    }
                    try
                    {
                        File.Move(Path.Combine(Application.StartupPath, "RMSGFiles", selectedFile), Path.Combine(Application.StartupPath, "RMSGFiles", newFileName), true);
                        AddItemToListBox("File renamed successfully.");
                        RefreshRMSGFileList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error renaming file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            MessageBox.Show("The RMSG file is a file that contains the message, the PC list and the message duration. It's used to quickly send messages to a list of computers without having to type it out every time. if you save a RMSG file as default.rmsg, it will load it automatically when the program starts.");
        }

        private void RMCManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            //If isScheduledBroadcast is true, close the program without asking.
            if (isScheduledBroadcast)
            {
                return;
            }
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

        private void ComputerSelectList_TextChanged(object sender, EventArgs e)
        {
            //For each line, count it on the PCCountLBL.
            int pcCount = ComputerSelectList.Lines.Length;
            PCCountLbl.Text = "PC Count: " + pcCount.ToString();
        }

        private void GreenButtonTimer_Tick(object sender, EventArgs e)
        {
            //On Tick, Change the color of the start broadcast button to 53, 48, 70 and then disable the timer.
            StartBroadcastBtn.BackColor = Color.FromArgb(53, 48, 70);
            GreenButtonTimer.Stop();
        }

        private void EmergencyModeCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            //Save checkbox state to the settings file.
            Properties.Settings.Default.EmergencyMode = EmergencyModeCheckbox.Checked;
            Properties.Settings.Default.Save();
        }

        private void EmergencyHelpLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //Messagebox with information on what the fast broadcast is. It will not check if the message sent, it will just send it and move on.
            MessageBox.Show("Fast broadcast is a mode that will not check if the message was sent to the computer. It will just attempt to send the message and move on to the next computer. This is useful if you need to send a message to a lot of computers quickly (e.g. in emergencies).");
        }

        private void IconsLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //Create a process 
            Process process = new()
            {
                //Set the startinfo to the process.
                StartInfo = new ProcessStartInfo("https://icons8.com/")
            };
            process.StartInfo.UseShellExecute = true;
            //Start the process.
            process.Start();

        }

        private void DontSaveHistoryLinkHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //Messagebox about broadcast history and what dont save does
            MessageBox.Show("The broadcast history is a log of all the messages that have been sent. If you don't want to save the broadcast history check the don't save history checkbox, it will not save the log of the messages that have been sent.");
        }

        private void DontSaveBroadcastHistoryCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            //Save the checkbox state to the settings file.
            Properties.Settings.Default.DontSaveBroadcastHistory = DontSaveBroadcastHistoryCheckbox.Checked;
            Properties.Settings.Default.Save();
        }

        private void BroadcastToHelpLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Enter the hostname or IP address of the Windows computer you want to send the message to. You can also use the Active Directory button to get a list of computers.");
        }

        private void MessagePCcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //Save the checkbox state to the settings file.
            Properties.Settings.Default.MessagePCEnabled = MessagePCcheckBox.Checked;
        }

        private void MessagePSExecCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //Save the checkbox state to the settings file.
            Properties.Settings.Default.MessagePSExecEnabled = MessagePSExecCheckBox.Checked;
        }

        private void MessageEmailcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //Save the checkbox state to the settings file.
            Properties.Settings.Default.MessageEmailEnabled = MessageEmailcheckBox.Checked;
        }

        private void ToggleRMSGListBtn_Click(object sender, EventArgs e)
        {
            //Set the ModulesTabControl dock style to fill. If it's already fill, set it back to right. Also change icon
            if (ModulesTabControl.Dock == DockStyle.Fill)
            {
                ModulesTabControl.Dock = DockStyle.Right;
                ToggleRMSGListBtn.Image = Properties.Resources.icons8_hide_24;
                ToggleRMSGListBtn.Text = "Hide RMSG List";
            }
            else
            {
                ModulesTabControl.Dock = DockStyle.Fill;
                ToggleRMSGListBtn.Image = Properties.Resources.icons8_expand_24;
                ToggleRMSGListBtn.Text = "Show RMSG List";
            }
        }

        private void ScheduleBroadcastBtn_Click(object sender, EventArgs e)
        {
            ScheduleBroadcastForm scheduleBroadcastForm = new();
            scheduleBroadcastForm.ShowDialog();
        }

        private void BroadcastHistoryBtn_Click(object sender, EventArgs e)
        {
            BroadcastHistoryForm broadcastHistoryForm = new();
            broadcastHistoryForm.Show();
        }

        private void SaveRMCRuntimeLogBtn_Click(object sender, EventArgs e)
        {
            //Save the loglist to a file in the application startup path. With the name of the file being the current date and time and before that "RMC_Runtime_Log_"
            string logFileName = "RMC_Runtime_Log_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".txt";
            try
            {
                File.WriteAllLines(Path.Combine(Application.StartupPath, logFileName), logList.Items.Cast<string>());
                AddItemToListBox("Runtime log saved to file: " + logFileName);
                MessageBox.Show("Runtime log saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving runtime log: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AddItemToListBox("Error saving runtime log: " + ex.Message);
            }
        }
    }
}
