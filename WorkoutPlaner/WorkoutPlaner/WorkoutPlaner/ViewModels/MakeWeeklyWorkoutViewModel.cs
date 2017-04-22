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
            WorkoutPlanName = "";
            DailyWorkouts = new ObservableCollection<DailyWorkout>();
            DailyWorkoutsToSave = new ObservableCollection<DailyWorkout>();
            for (int i = 0; i < 7; i++)
            {
                DailyWorkoutsToSave.Add(new DailyWorkout());
            }
            Service = new WorkoutService();
            SelectedWorkout = new DailyWorkout();
            AddDailyWorkoutCommand = new DelegateCommand(AddDailyWorkout);
            SaveWeeklyWorkoutCommand = new DelegateCommand(SaveWeeklyWorkoutAsync);
        }
        #region Properties
        private bool _isValid = false;
        public bool IsValid
        {
            get { return _isValid; }
            set { SetProperty(ref _isValid, value); }
        }
        public int SelectedDay { get; set; }
        private string _wpn;
        public string WorkoutPlanName
        {
            get { return _wpn; }
            set { SetProperty(ref _wpn, value);
                CheckModelState();
            }
        }

        private void CheckModelState()
        {
            bool valid = true;
            if (DailyWorkoutsToSave != null)
                foreach (var item in DailyWorkoutsToSave)
                {
                    if (item.Name == null || item.Name == "")
                        valid = false;
                }
            else valid = false;
            if (WorkoutPlanName != "" && valid)
                IsValid = true;
        }

        private DailyWorkout _sw;
        public DailyWorkout SelectedWorkout
        {
            get { return _sw; }
            set { SetProperty(ref _sw, value); }
        }
        private ObservableCollection<DailyWorkout> _dwts;
        public ObservableCollection<DailyWorkout> DailyWorkoutsToSave
        {
            get { return _dwts; }
            set { SetProperty(ref _dwts, value); }
        }
        private ObservableCollection<DailyWorkout> _dw;
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
            DailyWorkoutsToSave[SelectedDay] = new DailyWorkout()
            {
                Name = SelectedWorkout.Name,
                Exercises = SelectedWorkout.Exercises,
                WorkoutType = SelectedWorkout.WorkoutType,
                Id = SelectedWorkout.Id
            } ;
            CheckModelState();
        }
        private async void SaveWeeklyWorkoutAsync()
        {
            WeeklyWorkout saveInstance = new WeeklyWorkout()
            {
                Name = WorkoutPlanName,
                DayOne = DailyWorkoutsToSave[0],
                DayTwo = DailyWorkoutsToSave[1],
                DayThree = DailyWorkoutsToSave[2],
                DayFour = DailyWorkoutsToSave[3],
                DayFive = DailyWorkoutsToSave[4],
                DaySix = DailyWorkoutsToSave[5],
                DaySeven = DailyWorkoutsToSave[6],
                WorkoutType = Models.Type.Weekly
            };
            await Service.PostWeeklyWorkout(saveInstance);
            OpenPage(nameof(MainPage));
        }
        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            Navigation = parameters["nav"] as INavigationService;
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
