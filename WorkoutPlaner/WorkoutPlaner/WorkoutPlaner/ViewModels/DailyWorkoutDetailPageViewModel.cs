using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using WorkoutPlaner.Models;

namespace WorkoutPlaner.ViewModels
{
    public class DailyWorkoutDetailPageViewModel : BindableBase,INavigationAware
    {
        #region Properties
        private string _name;
        public string Name {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
        private DailyWorkout _current;
        public DailyWorkout Current {
            get { return _current; }
            set { SetProperty(ref _current, value); }
        }
        private ObservableCollection<ExerciseItem> _ei;
        public ObservableCollection<ExerciseItem> ExerciseItems {
            get { return _ei; }
            set { SetProperty(ref _ei, value); }
        }
        #endregion
        public DailyWorkoutDetailPageViewModel()
        {
            Current = new DailyWorkout();
            ExerciseItems = new ObservableCollection<ExerciseItem>();
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
           Current = parameters.GetValue<DailyWorkout>("workout");
           ExerciseItems = new ObservableCollection<ExerciseItem>(
                Current.Exercises);
           this.Name = Current.Name;
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            
        }
    }
}
