using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Appointments;
using Windows.UI.Xaml;
using WorkoutPlaner.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkoutPlaner.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DatePickerModalPage : ContentPage,INavigationAware
    {
        private string workoutName;
        public DatePickerModalPage()
        {
            InitializeComponent();
            for (int i = 0; i < 24; i++)
            {
                pickerHour.Items.Add(i.ToString());
            }
            
            pickerMin.Items.Add("00");
            pickerMin.Items.Add("30");
            pickerHour.SelectedIndex = 0;
            pickerMin.SelectedIndex = 0;

        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
           
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            workoutName = parameters["name"]as string;
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
           
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void Save_Clicked(object sender, EventArgs e)
        {
            CalendarService service = new CalendarService();
            var newDate = new DateTime(datePicker.Date.Year, datePicker.Date.Month, datePicker.Date.Day
                ,Int32.Parse(pickerHour.Items[pickerHour.SelectedIndex])
                ,Int32.Parse(pickerMin.Items[pickerMin.SelectedIndex]),
                 0);
            service.SaveDailyWorkout(workoutName,newDate);
            Navigation.PopModalAsync();
        }
       
    }

}
