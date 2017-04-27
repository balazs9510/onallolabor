using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using WorkoutPlaner.Models;
using WorkoutPlaner.Services;
using WorkoutPlaner.Views;

namespace WorkoutPlaner.ViewModels
{
    public class DailyWorkoutDetailPageViewModel : BindableBase, INavigationAware
    {
        #region Properties
        public DelegateCommand EditCommand { get; set; }
        public WorkoutService Service { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public DelegateCommand ImportToCalendarCommand { get; set; }
      
        private DailyWorkout _current;
        public DailyWorkout Current
        {
            get { return _current; }
            set { SetProperty(ref _current, value); }
        }
        private ObservableCollection<ExerciseItem> _ei;
        public ObservableCollection<ExerciseItem> ExerciseItems
        {
            get { return _ei; }
            set { SetProperty(ref _ei, value); }
        }
        public INavigationService Navigation { get; set; }
        #endregion

        public DailyWorkoutDetailPageViewModel()
        {
            Current = new DailyWorkout() { Name="Töltés..."};
            Service = new WorkoutService();
            ExerciseItems = new ObservableCollection<ExerciseItem>();
            ImportToCalendarCommand = new DelegateCommand(DatePickerModalPageOpen);
            DeleteCommand = new DelegateCommand(DeleteWorkoutAsync);
            EditCommand = new DelegateCommand(Edit);
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            Current = parameters["workout"] as DailyWorkout;
            if (Current != null)
            {
                if (Current.Exercises != null)
                    ExerciseItems = new ObservableCollection<ExerciseItem>(
                         Current.Exercises);
             

            }
           
            Navigation = parameters["nav"] as INavigationService; ; 

        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {

        }
        private async void DatePickerModalPageOpen()
        {
            var navParam = new NavigationParameters();
            navParam.Add("name", Current.Name);
            await Navigation.NavigateAsync("DatePickerModalPage", navParam, true, true);
        }
    
        private async void DeleteWorkoutAsync()
        {
            await Service.DeleteDailyWorkout(Current);
            OpenPageAsync(nameof(MainPage));
        }
        private void Edit()
        {
            var navParam = new NavigationParameters();
            navParam.Add("nav", Navigation);
            navParam.Add("workout", Current);
            OpenPageAsync(nameof(MakeDailyWorkoutPage), navParam);

        }
        private async void OpenPageAsync(string pageName, NavigationParameters navParam = null)
        {
            await Navigation.NavigateAsync(pageName, navParam);
        }
    }
}
