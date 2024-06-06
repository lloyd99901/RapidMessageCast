using RapidMessageCast_Manager.BroadcastModules;
using RapidMessageCast_Manager.Internal_RMC_Components;
using System.Diagnostics;
using System.DirectoryServices;
using System.Text.RegularExpressions;

//--RapidMessageCast Software--
//RMCManagerForm.cs - RapidMessageCast Manager

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
//Email and PSExec modules are not implemented yet. They are placeholders for future development.
//Add a form to select the OU's from the Active Directory. For now, it will just add all computers from the Computers OU.
//Filters PCList based on custom user regex pattern.
//WOL added, but testing is needed.
//Panic Button that activates broadcasting immediately with a predefined message. (AutoHotKey script would work here)
//Test broadcasting with unhandled exceptions, and see if the program can recover from it. If it pauses broadcasting, add a try catch to that function to prevent it from pausing. This can be done closer to completion.
//It might be an idea to disable all msgbox popups during startup, also check if there are msgboxes during the broadcast. Remove those.
//Can the private readonly Action<string> _logAction = logAction; be put on all external classes instead of using the RMCForm every time to call the AddTextToLogList function? This will make the code cleaner and may also prevent instabilities. Investigate this.
//Add a function to check if the message is too long for the msg command. If it is, split the message into multiple messages. (This is a low priority task, but might be a good idea in the future)
//Add icons (like x or tick boxes) to the broadcast History list.

namespace RapidMessageCast_Manager
{
    public partial class RMCManager : Form
    {
        public string versionNumb = "v0.1";
        private static readonly char[] PCseparatorArray = ['\n', '\r']; //Used for PCList parsing.
        readonly ImageList tabControlImageList = new(); //Used for the icons on the tabs.
        private bool isScheduledBroadcast = false; //Used for scheduled broadcasts. If true, the program will close after the broadcast has finished.
        readonly BroadcastController broadcastController = new(); //Create a new instance of the BroadcastController class.
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
            AddTextToLogList($"Info - [RMC Startup]: Starting RMC GUI {versionNumb}. Welcome, {Environment.UserName}");
            CheckCommandLineArguments();
            LoadGlobalSettings();
            CheckSystemState();
            AddIconsToTabControls();
            HandleDefaultRMSGFile();
            UpdateUIWithVersionInformation();
            RefreshRMSGFileList();
            InitalizeToolTipHelp();
            RMC_IO_Manager.AttemptToCreateRMCDirectories();
            AddTextToLogList("Info - [RMC Startup]: RMC GUI is now ready.");
        }


        #region Functions
        //Start of the functions.
        private void InitalizeToolTipHelp()
        {
            try
            {
                Control[] checkboxes = [EmergencyModeCheckbox, MessagePCcheckBox, MessageEmailcheckBox, MessagePSExecCheckBox, ReattemptOnErrorCheckbox, DontSaveBroadcastHistoryCheckbox];
                Control[] buttons = [StartBroadcastBtn, clearLogBtn, SaveRMCRuntimeLogBtn, SaveRMCFileBTN, QuickSaveRMSGBtn, RefreshRMSGListBtn, DeleteSelectedRMSGFileBtn, RenameSelectedRMSGBtn, LoadSelectedRMSGBtn, SavePCListTxtBtn, SaveMessageTxtBtn, OpenRMCFileBtn, ActiveDirectorySelectBtn, MessageOpenTxtBtn, ComputerListLoadFromFileBtn];
                string[] checkboxToolTips = ["Enable emergency mode. This will send the message without checking if it was sent.", "Enable the PC message module.", "Enable the Email message module.", "Enable the PSExec message module.", "Reattempt to send the message if an error occurs.", "Don't save the broadcast history after RMC completes a broadcast."];
                string[] buttonToolTips = ["Start the broadcast.", "Clear the log list.", "Save the log list to a file.", "Save the RMC file.", "Quick save the RMC file.", "Refresh the RMC file list.", "Delete the selected RMC file.", "Rename the selected RMC file.", "Load the selected RMC file.", "Save the PC list to a file.", "Save the message text to a file.", "Open a RMC file.", "Select computers from Active Directory.", "Open a message text file.", "Open a computer list file."];

                for (int i = 0; i < checkboxes.Length; i++)
                {
                    ToolTip toolTip = new();
                    toolTip.SetToolTip(checkboxes[i], checkboxToolTips[i]);
                }
                for (int i = 0; i < buttons.Length; i++)
                {
                    ToolTip toolTip = new();
                    toolTip.SetToolTip(buttons[i], buttonToolTips[i]);
                }
            }
            catch (Exception ex)
            {
                AddTextToLogList($"Error - [ToolTipHelp]: Failed to initalize the tooltip help, this is not a critical error: {ex}"); //This error isn't critical, so it can be ignored tbh.
            }
        }

