﻿//--RapidMessageCast Software--
//RMCUpdateChecker.cs - RapidMessageCast Manager

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
    internal class RMCUpdateChecker
    {
        internal static readonly string[] separator = ["\"tag_name\":\""];

        //Check for github releases that are newer than the current version
        public static bool CheckForUpdates()
        {
            //Get the latest release from the github api, but use httpclient instead of webclient, with a try catch block
            string latestRelease = "";
            try
            {
                using HttpClient client = new();
                client.DefaultRequestHeaders.Add("User-Agent", "RapidMessageCast");
                latestRelease = client.GetStringAsync("https://api.github.com/repos/lloyd99901/RapidMessageCast/releases/latest").Result;
            }
            catch (Exception)
            {
                return false;
            }

            //Get the version number from the latest release
            string latestVersion = latestRelease.Split(separator, StringSplitOptions.None)[1].Split('"')[0];

            //Get the current version
            string currentVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName()?.Version?.ToString() ?? "0.0.0.0";

            //Check if the latest version is newer than the current version
            if (new Version(latestVersion) > new Version(currentVersion))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
