using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutPlaner.Models;

namespace WorkoutPlaner.Services
{
    public interface ICalendarSaver
    {
        void SaveDailyWorkoutToCalendar(string name, DateTime time);
        void SaveWeeklyWorkoutToCalendar(DailyWorkout[] workouts, DateTime time);
        void SaveMonthlyWorkoutToCalendar(WeeklyWorkout[] workouts, DateTime time);
    }
}
