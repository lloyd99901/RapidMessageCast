//--RapidMessageCast Software--
//EmailBroadcastModule.cs - RapidMessageCast Manager

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

using RapidMessageCast_Manager.Internal_RMC_Components;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Xml.Serialization;

namespace RapidMessageCast_Manager.BroadcastModules
{
    internal class EmailBroadcastModule
    {
        readonly HistoryManager broadcastHistoryHandler = new();
        //public static void SendTestEmail(string FQDNServer, string FromEmailAddress, AuthMode AuthMode, string AccountText, string Password, string TestTargetEmailAddress)
        //{
        //    BeginEmailCast(FQDNServer, FromEmailAddress, AuthMode, AccountText, Password, TestTargetEmailAddress);
        //}
        public void BeginEmailCast(string RMCEmailFileLocation, string FQDNServer, string FromEmailAddress, AuthMode AuthMode, string AccountText, string Password, string TargetEmailAddresses)
        {
            if (Application.OpenForms.Count == 0 || Application.OpenForms[0] is not RMCManager RMCManagerForm) //If this happens, something went really wrong here...
            {
                MessageBox.Show("Fatal Error - EmailBroadcastModule has reported a critical error, it is recommeneded that you restart RapidMessageCast. Details: Error with communicating with RMCManagerForm while attempting to broadcast. RMCManagerForm reported as null.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            broadcastHistoryHandler.AddToHistory(RMCEnums.Email, $"===RapidMessageCast=== - Version: {RMCManagerForm.versionNumb}");
            broadcastHistoryHandler.AddToHistory(RMCEnums.Email, $"START - Broadcast has started. Broadcast started by: {Environment.UserName} - System Name: {Environment.MachineName}");
            broadcastHistoryHandler.AddToHistory(RMCEnums.Email, $"Broadcast Type: Email");
            broadcastHistoryHandler.AddToHistory(RMCEnums.Email, $"RMCEmail File: {RMCEmailFileLocation}");
            broadcastHistoryHandler.AddToHistory(RMCEnums.Email, $"Broadcast Target: {TargetEmailAddresses}");
            broadcastHistoryHandler.AddToHistory(RMCEnums.Email, $"Broadcast Server: {FQDNServer}");
            broadcastHistoryHandler.AddToHistory(RMCEnums.Email, $"Broadcast Account: {AccountText}");
            broadcastHistoryHandler.AddToHistory(RMCEnums.Email, $"Broadcast Auth Mode: {AuthMode}");
            broadcastHistoryHandler.AddToHistory(RMCEnums.Email, "============================================");
            //Check if all parameters are valid and meet the requirements
            if (string.IsNullOrWhiteSpace(FQDNServer) || string.IsNullOrWhiteSpace(FromEmailAddress) || string.IsNullOrWhiteSpace(AccountText) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(TargetEmailAddresses))
            {
                return;
            }
            //Filter invalid email addresses
            TargetEmailAddresses = RegexFilters.FilterInvalidEmailAddresses(TargetEmailAddresses);
            //Check if there are any valid email addresses
            if (string.IsNullOrWhiteSpace(TargetEmailAddresses))
            {
                return;
            }
            //Check if FQDN is valid
            if (!RegexFilters.FilterInvalidFQDN(FQDNServer))
            {
                return;
            }
            //Email Sending Code Here - Read the RMCEmail file via iomanager and send the email
            string emailSubject = string.Empty;
            string emailBody = string.Empty;
            bool isEmailBodyHTML = false;
            Encoding subjectEncodingType = Encoding.UTF8;
            Encoding bodyEncodingType = Encoding.UTF8;
            //Read the RMCEmail file IOManager LoadRMCEmailFile XML.
            try
            {
                string[] EmailFileContents = IOManager.LoadRMCEmailFile(RMCEmailFileLocation);
                //check if first 0 index is empty, if it isn't, something went wrong
                if (string.IsNullOrWhiteSpace(EmailFileContents[0]))
                {
                    return;
                }
                //Check if the email subject is empty, if it is, return
                if (string.IsNullOrWhiteSpace(EmailFileContents[1]))
                {
                    return;
                }
                //Check if the email body is empty, if it is, return
                if (string.IsNullOrWhiteSpace(EmailFileContents[2]))
                {
                    return;
                }
                //Check if the email body is HTML
                if (EmailFileContents[3].ToLower() == "true")
                {
                    isEmailBodyHTML = true;
                }
                //Since this is now checked, send the email
                emailSubject = EmailFileContents[1];
                emailBody = EmailFileContents[2];
                //Send the email [TODO: PORT IS NOT CORRECT]
                SendEmail(FQDNServer, 25, FromEmailAddress, AuthMode, AccountText, Password, TargetEmailAddresses, emailSubject, emailBody, isEmailBodyHTML, subjectEncodingType, bodyEncodingType);
            }
            catch
            {
                return;
            }
        }
        private static void SendEmail(string FQDNServer, int FQDNPort, string FromEmailAddress, AuthMode AuthMode, string AccountText, string Password, string TargetEmailAddresses, string EmailSubject, string EmailBody, bool isEmailBodyHTML, Encoding SubjectEncodingType, Encoding BodyEncodingType)
        {
            using var smtpClient = new SmtpClient(FQDNServer, FQDNPort);
            //Check what authentication mode is being used, if its none, then no credentials are needed, if its basic, then use the account text and password, if its SSL, then use the account text and password, if its NTLM, use default credentials and enable SSL
            switch (AuthMode)
            {
                case AuthMode.None:
                    smtpClient.UseDefaultCredentials = true;
                    break;
                case AuthMode.Basic:
                    smtpClient.Credentials = new NetworkCredential(AccountText, Password);
                    break;
                case AuthMode.SSL:
                    smtpClient.Credentials = new NetworkCredential(AccountText, Password);
                    smtpClient.EnableSsl = true;
                    break;
                case AuthMode.NTLM:
                    smtpClient.UseDefaultCredentials = true;
                    smtpClient.EnableSsl = true;
                    break;
            }

            using var mailMessage = new MailMessage();
            try
            {
                mailMessage.From = new MailAddress(FromEmailAddress);
                mailMessage.Subject = EmailSubject;
                mailMessage.Body = EmailBody;
                mailMessage.IsBodyHtml = isEmailBodyHTML;
                mailMessage.SubjectEncoding = SubjectEncodingType;
                mailMessage.BodyEncoding = BodyEncodingType;

                foreach (var emailAddress in TargetEmailAddresses.Split(';'))
                {
                    mailMessage.To.Add(emailAddress);
                }

                smtpClient.Send(mailMessage);
            }
            catch (SmtpException smtpEx)
            {
                // Log SMTP-specific errors
                Console.WriteLine($"SMTP Error: {smtpEx.Message}");
            }
            catch (FormatException formatEx)
            {
                // Log format-specific errors
                Console.WriteLine($"Email Format Error: {formatEx.Message}");
            }
            catch (Exception ex)
            {
                // Log general errors
                Console.WriteLine($"General Error: {ex.Message}");
            }
        }

    }
}
