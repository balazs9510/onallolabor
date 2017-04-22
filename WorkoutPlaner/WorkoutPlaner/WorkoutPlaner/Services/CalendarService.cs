
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutPlaner.Models;
using Xamarin.Forms;

namespace WorkoutPlaner.Services
{
    public class CalendarService 
    {
        public ICalendarSaver Saver { get; set; }

        public CalendarService()
        {
            var saver = DependencyService.Get<ICalendarSaver>();
            Saver = saver;
        }
        public void SaveDailyWorkout(string name, DateTime time)
        {
            Saver.SaveDailyWorkoutToCalendar(name , time);
        }
        public void SaveWeeklyWorkout(DailyWorkout[] workouts,DateTime time)
        {
            Saver.SaveWeeklyWorkoutToCalendar(workouts, time);
        }
        public void SaveMonthlyWorkout(WeeklyWorkout[] workouts, DateTime time)
        {
            Saver.SaveMonthlyWorkoutToCalendar(workouts, time);
        }
    }
}
