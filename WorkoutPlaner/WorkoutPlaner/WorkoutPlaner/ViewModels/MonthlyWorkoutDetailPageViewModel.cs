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
    public class MonthlyWorkoutDetailPageViewModel : BindableBase,INavigationAware
    {

        public MonthlyWorkoutDetailPageViewModel()
        {
            Service = new WorkoutService();
            Current = new MonthlyWorkout()
            {
                Id = -1,
                Name = "Töltés ...",
                WeekOne = new WeeklyWorkout() { Name = "-"},
                WeekTwo = new WeeklyWorkout() { Name = "-" },
                WeekThree = new WeeklyWorkout() { Name = "-" },
                WeekFour = new WeeklyWorkout() { Name = "-" },
            };
            DeleteCommand = new DelegateCommand(DeleteAsync);
            EditCommand = new DelegateCommand(Edit);
        }
       
        #region Properties
        public DelegateCommand DeleteCommand { get; set; }
        public DelegateCommand EditCommand { get; set; }
        private MonthlyWorkout _current;
        public MonthlyWorkout Current {
            get { return _current; }
            set { SetProperty(ref _current, value); }
        }
        public WorkoutService Service { get; set; }
        public INavigationService Navigation { get; set; }
        #endregion
        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            Navigation = parameters["nav"] as INavigationService;
            if (parameters["workout"] != null)
            {
                Current = parameters["workout"] as MonthlyWorkout;
            }
            else
            {
                OpenPageAsync(nameof(MainPage));
            }
            
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
           
        }

        private async void OpenPageAsync(string nameOfPage,NavigationParameters navParam=null)
        {
            await Navigation.NavigateAsync(nameOfPage, navParam);
        }
        private void Edit()
        {
            var navParams = new NavigationParameters();
            navParams.Add("nav", Navigation);
            navParams.Add("workout", Current);
            OpenPageAsync(nameof(MakeMonthlyWorkoutPage), navParams);
        }

        private async void DeleteAsync()
        {
            await Service.DeleteMontlyWorkout(Current);
            OpenPageAsync(nameof(MainPage));
        }
    }
}
