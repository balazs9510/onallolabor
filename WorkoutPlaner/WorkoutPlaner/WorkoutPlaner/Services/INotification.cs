using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutPlaner.Services
{
    public interface INotification
    {
        void ShowNotification(string text);
    }
}
