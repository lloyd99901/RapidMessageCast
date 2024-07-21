using System.Text.RegularExpressions;

//--RapidMessageCast Software--
//RegexFilters.cs - RapidMessageCast Manager

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
    internal class RegexFilters
    {
        public static string FilterInvalidPCNames(string text)
        {
            // Regular expression to match characters that are not allowed in NetBIOS or Windows hostnames
            // This pattern was incorrect for filtering invalid characters. It was designed to validate correct names, not filter characters.
            // To filter invalid characters, we need a pattern that matches characters NOT allowed in the names.
            string pattern = @"[^A-Za-z0-9-\n]"; //The previous filter would remove the PC from the list if it was only one PC. This will fix that.

            // Replace invalid characters with empty string
            return Regex.Replace(text, pattern, "");
        }
        public static string FilterInvalidMessage(string text)
        {
            // Regular expression to match characters that are not allowed in messages
            string pattern = @"[\/]";

            // Replace invalid characters with empty string
            return Regex.Replace(text, pattern, "");
        }
        public static string FilterInvalidMACAddresses(string text)
        {
            // Regular expression to match characters that are not allowed in MAC addresses
            string pattern = @"^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$";

            // Replace invalid characters with empty string
            return Regex.Replace(text, pattern, "");
        }
        public static string FilterInvalidEmailAddresses(string text)
        {
            // Regular expression to match characters that are not allowed in email addresses
            string pattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

            // Replace invalid characters with empty string
            return Regex.Replace(text, pattern, "");
        }
        public static bool FilterInvalidFQDN(string text)
        {
            // Regular expression to match characters that are not allowed in FQDNs
            string pattern = @"^((?!-)[A-Za-z0-9-]{1,63}(?<!-)\.)+[A-Za-z]{2,6}$";

            // Check if the FQDN is valid
            return Regex.IsMatch(text, pattern);
        }
    }
}
