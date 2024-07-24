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
//WOL added, but testing is needed.
//Test broadcasting with unhandled exceptions, and see if the program can recover from it. If it pauses broadcasting, add a try catch to that function to prevent it from pausing. This can be done closer to completion.
//It might be an idea to disable all msgbox popups during startup, also check if there are msgboxes during the broadcast. Remove those.
//Add a function to check if the message is too long for the msg command. If it is, split the message into multiple messages. (This is a low priority task, but might be a good idea in the future)
//Add icons (like x or tick boxes) to the broadcast History list. In other words, redo the broadcast history window.
//Test scheduled broadcasts and panic button. Make sure they work as intended.
//Could make the CLI redundant by checking if a STARTUP.rmsg exists, if it does, load it and start the broadcast. The command line argument function is already in place, so this should be easy to implement.
//Add PSexec saves to the IO manager.
//Fix me list:
//(1) [FIXME] 21/07/24

namespace RapidMessageCast_Manager
{
    public partial class RMCManager : Form
    {
        public string versionNumb = Application.ProductVersion; //Get the version number of the program.
        private static readonly char[] PCseparatorArray = ['\n', '\r']; //Used for PCList parsing.
        readonly ImageList tabControlImageList = new(); //Used for the icons on the tabs.
        private bool dontPromptClosureMessage = false; //Used for scheduled broadcasts. If true, the program will close after the broadcast has finished without a warning message.
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
            //This if statement is to prevent the program from loading fully when the program is started with a command line argument (This will prevent slowdowns when doing automatic broadcasting).
            if (!dontPromptClosureMessage)
            {
                LoadGlobalSettings();
                CheckSystemState();
                AddIconsToTabControls();
                HandleDefaultRMSGFile();
                UpdateUIWithVersionInformation();
                RefreshRMSGFileList();
                InitalizeToolTipHelp();
                RMC_IO_Manager.AttemptToCreateRMCDirectories();
            }
            AddTextToLogList("Info - [RMC Startup]: RMC GUI is now ready.");
        }

