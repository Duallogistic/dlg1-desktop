
namespace AmazonDeliveryPlanner
{
    partial class BrowserUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.showDevToolsButton = new System.Windows.Forms.Button();
            this.refrehPageButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // showDevToolsButton
            // 
            this.showDevToolsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.showDevToolsButton.Location = new System.Drawing.Point(797, 408);
            this.showDevToolsButton.Name = "showDevToolsButton";
            this.showDevToolsButton.Size = new System.Drawing.Size(32, 23);
            this.showDevToolsButton.TabIndex = 0;
            this.showDevToolsButton.Text = "D t";
            this.showDevToolsButton.UseVisualStyleBackColor = true;
            this.showDevToolsButton.Click += new System.EventHandler(this.showDevToolsButton_Click);
            // 
            // refrehPageButton
            // 
            this.refrehPageButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.refrehPageButton.Location = new System.Drawing.Point(754, 379);
            this.refrehPageButton.Name = "refrehPageButton";
            this.refrehPageButton.Size = new System.Drawing.Size(75, 23);
            this.refrehPageButton.TabIndex = 31;
            this.refrehPageButton.Text = "Refresh";
            this.refrehPageButton.UseVisualStyleBackColor = true;
            this.refrehPageButton.Click += new System.EventHandler(this.refrehPageButton_Click);
            // 
            // BrowserUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.refrehPageButton);
            this.Controls.Add(this.showDevToolsButton);
            this.Name = "BrowserUserControl";
            this.Size = new System.Drawing.Size(848, 434);
            this.Load += new System.EventHandler(this.UserControl1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button showDevToolsButton;
        private System.Windows.Forms.Button refrehPageButton;
    }
}
