using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WorkoutPlaner.Models;
using WorkoutPlaner.Services;
using WorkoutPlaner.Views;

namespace WorkoutPlaner.ViewModels
{
    public class MakeMonthlyWorkoutViewModel : BindableBase, INavigationAware
    {
        public MakeMonthlyWorkoutViewModel()
        {
           
            Current = new MonthlyWorkout()
            {
                Name = "",
                WeekOne = new WeeklyWorkout() { Name = "-" },
                WeekTwo = new WeeklyWorkout() { Name = "-" },
                WeekThree = new WeeklyWorkout() { Name = "-" },
                WeekFour = new WeeklyWorkout() { Name = "-" },

            };
            Service = new WorkoutService();
  

            AddWeeklyWorkoutCommand = new DelegateCommand(AddWeeklyWorkout);
            SaveMonthlyWorkoutCommand = new DelegateCommand(SaveMonthlyWorkoutAsync);
        }
        #region Properties
        private MonthlyWorkout _current;
        public MonthlyWorkout Current
        {
            get { return _current; }
            set { SetProperty(ref _current, value); CheckModelState(); }
        }
        private int _id = -1;
        private bool _isValid = false;
        public bool IsValid
        {
            get { return _isValid; }
            set { SetProperty(ref _isValid, value); }
        }
        public WorkoutService Service { get; set; }
     
        private WeeklyWorkout _sww;
        public WeeklyWorkout SelectedWW
        {
            get { return _sww; }
            set { SetProperty(ref _sww, value); }
        }
        public int SelectedDay { get; set; }
        private ObservableCollection<WeeklyWorkout> _ww;
        public ObservableCollection<WeeklyWorkout> WeeklyWorkouts
        {
            get { return _ww; }
            set { SetProperty(ref _ww, value); }
        }

        public DelegateCommand AddWeeklyWorkoutCommand { get; set; }
        public DelegateCommand SaveMonthlyWorkoutCommand { get; set; }
        public INavigationService Navigation { get; set; }

        #endregion
        private void AddWeeklyWorkout()
        {
            switch (SelectedDay)
            {
                case 0:
                    Current = new MonthlyWorkout()
                    {
                        Name = Current.Name,
                        WeekOne = SelectedWW,
                        WeekTwo = Current.WeekTwo,
                        WeekThree = Current.WeekThree,
                        WeekFour = Current.WeekFour
                    };
                    break;
                case 1:
                    Current = new MonthlyWorkout()
                    {

                        Name = Current.Name,
                        WeekOne = Current.WeekOne,
                        WeekTwo = SelectedWW,
                        WeekThree = Current.WeekThree,
                        WeekFour = Current.WeekFour
                    };
                    break;
                case 2:
                    Current = new MonthlyWorkout()
                    {

                        Name = Current.Name,
                        WeekOne = Current.WeekTwo,
                        WeekTwo = Current.WeekTwo,
                        WeekThree = SelectedWW,
                        WeekFour = Current.WeekFour
                    }; break;
                case 3:
                    Current = new MonthlyWorkout()
                    {

                        Name = Current.Name,
                        WeekOne = Current.WeekTwo,
                        WeekTwo = Current.WeekTwo,
                        WeekThree = Current.WeekThree,
                        WeekFour = SelectedWW
                    }; ; break;
            }
            CheckModelState();
        }
        private async void SaveMonthlyWorkoutAsync()
        {
            if (_id!=-1)
            {
                Current.Id = _id;
                await Service.PutMonthlyWorkout(Current);
            }
            else
                await Service.PostMonthlyWorkout(Current);
            OpenPage(nameof(MainPage));
        }
        private void CheckModelState()
        {
            var res = true;
            if (Current.WeekOne.Name.Equals("-") ||
                Current.WeekTwo.Name.Equals("-") ||
                Current.WeekThree.Name.Equals("-") ||
                Current.WeekFour.Name.Equals("-"))
                res = false;
            if (Current.Name != null && Current.Name.Length > 3 && res)
                IsValid = true;
            else
                IsValid = false;
        }
        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            Navigation = parameters["nav"] as INavigationService;
            var savedInstance = parameters["workout"] as MonthlyWorkout;
            if(savedInstance!=null)
            {
                _id = savedInstance.Id;
                Current = savedInstance;
            }
            Load(savedInstance);
        }
        private async void OpenPage(string name)
        {
            await Navigation.NavigateAsync(name);
        }
        public void OnNavigatingTo(NavigationParameters parameters)
        {

        }
        private async void Load(MonthlyWorkout savedInstance = null)
        {
           
            var weeklyWorkouts = await Service.GetWeeklyWorkoutsAsync();
            if (weeklyWorkouts != null)
                WeeklyWorkouts = new ObservableCollection<WeeklyWorkout>(weeklyWorkouts);
        }
    }
}
