using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmazonDeliveryPlanner
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            AddUrlToList();
        }

        void RefreshUrlList()
        {
            urlListBox.Items.Clear();

            urlListBox.Items.AddRange(GlobalContext.Urls.ToArray());
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            RefreshUrlList();
        }

        void AddUrlToList()
        {
            if (!string.IsNullOrWhiteSpace(urlTextBox.Text))
            {
                GlobalContext.Urls.Add(urlTextBox.Text);
                urlTextBox.Text = "";

                RefreshUrlList();
            }
        }

        void DeleteSelectedURL()
        {
            if (urlListBox.SelectedIndex >= 0)
            {
                GlobalContext.Urls.RemoveAt(urlListBox.SelectedIndex);
                RefreshUrlList();
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            DeleteSelectedURL();
        }
    }
}
