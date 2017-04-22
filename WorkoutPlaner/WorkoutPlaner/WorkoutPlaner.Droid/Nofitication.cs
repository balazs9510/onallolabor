using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using WorkoutPlaner.Services;
using Plugin.CurrentActivity;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(WorkoutPlaner.Droid.Nofitication))]
namespace WorkoutPlaner.Droid
{
    public class Nofitication : INotification
    {
        public void ShowNotification(string text)
        {           
           
            Toast.MakeText(Forms.Context, text, ToastLength.Long).Show();
        }
    }
}