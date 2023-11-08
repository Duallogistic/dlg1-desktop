using AmazonDeliveryPlanner.API;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace AmazonDeliveryPlanner
{
    public partial class OpenDriverForm : Form
    {
        Driver[] drivers;
        IEnumerable<Driver> filteredDrivers;
        Driver selectedDriver;
        bool displayOnlyDriverGroupName;

        public Driver[] Drivers { get => drivers; /*set => drivers = value;*/ }
        public Driver SelectedDriver { get => selectedDriver; set => selectedDriver = value; }
        public bool DisplayOnlyDriverGroupName { get => displayOnlyDriverGroupName; set => displayOnlyDriverGroupName = value; }

        public OpenDriverForm(Driver[] drivers)
        {
            InitializeComponent();

            this.drivers = drivers;
            this.filteredDrivers = drivers.ToArray();
            // this.drivers.CopyTo(this.filteredDrivers, 0);

            Driver._ListModeToString = true;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {

        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void EditPlanningNotesForm_Load(object sender, EventArgs e)
        {
            displayOnlyDriverGroupNameCheckBox.Checked = displayOnlyDriverGroupName;

            this.ActiveControl = filterDriversTextBox;
            filterDriversTextBox.Focus();

            RefreshFilteredDriverList();
        }

        //void SelectDriver()
        //{            
        //    this.DialogResult = DialogResult.OK;
        //}

        private void OpenDriverForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //if (e.KeyCode == Keys.Escape)
            //{
            //    if (!string.IsNullOrWhiteSpace(filterDriversTextBox.Text))
            //        filterDriversTextBox.Text = "";
            //    else
            //        Close();
            //}

            //if (filteredDriversListBox.Items.Count > 0)
            //{
            //    if (e.KeyCode == Keys.Down)
            //    {
            //        this.ActiveControl = filteredDriversListBox;
            //        filteredDriversListBox.Focus();

            //        filteredDriversListBox.SelectedIndex = 0;
            //    }
            //    else
            //    if (e.KeyCode == Keys.Up)
            //    {
            //        this.ActiveControl = filteredDriversListBox;
            //        filteredDriversListBox.Focus();

            //        filteredDriversListBox.SelectedIndex = filteredDriversListBox.Items.Count - 1;
            //    }
            //}

            //if (e.KeyCode == Keys.Enter)
            //    OpenDriver();
            //// Save();
        }

        private void filteredDriversListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        Driver GetSelectedDriver()
        {
            if (filteredDriversListBox.SelectedIndex < 0)
                return null;
            else
                return (Driver)filteredDriversListBox.SelectedItem;
        }

        private void filteredDriversListBox_Click(object sender, EventArgs e)
        {

        }

        private void filteredDriversListBox_DoubleClick(object sender, EventArgs e)
        {
            OpenDriver();
        }

        private void OpenDriver()
        {
            Driver selectedDriver = GetSelectedDriver();

            if (selectedDriver != null)
            {
                this.selectedDriver = selectedDriver;

                this.DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void filterDriversTextBox_TextChanged(object sender, EventArgs e)
        {
            filteredDrivers = drivers.Where(dr => (dr.group_name + " " + dr.first_name + " " + dr.last_name + " " + dr.reg_plate).IndexOf(filterDriversTextBox.Text, StringComparison.CurrentCultureIgnoreCase) >= 0);

            RefreshFilteredDriverList();
        }

        void RefreshFilteredDriverList()
        {
            this.filteredDriversListBox.Items.Clear();
            this.filteredDriversListBox.Items.AddRange(filteredDrivers.ToArray());
        }

        private void OpenDriverForm_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void OpenDriverForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (!string.IsNullOrWhiteSpace(filterDriversTextBox.Text))
                    filterDriversTextBox.Text = "";
                else
                    Close();
            }

            if (filteredDriversListBox.Items.Count > 0 && this.ActiveControl != filteredDriversListBox)
            {
                if (e.KeyCode == Keys.Down)
                {
                    this.ActiveControl = filteredDriversListBox;
                    filteredDriversListBox.Focus();

                    filteredDriversListBox.SelectedIndex = 0;
                }
                else
                if (e.KeyCode == Keys.Up)
                {
                    this.ActiveControl = filteredDriversListBox;
                    filteredDriversListBox.Focus();

                    filteredDriversListBox.SelectedIndex = filteredDriversListBox.Items.Count - 1;
                }
            }

            if (e.KeyCode == Keys.Enter)
                OpenDriver();
            // Save();
        }

        private void OpenDriverForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Driver._ListModeToString = false;
        }

        private void displayOnlyDriverGroupNameCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Driver._ListModeToString = !displayOnlyDriverGroupNameCheckBox.Checked;
            displayOnlyDriverGroupName = displayOnlyDriverGroupNameCheckBox.Checked;
            RefreshFilteredDriverList();
        }
    }
}
