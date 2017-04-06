using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutPlaner.Models
{
    public enum Type
    {
        Daily,
        Weekly,
        Monthly
    }
    public abstract class BaseWorkout
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Type WorkoutType { get; set; }

    }
    public class DailyWorkout : BaseWorkout
    {
        public ICollection<ExerciseItem> Exercises { get; set; }
    }
    public class WeeklyWorkout : BaseWorkout
    {
        public DailyWorkout DayOne { get; set; }
        public DailyWorkout DayTwo { get; set; }
        public DailyWorkout DayThree { get; set; }
        public DailyWorkout DayFour { get; set; }
        public DailyWorkout DayFive { get; set; }
        public DailyWorkout DaySix { get; set; }
        public DailyWorkout DaySeven { get; set; }
    }
    public class MonthlyWorkout : BaseWorkout
    {
        public WeeklyWorkout WeekOne { get; set; }
        public WeeklyWorkout WeekTwo { get; set; }
        public WeeklyWorkout WeekThree { get; set; }
        public WeeklyWorkout WeekFour { get; set; }

    }
}
