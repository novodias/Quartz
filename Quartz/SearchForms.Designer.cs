namespace Quartz
{
    partial class SearchForms
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SearchTextBox = new System.Windows.Forms.TextBox();
            this.ResultRichTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // SearchTextBox
            // 
            this.SearchTextBox.Location = new System.Drawing.Point(12, 12);
            this.SearchTextBox.Name = "SearchTextBox";
            this.SearchTextBox.PlaceholderText = "Search Keyword - Press Enter to Filter";
            this.SearchTextBox.Size = new System.Drawing.Size(600, 23);
            this.SearchTextBox.TabIndex = 0;
            this.SearchTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SearchTextBox_KeyPress);
            // 
            // ResultRichTextBox
            // 
            this.ResultRichTextBox.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ResultRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ResultRichTextBox.Location = new System.Drawing.Point(12, 41);
            this.ResultRichTextBox.Name = "ResultRichTextBox";
            this.ResultRichTextBox.ReadOnly = true;
            this.ResultRichTextBox.Size = new System.Drawing.Size(600, 388);
            this.ResultRichTextBox.TabIndex = 1;
            this.ResultRichTextBox.Text = "";
            // 
            // SearchForms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.ResultRichTextBox);
            this.Controls.Add(this.SearchTextBox);
            this.Name = "SearchForms";
            this.Text = "Search";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox SearchTextBox;
        private RichTextBox ResultRichTextBox;
    }
}