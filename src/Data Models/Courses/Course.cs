using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UoftTimetableGenerator.DataModels
{
    public class Course
    {
        // A constructor used to convert DataContext.Course to DataModels.Course
        internal Course(DataContext.Course oldCourse)
        {
            CourseCode = oldCourse.Code;
            Title = oldCourse.Title;
            Term = oldCourse.Term.ToString();

            // Sort the activities based on their type
            List<DataContext.Activity> oldLectures = new List<DataContext.Activity>();
            List<DataContext.Activity> oldTutorials = new List<DataContext.Activity>();
            List<DataContext.Activity> oldPracticals = new List<DataContext.Activity>();
            foreach (DataContext.Activity oldActivity in oldCourse.Activities)
            {
                switch(oldActivity.ActivityType)
                {
                    case 'L':
                        oldLectures.Add(oldActivity);
                        break;
                    case 'T':
                        oldTutorials.Add(oldActivity);
                        break;
                    case 'P':
                        oldPracticals.Add(oldActivity);
                        break;
                }
            }

            // Add these activities based on their type (if its empty remove it)
            Activities = new List<Activity>();
            if (oldLectures.Count > 0)
                Activities.Add(new Activity(oldLectures, "Lecture"));
            if (oldTutorials.Count > 0)
                Activities.Add(new Activity(oldTutorials, "Tutorials"));
            if (oldPracticals.Count > 0)
                Activities.Add(new Activity(oldPracticals, "Practicals"));
        }

        public string CourseCode { get; set; }
        public string Title { get; set; }
        public string Term { get; set; }
        public List<Activity> Activities { get; set; }
    }
}
