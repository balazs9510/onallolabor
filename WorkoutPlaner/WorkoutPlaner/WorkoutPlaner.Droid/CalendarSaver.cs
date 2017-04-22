using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;

using Android.Widget;
using WorkoutPlaner.Services;
using Android.Provider;
using Java.Util;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using WorkoutPlaner.Models;

[assembly: Xamarin.Forms.Dependency(typeof(WorkoutPlaner.Droid.CalendarSaver))]
namespace WorkoutPlaner.Droid
{
    class CalendarSaver : ICalendarSaver
    {
        public void SaveDailyWorkoutToCalendar(string name, DateTime time)
        {

            ContentValues cw = new ContentValues();
            //var calendarsUri = CalendarContract.Calendars.ContentUri;
            //string[] calendarsProjection = {
            //     CalendarContract.Calendars.InterfaceConsts.Id,};
            
            //var cursor = Forms.Context.ContentResolver.Query(calendarsUri,null,null,null,null);
            //cursor.MoveToFirst();
            //int _calId = cursor.GetInt(cursor.GetColumnIndex(CalendarContract.Calendars.InterfaceConsts.Id));
            cw.Put(CalendarContract.Events.InterfaceConsts.CalendarId,
            0);
            cw.Put(CalendarContract.Events.InterfaceConsts.Title, name);
            cw.Put(CalendarContract.Events.InterfaceConsts.Dtstart,
                GetDateTimeMS(time.Year, time.Month, time.Day, time.Hour, time.Minute)
             );
            cw.Put(CalendarContract.Events.InterfaceConsts.Dtend,
                GetDateTimeMS(time.Year, time.Month, time.Day, time.Hour + 2, time.Minute)
             );
            cw.Put(CalendarContract.Events.InterfaceConsts.EventTimezone,
                "UTC");
            cw.Put(CalendarContract.Events.InterfaceConsts.EventEndTimezone,
                "UTC");
            var context = Forms.Context.ContentResolver;
            
            var uri = context.Insert(CalendarContract.Events.ContentUri, cw);
        }

        public void SaveMonthlyWorkoutToCalendar(WeeklyWorkout[] workouts, DateTime time)
        {
            throw new NotImplementedException();
        }

        public void SaveWeeklyWorkoutToCalendar(DailyWorkout[] workouts, DateTime time)
        {
            throw new NotImplementedException();
        }

        long GetDateTimeMS(int yr, int month, int day, int hr, int min)
        {

            Calendar c = Calendar.GetInstance(Java.Util.TimeZone.Default);

            c.Set(Calendar.DayOfMonth, 15);
            c.Set(Calendar.HourOfDay, hr);
            c.Set(Calendar.Minute, min);
            c.Set(Calendar.Month, Calendar.December);
            c.Set(Calendar.Year, 2011);

            return c.TimeInMillis;
        }
    }
}