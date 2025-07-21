using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarManagementWinForms
{
    public partial class StartCarForm : Form
    {
        // Holds the selected car model after the user clicks "Start"
        public string SelectedModel { get; private set; }

        // Stores the current language for localization (default is English)
        private string currentLanguage = "English";

        // Constructor that receives a list of cars and populates the model ComboBox
        public StartCarForm(List<Car> cars)
        {
            InitializeComponent();

            // Add distinct car models to the ComboBox
            comboBoxModels.Items.AddRange(cars.Select(c => c.Model).Distinct().ToArray());

            // Select the first model by default if available
            if (comboBoxModels.Items.Count > 0)
                comboBoxModels.SelectedIndex = 0;

            // Assign click event handlers
            btnStart.Click += BtnStart_Click;
            btnCancel.Click += BtnCancel_Click;
        }

        // This method is called from the main form to apply the selected language
        public void SetLanguage(string lang)
        {
            currentLanguage = lang;

            if (lang == "فارسی")
            {
                // Enable RTL layout and set Persian texts
                this.RightToLeft = RightToLeft.Yes;
                this.RightToLeftLayout = true;

                label1.Text = "مدل ماشین را انتخاب کنید:";
                btnStart.Text = "استارت";
                btnCancel.Text = "انصراف";
            }
            else
            {
                // Set to left-to-right and apply English texts
                this.RightToLeft = RightToLeft.No;
                this.RightToLeftLayout = false;

                label1.Text = "Select Car Model:";
                btnStart.Text = "Start";
                btnCancel.Text = "Cancel";
            }
        }

        // Handles the click event for the "Start" button
        private void BtnStart_Click(object sender, EventArgs e)
        {
            // If no model is selected, show a warning message
            if (comboBoxModels.SelectedItem == null)
            {
                MessageBox.Show(
                    currentLanguage == "فارسی" ? "لطفا یک مدل را انتخاب کنید." : "Please select a model.",
                    currentLanguage == "فارسی" ? "هشدار" : "Warning",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            // Save selected model and close the form with OK result
            SelectedModel = comboBoxModels.SelectedItem.ToString();
            DialogResult = DialogResult.OK;
            Close();
        }

        // Handles the click event for the "Cancel" button
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        // This method is triggered when the form is closing
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // If user has not confirmed, ask for confirmation before closing
            if (this.DialogResult != DialogResult.OK)
            {
                DialogResult result = MessageBox.Show(
                    currentLanguage == "فارسی" ? "آیا مطمئن هستید که می‌خواهید انصراف دهید و فرم بسته شود؟" : "Are you sure you want to cancel and close?",
                    currentLanguage == "فارسی" ? "تأیید انصراف" : "Confirm Cancel",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.No)
                {
                    // Cancel the form closing event
                    e.Cancel = true;
                }
            }

            base.OnFormClosing(e);
        }
    }
}