        #region Functions
        //Start of the functions.
        private void InitalizeToolTipHelp()
        {
            try
            {
                Control[] checkboxes = [FastBroadcastModeCheckbox, MessagePCcheckBox, MessageEmailcheckBox, PSExecModuleEnableCheckBox, ReattemptOnErrorCheckbox, DontSaveBroadcastHistoryCheckbox];
                Control[] buttons = [StartBroadcastBtn, clearLogBtn, SaveRMCRuntimeLogBtn, SaveRMCFileBTN, QuickSaveRMSGBtn, RefreshQuickLoadListBtn, DeleteSelectedRMSGFileBtn, RenameSelectedRMSGBtn, LoadSelectedRMSGBtn, SavePCListTxtBtn, PCSaveMessageTxtBtn, OpenRMCFileBtn, ActiveDirectorySelectBtn, PCMessageOpenTxtBtn, PCComputerListOpenBtn];
                string[] checkboxToolTips = ["Enable fast broadcasting mode. This will send the message without checking if it was sent.", "Enable the PC message module.", "Enable the Email message module.", "Enable the PSExec message module.", "Reattempt to send the message if an error occurs.", "Don't save the broadcast history after RMC completes a broadcast."];
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
                AddTextToLogList("Info - [CheckCommandLineArguments]: Startup command line arguments detected. Checking for RMSG file.");
                AddTextToLogList($"Info - [CheckCommandLineArguments]: Command line arguments: {string.Join(" ", args)}");

                if (args.Length >= 2 && args[1].Equals("panic", StringComparison.CurrentCultureIgnoreCase))
                {
                    try
                    {
                        dontPromptClosureMessage = true;
                        //Panic button that activates broadcasting immediately with a predefined message.
                        AddTextToLogList("Info - [CheckCommandLineArguments]: [PANIC!] Panic Startup detected. Loading RMSG file and starting emergency alert broadcast...");
                        //Check if args 2 is vaild or even exists. If it doesn't, use the default panic message.
                        if (args.Length > 2)
                        {
                            LoadAndParseRMSGFile(args[2]);
                        }
                        else
                        {
                            AddTextToLogList("Error - [CheckCommandLineArguments]: PANIC broadcast - PANIC message not loaded. Attempting to load PANIC.rmsg...");
                            LoadAndParseRMSGFile($"{Application.StartupPath}\\RMSGFiles\\PANIC.rmsg");
                        }
                        //Check if the message loaded, if it didn't then as a failsafe, set the message to a predefined message.
                        if (PCBroadcastMessageTxt.Text == "")
                        {
                            AddTextToLogList("Error - [CheckCommandLineArguments]: PANIC broadcast - PANIC message not loaded. Using predefined message.");
                            PCBroadcastMessageTxt.Text = "PANIC BUTTON ALERT: This is a PANIC message. Please evacuate the building immediately. This is not a drill.";
                        }
                        //Check if the PC list is empty, if it is, then close the program.
                        if (PCBroadcastToList.Text == "")
                        {
                            AddTextToLogList("Critical - [CheckCommandLineArguments]: PANIC broadcast - PC list is empty. Closing program.");
                            MessageBox.Show("Critical error! PANIC button failed to broadcast. Please check the RMC log list for more information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Application.Exit();
                        }
                        AddTextToLogList("Info - [CheckCommandLineArguments]: PANIC broadcast - Starting broadcast.");
                        StartBroadcastBtn_Click(this, EventArgs.Empty);
                        CloseAfterAllModulesAreFinished();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Critical error! Panic button failed to broadcast. Please check the RMC log list for more information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        AddTextToLogList($"Critical - [CheckCommandLineArguments]: [PANIC Error!] Panic button failed to broadcast: {ex}");
                    }
                }
                else if (args.Length == 2)
                {
                    AddTextToLogList("Info - [CheckCommandLineArguments]: Loading RMSG file from command line argument.");
                    LoadAndParseRMSGFile(args[1]);
                }
                else if (args.Length == 3 && args[1].Equals("schedule", StringComparison.CurrentCultureIgnoreCase)) //Arugment 1 is schedule, argument 2 is the RMSG file.
                {
                    AddTextToLogList("Info - [CheckCommandLineArguments]: [SCHEDULE] Startup loading RMSG file from command line argument and starting scheduled broadcast.");
                    dontPromptClosureMessage = true;
                    RunScheduledBroastcast(args[2]);
                }
            }
        }

        private async void CloseAfterAllModulesAreFinished()
        {
            //Check if all modules are finished. If they are, close the program.
            //use the broadcastController to check if all modules are finished via the AreAnyModulesRunning void func
            //if they are not finished, then repeat this every 5 seconds until they are.
            //if they are finished, then close the program. PS. This is the simplest code in this project. :)
            while (broadcastController.AreAnyModulesRunning())
            {
                await Task.Delay(5000); //broadcastController will handle hung modules so we don't need to worry about that in this function.
            }
            dontPromptClosureMessage = true; //Setting this true will prevent the program from asking the user if they want to close.
            Application.Exit();
        }

        private void HandleDefaultRMSGFile()
        {
            var defaultRMSGPath = $"{Application.StartupPath}\\RMSGFiles\\default.rmsg";
            if (File.Exists(defaultRMSGPath))
            {
                LoadAndParseRMSGFile(defaultRMSGPath);
                AddTextToLogList("Info - [HandleDefaultRMSGFile]: Default.rmsg file loaded.");
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
            //(1) [FIXME] 21/07/24 - Why is this function only running the PC module? It should check if the other modules are enabled and run them as well. TODO: Fix this.
            LoadAndParseRMSGFile(RMSGFile); //Load the RMSG file into the program.
            //Check if modules are selected. If not, close the program.
            if (!MessagePCcheckBox.Checked && !MessageEmailcheckBox.Checked && !PSExecModuleEnableCheckBox.Checked)
            {
                Application.Exit();
            }
            //Check if message or pclist is empty. If it is, close the program.
            if (PCBroadcastMessageTxt.Text == "" || PCBroadcastToList.Text == "")
            {
                Application.Exit();
            }
            int totalSeconds = ((int)PCexpiryHourTime.Value * 3600) + ((int)PCexpiryMinutesTime.Value * 60) + (int)PCexpirySecondsTime.Value; //Calculate the total seconds from the hours, minutes and seconds for the message duration.
            await
            broadcastController.StartBroadcastModule(RMCEnums.PC, PCBroadcastMessageTxt.Text, PCBroadcastToList.Text, totalSeconds, FastBroadcastModeCheckbox.Checked, ReattemptOnErrorCheckbox.Checked, DontSaveBroadcastHistoryCheckbox.Checked, dontPromptClosureMessage);
            CloseAfterAllModulesAreFinished();
        }

        private void CheckSystemState()
        {
            var systemChecker = new SystemHealthCheck(AddTextToLogList);
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
                PSExecModuleEnableCheckBox.Enabled = false;
                PsExecLabel.Text = "PsExec (Disabled)";
            }
            systemChecker.AreCriticalServicesRunning();
            AddTextToLogList("Info - [CheckSystemState]: System state check completed.");
        }
        public void CheckForUpdates()
        {
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
                MessageBox.Show("No updates available or no connection is detected.", "Update Check", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                AddTextToLogList($"Error - [AddIconsToTabControls]: Failed to add images to the GUI form tabcontrol: {ex}");
            }
        }

        public void AddTextToLogList(string item)
        {
            //Debug States:
            //Info - Used for general information.
            //Error - Used for errors that are not critical.
            //Critical - Used for critical errors that could impact the program or its ability to message pcs.
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
                    Console.WriteLine($"Critical - [AddTextToLogList] Error! An exception occurred when adding an item to the loglist: {ex}");
                }
                catch
                {
                    MessageBox.Show("Fatal Error! Unable to write to console! RMC will attempt save the debug log and will terminate.", "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SaveRMCRuntimeLogBtn_Click(this, EventArgs.Empty); //Attempt Save the loglist to a file.
                    Environment.Exit(713); //Bail out. If we can't even write to the console, then something is seriously wrong. ERROR_FATAL_APP_EXIT
                }
            }
        }

