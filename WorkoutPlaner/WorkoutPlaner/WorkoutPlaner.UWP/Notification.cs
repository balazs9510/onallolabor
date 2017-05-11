using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls.Primitives;
using WorkoutPlaner.Services;
[assembly: Xamarin.Forms.Dependency(typeof(WorkoutPlaner.UWP.Notification))]
namespace WorkoutPlaner.UWP
{
    public class Notification : INotification,IDialogBuilder
    {
        public void ShowDialog(string name, string description)
        {
            new MessageDialog(description, name).ShowAsync();
        }

        public void ShowNotification(string text)
        {
            new MessageDialog(text).ShowAsync();
        }
    }
}
