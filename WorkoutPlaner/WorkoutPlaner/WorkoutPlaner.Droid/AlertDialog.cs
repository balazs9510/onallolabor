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
using Xamarin.Forms;

namespace WorkoutPlaner.Droid
{
    public class AlertDialog
    {
        public AlertDialog(string title, string message)
        {
            new Android.App.AlertDialog.Builder(Forms.Context)
                .SetTitle(title)
                .SetMessage(message)
                .SetNegativeButton("Bezár", new MyOnClickListener())
                .Create();
        }
        
    }
    public class MyOnClickListener : IDialogInterfaceOnClickListener
    {
        public IntPtr Handle => throw new NotImplementedException();

        public void Dispose()
        {
           
        }

        public void OnClick(IDialogInterface dialog, int which)
        {
            dialog.Dismiss();
        }
    }
}