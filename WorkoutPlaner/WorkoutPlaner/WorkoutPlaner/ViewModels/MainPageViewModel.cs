using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using WorkoutPlaner.Models;
using WorkoutPlaner.Services;
using WorkoutPlaner.Views;
using Xamarin.Forms;
[assembly: Xamarin.Forms.Dependency(typeof(WorkoutPlaner.ViewModels.MainPageViewModel))]
namespace WorkoutPlaner.ViewModels
{
    public class MainPageViewModel : BindableBase, INavigationAware
    {
        #region Properties
        private BaseWorkout _sdw;
        public BaseWorkout SelectedDWorkout
        {
            get { return _sdw; }
            set
            {
                SetProperty(ref _sdw, value);
                OpenDetailsPage(Models.Type.Daily);
            }
        }
        private BaseWorkout _sww;
        public BaseWorkout SelectedWWorkout
        {
            get { return _sww; }
            set
            {
                SetProperty(ref _sww, value);
                OpenDetailsPage(Models.Type.Weekly);
            }
        }
        private BaseWorkout _smw;
        public BaseWorkout SelectedMWorkout
        {
            get { return _smw; }
            set
            {
                SetProperty(ref _smw, value);
                OpenDetailsPage(Models.Type.Monthly);
            }
        }
        private ObservableCollection<DailyWorkout> _DailyWorkouts;
        public ObservableCollection<DailyWorkout> DailyWorkouts
        {
            get { return _DailyWorkouts; }
            set { SetProperty(ref _DailyWorkouts, value); }
        }
        private ObservableCollection<WeeklyWorkout> _WeeklyWorkouts;
        public ObservableCollection<WeeklyWorkout> WeeklyWorkouts
        {
            get { return _WeeklyWorkouts; }
            set { SetProperty(ref _WeeklyWorkouts, value); }
        }
        private ObservableCollection<MonthlyWorkout> _MonthlyWorkouts;
        public ObservableCollection<MonthlyWorkout> MonthlyWorkouts
        {
            get { return _MonthlyWorkouts; }
            set { SetProperty(ref _MonthlyWorkouts, value); }
        }
        private double _progress;
        public double Progress
        {
            get { return _progress; }
            set { SetProperty(ref _progress, value); }
        }
        public DelegateCommand MuscleGroupClicked { get; set; }
        public DelegateCommand ExercisesClicked { get; set; }
        public DelegateCommand AddDWorkoutPlanClicked { get; set; }
        public DelegateCommand AddWWorkoutPlanClicked { get; set; }
        public DelegateCommand AddMWorkoutPlanClicked { get; set; }
        public INavigationService Navigation { get; set; }
        public WorkoutService Service { get; set; }
        private bool _ps;
        public bool ProgresSeen
        {
            get { return _ps; }
            set { SetProperty(ref _ps, value); }
        }
        #endregion

        public MainPageViewModel(INavigationService navigationService)
        {

            DailyWorkouts = new ObservableCollection<DailyWorkout>();
            WeeklyWorkouts = new ObservableCollection<WeeklyWorkout>();
            MonthlyWorkouts = new ObservableCollection<MonthlyWorkout>();
            ProgresSeen = false;
            MuscleGroupClicked = new DelegateCommand(OpenMuscleGroups);
            ExercisesClicked = new DelegateCommand(OpenExercises);
            AddDWorkoutPlanClicked = new DelegateCommand(OpenDailyExerciseMaker);
            AddWWorkoutPlanClicked = new DelegateCommand(OpenWeeklyExerciseMaker);
            AddMWorkoutPlanClicked = new DelegateCommand(OpenMonthlyExerciseMaker);
            Service = new WorkoutService();
            Navigation = navigationService;

            // Debug.WriteLine("ez már nem fut ám le");
        }
        private async void OpenMuscleGroups()
        {
            //await Navigation.NavigateAsync(nameof(MuscleGroupPage));
            // Debug.WriteLine("ez már nem fut ám le");
        }
        private async void OpenExercises()
        {
            await Navigation.NavigateAsync(nameof(ExercisesPage));
            // Debug.WriteLine("ez már nem fut ám le");
        }
        private async void OpenDailyExerciseMaker()
        {
            var navParam = new NavigationParameters();
            navParam.Add("nav", this.Navigation);
            await Navigation.NavigateAsync(nameof(MakeDailyWorkoutPage), navParam);
        }
        private async void OpenWeeklyExerciseMaker()
        {
            var navParam = new NavigationParameters();
            navParam.Add("nav", this.Navigation);
            await Navigation.NavigateAsync(nameof(MakeWeeklyWorkoutPage), navParam);
        }
        private async void OpenMonthlyExerciseMaker()
        {
            var navParam = new NavigationParameters();
            navParam.Add("nav", this.Navigation);
            await Navigation.NavigateAsync(nameof(MakeMonthlyWorkoutPage), navParam);
        }
        private async void OpenDetailsPage(Models.Type t)
        {
            switch (t)
            {
                case Models.Type.Daily:
                    if (SelectedDWorkout != null)
                    {
                        var navParam = new NavigationParameters();
                        navParam.Add("workout", SelectedDWorkout);
                        navParam.Add("nav", Navigation);
                        await Navigation.NavigateAsync("DailyWorkoutDetailPage", navParam);
                    }                   
                    break;
                case Models.Type.Weekly: break;
                case Models.Type.Monthly: break;
            }


        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }
        private async void LoadAsync()
        {
            ProgresSeen = true;
            Progress = 0;
            var ex = await Service.GetDailyWorkoutsAsync();
            if (ex != null)
            {
                DailyWorkouts = new ObservableCollection<DailyWorkout>(ex);
                Progress = 1 / 3;
            }

            var weekly = await Service.GetWeeklyWorkoutsAsync();
            if (weekly != null)
            {
                WeeklyWorkouts = new ObservableCollection<WeeklyWorkout>(weekly);
                Progress = 2 / 3;
            }
            var monthly = await Service.GetMonthlyWorkoutsAsync();
            if (monthly != null)
            {
                MonthlyWorkouts = new ObservableCollection<MonthlyWorkout>(monthly);
                ProgresSeen = false;
            }
            ProgresSeen = false;
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            LoadAsync();
            
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {

        }

    }
}
