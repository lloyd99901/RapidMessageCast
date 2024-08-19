//--RapidMessageCast Software--
//HistoryHandler.cs - RapidMessageCast Manager

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
    internal class HistoryManager
    {
        private readonly Dictionary<RMCEnums, List<string>> broadcastHistoryBuffers = new()
        {
            { RMCEnums.PC, new List<string>() },
            { RMCEnums.Email, new List<string>() },
            { RMCEnums.PSExec, new List<string>() }
        };

        public void AddToHistory(RMCEnums module, string message)
        {
            if (broadcastHistoryBuffers.ContainsKey(module))
            {
                broadcastHistoryBuffers[module].Add($"{DateTime.Now:MM-dd-yyyy HH:mm:ss} - {module}: {message}");
            }
        }

        public void SaveBroadcastHistory(bool dontSave, RMCEnums module)
        {
            if (Application.OpenForms.Count == 0 || Application.OpenForms[0] is not RMCManager RMCManagerForm)
            {
                MessageBox.Show("Fatal Error - HistoryHandler has reported a critical error, it is recommended that you restart RapidMessageCast. Details: Error with communicating with RMCManagerForm while attempting to save broadcast history. RMCManagerForm reported as null.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dontSave)
            {
                RMCManagerForm.TraceLog("Info - [HistoryHandler]: Broadcast history save halted. Don't save history checkbox is checked.");
                return;
            }

            string broadcastHistoryFileName = $"{module}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt";
            string directoryPath = Path.Combine(Application.StartupPath, "Broadcast History Logs");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string filePath = Path.Combine(directoryPath, broadcastHistoryFileName);

            try
            {
                if (broadcastHistoryBuffers.ContainsKey(module))
                {
                    File.WriteAllLines(filePath, broadcastHistoryBuffers[module]);
                    RMCManagerForm.TraceLog($"Info - [HistoryHandler]: History saved to file: {broadcastHistoryFileName}");
                }
            }
            catch (Exception ex)
            {
                RMCManagerForm.TraceLog($"Error - [HistoryHandler]: Failure in saving broadcast history. {ex}");
                if (broadcastHistoryBuffers.ContainsKey(module))
                {
                    RetrySaveBroadcastHistory(broadcastHistoryBuffers[module], filePath, RMCManagerForm);
                }
            }
            ClearHistoryBuffer(module);
        }

        private void ClearHistoryBuffer(RMCEnums module)
        {
            if (broadcastHistoryBuffers.ContainsKey(module))
            {
                broadcastHistoryBuffers[module].Clear();
            }
        }

        private static void RetrySaveBroadcastHistory(List<string> broadcastHistoryBuffer, string filePath, RMCManager RMCManagerForm)
        {
            int retryDelay = new Random().Next(1000, 5000);
            Thread.Sleep(retryDelay);

            try
            {
                File.WriteAllLines(filePath, broadcastHistoryBuffer);
                RMCManagerForm.TraceLog($"Info - [HistoryHandler]: History saved to file after retry: {Path.GetFileName(filePath)}");
            }
            catch (Exception ex)
            {
                RMCManagerForm.TraceLog($"Error - [HistoryHandler]: Failure in saving broadcast history after retry. {ex}");
            }
        }
    }
}
