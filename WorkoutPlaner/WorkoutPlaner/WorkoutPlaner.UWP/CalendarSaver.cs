using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Appointments;
using WorkoutPlaner.Models;
using WorkoutPlaner.Services;
[assembly : Xamarin.Forms.Dependency(typeof(WorkoutPlaner.UWP.CalendarSaver))]
namespace WorkoutPlaner.UWP
{
    public class CalendarSaver : ICalendarSaver
    {
        public void SaveDailyWorkoutToCalendar(string name, DateTime time)
        {
            var appointment = new Windows.ApplicationModel.Appointments.Appointment();
            appointment.Subject = name;
            appointment.StartTime = time;
            makeAppointment(appointment);
        }

        public void SaveMonthlyWorkoutToCalendar(WeeklyWorkout[] workouts, DateTime time)
        {
            for (int i = 0; i < 4; i++)
            {
                if (workouts[i] != null)
                {
                    List<DailyWorkout> a = new List<DailyWorkout>();
                    a.Add(workouts[i].DayOne);
                    a.Add(workouts[i].DayTwo);
                    a.Add(workouts[i].DayThree);
                    a.Add(workouts[i].DayFour);
                    a.Add(workouts[i].DayFive);
                    a.Add(workouts[i].DaySix);
                    a.Add(workouts[i].DaySeven);

                    SaveWeeklyWorkoutToCalendar(a.ToArray(), time.AddDays(7));
                }
            }
        }

        public void SaveWeeklyWorkoutToCalendar(DailyWorkout[] workouts, DateTime time)
        {
            for (int i = 0; i < 7; i++)
            {
                if (workouts[i] != null)
                {
                    var appointment = new Windows.ApplicationModel.Appointments.Appointment();
                    appointment.Subject = workouts[i].Name;
                    appointment.StartTime = time.AddDays(i);
                    makeAppointment(appointment);
                }
                
            }
        }


        private async void makeAppointment(Appointment appointment)
        {
            await AppointmentManager.ShowEditNewAppointmentAsync(appointment);
        }
    }
}
