using System.Text.RegularExpressions;

namespace RapidMessageCast_Manager.Internal_RMC_Components
{
    internal class InternalRegexFilters
    {
        public static string FilterInvalidPCNames(string text)
        {
            // Regular expression to match characters that are not allowed in NetBIOS or Windows hostnames
            string pattern = @"^(?!-)[A-Za-z0-9-]{1,15}(?<!-)$";

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
