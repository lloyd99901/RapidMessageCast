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
    }
}
