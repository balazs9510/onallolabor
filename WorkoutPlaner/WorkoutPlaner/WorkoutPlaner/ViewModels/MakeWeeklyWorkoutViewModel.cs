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
    public class MakeWeeklyWorkoutViewModel : BindableBase, INavigationAware
    {
        public MakeWeeklyWorkoutViewModel()
        {
            SelectedDay = 0;
            _id = -1;
            Current = new WeeklyWorkout()
            {
                Name = "",
                DayOne = new DailyWorkout() { Name = "-" },
                DayTwo = new DailyWorkout() { Name = "-" },
                DayThree = new DailyWorkout() { Name = "-" },
                DayFour = new DailyWorkout() { Name = "-" },
                DayFive = new DailyWorkout() { Name = "-" },
                DaySix = new DailyWorkout() { Name = "-" },
                DaySeven = new DailyWorkout() { Name = "-" },
            };
            Service = new WorkoutService();
            SelectedWorkout = new DailyWorkout();
            AddDailyWorkoutCommand = new DelegateCommand(AddDailyWorkout);
            SaveWeeklyWorkoutCommand = new DelegateCommand(SaveWeeklyWorkoutAsync);
        }
        #region Properties
        public WeeklyWorkout Current
        {
            get { return _current; }
            set { SetProperty(ref _current, value); CheckModelState(); }
        }
        private bool _isValid = false;
        public bool IsValid
        {
            get { return _isValid; }
            set { SetProperty(ref _isValid, value); }
        }
        public int SelectedDay { get; set; }


        private void CheckModelState()
        {
            bool valid = true;
            if (Current.DayOne.Name.Equals("-") ||
                Current.DayTwo.Name.Equals("-") ||
                Current.DayThree.Name.Equals("-") ||
                Current.DayFour.Name.Equals("-") ||
                Current.DayFive.Name.Equals("-") ||
                Current.DaySix.Name.Equals("-") ||
                Current.DaySeven.Name.Equals("-"))
                valid = false;

            if (Current.Name.Length > 3 && valid)
                IsValid = true;
            else
                IsValid = false;
        }

        private DailyWorkout _sw;
        public DailyWorkout SelectedWorkout
        {
            get { return _sw; }
            set { SetProperty(ref _sw, value); }
        }

        private ObservableCollection<DailyWorkout> _dw;
        private WeeklyWorkout _current;
        private int _id;

        public ObservableCollection<DailyWorkout> DailyWorkouts
        {
            get { return _dw; }
            set { SetProperty(ref _dw, value); }
        }
        public WorkoutService Service { get; set; }
        public DelegateCommand AddDailyWorkoutCommand { get; set; }
        public DelegateCommand SaveWeeklyWorkoutCommand { get; set; }
        public INavigationService Navigation { get; private set; }

        #endregion
        private void AddDailyWorkout()
        {
            if (SelectedWorkout != null)
                switch (SelectedDay)
                {
                    case 0:
                        Current = new WeeklyWorkout()
                        {
                            Name = Current.Name,
                            DayOne = SelectedWorkout,
                            DayTwo = Current.DayTwo,
                            DayThree = Current.DayThree,
                            DayFour = Current.DayFour,
                            DayFive = Current.DayFive,
                            DaySix = Current.DaySix,
                            DaySeven = Current.DaySeven,
                        };
                        break;
                    case 1:
                        Current = new WeeklyWorkout()
                        {
                            Name = Current.Name,
                            DayTwo = SelectedWorkout,
                            DayOne = Current.DayOne,
                            DayThree = Current.DayThree,
                            DayFour = Current.DayFour,
                            DayFive = Current.DayFive,
                            DaySix = Current.DaySix,
                            DaySeven = Current.DaySeven,
                        };
                        break;
                    case 2:
                        Current = new WeeklyWorkout()
                        {
                            Name = Current.Name,
                            DayThree = SelectedWorkout,
                            DayTwo = Current.DayTwo,
                            DayOne = Current.DayOne,
                            DayFour = Current.DayFour,
                            DayFive = Current.DayFive,
                            DaySix = Current.DaySix,
                            DaySeven = Current.DaySeven,
                        };
                        break;
                    case 3:
                        Current = new WeeklyWorkout()
                        {
                            Name = Current.Name,
                            DayFour = SelectedWorkout,
                            DayTwo = Current.DayTwo,
                            DayThree = Current.DayThree,
                            DayOne = Current.DayOne,
                            DayFive = Current.DayFive,
                            DaySix = Current.DaySix,
                            DaySeven = Current.DaySeven,
                        };
                        break;
                    case 4:
                        Current = new WeeklyWorkout()
                        {
                            Name = Current.Name,
                            DayFive = SelectedWorkout,
                            DayTwo = Current.DayTwo,
                            DayThree = Current.DayThree,
                            DayFour = Current.DayFour,
                            DayOne = Current.DayOne,
                            DaySix = Current.DaySix,
                            DaySeven = Current.DaySeven,
                        };
                        break;
                    case 5:
                        Current = new WeeklyWorkout()
                        {
                            Name = Current.Name,
                            DaySix = SelectedWorkout,
                            DayTwo = Current.DayTwo,
                            DayThree = Current.DayThree,
                            DayFour = Current.DayFour,
                            DayFive = Current.DayFive,
                            DayOne = Current.DayOne,
                            DaySeven = Current.DaySeven,
                        };
                        break;
                    case 6:
                        Current = new WeeklyWorkout()
                        {
                            Name = Current.Name,
                            DaySeven = SelectedWorkout,
                            DayTwo = Current.DayTwo,
                            DayThree = Current.DayThree,
                            DayFour = Current.DayFour,
                            DayFive = Current.DayFive,
                            DaySix = Current.DaySix,
                            DayOne = Current.DayOne,
                        };
                        break;
                }
            CheckModelState();
        }
        private async void SaveWeeklyWorkoutAsync()
        {
            if (_id != -1)
            {
                Current.Id = _id;
                await Service.PutWeeklyWorkout(Current);
            }
            else
                await Service.PostWeeklyWorkout(Current);
            OpenPage(nameof(MainPage));
        }
        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            Navigation = parameters["nav"] as INavigationService;
            var savedInstance = parameters["workout"] as WeeklyWorkout;
            if (savedInstance != null)
            {
                Current = savedInstance;
                _id = savedInstance.Id;
            }
            Load();
        }
        private async void Load()
        {
            var dailyWorkouts = await Service.GetDailyWorkoutsAsync();
            if (dailyWorkouts != null)
                DailyWorkouts = new ObservableCollection<DailyWorkout>(dailyWorkouts);
        }
        public void OnNavigatingTo(NavigationParameters parameters)
        {

        }
        private async void OpenPage(string name)
        {
            await Navigation.NavigateAsync(name);
        }
    }
}