        private void LoadGlobalSettings()
        {
            //Load the settings from the settings file.
            FastBroadcastModeCheckbox.Checked = Properties.Settings.Default.EmergencyMode;
            DontSaveBroadcastHistoryCheckbox.Checked = Properties.Settings.Default.DontSaveBroadcastHistory;
            MessagePCcheckBox.Checked = Properties.Settings.Default.MessagePCEnabled;
            PSExecModuleEnableCheckBox.Checked = Properties.Settings.Default.MessagePSExecEnabled;
            MessageEmailcheckBox.Checked = Properties.Settings.Default.MessageEmailEnabled;
            WOLPortNumberBox.Value = Properties.Settings.Default.MagicPortNumber;
            ReattemptOnErrorCheckbox.Checked = Properties.Settings.Default.ReattemptOnError;
            AddTextToLogList($"Info - [LoadGlobalSettings]: {Properties.Settings.Default.Properties.Count} Program settings loaded.");
        }
        private static void SetCheckboxState(CheckBox checkBox, string value)
        {
            checkBox.Checked = value == "True";
        }
        private void LoadAndParseRMSGFile(string filePath)
        {
            AddTextToLogList($"Info - [LoadRMSGFileInProgram]: Loading RMSG file: {Path.GetFileName(filePath)}");
            string[] RMSGFileValues = RMC_IO_Manager.LoadRMSGFile(filePath);

            if (!string.IsNullOrEmpty(RMSGFileValues[0]))
            {
                //If first word is error, addtext and return. If warning, addtext but continue.
                if (RMSGFileValues[0].StartsWith("Warning"))
                {
                    AddTextToLogList($"Warning - [LoadRMSGFileInProgram]: Loading message returned a non-critical warning: {RMSGFileValues[0]} - Continuing anyway.");
                }
                else if (RMSGFileValues[0].StartsWith("Error"))
                {
                    AddTextToLogList($"Error - [LoadRMSGFileInProgram]: Loading message returned an error: {RMSGFileValues[0]} - Loading of RMSG file halted.");
                    return;
                }
                else
                {
                    AddTextToLogList($"Error - [LoadRMSGFileInProgram]: Loading message returned an unknown error: {RMSGFileValues[0]} - Loading of RMSG file halted.");
                    return;
                }
            }
            AddTextToLogList($"Info - [LoadRMSGFileInProgram]: Parsing RMSG file: {Path.GetFileName(filePath)}");
            try
            {
                //Check RMCSoftwareVersion, if it doesn't match the current version, then show a message box.
                if (RMSGFileValues[20] != versionNumb)
                {
                    MessageBox.Show("The RMSG file was created with a different version of RMC. Please save this file again to convert this save to one that is compatible with this current version of RMC.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AddTextToLogList($"Info - [LoadRMSGFileInProgram]: RMSG file was created with a different version of RMC. RMSG resave might be required. RMC Version: {versionNumb}, RMSG Version: {RMSGFileValues[20]}");
                }
                PCBroadcastMessageTxt.Text = RMSGFileValues[1];
                PCBroadcastToList.Text = RMSGFileValues[2];
                PCexpiryHourTime.Value = Convert.ToDecimal(RMSGFileValues[3]);
                PCexpiryMinutesTime.Value = Convert.ToDecimal(RMSGFileValues[4]);
                PCexpirySecondsTime.Value = Convert.ToDecimal(RMSGFileValues[5]);
                SetCheckboxState(FastBroadcastModeCheckbox, RMSGFileValues[6]);
                SetCheckboxState(MessagePCcheckBox, RMSGFileValues[7]);
                SetCheckboxState(MessageEmailcheckBox, RMSGFileValues[8]);
                SetCheckboxState(PSExecModuleEnableCheckBox, RMSGFileValues[9]);
                SetCheckboxState(ReattemptOnErrorCheckbox, RMSGFileValues[10]);
                SetCheckboxState(DontSaveBroadcastHistoryCheckbox, RMSGFileValues[11]);
                WOLTextbox.Text = RMSGFileValues[12];
                WOLPortNumberBox.Value = Convert.ToDecimal(RMSGFileValues[13]);
                AddressOfSMTPServerTxt.Text = RMSGFileValues[14];
                EmailPortNumber.Value = Convert.ToDecimal(RMSGFileValues[15]);
                SenderAddressTxt.Text = RMSGFileValues[16];
                EmailAuthTypecomboBox.Text = RMSGFileValues[17];
                EmailAccountTextbox.Text = RMSGFileValues[18];
                EmailPasswordTextbox.Text = RMSGFileValues[19];
                AddTextToLogList($"Info - [LoadRMSGFileInProgram]: RMSG File loaded successfully: {Path.GetFileName(filePath)}");
            }
            catch (FormatException ex1)
            {
                AddTextToLogList($"Error - [LoadRMSGFileInProgram]: Format Parse Failure - Format exception when loading RMSG file, RMSG file is not loading correctly! {ex1}");
                MessageBox.Show("A RMSG format error has occurred. Please check the debug log for file errors.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadGlobalSettings(); //If the RMSG file fails to load, then load the global settings instead.
                return;
            }
            catch (Exception ex)
            {
                AddTextToLogList($"Error - [LoadRMSGFileInProgram]: General Parse Failure: {ex} - Will now load global settings.");
                LoadGlobalSettings(); //If the RMSG file fails to load, then load the global settings instead.
                return;
            }
        }

        private void RefreshRMSGFileList()
        {
            //Add all files that exists in the application startup + RMSGFiles directory into the RMSGFiles listbox.
            try
            {
                QuickLoadListbox.Items.Clear();
                string[] files = Directory.GetFiles(Application.StartupPath + "\\RMSGFiles\\");
                foreach (string file in files)
                {
                    QuickLoadListbox.Items.Add(Path.GetFileName(file));
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
                    PCBroadcastToList.Text += resEnt.GetDirectoryEntry().Name + "\r\n";
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
            int remainingCharacters = 255 - PCBroadcastMessageTxt.TextLength;
            if (remainingCharacters >= 0)
            {
                PCMessageCharLimitLbl.Text = $"Length Remaining: {remainingCharacters}";
                PCMessageCharLimitLbl.ForeColor = Color.White;
            }
            else
            {
                // Prevent entering more characters than the limit
                PCBroadcastMessageTxt.Text = PCBroadcastMessageTxt.Text[..255];
            }
            if (remainingCharacters <= 30)
            {
                PCMessageCharLimitLbl.ForeColor = Color.Red;
            }
            else if (remainingCharacters <= 100)
            {
                PCMessageCharLimitLbl.ForeColor = Color.Yellow;
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
                LoadAndParseRMSGFile(openFileDialog.FileName);
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
                Enum.TryParse(EmailAuthTypecomboBox.Text, out AuthMode authMode);
                RMC_IO_Manager.SaveRMSGFile(fileName, PCBroadcastMessageTxt.Text, PCBroadcastToList.Text, WOLTextbox.Text, (int)WOLPortNumberBox.Value, PCexpiryHourTime.Value.ToString(), PCexpiryMinutesTime.Value.ToString(), PCexpirySecondsTime.Value.ToString(), FastBroadcastModeCheckbox.Checked, MessagePCcheckBox.Checked, MessageEmailcheckBox.Checked, PSExecModuleEnableCheckBox.Checked, ReattemptOnErrorCheckbox.Checked, DontSaveBroadcastHistoryCheckbox.Checked, AddressOfSMTPServerTxt.Text, EmailPortNumber.Value, SenderAddressTxt.Text, authMode, EmailAccountTextbox.Text, EmailPasswordTextbox.Text);
                RefreshRMSGFileList();
            }
        }

        private void OpenMessageTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new RMC_IO_Manager(AddTextToLogList).OpenFileAndProcessContents(PCBroadcastMessageTxt, "OpenMessageAsTxt", "OpenMessageAsTxt", RegexFilters.FilterInvalidMessage);
        }

        private void OpenSendComputerListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new RMC_IO_Manager(AddTextToLogList).OpenFileAndProcessContents(PCBroadcastToList, "OpenPCList", "OpenPCList", RegexFilters.FilterInvalidPCNames);
        }

        private void OpenMacAddressfromTxtBtn_Click(object sender, EventArgs e)
        {
            new RMC_IO_Manager(AddTextToLogList).OpenFileAndProcessContents(WOLTextbox, "MacAddressfromTxt", "OpenMacAddressfromTxt", RegexFilters.FilterInvalidPCNames);
        }

        private void SaveMacAddressesAsTXTBtn_Click(object sender, EventArgs e)
        {
            new RMC_IO_Manager(AddTextToLogList).SaveFileFromTextBox(WOLTextbox, "SaveMacAddressesAsTXTBtn", "SaveMacAddressesAsTXTBtn");
        }

        private void SaveComputerListBtn_Click(object sender, EventArgs e)
        {
            new RMC_IO_Manager(AddTextToLogList).SaveFileFromTextBox(PCBroadcastToList, "SavePCList", "SavePCList");
        }

        private void SaveMessageBtn_Click(object sender, EventArgs e)
        {
            new RMC_IO_Manager(AddTextToLogList).SaveFileFromTextBox(PCBroadcastMessageTxt, "SaveMessageBtn", "SaveMessageBtn");
        }

        private void ComputerSelectList_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow control key combinations (e.g., CTRL + V for paste)
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                return;
            }

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
            bool isMessagePSExecChecked = PSExecModuleEnableCheckBox.Checked;

            if (!isMessagePCChecked && !isMessageEmailChecked && !isMessagePSExecChecked)
            {
                AddTextToLogList("Error - [InitBroadcast]: No modules are enabled. Unable to broadcast.");
                MessageBox.Show("No modules are enabled. Please enable at least one module before starting the broadcast.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (isMessagePCChecked)
            {
                AddTextToLogList("Info - [InitBroadcast]: PC message module is selected. Notifying the broadcast controller to start the PC broadcast.");

                string messageText = PCBroadcastMessageTxt.Text;
                string computerSelectListText = PCBroadcastToList.Text;

                if (string.IsNullOrWhiteSpace(messageText) || string.IsNullOrWhiteSpace(computerSelectListText))
                {
                    MessageBox.Show("Message or PC list is empty. Please fill in the message and PC list before starting the broadcast.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AddTextToLogList("Error - [InitBroadcast]: PC Broadcast - Message textbox or PC list are empty. Broadcast halted.");
                    return;
                }

                int totalSeconds = ((int)PCexpiryHourTime.Value * 3600) + ((int)PCexpiryMinutesTime.Value * 60) + (int)PCexpirySecondsTime.Value;
                await broadcastController.StartBroadcastModule(RMCEnums.PC, messageText, computerSelectListText, totalSeconds, FastBroadcastModeCheckbox.Checked, ReattemptOnErrorCheckbox.Checked, DontSaveBroadcastHistoryCheckbox.Checked, dontPromptClosureMessage);
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
            string? selectedFile = QuickLoadListbox.SelectedItem?.ToString();
            if (selectedFile != null)
            {
                LoadAndParseRMSGFile(Path.Combine(Application.StartupPath, "RMSGFiles", selectedFile));
            }
        }

        private void QuickSaveRMSGBtn_Clicked(object sender, EventArgs e)
        {
            //Save a quick save file name based on the current date and time.
            string quickSaveFileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".rmsg";

            AddTextToLogList($"Info - [QuickSaveBtn]: Quick saving RMSG file: {quickSaveFileName}");
            //Use the SaveRMSGFile in the RMC_IO_Manager to save the file.
            Enum.TryParse(EmailAuthTypecomboBox.Text, out AuthMode authMode);
            RMC_IO_Manager.SaveRMSGFile(Path.Combine(Application.StartupPath, "RMSGFiles", quickSaveFileName), PCBroadcastMessageTxt.Text, PCBroadcastToList.Text, WOLTextbox.Text, (int)WOLPortNumberBox.Value, PCexpiryHourTime.Value.ToString(), PCexpiryMinutesTime.Value.ToString(), PCexpirySecondsTime.Value.ToString(), FastBroadcastModeCheckbox.Checked, MessagePCcheckBox.Checked, MessageEmailcheckBox.Checked, PSExecModuleEnableCheckBox.Checked, ReattemptOnErrorCheckbox.Checked, DontSaveBroadcastHistoryCheckbox.Checked, AddressOfSMTPServerTxt.Text, EmailPortNumber.Value, SenderAddressTxt.Text, authMode, EmailAccountTextbox.Text, EmailPasswordTextbox.Text);
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
            string? selectedFile = QuickLoadListbox.SelectedItem?.ToString();
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
            string? selectedFile = QuickLoadListbox.SelectedItem?.ToString();
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
            MessageBox.Show("The RMSG file is a file that contains the configuration of the RMC program when it was saved. It's used to quickly send messages to a list of computers without having to type it out every time. if you save a RMSG file as default.rmsg, it will load it automatically when the program starts.");
        }

        private void RMCManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                AddTextToLogList($"Info - [RMC Manager]: Program closure has been triggered. (Closure Type: {dontPromptClosureMessage}).");
                // If isScheduledBroadcast is true, close the program without asking.
                if (dontPromptClosureMessage)
                {
                    if (!broadcastController.AreAnyModulesRunning())
                    {
                        AddTextToLogList("Info - [RMC Manager]: Scheduled programming has completed. closing...");
                        return;
                    }
                    else
                    {
                        AddTextToLogList("Warning - [RMC Manager]: Scheduled programming is still running. Closing the program will stop the broadcast. Rejecting close request and checking again in 10 seconds...");
                        e.Cancel = true;
                        //retry the close after 10 seconds.
                        Task.Delay(10000).ContinueWith(_ => CloseAfterAllModulesAreFinished());
                        return;
                    }
                }
                if (broadcastController.AreAnyModulesRunning()) // User might have clicked close on accident since there is a broadcast in progress. Confirm here.
                {
                    AddTextToLogList("Warning - [RMC Manager]: Broadcast is still running. Closing the program will stop the broadcast. Confirming close request.");
                    DialogResult dialogResult = MessageBox.Show("Are you sure you want to close the program? The broadcast is still running. Closing the program will stop the broadcast.", "Close Program - RapidMessageCast Manager", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
                // Ask the user if they want to close the program if they press the X button. However, if the MessageTxt and pclist is empty, close the program without asking.
                if (!(PCBroadcastMessageTxt.Text == "" && PCBroadcastToList.Text == ""))
                {
                    DialogResult dialogResult = MessageBox.Show("Are you sure you want to close the program? Any unsaved data will be lost.", "Close Program - RapidMessageCast Manager", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }
            finally
            {
                // Attempt Save the rmc runtime logfile by clicking the save log button.
                AddTextToLogList("Info - [RMC Manager]: Saving log file and closing... [END OF LOG]");
                SaveRMCRuntimeLogBtn_Click(sender, e);
            }
        }

        private void ComputerSelectList_TextChanged(object sender, EventArgs e)
        {
            //For each line, count it on the PCCountLBL.
            int pcCount = PCBroadcastToList.Lines.Length;
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
            Properties.Settings.Default.EmergencyMode = FastBroadcastModeCheckbox.Checked;
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
            Properties.Settings.Default.MessagePSExecEnabled = PSExecModuleEnableCheckBox.Checked;
            Properties.Settings.Default.Save();
        }

        private void MessageEmailcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //Save the checkbox state to the settings file.
            Properties.Settings.Default.MessageEmailEnabled = MessageEmailcheckBox.Checked;
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
            PCBroadcastToList.Text += Environment.MachineName + "\r\n";
        }

        private void ClearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PCBroadcastToList.Clear();
        }

        private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PCBroadcastToList.SelectAll();
        }

        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PCBroadcastToList.Undo();
        }

        private void RMCManager_Shown(object sender, EventArgs e)
        {
            //check if scheduled broadcast is true. If it is, hide the form.
            if (dontPromptClosureMessage)
            {
                Hide();
            }
        }

        private async void TestBroadcastMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Broadcast the message to just the computer that the program is running on.
            if (PCBroadcastMessageTxt.Text == "" || PCBroadcastToList.Text == "")
            {
                MessageBox.Show("Message or PC list is empty. Please fill in the message and PC list before starting the broadcast.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AddTextToLogList("Error - [InitBroadcast]: Message or PC list is empty. Broadcast halted.");
                return;
            }
            int totalSeconds = ((int)PCexpiryHourTime.Value * 3600) + ((int)PCexpiryMinutesTime.Value * 60) + (int)PCexpirySecondsTime.Value; //Calculate the total seconds from the hours, minutes and seconds for the message duration.
            //pcBroadcastModule.BroadcastPCMessage(MessageTxt.Text, Environment.MachineName, totalSeconds, false, EmergencyModeCheckbox.Checked, ReattemptOnErrorCheckbox.Checked, DontSaveBroadcastHistoryCheckbox.Checked, isScheduledBroadcast);
            await
            broadcastController.StartBroadcastModule(RMCEnums.PC, PCBroadcastMessageTxt.Text, Environment.MachineName, totalSeconds, FastBroadcastModeCheckbox.Checked, ReattemptOnErrorCheckbox.Checked, DontSaveBroadcastHistoryCheckbox.Checked, dontPromptClosureMessage);
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
                if (!WakeOnLANManager.IsValidMacAddress(macAddress))
                {
                    //MessageBox.Show("Invalid MAC address: " + macAddress, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AddTextToLogList($"Error - [SendWOLPacket]: Invalid MAC address: {macAddress}");
                    return;
                }
                AddTextToLogList($"Info - [SendWOLPacket]: Sending WOL packet to MAC address: {macAddress}");
                await
                WakeOnLANManager.WakeOnLan(macAddress, (int)WOLPortNumberBox.Value);
            }
        }

        private void MagicPortNumberBox_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.MagicPortNumber = WOLPortNumberBox.Value;
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

        private void OpenRMCLogFolderBtn_Click(object sender, EventArgs e)
        {
            //Create a process 
            Process process = new()
            {
                //Set the startInfo to the process.
                StartInfo = new ProcessStartInfo("RMC Runtime Logs")
                {
                    UseShellExecute = true
                }
            };
            //Start the process.
            process.Start();
        }
        private void FilterPCListBtn_Click(object sender, EventArgs e)
        {
            //Get the text from the PC list and send it to the filter form.
            FilterPCListForm filterForm = new(PCBroadcastToList.Text);
            //Then after the form closes, get the text from the form and set it to the PC list.
            if (filterForm.ShowDialog() == DialogResult.OK)
            {
                PCBroadcastToList.Text = filterForm.MessagePCList.Text;
            }
        }

        private void CheckforUpdatesLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CheckForUpdates();
        }

        private void RenewIPBtn_Click(object sender, EventArgs e)
        {
            AddTextToLogList("Info - [RenewIPBtn]: Renewing the IP address of the computer.");
            SystemServiceManager.ReleaseAndRenewIP();
        }

        private void ShowRMSGListBtn_Click(object sender, EventArgs e)
        {
            //Set the ModulesTabControl dock style to right. 
            ModulesTabControl.Dock = DockStyle.Right;
            ShowRMSGListBtn.Visible = false;
        }

        private void HideQuickLoadBtn_Click(object sender, EventArgs e)
        {
            //Set the ModulesTabControl dock style to fill.
            ModulesTabControl.Dock = DockStyle.Fill;
            ShowRMSGListBtn.Visible = true;
        }

        private void OpenSaveLocationBtn_Click(object sender, EventArgs e)
        {
            //Create a process 
            Process process = new()
            {
                //Set the startInfo to the process.
                StartInfo = new ProcessStartInfo("RMSGFiles")
                {
                    UseShellExecute = true
                }
            };
            //Start the process.
            process.Start();
        }
    }
}