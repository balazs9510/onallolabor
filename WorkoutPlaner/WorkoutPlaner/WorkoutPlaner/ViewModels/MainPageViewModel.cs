using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using WorkoutPlaner.Services;

namespace WorkoutPlaner.ViewModels
{
    public class MainPageViewModel : BindableBase,INavigationAware
    {
        //private ObservableCollection<BaseWorkout> _DailyWorkouts;
        //public ObservableCollection<BaseWorkout> DailyWorkouts
        //{
        //    get { return _DailyWorkouts; }
        //    set { SetProperty(ref _DailyWorkouts, value); }
        //}
        public DelegateCommand MuscleGroupClicked { get; set; }
        public DelegateCommand ExercisesClicked { get; set; }
        public DelegateCommand AddWorkoutPlanClicked { get; set; }
        public INavigationService Navigation { get; set; }
        public WorkoutService Service { get; set; }

        public MainPageViewModel(INavigationService navigationService)
        {
            MuscleGroupClicked = new DelegateCommand(OpenMuscleGroups);
            ExercisesClicked = new DelegateCommand(OpenExercises);
            AddWorkoutPlanClicked = new DelegateCommand(OpenExerciseMaker);
            Service = new WorkoutService();
            Navigation = navigationService;
            // Debug.WriteLine("ez már nem fut ám le");
        }
        private async void OpenMuscleGroups()
        {
            await Navigation.NavigateAsync("MuscleGroupPage");
            // Debug.WriteLine("ez már nem fut ám le");
        }
        private async void OpenExercises()
        {
            await Navigation.NavigateAsync("ExercisesPage");
            // Debug.WriteLine("ez már nem fut ám le");
        }
        private async void OpenExerciseMaker()
        {
            var navParam = new NavigationParameters();
            navParam.Add("navigation", this.Navigation);

            await Navigation.NavigateAsync("WorkOutPlanMakerPage", navParam);
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

            //DailyWorkouts = new ObservableCollection<BaseWorkout>(
            //    Service.GetAllDailyWorkouts())
            //    ;
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            //DailyWorkouts = new ObservableCollection<BaseWorkout>(
            //    Service.GetAllDailyWorkouts())
            //    ;
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {

        }
    }
}
