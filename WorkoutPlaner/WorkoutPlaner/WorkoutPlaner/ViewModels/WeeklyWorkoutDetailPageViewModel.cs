using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using WorkoutPlaner.Models;
using WorkoutPlaner.Services;
using WorkoutPlaner.Views;

namespace WorkoutPlaner.ViewModels
{
    public class WeeklyWorkoutDetailPageViewModel : BindableBase, INavigationAware
    {
        #region Properties
        public DelegateCommand EditCommand { get; set; }
        public WorkoutService Service { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
        public INavigationService Navigation { get; set; }
        private WeeklyWorkout _current;
        public WeeklyWorkout Current
        {
            get { return _current; }
            set { SetProperty(ref _current, value); }
        }
  
        #endregion
        public WeeklyWorkoutDetailPageViewModel()
        {
            Name = "Töltés...";
            Service = new WorkoutService();
            DeleteCommand = new DelegateCommand(DeleteWorkoutAsync);
            EditCommand = new DelegateCommand(Edit);
        }

        private void Edit()
        {
            var navParams = new NavigationParameters();
            navParams.Add("nav", Navigation);
            navParams.Add("workout", Current);
            OpenPageAsync(nameof(MakeWeeklyWorkoutPage), navParams);
        }

        private async void DeleteWorkoutAsync()
        {
            await Service.DeleteWeeklyWorkout(Current);
            OpenPageAsync(nameof(MainPage));
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {

        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            Current = parameters["workout"] as WeeklyWorkout;
            this.Name = Current.Name;
            Navigation = parameters["nav"] as INavigationService;
        }
        private async void OpenPageAsync(string pageName, NavigationParameters navParam = null)
        {
            await Navigation.NavigateAsync(pageName, navParam);
        }
    }
}