        private void CheckCommandLineArguments()
        {
            var args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                AddTextToLogList("Info - [RMC Manager]: Startup command line arguments detected. Checking for RMSG file.");

                if (args.Length == 2)
                {
                    AddTextToLogList("Info - [RMC Manager]: Loading RMSG file from command line argument.");
                    LoadRMSGFileInProgram(args[1]);
                }
                else if (args.Length == 3 && args[2] == "schedule")
                {
                    AddTextToLogList("Info - [RMC Manager]: [SCHEDULE] Startup loading RMSG file from command line argument and starting scheduled broadcast.");
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
                AddTextToLogList("Info - [RMC Manager]: Default.rmsg file loaded.");
            }
        }

        private void UpdateUIWithVersionInformation()
        {
            versionLbl.Text = versionNumb;
            verNumbLblAboutLbl.Text = $"by lloyd99901 | {versionNumb}";
            Text = $"RapidMessageCast - {versionNumb}";
        }

        private async void RunScheduledBroastcast(string RMSGFile)
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
            await
            broadcastController.StartBroadcastModule(RMCEnums.PC, MessageTxt.Text, ComputerSelectList.Text, totalSeconds, EmergencyModeCheckbox.Checked, ReattemptOnErrorCheckbox.Checked, DontSaveBroadcastHistoryCheckbox.Checked, isScheduledBroadcast);
        }

        private void CheckSystemState()
        {
            var systemChecker = new SystemCheckModule(AddTextToLogList);
            AddTextToLogList("Info - [CheckSystemState]: Checking system state...");
            systemChecker.CheckFileExistence("C:\\Windows\\System32\\msg.exe",
                "Critical - [CheckSystemState]: msg.exe not found in the System32 folder. Please ensure that you have a supported operating system edition.",
                "msg.exe not found in the System32 folder. Please ensure that you have a supported operating system edition.");
            systemChecker.CheckAdministratorStatus();
            systemChecker.CheckSystemMemory();
            systemChecker.CheckNetworkAvailability();
            systemChecker.CheckTCPPort(445,
                "Info - [CheckSystemState]: TCP Port 445 is open.",
                "Critical - [CheckSystemState]: TCP Port 445 is closed. Sending messages may not be possible.",
                "RMC has detected that your computer's TCP port 445 is closed. This port is required for msg broadcasting.");
            if (!systemChecker.CheckPSExecPresence())
            {
                //Disable checkbox and set text to PSExec (Disabled)
                MessagePSExecCheckBox.Enabled = false;
                PsExecLabel.Text = "PsExec (Disabled)";
            }
            AddTextToLogList("Info - [CheckSystemState]: System state check completed.");
            //Check for RMC program updates
            AddTextToLogList("Info - [RMCUpdate]: Checking for RMC program updates...");
            if (RMCUpdateChecker.CheckForUpdates())
            {
                AddTextToLogList("Info - [RMCUpdate]: An update is available. Please download the latest version from the GitHub repository to ensure stability.");
                MessageBox.Show("An update is available. Please download the latest version from the GitHub repository to ensure stability.", "Update Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                AddTextToLogList("Info - [RMCUpdate]: No RMC updates available or no connection is detected.");
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
                AddTextToLogList($"Error - [RMC GUI]: Failed to add images to the form tabcontrol: {ex}");
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
            //Then what class it was called by, eg GUI, RMC_IO_Manager, etc.
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
                try
                {
                    //This exception has to be caught, otherwise the program will crash if an error occurs in the loglist.
                    Console.WriteLine($"Critical - [LogWriter] Error! An exception occurred when adding an item to the loglist: {ex}");
                }
                catch
                {
                    //If the program throws an error here, then something went disastrously wrong. This will prevent the program from crashing, but at this point if we can't even write to the console then we are in trouble.
                }
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
            AddTextToLogList($"Info - [LoadGlobalSettings]: {Properties.Settings.Default.Properties.Count} Program settings loaded.");
        }
        private static void SetCheckboxState(CheckBox checkBox, string value)
        {
            checkBox.Checked = value == "True";
        }
        private void LoadRMSGFileInProgram(string filePath)
        {
            AddTextToLogList($"Info - [LoadRMSGFileInProgram]: Loading RMSG file: {Path.GetFileName(filePath)}");
            string[] RMSGFileValues = RMC_IO_Manager.LoadRMSGFile(filePath);

            if (!string.IsNullOrEmpty(RMSGFileValues[0]))
            {
                MessageBox.Show($"Error loading message: {RMSGFileValues[0]}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AddTextToLogList($"Error - [LoadRMSGFileInProgram]: loading message returned an error: {RMSGFileValues[0]}");
                return;
            }
            AddTextToLogList($"Info - [LoadRMSGFileInProgram]: Parsing RMSG file: {Path.GetFileName(filePath)}");
            try
            {
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
                WOLTextbox.Text = RMSGFileValues[12];
                MagicPortNumberBox.Value = Convert.ToDecimal(RMSGFileValues[13]);
                AddTextToLogList($"Info - [LoadRMSGFileInProgram]: RMSG File loaded successfully: {Path.GetFileName(filePath)}");
            }
            catch(Exception ex)
            {
                AddTextToLogList($"Error - [LoadRMSGFileInProgram]: Attempt to parse RMSG file failed: {ex}");
                return;
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
                AddTextToLogList($"Info - [RefreshRMSGFileList]: RMSG list refreshed. Amount of files on the list: {files.Length}");
            }
            catch (Exception ex)
            {
                AddTextToLogList($"Error - [RefreshRMSGFileList]: Failure in creating QuickLoad folder(s)/Refreshing the RMSGFileList {ex}");
            }
        }

        //End of the functions, start of the events.
        #endregion Functions
        private void ActiveDirectorySelectBtn_Click(object sender, EventArgs e)
        {
            //If possible, I will create a form to select the OU's from the Active Directory. For now, it will just add all computers from the Computers OU.
            //--Connect to the Active Directory, show a form window with all the available OU's, let the user select one or all. Then add all the computers from the OU that the user selected to the pclist.--
            AddTextToLogList("Info - [Active Directory]: Attempting to connect to Active Directory.");
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
                AddTextToLogList("Info - [Active Directory] Computers from Active Directory added to the list.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error connecting to Active Directory: {ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AddTextToLogList($"Error - [Active Directory]: Failure in connecting to Active Directory. {ex}");
            }

        }

        private void MessageTxt_TextChanged(object sender, EventArgs e)
        {
            int remainingCharacters = 255 - MessageTxt.TextLength;
            if (remainingCharacters >= 0)
            {
                MessageLimitLbl.Text = $"Length Remaining: {remainingCharacters}";
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
        private void OpenRMCFileBtn_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new()
            {
                InitialDirectory = Application.StartupPath,
                Filter = "Rapid Message Files (*.rmsg)|*.rmsg|All Files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadRMSGFileInProgram(openFileDialog.FileName);
            }
        }
        private void SaveRMCFileBTN_Click(object sender, EventArgs e)
        {
            using SaveFileDialog saveFileDialog = new()
            {
                InitialDirectory = Application.StartupPath,
                Filter = "Rapid Message Files (*.rmsg)|*.rmsg|All Files (*.*)|*.*"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog.FileName;
                AddTextToLogList($"Info - [SaveRMSGBtn]: Saving RMSG file: {fileName}");
                RMC_IO_Manager.SaveRMSGFile(fileName, MessageTxt.Text, ComputerSelectList.Text, WOLTextbox.Text, (int)MagicPortNumberBox.Value, expiryHourTime.Value.ToString(), expiryMinutesTime.Value.ToString(), expirySecondsTime.Value.ToString(), EmergencyModeCheckbox.Checked, MessagePCcheckBox.Checked, MessageEmailcheckBox.Checked, MessagePSExecCheckBox.Checked, ReattemptOnErrorCheckbox.Checked, DontSaveBroadcastHistoryCheckbox.Checked);
                RefreshRMSGFileList();
            }
        }

        private void OpenMessageTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new RMC_IO_Manager(AddTextToLogList).OpenFileAndProcessContents(MessageTxt, "OpenMessageAsTxt", "OpenMessageAsTxt", InternalRegexFilters.FilterInvalidMessage);
        }

        private void OpenSendComputerListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new RMC_IO_Manager(AddTextToLogList).OpenFileAndProcessContents(ComputerSelectList, "OpenPCList", "OpenPCList", InternalRegexFilters.FilterInvalidPCNames);
        }

        private void OpenMacAddressfromTxtBtn_Click(object sender, EventArgs e)
        {
            new RMC_IO_Manager(AddTextToLogList).OpenFileAndProcessContents(WOLTextbox, "MacAddressfromTxt", "OpenMacAddressfromTxt", InternalRegexFilters.FilterInvalidPCNames);
        }

        private void SaveMacAddressesAsTXTBtn_Click(object sender, EventArgs e)
        {
            new RMC_IO_Manager(AddTextToLogList).SaveFileFromTextBox(WOLTextbox, "SaveMacAddressesAsTXTBtn", "SaveMacAddressesAsTXTBtn");
        }

        private void SaveComputerListBtn_Click(object sender, EventArgs e)
        {
            new RMC_IO_Manager(AddTextToLogList).SaveFileFromTextBox(ComputerSelectList, "SavePCList", "SavePCList");
        }

        private void SaveMessageBtn_Click(object sender, EventArgs e)
        {
            new RMC_IO_Manager(AddTextToLogList).SaveFileFromTextBox(MessageTxt, "SaveMessageBtn", "SaveMessageBtn");
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

        private async void StartBroadcastBtn_Click(object sender, EventArgs e)
        {
            AddTextToLogList("Info - [InitBroadcast]: Broadcast button pressed. Running selected modules...");

            bool isMessagePCChecked = MessagePCcheckBox.Checked;
            bool isMessageEmailChecked = MessageEmailcheckBox.Checked;
            bool isMessagePSExecChecked = MessagePSExecCheckBox.Checked;

            if (!isMessagePCChecked && !isMessageEmailChecked && !isMessagePSExecChecked)
            {
                AddTextToLogList("Error - [InitBroadcast]: No modules are enabled. Unable to broadcast.");
                MessageBox.Show("No modules are enabled. Please enable at least one module before starting the broadcast.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (isMessagePCChecked)
            {
                AddTextToLogList("Info - [InitBroadcast]: PC message module is enabled. Starting PC message cast...");

                string messageText = MessageTxt.Text;
                string computerSelectListText = ComputerSelectList.Text;

                if (string.IsNullOrWhiteSpace(messageText) || string.IsNullOrWhiteSpace(computerSelectListText))
                {
                    MessageBox.Show("Message or PC list is empty. Please fill in the message and PC list before starting the broadcast.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AddTextToLogList("Error - [InitBroadcast]: PC Broadcast - Message textbox or PC list are empty. Broadcast halted.");
                    return;
                }

                int totalSeconds = ((int)expiryHourTime.Value * 3600) + ((int)expiryMinutesTime.Value * 60) + (int)expirySecondsTime.Value;
                await broadcastController.StartBroadcastModule(RMCEnums.PC, messageText, computerSelectListText, totalSeconds, EmergencyModeCheckbox.Checked, ReattemptOnErrorCheckbox.Checked, DontSaveBroadcastHistoryCheckbox.Checked, isScheduledBroadcast);
            }

            if (isMessageEmailChecked)
            {
                AddTextToLogList("Info - [InitBroadcast]: Email module is enabled. Starting email cast.");
                MessageBox.Show("Email module is not implemented yet. This is a placeholder message.", "Email Module", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (isMessagePSExecChecked)
            {
                AddTextToLogList("Info - [InitBroadcast]: PSExec module is enabled. Starting PSExec cast.");
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
                AddTextToLogList("Info - [RMC Manager]: Loglist cleared.");
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

        private void QuickSaveRMSGBtn_Clicked(object sender, EventArgs e)
        {
            //Save a quick save file name based on the current date and time.
            string quickSaveFileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".rmsg";

            AddTextToLogList($"Info - [QuickSaveBtn]: Quick saving RMSG file: {quickSaveFileName}");
            //Use the SaveRMSGFile in the RMC_IO_Manager to save the file.
            RMC_IO_Manager.SaveRMSGFile(Path.Combine(Application.StartupPath, "RMSGFiles", quickSaveFileName), MessageTxt.Text, ComputerSelectList.Text, WOLTextbox.Text, (int)MagicPortNumberBox.Value, expiryHourTime.Value.ToString(), expiryMinutesTime.Value.ToString(), expirySecondsTime.Value.ToString(), EmergencyModeCheckbox.Checked, MessagePCcheckBox.Checked, MessageEmailcheckBox.Checked, MessagePSExecCheckBox.Checked, ReattemptOnErrorCheckbox.Checked, DontSaveBroadcastHistoryCheckbox.Checked);
            RefreshRMSGFileList();
        }

        private void RefreshRMSGListBtn_Click(object sender, EventArgs e)
        {
            AddTextToLogList("Info - [RefreshRMSGBtn]: Refreshing RMSG file list.");
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
                    DialogResult dialogResult = MessageBox.Show($"Are you sure you want to delete the file: {selectedFile}?", "Delete File", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        File.Delete(Path.Combine(Application.StartupPath, "RMSGFiles", selectedFile));
                        AddTextToLogList("Info - [DeleteSelectedRMSGFile]: File deleted: " + selectedFile);
                        RefreshRMSGFileList();
                    }
                }
                catch (Exception ex)
                {
                    AddTextToLogList($"Error - [DeleteSelectedRMSGFile]: Failure in deleting file: {ex}");
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
                    AddTextToLogList("Error - [RenameSelectedRMSGBtn]: Error renaming file, invalid characters in the filename.");
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
                    AddTextToLogList($"Info - [RenameSelectedRMSGBtn]: File renamed: {selectedFile} to {newFileName}");
                    RefreshRMSGFileList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error renaming file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AddTextToLogList($"Error - [RenameSelectedRMSGBtn]: Failure in renaming file: {ex.Message}");
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
            AddTextToLogList("Info - [RMC Manager]: Program close has been triggered. Attempting to save log file and then attempt closure.");
            //Attempt Save the rmc runtime logfile by clicking the save log button.
            SaveRMCRuntimeLogBtn_Click(sender, e);
            //If isScheduledBroadcast is true, close the program without asking.
            if (isScheduledBroadcast)
            {
                AddTextToLogList("Warning - [RMC Manager]: Scheduled broadcast is still running! Prohibiting program from closing.");
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
            PCCountLbl.Text = $"PC Count: {pcCount}";
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
            MessageBox.Show("Fast broadcast is a mode that will not check if the message was sent to the computer successfully. If disabled, the program will send the messages slowly and will check if the message sent successfully. This is useful if you need to send a message to a lot of computers quickly (e.g. in emergencies).");
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
            bool isFill = ModulesTabControl.Dock == DockStyle.Fill;
            ModulesTabControl.Dock = isFill ? DockStyle.Right : DockStyle.Fill;
            ToggleRMSGListBtn.Image = isFill ? Properties.Resources.icons8_hide_24 : Properties.Resources.icons8_expand_24;
            ToggleRMSGListBtn.Text = isFill ? "Hide RMSG List" : "Show RMSG List";
        }

        private void ScheduleBroadcastBtn_Click(object sender, EventArgs e)
        {
            AddTextToLogList("Info - [ScheduleBroadcast]: Opening the schedule broadcast form.");
            ScheduleBroadcastForm scheduleBroadcastForm = new();
            scheduleBroadcastForm.ShowDialog();
        }

        private void BroadcastHistoryBtn_Click(object sender, EventArgs e)
        {
            AddTextToLogList("Info - [BroadcastHistory]: Opening the broadcast history form.");
            BroadcastHistoryForm broadcastHistoryForm = new();
            broadcastHistoryForm.Show();
        }

        private void SaveRMCRuntimeLogBtn_Click(object sender, EventArgs e)
        {
            //Save the loglist to a file in the application startup path / the folder called RMCRuntimeLogFiles. With the name of the file being the current date and time and before that "RMC_Runtime_Log_"
            string logFileName = $"RMC_Runtime_Log_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt";
            try
            {
                File.WriteAllLines(Application.StartupPath + "\\RMC Runtime Logs\\" + logFileName, logList.Items.Cast<string>().ToArray());
                AddTextToLogList($"Info - [RuntimeSaveLog]: Runtime log saved to file: {logFileName}");
            }
            catch (Exception ex)
            {
                AddTextToLogList($"Error - [RuntimeSaveLog]: Failure in saving runtime log. {ex}");
                MessageBox.Show($"Error saving runtime log: {ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private async void TestBroadcastMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Broadcast the message to just the computer that the program is running on.
            if (MessageTxt.Text == "" || ComputerSelectList.Text == "")
            {
                MessageBox.Show("Message or PC list is empty. Please fill in the message and PC list before starting the broadcast.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AddTextToLogList("Error - [InitBroadcast]: Message or PC list is empty. Broadcast halted.");
                return;
            }
            int totalSeconds = ((int)expiryHourTime.Value * 3600) + ((int)expiryMinutesTime.Value * 60) + (int)expirySecondsTime.Value; //Calculate the total seconds from the hours, minutes and seconds for the message duration.
            //pcBroadcastModule.BroadcastPCMessage(MessageTxt.Text, Environment.MachineName, totalSeconds, false, EmergencyModeCheckbox.Checked, ReattemptOnErrorCheckbox.Checked, DontSaveBroadcastHistoryCheckbox.Checked, isScheduledBroadcast);
            await
            broadcastController.StartBroadcastModule(RMCEnums.PC, MessageTxt.Text, Environment.MachineName, totalSeconds, EmergencyModeCheckbox.Checked, ReattemptOnErrorCheckbox.Checked, DontSaveBroadcastHistoryCheckbox.Checked, isScheduledBroadcast);
        }

        private async void SendWOLPacketBtn_Click(object sender, EventArgs e)
        {
            AddTextToLogList("Info - [SendWOLPacket]: Sending WOL packet to the PC's in the list.");
            //Send a WOL packet to all mac addresses in the WOLTextbox. But first check if the textbox is empty or contains invalid mac addresses.
            string[] macAddresses = WOLTextbox.Text.Split(PCseparatorArray, StringSplitOptions.RemoveEmptyEntries);
            if (macAddresses.Length == 0)
            {
                MessageBox.Show("No MAC addresses in the textbox. Please add MAC addresses before sending a WOL packet.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AddTextToLogList("Error - [SendWOLPacket]: No MAC addresses in the textbox. Please add MAC addresses before sending a WOL packet.");
                return;
            }
            foreach (string macAddress in macAddresses)
            {
                if (!WakeOnLANModule.IsValidMacAddress(macAddress))
                {
                    //MessageBox.Show("Invalid MAC address: " + macAddress, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AddTextToLogList($"Error - [SendWOLPacket]: Invalid MAC address: {macAddress}");
                    return;
                }
                AddTextToLogList($"Info - [SendWOLPacket]: Sending WOL packet to MAC address: {macAddress}");
                await
                WakeOnLANModule.WakeOnLan(macAddress, (int)MagicPortNumberBox.Value);
            }
        }

        private void MagicPortNumberBox_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.MagicPortNumber = MagicPortNumberBox.Value;
            Properties.Settings.Default.Save();
        }
        private void ReattemptonErrorHelpLbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("If the program fails to send a message to a PC, it will reattempt to send the message to the PC. This will only happen once per PC. If it fails again, it will not reattempt to send the message.");
        }

        private void ReattemptOnErrorCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ReattemptOnError = ReattemptOnErrorCheckbox.Checked;
            Properties.Settings.Default.Save();
        }
    }
}
