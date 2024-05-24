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
        readonly List<string> PCbroadcastHistoryBuffer = []; //Buffer for the broadcast history. This will be saved to a file after the broadcast has finished.
        readonly List<string> EmailbroadcastHistoryBuffer = []; //Buffer for the broadcast history. This will be saved to a file after the broadcast has finished.
        readonly List<string> PSExecbroadcastHistoryBuffer = []; //Buffer for the broadcast history. This will be saved to a file after the broadcast has finished.

        public void AddToHistory(RMCEnums Module, string message)
        {
            //Depending on the module, add the message to the correct buffer.
            switch (Module)
            {
                case RMCEnums.PC:
                    PCbroadcastHistoryBuffer.Add($"{DateTime.Now:MM-dd-yyyy HH:mm:ss} - {Module}: {message}");
                    break;
                case RMCEnums.Email:
                    EmailbroadcastHistoryBuffer.Add($"{DateTime.Now:MM-dd-yyyy HH:mm:ss} - {Module}: {message}");
                    break;
                case RMCEnums.PSExec:
                    PSExecbroadcastHistoryBuffer.Add($"{DateTime.Now:MM-dd-yyyy HH:mm:ss} - {Module}: {message}");
                    break;
            }
        }
        public void SaveBroadcastHistory(bool dontSave, RMCEnums Module)
        {
            if (Application.OpenForms.Count == 0 || Application.OpenForms[0] is not RMCManager RMCManagerForm) //If this happens, something went really wrong here...
            {
                MessageBox.Show("Fatal Error - BroadcastHistoryHandler has reported a critical error, it is recommeneded that you restart RapidMessageCast. Details: Error with communicating with RMCManagerForm while attempting to save broadcast history. RMCManagerForm reported as null.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dontSave) //If the user has checked the "Don't save history" checkbox, don't save the history.
            {
                RMCManagerForm.AddTextToLogList("Info - [BroadcastHistoryHandler]: Broadcast history save halted. Don't save history checkbox is checked.");
                return;
            }
            //Set the filename to the enum and the date and time.
            string broadcastHistoryFileName = $"{Module}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt";
            string directoryPath = Path.Combine(Application.StartupPath, "BroadcastHistory");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string filePath = Path.Combine(directoryPath, broadcastHistoryFileName);

            try
            {
                //File.WriteAllLines(filePath, broadcastHistoryBuffer);
                switch (Module)
                {
                    case RMCEnums.PC:
                        File.WriteAllLines(filePath, PCbroadcastHistoryBuffer);
                        break;
                    case RMCEnums.Email:
                        File.WriteAllLines(filePath, EmailbroadcastHistoryBuffer);
                        break;
                    case RMCEnums.PSExec:
                        File.WriteAllLines(filePath, PSExecbroadcastHistoryBuffer);
                        break;
                }
                RMCManagerForm.AddTextToLogList($"Info - [BroadcastHistoryHandler]: History saved to file: {broadcastHistoryFileName}");
            }
            catch (Exception ex)
            {
                RMCManagerForm.AddTextToLogList($"Error - [BroadcastHistoryHandler]: Failure in saving broadcast history. {ex}");
                switch (Module)
                {
                    case RMCEnums.PC:
                        RetrySaveBroadcastHistory(PCbroadcastHistoryBuffer, filePath, RMCManagerForm);
                        break;
                    case RMCEnums.Email:
                        RetrySaveBroadcastHistory(EmailbroadcastHistoryBuffer, filePath, RMCManagerForm);
                        break;
                    case RMCEnums.PSExec:
                        RetrySaveBroadcastHistory(PSExecbroadcastHistoryBuffer, filePath, RMCManagerForm);
                        break;
                }
            }
            ClearHistoryBuffer(Module);
        }

        private void ClearHistoryBuffer(RMCEnums module)
        {
            switch (module)
            {
                case RMCEnums.PC:
                    PCbroadcastHistoryBuffer.Clear();
                    break;
                case RMCEnums.Email:
                    EmailbroadcastHistoryBuffer.Clear();
                    break;
                case RMCEnums.PSExec:
                    PSExecbroadcastHistoryBuffer.Clear();
                    break;
            }
        }

        private static void RetrySaveBroadcastHistory(List<string> broadcastHistoryBuffer, string filePath, RMCManager RMCManagerForm)
        {
            int retryDelay = new Random().Next(1000, 5000);
            Thread.Sleep(retryDelay);

            try
            {
                File.WriteAllLines(filePath, broadcastHistoryBuffer);
                RMCManagerForm.AddTextToLogList($"Info - [BroadcastHistoryHandler]: History saved to file after retry: {Path.GetFileName(filePath)}");
            }
            catch (Exception ex)
            {
                RMCManagerForm.AddTextToLogList($"Error - [BroadcastHistoryHandler]: Failure in saving broadcast history after retry. {ex}");
            }
        }
    }
}
