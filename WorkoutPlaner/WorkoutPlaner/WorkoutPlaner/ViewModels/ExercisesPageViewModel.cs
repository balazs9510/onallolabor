﻿using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WorkoutPlaner.Models;
using WorkoutPlaner.Services;
using Xamarin.Forms;

namespace WorkoutPlaner.ViewModels
{
    public class ExercisesPageViewModel : BindableBase, INavigationAware
    {
        private Exercise _selected;
        public Exercise Selected {
            get { return _selected; }
            set { SetProperty(ref _selected, value);
                if(_selected!=null)
                DependencyService.Get<IDialogBuilder>()
                    .ShowDialog(_selected.Name, _selected.Description);
            }
        }
        private ObservableCollection<Exercise> _exercises;
        public ObservableCollection<Exercise> Exercises {
            get { return _exercises; }
            set { SetProperty(ref _exercises, value); }
        }
        public WorkoutService Service { get; set; }
        public ExercisesPageViewModel()
        {
            Exercises = new ObservableCollection<Exercise>();
            Service = new WorkoutService();
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            Load();

        }
        private async void Load()
        {
            var exercises = await Service.GetExercisesAsync();
            foreach (var exercise in exercises)
            {
                Exercises.Add(exercise);
            }
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
        }
    }
}
