using AmazonDeliveryPlanner.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmazonDeliveryPlanner
{
    public partial class PlannerSelectorForm : Form
    {
        Planner[] planners;
        IEnumerable<Planner> filteredPlanners;
        Planner selectedPlanner;
        bool displayOnlyPlannerGroupName;

        public Planner[] Planners { get => planners; /*set => planners = value;*/ }
        public Planner SelectedPlanner { get => selectedPlanner; set => selectedPlanner = value; }
        public bool DisplayOnlyPlannerGroupName { get => displayOnlyPlannerGroupName; set => displayOnlyPlannerGroupName = value; }

        public PlannerSelectorForm(Planner[] planners)
        {
            InitializeComponent();

            this.planners = planners;
            this.filteredPlanners = planners.ToArray();
            // this.planners.CopyTo(this.filteredDrivers, 0);
            
            Planner._ListModeToString = true;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            OpenPlanner();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {            
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void EditPlanningNotesForm_Load(object sender, EventArgs e)
        {
            displayOnlyPlannerGroupNameCheckBox.Checked = displayOnlyPlannerGroupName;

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

        Planner GetSelectedPlanner()
        {
            if (filteredDriversListBox.SelectedIndex < 0)
                return null;
            else
                return (Planner)filteredDriversListBox.SelectedItem;
        }

        private void filteredDriversListBox_Click(object sender, EventArgs e)
        {

        }

        private void filteredDriversListBox_DoubleClick(object sender, EventArgs e)
        {
            OpenPlanner();
        }

        private void OpenPlanner()
        {
            Planner selectedPlanner = GetSelectedPlanner();

            if (selectedPlanner != null)
            {
                this.selectedPlanner = selectedPlanner;

                this.DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void filterDriversTextBox_TextChanged(object sender, EventArgs e)
        {
            filteredPlanners = planners.Where(dr => (dr.last_name + " " + dr.first_name + " " + dr.role_name + " " + dr.email).IndexOf(filterDriversTextBox.Text, StringComparison.CurrentCultureIgnoreCase) >= 0);

            RefreshFilteredDriverList();
        }

        void RefreshFilteredDriverList()
        {
            this.filteredDriversListBox.Items.Clear();
            this.filteredDriversListBox.Items.AddRange(filteredPlanners.ToArray());
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
                OpenPlanner();
            // Save();
        }

        private void OpenDriverForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Planner._ListModeToString = false;
        }

        private void displayOnlyDriverGroupNameCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Planner._ListModeToString = !displayOnlyPlannerGroupNameCheckBox.Checked;
            displayOnlyPlannerGroupName = displayOnlyPlannerGroupNameCheckBox.Checked;
            RefreshFilteredDriverList();
        }
    }
}
