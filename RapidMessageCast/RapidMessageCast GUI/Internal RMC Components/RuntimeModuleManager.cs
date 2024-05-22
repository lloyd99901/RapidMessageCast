using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//--RapidMessageCast Software--
//ModuleWatchDog.cs - RapidMessageCast Manager

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
    internal class RuntimeModuleManager
    {
        //bool array for the modules. Each one will be true or false depending on if the module is running.
        static readonly bool[] moduleRunning = new bool[3]; //0 = PC, 1 = Email, 2 = PSExec //Note this may be a bad implementation but this will work for now.

        public static bool IsModuleRunning()
        {
            //Check if any of the 3 bools in the moduleRunning array are true. If they are, return true.
            for (int i = 0; i < moduleRunning.Length; i++)
            {
                if (moduleRunning[i])
                {
                    return true;
                }
            }
            return false; //If none of the bools are true, return false. This means that no modules are running.
        }
        public void SetModuleRunning(RMCEnums module, bool running)
        {
            //Set the moduleRunning bool for the specified module to the specified value. if PC then 0, if Email then 1, if PSExec then 2.
            switch (module)
            {
                case RMCEnums.PC:
                    moduleRunning[0] = running;
                    break;
                case RMCEnums.Email:
                    moduleRunning[1] = running;
                    break;
                case RMCEnums.PSExec:
                    moduleRunning[2] = running;
                    break;
                default:
                    if (Application.OpenForms.Count == 0 || Application.OpenForms[0] is not RMCManager RMCManagerForm) //If this happens, something went really wrong here...
                    {
                        MessageBox.Show("Fatal Error - RMC Broadcast Watchdog has reported a critical error, it is recommeneded that you restart RapidMessageCast. Details: Error with communicating with RMCManagerForm while attempting to set module running status to watchdog. RMCManagerForm reported as null.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    RMCManagerForm.AddTextToLogList("Error - [BroadcastWatchdog] - An error occured while trying to set the module running status to watchdog. The module specified was not found.");
                    break;
            }
        }
    }
}
