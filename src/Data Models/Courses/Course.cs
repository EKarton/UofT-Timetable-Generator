using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UoftTimetableGenerator.DataModels
{
    /// <summary>
    /// A class used to represent a UofT course
    /// </summary>
    public class Course
    {
        /// <summary>
        /// Constructs a blank course
        /// </summary>
        public Course()
        {
        }

        /// <summary>
        /// A constructor used to convert DataContext.Course to DataModels.Course
        /// </summary>
        /// <param name="oldCourse">Raw data about a course from the database</param>
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
                Activities.Add(new Activity(oldLectures, "Lecture", this));
            if (oldTutorials.Count > 0)
                Activities.Add(new Activity(oldTutorials, "Tutorials", this));
            if (oldPracticals.Count > 0)
                Activities.Add(new Activity(oldPracticals, "Practicals", this));
        }

        /// <summary>
        /// Get / set the course code of this course
        /// </summary>
        public string CourseCode { get; set; }

        /// <summary>
        /// Get / set the title of this course
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Get / set the term of this course (either 'F' or 'S' or 'Y')
        /// </summary>
        public string Term { get; set; }

        /// <summary>
        /// Get / set the description of this course
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Get / set the campus location of this course
        /// </summary>
        public string Campus { get; set; }

        /// <summary>
        /// Get / set the activities associated with this course
        /// </summary>
        public List<Activity> Activities { get; set; }
    }
}
