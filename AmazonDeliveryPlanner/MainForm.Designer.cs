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
            this.openSettingsButton = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.driverListBox = new System.Windows.Forms.ListBox();
            this.activeDriversRadioButton = new System.Windows.Forms.RadioButton();
            this.drivers_9_11_radioButton = new System.Windows.Forms.RadioButton();
            this.drivers_24_45_radioButton = new System.Windows.Forms.RadioButton();
            this.refreshDriversButton = new System.Windows.Forms.Button();
            this.allRadioButton = new System.Windows.Forms.RadioButton();
            this.driversPanel = new System.Windows.Forms.Panel();
            this.showDriversBrowserControlDevToolsButton = new System.Windows.Forms.Button();
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.sessionsTabPage = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.refreshDriverListBrowserButton = new System.Windows.Forms.Button();
            this.adminTabPage = new System.Windows.Forms.TabPage();
            this.goForwardButton = new System.Windows.Forms.Button();
            this.adminBrowserPanel = new System.Windows.Forms.Panel();
            this.goBackButton = new System.Windows.Forms.Button();
            this.loadUrlButton = new System.Windows.Forms.Button();
            this.urlTextBox = new System.Windows.Forms.TextBox();
            this.decreaseTextSizeButton = new System.Windows.Forms.Button();
            this.increaseTextSizeButton = new System.Windows.Forms.Button();
            this.refrehPageButton = new System.Windows.Forms.Button();
            this.showDevToolsButton = new System.Windows.Forms.Button();
            this.loggingTabPage = new System.Windows.Forms.TabPage();
            this.autoScrollCheckBox = new System.Windows.Forms.CheckBox();
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.toggleLeftPanelVisibilityButton = new System.Windows.Forms.Button();
            this.showOpenDriverFormButton = new System.Windows.Forms.Button();
            this.plannerLabel = new System.Windows.Forms.Label();
            this.driversPanel.SuspendLayout();
            this.mainTabControl.SuspendLayout();
            this.sessionsTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.adminTabPage.SuspendLayout();
            this.loggingTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // openSettingsButton
            // 
            this.openSettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.openSettingsButton.Location = new System.Drawing.Point(1476, 2);
            this.openSettingsButton.Margin = new System.Windows.Forms.Padding(0);
            this.openSettingsButton.Name = "openSettingsButton";
            this.openSettingsButton.Size = new System.Drawing.Size(86, 20);
            this.openSettingsButton.TabIndex = 2;
            this.openSettingsButton.Text = "Configuratie";
            this.openSettingsButton.UseVisualStyleBackColor = true;
            this.openSettingsButton.Click += new System.EventHandler(this.openSettingsButton_Click);
            // 
            // tabControl
            // 
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1260, 775);
            this.tabControl.TabIndex = 0;
            // 
            // driverListBox
            // 
            this.driverListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.driverListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.driverListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.driverListBox.FormattingEnabled = true;
            this.driverListBox.Location = new System.Drawing.Point(0, 53);
            this.driverListBox.Name = "driverListBox";
            this.driverListBox.Size = new System.Drawing.Size(281, 719);
            this.driverListBox.TabIndex = 3;
            this.driverListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.driverListBox_DrawItem);
            this.driverListBox.SelectedIndexChanged += new System.EventHandler(this.driverListBox_SelectedIndexChanged);
            this.driverListBox.DoubleClick += new System.EventHandler(this.driverListBox_DoubleClick);
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
            // refreshDriversButton
            // 
            this.refreshDriversButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.refreshDriversButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.refreshDriversButton.Location = new System.Drawing.Point(252, 739);
            this.refreshDriversButton.Name = "refreshDriversButton";
            this.refreshDriversButton.Size = new System.Drawing.Size(28, 31);
            this.refreshDriversButton.TabIndex = 7;
            this.refreshDriversButton.Text = "🔄";
            this.refreshDriversButton.UseVisualStyleBackColor = true;
            this.refreshDriversButton.Click += new System.EventHandler(this.refreshDriversButton_Click);
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
            // driversPanel
            // 
            this.driversPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.driversPanel.Controls.Add(this.refreshDriversButton);
            this.driversPanel.Controls.Add(this.allRadioButton);
            this.driversPanel.Controls.Add(this.drivers_24_45_radioButton);
            this.driversPanel.Controls.Add(this.drivers_9_11_radioButton);
            this.driversPanel.Controls.Add(this.activeDriversRadioButton);
            this.driversPanel.Controls.Add(this.driverListBox);
            this.driversPanel.Location = new System.Drawing.Point(3, 3);
            this.driversPanel.Name = "driversPanel";
            this.driversPanel.Size = new System.Drawing.Size(284, 2051);
            this.driversPanel.TabIndex = 4;
            // 
            // showDriversBrowserControlDevToolsButton
            // 
            this.showDriversBrowserControlDevToolsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.showDriversBrowserControlDevToolsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.showDriversBrowserControlDevToolsButton.Location = new System.Drawing.Point(28, 752);
            this.showDriversBrowserControlDevToolsButton.Margin = new System.Windows.Forms.Padding(0);
            this.showDriversBrowserControlDevToolsButton.Name = "showDriversBrowserControlDevToolsButton";
            this.showDriversBrowserControlDevToolsButton.Size = new System.Drawing.Size(26, 23);
            this.showDriversBrowserControlDevToolsButton.TabIndex = 9;
            this.showDriversBrowserControlDevToolsButton.Text = "ੴ";
            this.showDriversBrowserControlDevToolsButton.UseVisualStyleBackColor = true;
            this.showDriversBrowserControlDevToolsButton.Click += new System.EventHandler(this.showDriversBrowserControlDevToolsButton_Click);
            // 
            // mainTabControl
            // 
            this.mainTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainTabControl.Controls.Add(this.sessionsTabPage);
            this.mainTabControl.Controls.Add(this.adminTabPage);
            this.mainTabControl.Controls.Add(this.loggingTabPage);
            this.mainTabControl.Location = new System.Drawing.Point(2, -1);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(1565, 807);
            this.mainTabControl.TabIndex = 5;
            // 
            // sessionsTabPage
            // 
            this.sessionsTabPage.Controls.Add(this.splitContainer1);
            this.sessionsTabPage.Location = new System.Drawing.Point(4, 22);
            this.sessionsTabPage.Name = "sessionsTabPage";
            this.sessionsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.sessionsTabPage.Size = new System.Drawing.Size(1557, 781);
            this.sessionsTabPage.TabIndex = 0;
            this.sessionsTabPage.Text = "Sessions";
            this.sessionsTabPage.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.DarkGray;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.showDriversBrowserControlDevToolsButton);
            this.splitContainer1.Panel1.Controls.Add(this.refreshDriverListBrowserButton);
            this.splitContainer1.Panel1.Controls.Add(this.driversPanel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl);
            this.splitContainer1.Size = new System.Drawing.Size(1551, 775);
            this.splitContainer1.SplitterDistance = 287;
            this.splitContainer1.TabIndex = 5;
            // 
            // refreshDriverListBrowserButton
            // 
            this.refreshDriverListBrowserButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.refreshDriverListBrowserButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.refreshDriverListBrowserButton.Location = new System.Drawing.Point(0, 752);
            this.refreshDriverListBrowserButton.Margin = new System.Windows.Forms.Padding(0);
            this.refreshDriverListBrowserButton.Name = "refreshDriverListBrowserButton";
            this.refreshDriverListBrowserButton.Size = new System.Drawing.Size(26, 23);
            this.refreshDriverListBrowserButton.TabIndex = 32;
            this.refreshDriverListBrowserButton.Text = "⟳";
            this.refreshDriverListBrowserButton.UseVisualStyleBackColor = true;
            this.refreshDriverListBrowserButton.Click += new System.EventHandler(this.refreshDriverListBrowserButton_Click);
            // 
            // adminTabPage
            // 
            this.adminTabPage.Controls.Add(this.goForwardButton);
            this.adminTabPage.Controls.Add(this.adminBrowserPanel);
            this.adminTabPage.Controls.Add(this.goBackButton);
            this.adminTabPage.Controls.Add(this.loadUrlButton);
            this.adminTabPage.Controls.Add(this.urlTextBox);
            this.adminTabPage.Controls.Add(this.decreaseTextSizeButton);
            this.adminTabPage.Controls.Add(this.increaseTextSizeButton);
            this.adminTabPage.Controls.Add(this.refrehPageButton);
            this.adminTabPage.Controls.Add(this.showDevToolsButton);
            this.adminTabPage.Location = new System.Drawing.Point(4, 22);
            this.adminTabPage.Name = "adminTabPage";
            this.adminTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.adminTabPage.Size = new System.Drawing.Size(1557, 781);
            this.adminTabPage.TabIndex = 1;
            this.adminTabPage.Text = "Admin";
            this.adminTabPage.UseVisualStyleBackColor = true;
            // 
            // goForwardButton
            // 
            this.goForwardButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.goForwardButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.goForwardButton.Location = new System.Drawing.Point(1430, 1);
            this.goForwardButton.Name = "goForwardButton";
            this.goForwardButton.Size = new System.Drawing.Size(26, 23);
            this.goForwardButton.TabIndex = 48;
            this.goForwardButton.Text = "→";
            this.goForwardButton.UseVisualStyleBackColor = true;
            this.goForwardButton.Click += new System.EventHandler(this.goForwardButton_Click);
            // 
            // adminBrowserPanel
            // 
            this.adminBrowserPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.adminBrowserPanel.Location = new System.Drawing.Point(3, 23);
            this.adminBrowserPanel.Name = "adminBrowserPanel";
            this.adminBrowserPanel.Size = new System.Drawing.Size(1552, 758);
            this.adminBrowserPanel.TabIndex = 47;
            // 
            // goBackButton
            // 
            this.goBackButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.goBackButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.goBackButton.Location = new System.Drawing.Point(1405, 1);
            this.goBackButton.Name = "goBackButton";
            this.goBackButton.Size = new System.Drawing.Size(26, 23);
            this.goBackButton.TabIndex = 46;
            this.goBackButton.Text = "←";
            this.goBackButton.UseVisualStyleBackColor = true;
            this.goBackButton.Click += new System.EventHandler(this.goBackButton_Click);
            // 
            // loadUrlButton
            // 
            this.loadUrlButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.loadUrlButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadUrlButton.Location = new System.Drawing.Point(1346, 0);
            this.loadUrlButton.Name = "loadUrlButton";
            this.loadUrlButton.Size = new System.Drawing.Size(26, 23);
            this.loadUrlButton.TabIndex = 45;
            this.loadUrlButton.Text = "►";
            this.loadUrlButton.UseVisualStyleBackColor = true;
            this.loadUrlButton.Click += new System.EventHandler(this.loadUrlButton_Click);
            // 
            // urlTextBox
            // 
            this.urlTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.urlTextBox.Location = new System.Drawing.Point(3, 3);
            this.urlTextBox.Name = "urlTextBox";
            this.urlTextBox.Size = new System.Drawing.Size(1337, 20);
            this.urlTextBox.TabIndex = 44;
            this.urlTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.urlTextBox_KeyUp);
            // 
            // decreaseTextSizeButton
            // 
            this.decreaseTextSizeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.decreaseTextSizeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.decreaseTextSizeButton.Location = new System.Drawing.Point(1490, 0);
            this.decreaseTextSizeButton.Name = "decreaseTextSizeButton";
            this.decreaseTextSizeButton.Size = new System.Drawing.Size(20, 23);
            this.decreaseTextSizeButton.TabIndex = 43;
            this.decreaseTextSizeButton.Text = "-";
            this.decreaseTextSizeButton.UseVisualStyleBackColor = true;
            this.decreaseTextSizeButton.Click += new System.EventHandler(this.decreaseTextSizeButton_Click);
            // 
            // increaseTextSizeButton
            // 
            this.increaseTextSizeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.increaseTextSizeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.increaseTextSizeButton.Location = new System.Drawing.Point(1470, 0);
            this.increaseTextSizeButton.Name = "increaseTextSizeButton";
            this.increaseTextSizeButton.Size = new System.Drawing.Size(20, 23);
            this.increaseTextSizeButton.TabIndex = 42;
            this.increaseTextSizeButton.Text = "+";
            this.increaseTextSizeButton.UseVisualStyleBackColor = true;
            this.increaseTextSizeButton.Click += new System.EventHandler(this.increaseTextSizeButton_Click);
            // 
            // refrehPageButton
            // 
            this.refrehPageButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.refrehPageButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.refrehPageButton.Location = new System.Drawing.Point(1376, 0);
            this.refrehPageButton.Margin = new System.Windows.Forms.Padding(0);
            this.refrehPageButton.Name = "refrehPageButton";
            this.refrehPageButton.Size = new System.Drawing.Size(26, 23);
            this.refrehPageButton.TabIndex = 40;
            this.refrehPageButton.Text = "⟳";
            this.refrehPageButton.UseVisualStyleBackColor = true;
            this.refrehPageButton.Click += new System.EventHandler(this.refrehPageButton_Click);
            // 
            // showDevToolsButton
            // 
            this.showDevToolsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.showDevToolsButton.Location = new System.Drawing.Point(1524, 0);
            this.showDevToolsButton.Margin = new System.Windows.Forms.Padding(0);
            this.showDevToolsButton.Name = "showDevToolsButton";
            this.showDevToolsButton.Size = new System.Drawing.Size(26, 23);
            this.showDevToolsButton.TabIndex = 39;
            this.showDevToolsButton.Text = "ੴ";
            this.showDevToolsButton.UseVisualStyleBackColor = true;
            this.showDevToolsButton.Click += new System.EventHandler(this.showDevToolsButton_Click);
            // 
            // loggingTabPage
            // 
            this.loggingTabPage.Controls.Add(this.autoScrollCheckBox);
            this.loggingTabPage.Controls.Add(this.logTextBox);
            this.loggingTabPage.Location = new System.Drawing.Point(4, 22);
            this.loggingTabPage.Name = "loggingTabPage";
            this.loggingTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.loggingTabPage.Size = new System.Drawing.Size(1557, 781);
            this.loggingTabPage.TabIndex = 2;
            this.loggingTabPage.Text = "Log";
            this.loggingTabPage.UseVisualStyleBackColor = true;
            // 
            // autoScrollCheckBox
            // 
            this.autoScrollCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.autoScrollCheckBox.AutoSize = true;
            this.autoScrollCheckBox.Location = new System.Drawing.Point(1458, 758);
            this.autoScrollCheckBox.Name = "autoScrollCheckBox";
            this.autoScrollCheckBox.Size = new System.Drawing.Size(75, 17);
            this.autoScrollCheckBox.TabIndex = 1;
            this.autoScrollCheckBox.Text = "Auto scroll";
            this.autoScrollCheckBox.UseVisualStyleBackColor = true;
            // 
            // logTextBox
            // 
            this.logTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logTextBox.Location = new System.Drawing.Point(3, 3);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTextBox.Size = new System.Drawing.Size(1551, 775);
            this.logTextBox.TabIndex = 0;
            // 
            // toggleLeftPanelVisibilityButton
            // 
            this.toggleLeftPanelVisibilityButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toggleLeftPanelVisibilityButton.Location = new System.Drawing.Point(1430, 2);
            this.toggleLeftPanelVisibilityButton.Margin = new System.Windows.Forms.Padding(0);
            this.toggleLeftPanelVisibilityButton.Name = "toggleLeftPanelVisibilityButton";
            this.toggleLeftPanelVisibilityButton.Size = new System.Drawing.Size(42, 20);
            this.toggleLeftPanelVisibilityButton.TabIndex = 6;
            this.toggleLeftPanelVisibilityButton.Text = "| █";
            this.toggleLeftPanelVisibilityButton.UseVisualStyleBackColor = true;
            this.toggleLeftPanelVisibilityButton.Click += new System.EventHandler(this.toggleLeftPanelVisibilityButton_Click);
            // 
            // showOpenDriverFormButton
            // 
            this.showOpenDriverFormButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.showOpenDriverFormButton.Location = new System.Drawing.Point(1380, 2);
            this.showOpenDriverFormButton.Margin = new System.Windows.Forms.Padding(0);
            this.showOpenDriverFormButton.Name = "showOpenDriverFormButton";
            this.showOpenDriverFormButton.Size = new System.Drawing.Size(42, 20);
            this.showOpenDriverFormButton.TabIndex = 7;
            this.showOpenDriverFormButton.Text = "🔎";
            this.showOpenDriverFormButton.UseVisualStyleBackColor = true;
            this.showOpenDriverFormButton.Click += new System.EventHandler(this.showOpenDriverFormButton_Click);
            // 
            // plannerLabel
            // 
            this.plannerLabel.Location = new System.Drawing.Point(1185, 4);
            this.plannerLabel.Name = "plannerLabel";
            this.plannerLabel.Size = new System.Drawing.Size(192, 17);
            this.plannerLabel.TabIndex = 6;
            this.plannerLabel.Text = "_____ loged in planner ____";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1565, 807);
            this.Controls.Add(this.plannerLabel);
            this.Controls.Add(this.showOpenDriverFormButton);
            this.Controls.Add(this.toggleLeftPanelVisibilityButton);
            this.Controls.Add(this.openSettingsButton);
            this.Controls.Add(this.mainTabControl);
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.MainForm_PreviewKeyDown);
            this.driversPanel.ResumeLayout(false);
            this.driversPanel.PerformLayout();
            this.mainTabControl.ResumeLayout(false);
            this.sessionsTabPage.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.adminTabPage.ResumeLayout(false);
            this.adminTabPage.PerformLayout();
            this.loggingTabPage.ResumeLayout(false);
            this.loggingTabPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button openSettingsButton;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.ListBox driverListBox;
        private System.Windows.Forms.RadioButton activeDriversRadioButton;
        private System.Windows.Forms.RadioButton drivers_9_11_radioButton;
        private System.Windows.Forms.RadioButton drivers_24_45_radioButton;
        private System.Windows.Forms.Button refreshDriversButton;
        private System.Windows.Forms.RadioButton allRadioButton;
        private System.Windows.Forms.Panel driversPanel;
        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.TabPage sessionsTabPage;
        private System.Windows.Forms.TabPage adminTabPage;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button toggleLeftPanelVisibilityButton;
        private System.Windows.Forms.TabPage loggingTabPage;
        private System.Windows.Forms.TextBox logTextBox;
        private System.Windows.Forms.CheckBox autoScrollCheckBox;
        private System.Windows.Forms.Panel adminBrowserPanel;
        private System.Windows.Forms.Button goBackButton;
        private System.Windows.Forms.Button loadUrlButton;
        private System.Windows.Forms.TextBox urlTextBox;
        private System.Windows.Forms.Button decreaseTextSizeButton;
        private System.Windows.Forms.Button increaseTextSizeButton;
        private System.Windows.Forms.Button refrehPageButton;
        private System.Windows.Forms.Button showDevToolsButton;
        private System.Windows.Forms.Button goForwardButton;
        private System.Windows.Forms.Button refreshDriverListBrowserButton;
        private System.Windows.Forms.Button showOpenDriverFormButton;
        private System.Windows.Forms.Button showDriversBrowserControlDevToolsButton;
        private System.Windows.Forms.Label plannerLabel;
    }
}