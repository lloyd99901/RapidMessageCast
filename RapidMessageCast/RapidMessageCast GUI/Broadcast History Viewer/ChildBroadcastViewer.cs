
using System.Text.RegularExpressions;

namespace RapidMessageCast_Manager.Broadcast_History_Viewer
{
    public partial class ChildBroadcastViewer : Form
    {
        Regex errorRegex = new Regex(@"\b(error|exception|fail(ed)?|fatal)\b", RegexOptions.IgnoreCase);
        Regex warningRegex = new Regex(@"\b(warning|caution|alert)\b", RegexOptions.IgnoreCase);
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
        }

        private void ChildBroadcastViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            LogList.Dispose();
            GC.Collect();
            Dispose();
        }
    }
}
