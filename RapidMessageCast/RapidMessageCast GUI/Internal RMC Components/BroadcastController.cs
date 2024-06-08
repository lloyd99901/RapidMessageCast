using RapidMessageCast_Manager.BroadcastModules;

//--RapidMessageCast Software--
//BroadcastController.cs - RapidMessageCast Manager
//Used to control and watch over the broadcast modules. This class is used to start and stop the modules, as well as check if any are running.

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
    internal class BroadcastController
    {
        private static readonly Dictionary<RMCEnums, bool> moduleRunning = new()
        {
            { RMCEnums.PC, false },
            { RMCEnums.Email, false },
            { RMCEnums.PSExec, false }
        };

        private readonly PCBroadcastModule pcBroadcastModule = new();

        public bool AreAnyModulesRunning() => moduleRunning.Values.Any(status => status); //I duplicated this bool because it's the simplest way to get around an issue where RMC manager couldn't access this.
        public static bool AreAnyModulesRunningPrivate() => moduleRunning.Values.Any(status => status);

        public async Task StartBroadcastModule(RMCEnums module, string message = "", string computerList = "", int totalSeconds = 0, bool emergencyMode = false, bool reattemptOnError = false, bool dontSaveBroadcastHistory = false, bool isScheduledBroadcast = false)
        {
            if (Application.OpenForms.Count == 0 || Application.OpenForms[0] is not RMCManager RMCManagerForm)
            {
                MessageBox.Show("Fatal Error - StartBroadcastModule has reported a critical error, it is recommended that you restart RapidMessageCast. BroadcastController reported as null.");
                return;
            }

            RMCManagerForm.AddTextToLogList($"Info - [BroadcastController]: Starting Broadcast Module: {module}...");

            switch (module)
            {
                case RMCEnums.PC:
                    pcBroadcastModule.BroadcastPCMessage(message, computerList, totalSeconds, false, emergencyMode, reattemptOnError, dontSaveBroadcastHistory, isScheduledBroadcast);
                    moduleRunning[RMCEnums.PC] = true;
                    break;
                case RMCEnums.Email:
                    //Insert Email Module Here
                    moduleRunning[RMCEnums.Email] = true;
                    break;
                case RMCEnums.PSExec:
                    //Insert PSExec Module Here
                    moduleRunning[RMCEnums.PSExec] = true;
                    break;
                default:
                    RMCManagerForm.AddTextToLogList($"Error - [BroadcastController] - An error occurred while trying to set the module running status to BroadcastController. The module specified was not found. Module name that was attempted: {module}");
                    break;
            }
            await RunModuleWatchdog();
        }

        public static void SetStatusOfBroadcastModule(RMCEnums module, bool running) //I love static methods. not really.
        {
            if (Application.OpenForms.Count == 0 || Application.OpenForms[0] is not RMCManager RMCManagerForm)
            {
                MessageBox.Show("Fatal Error - SetStatusOfBroadcastModule has reported a critical error, it is recommended that you restart RapidMessageCast. BroadcastController reported as null.");
                return;
            }

            RMCManagerForm.AddTextToLogList($"Info - [BroadcastController]: Setting status of {module} module to the following state: {running}...");

            switch (module)
            {
                case RMCEnums.PC:
                    moduleRunning[RMCEnums.PC] = running;
                    break;
                case RMCEnums.Email:
                    moduleRunning[RMCEnums.Email] = running;
                    break;
                case RMCEnums.PSExec:
                    moduleRunning[RMCEnums.PSExec] = running;
                    break;
                default:
                    RMCManagerForm.AddTextToLogList("Error - [BroadcastController] - An error occurred while trying to set the module running status to BroadcastController. The module specified was not found.");
                    break;
            }
        }

        private static async Task RunModuleWatchdog()
        {
            if (Application.OpenForms.Count == 0 || Application.OpenForms[0] is not RMCManager RMCManagerForm)
            {
                MessageBox.Show("Fatal Error - RunModuleWatchdog has reported a critical error, it is recommended that you restart RapidMessageCast.");
                return;
            }

            int timeout = 180000; // 3 minutes in milliseconds
            using var cts = new CancellationTokenSource(timeout);

            try
            {
                await Task.Run(() =>
                {
                    while (AreAnyModulesRunningPrivate())
                    {
                        Thread.Sleep(1000);

                        if (cts.Token.IsCancellationRequested)
                        {
                            break;
                        }
                    }
                }, cts.Token);

                if (cts.Token.IsCancellationRequested)
                {
                    RMCManagerForm.AddTextToLogList("Warning - [RunModuleWatchdog] - A hung broadcast module has been detected. Alerting user.");
                    MessageBox.Show("The broadcast watchdog has detected that a broadcast module is hung.", "RapidMessageCast - Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    RMCManagerForm.AddTextToLogList("Info - [RunModuleWatchdog] - All broadcast modules have finished. RMC Program is now idle.");
                }
            }
            catch (TaskCanceledException)
            {
                RMCManagerForm.AddTextToLogList("Warning - [RunModuleWatchdog] - A task canceled exception has been been detected. Alerting user.");
                MessageBox.Show("The broadcast watchdog has thrown a task cancelled exception. This means a broadcast module is hung.", "RapidMessageCast - Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

}
