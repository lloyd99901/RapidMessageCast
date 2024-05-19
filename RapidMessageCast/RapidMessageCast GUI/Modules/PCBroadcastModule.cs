//--RapidMessageCast Software--
//PCBroadcastModule.cs - RapidMessageCast Manager

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

using System.Diagnostics;

namespace RapidMessageCast_Manager.Modules
{
    internal class PCBroadcastModule
    {
        readonly RMC_IO_Manager RMC_IO_ManagerClass = new();
        readonly List<string> broadcastHistoryBuffer = []; //Buffer for the broadcast history. This will be saved to a file after the broadcast has finished.
        private static readonly char[] PCseparatorArray = ['\n', '\r']; //Used for PCList parsing.
        public void BroadcastPCMessage(string message, string PCList, int duration, bool HasThisBeenReattempted, bool emergencyMode, bool isReattemptOnErrorChecked, bool isDontSaveBroadcastHistoryChecked, bool isScheduledBroadcast)
        {
            RMCManager RMCManagerForm = (RMCManager)Application.OpenForms[0];
            //Check if RMCManagerForm is null. If it is, return an error.
            if (RMCManagerForm == null)
            {
                MessageBox.Show("Fatal error: The PCBroadcast module reported RMCManagerForm as NULL. Please restart the program.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            RMCManagerForm.AddTextToLogList("Info - BroadcastPCMessage: PC Broadcast has been started.");
            //Broadcast message to all PCs
            //Check if msg.exe exists in the system32 folder. If not, display a message to the user.
            if (!File.Exists("C:\\Windows\\System32\\msg.exe"))
            {
                RMCManagerForm.AddTextToLogList("Critical - BeginPCMessageCast: An attempt to broadcast was made but msg.exe was not found in the System32 folder. Please ensure that you have a supported operating system.");
                MessageBox.Show("msg.exe not found in the System32 folder. Please ensure that you have a supported operating system.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Clear BroadcastHistory list.
            broadcastHistoryBuffer.Clear();
            //add to broadcast history program name and version.
            broadcastHistoryBuffer.Add("===RapidMessageCast=== - Version: " + RMCManagerForm.versionNumb);
            broadcastHistoryBuffer.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - START - Broadcast has started.");
            broadcastHistoryBuffer.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - Broadcast started by: " + Environment.UserName + " - System Name: " + Environment.MachineName);
            //Add that broadcast has started to the loglist.
            RMCManagerForm.AddTextToLogList("Info - BeginPCMessageCast: PC Broadcast has been started.");
            //Check if isScheduledBroadcast is true. If it is, add to the broadcast history that it's a scheduled broadcast.
            if (isScheduledBroadcast)
            {
                broadcastHistoryBuffer.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - Info - Scheduled broadcast has started.");
            }
            //Add the Message to the broadcast history and also what user it was sent by.
            broadcastHistoryBuffer.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - Message - Message Content: " + message);
            //Also add the duration of the message to the broadcast history.
            broadcastHistoryBuffer.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - Duration - Message Duration: " + duration + " seconds");
            //Add if emergency mode is enabled to the broadcast history.
            if (emergencyMode)
            {
                //add to loglist that emergency mode is enabled.
                RMCManagerForm.AddTextToLogList("Notice - BeginPCMessageCast: Emergency mode is enabled. RMC will not wait for the msg processes to exit.");
                broadcastHistoryBuffer.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - Notice - Emergency mode is enabled. RMC will not wait for the msg processes to exit.");
            }
            string[] pcNames = PCList.Split(PCseparatorArray, StringSplitOptions.RemoveEmptyEntries);
            //Set StartBroadcastBtn text to Starting broadcast.
            foreach (string pcName in pcNames)
            {
                Task.Run(() =>
                {
                    try
                    {
                        RMCManagerForm.AddTextToLogList($"Info - BeginPCMessageCast: Preparing to message PC: {pcName} ...");
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
                        RMCManagerForm.AddTextToLogList($"Info - BeginPCMessageCast: MSG process started for \"{pcName}\". (Process ID: {process.Id})");

                        //Check if emergency mode is enabled. if it is, do not wait for the process to exit.
                        if (!emergencyMode)
                        {
                            //Check if the program exits without any errors.
                            if (!process.WaitForExit(1500))
                            {
                                //Add the error to the broadcast history.
                                broadcastHistoryBuffer.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - \"" + pcName + "\" - ERROR - The process did not exit in time.");
                                RMCManagerForm.AddTextToLogList($"Error - BeginPCMessageCast: The process did not exit in time for PC: {pcName}");
                            }
                            else
                            {
                                //Add the success to the broadcast history.
                                broadcastHistoryBuffer.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - \"" + pcName + "\" - SUCCESS - MSG.exe process exited within allocated timelimit.");
                                RMCManagerForm.AddTextToLogList($"Info - BeginPCMessageCast: SUCCESS! MSG.exe process exited within allocated timelimit: {pcName}");
                            }
                        }
                        else
                        {
                            //Add PC name to the broadcast history. But write unknown if message was sent or not.
                            broadcastHistoryBuffer.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - (Unknown if successful) MSG process started for \"" + pcName + "\" - Process ID:" + process.Id);
                        }
                    }
                    catch (Exception ex)
                    {
                        broadcastHistoryBuffer.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + pcName + " - ERROR - " + ex.ToString());
                        RMCManagerForm.AddTextToLogList($"Critical - BeginPCMessageCast: Broadcast module reported an error. Failure to send command for PC: {pcName} | Error Details: {ex}");
                        RMCManagerForm.StartBroadcastBtn.BackColor = Color.DarkRed;
                        if (!HasThisBeenReattempted & isReattemptOnErrorChecked) //If the message has not been reattempted and the reattempt on error checkbox is enabled, reattempt the message.
                        {
                            broadcastHistoryBuffer.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + pcName + " - Attempting to message PC again - ");
                            RMCManagerForm.AddTextToLogList($"Warning - BeginPCMessageCast: Reattempting to message PC again for a final time: {pcName}");
                            BroadcastPCMessage(message, pcName, duration, true, emergencyMode, isReattemptOnErrorChecked, isDontSaveBroadcastHistoryChecked, isScheduledBroadcast);
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
                broadcastHistoryBuffer.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - END - Broadcast has ended.");
                RMCManagerForm.AddTextToLogList("Info - BeginPCMessageCast: RMC detected no remaining MSG processes. Broadcast has finished. Saving broadcast log...");
                RMC_IO_Manager.SaveBroadcastHistory(broadcastHistoryBuffer, isDontSaveBroadcastHistoryChecked);
                if (isScheduledBroadcast)
                {
                    //Close the program if it's a scheduled broadcast.
                    RMCManagerForm.AddTextToLogList("Info - Automated Broadcast: Scheduled broadcast finished. Closing program.");
                    Application.Exit();
                }
                //Set StartBroadcastBtn text to Start Broadcast.
            });
            //Set the start broadcast button to green and start the timer to change it back to the original color.
            RMCManagerForm.StartBroadcastBtn.BackColor = Color.Green;
            RMCManagerForm.GreenButtonTimer.Start();
        }
    }
}
