using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutPlaner.Models
{
    public class ExerciseItem : Exercise
    {
        public ExerciseItem() : base() { }
        public ExerciseItem(Exercise exe)
        {
            this.Name = exe.Name;
            this.Id = exe.Id;
            this.MuscleGroup = exe.MuscleGroup;
            
        }
        
        public int SerialNumber { get; set; }
        public int Reps { get; set; }
    }
}
