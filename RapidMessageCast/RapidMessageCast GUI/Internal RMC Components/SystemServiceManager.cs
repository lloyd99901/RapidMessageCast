using System.Diagnostics;
using System.ServiceProcess;

//--RapidMessageCast Software--
//SystemServiceManager.cs - RapidMessageCast Manager

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
    internal class SystemServiceManager
    {
        //public static void RestartService(string serviceName)
        //{
        //    try
        //    {
        //        using (ServiceController service = new ServiceController(serviceName))
        //        {
        //            if (service.Status == ServiceControllerStatus.Running)
        //            {
        //                service.Stop();
        //                service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromMinutes(1)); // Timeout after 1 minute
        //            }
        //            service.Start();
        //            service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromMinutes(1)); // Timeout after 1 minute
        //        }
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        Console.WriteLine($"Error: Could not find the service '{serviceName}' on the computer. {ex.Message}");
        //    }
        //    catch (System.ServiceProcess.TimeoutException ex)
        //    {
        //        Console.WriteLine($"Error: Timeout while trying to stop/start the service '{serviceName}'. {ex.Message}");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        //    }
        //}
        public static void ReleaseAndRenewIP()
        {
            try
            {
                string[] commands = ["/release", "/renew"];
                foreach (var command in commands)
                {
                    ProcessStartInfo psi = new("ipconfig", command)
                    {
                        WindowStyle = ProcessWindowStyle.Hidden
                    };
                    Process? process = Process.Start(psi);
                    if (process != null)
                    {
                        //wait for exit but don't halt UI graphic call
                        process.EnableRaisingEvents = true;
                        process.Exited += (sender, e) =>
                        {
                            MessageBox.Show($"IP configuration command '{command}' completed with exit code {process.ExitCode}");
                            process.Dispose();
                        };
                    }
                    else
                    {
                        // Handle the case where the process could not be started
                        MessageBox.Show($"Failed to start the process with command: {command}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        public static bool AreSpecifiedServicesRunning(string[] serviceNames)
        {
            foreach (string serviceName in serviceNames)
            {
                try
                {
                    using ServiceController service = new(serviceName);
                    if (service.Status != ServiceControllerStatus.Running)
                    {
                        return false; // If any service is not running, return false
                    }
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"Error: Could not find or access the service '{serviceName}'. {ex.Message}");
                    return false; // Return false if there's an error accessing the service
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An unexpected error occurred while checking the service '{serviceName}': {ex.Message}");
                    return false; // Return false for any other unexpected errors
                }
            }
            return true; // All specified services are running
        }
    }
}
