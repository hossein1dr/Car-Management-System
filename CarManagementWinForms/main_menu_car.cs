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
    public partial class main_menu_car : Form
    {
        // List to store all cars
        private List<Car> cars = new List<Car>();

        // Stores the current language (default: English )
        private string currentLanguage = "English";

        public main_menu_car()
        {
            InitializeComponent();

            // Handle language selection change
            comboBoxLanguage.SelectedIndexChanged += comboBoxLanguage_SelectedIndexChanged;

            // Set default language if exists
            if (comboBoxLanguage.Items.Contains("English"))
                comboBoxLanguage.SelectedItem = "English";
            else if (comboBoxLanguage.Items.Contains("فارسی"))
                comboBoxLanguage.SelectedItem = "فارسی";

            // Set current language and apply it
            currentLanguage = comboBoxLanguage.SelectedItem.ToString();
            ChangeLanguage(currentLanguage);
        }

        // When language combo box is changed
        private void comboBoxLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentLanguage = comboBoxLanguage.SelectedItem.ToString();
            ChangeLanguage(currentLanguage);
        }

        // Updates UI texts based on selected language
        private void ChangeLanguage(string lang)
        {
            if (lang == "فارسی")
            {
                this.RightToLeft = RightToLeft.Yes;
                this.RightToLeftLayout = true;

                btnAdd.Text = "اضافه کردن";
                btnRemove.Text = "حذف";
                btnDetails.Text = "جزئیات";
                btnStart.Text = "استارت";

                listViewCars.Columns[0].Text = "برند";
                listViewCars.Columns[1].Text = "مدل";
                listViewCars.Columns[2].Text = "نوع";
                listViewCars.Columns[3].Text = "رنگ";
                listViewCars.Columns[4].Text = "سال";

                labelLanguage.Text = "زبان";
            }
            else
            {
                this.RightToLeft = RightToLeft.No;
                this.RightToLeftLayout = false;

                btnAdd.Text = "Add";
                btnRemove.Text = "Remove";
                btnDetails.Text = "Details";
                btnStart.Text = "Start";

                listViewCars.Columns[0].Text = "Brand";
                listViewCars.Columns[1].Text = "Model";
                listViewCars.Columns[2].Text = "Type";
                listViewCars.Columns[3].Text = "Color";
                listViewCars.Columns[4].Text = "Year";

                labelLanguage.Text = "Language";
            }

            // Update global texts
            LanguageTexts.SetLanguage(lang);
        }

        // Refresh the list view with current car list
        private void UpdateListView()
        {
            listViewCars.Items.Clear();

            foreach (Car car in cars)
            {
                ListViewItem item = new ListViewItem(car.Brand);
                item.SubItems.Add(car.Model);
                item.SubItems.Add(car.Type);
                item.SubItems.Add(car.Color);
                item.SubItems.Add(car.Year.ToString());

                listViewCars.Items.Add(item);
            }
        }

        // Handle "Add" button click
        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (AddCarForm addForm = new AddCarForm(cars))
            {
                addForm.SetLanguage(currentLanguage);

                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    cars.Add(addForm.NewCar);
                    UpdateListView();

                    MessageBox.Show(
                        LanguageTexts.MsgCarAddedSuccess,
                        LanguageTexts.TitleSuccess,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
        }

        // Handle form closing to confirm before exit
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.OK)
            {
                var result = MessageBox.Show(
                    currentLanguage == "فارسی" ? "آیا مطمئن هستید که می‌خواهید فرم را ببندید؟" : "Are you sure you want to close the form?",
                    currentLanguage == "فارسی" ? "تأیید بستن" : "Confirm Close",
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

        // Handle "Remove" button click
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (listViewCars.SelectedItems.Count == 0)
            {
                MessageBox.Show(LanguageTexts.MsgSelectCarToRemove, LanguageTexts.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(LanguageTexts.MsgConfirmDelete, LanguageTexts.TitleConfirmDelete, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                int index = listViewCars.SelectedIndices[0];
                cars.RemoveAt(index);
                UpdateListView();
            }
        }

        // Handle "Details" button click
        private void btnDetails_Click(object sender, EventArgs e)
        {
            if (listViewCars.SelectedItems.Count == 0)
            {
                MessageBox.Show(LanguageTexts.MsgSelectCarToViewDetails, LanguageTexts.TitleWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int index = listViewCars.SelectedIndices[0];
            Car selectedCar = cars[index];

            // Generate multilingual message
            string details = (currentLanguage == "فارسی") ?
                $"برند: {selectedCar.Brand}\nمدل: {selectedCar.Model}\nنوع: {selectedCar.Type}\nرنگ: {selectedCar.Color}\nسال: {selectedCar.Year}" :
                $"Brand: {selectedCar.Brand}\nModel: {selectedCar.Model}\nType: {selectedCar.Type}\nColor: {selectedCar.Color}\nYear: {selectedCar.Year}";

            MessageBox.Show(details, LanguageTexts.TitleCarDetails, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Handle "Start" button click
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (cars.Count == 0)
            {
                MessageBox.Show(LanguageTexts.MsgNoCarsToStart, LanguageTexts.TitleInfo, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (StartCarForm startForm = new StartCarForm(cars))
            {
                startForm.SetLanguage(currentLanguage);

                if (startForm.ShowDialog() == DialogResult.OK)
                {
                    string modelToStart = startForm.SelectedModel;

                    var selectedCars = cars.Where(c => c.Model == modelToStart).ToList();

                    if (selectedCars.Count == 0)
                    {
                        MessageBox.Show(LanguageTexts.MsgNoCarFound, LanguageTexts.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    StringBuilder sb = new StringBuilder();
                    foreach (Car car in selectedCars)
                    {
                        sb.AppendLine(car.Start());
                    }

                    MessageBox.Show(
                        sb.ToString(),
                        (currentLanguage == "فارسی") ? $"استارت مدل: {modelToStart}" : $"Starting model: {modelToStart}"
                    );
                }
            }
        }

        // Static class to hold all translatable texts
        public static class LanguageTexts
        {
            public static string MsgSelectCarToRemove;
            public static string TitleWarning;
            public static string MsgConfirmDelete;
            public static string TitleConfirmDelete;
            public static string MsgCarAddedSuccess;
            public static string TitleSuccess;
            public static string MsgNoCarsToStart;
            public static string TitleInfo;
            public static string MsgSelectCarToViewDetails;
            public static string TitleCarDetails;
            public static string MsgNoCarFound;
            public static string TitleError;

            // Set texts based on language
            public static void SetLanguage(string lang)
            {
                if (lang == "فارسی")
                {
                    MsgSelectCarToRemove = "لطفا یک ماشین را برای حذف انتخاب کنید.";
                    TitleWarning = "هشدار";
                    MsgConfirmDelete = "آیا از حذف ماشین انتخاب‌شده مطمئن هستید؟";
                    TitleConfirmDelete = "تأیید حذف";
                    MsgCarAddedSuccess = "ماشین با موفقیت اضافه شد!";
                    TitleSuccess = "موفقیت";
                    MsgNoCarsToStart = "ماشینی برای استارت وجود ندارد.";
                    TitleInfo = "اطلاعات";
                    MsgSelectCarToViewDetails = "لطفا یک ماشین را برای مشاهده جزئیات انتخاب کنید.";
                    TitleCarDetails = "جزئیات ماشین";
                    MsgNoCarFound = "ماشین مورد نظر پیدا نشد.";
                    TitleError = "خطا";
                }
                else
                {
                    MsgSelectCarToRemove = "Please select a car to remove.";
                    TitleWarning = "Warning";
                    MsgConfirmDelete = "Are you sure you want to delete the selected car?";
                    TitleConfirmDelete = "Confirm Delete";
                    MsgCarAddedSuccess = "Car added successfully!";
                    TitleSuccess = "Success";
                    MsgNoCarsToStart = "No cars available to start.";
                    TitleInfo = "Info";
                    MsgSelectCarToViewDetails = "Please select a car to view details.";
                    TitleCarDetails = "Car Details";
                    MsgNoCarFound = "No car found with the selected model.";
                    TitleError = "Error";
                }
            }
        }
    }
}
