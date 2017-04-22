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
            WorkoutPlanName = "";
            Service = new WorkoutService();
            WeeklyWorkoutsToSave = new ObservableCollection<WeeklyWorkout>();
            for (int i = 0; i < 4; i++)
            {
                WeeklyWorkoutsToSave.Add(new WeeklyWorkout());
            }

            AddWeeklyWorkoutCommand = new DelegateCommand(AddWeeklyWorkout);
            SaveMonthlyWorkoutCommand = new DelegateCommand(SaveMonthlyWorkoutAsync);
        }
        #region Properties
        private int _id = -1;
        private bool _isValid = false;
        public bool IsValid
        {
            get { return _isValid; }
            set { SetProperty(ref _isValid, value); }
        }
        public WorkoutService Service { get; set; }
        private string _wpn;
        public string WorkoutPlanName
        {
            get { return _wpn; }
            set
            {
                SetProperty(ref _wpn, value);
                CheckModelState();
            }
        }

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
        private ObservableCollection<WeeklyWorkout> _wwts;
        public ObservableCollection<WeeklyWorkout> WeeklyWorkoutsToSave
        {
            get { return _wwts; }
            set { SetProperty(ref _wwts, value); }
        }
        #endregion
        private void AddWeeklyWorkout()
        {
            WeeklyWorkoutsToSave[SelectedDay] = SelectedWW;
            CheckModelState();
        }
        private async void SaveMonthlyWorkoutAsync()
        {
            MonthlyWorkout saveInstance = new MonthlyWorkout()
            {
                Name = WorkoutPlanName,
                WeekOne = WeeklyWorkoutsToSave[0],
                WeekTwo = WeeklyWorkoutsToSave[1],
                WeekThree = WeeklyWorkoutsToSave[2],
                WeekFour = WeeklyWorkoutsToSave[3],
            };
            if (_id != -1)
            {
                await Service.PutMonthlyWorkout(saveInstance);
            }
            else
                await Service.PostMonthlyWorkout(saveInstance);
            OpenPage(nameof(MainPage));
        }
        private void CheckModelState()
        {
            var res = true;
            if (WeeklyWorkoutsToSave != null)
                foreach (var item in WeeklyWorkoutsToSave)
                {
                    if (item.Name == null || item.Name.Equals(""))
                        res = false;
                }
            else
                res = false;
            if (WorkoutPlanName != "" && res)
                IsValid = true;
        }
        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            Navigation = parameters["nav"] as INavigationService;
            var savedInstance = parameters["wokrout"] as MonthlyWorkout;
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
            if (savedInstance != null)
            {
                WorkoutPlanName = savedInstance.Name;
                WeeklyWorkoutsToSave.Add(savedInstance.WeekOne);
                WeeklyWorkoutsToSave.Add(savedInstance.WeekTwo);
                WeeklyWorkoutsToSave.Add(savedInstance.WeekThree);
                WeeklyWorkoutsToSave.Add(savedInstance.WeekFour);
                _id = savedInstance.Id;
            }
            var weeklyWorkouts = await Service.GetWeeklyWorkoutsAsync();
            if (weeklyWorkouts != null)
                WeeklyWorkouts = new ObservableCollection<WeeklyWorkout>(weeklyWorkouts);
        }
    }
}
