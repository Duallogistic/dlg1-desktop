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
    public partial class EditPlanningNotesForm : Form
    {
        string planningNotes;

        public string PlanningNotes { get => planningNotes; set => planningNotes = value; }

        public EditPlanningNotesForm(string planningNotes)
        {
            InitializeComponent();

            this.planningNotes = planningNotes;
            textBox1.Text = this.planningNotes;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        void Save()
        {
            this.planningNotes = textBox1.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void EditPlanningNotesForm_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Control && (e.KeyCode == Keys.Enter))
                Save();
        }
    }
}
