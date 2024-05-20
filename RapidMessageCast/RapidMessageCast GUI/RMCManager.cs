using RapidMessageCast_Manager.Modules;
using System.Diagnostics;
using System.DirectoryServices;
using System.Security.Principal;
using System.Text.RegularExpressions;

//--RapidMessageCast Software--
//RMCManager.cs - RapidMessageCast Manager

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

//RMC Todo list:
//Work on string concatenation for the code. It's not efficient to use the + operator for strings. StringBuilder could work as well.
//Email and PSExec modules are not implemented yet. They are placeholders for future development.
//Add a form to select the OU's from the Active Directory. For now, it will just add all computers from the Computers OU.
//Need to look into how multiple modules can save to the same broadcast history file. It's currently only saving the PC module. Might need to add a bool for each module, showing their status, then saving.
//Filters PCList based on custom user regex pattern.
//WOL added, but testing is needed.
//Panic Button that activates broadcasting immediately with a predefined message.

namespace RapidMessageCast_Manager
{
    public partial class RMCManager : Form
    {
        public string versionNumb = "v0.1";
        private static readonly char[] PCseparatorArray = ['\n', '\r']; //Used for PCList parsing.
        readonly List<string> broadcastHistoryBuffer = []; //Buffer for the broadcast history. This will be saved to a file after the broadcast has finished.
        readonly ImageList tabControlImageList = new(); //Used for the icons on the tabs.
        private bool isScheduledBroadcast = false; //Used for scheduled broadcasts. If true, the program will close after the broadcast has finished.
        readonly PCBroadcastModule pcBroadcastModule = new(); //Create a new instance of the PCBroadcastModule class.
        public RMCManager()
        {
            //Add images to the tabcontrol. Used for the icons on the tabs.
            tabControlImageList.Images.Add("Broadcast", Properties.Resources.icons8_radio_tower_24_black);
            tabControlImageList.Images.Add("PC", Properties.Resources.icons8_pc_24_black);
            tabControlImageList.Images.Add("Message", Properties.Resources.icons8_chat_bubble_24_black);
            tabControlImageList.Images.Add("Email", Properties.Resources.icons8_email_24_black);
            tabControlImageList.Images.Add("WakeOnLAN", Properties.Resources.icons8_turn_on_24_black);
            tabControlImageList.Images.Add("PSEXEC", Properties.Resources.icons8_run_command_24);
            tabControlImageList.Images.Add("Logs", Properties.Resources.icons8_website_bug_24);
            tabControlImageList.Images.Add("About", Properties.Resources.icons8_about_24);
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            AddTextToLogList($"Info - RMC Manager: Starting RMC GUI {versionNumb}. Welcome, {Environment.UserName}");
            CheckCommandLineArguments();
            CheckSystemState();
            LoadGlobalSettings();
            AddIconsToTabControls();
            HandleDefaultRMSGFile();
            UpdateUIWithVersionInformation();
            RefreshRMSGFileList();
            AddTextToLogList("Info - RMCManager: RMC GUI is now ready.");
        }
        
        #region Functions
        //Start of the functions.

        private void CheckCommandLineArguments()
        {
            var args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                AddTextToLogList("Info - RMCManager: Startup command line arguments detected.");

                if (args.Length == 2)
                {
                    AddTextToLogList("Info - RMCManager: Startup loading RMSG file from command line argument.");
                    LoadRMSGFileInProgram(args[1]);
                }
                else if (args.Length == 3 && args[2] == "schedule")
                {
                    AddTextToLogList("Info - RMCManager: Startup loading RMSG file from command line argument and starting scheduled broadcast.");
                    isScheduledBroadcast = true;
                    RunScheduledBroastcast(args[1]);
                }
            }
        }

