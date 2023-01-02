using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quartz
{
    public partial class SearchForms : Form
    {
        private IReadOnlyCollection<string> Strings;
        private readonly StringBuilder _stringBuilder;
        
        public SearchForms(IReadOnlyCollection<string> strings)
        {
            InitializeComponent();

            Strings = strings;
            _stringBuilder = new StringBuilder().AppendJoin('\n', Strings);
            ResultRichTextBox.Text = _stringBuilder.ToString();
        }

        private void SearchTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Keys.Enter != (Keys)e.KeyChar)
            {
                return;
            }

            e.Handled = true;
            
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                ResultRichTextBox.Text = _stringBuilder.ToString();
                return;
            }

            var result = Strings.Where(s => s.ToLower().Contains(SearchTextBox.Text.ToLower()))
                .ToList();

            if (result.Count == 0)
            {
                ResultRichTextBox.Text = "Not found";
                return;
            }
            
            ResultRichTextBox.Text = "";
            foreach (var text in result)
            {
                ResultRichTextBox.AppendText(text + Environment.NewLine);
            }
        }
    }
}
