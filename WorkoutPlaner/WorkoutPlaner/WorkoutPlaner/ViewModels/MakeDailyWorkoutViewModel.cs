﻿using Prism.Commands;
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
    public class MakeDailyWorkoutViewModel : BindableBase, INavigationAware
    {
        public MakeDailyWorkoutViewModel()
        {
            AddedExercises = new ObservableCollection<ExerciseItem>();
            CurrentExercises = new ObservableCollection<Exercise>();
            Service = new WorkoutService();
            WorkoutPlanName = "";
            SearchExerciseCommand = new DelegateCommand(SearchPressedAsync);
            AddExerciseCommand = new DelegateCommand(AddExercisePressed);
            SaveWorkoutPlanCommand = new DelegateCommand(SaveWorkoutPlanPressedAsync);

        }
        #region Properties
        public INavigationService Navigation { get; set; }
        public WorkoutService Service { get; set; }
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
        public DelegateCommand AddExerciseCommand { get; set; }
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
            set { SetProperty(ref _addedEx, value); CheckModelState(); }
        }
        private bool _isValid = false;
        public bool IsValid
        {
            get { return _isValid; }
            set { SetProperty(ref _isValid, value); }
        }
        private String _wpn;
        public String WorkoutPlanName
        {
            get { return _wpn; }
            set
            {
                SetProperty(ref _wpn, value);
                CheckModelState();
            }
        }

        private void CheckModelState()
        {
            if (WorkoutPlanName != null && WorkoutPlanName.Length > 3 && AddedExercises.Any())
                IsValid = true;
            else
                IsValid = false;
        }

        private int _serialStepper;
        public int SerialStepper
        {
            get { return _serialStepper; }
            set { SetProperty(ref _serialStepper, value); }
        }
        private int _repsStepper;
        private int _id = -1;

        public int RepsStepper
        {
            get { return _repsStepper; }
            set { SetProperty(ref _repsStepper, value); }
        }
        public DelegateCommand SaveWorkoutPlanCommand { get; set; }
        public DelegateCommand SearchExerciseCommand { get; set; }
        #endregion
        #region Commands
        private async void SaveWorkoutPlanPressedAsync()
        {
            if (_id != -1)
            {
                await Service.PutDailyWorkout(new DailyWorkout()
                {
                    Id = _id,
                    Name = WorkoutPlanName,
                    WorkoutType = Models.Type.Daily,
                    Exercises = AddedExercises
                });
            }
            else
                await Service.PostDailyWorkout(new DailyWorkout()
                {
                    Id = _id,
                    Name = WorkoutPlanName,
                    WorkoutType = Models.Type.Daily,
                    Exercises = AddedExercises
                });
            OpenPage(nameof(MainPage));
        }
        private async void OpenPage(string name)
        {
            await Navigation.NavigateAsync(name);
        }
        private void AddExercisePressed()
        {
            if (SelectedItem != null)
            {
                var newExercise = new ExerciseItem(SelectedItem)
                {
                    SerialNumber = SerialStepper,
                    Reps = RepsStepper
                };
                this.AddedExercises.Add(newExercise);
                this.SelectedItem = null;
                CheckModelState();

            }

        }
        public async void SearchPressedAsync()
        {
            var exercises = await Service.GetExercisesAsync();
            CurrentExercises = new ObservableCollection<Exercise>(
                exercises.Where(e => e.Name.Contains(SearchBarText)).ToList());
        }

        #endregion

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            Navigation = parameters["nav"] as INavigationService;
            var savedInstance = parameters["workout"] as DailyWorkout;
            if (savedInstance != null)
            {
                
                AddedExercises = new ObservableCollection<ExerciseItem>(savedInstance.Exercises);

            }
            LoadAsync(savedInstance);

        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {

        }
        private async void LoadAsync(DailyWorkout savedInstance)
        {
            if (savedInstance != null)
            {
                WorkoutPlanName = savedInstance.Name;
                if (savedInstance.Exercises != null)
                    AddedExercises = new ObservableCollection<ExerciseItem>(savedInstance.Exercises);
                _id = savedInstance.Id;
            }
            var exercises = await Service.GetExercisesAsync();
            if (exercises != null)
                CurrentExercises = new ObservableCollection<Exercise>(exercises);
        }
    }
}
