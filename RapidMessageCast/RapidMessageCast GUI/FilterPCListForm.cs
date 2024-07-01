using System.Text.RegularExpressions;

namespace RapidMessageCast_Manager
{
    public partial class FilterPCListForm : Form
    {
        string OriginalPCList = "";
        public FilterPCListForm(string PCList)
        {
            OriginalPCList = PCList;
            InitializeComponent();
        }

        private void FilterPCListForm_Load(object sender, EventArgs e)
        {
            MessagePCList.Text = OriginalPCList;
        }

        private void AddRegexBtn_Click(object sender, EventArgs e)
        {
            //check if input is nothing or whitespace
            if (string.IsNullOrWhiteSpace(TempRegexTxt.Text))
            {
                RegexlogList.Items.Add("Invalid Regex: Empty");
                return;
            }
            try
            {
                Regex.IsMatch(MessagePCList.Text, TempRegexTxt.Text);
                AllRegexFiltersListbox.Items.Add(TempRegexTxt.Text);
                TempRegexTxt.Text = "";
                //Apply Filters
                ApplyRegexFilters();
            }
            catch (Exception ex)
            {
                RegexlogList.Items.Add("Invalid Regex: " + ex.Message);
            }
        }

        private void ApplyRegexFilters()
        {
            //Apply Filters
            string[] Filters = new string[AllRegexFiltersListbox.Items.Count];
            AllRegexFiltersListbox.Items.CopyTo(Filters, 0);
            string FilteredPCList = OriginalPCList;
            foreach (string Filter in Filters)
            {
                FilteredPCList = Regex.Replace(FilteredPCList, Filter, "");
                //add a new line
            }
            FilteredPCList = Regex.Replace(FilteredPCList, @"\s+", "\r\n");
            MessagePCList.Text = FilteredPCList;
        }

        private void DeleteSelectedRegexBtn_Click(object sender, EventArgs e)
        {
            if (AllRegexFiltersListbox.SelectedIndex != -1)
            {
                AllRegexFiltersListbox.Items.RemoveAt(AllRegexFiltersListbox.SelectedIndex);
                //Apply Filters
                ApplyRegexFilters();
            }
        }

        private void ApplyBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
