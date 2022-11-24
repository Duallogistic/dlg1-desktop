namespace AmazonDeliveryPlanner
{
    partial class MainForm
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.addSessionButton = new System.Windows.Forms.Button();
            this.openSettingsButton = new System.Windows.Forms.Button();
            this.driverListBox = new System.Windows.Forms.ListBox();
            this.driversPanel = new System.Windows.Forms.Panel();
            this.allRadioButton = new System.Windows.Forms.RadioButton();
            this.refreshDriversButton = new System.Windows.Forms.Button();
            this.drivers_24_45_radioButton = new System.Windows.Forms.RadioButton();
            this.drivers_9_11_radioButton = new System.Windows.Forms.RadioButton();
            this.activeDriversRadioButton = new System.Windows.Forms.RadioButton();
            this.driversPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Location = new System.Drawing.Point(213, 43);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1350, 761);
            this.tabControl.TabIndex = 0;
            // 
            // addSessionButton
            // 
            this.addSessionButton.Location = new System.Drawing.Point(3, 10);
            this.addSessionButton.Name = "addSessionButton";
            this.addSessionButton.Size = new System.Drawing.Size(111, 27);
            this.addSessionButton.TabIndex = 1;
            this.addSessionButton.Text = "Adauga sesiune";
            this.addSessionButton.UseVisualStyleBackColor = true;
            this.addSessionButton.Click += new System.EventHandler(this.addSessionButton_Click);
            // 
            // openSettingsButton
            // 
            this.openSettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.openSettingsButton.Location = new System.Drawing.Point(1469, 10);
            this.openSettingsButton.Name = "openSettingsButton";
            this.openSettingsButton.Size = new System.Drawing.Size(86, 27);
            this.openSettingsButton.TabIndex = 2;
            this.openSettingsButton.Text = "Configuratie";
            this.openSettingsButton.UseVisualStyleBackColor = true;
            this.openSettingsButton.Click += new System.EventHandler(this.openSettingsButton_Click);
            // 
            // driverListBox
            // 
            this.driverListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.driverListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.driverListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.driverListBox.FormattingEnabled = true;
            this.driverListBox.Location = new System.Drawing.Point(0, 56);
            this.driverListBox.Name = "driverListBox";
            this.driverListBox.Size = new System.Drawing.Size(201, 706);
            this.driverListBox.TabIndex = 3;
            this.driverListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.driverListBox_DrawItem);
            this.driverListBox.SelectedIndexChanged += new System.EventHandler(this.driverListBox_SelectedIndexChanged);
            this.driverListBox.DoubleClick += new System.EventHandler(this.driverListBox_DoubleClick);
            // 
            // driversPanel
            // 
            this.driversPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.driversPanel.Controls.Add(this.allRadioButton);
            this.driversPanel.Controls.Add(this.refreshDriversButton);
            this.driversPanel.Controls.Add(this.drivers_24_45_radioButton);
            this.driversPanel.Controls.Add(this.drivers_9_11_radioButton);
            this.driversPanel.Controls.Add(this.activeDriversRadioButton);
            this.driversPanel.Controls.Add(this.driverListBox);
            this.driversPanel.Location = new System.Drawing.Point(3, 43);
            this.driversPanel.Name = "driversPanel";
            this.driversPanel.Size = new System.Drawing.Size(204, 761);
            this.driversPanel.TabIndex = 4;
            // 
            // allRadioButton
            // 
            this.allRadioButton.AutoSize = true;
            this.allRadioButton.Checked = true;
            this.allRadioButton.Location = new System.Drawing.Point(75, 7);
            this.allRadioButton.Name = "allRadioButton";
            this.allRadioButton.Size = new System.Drawing.Size(36, 17);
            this.allRadioButton.TabIndex = 8;
            this.allRadioButton.TabStop = true;
            this.allRadioButton.Text = "All";
            this.allRadioButton.UseVisualStyleBackColor = true;
            this.allRadioButton.CheckedChanged += new System.EventHandler(this.driversRadioButton_CheckedChanged);
            // 
            // refreshDriversButton
            // 
            this.refreshDriversButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.refreshDriversButton.Location = new System.Drawing.Point(148, 738);
            this.refreshDriversButton.Name = "refreshDriversButton";
            this.refreshDriversButton.Size = new System.Drawing.Size(56, 23);
            this.refreshDriversButton.TabIndex = 7;
            this.refreshDriversButton.Text = "Refresh";
            this.refreshDriversButton.UseVisualStyleBackColor = true;
            this.refreshDriversButton.Click += new System.EventHandler(this.refreshDriversButton_Click);
            // 
            // drivers_24_45_radioButton
            // 
            this.drivers_24_45_radioButton.AutoSize = true;
            this.drivers_24_45_radioButton.Location = new System.Drawing.Point(75, 30);
            this.drivers_24_45_radioButton.Name = "drivers_24_45_radioButton";
            this.drivers_24_45_radioButton.Size = new System.Drawing.Size(54, 17);
            this.drivers_24_45_radioButton.TabIndex = 6;
            this.drivers_24_45_radioButton.Text = "24/45";
            this.drivers_24_45_radioButton.UseVisualStyleBackColor = true;
            this.drivers_24_45_radioButton.CheckedChanged += new System.EventHandler(this.driversRadioButton_CheckedChanged);
            // 
            // drivers_9_11_radioButton
            // 
            this.drivers_9_11_radioButton.AutoSize = true;
            this.drivers_9_11_radioButton.Location = new System.Drawing.Point(8, 30);
            this.drivers_9_11_radioButton.Name = "drivers_9_11_radioButton";
            this.drivers_9_11_radioButton.Size = new System.Drawing.Size(48, 17);
            this.drivers_9_11_radioButton.TabIndex = 5;
            this.drivers_9_11_radioButton.Text = "9/11";
            this.drivers_9_11_radioButton.UseVisualStyleBackColor = true;
            this.drivers_9_11_radioButton.CheckedChanged += new System.EventHandler(this.driversRadioButton_CheckedChanged);
            // 
            // activeDriversRadioButton
            // 
            this.activeDriversRadioButton.AutoSize = true;
            this.activeDriversRadioButton.Location = new System.Drawing.Point(9, 7);
            this.activeDriversRadioButton.Name = "activeDriversRadioButton";
            this.activeDriversRadioButton.Size = new System.Drawing.Size(55, 17);
            this.activeDriversRadioButton.TabIndex = 4;
            this.activeDriversRadioButton.Text = "Active";
            this.activeDriversRadioButton.UseVisualStyleBackColor = true;
            this.activeDriversRadioButton.CheckedChanged += new System.EventHandler(this.driversRadioButton_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1565, 807);
            this.Controls.Add(this.driversPanel);
            this.Controls.Add(this.openSettingsButton);
            this.Controls.Add(this.addSessionButton);
            this.Controls.Add(this.tabControl);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.driversPanel.ResumeLayout(false);
            this.driversPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.Button addSessionButton;
        private System.Windows.Forms.Button openSettingsButton;
        private System.Windows.Forms.ListBox driverListBox;
        private System.Windows.Forms.Panel driversPanel;
        private System.Windows.Forms.RadioButton drivers_24_45_radioButton;
        private System.Windows.Forms.RadioButton drivers_9_11_radioButton;
        private System.Windows.Forms.RadioButton activeDriversRadioButton;
        private System.Windows.Forms.Button refreshDriversButton;
        private System.Windows.Forms.RadioButton allRadioButton;
    }
}