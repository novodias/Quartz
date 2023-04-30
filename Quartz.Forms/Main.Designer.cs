namespace Quartz.Forms
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.SelectServerJar = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.MemoryAllocationTextBox = new System.Windows.Forms.TextBox();
            this.OpenButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.JavaPathComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ServerSettingsButton = new System.Windows.Forms.Button();
            this.AddPluginButton = new System.Windows.Forms.Button();
            this.KillButton = new System.Windows.Forms.Button();
            this.OpenFolderButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.PresetsComboBox = new System.Windows.Forms.ComboBox();
            this.OutputRichTextBox = new System.Windows.Forms.RichTextBox();
            this.OutputLabel = new System.Windows.Forms.Label();
            this.SendTextBox = new System.Windows.Forms.TextBox();
            this.ClearCommandButton = new System.Windows.Forms.Button();
            this.clearOutput = new System.Windows.Forms.Button();
            this.SendButton = new System.Windows.Forms.Button();
            this.SaveAndCloseButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.ServerGroupBox = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.DiscordWebhookCheckBox = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.WebhookUrlTextBox = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.SearchButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.PlayersListBox = new System.Windows.Forms.ListBox();
            this.ActivityTime = new System.Windows.Forms.Label();
            this.QuartzNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.CreatePresetButton = new System.Windows.Forms.Button();
            this.RemovePresetButton = new System.Windows.Forms.Button();
            this.PresetNameTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.ServerGroupBox.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SelectServerJar
            // 
            this.SelectServerJar.Location = new System.Drawing.Point(6, 22);
            this.SelectServerJar.Name = "SelectServerJar";
            this.SelectServerJar.Size = new System.Drawing.Size(227, 28);
            this.SelectServerJar.TabIndex = 1;
            this.SelectServerJar.Text = "Select the server .jar";
            this.SelectServerJar.UseVisualStyleBackColor = true;
            this.SelectServerJar.Click += new System.EventHandler(this.SelectServerJar_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 284);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Memory Allocation";
            // 
            // MemoryAllocationTextBox
            // 
            this.MemoryAllocationTextBox.Location = new System.Drawing.Point(121, 281);
            this.MemoryAllocationTextBox.Name = "MemoryAllocationTextBox";
            this.MemoryAllocationTextBox.PlaceholderText = "2 (GB)";
            this.MemoryAllocationTextBox.Size = new System.Drawing.Size(112, 23);
            this.MemoryAllocationTextBox.TabIndex = 4;
            this.MemoryAllocationTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnlyNumbers_KeyPress);
            // 
            // OpenButton
            // 
            this.OpenButton.Enabled = false;
            this.OpenButton.Location = new System.Drawing.Point(121, 310);
            this.OpenButton.Name = "OpenButton";
            this.OpenButton.Size = new System.Drawing.Size(112, 28);
            this.OpenButton.TabIndex = 7;
            this.OpenButton.Text = "Open";
            this.OpenButton.UseVisualStyleBackColor = true;
            this.OpenButton.Click += new System.EventHandler(this.OpenButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.JavaPathComboBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.ServerSettingsButton);
            this.groupBox1.Controls.Add(this.AddPluginButton);
            this.groupBox1.Controls.Add(this.KillButton);
            this.groupBox1.Controls.Add(this.OpenFolderButton);
            this.groupBox1.Controls.Add(this.OpenButton);
            this.groupBox1.Controls.Add(this.SelectServerJar);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.MemoryAllocationTextBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 113);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(239, 347);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Configuration";
            // 
            // JavaPathComboBox
            // 
            this.JavaPathComboBox.FormattingEnabled = true;
            this.JavaPathComboBox.Location = new System.Drawing.Point(41, 252);
            this.JavaPathComboBox.Name = "JavaPathComboBox";
            this.JavaPathComboBox.Size = new System.Drawing.Size(192, 23);
            this.JavaPathComboBox.TabIndex = 23;
            this.JavaPathComboBox.SelectedIndexChanged += new System.EventHandler(this.JavaPathComboBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 255);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 15);
            this.label3.TabIndex = 22;
            this.label3.Text = "Java";
            // 
            // ServerSettingsButton
            // 
            this.ServerSettingsButton.Location = new System.Drawing.Point(6, 90);
            this.ServerSettingsButton.Name = "ServerSettingsButton";
            this.ServerSettingsButton.Size = new System.Drawing.Size(227, 28);
            this.ServerSettingsButton.TabIndex = 21;
            this.ServerSettingsButton.Text = "Server Settings";
            this.ServerSettingsButton.UseVisualStyleBackColor = true;
            this.ServerSettingsButton.Click += new System.EventHandler(this.ServerSettingsButton_Click);
            // 
            // AddPluginButton
            // 
            this.AddPluginButton.Location = new System.Drawing.Point(6, 56);
            this.AddPluginButton.Name = "AddPluginButton";
            this.AddPluginButton.Size = new System.Drawing.Size(227, 28);
            this.AddPluginButton.TabIndex = 20;
            this.AddPluginButton.Text = "Add Plugins";
            this.AddPluginButton.UseVisualStyleBackColor = true;
            this.AddPluginButton.Click += new System.EventHandler(this.AddPluginButton_Click);
            // 
            // KillButton
            // 
            this.KillButton.Enabled = false;
            this.KillButton.Location = new System.Drawing.Point(6, 310);
            this.KillButton.Name = "KillButton";
            this.KillButton.Size = new System.Drawing.Size(109, 28);
            this.KillButton.TabIndex = 8;
            this.KillButton.Text = "Kill";
            this.KillButton.UseVisualStyleBackColor = true;
            this.KillButton.Click += new System.EventHandler(this.KillButton_Click);
            // 
            // OpenFolderButton
            // 
            this.OpenFolderButton.Location = new System.Drawing.Point(6, 124);
            this.OpenFolderButton.Name = "OpenFolderButton";
            this.OpenFolderButton.Size = new System.Drawing.Size(227, 28);
            this.OpenFolderButton.TabIndex = 12;
            this.OpenFolderButton.Text = "Open Folder";
            this.OpenFolderButton.UseVisualStyleBackColor = true;
            this.OpenFolderButton.Click += new System.EventHandler(this.OpenFolderButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 15);
            this.label4.TabIndex = 25;
            this.label4.Text = "Presets";
            // 
            // PresetsComboBox
            // 
            this.PresetsComboBox.FormattingEnabled = true;
            this.PresetsComboBox.Location = new System.Drawing.Point(68, 21);
            this.PresetsComboBox.Name = "PresetsComboBox";
            this.PresetsComboBox.Size = new System.Drawing.Size(177, 23);
            this.PresetsComboBox.TabIndex = 24;
            this.PresetsComboBox.SelectedIndexChanged += new System.EventHandler(this.PresetsComboBox_SelectedIndexChanged);
            // 
            // OutputRichTextBox
            // 
            this.OutputRichTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.OutputRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.OutputRichTextBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.OutputRichTextBox.HideSelection = false;
            this.OutputRichTextBox.Location = new System.Drawing.Point(6, 40);
            this.OutputRichTextBox.Name = "OutputRichTextBox";
            this.OutputRichTextBox.ReadOnly = true;
            this.OutputRichTextBox.Size = new System.Drawing.Size(552, 329);
            this.OutputRichTextBox.TabIndex = 9;
            this.OutputRichTextBox.TabStop = false;
            this.OutputRichTextBox.Text = "";
            this.OutputRichTextBox.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.OutputRichTextBox_LinkClicked);
            // 
            // OutputLabel
            // 
            this.OutputLabel.AutoSize = true;
            this.OutputLabel.Font = new System.Drawing.Font("Segoe UI", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.OutputLabel.Location = new System.Drawing.Point(6, 12);
            this.OutputLabel.Name = "OutputLabel";
            this.OutputLabel.Size = new System.Drawing.Size(68, 23);
            this.OutputLabel.TabIndex = 10;
            this.OutputLabel.Text = "Output";
            // 
            // SendTextBox
            // 
            this.SendTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SendTextBox.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SendTextBox.Location = new System.Drawing.Point(3, 3);
            this.SendTextBox.Name = "SendTextBox";
            this.SendTextBox.PlaceholderText = " help";
            this.SendTextBox.Size = new System.Drawing.Size(380, 26);
            this.SendTextBox.TabIndex = 13;
            this.SendTextBox.WordWrap = false;
            this.SendTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SendTextBox_KeyPress);
            // 
            // ClearCommandButton
            // 
            this.ClearCommandButton.Location = new System.Drawing.Point(470, 3);
            this.ClearCommandButton.Name = "ClearCommandButton";
            this.ClearCommandButton.Size = new System.Drawing.Size(75, 28);
            this.ClearCommandButton.TabIndex = 14;
            this.ClearCommandButton.Text = "Clear";
            this.ClearCommandButton.UseVisualStyleBackColor = true;
            this.ClearCommandButton.Click += new System.EventHandler(this.ClearCommandButton_Click);
            // 
            // clearOutput
            // 
            this.clearOutput.Location = new System.Drawing.Point(414, 37);
            this.clearOutput.Name = "clearOutput";
            this.clearOutput.Size = new System.Drawing.Size(131, 28);
            this.clearOutput.TabIndex = 15;
            this.clearOutput.Text = "Clear Output";
            this.clearOutput.UseVisualStyleBackColor = true;
            this.clearOutput.Click += new System.EventHandler(this.clearOutput_Click);
            // 
            // SendButton
            // 
            this.SendButton.Location = new System.Drawing.Point(389, 3);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(75, 28);
            this.SendButton.TabIndex = 16;
            this.SendButton.Text = "Send";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // SaveAndCloseButton
            // 
            this.SaveAndCloseButton.Location = new System.Drawing.Point(140, 37);
            this.SaveAndCloseButton.Name = "SaveAndCloseButton";
            this.SaveAndCloseButton.Size = new System.Drawing.Size(131, 28);
            this.SaveAndCloseButton.TabIndex = 17;
            this.SaveAndCloseButton.Text = "Save And Close";
            this.SaveAndCloseButton.UseVisualStyleBackColor = true;
            this.SaveAndCloseButton.Click += new System.EventHandler(this.SaveAndCloseButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(277, 37);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(131, 28);
            this.SaveButton.TabIndex = 18;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // ServerGroupBox
            // 
            this.ServerGroupBox.Controls.Add(this.label7);
            this.ServerGroupBox.Controls.Add(this.DiscordWebhookCheckBox);
            this.ServerGroupBox.Controls.Add(this.label6);
            this.ServerGroupBox.Controls.Add(this.WebhookUrlTextBox);
            this.ServerGroupBox.Controls.Add(this.flowLayoutPanel1);
            this.ServerGroupBox.Controls.Add(this.label1);
            this.ServerGroupBox.Controls.Add(this.PlayersListBox);
            this.ServerGroupBox.Controls.Add(this.ActivityTime);
            this.ServerGroupBox.Controls.Add(this.OutputRichTextBox);
            this.ServerGroupBox.Controls.Add(this.OutputLabel);
            this.ServerGroupBox.Location = new System.Drawing.Point(257, 12);
            this.ServerGroupBox.Name = "ServerGroupBox";
            this.ServerGroupBox.Size = new System.Drawing.Size(756, 448);
            this.ServerGroupBox.TabIndex = 19;
            this.ServerGroupBox.TabStop = false;
            this.ServerGroupBox.Text = "Server";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(564, 382);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 15);
            this.label7.TabIndex = 27;
            this.label7.Text = "Discord Webhook";
            // 
            // DiscordWebhookCheckBox
            // 
            this.DiscordWebhookCheckBox.AutoSize = true;
            this.DiscordWebhookCheckBox.Location = new System.Drawing.Point(682, 381);
            this.DiscordWebhookCheckBox.Name = "DiscordWebhookCheckBox";
            this.DiscordWebhookCheckBox.Size = new System.Drawing.Size(68, 19);
            this.DiscordWebhookCheckBox.TabIndex = 26;
            this.DiscordWebhookCheckBox.Text = "Enabled";
            this.DiscordWebhookCheckBox.UseVisualStyleBackColor = true;
            this.DiscordWebhookCheckBox.CheckedChanged += new System.EventHandler(this.DiscordWebhookCheckBox_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(564, 419);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(28, 15);
            this.label6.TabIndex = 25;
            this.label6.Text = "URL";
            // 
            // WebhookUrlTextBox
            // 
            this.WebhookUrlTextBox.Location = new System.Drawing.Point(598, 416);
            this.WebhookUrlTextBox.Name = "WebhookUrlTextBox";
            this.WebhookUrlTextBox.PlaceholderText = "Webhook URL";
            this.WebhookUrlTextBox.Size = new System.Drawing.Size(152, 23);
            this.WebhookUrlTextBox.TabIndex = 24;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.SendTextBox);
            this.flowLayoutPanel1.Controls.Add(this.SendButton);
            this.flowLayoutPanel1.Controls.Add(this.ClearCommandButton);
            this.flowLayoutPanel1.Controls.Add(this.SearchButton);
            this.flowLayoutPanel1.Controls.Add(this.SaveAndCloseButton);
            this.flowLayoutPanel1.Controls.Add(this.SaveButton);
            this.flowLayoutPanel1.Controls.Add(this.clearOutput);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(6, 374);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(552, 68);
            this.flowLayoutPanel1.TabIndex = 23;
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(3, 37);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(131, 28);
            this.SearchButton.TabIndex = 22;
            this.SearchButton.Text = "Filter";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(564, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 23);
            this.label1.TabIndex = 21;
            this.label1.Text = "Players";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // PlayersListBox
            // 
            this.PlayersListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.PlayersListBox.FormattingEnabled = true;
            this.PlayersListBox.ItemHeight = 15;
            this.PlayersListBox.Location = new System.Drawing.Point(564, 40);
            this.PlayersListBox.Name = "PlayersListBox";
            this.PlayersListBox.Size = new System.Drawing.Size(186, 330);
            this.PlayersListBox.TabIndex = 20;
            // 
            // ActivityTime
            // 
            this.ActivityTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ActivityTime.AutoSize = true;
            this.ActivityTime.Font = new System.Drawing.Font("Segoe UI", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.ActivityTime.Location = new System.Drawing.Point(361, 12);
            this.ActivityTime.Name = "ActivityTime";
            this.ActivityTime.Size = new System.Drawing.Size(197, 23);
            this.ActivityTime.TabIndex = 19;
            this.ActivityTime.Text = "Activity Time: 00:00:00";
            this.ActivityTime.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // QuartzNotifyIcon
            // 
            this.QuartzNotifyIcon.BalloonTipTitle = "Quartz";
            this.QuartzNotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("QuartzNotifyIcon.Icon")));
            this.QuartzNotifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.QuartzNotifyIcon_MouseClick);
            // 
            // CreatePresetButton
            // 
            this.CreatePresetButton.Location = new System.Drawing.Point(18, 79);
            this.CreatePresetButton.Name = "CreatePresetButton";
            this.CreatePresetButton.Size = new System.Drawing.Size(109, 28);
            this.CreatePresetButton.TabIndex = 26;
            this.CreatePresetButton.Text = "Create Preset";
            this.CreatePresetButton.UseVisualStyleBackColor = true;
            this.CreatePresetButton.Click += new System.EventHandler(this.CreatePresetButton_Click);
            // 
            // RemovePresetButton
            // 
            this.RemovePresetButton.Location = new System.Drawing.Point(136, 79);
            this.RemovePresetButton.Name = "RemovePresetButton";
            this.RemovePresetButton.Size = new System.Drawing.Size(109, 28);
            this.RemovePresetButton.TabIndex = 27;
            this.RemovePresetButton.Text = "Remove Preset";
            this.RemovePresetButton.UseVisualStyleBackColor = true;
            this.RemovePresetButton.Click += new System.EventHandler(this.RemovePresetButton_Click);
            // 
            // PresetNameTextBox
            // 
            this.PresetNameTextBox.Location = new System.Drawing.Point(68, 50);
            this.PresetNameTextBox.Name = "PresetNameTextBox";
            this.PresetNameTextBox.PlaceholderText = "Preset Name";
            this.PresetNameTextBox.Size = new System.Drawing.Size(177, 23);
            this.PresetNameTextBox.TabIndex = 28;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 54);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 15);
            this.label5.TabIndex = 29;
            this.label5.Text = "Name";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1025, 472);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.PresetNameTextBox);
            this.Controls.Add(this.RemovePresetButton);
            this.Controls.Add(this.ServerGroupBox);
            this.Controls.Add(this.CreatePresetButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.PresetsComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "Quartz";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.Resize += new System.EventHandler(this.Main_Resize);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ServerGroupBox.ResumeLayout(false);
            this.ServerGroupBox.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button SelectServerJar;
        private Label label2;
        private TextBox MemoryAllocationTextBox;
        private Button OpenButton;
        private GroupBox groupBox1;
        private Button KillButton;
        private RichTextBox OutputRichTextBox;
        private Label OutputLabel;
        private Button OpenFolderButton;
        private TextBox SendTextBox;
        private Button ClearCommandButton;
        private Button clearOutput;
        private Button SendButton;
        private Button SaveAndCloseButton;
        private Button SaveButton;
        private GroupBox ServerGroupBox;
        private Label ActivityTime;
        private ListBox PlayersListBox;
        private Button AddPluginButton;
        private Label label1;
        private Button ServerSettingsButton;
        private ComboBox JavaPathComboBox;
        private Label label3;
        private Button SearchButton;
        private FlowLayoutPanel flowLayoutPanel1;
        private NotifyIcon QuartzNotifyIcon;
        private Label label4;
        private ComboBox PresetsComboBox;
        private Button RemovePresetButton;
        private Button CreatePresetButton;
        private TextBox PresetNameTextBox;
        private Label label5;
        private Label label7;
        private CheckBox DiscordWebhookCheckBox;
        private Label label6;
        private TextBox WebhookUrlTextBox;
    }
}