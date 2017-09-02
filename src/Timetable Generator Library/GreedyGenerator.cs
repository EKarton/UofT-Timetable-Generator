using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UoftTimetableGenerator.DataModels;

namespace UoftTimetableGenerator.Generator
{
    public class GreedyGenerator : ITimetableGenerator
    {
        private List<Section[]> requiredSections = new List<Section[]>();
        private Preferences preferences;
        private Restrictions restrictions;
        private int numTimetablesToGenerate = 16;

        public GreedyGenerator(List<Course> courses, Preferences preferences, Restrictions restrictions)
        {
            this.preferences = preferences;
            this.restrictions = restrictions;

            // Populating requiredSections[] and its associated terms[]
            foreach (Course course in courses)
            {
                char term = course.Term[0];
                foreach (Activity activity in course.Activities)
                    requiredSections.Add(activity.Sections.ToArray());
            }
        }

        public int NumTimetablesToGenerate
        {
            get { return numTimetablesToGenerate; }
            set { numTimetablesToGenerate = value; }
        }

        public List<YearlyTimetable> GetTimetables()
        {
            throw new NotImplementedException();
        }
    }
}
