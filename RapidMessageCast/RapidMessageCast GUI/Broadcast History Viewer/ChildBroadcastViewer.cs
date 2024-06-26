﻿using System.Text.RegularExpressions;

//--RapidMessageCast Software--
//ChildBroadcastViewer.cs - RapidMessageCast Manager

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

namespace RapidMessageCast_Manager.Broadcast_History_Viewer
{
    public partial class ChildBroadcastViewer : Form
    {
        readonly Regex errorRegex = new(@"\b(error|exception|fail(ed)?|fatal)\b", RegexOptions.IgnoreCase);
        readonly Regex warningRegex = new(@"\b(warning|caution|alert)\b", RegexOptions.IgnoreCase);
        readonly Regex criticalRegex = new(@"\b(critical|urgent|important)\b", RegexOptions.IgnoreCase);
        readonly string FilePath = "";
        public ChildBroadcastViewer(string FileLocation)
        {
            InitializeComponent();
            FilePath = FileLocation;
        }

        private void ChildBroadcastViewer_Load(object sender, EventArgs e)
        {
            Text = FilePath;
            LoadLogEntries();
        }

        private void LoadInsightOnLogEntries()
        {
            //Count the number of critical, error, warning, and info messages in the log file, Then display the count in the insight label.
            int criticalCount = 0;
            int errorCount = 0;
            int warningCount = 0;
            int infoCount = 0;
            string[] lines = LogList.Text.Split("\n");
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                Match errorMatch = errorRegex.Match(line);
                if (errorMatch.Success)
                {
                    errorCount++;
                    continue;
                }

                Match warningMatch = warningRegex.Match(line);
                if (warningMatch.Success)
                {
                    warningCount++;
                    continue;
                }
                Match criticalMatch = criticalRegex.Match(line);
                if (criticalMatch.Success)
                {
                    criticalCount++;
                    continue;
                }
                //If all of the above conditions are not met, then the message is an info message.
                infoCount++;
            }
            //Also get any text that is in "" and count those as the number of PC's that the message was sent to.
            MatchCollection pcMatches = Regex.Matches(LogList.Text, "\"([^\"]*)\"");
            int pcCount = pcMatches.Count;
            MessageInsightsLabel.Text = $"Number of log types:\nCritical: {criticalCount}\nError: {errorCount}\nWarning: {warningCount}\nInfo: {infoCount}\nPC Count: {pcCount}";
            //With the PCMatches, now for each match, get the text inside the "" and add it to the PC list.
            foreach (Match match in pcMatches.Cast<Match>())
            {
                string pcName = match.Groups[1].Value;
                if (!PCList.Items.Contains(pcName))
                {
                    PCList.Items.Add(pcName);
                }
            }
            //Colour the insight label based on the number of critical, error, warning, and info messages.
            if (criticalCount > 0)
            {
                MessageInsightsLabel.BackColor = Color.Red;
            }
            else if (errorCount > 0)
            {
                MessageInsightsLabel.BackColor = Color.Orange;
            }
            else if (warningCount > 0)
            {
                MessageInsightsLabel.BackColor = Color.Yellow;
            }
            else
            {
                MessageInsightsLabel.BackColor = Color.Green;
            }
        }

        private void LoadLogEntries()
        {
            LogList.Clear();
            LogList.Visible = false;
            string[] lines = File.ReadAllLines(FilePath);
            // Add each line to the list box, if it contains error, warning, or info, colour it accordingly
            for (int i = 0; i < lines.Length; i++)
            {
                Refresh();
                string line = lines[i];

                // Check if the line contains an error message
                Match errorMatch = errorRegex.Match(line);
                if (errorMatch.Success)
                {
                    // Set the color of the error message to red
                    LogList.SelectionColor = Color.Yellow;
                    LogList.SelectionBackColor = Color.Red;
                    LogList.AppendText(line);
                    LogList.AppendText(Environment.NewLine);
                    continue;
                }

                // Check if the line contains a warning message
                Match warningMatch = warningRegex.Match(line);
                if (warningMatch.Success)
                {
                    // Set the color of the warning message to orange
                    LogList.SelectionColor = Color.Black;
                    LogList.SelectionBackColor = Color.Yellow;
                    LogList.AppendText(line);
                    LogList.AppendText(Environment.NewLine);

                    continue;
                }

                // If the line does not contain an error, warning or info message, just append it as is
                LogList.AppendText(line);
                LogList.AppendText(Environment.NewLine);

                Refresh();
            }
            LogList.Visible = true;
            LoadInsightOnLogEntries();
        }

        private void ChildBroadcastViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            LogList.Dispose();
            GC.Collect();
            Dispose();
        }
    }
}
