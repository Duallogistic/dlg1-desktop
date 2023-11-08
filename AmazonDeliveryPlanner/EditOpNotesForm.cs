using System;
using System.Windows.Forms;

namespace AmazonDeliveryPlanner
{
    public partial class EditOpNotesForm : Form
    {
        string opNotes;

        public string OpNotes { get => opNotes; set => opNotes = value; }

        public EditOpNotesForm(string opNotes)
        {
            InitializeComponent();

            this.opNotes = opNotes;
            textBox1.Text = this.opNotes;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void EditPlanningNotesForm_Load(object sender, EventArgs e)
        {

        }

        void Save()
        {
            this.opNotes = textBox1.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void EditOpNotesForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Control && (e.KeyCode == Keys.Enter))
                Save();
        }
    }
}
