using AmazonDeliveryPlanner.API;
using AmazonDeliveryPlanner.API.data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace AmazonDeliveryPlanner
{
    public partial class DriverUserControl : UserControl
    {
        Driver driver;

        API.data.DriverRouteEntity driverRouteEntity;

        //string plan_note = null;
        //string op_note = null;

        public event EventHandler/*<SessionClosedEventArgs>*/ SessionClosed;
        public event EventHandler<OpenURLEventArgs> OpenURL;

        public DriverUserControl(Driver driver)
        {
            InitializeComponent();

            this.driver = driver;
        }

        public Driver Driver { get => driver; }

        private void DriverUserControl_Load(object sender, EventArgs e)
        {
            //savedIdLabel.Text = "not saved";

            locationLabel.Text = driver.more_info.address.Length > 55 ? driver.more_info.address.Substring(0, 55) + "..." : driver.more_info.address;
            odometerLabel.Text = driver.more_info.km.ToString() + " km";
            regPlateLabel.Text = driver.reg_plate;
            currentJobLabel.Text = "?";
            nextJobLabel.Text = "?";

            testLabel.Text = string.Format("{0}  {1} {2}  {3}", driver.driver_id, driver.first_name, driver.last_name, driver.group_name);

            fileDownloadedLabel.Text = "";
            //dayRadioButton.Checked = true;
        }

        /*
        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                // MessageBox.Show("Not implemented", GlobalContext.ApplicationTitle);

                //string plan_note = this.plan_note; // (driverRouteEntity != null ? driverRouteEntity.plan_note : null);
                //string op_note = this.op_note; // (driverRouteEntity != null ? driverRouteEntity.op_note : null);
                long? id = (driverRouteEntity != null ? driverRouteEntity.id : null);

                driverRouteEntity = DriversAPI.PostRoute(new API.data.DriverRouteEntity()
                {
                    id = id,
                    driver_id = driver.driver_id,
                    //vrid = vridTextBox.Text,
                    //loc1 = location1TextBox.Text,
                    //loc2 = location2TextBox.Text,
                    //loc3 = location3TextBox.Text,
                    //pick_up_date = dateTimePicker.Value,
                    //plan_note = plan_note,
                    //op_note = op_note,
                    //shift = dayRadioButton.Checked ? 'D' : 'N',
                });//.Result;

                if (driverRouteEntity.id == null)
                {
                    MessageBox.Show("The returned id is empty/null.", GlobalContext.ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                plan_note = driverRouteEntity.plan_note;
                op_note = driverRouteEntity.op_note;

                savedIdLabel.Text = "saved id: " + driverRouteEntity.id;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("422"))
                    MessageBox.Show("Duplicate entry. Probably duplicate VRID (same VRID saved on a second route).", GlobalContext.ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show(ex.Message, GlobalContext.ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void editPlanningNotesButton_Click(object sender, EventArgs e)
        {
            // EditPlanningNotesForm epnForm = new EditPlanningNotesForm(driverRouteEntity.plan_note);
            EditPlanningNotesForm epnForm = new EditPlanningNotesForm(plan_note);

            if (epnForm.ShowDialog() == DialogResult.OK)
                plan_note = epnForm.PlanningNotes;
            //driverRouteEntity.plan_note = epnForm.PlanningNotes;
        }

        private void editOpNotesButton_Click(object sender, EventArgs e)
        {
            EditOpNotesForm eonForm = new EditOpNotesForm(op_note);
            // EditOpNotesForm eonForm = new EditOpNotesForm(driverRouteEntity.op_note);

            if (eonForm.ShowDialog() == DialogResult.OK)
                op_note = eonForm.OpNotes;
            // driverRouteEntity.op_note = eonForm.OpNotes;
        }
        */
        private void closeButton_Click(object sender, EventArgs e)
        {
            // Show a confirmation dialog to the user
            DialogResult result = MessageBox.Show("Are you sure you want to close the session?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Check the user's response from the confirmation dialog
            if (result == DialogResult.Yes)
            {
                
                // Raise the SessionClosed event if the user confirms
                if (SessionClosed != null)
                    SessionClosed(this, EventArgs.Empty);
            }
            // If the user clicks "No," the action will be canceled and the event won't be raised.
        }

        private void openAddressGoogleMapsButton_Click(object sender, EventArgs e)
        {
            string gmURL = "https://www.google.com/maps/search/?api=1&query=" + HttpUtility.UrlEncode(driver.more_info.address);

            // System.Diagnostics.Process.Start(gmURL);
            this.OpenURL(this, new OpenURLEventArgs(gmURL, this));
        }

        private void addBrowserTabButton_Click(object sender, EventArgs e)
        {
            this.OpenURL(this, new OpenURLEventArgs("", this));
        }

        Timer t;

        public void UpdateUploadLabel(string text)
        {
            fileDownloadedLabel.Text = text;

            if (t != null)
            {
                t.Enabled = false;
                t.Dispose();
                t = null;
            }

            t = new Timer();

            t.Interval = 14000;
            t.Tick += T_Tick;
            t.Enabled = true;            
        }

        private void T_Tick(object sender, EventArgs e)
        {
            (sender as Timer).Enabled = false;
            t = null;

            fileDownloadedLabel.Text = "";
        }

        public void UpdateAutoDownloadLabel(string text)
        {
            autoDownloadStatusLabel.Text = text;
        }

        private void DriverUserControl_Resize(object sender, EventArgs e)
        {
            this.PerformLayout();
        }
    }

    //public class SessionClosedEventArgs : EventArgs
    //{
    //    public SessionClosedEventArgs(DriverUserControl driverUC)
    //    {
    //        DriverUC = driverUC;
    //    }

    //    public DriverUserControl DriverUC { get; set; }
    //}

    public class OpenURLEventArgs : EventArgs
    {
        public OpenURLEventArgs(string url, DriverUserControl driverUC)
        {
            URL = url;
            DriverUC = driverUC;
        }

        public DriverUserControl DriverUC { get; set; }
        public string URL { get; set; }
    }

    //// Wrap event invocations inside a protected virtual method
    //// to allow derived classes to override the event invocation behavior
    //protected virtual void OnRaiseCustomEvent(ProjectsAddedEventArgs e)
    //{
    //    // Make a temporary copy of the event to avoid possibility of
    //    // a race condition if the last subscriber unsubscribes
    //    // immediately after the null check and before the event is raised.
    //    EventHandler<ProjectsAddedEventArgs> raiseEvent = ProjectsAdded;

    //    // Event will be null if there are no subscribers
    //    if (raiseEvent != null)
    //    {
    //        // Format the string to send inside the CustomEventArgs parameter
    //        e.Message += $" at {DateTime.Now}";

    //        // Call to raise the event.
    //        raiseEvent(this, e);
    //    }
    //}
}
