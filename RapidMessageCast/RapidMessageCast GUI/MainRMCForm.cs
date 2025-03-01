using RapidMessageCast_Manager.Internal_RMC_Components;
using System.DirectoryServices;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
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
//Also move the WOL page to the main tab control instead of the PC tab control.
//Add WOL to the broadcast controller: Do the WOL, add a delay, and then broadcast the MSG message, do not delay any other modules.
//Add Run Program as a module. This will allow the user to run a program on the local machine that can do anything.
//Fix me list:
//(1) [FIXME] - 21/07/24 - Why is this function (ScheduledBroadcast) only running the PC module? It should check if the other modules are enabled and run them as well. TODO: Fix this.
//(2) [Critical Fault] - 03/08/24 - There is a fault where if there are too many msg commands sent at once, the entire network service stack on the broadcaster machine freezes. Primary suspect is DNS Service hanging, I believe this is because the DNS Client has to parse alot of DNS hostnames quickly so it gets overwhelmed. This is a theory, but it is the most likely cause. This is a critical fault that needs to be fixed before the program can be released. Try to make a service restart for DNS Client if this happens. This is a critical fault that needs to be fixed before the program can be released. Try to make a service restart for DNS Client if this happens.
//Reenable TRACELOG SAVES WHEN FORM CLOSED once built, is it just writing loads of logs to my disk when im constantly testing.

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
            TraceLog($"Info - [{MethodBase.GetCurrentMethod()?.Name ?? "FormLoad"}]: Starting RMC {versionNumb}. Welcome, {Environment.UserName}");
            CheckCommandLineArguments();
            //This if statement is to prevent the program from loading fully when the program is started with a command line argument (This will prevent slowdowns when doing automatic broadcasting).
            if (!dontPromptClosureMessage)
            {
                LoadGlobalSettings();
                CheckSystemState();
                AddIconsToTabControls();
                TraceLog(IOManager.AttemptToCreateRMCDirectories());
                HandleAutoStartRMSGFiles();
                UpdateUIWithVersionInformation();
                RefreshRMSGFileList();
                InitalizeToolTipHelp();
            }
            TraceLog("Info - [RMC Startup]: RMC is now ready.");
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
                TraceLog($"Error - [ToolTipHelp]: Failed to initalize the tooltip help, this is not a critical error: {ex}"); //This error isn't critical, so it can be ignored tbh.
            }
        }

        public static string EncryptData(string data)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            byte[] protectedBytes = ProtectedData.Protect(dataBytes, null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(protectedBytes);
        }

        public static string DecryptData(string protectedData)
        {
            byte[] protectedBytes = Convert.FromBase64String(protectedData);
            byte[] dataBytes = ProtectedData.Unprotect(protectedBytes, null, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(dataBytes);
        }

        private void UpdateStartBroadcastButtonText(string text)
        {
            if (StartBroadcastBtn.InvokeRequired)
            {
                StartBroadcastBtn.Invoke(new System.Windows.Forms.MethodInvoker(() => UpdateStartBroadcastButtonText(text)));
            }
            else
            {
                StartBroadcastBtn.Text = text;
            }
        }

        private void NotifyBroadcastStatus()
        {
            if (broadcastController.AreAnyModulesRunning())
            {
                UpdateStartBroadcastButtonText("Broadcasting...");
                StartBroadcastBtn.BackColor = Color.Green;
                Task.Delay(2000).ContinueWith(_ => NotifyBroadcastStatus());
            }
            else
            {
                //set backcolour to 53, 48, 70
                StartBroadcastBtn.BackColor = Color.FromArgb(53, 48, 70);
                UpdateStartBroadcastButtonText("Start Message Broadcast");
            }
        }

        private void CheckCommandLineArguments()
        {
            var args = Environment.GetCommandLineArgs();
            if (args.Length <= 1) return;

            TraceLog("Info - [CheckCommandLineArguments]: Startup command line arguments detected. Checking for RMSG file.");
            TraceLog($"Info - [CheckCommandLineArguments]: Command line arguments: {string.Join(" ", args)}");

            try
            {
                switch (args[1].ToLower())
                {
                    case "panic":
                        HandlePanicMode(args);
                        break;
                    case "schedule" when args.Length == 3:
                        TraceLog("Info - [CheckCommandLineArguments]: [SCHEDULE] Startup loading RMSG file from command line argument and starting scheduled broadcast.");
                        dontPromptClosureMessage = true;
                        RunScheduledBroastcast(args[2]);
                        break;
                    default:
                        if (args.Length == 2)
                        {
                            TraceLog("Info - [CheckCommandLineArguments]: Loading RMSG file from command line argument.");
                            LoadAndParseRMSGFile(args[1]);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Critical error! Panic button failed to broadcast. Please check the RMC log list for more information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TraceLog($"Critical - [CheckCommandLineArguments]: [PANIC Error!] Panic button failed to broadcast: {ex}");
            }
        }

        private void HandlePanicMode(string[] args)
        {
            dontPromptClosureMessage = true;
            TraceLog("Info - [CheckCommandLineArguments]: [PANIC!] Panic Startup detected. Loading RMSG file and starting emergency alert broadcast...");

            string rmsgFilePath = args.Length > 2 ? args[2] : $"{Application.StartupPath}\\RMSGFiles\\PANIC.rmsg";
            LoadAndParseRMSGFile(rmsgFilePath);

            if (string.IsNullOrEmpty(PCBroadcastMessageTxt.Text))
            {
                TraceLog("Error - [CheckCommandLineArguments]: PANIC broadcast - PANIC message not loaded. Asking if allowed to use predefined message.");
                //Ask user if they want to use the predefined message.
                DialogResult dialogResult = MessageBox.Show("PANIC message not loaded. Use predefined message?\n \"PANIC BUTTON ALERT: This is a PANIC message. Please evacuate the building immediately. This is not a drill.\"", "PANIC Message", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    TraceLog("Info - [CheckCommandLineArguments]: PANIC broadcast - User approved using predefined message, broadcasting predefined PANIC message.");
                    PCBroadcastMessageTxt.Text = "PANIC BUTTON ALERT: This is a PANIC message. Please evacuate the building immediately. This is not a drill.";
                }
                else
                {
                    TraceLog("Critical - [CheckCommandLineArguments]: PANIC broadcast - PANIC message not loaded. Closing program.");
                    MessageBox.Show("Critical error! PANIC button failed to broadcast. Please check the RMC log list for more information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
            }

            if (string.IsNullOrEmpty(PCBroadcastToList.Text))
            {
                TraceLog("Critical - [CheckCommandLineArguments]: PANIC broadcast - PC list is empty. Closing program.");
                MessageBox.Show("Critical error! PANIC button failed to broadcast. Please check the RMC log list for more information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            TraceLog("Info - [CheckCommandLineArguments]: PANIC broadcast - RMSG parsed, Starting broadcast...");
            StartBroadcastBtn_Click(this, EventArgs.Empty);
            CloseAfterAllModulesAreFinished();
        }


        private async void CloseAfterAllModulesAreFinished()
        {
            TraceLog("Info - [CloseAfterAllModulesAreFinished]: Closing program after all modules are finished....");
            //Check if all modules are finished. If they are, close the program.
            //use the broadcastController to check if all modules are finished via the AreAnyModulesRunning void func
            //if they are not finished, then repeat this every 5 seconds until they are.
            //if they are finished, then close the program. PS. This is the simplest code in this project. :)
            while (broadcastController.AreAnyModulesRunning())
            {
                TraceLog("Info - [CloseAfterAllModulesAreFinished]: (Timer TICK) Waiting for all modules to finish...");
                await Task.Delay(5000); //broadcastController will handle hung modules so we don't need to worry about that in this function.
            }
            TraceLog("Info - [CloseAfterAllModulesAreFinished]: All modules are finished. Closing program.");
            dontPromptClosureMessage = true; //Setting this true will prevent the program from asking the user if they want to close.
            Application.Exit();
        }

        private void HandleAutoStartRMSGFiles()
        {
            var rmsgFiles = new[]
            {
                $"{Application.StartupPath}\\RMSGFiles\\default.rmsg",
                $"{Application.StartupPath}\\RMSGFiles\\autobroadcast.rmsg"
            };

            foreach (var filePath in rmsgFiles)
            {
                if (File.Exists(filePath))
                {
                    LoadAndParseRMSGFile(filePath);
                    TraceLog($"Info - [HandleAutoStartRMSGFiles]: {Path.GetFileName(filePath)} file loaded.");

                    if (filePath.EndsWith("autobroadcast.rmsg"))
                    {
                        TraceLog("Info - [HandleAutoStartRMSGFiles]: autobroadcast.rmsg file detected. Starting scheduled broadcast.");
                        dontPromptClosureMessage = true;
                        RunScheduledBroastcast(filePath);
                    }
                }
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
            TraceLog("Info - [ScheduledBroadcast]: Starting scheduled broadcast...");
            //(1) [FIXME] 21/07/24 - Why is this function only running the PC module? It should check if the other modules are enabled and run them as well. TODO: Fix this.
            LoadAndParseRMSGFile(RMSGFile); //Load the RMSG file into the program.
            //Check if modules are selected. If not, close the program.
            if (!MessagePCcheckBox.Checked && !MessageEmailcheckBox.Checked && !PSExecModuleEnableCheckBox.Checked)
            {
                TraceLog("Critical - [ScheduledBroadcast]: No modules are selected. Closing program.");
                Application.Exit();
            }
            //Check if message or pclist is empty. If it is, close the program.
            if (PCBroadcastMessageTxt.Text == "" || PCBroadcastToList.Text == "")
            {
                TraceLog("Critical - [ScheduledBroadcast]: Message or PC list is empty. Closing program.");
                Application.Exit();
            }
            int totalSeconds = ((int)PCexpiryHourTime.Value * 3600) + ((int)PCexpiryMinutesTime.Value * 60) + (int)PCexpirySecondsTime.Value; //Calculate the total seconds from the hours, minutes and seconds for the message duration.
            await
            broadcastController.StartPCBroadcastModule(PCBroadcastMessageTxt.Text, PCBroadcastToList.Text, totalSeconds, FastBroadcastModeCheckbox.Checked, ReattemptOnErrorCheckbox.Checked, DontSaveBroadcastHistoryCheckbox.Checked, dontPromptClosureMessage);
            TraceLog("Info - [ScheduledBroadcast]: Scheduled broadcast completed.");
            CloseAfterAllModulesAreFinished();
        }

        private void CheckSystemState()
        {
            var systemChecker = new SystemHealthCheck(TraceLog);
            TraceLog("Info - [CheckSystemState]: Checking system state...");
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
            TraceLog("Info - [CheckSystemState]: System state check completed.");
        }
        public void CheckForUpdates()
        {
            //Check for RMC program updates
            TraceLog("Info - [RMCUpdate]: Checking for RMC program updates...");
            if (RMCUpdateChecker.CheckForUpdates())
            {
                TraceLog("Info - [RMCUpdate]: An update is available. Please download the latest version from the GitHub repository to ensure stability.");
                MessageBox.Show("An update is available. Please download the latest version from the GitHub repository to ensure stability.", "Update Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                TraceLog("Info - [RMCUpdate]: No RMC updates available or no connection is detected.");
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
                TraceLog($"Error - [AddIconsToTabControls]: Failed to add images to the GUI form tabcontrol: {ex}");
            }
        }

        public void TraceLog(string item)
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
                    logList.Invoke(new System.Windows.Forms.MethodInvoker(() => TraceLog(item))); //This was added to prevent crashing when there are multiple threads trying to access the listbox.
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
                    Console.WriteLine($"Critical - [TraceLog] Error! An exception occurred when adding an item to the loglist: {ex}");
                    //Attempt to write the exception message to a file using file.writealltext.
                    File.WriteAllText($"{Application.StartupPath}\\RMC Runtime Logs\\RMC_Critical_Exception.txt", $"Critical - [AddTextToLogList] Error! An exception occurred when adding an item to the loglist: {ex}");
                }
                catch
                {
                    SaveRMCRuntimeLogBtn_Click(this, EventArgs.Empty); //Attempt Save the loglist to a file.
                    throw new Exception("RMC has encountered a critical error. Please close RMC and check the RMC log list for more information about what went wrong.");
                }
            }
        }
        //private void LogList_DrawItem(object sender, DrawItemEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Index < 0) return;

        //        ListBox listBox = sender as ListBox;
        //        string itemText = listBox.Items[e.Index].ToString();
        //        Color textColor = Color.Black; // Default color

        //        // Determine color based on log level
        //        if (itemText.Contains("Critical")) textColor = Color.Red;
        //        else if (itemText.Contains("Error")) textColor = Color.OrangeRed;
        //        else if (itemText.Contains("Warning")) textColor = Color.Orange;
        //        else if (itemText.Contains("Notice")) textColor = Color.Blue;
        //        else if (itemText.Contains("Info")) textColor = Color.Green;

        //        // Draw background
        //        e.DrawBackground();

        //        // Draw text with color
        //        using (Brush brush = new SolidBrush(textColor))
        //        {
        //            e.Graphics.DrawString(itemText, e.Font, brush, e.Bounds);
        //        }

        //        e.DrawFocusRectangle(); // Optional, draws focus rectangle when selected
        //    }
        //    catch (Exception ex)
        //    {
        //        TraceLog($"Error - [{MethodBase.GetCurrentMethod()?.Name ?? "DrawItem"}] Failure in drawing log list item. {ex.Message}");
        //    }
        //}

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
            TraceLog($"Info - [LoadGlobalSettings]: {Properties.Settings.Default.Properties.Count} Program settings loaded.");
        }
        private static void SetCheckboxState(CheckBox checkBox, string value)
        {
            checkBox.Checked = value == "True";
        }
        private void LoadAndParseRMSGFile(string filePath)
        {
            TraceLog($"Info - [LoadAndParseRMSGFile]: Loading RMSG file: {Path.GetFileName(filePath)}");
            string[] RMSGFileValues = IOManager.LoadRMSGFile(filePath);

            if (!string.IsNullOrEmpty(RMSGFileValues[0]))
            {
                //If first word is error, addtext and return. If warning, addtext but continue.
                if (RMSGFileValues[0].StartsWith("Warning"))
                {
                    TraceLog($"Warning - [LoadAndParseRMSGFile]: Loading message returned a non-critical warning: {RMSGFileValues[0]} - Continuing anyway.");
                }
                else if (RMSGFileValues[0].StartsWith("Error"))
                {
                    MessageBox.Show("A RMSG file error has occurred. Please check the debug log for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TraceLog($"Error - [LoadAndParseRMSGFile]: Loading message returned an error: {RMSGFileValues[0]} - Loading of RMSG file halted.");
                    return;
                }
                else
                {
                    MessageBox.Show("An unknown file load error has occurred. Please check the debug log for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TraceLog($"Error - [LoadAndParseRMSGFile]: Loading message returned an unknown error: {RMSGFileValues[0]} - Loading of RMSG file halted.");
                    return;
                }
            }
            TraceLog($"Info - [LoadAndParseRMSGFile]: Starting to parse RMSG file: {Path.GetFileName(filePath)}");
            try
            {
                //Check RMCSoftwareVersion, if it doesn't match the current version, then show a message box.
                if (RMSGFileValues[20] != versionNumb)
                {
                    MessageBox.Show($"This RMSG file was created with a different version of RMC (Expected Version: {versionNumb}, Actual File Version: {RMSGFileValues[20]}). RMC will attempt to load this file, and once the file is loaded you can save this file again to convert this save to one that is compatible with the current version of RMC.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TraceLog($"Info - [LoadAndParseRMSGFile]: RMSG file was created with a different version of RMC. RMSG resave might be required. RMC Version: {versionNumb}, RMSG Version: {RMSGFileValues[20]}. Attempting to load...");
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
                try
                {
                    if (EmailPasswordTextbox.Text != "")
                    {
                        EmailPasswordTextbox.Text = DecryptData(RMSGFileValues[19]);
                    }
                }
                catch (Exception ex)
                {
                    TraceLog($"Warning - [LoadAndParseRMSGFile]: Failed to decrypt email password: {ex}");
                }
                TraceLog($"Info - [LoadAndParseRMSGFile]: RMSG File loaded successfully: {Path.GetFileName(filePath)}");
            }
            catch (FormatException ex1)
            {
                TraceLog($"Error - [LoadAndParseRMSGFile]: Format Parse Failure - Format exception when loading RMSG file, RMSG file is not loading correctly! {ex1}");
                MessageBox.Show("A RMSG format error has occurred. Please check the debug log for file errors.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadGlobalSettings(); //If the RMSG file fails to load, then load the global settings instead.
                return;
            }
            catch (Exception ex)
            {
                TraceLog($"Error - [LoadAndParseRMSGFile]: General Parse Failure: {ex} - Will now load global settings.");
                MessageBox.Show("A general RMSG parse error has occurred. Please check the debug log for file errors.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                string[] files = Directory.GetFiles(Application.StartupPath + "\\RMSG Files\\");
                foreach (string file in files)
                {
                    QuickLoadListbox.Items.Add(Path.GetFileName(file));
                }
                //Add to loglist that the RMSG files have been loaded. with the amount of files.
                TraceLog($"Info - [RefreshRMSGFileList]: RMSG list refreshed. Amount of files on the list: {files.Length}");
            }
            catch (Exception ex)
            {
                TraceLog($"Error - [RefreshRMSGFileList]: Failure in creating QuickLoad folder(s)/Refreshing the RMSGFileList {ex}");
            }
        }

        //End of the functions, start of the events.
        #endregion Functions
        private void ActiveDirectorySelectBtn_Click(object sender, EventArgs e)
        {
            //If possible, I will create a form to select the OU's from the Active Directory. For now, it will just add all computers from the Computers OU.
            //--Connect to the Active Directory, show a form window with all the available OU's, let the user select one or all. Then add all the computers from the OU that the user selected to the pclist.--
            TraceLog("Info - [Active Directory]: Attempting to connect to Active Directory.");
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
                TraceLog("Info - [Active Directory] Computers from Active Directory added to the list.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error connecting to Active Directory: {ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TraceLog($"Error - [Active Directory]: Failure in connecting to Active Directory. {ex}");
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
                TraceLog($"Info - [SaveRMSGBtn]: Saving RMSG file: {fileName}");
                Enum.TryParse(EmailAuthTypecomboBox.Text, out AuthMode authMode);
                IOManager.SaveRMSGFile(fileName, PCBroadcastMessageTxt.Text, PCBroadcastToList.Text, WOLTextbox.Text, (int)WOLPortNumberBox.Value, PCexpiryHourTime.Value.ToString(), PCexpiryMinutesTime.Value.ToString(), PCexpirySecondsTime.Value.ToString(), FastBroadcastModeCheckbox.Checked, MessagePCcheckBox.Checked, MessageEmailcheckBox.Checked, PSExecModuleEnableCheckBox.Checked, ReattemptOnErrorCheckbox.Checked, DontSaveBroadcastHistoryCheckbox.Checked, AddressOfSMTPServerTxt.Text, EmailPortNumber.Value, SenderAddressTxt.Text, authMode, EmailAccountTextbox.Text, EmailPasswordTextbox.Text);
                RefreshRMSGFileList();
            }
        }

        private void OpenMessageTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new IOManager(TraceLog).OpenFileAndProcessContents(PCBroadcastMessageTxt, "OpenMessageAsTxt", "OpenMessageAsTxt", RegexFilters.FilterInvalidMessage);
        }

        private void OpenSendComputerListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new IOManager(TraceLog).OpenFileAndProcessContents(PCBroadcastToList, "OpenPCList", "OpenPCList", RegexFilters.FilterInvalidPCNames);
        }

        private void OpenMacAddressfromTxtBtn_Click(object sender, EventArgs e)
        {
            new IOManager(TraceLog).OpenFileAndProcessContents(WOLTextbox, "MacAddressfromTxt", "OpenMacAddressfromTxt", RegexFilters.FilterInvalidPCNames);
        }

        private void SaveMacAddressesAsTXTBtn_Click(object sender, EventArgs e)
        {
            new IOManager(TraceLog).SaveFileFromTextBox(WOLTextbox, "SaveMacAddressesAsTXTBtn", "SaveMacAddressesAsTXTBtn");
        }

        private void SaveComputerListBtn_Click(object sender, EventArgs e)
        {
            new IOManager(TraceLog).SaveFileFromTextBox(PCBroadcastToList, "SavePCList", "SavePCList");
        }

        private void SaveMessageBtn_Click(object sender, EventArgs e)
        {
            new IOManager(TraceLog).SaveFileFromTextBox(PCBroadcastMessageTxt, "SaveMessageBtn", "SaveMessageBtn");
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

        private void StartBroadcastBtn_Click(object sender, EventArgs e)
        {
            TraceLog("Info - [InitBroadcast]: Broadcast button pressed. Preparing to run selected modules...");

            bool isMessagePCChecked = MessagePCcheckBox.Checked;
            bool isMessageEmailChecked = MessageEmailcheckBox.Checked;
            bool isMessagePSExecChecked = PSExecModuleEnableCheckBox.Checked;

            if (!isMessagePCChecked && !isMessageEmailChecked && !isMessagePSExecChecked)
            {
                TraceLog("Error - [InitBroadcast]: No modules are enabled. Unable to broadcast.");
                MessageBox.Show("No modules are enabled. Please enable at least one module before starting the broadcast.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (isMessagePCChecked)
            {
                TraceLog("Info - [InitBroadcast]: PC module is enabled. Starting PC cast.");
                string messageText = PCBroadcastMessageTxt.Text;
                string computerSelectListText = PCBroadcastToList.Text;

                if (string.IsNullOrWhiteSpace(messageText) || string.IsNullOrWhiteSpace(computerSelectListText))
                {
                    MessageBox.Show("Message or PC list is empty. Please fill in the message and PC list before starting the broadcast.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TraceLog("Error - [InitBroadcast]: PC Broadcast - Message textbox or PC list are empty. Broadcast halted.");
                    return;
                }

                int totalSeconds = ((int)PCexpiryHourTime.Value * 3600) + ((int)PCexpiryMinutesTime.Value * 60) + (int)PCexpirySecondsTime.Value;
                _ = broadcastController.StartPCBroadcastModule(messageText, computerSelectListText, totalSeconds, FastBroadcastModeCheckbox.Checked, ReattemptOnErrorCheckbox.Checked, DontSaveBroadcastHistoryCheckbox.Checked, dontPromptClosureMessage);
            }

            if (isMessageEmailChecked)
            {
                TraceLog("Info - [InitBroadcast]: Email module is enabled. Starting email cast.");
                //MessageBox.Show("Email module is not implemented yet. This is a placeholder message.", "Email Module", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (isMessagePSExecChecked)
            {
                //Check if PSexec label contains (Disabled), if it does, then show a message box and return.
                if (PsExecLabel.Text.Contains("(Disabled)"))
                {
                    TraceLog("Error - [InitBroadcast]: PSExec module is disabled due to startup PsExec check failure. Unable to start PSExec cast.");
                    MessageBox.Show("PSExec module is disabled since it either doesn't exist or is not available to use. Please check if PSExec is present in the RMC root folder before starting a broadcast.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                TraceLog("Info - [InitBroadcast]: PSExec module is enabled and passed check. Starting PSExec cast.");
                //MessageBox.Show("PSExec module is not implemented yet. This is a placeholder message.", "PSExec Module", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            NotifyBroadcastStatus();
        }

        private void ClearLogBtn_Click(object sender, EventArgs e)
        {
            //ask the user if they are sure they want to clear the loglist. If yes, clear it.
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to clear the log list?", "Clear Log List", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                logList.Items.Clear();
                TraceLog("Info - [RMC Manager]: Loglist cleared.");
            }
        }

        private void LoadSelectedRMSGBtn_Click(object sender, EventArgs e)
        {
            //Get the name of the selected item on the listbox, and load it into the LoadRMSGFile function.
            string? selectedFile = QuickLoadListbox.SelectedItem?.ToString();
            if (selectedFile != null)
            {
                LoadAndParseRMSGFile(Path.Combine(Application.StartupPath, "RMSG Files", selectedFile));
            }
        }

        private void QuickSaveRMSGBtn_Clicked(object sender, EventArgs e)
        {
            //Save a quick save file name based on the current date and time.
            string quickSaveFileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".rmsg";

            TraceLog($"Info - [QuickSaveBtn]: Quick saving RMSG file: {quickSaveFileName}");
            //Use the SaveRMSGFile in the RMC_IO_Manager to save the file.
            Enum.TryParse(EmailAuthTypecomboBox.Text, out AuthMode authMode);

            string encryptedPassword;
            try
            {
                if (string.IsNullOrWhiteSpace(EmailPasswordTextbox.Text))
                {
                    encryptedPassword = string.Empty;
                }
                else
                {
                    encryptedPassword = EncryptData(EmailPasswordTextbox.Text);
                }
            }
            catch (Exception ex)
            {
                encryptedPassword = string.Empty; //Prevent saving the password if it fails to encrypt.
                TraceLog($"Warning - [SaveQuickRMSGFile]: Failed to encrypt email password. Saving without encrypted password. Exception: {ex.Message}");
            }

            IOManager.SaveRMSGFile(
                Path.Combine(Application.StartupPath, "RMSG Files", quickSaveFileName),
                PCBroadcastMessageTxt.Text,
                PCBroadcastToList.Text,
                WOLTextbox.Text,
                (int)WOLPortNumberBox.Value,
                PCexpiryHourTime.Value.ToString(),
                PCexpiryMinutesTime.Value.ToString(),
                PCexpirySecondsTime.Value.ToString(),
                FastBroadcastModeCheckbox.Checked,
                MessagePCcheckBox.Checked,
                MessageEmailcheckBox.Checked,
                PSExecModuleEnableCheckBox.Checked,
                ReattemptOnErrorCheckbox.Checked,
                DontSaveBroadcastHistoryCheckbox.Checked,
                AddressOfSMTPServerTxt.Text,
                EmailPortNumber.Value,
                SenderAddressTxt.Text,
                authMode,
                EmailAccountTextbox.Text,
                encryptedPassword
            );
            RefreshRMSGFileList();
        }

        private void RefreshRMSGListBtn_Click(object sender, EventArgs e)
        {
            TraceLog("Info - [RefreshRMSGBtn]: Refreshing RMSG file list.");
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
                        File.Delete(Path.Combine(Application.StartupPath, "RMSG Files", selectedFile));
                        TraceLog("Info - [DeleteSelectedRMSGFile]: File deleted: " + selectedFile);
                        RefreshRMSGFileList();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("File deletion error. Please check the debug log for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TraceLog($"Error - [DeleteSelectedRMSGFile]: Failure in deleting file: {ex}");
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
                    TraceLog("Error - [RenameSelectedRMSGBtn]: Error renaming file, invalid characters in the filename.");
                    MessageBox.Show("The file name contains invalid characters. Please enter a valid file name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Check if the file already exists and prompt for overwrite
                string filePath = Path.Combine(Application.StartupPath, "RMSG Files", newFileName);
                if (File.Exists(filePath))
                {
                    DialogResult dialogResult = MessageBox.Show("The file already exists. Do you want to overwrite it?", "Overwrite File", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    if (dialogResult == DialogResult.No)
                        return;
                    TraceLog($"Info - [RenameSelectedRMSGBtn]: File overwrite request approved on file: {newFileName}");
                }

                try
                {
                    // Move the file to the new location
                    File.Move(Path.Combine(Application.StartupPath, "RMSG Files", selectedFile), filePath, true);
                    TraceLog($"Info - [RenameSelectedRMSGBtn]: File renamed: {selectedFile} to {newFileName}");
                    RefreshRMSGFileList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error renaming file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TraceLog($"Error - [RenameSelectedRMSGBtn]: Failure in renaming file: {ex.Message}");
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
            MessageBox.Show("The RMSG file is a file that contains the configuration of the RMC program when it was saved. It's used to quickly send messages to a list of computers without having to type it out every time.\n\nIf you save a RMSG file as default.rmsg, it will load it automatically when the program starts. And if you save a RMSG file as autobroadcast.rmsg, if a person or program opens RMC again, it will automatically broadcast all the contents of autobroadcast.rmsg.");
        }

        private void RMCManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                TraceLog($"Info - [RMC Manager]: Program closure has been triggered. (Closure Type: {dontPromptClosureMessage}).");
                // If isScheduledBroadcast is true, close the program without asking.
                if (dontPromptClosureMessage)
                {
                    if (!broadcastController.AreAnyModulesRunning())
                    {
                        TraceLog("Info - [RMC Manager]: Scheduled programming has completed. closing...");
                        return;
                    }
                    else
                    {
                        TraceLog("Warning - [RMC Manager]: Scheduled programming is still running. Closing the program will stop the broadcast. Rejecting close request and checking again in 10 seconds...");
                        e.Cancel = true;
                        //retry the close after 10 seconds.
                        Task.Delay(10000).ContinueWith(_ => CloseAfterAllModulesAreFinished());
                        return;
                    }
                }
                if (broadcastController.AreAnyModulesRunning()) // User might have clicked close on accident since there is a broadcast in progress. Confirm here.
                {
                    TraceLog("Warning - [RMC Manager]: Broadcast is still running. Closing the program will stop the broadcast. Confirming close request.");
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
                TraceLog("Info - [RMC Manager]: Saving log file and closing... [END OF LOG]");

                //Reenable this once built, is it just writing loads of logs to my disk when im constantly testing. so i have disabled it for now.
                SaveRMCRuntimeLogBtn_Click(sender, e);
            }
        }

        private void ComputerSelectList_TextChanged(object sender, EventArgs e)
        {
            //For each line, count it on the PCCountLBL.
            int pcCount = PCBroadcastToList.Lines.Length;
            PCCountLbl.Text = $"PC Count: {pcCount}";
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
            IOManager.OpenLinkOrFolder("https://icons8.com/");
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
            TraceLog("Info - [ScheduleBroadcast]: Opening the schedule broadcast form.");
            ScheduleBroadcastForm scheduleBroadcastForm = new();
            scheduleBroadcastForm.ShowDialog();
        }

        private void BroadcastHistoryBtn_Click(object sender, EventArgs e)
        {
            TraceLog("Info - [BroadcastHistory]: Opening the broadcast history form.");
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
                TraceLog($"Info - [RuntimeSaveLog]: Runtime log saved to file: {logFileName}");
            }
            catch (Exception ex)
            {
                TraceLog($"Error - [RuntimeSaveLog]: Failure in saving runtime log. {ex}");
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
            //Check if scheduled broadcast is true. If it is, hide the form.
            if (dontPromptClosureMessage)
            {
                Hide();
            }
        }

        private async void TestBroadcastMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Broadcast the message to just the computer that the program is running on.
            TraceLog("Info - [TestBroadcastMessage]: Testing the broadcast message to the local computer.");
            if (PCBroadcastMessageTxt.Text == "")
            {
                MessageBox.Show("Message is empty. Please fill in the message before starting the broadcast.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TraceLog("Error - [TestBroadcastMessage]: Message is empty. Broadcast halted.");
                return;
            }
            int totalSeconds = ((int)PCexpiryHourTime.Value * 3600) + ((int)PCexpiryMinutesTime.Value * 60) + (int)PCexpirySecondsTime.Value; //Calculate the total seconds from the hours, minutes and seconds for the message duration.
            //pcBroadcastModule.BroadcastPCMessage(MessageTxt.Text, Environment.MachineName, totalSeconds, false, EmergencyModeCheckbox.Checked, ReattemptOnErrorCheckbox.Checked, DontSaveBroadcastHistoryCheckbox.Checked, isScheduledBroadcast);
            await
            broadcastController.StartPCBroadcastModule(PCBroadcastMessageTxt.Text, Environment.MachineName, totalSeconds, FastBroadcastModeCheckbox.Checked, ReattemptOnErrorCheckbox.Checked, DontSaveBroadcastHistoryCheckbox.Checked, dontPromptClosureMessage);
        }

        private async void SendWOLPacketBtn_Click(object sender, EventArgs e)
        {
            TraceLog("Info - [SendWOLPacket]: Sending WOL packet to the PC's in the list.");
            //Send a WOL packet to all mac addresses in the WOLTextbox. But first check if the textbox is empty or contains invalid mac addresses.
            string[] macAddresses = WOLTextbox.Text.Split(PCseparatorArray, StringSplitOptions.RemoveEmptyEntries);
            if (macAddresses.Length == 0)
            {
                MessageBox.Show("No MAC addresses in the textbox. Please add MAC addresses before sending a WOL packet.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TraceLog("Error - [SendWOLPacket]: No MAC addresses in the textbox. Please add MAC addresses before sending a WOL packet.");
                return;
            }
            foreach (string macAddress in macAddresses)
            {
                if (!WakeOnLANManager.IsValidMacAddress(macAddress))
                {
                    //MessageBox.Show("Invalid MAC address: " + macAddress, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TraceLog($"Error - [SendWOLPacket]: Invalid MAC address: {macAddress}");
                    return;
                }
                TraceLog($"Info - [SendWOLPacket]: Sending WOL packet to MAC address: {macAddress}");
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
            IOManager.OpenLinkOrFolder("RMC Runtime Logs");
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
            //Ask before renewing the IP address of the computer.
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to renew the IP address of the computer?", "Renew IP Address", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.No)
            {
                return;
            }
            TraceLog("Info - [RenewIPBtn]: Renewing the IP address of the computer.");
            SystemServiceManager.ReleaseAndRenewIP();
        }

        private void ShowRMSGListBtn_Click(object sender, EventArgs e)
        {
            MainSplitContainer.Panel1Collapsed = false;
            ShowRMSGListBtn.Visible = false;
        }

        private void HideQuickLoadBtn_Click(object sender, EventArgs e)
        {
            MainSplitContainer.Panel1Collapsed = true;
            ShowRMSGListBtn.Visible = true;
        }

        private void OpenSaveLocationBtn_Click(object sender, EventArgs e)
        {
            IOManager.OpenLinkOrFolder("RMSG Files");
        }

        private void RMCManager_Resize(object sender, EventArgs e)
        {
            // Determine the new font size based on the form's width or height
            float newFontSize = Math.Max(9, Math.Min(this.ClientSize.Width / 40, this.ClientSize.Height / 40));
            // Apply the new font size
            PCBroadcastMessageTxt.Font = new Font(PCBroadcastMessageTxt.Font.FontFamily, newFontSize);
            //TraceLog($"Info - [RMC Manager]: Resized the form. New font size: {newFontSize}");
        }

        private void EmailEditBtn_Click(object sender, EventArgs e)
        {
            //Open EmailEditor form. But wait for it to close, if the form returns a file path, put that path in the SelectedEmailFileTxtbox.
            EmailEditor emailEditorForm = new();
            if (emailEditorForm.ShowDialog() == DialogResult.OK)
            {
                AddressOfSMTPServerTxt.Text = emailEditorForm.SelectedEmailFile;
            }
        }

        private void SelectRMCEmailFileBtn_Click(object sender, EventArgs e)
        {
            //Open file dialog to select a file. If the file is selected, put the file path in the SelectedEmailFileTxtbox. with Filter of .RMCEmail
            using OpenFileDialog openFileDialog = new()
            {
                InitialDirectory = Application.StartupPath + "Email Files",
                Filter = "RMC Email Files (*.RMCEmail)|*.RMCEmail|All Files (*.*)|*.*"
            };
            //If the file is selected, put the file path in the SelectedEmailFileTxtbox.
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                AddressOfSMTPServerTxt.Text = openFileDialog.FileName;
            }
        }
    }
}