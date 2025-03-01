using RapidMessageCast_Manager.BroadcastModules;
using System.Diagnostics;

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

        public bool AreAnyModulesRunning() => moduleRunning.Values.Any(status => status); //I duplicated this bool because it's the simplest way to get around an issue where RMC manager couldn't access this.
        public static bool AreAnyModulesRunningPrivate() => moduleRunning.Values.Any(status => status);
        
        public async Task StartPCBroadcastModule(string message = "", string computerList = "", int totalSeconds = 0, bool emergencyMode = false, bool reattemptOnError = false, bool dontSaveBroadcastHistory = false, bool isScheduledBroadcast = false)
        {
            if (Application.OpenForms.Count == 0 || Application.OpenForms[0] is not RMCManager RMCManagerForm)
            {
                MessageBox.Show("Fatal Error - StartBroadcastModule has reported a critical error, it is recommended that you restart RapidMessageCast. BroadcastController reported as null.");
                return;
            }
            PCBroadcastModule pcBroadcastModule = new();
            RMCManagerForm.TraceLog($"Info - [BroadcastController]: Starting StartPCBroadcastModule...");
            moduleRunning[RMCEnums.PC] = true;
            pcBroadcastModule.BroadcastPCMessage(message, computerList, totalSeconds, false, emergencyMode, reattemptOnError, dontSaveBroadcastHistory, isScheduledBroadcast);
            await MonitorBroadcastModules(false);
        }
        public async Task StartEmailBroadcastModule() //TODO: Add parameters to this method to allow for more customization of the email broadcast.
        {
            if (Application.OpenForms.Count == 0 || Application.OpenForms[0] is not RMCManager RMCManagerForm)
            {
                MessageBox.Show("Fatal Error - StartBroadcastModule has reported a critical error, it is recommended that you restart RapidMessageCast. BroadcastController reported as null.");
                return;
            }
            RMCManagerForm.TraceLog($"Info - [BroadcastController]: Starting StartEmailBroadcastModule...");
            moduleRunning[RMCEnums.Email] = true;
            EmailBroadcastModule emailBroadcastModule = new();
            await MonitorBroadcastModules(false);
        }

        public static void SetStatusOfBroadcastModule(RMCEnums module, bool running) //I love static methods. not really.
        {
            if (Application.OpenForms.Count == 0 || Application.OpenForms[0] is not RMCManager RMCManagerForm)
            {
                MessageBox.Show("Fatal Error - SetStatusOfBroadcastModule has reported a critical error, it is recommended that you restart RapidMessageCast. BroadcastController reported as null.");
                return;
            }

            RMCManagerForm.TraceLog($"Info - [BroadcastController]: Setting status of {module} module to the following state: {running}...");

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
                    RMCManagerForm.TraceLog("Error - [BroadcastController] - An error occurred while trying to set the module running status to BroadcastController. The module specified was not found.");
                    break;
            }
        }

        private static async Task MonitorBroadcastModules(bool isSecondTimeRunningWatchdog)
        {
            if (Application.OpenForms.Count == 0 || Application.OpenForms[0] is not RMCManager RMCManagerForm)
            {
                MessageBox.Show("Fatal Error - MonitorBroadcastModules has reported a critical error, it is recommended that you restart RapidMessageCast.");
                return;
            }

            if (isSecondTimeRunningWatchdog)
            {
                RMCManagerForm.TraceLog("Warning - [MonitorBroadcastModules] - Running watchdog for the second time.");
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
                    await HandleHungBroadcastModule(isSecondTimeRunningWatchdog, RMCManagerForm);
                }
                else
                {
                    RMCManagerForm.TraceLog("Info - [MonitorBroadcastModules] - All broadcast modules have finished. RMC Program is now idle.");
                }
            }
            catch (TaskCanceledException)
            {
                await HandleHungBroadcastModule(isSecondTimeRunningWatchdog, RMCManagerForm);
            }
        }

        private static async Task HandleHungBroadcastModule(bool isSecondTimeRunningWatchdog, RMCManager RMCManagerForm)
        {
            RMCManagerForm.TraceLog("Critical - [MonitorBroadcastModules] - A hung broadcast module has been detected. Prompting user with force module stop request...");
            if (MessageBox.Show("A broadcast module has hung. This means that a broadcast module has not finished in the expected time frame. Do you want to force stop this broadcast?", "RapidMessageCast - Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                RMCManagerForm.TraceLog("Info - [MonitorBroadcastModules] - User action - Force stopping modules...");
                RMCManagerForm.TraceLog("Info - [MonitorBroadcastModules] - Detailed log: Module states: " + string.Join(", ", moduleRunning.Select(kv => $"{kv.Key}: {kv.Value}")));
                //Force stop the running modules.
                foreach (var module in moduleRunning)
                {
                    if (module.Value)
                    {
                        SetStatusOfBroadcastModule(module.Key, false); //The internal module class should handle the stopping of the module. It will monitor the status of the BrodcastStatus and stop the module if it's still running.
                    }
                }
            }

            if (isSecondTimeRunningWatchdog)
            {
                RMCManagerForm.TraceLog("Critical - [MonitorBroadcastModules] - A hung broadcast module has been detected again. RMC is stuck. Call RMCDebugCall.");
                RMCDebugCall(RMCManagerForm, "HandleHungBroadcastModule");
                // Alert the user with a more urgent message
                MessageBox.Show("A broadcast module has hung for the second time. It is recommended that you restart RapidMessageCast. A detailed debug text is on the log tab.", "RapidMessageCast - Critical Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //Find out what module is still running and stop it.
                foreach (var module in moduleRunning)
                {
                    if (module.Value)
                    {
                        SetStatusOfBroadcastModule(module.Key, false); //The internal module class should handle the stopping of the module. It will monitor the status of the BrodcastStatus and stop the module if it's still running.
                    }
                }

            }
            else
            {
                await MonitorBroadcastModules(true);
            }
        }

        private static void RMCDebugCall(RMCManager RMCManagerForm, string classCall)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var threadId = Environment.CurrentManagedThreadId;
            RMCManagerForm.TraceLog($"Critical - [RMCDebugCall] - {classCall} has called RMCDebugCall: [{timestamp}] [Thread {threadId}]");
            RMCManagerForm.TraceLog("Critical - [RMCDebugCall] - Detailed log: BroadcastController module states: " + string.Join(", ", moduleRunning.Select(kv => $"{kv.Key}: {kv.Value}")));
            RMCManagerForm.TraceLog("Critical - [RMCDebugCall] - Detailed log: OpenForms count: " + Application.OpenForms.Count);
            RMCManagerForm.TraceLog("Critical - [RMCDebugCall] - Detailed log: OpenForms: " + string.Join(", ", Application.OpenForms.Cast<Form>().Select(f => f.Name)));
            RMCManagerForm.TraceLog("Critical - [RMCDebugCall] - Detailed log: RMC process memory taken: " + Process.GetCurrentProcess().WorkingSet64);
            RMCManagerForm.TraceLog("Critical - [RMCDebugCall] - Detailed log: RMC process CPU usage: " + Process.GetCurrentProcess().TotalProcessorTime);
            RMCManagerForm.TraceLog("Critical - [RMCDebugCall] - Detailed log: RMC process threads: " + Process.GetCurrentProcess().Threads.Count);
            RMCManagerForm.TraceLog("Critical - [RMCDebugCall] - Detailed log: RMC process virtual memory: " + Process.GetCurrentProcess().VirtualMemorySize64);
            RMCManagerForm.TraceLog("Critical - [RMCDebugCall] - Debug call completed.");
        }
    }
}
