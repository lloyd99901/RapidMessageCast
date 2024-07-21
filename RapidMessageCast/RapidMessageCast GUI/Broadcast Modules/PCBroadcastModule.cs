using RapidMessageCast_Manager.Internal_RMC_Components;
using System.Diagnostics;

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

namespace RapidMessageCast_Manager.BroadcastModules
{
    internal class PCBroadcastModule
    {
        //readonly List<string> broadcastHistoryBuffer = []; //Buffer for the broadcast history. This will be saved to a file after the broadcast has finished.
        private static readonly char[] PCseparatorArray = ['\n', '\r']; //Used for PCList parsing.
        readonly HistoryManager broadcastHistoryHandler = new(); //Create a new instance of the broadcastHistoryHandler class.
        public void BroadcastPCMessage(string message, string PCList, int duration, bool HasThisBeenReattempted, bool emergencyMode, bool isReattemptOnErrorChecked, bool isDontSaveBroadcastHistoryChecked, bool isScheduledBroadcast)
        {
            if (Application.OpenForms.Count == 0 || Application.OpenForms[0] is not RMCManager RMCManagerForm) //If this happens, something went really wrong here...
            {
                MessageBox.Show("Fatal Error - PCBroadcastModule has reported a critical error, it is recommeneded that you restart RapidMessageCast. Details: Error with communicating with RMCManagerForm while attempting to broadcast. RMCManagerForm reported as null.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            RMCManagerForm.AddTextToLogList($"Info - [PCBroadcastModule]: PC Broadcast has been started. Started by: {Environment.UserName} - System Name: {Environment.MachineName}");
            //broadcastHistoryBuffer.Clear();
            //Broadcast message to all PCs
            //Check if msg.exe exists in the system32 folder. If not, display a message to the user.
            if (!File.Exists("C:\\Windows\\System32\\msg.exe"))
            {
                RMCManagerForm.AddTextToLogList("Critical - [PCBroadcastModule]: An attempt to broadcast was made but msg.exe was not found in the System32 folder. Please ensure that you have a supported operating system.");
                MessageBox.Show("msg.exe not found in the System32 folder. Please ensure that you have a supported operating system.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //broadcastHistoryBuffer.Clear();
            broadcastHistoryHandler.AddToHistory(RMCEnums.PC, $"===RapidMessageCast=== - Version: {RMCManagerForm.versionNumb}");
            broadcastHistoryHandler.AddToHistory(RMCEnums.PC, $"START - Broadcast has started. Broadcast started by: {Environment.UserName} - System Name: {Environment.MachineName}");
            broadcastHistoryHandler.AddToHistory(RMCEnums.PC, $"Message - Message Content: {message}");
            broadcastHistoryHandler.AddToHistory(RMCEnums.PC, $"Duration - Message Duration: {duration} seconds");
            broadcastHistoryHandler.AddToHistory(RMCEnums.PC, $"Reattempt on Failure - : {isReattemptOnErrorChecked}");
            broadcastHistoryHandler.AddToHistory(RMCEnums.PC, $"Emergency Mode - Emergency Mode: {emergencyMode}");
            broadcastHistoryHandler.AddToHistory(RMCEnums.PC, $"Save Broadcast History - Save Broadcast History: {isDontSaveBroadcastHistoryChecked}");
            broadcastHistoryHandler.AddToHistory(RMCEnums.PC, "============================================");
            //Check if isScheduledBroadcast is true. If it is, add to the broadcast history that it's a scheduled broadcast.
            if (isScheduledBroadcast)
            {
                broadcastHistoryHandler.AddToHistory(RMCEnums.PC, "Info - Scheduled broadcast has started.");
            }
            //Add if emergency mode is enabled to the broadcast history.
            if (emergencyMode)
            {
                //add to loglist that emergency mode is enabled.
                RMCManagerForm.AddTextToLogList("Notice - [PCBroadcastModule]: Emergency mode is enabled. RMC will not wait for the msg processes to exit.");
                broadcastHistoryHandler.AddToHistory(RMCEnums.PC, "Notice - Emergency mode is enabled. RMC will not wait for the msg processes to exit.");
            }
            string[] pcNames = PCList.Split(PCseparatorArray, StringSplitOptions.RemoveEmptyEntries);
            //Set StartBroadcastBtn text to Starting broadcast.
            foreach (string pcName in pcNames)
            {
                Task.Run(() =>
                {
                    try
                    {
                        //RMCManagerForm.AddTextToLogList($"Info - [PCBroadcastModule]: Preparing to message PC: {pcName} ...");
                        var process = StartMsgProcess(pcName, message, duration);
                        //Check if it returns a blank process. If it does, add an error to the broadcast history.
                        if (process.Id == 0)
                        {
                            broadcastHistoryHandler.AddToHistory(RMCEnums.PC, $"Error - [PCBroadcastModule]: The process did not start correctly for PC: {pcName} [Process ID was 0]. Continuing to the next one...");
                            RMCManagerForm.AddTextToLogList($"Error - [PCBroadcastModule]: The process did not start correctly for PC: {pcName}");
                        }
                        else
                        {
                            RMCManagerForm.AddTextToLogList($"Info - [PCBroadcastModule]: MSG process started for \"{pcName}\". (Process ID: {process.Id})");
                        }

                        //Check if emergency mode is enabled. if it is, do not wait for the process to exit.
                        if (!emergencyMode)
                        {
                            //Check if the program exits without any errors.
                            if (!process.WaitForExit(1500))
                            {
                                //Add the error to the broadcast history.
                                broadcastHistoryHandler.AddToHistory(RMCEnums.PC, $"Error - [PCBroadcastModule]: The process did not exit in time for PC: {pcName}");
                                RMCManagerForm.AddTextToLogList($"Error - [PCBroadcastModule]: The process did not exit in time for PC: {pcName}");
                            }
                            else
                            {
                                //Add the success to the broadcast history.
                                broadcastHistoryHandler.AddToHistory(RMCEnums.PC, $"Info - [PCBroadcastModule]: SUCCESS! MSG.exe process exited within allocated timelimit: {pcName}");
                                RMCManagerForm.AddTextToLogList($"Info - [PCBroadcastModule]: SUCCESS! MSG.exe process exited within allocated timelimit: {pcName}");
                            }
                        }
                        else
                        {
                            //Add PC name to the broadcast history. But write unknown if message was sent or not.
                            broadcastHistoryHandler.AddToHistory(RMCEnums.PC, $" - Unknown if successful - MSG process started for \"{pcName}\" - Process ID: {process.Id}");
                        }
                    }
                    catch (Exception ex)
                    {
                        broadcastHistoryHandler.AddToHistory(RMCEnums.PC, $"Critical - [PCBroadcastModule]: Broadcast module reported an error. Failure to send command for PC: {pcName} | Error Details: {ex}");
                        RMCManagerForm.AddTextToLogList($"Critical - [PCBroadcastModule]: Broadcast module reported an error. Failure to send command for PC: {pcName} | Error Details: {ex}");
                        RMCManagerForm.StartBroadcastBtn.BackColor = Color.DarkRed;
                        if (!HasThisBeenReattempted & isReattemptOnErrorChecked) //If the message has not been reattempted and the reattempt on error checkbox is enabled, reattempt the message.
                        {
                            broadcastHistoryHandler.AddToHistory(RMCEnums.PC, $"Info - [PCBroadcastModule]: Reattempting to message PC again: {pcName}");
                            RMCManagerForm.AddTextToLogList($"Warning - [PCBroadcastModule]: Reattempting to message PC again for a final time: {pcName}");
                            BroadcastPCMessage(message, pcName, duration, true, emergencyMode, isReattemptOnErrorChecked, isDontSaveBroadcastHistoryChecked, isScheduledBroadcast);
                        }
                    }
                });
            }
            WaitForMSGTasksToFinish(RMCManagerForm, isDontSaveBroadcastHistoryChecked, isScheduledBroadcast);
            //Set the start broadcast button to green and start the timer to change it back to the original color.
            RMCManagerForm.StartBroadcastBtn.BackColor = Color.Green;
            RMCManagerForm.GreenButtonTimer.Start();
        }

        private void WaitForMSGTasksToFinish(RMCManager RMCManagerForm, bool isDontSaveBroadcastHistoryChecked, bool isScheduledBroadcast)
        {
            //Wait for all processes that contain msg.exe to close before saving the broadcast history.
            //Create a var outside this loop that will be used to count the amount of msg.exe processes.
            int msgProcesses = 0;
            Task.Run(() =>
            {
                const int maxWaitTime = 120; // Maximum wait time in seconds
                int elapsedWaitTime = 0; // Elapsed wait time in seconds
                while (elapsedWaitTime < maxWaitTime)
                {
                    if (Process.GetProcessesByName("msg").Length == 0)
                    {
                        // Exit the loop if there are no "msg" processes
                        break;
                    }

                    Thread.Sleep(1000);
                    elapsedWaitTime++;
                    //Get the amount of msg processes running in count. Store it in msgProcesses.
                    msgProcesses = Process.GetProcessesByName("msg").Length;
                    RMCManagerForm.AddTextToLogList($"Info - [PCBroadcastModule]: RMC is now waiting for all MSG processes to close. There are {msgProcesses} msg processes running on this system. Elapsed time: {elapsedWaitTime} seconds. (max time: 120 seconds) If hung, msg processes will be terminated. ");

                    if (elapsedWaitTime >= maxWaitTime)
                    {
                        RMCManagerForm.AddTextToLogList("Warning - [PCBroadcastModule]: RMC detected a possible hung module. Exiting wait loop and attempting to terminate hung msg processes...");
                        broadcastHistoryHandler.AddToHistory(RMCEnums.PC, "Warning - RMC detected a possible hung module. Exiting wait loop and attempting to terminate hung msg processes...");
                        // Force terminate all msg processes.
                        foreach (var process in Process.GetProcessesByName("msg"))
                        {
                            process.Kill();
                        }
                    }
                }

                //Add end of broadcast to the broadcast history.
                broadcastHistoryHandler.AddToHistory(RMCEnums.PC, "END - PC Broadcast has ended.");
                RMCManagerForm.AddTextToLogList("Info - [PCBroadcastModule]: RMC detected no remaining MSG processes. Broadcast has finished. Saving broadcast log...");
                broadcastHistoryHandler.SaveBroadcastHistory(isDontSaveBroadcastHistoryChecked, RMCEnums.PC);
                BroadcastController.SetStatusOfBroadcastModule(RMCEnums.PC, false); //Tells the broadcast controller that the PC module is no longer running.

                if (isScheduledBroadcast)
                {
                    //Close the program if it's a scheduled broadcast.
                    RMCManagerForm.AddTextToLogList("Info - [PCBroadcastModule]: WaitForMSGTasksToFinish - Scheduled broadcast finished.");
                }
            });
        }

        static Process StartMsgProcess(string pcName, string message, int duration)
        {
            try
            {
                var processInfo = new ProcessStartInfo
                {
                    FileName = "C:\\Windows\\System32\\msg.exe",
                    Arguments = $"* /TIME:{duration} /SERVER:{pcName} \"{message}\"",
                    CreateNoWindow = true,
                    UseShellExecute = false
                };

                var process = new Process { StartInfo = processInfo };
                process.Start();
                return process;
            }
            catch
            {
                //Catch this error but don't pause the program since it may impact emergency broadcasts.
                return new Process();
            }
        }
    }
}
