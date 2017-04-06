using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WorkoutPlaner.Models;
using WorkoutPlaner.Services;
using Xamarin.Forms;

namespace WorkoutPlaner.ViewModels
{
    public class WorkoutPlanMakerPageViewModel : BindableBase, INavigationAware
    {

        
        public WorkoutPlanMakerPageViewModel()
        {
            AddedExercises = new ObservableCollection<ExerciseItem>();
            CurrentExercises = new ObservableCollection<Exercise>();
            Service = new WorkoutService();
            WorkoutPlanName = "";
            DailyWorkoutsToSave = new ObservableCollection<DailyWorkout>();
            for (int i = 0; i < 7; i++)
            {
                DailyWorkoutsToSave.Add(new DailyWorkout());
            }
            SelectedDW = new DailyWorkout();
            SearchExerciseCommand = new DelegateCommand(SearchPressedAsync);
            AddExerciseCommand = new DelegateCommand(AddExercisePressed);
            SaveWorkoutPlanCommand = new DelegateCommand(SaveWorkoutPlanPressedAsync);
            AddDailyWorkoutCommand = new DelegateCommand(AddDailyWorkout);
        }

        private void AddDailyWorkout()
        {
            DailyWorkoutsToSave[SelectedDay] = SelectedDW;
            SelectedDW = null;
        }


        #region Properties
        private DailyWorkout _sdw;
        public DailyWorkout SelectedDW {
            get { return _sdw; }
            set { SetProperty(ref _sdw, value); }
        }
        private ObservableCollection<DailyWorkout> _dwts;
        public ObservableCollection<DailyWorkout> DailyWorkoutsToSave {
            get { return _dwts; }
            set { SetProperty(ref _dwts,value); }
        }
        public int SelectedDay { get; set; }
        public INavigationService Navigation { get; set; }
        private String _wpn;
        public String WorkoutPlanName
        {
            get { return _wpn; }
            set { SetProperty(ref _wpn, value); }
        }
        private int _serialStepper;
        public int SerialStepper
        {
            get { return _serialStepper; }
            set { SetProperty(ref _serialStepper, value); }
        }
        private int _repsStepper;
        public int RepsStepper
        {
            get { return _repsStepper; }
            set { SetProperty(ref _repsStepper, value); }
        }
        public DelegateCommand SaveWorkoutPlanCommand { get; set; }
        public DelegateCommand SearchExerciseCommand { get; set; }
        public DelegateCommand AddExerciseCommand { get; set; }
        public DelegateCommand AddDailyWorkoutCommand { get; set; }
        private Exercise _selectedItem;
        public Exercise SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                SetProperty(ref _selectedItem, value);
                AddExerciseCommand.Execute();
            }
        }
        private string _sbt;
        public string SearchBarText
        {
            get { return _sbt; }
            set { SetProperty(ref _sbt, value); }
        }
        private ObservableCollection<Exercise> _currEx;
        public ObservableCollection<Exercise> CurrentExercises
        {
            get { return _currEx; }
            set
            {
                SetProperty(ref _currEx, value);
            }
        }
        private ObservableCollection<ExerciseItem> _addedEx;
        public ObservableCollection<ExerciseItem> AddedExercises
        {
            get { return _addedEx; }
            set { SetProperty(ref _addedEx, value); }
        }

        public WorkoutService Service { get; set; }
        private ObservableCollection<DailyWorkout> _dw;
        public ObservableCollection<DailyWorkout> DailyWorkouts {
            get { return _dw; }
            set { SetProperty(ref _dw, value); }
        }
        #endregion


        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            Navigation = parameters["navigation"] as INavigationService;
        }
        private async void SaveWorkoutPlanPressedAsync()
        {
            await Service.PostDailyWorkout(new DailyWorkout()
            {
                Name = WorkoutPlanName,
                WorkoutType = Models.Type.Daily,
                Exercises = AddedExercises
            });
            await OpenMainPage();
        }
        private async Task OpenMainPage()
        {
            await Navigation.GoBackAsync();
            //await Navigation.NavigateAsync("MainPage");
        }
        private void AddExercisePressed()
        {
            if (SelectedItem != null) {
                var newExercise = new ExerciseItem(SelectedItem)
                {
                    SerialNumber = SerialStepper,
                    Reps = RepsStepper
                };
                this.AddedExercises.Add(newExercise);
            }

        }
       

        public async void SearchPressedAsync()
        {
            var exercises = await Service.GetExercisesAsync();
            CurrentExercises = new ObservableCollection<Exercise>(
                exercises.Where(e => e.Name.Contains(SearchBarText)).ToList());
        }


        public void OnNavigatingTo(NavigationParameters parameters)
        {
            Load();
        }
        private async void Load()
        {
            var exercises = await Service.GetExercisesAsync();
            CurrentExercises = new ObservableCollection<Exercise>(exercises);
            var dailyWorkouts = await Service.GetDailyWorkoutsAsync();
            DailyWorkouts = new ObservableCollection<DailyWorkout>(dailyWorkouts);
        }
    }
}
