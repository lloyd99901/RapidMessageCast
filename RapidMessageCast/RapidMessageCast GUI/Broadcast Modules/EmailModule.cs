//--RapidMessageCast Software--
//EmailModule.cs - RapidMessageCast Manager

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
using System.Windows.Forms;

namespace RapidMessageCast_Manager.BroadcastModules
{
    internal class EmailModule
    {
        readonly HistoryManager broadcastHistoryHandler = new();
        //public static void SendTestEmail(string FQDNServer, string FromEmailAddress, AuthMode AuthMode, string AccountText, string Password, string TestTargetEmailAddress)
        //{
        //    BeginEmailCast(FQDNServer, FromEmailAddress, AuthMode, AccountText, Password, TestTargetEmailAddress);
        //}
        public void BeginEmailCast(string FQDNServer, string FromEmailAddress, AuthMode AuthMode, string AccountText, string Password, string TargetEmailAddresses)
        {
            //if (Application.OpenForms.Count == 0 || Application.OpenForms[0] is not RMCManager RMCManagerForm) //If this happens, something went really wrong here...
            //{
            //    MessageBox.Show("Fatal Error - PCBroadcastModule has reported a critical error, it is recommeneded that you restart RapidMessageCast. Details: Error with communicating with RMCManagerForm while attempting to broadcast. RMCManagerForm reported as null.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
            //broadcastHistoryHandler.AddToHistory(RMCEnums.Email, $"===RapidMessageCast=== - Version: {RMCManagerForm.versionNumb}");
            //broadcastHistoryHandler.AddToHistory(RMCEnums.Email, $"START - Broadcast has started. Broadcast started by: {Environment.UserName} - System Name: {Environment.MachineName}");
            //broadcastHistoryHandler.AddToHistory(RMCEnums.Email, $"Message - Message Content: {message}");
            //broadcastHistoryHandler.AddToHistory(RMCEnums.Email, $"Duration - Message Duration: {duration} seconds");
            //broadcastHistoryHandler.AddToHistory(RMCEnums.Email, $"Reattempt on Failure - : {isReattemptOnErrorChecked}");
            //broadcastHistoryHandler.AddToHistory(RMCEnums.Email, $"Emergency Mode - Emergency Mode: {emergencyMode}");
            //broadcastHistoryHandler.AddToHistory(RMCEnums.Email, $"Save Broadcast History - Save Broadcast History: {isDontSaveBroadcastHistoryChecked}");
            //broadcastHistoryHandler.AddToHistory(RMCEnums.Email, "============================================");
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
            //TODO - Implement Email Sending
        }
        private static void SendEmail(string FQDNServer, int FQDNPort, string FromEmailAddress, AuthMode AuthMode, string AccountText, string Password, string TargetEmailAddresses, string EmailSubject, string EmailBody, bool isEmailBodyHTML, Encoding SubjectEncodingType, Encoding BodyEncodingType)
        {
            using (var smtpClient = new SmtpClient(FQDNServer, FQDNPort))
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(AccountText, Password);
                smtpClient.EnableSsl = AuthMode == AuthMode.SSL;

                using (var mailMessage = new MailMessage())
                {
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

    }
}
