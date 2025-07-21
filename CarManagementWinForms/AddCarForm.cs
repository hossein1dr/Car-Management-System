using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CarManagementWinForms
{
    public partial class AddCarForm : Form
    {
        // Property to hold the newly created car that will be passed to the main form
        public Car NewCar { get; private set; }

        // A list of existing cars used for duplicate checking
        private List<Car> existingCars;

        // Holds the current language (default is English)
        private string currentLanguage = "English";

        // Constructor that accepts existing cars and initializes components
        public AddCarForm(List<Car> cars)
        {
            InitializeComponent();

            existingCars = cars;

            // Initialize the combo box with English car types
            comboBoxType.Items.AddRange(new string[] {
                "Ordinary", "Race", "Sports", "Super Sports", "Classic"
            });
            comboBoxType.SelectedIndex = 0;

            // Assign button event handlers
            btnOK.Click += btnOK_Click;
            btnCancel.Click += btnCancel_Click;
        }

        // Method to switch the UI language dynamically
        public void SetLanguage(string lang)
        {
            currentLanguage = lang;

            if (lang == "فارسی")
            {
                // Apply RTL layout for Persian
                this.RightToLeft = RightToLeft.Yes;
                this.RightToLeftLayout = true;

                // Change all labels and buttons to Persian
                labelBrand.Text = "برند ماشین:";
                labelModel.Text = "مدل ماشین:";
                labelColor.Text = "رنگ ماشین:";
                labelYear.Text = "سال ساخت:";
                labelType.Text = "نوع ماشین:";

                btnOK.Text = "تأیید";
                btnCancel.Text = "انصراف";

                // Set Persian car types
                comboBoxType.Items.Clear();
                comboBoxType.Items.AddRange(new string[] { "معمولی", "مسابقه‌ای", "اسپرت", "سوپر اسپرت", "کلاسیک" });
                comboBoxType.SelectedIndex = 0;
            }
            else
            {
                // Apply LTR layout for English
                this.RightToLeft = RightToLeft.No;
                this.RightToLeftLayout = false;

                // Change all labels and buttons to English
                labelBrand.Text = "Car Brand:";
                labelModel.Text = "Car Model:";
                labelColor.Text = "Car Color:";
                labelYear.Text = "Manufacture Year:";
                labelType.Text = "Car Type:";

                btnOK.Text = "OK";
                btnCancel.Text = "Cancel";

                // Set English car types
                comboBoxType.Items.Clear();
                comboBoxType.Items.AddRange(new string[] { "Ordinary", "Race", "Sports", "Super Sports", "Classic" });
                comboBoxType.SelectedIndex = 0;
            }
        }

        // When the OK button is clicked
        private void btnOK_Click(object sender, EventArgs e)
        {
            // Validate that all text fields are filled
            if (string.IsNullOrWhiteSpace(textBoxBrand.Text) ||
                string.IsNullOrWhiteSpace(textBoxModel.Text) ||
                string.IsNullOrWhiteSpace(textBoxColor.Text))
            {
                MessageBox.Show(
                    currentLanguage == "فارسی" ? "لطفا همه فیلدها را به درستی پر کنید." : "Please fill all fields correctly.",
                    currentLanguage == "فارسی" ? "خطا" : "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            // Validate that the year field is a valid number
            if (!int.TryParse(textBoxYear.Text, out int year))
            {
                MessageBox.Show(
                    currentLanguage == "فارسی" ? "لطفا عدد معتبری برای سال وارد کنید." : "Please enter a valid number for the year.",
                    currentLanguage == "فارسی" ? "ورودی نامعتبر" : "Invalid Input",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                textBoxYear.Focus();
                return;
            }

            // Extract and clean user inputs
            string brand = textBoxBrand.Text.Trim();
            string model = textBoxModel.Text.Trim();
            string color = textBoxColor.Text.Trim();
            string type = comboBoxType.SelectedItem.ToString();

            // Check for duplicate entry
            bool isDuplicate = existingCars.Any(car =>
                car.Brand.Equals(brand, StringComparison.OrdinalIgnoreCase) &&
                car.Model.Equals(model, StringComparison.OrdinalIgnoreCase) &&
                car.Color.Equals(color, StringComparison.OrdinalIgnoreCase) &&
                car.Year == year &&
                car.Type.Equals(type, StringComparison.OrdinalIgnoreCase)
            );

            // If duplicate, show error and stop
            if (isDuplicate)
            {
                MessageBox.Show(
                    currentLanguage == "فارسی" ? "این ماشین قبلا ثبت شده است!" : "This car already exists!",
                    currentLanguage == "فارسی" ? "ورودی تکراری" : "Duplicate Entry",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            // Create new car object and return to main form
            NewCar = new Car(brand, model, color, year, type);
            DialogResult = DialogResult.OK;
            Close();
        }

        // When Cancel button is clicked
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        // Override form closing to ask for confirmation if not accepted
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.OK)
            {
                DialogResult result = MessageBox.Show(
                    currentLanguage == "فارسی" ? "آیا مطمئن هستید می‌خواهید انصراف دهید و فرم بسته شود؟" : "Are you sure you want to cancel and close?",
                    currentLanguage == "فارسی" ? "تأیید انصراف" : "Confirm Cancel",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }

            base.OnFormClosing(e);
        }
    }
}