        private void HandleDefaultRMSGFile()
        {
            var defaultRMSGPath = $"{Application.StartupPath}\\RMSGFiles\\default.rmsg";
            if (File.Exists(defaultRMSGPath))
            {
                LoadRMSGFileInProgram(defaultRMSGPath);
                AddTextToLogList("Info - RMCManager: Default.rmsg file loaded.");
            }
        }

        private void UpdateUIWithVersionInformation()
        {
            versionLbl.Text = versionNumb;
            verNumbLblAboutLbl.Text = $"by lloyd99901 | {versionNumb}";
            Text = $"RapidMessageCast GUI - {versionNumb}";
        }

        private void RunScheduledBroastcast(string RMSGFile)
        {
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
            //BeginPCMessageCast(MessageTxt.Text, ComputerSelectList.Text, totalSeconds, false); //Start the message cast.
            pcBroadcastModule.BroadcastPCMessage(MessageTxt.Text, ComputerSelectList.Text, totalSeconds, false, EmergencyModeCheckbox.Checked, ReattemptOnErrorCheckbox.Checked, DontSaveBroadcastHistoryCheckbox.Checked, isScheduledBroadcast);
        }

        private void CheckSystemState()
        {
            var systemChecker = new SystemCheckModule(AddTextToLogList);
            AddTextToLogList("Info - System Status Check: Checking system state...");
            systemChecker.CheckFileExistence("C:\\Windows\\System32\\msg.exe",
                "Critical - System Status Check: msg.exe not found in the System32 folder. Please ensure that you have a supported operating system edition.",
                "msg.exe not found in the System32 folder. Please ensure that you have a supported operating system edition.");
            systemChecker.CheckAdministratorStatus();
            systemChecker.CheckSystemMemory();
            systemChecker.CheckNetworkAvailability();
            systemChecker.CheckTCPPort(445,
                "Info - System Status Check: TCP Port 445 is open.",
                "Critical - System Status Check: TCP Port 445 is closed. Sending messages may not be possible.",
                "RMC has detected that your computer's TCP port 445 is closed. This port is required for msg broadcasting.");
            systemChecker.CheckPSExecPresence();
            AddTextToLogList("Info - System Status Check: System state check completed.");
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
                ModulesTabControl.TabPages[2].ImageIndex = 5;
                ModulesTabControl.TabPages[3].ImageIndex = 7;
                ModulesTabControl.TabPages[4].ImageIndex = 6;
                //PCTabControl1 as well.
                PCtabControl1.ImageList = tabControlImageList;
                PCtabControl1.TabPages[0].ImageIndex = 2;
                PCtabControl1.TabPages[1].ImageIndex = 0;
                PCtabControl1.TabPages[2].ImageIndex = 4;
            }
            catch (Exception ex)
            {
                AddTextToLogList($"Error - GUI: Failed to add images to the form tabcontrol: {ex}");
            }
        }

        public void AddTextToLogList(string item)
        {
            //Debug States:
            //Info - Used for general information.
            //Error - Used for errors that are not critical.
            //Critical - Used for critical errors that could impact the program or its ability to message pc's.
            //Warning - Used for warnings that are not critical.
            //Notice - Used for notices that are not critical.
            //Then what it was called by, eg GUI, RMC_IO_Manager, etc.
            try //This is a failsafe try catch to prevent the program from crashing if an error occurs.
            {
                if (logList.InvokeRequired)
                {
                    logList.Invoke(new MethodInvoker(() => AddTextToLogList(item))); //This was added to prevent crashing when there are multiple threads trying to access the listbox.
                }
                else
                {
                    //add item to loglist with Date Time, Event Type and the item.
                    logList.Items.Add($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {item}");
                    logList.TopIndex = logList.Items.Count - 1; // Scroll to the latest item
                }
            }
            catch (Exception ex)
            {
                //This exception has to be caught, otherwise the program will crash if an error occurs in the loglist.
                Console.WriteLine($"LogWriter Error! An exception occurred when adding an item to the loglist: {ex}");
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
            MagicPortNumberBox.Value = Properties.Settings.Default.MagicPortNumber;
            ReattemptOnErrorCheckbox.Checked = Properties.Settings.Default.ReattemptOnError;
            AddTextToLogList($"Info - RMC Settings: Program settings loaded. Number of settings loaded: {Properties.Settings.Default.Properties.Count}");
        }

        private static string FilterInvalidCharacters(string text)
        {
            // Regular expression to match characters that are not allowed in NetBIOS or Windows hostnames
            string pattern = @"[^\p{L}\p{N}\-\._\n\r]";

            // Replace invalid characters with empty string
            return Regex.Replace(text, pattern, "");
        }
        private static void SetCheckboxState(CheckBox checkBox, string value)
        {
            checkBox.Checked = value == "True";
        }
        private void LoadRMSGFileInProgram(string filePath)
        {
            //Use RMSG_IO_Manager.LoadRMSGFile(openFileDialog.FileName); and store the return values in a string array.
            string[] RMSGFileValues = RMC_IO_Manager.LoadRMSGFile(filePath);

            //Check if the first value in the array is "Error". If it is, report it to the user and add to loglist
            if (RMSGFileValues[0] != "") //If there is something in the first value, it means that there is an error. Break and report it to the user.
            {
                MessageBox.Show("Error loading message: " + RMSGFileValues[0], "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AddTextToLogList("Error - LoadRMSGFileInProgram: loading message returned an error: " + RMSGFileValues[0]);
            }
            else
            {
                //Populate the textboxes with the values from the array.
                MessageTxt.Text = RMSGFileValues[1];
                ComputerSelectList.Text = RMSGFileValues[2];
                expiryHourTime.Value = Convert.ToDecimal(RMSGFileValues[3]);
                expiryMinutesTime.Value = Convert.ToDecimal(RMSGFileValues[4]);
                expirySecondsTime.Value = Convert.ToDecimal(RMSGFileValues[5]);
                SetCheckboxState(EmergencyModeCheckbox, RMSGFileValues[6]);
                SetCheckboxState(MessagePCcheckBox, RMSGFileValues[7]);
                SetCheckboxState(MessageEmailcheckBox, RMSGFileValues[8]);
                SetCheckboxState(MessagePSExecCheckBox, RMSGFileValues[9]);
                SetCheckboxState(ReattemptOnErrorCheckbox, RMSGFileValues[10]);
                SetCheckboxState(DontSaveBroadcastHistoryCheckbox, RMSGFileValues[11]);
                //Add to loglist with the name of file that was loaded.
                AddTextToLogList("Info - LoadRMSGFileInProgram: RMSG File loaded successfully: " + Path.GetFileName(filePath));
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
                AddTextToLogList("Info - RefreshRMSGFileList: RMSG list refreshed. Amount of files on the list: " + files.Length);
            }
            catch (Exception ex)
            {
                AddTextToLogList("Error - RefreshRMSGFileList: Failure in creating QuickLoad folder(s)/Refreshing the RMSGFileList " + ex.ToString());
            }
        }

        //End of the functions, start of the events.
        #endregion Functions
        private void ActiveDirectorySelectBtn_Click(object sender, EventArgs e)
        {
            //If possible, I will create a form to select the OU's from the Active Directory. For now, it will just add all computers from the Computers OU.
            //--Connect to the Active Directory, show a form window with all the available OU's, let the user select one or all. Then add all the computers from the OU that the user selected to the pclist.--
            AddTextToLogList("Info - Active Directory: Attempting to connect to Active Directory.");
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
                AddTextToLogList("Info - Computers from Active Directory added to the list.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to Active Directory: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AddTextToLogList("Error - Active Directory: Failure in connecting to Active Directory. " + ex.ToString());
            }

        }

        private void MessageTxt_TextChanged(object sender, EventArgs e)
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

        private void MessageTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            //invalidcharspattern is a regex pattern that will check if the key pressed is invalid. If it is, it will suppress the key press. BAD CHARACTERS: /
            string invalidCharsPattern = @"[\/]";
            if (Regex.IsMatch(e.KeyChar.ToString(), invalidCharsPattern))
            {
                // Suppress the invalid key press
                e.Handled = true;
            }
        }

        private void OpenMessageTextToolStripMenuItem_Click(object sender, EventArgs e)
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
                    AddTextToLogList("Info - Message text loaded from file: " + Path.GetFileName(filePath));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error reading the file: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AddTextToLogList("Error - OpenMessage: Failure in reading the message txt file: " + ex.ToString());
                }
            }
        }

        private void OpenSendComputerListToolStripMenuItem_Click(object sender, EventArgs e)
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
                    AddTextToLogList("Info - PC list loaded from file: " + Path.GetFileName(filePath));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error - Error reading the PC list file: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AddTextToLogList("Error - OpenPCList: Failure in reading the PC list file: " + ex.ToString());
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
                    AddTextToLogList("Info - PC list saved successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error - Error saving PC list: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AddTextToLogList("Error - SavePCList: Failure in saving PC list: " + ex.ToString());
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
                    AddTextToLogList("Info - SaveMessageBtn: Message text saved successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving message text: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AddTextToLogList("Error - SaveMessageBtn: Failure in saving message text: " + ex.ToString());
                }
            }
        }

        private void StartBroadcastBtn_Click(object sender, EventArgs e)
        {
            AddTextToLogList("Info - StartBroadcast: Broadcast triggered, checking what modules are turned on, then will start the modules.");
            //If nothing is enabled, display a message to the user.
            if (!MessagePCcheckBox.Checked && !MessageEmailcheckBox.Checked && !MessagePSExecCheckBox.Checked)
            {
                AddTextToLogList("Error - StartBroadcast: No modules are enabled. Unable to broadcast.");
                MessageBox.Show("No modules are enabled. Please enable at least one module before starting the broadcast.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Clear broadcast history buffer.
            broadcastHistoryBuffer.Clear();

            if (MessagePCcheckBox.Checked)
            {
                AddTextToLogList("Info - StartBroadcast: Message module is enabled. Starting message cast.");

                // Check if message or PC list is empty
                if (string.IsNullOrWhiteSpace(MessageTxt.Text) || string.IsNullOrWhiteSpace(ComputerSelectList.Text))
                {
                    MessageBox.Show("Message or PC list is empty. Please fill in the message and PC list before starting the broadcast.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AddTextToLogList("Error - StartBroadcast: Message or PC list is empty. Broadcast halted.");
                    return;
                }

                int totalSeconds = ((int)expiryHourTime.Value * 3600) + ((int)expiryMinutesTime.Value * 60) + (int)expirySecondsTime.Value;
                pcBroadcastModule.BroadcastPCMessage(MessageTxt.Text, ComputerSelectList.Text, totalSeconds, false, EmergencyModeCheckbox.Checked, ReattemptOnErrorCheckbox.Checked, DontSaveBroadcastHistoryCheckbox.Checked, isScheduledBroadcast);
            }
            //Check if Email module is enabled. If it is, start the Email cast.
            if (MessageEmailcheckBox.Checked)
            {
                AddTextToLogList("Info - StartBroadcast: Email module is enabled. Starting email cast.");
                //Start the Email cast.
                //send test messagebox to the user.
                MessageBox.Show("Email module is not implemented yet. This is a placeholder message.", "Email Module", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //Check if PSExec module is enabled. If it is, start the PSExec cast.
            if (MessagePSExecCheckBox.Checked)
            {
                AddTextToLogList("Info - StartBroadcast: PSExec module is enabled. Starting PSExec cast.");
                //Start the PSExec cast.
                //send test messagebox to the user.
                MessageBox.Show("PSExec module is not implemented yet. This is a placeholder message.", "PSExec Module", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ClearLogBtn_Click(object sender, EventArgs e)
        {
            //ask the user if they are sure they want to clear the loglist. If yes, clear it.
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to clear the log list?", "Clear Log List", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                logList.Items.Clear();
                AddTextToLogList("Info - ClearLogBtn: Loglist cleared.");
            }
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
                AddTextToLogList("Info - SaveRMSGBtn: Saving RMSG file: " + saveFileDialog.FileName);
                RMC_IO_Manager.SaveRMSGFile(saveFileDialog.FileName, MessageTxt.Text, ComputerSelectList.Text, expiryHourTime.Value.ToString(), expiryMinutesTime.Value.ToString(), expirySecondsTime.Value.ToString(), EmergencyModeCheckbox.Checked, MessagePCcheckBox.Checked, MessageEmailcheckBox.Checked, MessagePSExecCheckBox.Checked, ReattemptOnErrorCheckbox.Checked, DontSaveBroadcastHistoryCheckbox.Checked);
                RefreshRMSGFileList();
            }
        }

        private void QuickSaveRMSGBtn_Clicked(object sender, EventArgs e)
        {
            //Save a quick save file name based on the current date and time.
            string quickSaveFileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".rmsg";

            AddTextToLogList("Info - QuickSaveBtn: Quick saving RMSG file: " + quickSaveFileName);
            //Use the SaveRMSGFile in the RMC_IO_Manager to save the file.
            RMC_IO_Manager.SaveRMSGFile(Path.Combine(Application.StartupPath, "RMSGFiles", quickSaveFileName), MessageTxt.Text, ComputerSelectList.Text, expiryHourTime.Value.ToString(), expiryMinutesTime.Value.ToString(), expirySecondsTime.Value.ToString(), EmergencyModeCheckbox.Checked, MessagePCcheckBox.Checked, MessageEmailcheckBox.Checked, MessagePSExecCheckBox.Checked, ReattemptOnErrorCheckbox.Checked, DontSaveBroadcastHistoryCheckbox.Checked);
            RefreshRMSGFileList();
        }

        private void RefreshRMSGListBtn_Click(object sender, EventArgs e)
        {
            AddTextToLogList("Info - RefreshRMSGBtn: Refreshing RMSG file list.");
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
                        AddTextToLogList("Info - DeleteSelectedRMSGFile: File deleted: " + selectedFile);
                        RefreshRMSGFileList();
                    }
                }
                catch (Exception ex)
                {
                    AddTextToLogList("Error - DeleteSelectedRMSGFile: Failure in deleting file: " + ex.ToString());
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
            // Get the selected file from the list box
            string? selectedFile = RMSGFileListBox.SelectedItem?.ToString();
            if (selectedFile != null)
            {
                // Prompt the user for a new file name
                string newFileName = Microsoft.VisualBasic.Interaction.InputBox("Enter the new name for the file. (the .rmsg at the end will be added if it's missing)", "Rename File", selectedFile);
                if (string.IsNullOrWhiteSpace(newFileName))
                    return;

                // Add .rmsg extension if missing
                if (!newFileName.EndsWith(".rmsg"))
                    newFileName += ".rmsg";

                // Check for invalid characters in the file name
                if (newFileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                {
                    AddTextToLogList("Error - RenameSelectedRMSGBtn: Error renaming file, invalid characters in the filename.");
                    return;
                }

                // Check if the file already exists and prompt for overwrite
                string filePath = Path.Combine(Application.StartupPath, "RMSGFiles", newFileName);
                if (File.Exists(filePath))
                {
                    DialogResult dialogResult = MessageBox.Show("The file already exists. Do you want to overwrite it?", "Overwrite File", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    if (dialogResult == DialogResult.No)
                        return;
                }

                try
                {
                    // Move the file to the new location
                    File.Move(Path.Combine(Application.StartupPath, "RMSGFiles", selectedFile), filePath, true);
                    AddTextToLogList($"Info - RenameSelectedRMSGBtn: File renamed: {selectedFile} to {newFileName}");
                    RefreshRMSGFileList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error renaming file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AddTextToLogList($"Error - RenameSelectedRMSGBtn: Failure in renaming file: {ex.Message}");
                }
            }
        }

        private void MessageDurationHelpLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //Open up a messagebox with Information on what the message duration is. (all 0 is never expires/closes)
            MessageBox.Show("The message duration is how long the message will be displayed on the screen for the user. The format is in HH:MM:SS. If it's set to all 0's, the message will never close until the user closes it.");
        }

        private void RMSGHelpLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //Open up a messagebox with Information on what the RMSG file is.
            MessageBox.Show("The RMSG file is a file that contains the message, the PC list and the message duration. It's used to quickly send messages to a list of computers without having to type it out every time. if you save a RMSG file as default.rmsg, it will load it automatically when the program starts.");
        }

        private void RMCManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            AddTextToLogList("Info - RMCManager: Program close has been triggered. Attempting to save log file and then attempt closure.");
            //Attempt Save the rmc runtime logfile by clicking the save log button.
            SaveRMCRuntimeLogBtn_Click(sender, e);
            //If isScheduledBroadcast is true, close the program without asking.
            if (isScheduledBroadcast)
            {
                AddTextToLogList("Scheduled broadcast is running. Prohibiting program from closing.");
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
            //Messagebox with Information on what the fast broadcast is. It will not check if the message sent, it will just send it and move on.
            MessageBox.Show("Fast broadcast is a mode that will not check if the message was sent to the computer. It will just attempt to send the message and move on to the next computer. This is useful if you need to send a message to a lot of computers quickly (e.g. in emergencies).");
        }

        private void IconsLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //Create a process 
            Process process = new()
            {
                //Set the startInfo to the process.
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
            AddTextToLogList("Info - ScheduleBroadcast: Opening the schedule broadcast form.");
            ScheduleBroadcastForm scheduleBroadcastForm = new();
            scheduleBroadcastForm.ShowDialog();
        }

        private void BroadcastHistoryBtn_Click(object sender, EventArgs e)
        {
            AddTextToLogList("Info - BroadcastHistory: Opening the broadcast history form.");
            BroadcastHistoryForm broadcastHistoryForm = new();
            broadcastHistoryForm.Show();
        }

        private void SaveRMCRuntimeLogBtn_Click(object sender, EventArgs e)
        {
            //Save the loglist to a file in the application startup path / the folder called RMCRuntimeLogFiles. With the name of the file being the current date and time and before that "RMC_Runtime_Log_"
            string logFileName = "RMC_Runtime_Log_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".txt";
            try
            {
                File.WriteAllLines(Application.StartupPath + "\\RMC Runtime Logs\\" + logFileName, logList.Items.Cast<string>().ToArray());
                AddTextToLogList("Runtime log saved to file: " + logFileName);
            }
            catch (Exception ex)
            {
                AddTextToLogList("Error - RuntimeSaveLog: Failure in saving runtime log. " + ex.ToString());
                MessageBox.Show("Error saving runtime log: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void AddThisPCToTheListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Get the current pc hostname and add it to the pclist.
            ComputerSelectList.Text += Environment.MachineName + "\r\n";
        }

        private void ClearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ComputerSelectList.Clear();
        }

        private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ComputerSelectList.SelectAll();
        }

        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ComputerSelectList.Undo();
        }

        private void RMCManager_Shown(object sender, EventArgs e)
        {
            //check if scheduled broadcast is true. If it is, hide the form.
            if (isScheduledBroadcast)
            {
                Hide();
            }
        }

        private void TestBroadcastMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Broadcast the message to just the computer that the program is running on.
            if (MessageTxt.Text == "" || ComputerSelectList.Text == "")
            {
                MessageBox.Show("Message or PC list is empty. Please fill in the message and PC list before starting the broadcast.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AddTextToLogList("Error - StartBroadcast: Message or PC list is empty. Broadcast halted.");
                return;
            }
            int totalSeconds = ((int)expiryHourTime.Value * 3600) + ((int)expiryMinutesTime.Value * 60) + (int)expirySecondsTime.Value; //Calculate the total seconds from the hours, minutes and seconds for the message duration.
            //BeginPCMessageCast(MessageTxt.Text, Environment.MachineName, totalSeconds, false);
            pcBroadcastModule.BroadcastPCMessage(MessageTxt.Text, Environment.MachineName, totalSeconds, false, EmergencyModeCheckbox.Checked, ReattemptOnErrorCheckbox.Checked, DontSaveBroadcastHistoryCheckbox.Checked, isScheduledBroadcast);
        }

        private async void SendWOLPacketBtn_Click(object sender, EventArgs e)
        {
            AddTextToLogList("Info - SendWOLPacket: Sending WOL packet to the PC's in the list.");
            //Send a WOL packet to all mac addresses in the WOLTextbox. But first check if the textbox is empty or contains invalid mac addresses.
            string[] macAddresses = WOLTextbox.Text.Split(PCseparatorArray, StringSplitOptions.RemoveEmptyEntries);
            if (macAddresses.Length == 0)
            {
                MessageBox.Show("No MAC addresses in the textbox. Please add MAC addresses before sending a WOL packet.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AddTextToLogList("Error - SendWOLPacket: No MAC addresses in the textbox. Please add MAC addresses before sending a WOL packet.");
                return;
            }
            foreach (string macAddress in macAddresses)
            {
                if (!WakeOnLANModule.IsValidMacAddress(macAddress))
                {
                    //MessageBox.Show("Invalid MAC address: " + macAddress, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AddTextToLogList("Error - SendWOLPacket: Invalid MAC address: " + macAddress);
                    return;
                }
                AddTextToLogList("Info - SendWOLPacket: Sending WOL packet to MAC address: " + macAddress);
                await
                WakeOnLANModule.WakeOnLan(macAddress, (int)MagicPortNumberBox.Value);
            }
        }

        private void OpenMacAddressfromTxtBtn_Click(object sender, EventArgs e)
        {
            //Open a file dialog to select a txt file, then load that into the wol textbox.
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
                    WOLTextbox.Text = filteredContents;
                    AddTextToLogList("Info - MacAddressfromTxt: Mac addresses loaded from file: " + Path.GetFileName(filePath));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error - Error reading the MAC address file: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AddTextToLogList("Error - OpenMacAddressfromTxt: Failure in reading the MAC address file: " + ex.ToString());
                }
            }
        }

        private void SaveMacAddressesAsTXTBtn_Click(object sender, EventArgs e)
        {
            //Save the mac addresses in the wol textbox to a txt file.
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
                    File.WriteAllText(filePath, WOLTextbox.Text);

                    AddTextToLogList("Info - SaveMacAddressesAsTXTBtn: Mac addresses saved successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving mac addresses: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AddTextToLogList("Error - SaveMacAddressesAsTXTBtn: Failure in saving mac addresses: " + ex.ToString());
                }
            }
        }

        private void MagicPortNumberBox_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.MagicPortNumber = MagicPortNumberBox.Value;
        }

        private void ReattemptonErrorHelpLbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("If the program fails to send a message to a PC, it will reattempt to send the message to the PC. This will only happen once per PC. If it fails again, it will not reattempt to send the message.");
        }

        private void ReattemptOnErrorCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            //Save
            Properties.Settings.Default.ReattemptOnError = ReattemptOnErrorCheckbox.Checked;
            Properties.Settings.Default.Save();
        }
    }
}
