//--RapidMessageCast Software--
//BroadcastHistoryHandler.cs - RapidMessageCast Manager

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


namespace RapidMessageCast_Manager.Internal_RMC_Components
{
    internal class BroadcastHistoryHandler
    {
        readonly List<string> broadcastHistoryBuffer = []; //Buffer for the broadcast history. This will be saved to a file after the broadcast has finished.

        public static void AddToHistory(RMCEnums Module, string message)
        {
            broadcastHistoryBuffer.Add($"{DateTime.Now:MM-dd-yyyy HH:mm:ss} - {Module}: {message}");
        }
        public void SaveBroadcastHistory(List<string> broadcastHistoryBuffer, bool dontSave)
        {
            if (Application.OpenForms.Count == 0 || Application.OpenForms[0] is not RMCManager RMCManagerForm) //If this happens, something went really wrong here...
            {
                MessageBox.Show("Fatal Error - RMC IO Manager has reported a critical error, it is recommeneded that you restart RapidMessageCast. Details: Error with communicating with RMCManagerForm while attempting to save broadcast history. RMCManagerForm reported as null.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Check if the other modules are still running. If they are, ignore this request. The last module to finish will save the history.
            if (RuntimeModuleManager.IsModuleRunning())
            {
                RMCManagerForm.AddTextToLogList("Info - [BroadcastWatchdog] - Broadcast History save was attempted, but other modules are running. Waiting for them to finish before saving history...");
                return;
            }

            if (dontSave) //If the user has checked the "Don't save history" checkbox, don't save the history.
            {
                RMCManagerForm.AddTextToLogList("Info - Broadcast: Broadcast history save halted. Don't save history checkbox is checked.");
                return;
            }

            string broadcastHistoryFileName = $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt";
            string directoryPath = Path.Combine(Application.StartupPath, "BroadcastHistory");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string filePath = Path.Combine(directoryPath, broadcastHistoryFileName);

            try
            {
                File.WriteAllLines(filePath, broadcastHistoryBuffer);
                RMCManagerForm.AddTextToLogList($"Info - Broadcast: History saved to file: {broadcastHistoryFileName}");
            }
            catch (Exception ex)
            {
                RMCManagerForm.AddTextToLogList($"Error - Broadcast: Failure in saving broadcast history. {ex}");
                RetrySaveBroadcastHistory(broadcastHistoryBuffer, filePath, RMCManagerForm);
            }
        }

        private static void RetrySaveBroadcastHistory(List<string> broadcastHistoryBuffer, string filePath, RMCManager RMCManagerForm)
        {
            int retryDelay = new Random().Next(1000, 5000);
            Thread.Sleep(retryDelay);

            try
            {
                File.WriteAllLines(filePath, broadcastHistoryBuffer);
                RMCManagerForm.AddTextToLogList($"Info - Broadcast: History saved to file after retry: {Path.GetFileName(filePath)}");
            }
            catch (Exception ex)
            {
                RMCManagerForm.AddTextToLogList($"Error - Broadcast: Failure in saving broadcast history after retry. {ex}");
            }
        }
    }
}
