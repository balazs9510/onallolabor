using Prism.Navigation;
using Prism.Unity;
using WorkoutPlaner.Services;
using WorkoutPlaner.ViewModels;
using WorkoutPlaner.Views;
using Xamarin.Forms;

namespace WorkoutPlaner
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();

            NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes()
        {
            
            Container.RegisterTypeForNavigation<NavigationPage>();
            Container.RegisterTypeForNavigation<MainPage>();            
            Container.RegisterTypeForNavigation<ExercisesPage>();            
            Container.RegisterTypeForNavigation<DailyWorkoutDetailPage>();
            Container.RegisterTypeForNavigation<DatePickerModalPage>();
            Container.RegisterTypeForNavigation<MakeDailyWorkoutPage,MakeDailyWorkoutViewModel>();
            Container.RegisterTypeForNavigation<MakeWeeklyWorkoutPage, MakeWeeklyWorkoutViewModel>();
            Container.RegisterTypeForNavigation<MakeMonthlyWorkoutPage, MakeMonthlyWorkoutViewModel>();

            Container.RegisterTypeForNavigation<WeeklyWorkoutDetailPage>();
            Container.RegisterTypeForNavigation<MonthlyWorkoutDetailPage>();
        }
    }
}
